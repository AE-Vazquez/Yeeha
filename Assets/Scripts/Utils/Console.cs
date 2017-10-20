using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
	public Text consoleText;
	public int flickerTime;

	private string nameScore;
	private int time;
	private bool promptShown;
	private bool hidden = false;

	// Use this for initialization
	void Start ()
	{
		nameScore = "";
		consoleText.text = "";
		time = 0;
		promptShown = false;
	}

	void FixedUpdate ()
	{
		if (!hidden) {
			if (time >= flickerTime) {
				time = 0;
				FlickerChange ();
			} else {
				time++;
			}
		} else if (hidden && consoleText.text.Length > 3) {
			FlickerChange ();
		}
	}

	// Update is called once per frame
	void Update ()
	{
		foreach (char c in Input.inputString) {
			if (c == "\b" [0]) {
				// User wants to delete
				if (nameScore.Length > 0) {
					consoleText.text = consoleText.text.Substring (0, consoleText.text.Length - 1);
					nameScore = nameScore.Substring (0, nameScore.Length - 1);
					if (hidden) {
						hidden = false;
					}
				}
			} else {
				if (c == "\n" [0] || c == "\r" [0]) {
					// User entered name
					HUDController.Instance.UpdateScoreName (nameScore);
					nameScore = "";
					consoleText.text = "";
				} else {
					nameScore += c;
					if ((promptShown) && (consoleText.text.Length > 0)) {
						consoleText.text = consoleText.text.Substring (0, consoleText.text.Length - 1);
						consoleText.text += c;
						if (consoleText.text.Length < 3) {
							consoleText.text += "_"; 
						} else {
							hidden = true;
						}
					} else {
						consoleText.text += c;
					}
				}
			}
		}
	}

	void FlickerChange ()
	{
		promptShown = !promptShown;
		FlickerUpdate ();
	}

	void FlickerUpdate ()
	{
		if (promptShown) {
			consoleText.text += "_";
		} else if (consoleText.text.Length > 0) {
			consoleText.text = consoleText.text.Substring (0, consoleText.text.Length - 1);
		}
	}
}