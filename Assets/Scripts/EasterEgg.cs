using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour {

	void OnTriggerEnter () {
        StartCoroutine(Restart());
	}

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(8.5f);
        GameObject.FindGameObjectWithTag("Player").SendMessage("Init");
    }
}
