using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FindNetWorkManager : MonoBehaviour {

    private NetworkManager Manager;

	// Use this for initialization
	void Start () {


        Manager = FindObjectOfType<NetworkManager>();

    }
	
	// Update is called once per frame
	void Update () {
	
	}


    public void Disconnect()
    {
        if(Network.isServer)
        {
            Manager.StopServer();
            Manager.StopHost();

        }
        Manager.StopServer();
        Manager.StopClient();

    }
}
