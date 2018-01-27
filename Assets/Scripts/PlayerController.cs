using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
	[Header("References")]
	public GameObject decalPrefab;
	public LayerMask decalsStickOn;
	public Text stepsText;
	public Text deadText;
	public List<Transform> respawnPoints;

	[Header("Balancing")]
	public float maxSteps = 10;

	private CharacterController controller;
	private float currentStepsNumber;
	private bool isDead = false;

	// Use this for initialization
	void Start () 
	{
		controller = GetComponent<CharacterController>();
		Init();
	}

	void Init()
	{
		isDead = false;
		currentStepsNumber = 0;
		deadText.gameObject.SetActive(false);
		controller.enabled = true;
		transform.position = respawnPoints[Random.Range(0, respawnPoints.Count)].position;
		transform.rotation = respawnPoints[Random.Range(0, respawnPoints.Count)].rotation;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isDead)
		{
			if ( CrossPlatformInputManager.GetButtonDown("Submit") )
			{
				Init();
			}
			return;
		}
		if (CrossPlatformInputManager.GetButtonDown("Fire1"))
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
			Die();
		}

		stepsText.text = string.Format("Steps : {0} / {1}", Mathf.RoundToInt(currentStepsNumber).ToString(), maxSteps);
	}

	void Die()
	{
		isDead = true;
		deadText.gameObject.SetActive(true);
		controller.enabled = false;
	}

	public int GetCurrentSteps()
	{
		return Mathf.RoundToInt(currentStepsNumber);
	}
}