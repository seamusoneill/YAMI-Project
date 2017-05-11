using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

//Script for obstacles. Clears the obstacle when the correct pickup is used and stops the car when it collides.

public class Obstacle : PlayerAction
{

    public Pickup.PickupType obstacleSolution = Pickup.PickupType.Null;
    public AudioClip useAxe;
    public AudioClip usePlank;
    public List<Sprite> SpritesToDraw;
    AudioSource source;
    //The pickup required to bypass this obstacle. Must be set from the editor.

    [SyncVar(hook = "UpdateObstacle")]
    public int health;

    void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = SpritesToDraw[SpritesToDraw.Count - 1];
        if (isServer)
        {
            NetworkServer.Spawn(this.gameObject);


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
        if (act == true)
        {
            if (player)
            {



                if (player.GetComponent<Player>().pickupType == obstacleSolution)
                {
                    if (obstacleSolution == Pickup.PickupType.Plank)
                    {
                        player.GetComponent<Player>().pickupType = Pickup.PickupType.Null;
                        print("OK TA PU DE PLANCHE");

                    }
                    Trigger();


                }
            }
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
        {
            source.PlayOneShot(useAxe);
            if (health > 0)
            {

                health -= 1;
            }
        }
        else
        {
            source.PlayOneShot(usePlank);
            if (health > 0)
            {

                health -= 1;
            }

        }

        this.GetComponent<SpriteRenderer>().sprite = SpritesToDraw[health];





    }




    public void UpdateObstacle(int health)
    {
        // object not properly uninstantiated on the client
        if (health <= 0)
        {
            this.GetComponent<SpriteRenderer>().sprite = SpritesToDraw[0];
            if (obstacleSolution == Pickup.PickupType.Axe)
            {
                player.GetComponent<Player>().usable.Remove(this.gameObject);

                GetComponent<BoxCollider2D>().enabled = false;
                GetComponentInChildren<CircleCollider2D>().enabled = false;
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
                this.gameObject.SetActive(false);

            }
            else
            {
                GetComponent<BoxCollider2D>().enabled = false;

                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
				GetComponent<EdgeCollider2D> ().enabled = false;
                GetComponentInChildren<CircleCollider2D>().enabled = false;
                this.GetComponent<SpriteRenderer>().sprite = SpritesToDraw[0];
                player.GetComponent<Player>().usable.Remove(this.gameObject);

            }


        }
    }
}

			

