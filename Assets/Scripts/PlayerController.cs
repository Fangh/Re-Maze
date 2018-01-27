using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	[Header("References")]
	public GameObject decalPrefab;
	public LayerMask decalsStickOn;

	// Use this for initialization
	void Start () 
	{
		
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
		
	}
}
