using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IManager
{
    private static EnemyManager _instance;
    public static EnemyManager Instance {get {return _instance;}}

    public GameObject pfab_FlyingEye;
    public GameObject pfab_Scroller;

    public int spawnAreaXAxisSemidiameter, spawnAreaYAxisSemidiameter;

    public float timeBetweenFlyingEyeSpawns; 
    private float timeOfLastFlyingEyeSpawn;

    public float timeBetweenScrollerSpawns;
    private float timeOfLastScrollerSpawn;

    public int maxFlyingEyeCount;
    [SerializeField] private int currentFlyingEyeCount;

    public int maxScrollerCount; //Scroller is a type of enemy. It may or may not already be in the game depending on when you're reading this.
    [SerializeField] private int currentScrollerCount;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Update()
    {
        ManageEnemySpawns();
    }

    public void ManageEnemySpawns()
    {
        SpawnFlyingEyes();
        SpawnScrollers();
    }

    public void SpawnFlyingEyes()
    {
        if ((currentFlyingEyeCount < maxFlyingEyeCount) && ((Time.time > (timeOfLastFlyingEyeSpawn + timeBetweenFlyingEyeSpawns)) || (currentFlyingEyeCount <= 0)))
        {
            GameObject.Instantiate(pfab_FlyingEye, DetermineSpawnPosition(), Quaternion.Euler(0, 0, 0), gameObject.transform);
            currentFlyingEyeCount += 1;
            timeOfLastFlyingEyeSpawn = Time.time;
        }
    }

    public void SpawnScrollers()
    {
        if ((currentScrollerCount < maxScrollerCount) && (Time.time > (timeOfLastScrollerSpawn + timeBetweenScrollerSpawns)))
        {
            GameObject.Instantiate(pfab_Scroller, DetermineSpawnPosition(), Quaternion.Euler(0, 0, 0), gameObject.transform);
            currentScrollerCount += 1;
            timeOfLastScrollerSpawn = Time.time;
        }
    }

    public Vector3 DetermineSpawnPosition()
    {
        float posX = Random.Range(transform.position.x - spawnAreaXAxisSemidiameter,
            transform.position.x + spawnAreaXAxisSemidiameter);
        float posY = Random.Range(transform.position.y - spawnAreaYAxisSemidiameter,
            transform.position.y + spawnAreaYAxisSemidiameter);

        return new Vector3(posX, posY, transform.position.z);
    }

    // All enemies are children of the EnemyManager in the hierarchy
    // EnemyManager kills all of its children with this method. Might be useful for a future powerup.
    public void DestroyAllEnemies()
    {
        for (int childIndex = 0; childIndex < transform.childCount; childIndex++)
        {
            IEnemy child = transform.GetChild(childIndex).GetComponent<IEnemy>();
            child.Die();
        }
    }

    public void DecrementCurrentFlyingEyeCount()
    {
        currentFlyingEyeCount -= 1;
    }

    public void DecrementCurrentScrollerCount()
    {
        currentScrollerCount -= 1;
    }
}
