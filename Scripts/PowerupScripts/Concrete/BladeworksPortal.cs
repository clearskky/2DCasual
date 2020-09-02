using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeworksPortal : MonoBehaviour
{
    public GameObject pfab_spiritBlade;
    
    public int maxNumberOfSwordsToSpawn;
    private int currentNumberOfSwordsToSpawn;
    private float timeOfLastSwordSpawn;

    private float spawnAreaXAxisSemidiameter;
    [SerializeField] private float spawnAreaYAxisSemidiameter;
    [SerializeField] private float timeBetweenSwordSpawns;
    
    private Vector2 initialSpawnPosition;
    private Vector2 lastSpawnPosition;

    void Start()
    {
        spawnAreaXAxisSemidiameter = Screen.width / 2;
        currentNumberOfSwordsToSpawn = maxNumberOfSwordsToSpawn;
        initialSpawnPosition = new Vector2((spawnAreaYAxisSemidiameter * (-1) + 40), transform.position.y);
        lastSpawnPosition = Vector2.zero;
    }

    void Update()
    {
        SpawnSwordsInOrder();
    }

    public void SpawnSwordsInOrder()
    {
        if ((currentNumberOfSwordsToSpawn > 0) && (Time.time > (timeOfLastSwordSpawn + timeBetweenSwordSpawns)))
        {
            GameObject.Instantiate(pfab_spiritBlade, DetermineOrderedSpawnPosition(), Quaternion.identity);
            currentNumberOfSwordsToSpawn -= 1;

            if (currentNumberOfSwordsToSpawn <= 0)
            {
                Die();
            }

            timeOfLastSwordSpawn = Time.time;
        }
    }

    public void SpawnSwordsRandomly()
    {
        if ((currentNumberOfSwordsToSpawn > 0) && (Time.time > (timeOfLastSwordSpawn + timeBetweenSwordSpawns)))
        {
            GameObject.Instantiate(pfab_spiritBlade, DetermineRandomSpawnPosition(), Quaternion.identity);
            currentNumberOfSwordsToSpawn -= 1;

            if (currentNumberOfSwordsToSpawn <= 0)
            {
                Die();
            }

            timeOfLastSwordSpawn = Time.time;
        }
    }

    public Vector3 DetermineOrderedSpawnPosition()
    {
        if (lastSpawnPosition == Vector2.zero)
        {
            lastSpawnPosition = initialSpawnPosition;
            return new Vector3(initialSpawnPosition.x, initialSpawnPosition.y, transform.position.z);
        }
        else
        {
            float posX = lastSpawnPosition.x + (float)((spawnAreaXAxisSemidiameter * 2) / maxNumberOfSwordsToSpawn);
            float posY = Random.Range(transform.position.y, spawnAreaYAxisSemidiameter);

            lastSpawnPosition.x = posX;
            lastSpawnPosition.y = posY;

            return new Vector3(posX, posY, transform.position.z);
        }
        
    }

    public Vector3 DetermineRandomSpawnPosition()
    {
        float posX = Random.Range(transform.position.x - spawnAreaXAxisSemidiameter,
            transform.position.x + spawnAreaXAxisSemidiameter);
        float posY = Random.Range(transform.position.y - spawnAreaYAxisSemidiameter,
            transform.position.y + spawnAreaYAxisSemidiameter);

        return new Vector3(posX, posY, transform.position.z);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
