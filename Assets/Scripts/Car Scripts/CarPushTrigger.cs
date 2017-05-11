using UnityEngine;
using System.Collections;

//Script used to identify when a player is standing behind the car
//and moving in a direction towards it, triggering the push.

public class CarPushTrigger : MonoBehaviour {



	void Start()
	{
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
        print(other);
	}

	void OnTriggerStay2D(Collider2D other)
	{


        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().CmdForceSwitchLight(false);
        }
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag ("Player")) {
            GetComponentInParent<CarPush>().TriggerPush(false);

            other.GetComponent<Player>().CmdForceSwitchLight(true); 
		}
	}
}
