using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroscopeManager : MonoBehaviour
{
    public int baseMovement;
    //public Text debugText;

    public Transform hills;
    public Transform mountains;
    public Transform wideCloud;

    public int mountainMovementMult, hillsMovementMult, wideCloudMovementMult;

    private Gyroscope gyroscope;
    private Quaternion rotation;
    private bool isGyroscopeActive;

    private float defaultMountainXPos, defaultHillsXPos, defaultWideCloudXPos;
    

    void Awake()
    {
        defaultMountainXPos = mountains.position.x;
        defaultHillsXPos = hills.position.x;
        defaultWideCloudXPos = wideCloud.position.x;
    }

    void Start()
    {
        EnableGyroscope();
    }
    void Update()
    {
        GyroMoveTheWorld();
    }

    public void EnableGyroscope()
    {
        if (!isGyroscopeActive)
        {
            if (SystemInfo.supportsGyroscope)
            {
                gyroscope = Input.gyro;
                gyroscope.enabled = true;
                isGyroscopeActive = true;
            }
        }
    }

    void GyroMoveTheWorld()
    {
        rotation = GyroToUnity(Input.gyro.attitude);
        //debugText.text = "x=" + rotation.x + " y=" + rotation.y + " z=" + rotation.z;
        mountains.position = new Vector3((defaultMountainXPos + (rotation.z * baseMovement * mountainMovementMult)), mountains.position.y, mountains.position.z);
        hills.position = new Vector3((defaultHillsXPos + (rotation.z * baseMovement * hillsMovementMult)), hills.position.y, hills.position.z);
        wideCloud.position = new Vector3((defaultWideCloudXPos + (rotation.z * baseMovement * wideCloudMovementMult)), wideCloud.position.y, wideCloud.position.z);
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
