using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour 
{
	public List<GameObject> sets;
	// Use this for initialization
	public void Init () 
	{
		sets.Clear();
		for( int i = 0; i < transform.childCount; i++)
		{
			GameObject o = transform.GetChild(i).gameObject;

			if ( o.name.Contains("Set_Decors_") )
			{
				sets.Add(o);
			}
		}

		ChangeSet();
	}

	public void DisableSets()
	{
		for( int i = 0; i < transform.childCount; i++)
		{
			GameObject o = transform.GetChild(i).gameObject;

			if ( o.name.Contains("Set_Decors_") )
			{
				o.SetActive(false);
			}
		}
	}
	
	public void ChangeSet()
	{
		DisableSets();
		if (sets.Count > 0)
		{
			float showSet = Random.value;
			if (showSet < 0.8f)
			{
				GameObject o = sets[Random.Range(0, sets.Count)];
				o.SetActive(true);
			}
		}		
	}
}