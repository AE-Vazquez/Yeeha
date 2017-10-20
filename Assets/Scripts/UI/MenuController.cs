using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
	//Attributes
	public GameObject scoresPanel;
	public List<Text> names;
	public List<Text> scores;
	public GameObject fadePanel;

	//Public methods
	public void OnPlayClick ()
	{
		if (GetComponent<AudioSource> ()) {
			GetComponent<AudioSource> ().Play ();
		}
		StartCoroutine ("WaitFade", 1.5f);
	}

	public void OnScoresClick ()
	{
		if (GetComponent<AudioSource> ()) {
			GetComponent<AudioSource> ().Play ();
		}
		LoadHighScores ();
		scoresPanel.SetActive (true);
	}

	public void OnScoresCloseClick ()
	{
		if (GetComponent<AudioSource> ()) {
			GetComponent<AudioSource> ().Play ();
		}
		scoresPanel.SetActive (false);
	}

	public void OnExitClick ()
	{
		if (GetComponent<AudioSource> ()) {
			GetComponent<AudioSource> ().Play ();
		}
		StartCoroutine ("WaitExit", 0.5f);
	}

	//Private method
	private void LoadHighScores ()
	{
		List<int> highScores = new List<int> ();
		List<string> highScoresNames = new List<string> ();
		string name;
		int score;
		for (int i = 1; i <= 10; i++) {
			name = PlayerPrefs.GetString (i.ToString (), "AAA");
			score = PlayerPrefs.GetInt (name, 0);
			highScoresNames.Add (name);
			highScores.Add (score);
		}
		for (int i = 0; i < names.Count; i++) {
			names [i].text = highScoresNames [i];
			scores [i].text = highScores [i].ToString ("D5");
		}
	}

	private IEnumerator WaitFade (float time)
	{
		fadePanel.GetComponent<Image> ().DOFade (0f, 0f);
		fadePanel.SetActive (true);
		fadePanel.GetComponent<Image> ().DOFade (1f, time);
		yield return new WaitForSeconds (time);
		SceneManager.LoadScene ("Tutorial");
	}

	private IEnumerator WaitExit (float time)
	{
		yield return new WaitForSeconds (time);
		Application.Quit ();
	}
}
