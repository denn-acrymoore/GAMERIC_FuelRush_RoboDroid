using System.Collections;
using UnityEngine;

public class ParticleGenerator : MonoBehaviour
{
    [SerializeField] private Transform particleSpawnPoint;
    [SerializeField] private GameObject[] particles;
    [SerializeField] private float spawnDelaySeconds = 2f;
    [SerializeField] private float spawnXOffset = 8f;
    [SerializeField] private float spawnYOffset = 2f;
    [SerializeField] private float spawnZOffset = 5f;

    private int currIdx = 0;

    private void Start()
    {
        StartCoroutine("SpawnParticleWithDelay");
    }

    IEnumerator SpawnParticleWithDelay()
    {
        while (true)
        {
            float randomX = Random.Range(-spawnXOffset, spawnXOffset);
            float randomY = Random.Range(0, spawnYOffset);
            float randomZ = Random.Range(-spawnZOffset, spawnZOffset);

            Instantiate(particles[(int)currIdx]
                , particleSpawnPoint.transform.position +  new Vector3(randomX, randomY, randomZ)
                , Quaternion.identity);

            ++currIdx;
            if (currIdx >= particles.Length)
            {
                currIdx = 0;
            }

            yield return new WaitForSeconds(spawnDelaySeconds);
        }
    }
}
