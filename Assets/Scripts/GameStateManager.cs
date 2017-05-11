using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameStateManager : NetworkManager {

    private List<GameObject> players;
    bool Isonline;
    //public GameObject Voiture;

	// Use this for initialization
	void Start () {
        Isonline = true;
        players = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {

        if(Isonline)
        {


            print(players.Count);
            CheckHealth();
        }

  
	
	}

    public virtual void OnClientEnterLobby()
    {
        print("lil");

    }

    public override void OnStartServer()
    {
        Isonline = true;
        base.OnStartServer();
    }

    public  override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if (Isonline)
        {
            GameObject player = (GameObject)GameObject.Instantiate(playerPrefab, GetStartPosition().position, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

            players.Add(player);

        }


    }

    private void CheckHealth()
    {

        foreach(GameObject joueurs in players)
        {

            if (joueurs.GetComponent<Player>().playerState == 2)
            {

                print(" HE'S DEAD MAURICE");
                this.ServerChangeScene("Menutest");
                Isonline = false;
                Network.Disconnect();
               
                this.StopHost();
          
            }
        }
        if ( ! Isonline)
        {
            players.Clear();
        }
/*
        if (Voiture.GetComponent<Health>().getCurrentHealth() < 1)
        {

            print("THE CAR IS FUCKING DEAD OMG OMG ");
        }
        */
    }


}
