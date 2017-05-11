using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Trigger : NetworkBehaviour
{

    public List<GameObject> Targets;
    public bool CarTriggerOnly;
    [SyncVar]
    public bool Isused;
    

    


    void Start () {
        
    

        GetComponent<SpriteRenderer>().enabled = false;
        Isused = false;
    }


   
    void OnTriggerEnter2D(Collider2D other)
    {
       if (!Isused)
        {

      
        if ( (other.CompareTag("Player") || other.gameObject.CompareTag("Voiture")) )
        {
           
        
            if (CarTriggerOnly)
            {

                if ( ! other.CompareTag("Voiture"))
                    {
                    return;
                     }
            }
           foreach(GameObject Tar in Targets)
            {
                Tar.GetComponent<Action>().Use();
                }
                Isused = true;
                Destroy(this);

                    
                    
        }
        }






    }




    // Update is called once per frame
    void Update () {
	
	}
}
