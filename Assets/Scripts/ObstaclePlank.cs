using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//Script for obstacles. Clears the obstacle when the correct pickup is used and stops the car when it collides.

public class ObstaclePlank : PlayerAction
{

    public Pickup.PickupType obstacleSolution = Pickup.PickupType.Null;
    public AudioClip useAxe;
    public AudioClip usePlank;
    AudioSource source;
    //The pickup required to bypass this obstacle. Must be set from the editor.

    [SyncVar]
    public int health;

    void Start()
    {
        if (isServer)
        {
            NetworkServer.Spawn(this.gameObject);
            health = 5;

        }
        source = GetComponent<AudioSource>();

        if (obstacleSolution == Pickup.PickupType.Null)
        {
            Debug.Log("No pickup solution provided for " + gameObject.ToString());
        }
        if (health == '0')
        {
            Debug.Log("This pickup has no health, it will be automatiaclly destroyed" + gameObject.ToString());
        }
    }

   
    public override void OnChangeState(bool act)
    {
        if (player.GetComponent<Player>().pickupType == obstacleSolution)
        {
            Trigger();
        }
    }


    [ServerCallback]
    void Trigger()
    {

        RpcTrigger();
    }

    [ClientRpc]
    void RpcTrigger()
    {

        if (obstacleSolution == Pickup.PickupType.Axe)
            source.PlayOneShot(useAxe);
        else
            source.PlayOneShot(usePlank);
        health -= 1;
        GetComponent<Animator>().SetInteger(obstacleSolution + "Solution", health);

        if (health <= 0)
        {
            GetComponent<Animator>().SetInteger(obstacleSolution + "Solution", health);
            GetComponent<EdgeCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponentInChildren<CircleCollider2D>().enabled = false;
            this.gameObject.SetActive(false);
        }


    }
}


