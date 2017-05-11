using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class CabaneGenerator : NetworkBehaviour {

    public List<GameObject> CabanSpawns;
    private int numberofspawn;

    public override void OnStartServer()
    {
        int GoodCabanRandom = Random.Range(0, 2);
        int iterator = 0;

        foreach(GameObject obj in CabanSpawns)
        {
            if(GoodCabanRandom == iterator)
            {
                obj.GetComponent<CabaneSpawner>().spawn(1);

            }
            else
            {
                obj.GetComponent<CabaneSpawner>().spawn(0);
            }
            iterator = iterator + 1;

        }






    }
        // Use this for initialization
        void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
