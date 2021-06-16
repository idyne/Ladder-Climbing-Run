using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    private bool isObjectSpawning = false;
    ObjectPooler objectPooler;
    public static ObjectSpawner instance;

    private void Awake()
    {
        instance = this;
        
    }
    private void Start()
    {

        objectPooler = ObjectPooler.Instance;
        SpawnLadder();
    }
    public void SpawnLadder()
    {
        for(int i =0; i<50; i++)
        {
            objectPooler.SpawnFromPool("Ladder", new Vector3(0, i, 0), Quaternion.identity);
        }
        
    }
}
