using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.PostProcessing.Utilities;
using DG.Tweening;

public class DebuffManager : MonoBehaviour {

    enum DebuffStage { Normal, Distortion, Colorblind, Blind };

    PlayerController player;
    Camera cam;
    DebuffStage currentStage = DebuffStage.Normal;
    PostProcessingBehaviour postpro;
    PostProcessingController postproCont;
    const float transitionDuration = 5;

    public PostProcessingProfile stdProfile;

	// Use this for initialization
	void Start () {
        cam = Camera.main;
        player = GetComponent<PlayerController>();
        postpro = transform.GetChild(0).GetComponent<PostProcessingBehaviour>();
        postproCont = postpro.GetComponent<PostProcessingController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (ProgressionHasChanged())
        {
            Debuff();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            DistortCamera();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Colorblindness();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Darkness();
        }
	}

    bool ProgressionHasChanged()
    {
        switch (currentStage)
        {
            case DebuffStage.Normal: return (GetProgression() > 0.3f);
            case DebuffStage.Distortion: return (GetProgression() > 0.6f);
            case DebuffStage.Colorblind: return (GetProgression() > 0.85f);
            case DebuffStage.Blind: return (GetProgression() >= 1f);
            default: Debug.LogError("wrong place");
                return false;
        }
    }

    float GetProgression()
    {
        Debug.Assert(player != null, "player not set in DebuffManager");
        return ((float)player.GetCurrentSteps() / player.maxSteps);
    }

    void Debuff()
    {
        switch (currentStage)
        {
            case DebuffStage.Normal:
                DistortCamera();
                currentStage = DebuffStage.Distortion;
                break;
            case DebuffStage.Distortion:
                Colorblindness();
                currentStage = DebuffStage.Colorblind;
                break;
            case DebuffStage.Colorblind:
                Darkness();
                currentStage = DebuffStage.Blind;
                break;
            case DebuffStage.Blind: Die();
                break;
            default: Debug.LogError("wrong place");
                break;
        }
    }

    void DistortCamera()
    {
        DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, 0.5f, transitionDuration);
    }

    void Colorblindness()
    {
        StartCoroutine(ColorblindnessCoroutine(Time.time, transitionDuration));
    }

    IEnumerator ColorblindnessCoroutine(float startTime, float duration)
    {
        yield return new WaitForSeconds(0.01f);
        if (postproCont.colorGrading.basic.saturation > 0)
        {
            Mathf.Lerp(1, 0, (Time.time - startTime)/ duration);
            postproCont.colorGrading.basic.saturation -= 0.01f;
            StartCoroutine(ColorblindnessCoroutine(startTime, duration));
        }
        else postproCont.colorGrading.basic.saturation = 0;
    }

    void Darkness()
    {
        DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, 1f, transitionDuration);
    }

    void Die()
    {
        cam.cullingMask = 0;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
    }

    void Restart()
    {
        currentStage = DebuffStage.Normal;
        RenderSettings.fogDensity = 0.1f;
        postproCont.colorGrading.basic.saturation = 1;
        cam.clearFlags = CameraClearFlags.Skybox;
        cam.cullingMask = 1 << 9;
    }
}
