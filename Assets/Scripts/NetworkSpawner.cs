using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;


public class NetworkSpawner : Action
{



 

    public List<GameObject> GameObjectsToSpawn;
    public bool SpawnAtLaunch;
    [SyncVar]
    bool HasBeenUsed = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnStartServer()
    {


        if (SpawnAtLaunch)
        {


                foreach (GameObject objects in GameObjectsToSpawn)
                {
                    Vector3 spawnPosition = transform.position;

                    Quaternion spawnRotation = Quaternion.identity;

                    GameObject ToSpawn = (GameObject)Instantiate(objects, spawnPosition, spawnRotation);
                    NetworkServer.Spawn(ToSpawn);
                }

            

            
        }
    }
    [ServerCallback]
    public override void  Use()
    {

        if (HasBeenUsed)
        {
            return;
        }

     
        foreach (GameObject objects in GameObjectsToSpawn)
        {
            spawn(objects,0);
          
        }

        HasBeenUsed = true;


    }


    public virtual void spawn(GameObject objects, int Animstate)
    {

        Vector3 spawnPosition = transform.position;

        Quaternion spawnRotation = Quaternion.identity;

        GameObject ToSpawn = (GameObject)Instantiate(objects, spawnPosition, spawnRotation);
        NetworkServer.Spawn(ToSpawn);


    }


}