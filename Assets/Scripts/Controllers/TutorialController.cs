using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialController : MonoBehaviour
{

	[Header ("Enemies")]
	public float saveDuration = 2f;

	private bool started = false;
	private static TutorialController instance = null;
	private TutorialSign targetSign = null;
	private float currentTime = 0f;

    public GameObject cowSign, sheepSign, chickenSign;


	public GameObject confeti;
    int completedSigns = 0;

	void Awake ()
	{
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}
	}

	void Start ()
	{

		InputManager.Instance.OnWaveDetectedCallback += OnWaveDetected;

        InputManager.Instance.OnWaveStoppedCallback += OnWaveStopped;

    }


	void Update ()
	{
        HandleInput();
	}
		
	//Public methods
	public static TutorialController Instance {
		get{ return instance; }
	}

	public void StartLevel ()
	{
        StartCoroutine(StartTutorial());
	}

    private void HandleInput()
    {

        if (Input.GetButtonDown("Type1"))
        {
            targetSign = cowSign.GetComponent<TutorialSign>();
        }
        if (Input.GetButtonDown("Type2"))
        {
            targetSign = sheepSign.GetComponent<TutorialSign>();
        }
        if (Input.GetButtonDown("Type3"))
        {
            targetSign = chickenSign.GetComponent<TutorialSign>();
        }

        if (targetSign != null && !targetSign.completed)
        {
            targetSign.StartSaving();
        }
    }

    public void OnWaveDetected ()
	{
        if(!started)
        {
            return;
        }
        switch(InputManager.Instance.GetWaveType())
        {
            case WaveType.Type1:
                targetSign = cowSign.GetComponent < TutorialSign>();
                break;
            case WaveType.Type2:
                targetSign = sheepSign.GetComponent < TutorialSign>();
                break;
            case WaveType.Type3:
                targetSign = chickenSign.GetComponent < TutorialSign>();
                break;
            default:
                break;
        }
        if(targetSign!=null && !targetSign.completed)
        {
            targetSign.StartSaving();
        }
	}

	public void OnWaveStopped ()
	{
		Debug.Log ("INPUT HA ENTRADO EN ONWAVE STOPPED");
		if (targetSign != null) {
			targetSign.GetComponent<TutorialSign> ().CancelSave ();
			targetSign = null;
		}
	}
    

	public void NotifyCompletion ()
	{
        completedSigns++;
		targetSign = null;
        if(completedSigns>=3)
        {
            StartCoroutine(FinishTutorial());
        }
	}


    IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(0.5f);

        sheepSign.GetComponent<TutorialSign>().Show();

        yield return new WaitForSeconds(0.6f);

        cowSign.GetComponent<TutorialSign>().Show();
        chickenSign.GetComponent<TutorialSign>().Show();

        yield return new WaitForSeconds(1f);
        started = true;
        yield return 0;
    }

    IEnumerator FinishTutorial()
    {
		if (confeti != null) {
			confeti.SetActive (true);
		}
        yield return new WaitForSeconds(2f);

		MasterController.Instance.lastLevel = 0;
		MasterController.Instance.LoadNextLevel ();

        yield return 0;
    }
}
