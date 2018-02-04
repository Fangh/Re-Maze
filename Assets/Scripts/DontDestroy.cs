using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour 
{

	void Awake()
	{
        if (GameObject.FindGameObjectsWithTag("Music").Length > 1)
            DestroyImmediate(gameObject);
        else
		    DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
