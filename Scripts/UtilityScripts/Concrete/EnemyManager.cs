using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IManager
{
    private static EnemyManager _instance;
    public static EnemyManager Instance {get {return _instance;}}

    public GameObject pfab_FlyingEye;
    public GameObject pfab_Scroller;

    public float TimeBetweenFlyingEyeSpawns; 
    private float _timeOfLastFlyingEyeSpawn;

    public float TimeBetweenScrollerSpawns;
    private float _timeOfLastScrollerSpawn;

    public int MaxFlyingEyeCount;
    private int _currentFlyingEyeCount;

    public int MaxScrollerCount; //Scroller is a type of enemy. It may or may not already be in the game depending on when you're reading this.
    private int _currentScrollerCount;

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
        //IncrementRespawnTimers();
    }

    public void ManageEnemySpawns()
    {

        SpawnFlyingEye();
    }

    public void SpawnFlyingEye()
    {
        if ((_currentFlyingEyeCount < MaxFlyingEyeCount) && (Time.time > (_timeOfLastFlyingEyeSpawn + TimeBetweenFlyingEyeSpawns)))
        {
            GameObject.Instantiate(pfab_FlyingEye, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0), gameObject.transform);
            _currentFlyingEyeCount += 1;
            _timeOfLastFlyingEyeSpawn = Time.time;
        }
        
    }

    // All enemies are children of the EnemyManager in the hierarchy
    // EnemyManager kills all of its children with this method. Might be useful for a future powerup.
    public void DestroyAllEnemies()
    {
        for (int childIndex = 0; childIndex < transform.childCount; childIndex++)
        {
            IEnemy child = transform.GetChild(childIndex).GetComponent<IEnemy>();
            child.InitiateDeathRoutine();
        }
    }

    public void DecrementCurrentFlyingEyeCount()
    {
        _currentFlyingEyeCount -= 1;
    }

    public void DecrementCurrentScrollerCount()
    {
        _currentScrollerCount -= 1;
    }
}
