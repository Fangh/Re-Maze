﻿using System.Collections;
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
    LayerMask stdMask;
    const float transitionDuration = 5;

    public PostProcessingProfile stdProfile;

	// Use this for initialization
	void Awake () 
    {
        cam = Camera.main;
        player = GetComponent<PlayerController>();
        postpro = transform.GetChild(0).GetComponent<PostProcessingBehaviour>();
        postproCont = postpro.GetComponent<PostProcessingController>();
        stdMask = cam.cullingMask;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (ProgressionHasChanged())
        {
            Debuff();
        }
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    Restart();
        //}
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    DistortCamera();
        //}
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    Colorblindness();
        //}
        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    Darkness();
        //}
	}

    bool ProgressionHasChanged()
    {
        switch (currentStage)
        {
            case DebuffStage.Normal: return (GetProgression() > 0.5f);
            case DebuffStage.Distortion: return (GetProgression() > 0.75f);
            case DebuffStage.Colorblind: return (GetProgression() > 0.95f);
            case DebuffStage.Blind: return (GetProgression() >= 1f);
            default: Debug.LogError("wrong place");
                return false;
        }
    }

    float GetProgression()
    {
        // Debug.Assert(player != null, "player not set in DebuffManager");
        return (player.GetCurrentSteps() / player.maxSteps);
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
        StartCoroutine(ColorblindnessCoroutine(Time.time, transitionDuration));
    }

    void Colorblindness()
    {
        DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, 0.5f, transitionDuration);
    }

    IEnumerator ColorblindnessCoroutine(float startTime, float duration)
    {
        yield return new WaitForSeconds(0.01f);
        if (postproCont.colorGrading.basic.saturation > 0)
        {
            postproCont.colorGrading.basic.saturation = Mathf.Lerp(1, 0, (Time.time - startTime) / duration);
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
        DOTween.KillAll();
    }

    public void Restart()
    {
        currentStage = DebuffStage.Normal;
        RenderSettings.fogDensity = 0.1f;
        postproCont.colorGrading.basic.saturation = 1;
        cam.clearFlags = CameraClearFlags.Skybox;
        cam.cullingMask = stdMask;
    }
}
