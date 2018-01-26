using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	public GameObject decalPrefab;

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
			if ( Physics.Raycast(transform.position, transform.forward, out hit, 2) )
			{
				GameObject.Instantiate(decalPrefab, hit.point, Quaternion.Euler( hit.normal) );				
			}
		}
		
	}
}
