using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeenieManager : MonoBehaviour {

    public static WeenieManager Instance;
    public GameObject weeniePrefab;
    Transform weenie;
    List<Vector3> childrenPos;

    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
    void Start()
    {
        Debug.Assert(weeniePrefab != null, "Assign weenie prefab to the script");
        childrenPos = new List<Vector3>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            childrenPos.Add(transform.GetChild(i).position);
        }
        weenie = GameObject.Instantiate(weeniePrefab, childrenPos[Random.Range(0, childrenPos.Count)], Quaternion.identity).transform;
	}

    public void Restart()
    {
        RandomPlacement();
    }

    void RandomPlacement()
    {
        // Debug.Assert(transform.childCount > 0, "There are no locations defined for the weenie");
        weenie.position = childrenPos[Random.Range(0, childrenPos.Count)];
    }
}
