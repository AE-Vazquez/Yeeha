using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	//Attributes
	[Header ("Scores")]
	public int globalScore;
	public int levelScore;
	public List<int> highScores;
	public List<string> highScoresNames;
	[Header ("Spawners")]
	public SpawnController leftSpawner;
	public SpawnController rightSpawner;
	[Header ("Health")]
	public int health = 3;
	[Header ("Configuration")]
	public float levelTime;
	public float minSpawnTime;
	public float maxSpawnTime;
	public float speedModifier;
	public float deathWaitTime = 1f;
	[Header ("Enemies")]
	public float saveDuration = 1.5f;

	private bool leftWaiting = false;
	private bool rightWaiting = false;
	private SpawnController nearestEnemyOrigin;
	private bool started = false;
	private static GameController instance = null;
	private Enemy targetEnemy = null;
	private float currentTime = 0f;

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
		LoadHighScores ();
		InputManager.Instance.OnWaveDetectedCallback += OnWaveDetected;

		InputManager.Instance.OnWaveStoppedCallback += OnWaveStopped;
	}


	void Update ()
	{
		if (started) {
			HandleSpawn ();
			HandleInput ();
			currentTime += Time.deltaTime;
			HUDController.Instance.UpdateTimer (currentTime / levelTime);
			if (currentTime >= levelTime) {
				//Level completed
				started = false;
				MasterController.Instance.lastLevelScore = levelScore;
				MasterController.Instance.LoadYeeha ();
			}
		}
	}
		
	//Public methods
	public static GameController Instance {
		get{ return instance; }
	}

	public void StartLevel ()
	{
		started = true;
	}

	public void DamageHealth ()
	{
		health--;
		HUDController.Instance.UpdateHealth (health);
		//Death
		if (health == 0) {
			started = false;
			CheckHighScore ("   ", globalScore);
			StartCoroutine ("WaitGameOver", deathWaitTime);
			MasterController.Instance.Reset ();
		}
	}

	public void UpdateScoreName (int pos, string name)
	{
		highScoresNames [pos] = name;
		SaveHighScores ();
	}

	public void OnWaveDetected ()
	{
		if (targetEnemy != null) {
			return;
		}
		GameObject firstEnemy = GetNearestEnemy (InputManager.Instance.GetWaveType ());
		if (firstEnemy == null) {
			return;
		}
		/*
		if (firstEnemy == null || firstEnemy.GetComponent<Enemy> ().enemyType != InputManager.Instance.GetWaveType ()) {
			return;
		}
        */
		targetEnemy = firstEnemy.GetComponent<Enemy> ();
		targetEnemy.StartSaving ();
	}

	public void OnWaveStopped ()
	{
		if (targetEnemy != null) {
			targetEnemy.GetComponent<Enemy> ().CancelSave ();
			targetEnemy = null;
		}
	}

	//Private methods
	private void HandleInput ()
	{
		GameObject firstEnemy = GetNearestEnemy ();

		if (Input.GetButtonDown ("Type1")) {
			if (firstEnemy != null && firstEnemy.GetComponent<Enemy> ().enemyType.Equals (WaveType.Type1)) {
				firstEnemy.GetComponent<Enemy> ().StartSaving ();
				//nearestEnemyOrigin.DestroyEnemy (firstEnemy);
				//UpdateScore (firstEnemy.GetComponent<Enemy> ());
			}
		}
		if (Input.GetButtonDown ("Type2")) {
			if (firstEnemy != null && firstEnemy.GetComponent<Enemy> ().enemyType.Equals (WaveType.Type2)) {
				firstEnemy.GetComponent<Enemy> ().StartSaving ();
				//nearestEnemyOrigin.DestroyEnemy (firstEnemy);
				//UpdateScore (firstEnemy.GetComponent<Enemy> ());
			}
		}
		if (Input.GetButtonDown ("Type3")) {
			if (firstEnemy != null && firstEnemy.GetComponent<Enemy> ().enemyType.Equals (WaveType.Type3)) {
				firstEnemy.GetComponent<Enemy> ().StartSaving ();
				//nearestEnemyOrigin.DestroyEnemy (firstEnemy);
				//UpdateScore (firstEnemy.GetComponent<Enemy> ());
			}
		}
	}

	private void HandleSpawn ()
	{
		if (!leftWaiting) {
			leftSpawner.SpawnRandomEnemy (speedModifier);
			float wait = Random.Range (minSpawnTime, maxSpawnTime);
			StartCoroutine ("WaitLeftSpawn", wait);
		}
		if (!rightWaiting) {
			rightSpawner.SpawnRandomEnemy (speedModifier);
			float wait = Random.Range (minSpawnTime, maxSpawnTime);
			StartCoroutine ("WaitRightSpawn", wait);
		}
	}

	private GameObject GetNearestEnemy ()
	{
		GameObject leftEnemy = leftSpawner.GetNearestEnemy ();
		GameObject rightEnemy = rightSpawner.GetNearestEnemy ();
		if (leftEnemy == null) {
			nearestEnemyOrigin = rightSpawner;
			return rightEnemy;
		} else if (rightEnemy == null) {
			nearestEnemyOrigin = leftSpawner;
			return leftEnemy;
		} else {
			if (leftEnemy.GetComponent<Enemy> ().GetDistanceToDeath () < rightEnemy.GetComponent<Enemy> ().GetDistanceToDeath ()) {
				nearestEnemyOrigin = leftSpawner;
				return leftEnemy;
			} else {
				nearestEnemyOrigin = rightSpawner;
				return rightEnemy;
			}
		}
	}

	private GameObject GetNearestEnemy (WaveType type)
	{
		GameObject leftEnemy = leftSpawner.GetNearestEnemy (type);
		GameObject rightEnemy = rightSpawner.GetNearestEnemy (type);
		if (leftEnemy == null) {
			nearestEnemyOrigin = rightSpawner;
			return rightEnemy;
		} else if (rightEnemy == null) {
			nearestEnemyOrigin = leftSpawner;
			return leftEnemy;
		} else {
			if (leftEnemy.GetComponent<Enemy> ().GetDistanceToDeath () < rightEnemy.GetComponent<Enemy> ().GetDistanceToDeath ()) {
				nearestEnemyOrigin = leftSpawner;
				return leftEnemy;
			} else {
				nearestEnemyOrigin = rightSpawner;
				return rightEnemy;
			}
		}
	}

	private void UpdateScore (Enemy enemy)
	{
		if (enemy.GetDistanceToDeath () <= 10f) {
			globalScore += (int)enemy.GetDistanceToDeath ();
			levelScore += (int)enemy.GetDistanceToDeath ();
		} else {
			globalScore += 10;
			levelScore += 10;
		}
		HUDController.Instance.UpdateScore (globalScore);
	}

	private void LoadHighScores ()
	{
		highScores = new List<int> ();
		highScoresNames = new List<string> ();
		string name;
		int score;
		for (int i = 1; i <= 10; i++) {
			name = PlayerPrefs.GetString (i.ToString (), "AAA");
			score = PlayerPrefs.GetInt (name, 0);
			highScoresNames.Add (name);
			highScores.Add (score);
		}
	}

	private void CheckHighScore (string name, int score)
	{
		int pos = -1;
		for (int i = 0; i < 10; i++) {
			if (highScores [i] < score) {
				pos = i;
				break;
			}
		}
		if (pos != -1) {
			highScores.Insert (pos, score);
			highScoresNames.Insert (pos, name);

			highScores.RemoveAt (10);
			highScoresNames.RemoveAt (10);
		}
	}

	private void SaveHighScores ()
	{
		PlayerPrefs.DeleteAll ();
		for (int i = 1; i <= 10; i++) {
			PlayerPrefs.SetString (i.ToString (), highScoresNames [i - 1]);
			PlayerPrefs.SetInt (highScoresNames [i - 1], highScores [i - 1]);
		}
	}

	private IEnumerator WaitLeftSpawn (float waitTime)
	{
		leftWaiting = true;
		yield return new WaitForSeconds (waitTime);
		leftWaiting = false;
	}

	private IEnumerator WaitRightSpawn (float waitTime)
	{
		rightWaiting = true;
		yield return new WaitForSeconds (waitTime);
		rightWaiting = false;
	}

	private IEnumerator WaitGameOver (float time)
	{
		yield return new WaitForSeconds (time);
		HUDController.Instance.ActivateHighScoresPanel ();
	}

	public void NotifySave ()
	{
		targetEnemy = null;
	}
}
