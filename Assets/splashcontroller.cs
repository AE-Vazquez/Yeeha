using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class splashcontroller : MonoBehaviour {

	public GameObject imagen1;
	public GameObject imagen2;
	public GameObject imagen3;

	// Use this for initialization
	void Start () {

		StartCoroutine ("splashintro");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator splashintro(){

		imagen1.GetComponent<Image> ().DOFade (0f, 0f);
		imagen2.GetComponent<Image> ().DOFade (0f, 0f);
		imagen3.GetComponent<Image> ().DOFade (0f, 0f);

		imagen1.SetActive (true);
		imagen1.GetComponent<Image> ().DOFade (1f, 1f);

		yield return new WaitForSeconds (1.5f);
		imagen1.GetComponent<Image> ().DOFade (0f, 1f);

		yield return new WaitForSeconds (1f);
		imagen1.SetActive (false);


		imagen2.SetActive (true);
		imagen2.GetComponent<Image> ().DOFade (1f, 1f);

		yield return new WaitForSeconds (1.5f);
		imagen2.GetComponent<Image> ().DOFade (0f, 1f);

		yield return new WaitForSeconds (1f);
		imagen2.SetActive (false);


		imagen3.SetActive (true);
		imagen3.GetComponent<Image> ().DOFade (1f, 1f);

		yield return new WaitForSeconds (3f);
		imagen3.GetComponent<Image> ().DOFade (0f, 1f);

		yield return new WaitForSeconds (1f);
		imagen3.SetActive (false);


		SceneManager.LoadScene ("Menu");
	}
}
