using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CabaneSpawner : NetworkBehaviour
{

   public GameObject CabaneWithAxe;
  public  GameObject CabaneWithoutAxe;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void spawn(int TypeOfCaban)
    {
        GameObject obj;
        if (TypeOfCaban == 1)
        {
             obj = CabaneWithAxe;

        }
        else
        {
             obj = CabaneWithoutAxe;

        }
      
        Vector3 spawnPosition = transform.position;

        Quaternion spawnRotation = Quaternion.identity;

        GameObject ToSpawn = (GameObject)Instantiate(obj, spawnPosition, spawnRotation);
        NetworkServer.Spawn(ToSpawn);
    }



}
