using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour
{
	//Attributes
	private static int MAX_ENEMIES = 20;

	[Header ("Enemy Prefabs")]
	public List<GameObject> enemies;
    public List<AudioClip> spawnSounds;

	private List<GameObject> pool = new List<GameObject> ();

    public AudioSource audioSource;

    Dictionary<WaveType, Queue<Enemy>> enemiesByType=new Dictionary<WaveType,Queue<Enemy>>();


    void Start()
    {

        audioSource=GetComponent<AudioSource>();
        enemiesByType.Add(WaveType.Type1, new Queue<Enemy>());
        enemiesByType.Add(WaveType.Type2, new Queue<Enemy>());
        enemiesByType.Add(WaveType.Type3, new Queue<Enemy>());
    }

	//Public methods
	public int SpawnRandomEnemy (float speedMultiplier)
	{
		if (pool.Count < MAX_ENEMIES) {
			int order = Random.Range (0, 3);
			enemies [order].GetComponent<Enemy> ().SetSpeedMultiplier (speedMultiplier);
			GameObject enemy = (GameObject)Instantiate (enemies [order]);
			float randomZ = Random.Range (transform.position.z - transform.localScale.z / 2, transform.position.z + transform.localScale.z / 2);
			enemy.transform.position = new Vector3 (transform.position.x, transform.position.y, randomZ);

			enemy.transform.Rotate (transform.rotation.eulerAngles);
            Enemy enemyScript = enemy.GetComponent<Enemy>();

            enemyScript.SetSpawner (this);
            if (audioSource)
            {
                audioSource.clip = spawnSounds[order];
                audioSource.Play();
            }
			pool.Add (enemy);

            enemiesByType[enemyScript.enemyType].Enqueue(enemyScript);
            
			return order;
		} else {
			return -1;
		}
	}

	public int SpawnEnemy (int enemyOrder, float speedMultiplier)
	{
		if (pool.Count < MAX_ENEMIES) {
			enemies [enemyOrder].GetComponent<Enemy> ().SetSpeedMultiplier (speedMultiplier);
			GameObject enemy = (GameObject)Instantiate (enemies [enemyOrder]);
			float randomZ = Random.Range (transform.position.z - transform.localScale.z / 2, transform.position.z + transform.localScale.z / 2);
			enemy.transform.position = new Vector3 (transform.position.x, transform.position.y, randomZ);

			enemy.transform.Rotate (transform.rotation.eulerAngles);

            enemy.GetComponent<Enemy>().SetSpawner(this);
            if (audioSource)
            {
                audioSource.clip = spawnSounds[enemyOrder];
                audioSource.Play();
            }
            pool.Add (enemy);

			return enemyOrder;
		} else {
			return -1;
		}
	}

	public GameObject GetNearestEnemy ()
	{
		float minDistance = float.MaxValue;
		GameObject enemy = null;
		for (int i = 0; i < pool.Count; i++) {
			if (!pool [i].GetComponent <Enemy> ().saved && !pool [i].GetComponent<Enemy> ().dead && pool [i].GetComponent<Enemy> ().GetDistanceToDeath () < minDistance) {
				minDistance = pool [i].GetComponent<Enemy> ().GetDistanceToDeath ();
				enemy = pool [i];
			}
		}
		return enemy;
	}

    public GameObject GetNearestEnemy(WaveType enemyType)
    {
        GameObject enemy=null;
        if(enemiesByType[enemyType].Count>0)
        {
            enemy = enemiesByType[enemyType].Peek().gameObject;
        }
        return enemy;
    }


    public void RemoveFromDictionary(Enemy enemy)
    {
        enemiesByType[enemy.enemyType].Dequeue();
    }

    public void DestroyEnemy (GameObject enemy)
	{
        
        pool.Remove (enemy);
		Destroy (enemy);
	}
}
