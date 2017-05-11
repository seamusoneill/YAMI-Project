using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DisconnectButton : MonoBehaviour {
    public  NetworkManager ble;
	// Use this for initialization
	void Start () {
        ble = FindObjectOfType<NetworkManager>();
     

    }
	
	// Update is called once per frame
	void Update () {
	
	}

   public void Disconnect()
    {
        print("TRY");
        
        Network.Disconnect();
        
        ble.StopClient();
        ble.StopHost();

        
    }
}
