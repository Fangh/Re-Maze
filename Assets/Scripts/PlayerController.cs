using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
	[Header("References")]
	public GameObject decalPrefab;
	public LayerMask decalsStickOn;
	public Text debugText;

	[Header("Balancing")]
	public float maxSteps = 10;

	private CharacterController controller;
	private float currentStepsNumber;

	// Use this for initialization
	void Start () 
	{
		controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButtonDown("Fire1"))
		{
			RaycastHit hit;
			if ( Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2, decalsStickOn) )
			{
				GameObject d = GameObject.Instantiate(decalPrefab, hit.point, Quaternion.identity );
				d.transform.LookAt(hit.point - hit.normal);		
			}
		}

		if ( controller.velocity.magnitude != 0 )
		{
			currentStepsNumber += Time.deltaTime;
		}

		if ( currentStepsNumber > maxSteps )
		{
			SceneManager.LoadScene(0);
		}

		debugText.text = string.Format("Steps : {0}", Mathf.RoundToInt(currentStepsNumber).ToString());
	}
}