using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeworksPortal : MonoBehaviour
{
    public GameObject pfab_spiritBlade;

    [SerializeField] private int numberOfSwordsToSpawn;
    [SerializeField] private int spawnAreaXAxisSemidiameter, spawnAreaYAxisSemidiameter;
    [SerializeField] private float timeBetweenSwordSpawns;
    
    private float timeOfLastSwordSpawn;

    void Update()
    {
        SpawnSwords();
    }

    public void SpawnSwords()
    {
        if ((numberOfSwordsToSpawn > 0) && (Time.time > (timeOfLastSwordSpawn + timeBetweenSwordSpawns)))
        {
            GameObject.Instantiate(pfab_spiritBlade, DetermineSpawnPosition(), Quaternion.identity);
            numberOfSwordsToSpawn -= 1;

            if (numberOfSwordsToSpawn <= 0)
            {
                Die();
            }

            timeOfLastSwordSpawn = Time.time;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public Vector3 DetermineSpawnPosition()
    {
        float posX = Random.Range(transform.position.x - spawnAreaXAxisSemidiameter,
            transform.position.x + spawnAreaXAxisSemidiameter);
        float posY = Random.Range(transform.position.y - spawnAreaYAxisSemidiameter,
            transform.position.y + spawnAreaYAxisSemidiameter);

        return new Vector3(posX, posY, transform.position.z);
    }
}
