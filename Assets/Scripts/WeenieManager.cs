using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeenieManager : MonoBehaviour {

    public GameObject weeniePrefab;
    Transform weenie;
    List<Vector3> childrenPos;

	// Use this for initialization
	void Start () {
        childrenPos = new List<Vector3>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            childrenPos.Add(transform.GetChild(i).position);
        }
        weenie = GameObject.Instantiate(weeniePrefab, childrenPos[Mathf.FloorToInt(Random.value * childrenPos.Count)], Quaternion.identity).transform;
	}

    public void Restart()
    {
        RandomPlacement();
    }

    void RandomPlacement()
    {
        Debug.Assert(transform.childCount > 0, "There are no locations defined for the weenie");
        weenie.position = childrenPos[Mathf.FloorToInt(Random.value * childrenPos.Count)];
    }
}
