using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using DG.Tweening;

public class DebuffManager : MonoBehaviour {

    enum DebuffStage { Normal, Distortion, Colorblind, Blind };

    PlayerController player;
    Camera cam;
    DebuffStage currentStage = DebuffStage.Normal;
    PostProcessingBehaviour postpro;

    public LayerMask std;
    public LayerMask blind;
    public PostProcessingProfile stdProfile;
    public PostProcessingProfile colorblindProfile;

	// Use this for initialization
	void Start () {
        cam = Camera.main;
        player = GetComponent<PlayerController>();
        postpro = transform.GetChild(0).GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();
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
            case DebuffStage.Normal: return (GetProgression() > 0.25f);
            case DebuffStage.Distortion: return (GetProgression() > 0.5f);
            case DebuffStage.Colorblind: return (GetProgression() > 0.75f);
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
        DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, 0.5f, 5f);
        //cam.fieldOfView = 120;
    }

    void Colorblindness()
    {
        postpro.profile = colorblindProfile;
    }

    void Darkness()
    {
        DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, 1f, 5f);
        //cam.farClipPlane = 8;
        //cam.cullingMask = blind;
    }

    void Die()
    {
        cam.cullingMask = 0;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
    }

    void Restart()
    {
        RenderSettings.fogDensity = 0.1f;
        currentStage = DebuffStage.Normal;
        cam.farClipPlane = 1000;
        cam.fieldOfView = 60;
        cam.cullingMask = std;
        postpro.profile = stdProfile;
        cam.clearFlags = CameraClearFlags.Skybox;
    }
}
