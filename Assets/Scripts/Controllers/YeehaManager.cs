using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class YeehaManager : MonoBehaviour
{

	public float time;

	public Text scoreText;
	public Text introText;
	public Text messageText;

	public GameObject yeehaPanel;

	public Slider barSlider;
	public GameObject sliderParticles;

	public Image yeehaImage;
	public Image x2image, x4image, x8image, x16image;

	private float multiplier = 1;

	public GameObject confeti;

	private bool listening = false;
	private bool fading = false;
	// Use this for initialization
	void Start ()
	{
		scoreText.text = "" + MasterController.Instance.lastLevelScore;
		scoreText.text = "" + 10000;
		VoiceInput.Instance.OnInputStartedCallback += InputStarted;
		VoiceInput.Instance.OnInputStoppedCallback += InputStopped;

		//Invoke("InputStarted",2f);

	}
	
	// Update is called once per frame
	void Update ()
	{
  
		if (listening) {
			yeehaImage.fillAmount += ((Time.deltaTime / time) * GetYeehaIncrement ());
			barSlider.value += ((Time.deltaTime / time) * GetYeehaIncrement ());
			if (yeehaImage.fillAmount >= 1 && multiplier < 16) {
				IncreaseParticleEmission ();
				x16image.rectTransform.DOPunchScale (new Vector3 (0.5f, 0.5f, 0.5f), 1.5f);
				multiplier = 16;
				listening = false;
			} else if (yeehaImage.fillAmount > 0.67f && multiplier < 8) {
				IncreaseParticleEmission ();
				x8image.rectTransform.DOPunchScale (new Vector3 (0.5f, 0.5f, 0.5f), 1.5f);
				multiplier = 8;
			} else if (yeehaImage.fillAmount > 0.34f && multiplier < 4) {
				IncreaseParticleEmission ();
				x4image.rectTransform.DOPunchScale (new Vector3 (0.5f, 0.5f, 0.5f), 1.5f);
				multiplier = 4;
			} else if (yeehaImage.fillAmount > 0.14f && multiplier < 2) {
				IncreaseParticleEmission ();
				x2image.rectTransform.DOPunchScale (new Vector3 (0.5f, 0.5f, 0.5f), 1.5f);
				multiplier = 2;
			}
		}
		if (!fading) {
			StartCoroutine (WaitFade(introText, 0.6f));
		}
		if (messageText.enabled && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))) {
			MasterController.Instance.LoadNextLevel ();
		}
	}

	public void InputStarted ()
	{
		introText.gameObject.SetActive (false);
		yeehaPanel.SetActive (true);
		VoiceInput.Instance.OnInputStartedCallback -= InputStarted;
		listening = true;
		Invoke ("TimeEnd", time);
	}

	public void InputStopped ()
	{
		if (listening) {
			VoiceInput.Instance.OnInputStoppedCallback -= InputStopped;
			TimeEnd ();
		}

	}

	private float GetYeehaIncrement ()
	{
		if (VoiceInput.Instance.dbValue > 5f) {
			return 1.5f;
		} else {

			return 1f;
		}
	}

	void UpdateScore ()
	{
		MasterController.Instance.lastLevelScore *= (int)multiplier;
		scoreText.text = "" + MasterController.Instance.lastLevelScore;
		scoreText.rectTransform.DOPunchScale (new Vector3 (0.35f, 0.35f, 0.35f), 0.5f);

	}

	public void TimeEnd ()
	{
		listening = false;
		DisableParticles ();
		CancelInvoke ();
		StartCoroutine (FinishYeeha ());
	}

	IEnumerator FinishYeeha ()
	{
		yield return new WaitForSeconds (1.5f);
		UpdateScore ();
		if (confeti) {
			confeti.SetActive (true);
		}
		messageText.enabled = true;
		StartCoroutine (WaitFade(messageText, 0.6f));
	}

	void DisableParticles ()
	{
        if (sliderParticles.GetComponentsInChildren<EllipsoidParticleEmitter>()[0] != null)
        {
            sliderParticles.GetComponentsInChildren<EllipsoidParticleEmitter>()[0].emit = false;
            sliderParticles.GetComponentsInChildren<EllipsoidParticleEmitter>()[1].emit = false;
        }
	}

	void IncreaseParticleEmission ()
	{
        if (sliderParticles.GetComponentsInChildren<EllipsoidParticleEmitter>()[0] != null) {
            sliderParticles.GetComponentsInChildren<EllipsoidParticleEmitter>()[0].minEmission *= 1.8f;
            sliderParticles.GetComponentsInChildren<EllipsoidParticleEmitter>()[0].maxEmission *= 1.8f;
        }
        if (sliderParticles.GetComponentsInChildren<EllipsoidParticleEmitter>()[1] != null)
        {
            sliderParticles.GetComponentsInChildren<EllipsoidParticleEmitter>()[1].minEmission *= 1.8f;
            sliderParticles.GetComponentsInChildren<EllipsoidParticleEmitter>()[1].maxEmission *= 1.8f;
        }
	}

	private IEnumerator WaitFade (Text text, float time)
	{
		fading = true;
		text.DOFade (0f, time);
		yield return new WaitForSeconds (time);
		text.DOFade (1f, time);
		yield return new WaitForSeconds (time);
		fading = false;
	}
}
