using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour {

	void OnTriggerEnter () {
        StartCoroutine(Restart());
	}

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        GameObject.FindGameObjectWithTag("Player").SendMessage("Init");
    }
}
