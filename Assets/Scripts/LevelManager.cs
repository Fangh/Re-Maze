using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 
{
	static public LevelManager Instance;
	[Header("References")]
	public Transform directionalLight;
	public List<Module> modules;
	public List<Material> skyboxes;
    Color stdFogColor = new Color(126f / 255f, 126f / 255f, 126f / 255f, 69f / 255f);

    Color day = new Color(123f / 255f, 146f / 255f, 223f / 255f, 255f / 255f);
    Color morning = new Color(176f / 255f, 123f / 255f, 144f / 255f, 255f / 255f);
    Color sunset = new Color(221f / 255f, 153f / 255f, 153f / 255f, 255f / 255f);

	LevelManager()
	{
		Instance = this;
	}

	void Start()
	{
		modules.AddRange( FindObjectsOfType<Module>() );
		foreach(Module m in modules)
			m.Init();
	}	

	public void ResetLevel()
	{
		// directionalLight.rotation = Quaternion.Euler( directionalLight.rotation.eulerAngles.x, directionalLight.rotation.eulerAngles.y + 90, directionalLight.rotation.eulerAngles.z);
        int skyboxIndex = Random.Range(0, skyboxes.Count);
        RenderSettings.skybox = skyboxes[skyboxIndex];
		RenderSettings.skybox.SetFloat("_Rotation", RenderSettings.skybox.GetFloat("_Rotation") + 90);
        switch (skyboxIndex)
        {
            case 0: RenderSettings.fogColor = day; break;
            case 1: RenderSettings.fogColor = morning; break;
            case 2: RenderSettings.fogColor = sunset; break;
            default : RenderSettings.fogColor = stdFogColor; break;
        }
		foreach (Module m in modules)
		{
			m.Init();
			m.ChangeSet();
		}
	}
}