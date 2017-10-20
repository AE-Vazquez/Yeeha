using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
	//Attributes
	public WaveType enemyType;
	public float speed;

	private float multiplier = 0f;

	public bool saved = false;

	public bool dead = false;

	private SpawnController spawner;

	void Start ()
	{
		BeginMovement ();
	}

	public void SetSpawner (SpawnController sp)
	{
		this.spawner = sp;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag.Equals ("Death")) {
			Die ();

		}
	}

	private void BeginMovement ()
	{
		transform.DOKill (transform);
		if (multiplier > 1f) {
			transform.DOMoveX (317.4f, speed * multiplier, false).SetEase (Ease.Linear).SetSpeedBased ();
		} else {
			transform.DOMoveX (317.4f, speed, false).SetEase (Ease.Linear).SetSpeedBased ();
		}
	}

	//Public methods
	public void SetSpeedMultiplier (float multiplier)
	{
		this.multiplier = multiplier;
	}

	public float GetDistanceToDeath ()
	{
		return Mathf.Abs (transform.position.x);
	}

	public void StartSaving ()
	{
		if (!saved) {
			Debug.Log ("Salvando...");
			DOTween.Kill (transform);
			transform.DOShakePosition (GameController.Instance.saveDuration, 0.1f);
			StartCoroutine (WaitUntilSave ());
		}
	}

	IEnumerator WaitUntilSave ()
	{
		float m_coroutineTime = GameController.Instance.saveDuration;

		while (m_coroutineTime > 0) {
			m_coroutineTime -= Time.deltaTime;
			yield return 0;
		}
		saved = true;
		GameController.Instance.NotifySave ();
		CompleteSave ();

		Cheer ();
		yield return 0;
	}

	public void Cheer ()
	{
		//Mostrar particulas o algo
	}

	/// <summary>
	/// Cambia de direccion hacia su spawned y se desvanece en 2 segundos
	/// </summary>
	public void CompleteSave ()
	{
        spawner.RemoveFromDictionary(this);
        DOTween.Kill (transform);
		transform.Rotate (0, 180, 0);
		transform.DOMoveX (spawner.transform.position.x, 1.5f * speed * Mathf.Clamp (multiplier, 1, multiplier), false).SetEase (Ease.Linear).SetSpeedBased ();
		transform.GetChild(1).GetComponent<Renderer> ().material.DOFade (0f, 1f).OnComplete (DestroySelf);
	}


	public void CancelSave ()
	{
		GameController.Instance.NotifySave ();
		Debug.Log ("HAS DEJADO DE SALVARME CABRON");
		StopAllCoroutines ();
		DOTween.Kill (transform);
		BeginMovement ();
	}

	public void DestroySelf ()
	{
		spawner.DestroyEnemy (gameObject);
	}

	void Die ()
	{
        spawner.RemoveFromDictionary(this);
		DOTween.Kill (transform);
		dead = true;
		if (GetComponent<Animator> () != null)
			GetComponent<Animator> ().SetTrigger ("fall");

		transform.DOMoveY (-10f, speed * 3, false).SetEase (Ease.Linear).SetSpeedBased ();
		transform.GetChild(1).GetComponent<Renderer> ().material.DOFade (0f, 1f).OnComplete (DestroySelf);
		GameController.Instance.DamageHealth ();
	}
}
