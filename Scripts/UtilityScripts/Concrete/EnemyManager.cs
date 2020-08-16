using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IManager
{
    private static EnemyManager _instance;
    public static EnemyManager Instance;

    public IEnemy pfab_FlyingEye;
    public IEnemy pfab_Scroller;

    public float TimeBetweenFlyingEyeSpawns, TimeOfLastFlyingEyeSpawn;
    public float TimeBetweenScrollerSpawns, TimeOfLastScrollerSpawn;

    public int MaxFlyingEyeCount, CurrentFlyingEyeCount;
    public int MaxScrollerCount, CurrentScrollerCount;

    void Start()
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
        IncrementRespawnTimers();
    }

    public void ManageEnemySpawns()
    {
        if (Time.time > (TimeOfLastFlyingEyeSpawn + TimeBetweenFlyingEyeSpawns) && CurrentFlyingEyeCount < MaxFlyingEyeCount)
        {
            SpawnEnemy<FlyingEye>(pfab_FlyingEye);
        }
    }

    public void IncrementRespawnTimers()
    {

    }

    public void SpawnEnemy<EnemyType>(IEnemy enemyPrefab) where EnemyType : MonoBehaviour
    {
        GameObject.Instantiate((EnemyType)enemyPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0,0,0));
    }

    public void DestroyAllEnemies()
    {
        for (int childIndex = 0; childIndex < transform.childCount; childIndex++)
        {
            IEnemy child = transform.GetChild(childIndex).GetComponent<IEnemy>();
            child.InitiateDeathRoutine();
        }
    }
}
