using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffManager : MonoBehaviour {

    enum DebuffStage { Normal, Distortion, Colorblind, Blind };

    PlayerController player;
    Camera cam;
    DebuffStage currentStage = DebuffStage.Normal;

    public LayerMask std;
    public LayerMask blind;

	// Use this for initialization
	void Start () {
        cam = Camera.main;
        player = GetComponent<PlayerController>();
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
	}

    bool ProgressionHasChanged()
    {
        switch (currentStage)
        {
            case DebuffStage.Normal: return (GetProgression() < 0.75f);
            case DebuffStage.Distortion: return (GetProgression() < 0.5f);
            case DebuffStage.Colorblind: return (GetProgression() < 0.25f);
            case DebuffStage.Blind: return false;
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
            case DebuffStage.Blind: // Nothing to do
                break;
            default: Debug.LogError("wrong place");
                break;
        }
    }

    void DistortCamera()
    {
        cam.fieldOfView = 100;
    }

    void Colorblindness()
    {
        // ToDo
    }

    void Darkness()
    {
        cam.cullingMask = blind;
    }

    void Restart()
    {
        currentStage = DebuffStage.Normal;
        cam.fieldOfView = 60;
        cam.cullingMask = std;
    }
}
