using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
	//Attributes
	[Header ("Init")]
	public GameObject waveImage;
	public GameObject numberImage;
	public float startTime = 0.4f;
	public float movementTime = 1f;
	public float waitTime = 1f;
	[Header ("In-Game")]
	public Text scoreText;
	public GameObject healthPanel;
	public Image timerFiller;
	public GameObject fadePanel;
    public GameObject bar;
    public GameObject bar2;
	public GameObject bar3;
    [Header ("High Scores Screen")]
	public GameObject highScoresPanel;
	public Text scoreMessageText;
	public List<Text> names;
	public List<Text> scores;
	public Text console;

	private static HUDController instance = null;
	private bool waiting = false;
	private int newScorePos = -1;

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
		scoreText.text = 0.ToString ("D5");
		StartCoroutine ("WaitInit");
	}

	void Update ()
	{
		if (scoreMessageText.enabled && !waiting) {
			StartCoroutine (WaitFade (scoreMessageText, 0.6f));
		}
		if (scoreMessageText.enabled && (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.KeypadEnter))) {
			highScoresPanel.SetActive (false);
			FadeScene (1.5f);
			StartCoroutine ("WaitLoadMenu", 1.5f);
		}
	}

	//Public methods
	public static HUDController Instance {
		get{ return instance; }
	}

	public void FadeScene (float time)
	{
		fadePanel.GetComponent<Image> ().DOFade (0f, 0f);
		fadePanel.SetActive (true);
		fadePanel.GetComponent<Image> ().DOFade (1f, time);
	}

	public void UpdateScore (int score)
	{
		scoreText.text = score.ToString ("D5");
		scoreText.rectTransform.DOComplete ();
		scoreText.rectTransform.DOPunchScale (new Vector3 (0.35f, 0.35f, 0.35f), 0.5f);
	}

	public void ActivateHighScoresPanel ()
	{
        bar.SetActive(false);
        bar2.SetActive(false);
		bar3.SetActive(false);
        newScorePos = -1;
		highScoresPanel.SetActive (true);
		for (int i = 0; i < scores.Count; i++) {
			scores [i].text = GameController.Instance.highScores [i].ToString ("D5");
			names [i].text = GameController.Instance.highScoresNames [i].ToString ();
			if (names [i].text.Equals ("   ")) {
				newScorePos = i;
			}
		}
		if (newScorePos != -1) {
			console.transform.position = names [newScorePos].transform.position;
			console.gameObject.SetActive (true);
			GetComponent<Console> ().enabled = true;
		} else {
			ActivateScoreMessage ();
		}
	}

	public void ActivateScoreMessage ()
	{
		scoreMessageText.enabled = true;
	}

	public void UpdateScoreName (string name)
	{
		if (newScorePos != -1) {
			names [newScorePos].text = name;
			console.gameObject.SetActive (false);
			GetComponent<Console> ().enabled = false;
			GameController.Instance.UpdateScoreName (newScorePos, name);
			ActivateScoreMessage ();
		}
	}

	public void UpdateHealth (int health)
	{
		switch (health) {
		case 0:
			healthPanel.SetActive (false);
			break;
		case 1:
			healthPanel.transform.GetChild (0).gameObject.GetComponent<Image>().enabled=true;
			healthPanel.transform.GetChild (1).gameObject.GetComponent<Image>().enabled = false;
                healthPanel.transform.GetChild (2).gameObject.GetComponent<Image>().enabled = false;
                break;
		case 2:
                healthPanel.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                healthPanel.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
                healthPanel.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false;
                break;
		case 3:
                healthPanel.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                healthPanel.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
                healthPanel.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = true;
                break;
		}
	}

	public void UpdateTimer (float fillAmount)
	{
		timerFiller.fillAmount = fillAmount;
	}

	//Private methods
	private IEnumerator WaitFade (Text text, float time)
	{
		waiting = true;
		text.DOFade (0f, time);
		yield return new WaitForSeconds (time);
		text.DOFade (1f, time);
		yield return new WaitForSeconds (time);
		waiting = false;
	}

	private IEnumerator WaitInit ()
	{
		yield return new WaitForSeconds (startTime);
		waveImage.transform.DOLocalMoveX (0f, movementTime, false);
		numberImage.transform.DOLocalMoveX (0f, movementTime, false);
		yield return new WaitForSeconds (movementTime + waitTime);

		if (GameController.Instance != null) {
			GameController.Instance.StartLevel ();
		} else if (TutorialController.Instance != null) {
			TutorialController.Instance.StartLevel ();
		}
		waveImage.transform.DOLocalMoveY (Screen.height + 800, movementTime, false);
		numberImage.transform.DOLocalMoveY (-Screen.height - 800, movementTime, false);
	}

	private IEnumerator WaitLoadMenu (float time)
	{
		yield return new WaitForSeconds (time);
		SceneManager.LoadScene ("Menu");
	}
}
