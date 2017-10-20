using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MasterController : MonoBehaviour
{
	//Attributes
	public int globalScore;
	public int lastLevelScore;
	public int lastLevel;

	private static MasterController instance = null;

	void Awake ()
	{
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad (gameObject);
	}

	//Public methods
	public static MasterController Instance {
		get { return instance; }
	}

	public void Reset ()
	{
		globalScore = 0;
		lastLevelScore = 0;
		lastLevel = 0;
	}


	public void LoadNextLevel ()
	{
		Debug.Log ("Last level " + lastLevel);
		globalScore += lastLevelScore;
		lastLevelScore = 0;
		lastLevel++;
		LoadLevel ();
	}

	//Private methods
	private void LoadLevel ()
	{
		if (HUDController.Instance.fadePanel != null) {
			HUDController.Instance.FadeScene (1.5f);
		}
		StartCoroutine ("WaitFadeLoad", 1.5f);
	}

	public void LoadYeeha()
	{
		HUDController.Instance.FadeScene (1.5f);
		StartCoroutine (WaitFadeLoadYeeha(1.5f));
	}

	private IEnumerator WaitFadeLoadYeeha (float time)
	{
		yield return new WaitForSeconds (time);
		SceneManager.LoadScene("YeehaScene");
		yield return 0;
	}


	private IEnumerator WaitFadeLoad (float time)
	{
		yield return new WaitForSeconds (time);
		switch (lastLevel) {
            case 1:
                SceneManager.LoadScene("Wave1");
                break;
            case 2:
			    SceneManager.LoadScene ("Wave2");
			break;
		    case 3:
			    SceneManager.LoadScene ("Wave3");
			    break;
		    case 4:
			    SceneManager.LoadScene ("Wave4");
			    break;
		    case 5:
			    SceneManager.LoadScene ("Wave5");

                break;
		case 6:
			lastLevel = 4;
			    SceneManager.LoadScene ("Wave4");
			    break;
		}
	}
}
