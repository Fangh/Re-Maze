using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Birds : MonoBehaviour 
{
	GameObject birdPoints;
	List<Transform> points = new List<Transform>();

	// Use this for initialization
	void Start () 
	{
		birdPoints = GameObject.Find("BirdPoints");
		for (int i = 0; i < birdPoints.transform.childCount; i++)
		{
			points.Add( birdPoints.transform.GetChild(i) );
		}

		GoToPoint();
	}
	
	// Update is called once per frame
	void Update () 
	{

		
	}

	void GoToPoint()
	{
		Vector3 pos = points[Random.Range(0, points.Count)].position;
		transform.DOMove( pos, Random.Range(10f,20f) ).OnComplete(GoToPoint ).SetEase(Ease.Linear);
		transform.LookAt(pos);
	}
}
