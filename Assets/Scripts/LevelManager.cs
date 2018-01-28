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
		RenderSettings.skybox = skyboxes[Random.Range(0, skyboxes.Count)];
		RenderSettings.skybox.SetFloat("_Rotation", RenderSettings.skybox.GetFloat("_Rotation") + 90);
		foreach (Module m in modules)
		{
			m.Init();
			m.ChangeSet();
		}
	}
}