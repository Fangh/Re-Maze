using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using DG.Tweening;

public class PlayerController : MonoBehaviour 
{
	[Header("References")]
    public GameObject decalSymbolPrefab;
    public GameObject decalCrossPrefab;
	public LayerMask decalsStickOn;
	public List<Transform> respawnPoints;
	public GameObject linePrefab;
	public GameObject birdPrefab;

	[Header("UI")]
	public Text stepsText;
	public Text deadText;
	public Image crosshair;
	public Image endPanel;
	
	[Header("Audio")]
	public AudioClip SFX_spray;
	public AudioClip SFX_startDrawing;
	public AudioClip SFX_stopDrawing;
	public AudioClip SFX_win;

	[Header("Balancing")]
	public float maxSteps = 10;
	public float lineLength = 3;
	public float distanceToSpray = 2;

	private CharacterController controller;
	private float currentStepsNumber;
	private bool isDead = false;
	private bool isDrawing = false;
	private LineRenderer currentLine;
	private Transform cameraTransform;
	private AudioSource audioSource;
	private bool firstColor = true;
	private bool win = false;

	// Use this for initialization
	void Start () 
	{
		controller = GetComponent<CharacterController>();
		cameraTransform = Camera.main.transform;
		audioSource = cameraTransform.GetComponent<AudioSource>();
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
		GameObject.Instantiate(birdPrefab, birdPrefab.transform.position, birdPrefab.transform.rotation);
		GetComponent<DebuffManager>().Restart();
		GetComponent<FirstPersonController>().enabled = true;
        GetComponent<FirstPersonController>().Reinit();
        GetComponent<DebuffManager>().Restart();
        WeenieManager.Instance.Restart();
		LevelManager.Instance.ResetLevel();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (CrossPlatformInputManager.GetButtonDown("Submit"))
        {
			if (!win)
            	Init();
			else
				Application.Quit();
        }
		if (isDead || win)
		{
			return;
		}
		if (CrossPlatformInputManager.GetButtonDown("Fire1"))
		{
			Spray();
		}
        //if (CrossPlatformInputManager.GetButtonDown("ChangeColor"))
        //{
        //    firstColor = !firstColor;
        //    crosshair.color = firstColor ? Color.red : Color.blue;
        //}

		if (CrossPlatformInputManager.GetButtonDown("Fire2"))
		{
            SprayCross();
            //GameObject line = GameObject.Instantiate(linePrefab, cameraTransform.position, Quaternion.identity);
            //currentLine = line.GetComponent<LineRenderer>();
            //currentLine.SetPosition(0, cameraTransform.position + cameraTransform.forward);
            //isDrawing = true;
            //audioSource.PlayOneShot(SFX_startDrawing);
		}

        //if ( isDrawing )
        //{
        //    currentLine.SetPosition(1, cameraTransform.position + cameraTransform.forward);

        //    if ( Vector3.Distance( currentLine.GetPosition(0), currentLine.GetPosition(1) ) > lineLength)
        //    {
        //        isDrawing = false;
        //        currentLine = null;
        //        audioSource.PlayOneShot(SFX_stopDrawing);
        //    }
        //}

        //if ( CrossPlatformInputManager.GetButtonUp("Fire2") )
        //{
        //    isDrawing = false;
        //    currentLine = null;
        //    audioSource.PlayOneShot(SFX_stopDrawing);
        //}

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

	void Spray()
	{
		RaycastHit hit;
		if ( Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, distanceToSpray, decalsStickOn) )
		{
			GameObject d = GameObject.Instantiate(decalSymbolPrefab, hit.point, Quaternion.identity );
			d.transform.LookAt(hit.point - hit.normal);
            d.transform.position = Vector3.MoveTowards(d.transform.position, cameraTransform.position, 0.16f);
			audioSource.PlayOneShot(SFX_spray);
            //d.GetComponentInChildren<Renderer>().material.color = new Color(0, 145f/255f, 79f/255f);
            //d.GetComponentInChildren<Renderer>().material.shader = Shader.Find("UI/Default");
            //d.transform.GetChild(0).Rotate(Vector3.forward, Random.value * 90);
		}
	}

    void SprayCross()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, distanceToSpray, decalsStickOn))
        {
            GameObject d = GameObject.Instantiate(decalCrossPrefab, hit.point, Quaternion.identity);
            d.transform.LookAt(hit.point - hit.normal);
            d.transform.position = Vector3.MoveTowards(d.transform.position, cameraTransform.position, 0.16f);
            audioSource.PlayOneShot(SFX_spray);
            d.GetComponentInChildren<Renderer>().material.color = Color.red;
            //d.GetComponentInChildren<Renderer>().material.shader = Shader.Find("UI/Default");
        }
    }

	void Die()
	{
		isDead = true;
		deadText.gameObject.SetActive(true);
		controller.enabled = false;
		GetComponent<FirstPersonController>().enabled = false;
	}

	public float GetCurrentSteps()
	{
		return currentStepsNumber;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Finish"))
		{
			endPanel.gameObject.SetActive(true);
			endPanel.DOFade(1, 2);
			stepsText.enabled = false;
			controller.enabled = false;
			GetComponent<FirstPersonController>().enabled = false;
			audioSource.PlayOneShot(SFX_win);
			win = true;
		}
	}
}