using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject zombiePrefab;
    public GameObject baseObject;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;

    private bool canSpawn = true;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (canSpawn)
        {
            // Check if player reached 200 points
            if (ScoreManager.instance.CurrentScore >= 200)
            {
                canSpawn = false;
                Debug.Log("🎉 Game Over! You reached 200 points!");

                // Stop spawning new zombies
                CancelInvoke();

                // Destroy all remaining zombies
                GameObject[] remainingZombies = GameObject.FindGameObjectsWithTag("Zombie");
                foreach (GameObject zombie in remainingZombies)
                {
                    Destroy(zombie);
                }

                // Show TMP win message
                ScoreManager.instance.ShowEndMessage("You've defended the lawn!");
                yield break;
            }

            SpawnZombie();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnZombie()
    {
        if (zombiePrefab == null || spawnPoints.Length == 0) return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newZombie = Instantiate(zombiePrefab, point.position, point.rotation);

        // Pass base reference to each zombie
        ZombieScript zs = newZombie.GetComponent<ZombieScript>();
        if (zs != null)
        {
            zs.baseObject = baseObject;
        }
    }
}
