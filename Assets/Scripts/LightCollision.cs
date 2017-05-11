using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightCollision : MonoBehaviour {

    List<GameObject> currentCollisions = new List<GameObject>();

	public AudioClip lampetorcheAudioClip;
    AudioSource lampeTorcheAttirance;




    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !other.isTrigger)
        {
          
            currentCollisions.Remove(other.gameObject);
            other.GetComponent<Enemy0>().OnUnlit(this.transform.parent.gameObject);
        }
    }

  public  void TurnOffCollision()
    {
        foreach(GameObject Ennemies in currentCollisions)
        {

            if (Ennemies) { 

            Ennemies.GetComponent<Enemy0>().OnUnlit(this.transform.parent.gameObject);
            }



        }
        currentCollisions.Clear();
     
    }
		

    // Use this for initialization
    void Start () {
        lampeTorcheAttirance = GetComponent<AudioSource>();
    
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Enemy") && !other.isTrigger)
        {
            currentCollisions.Add(other.gameObject);

            other.GetComponent<Enemy0>().OnLit(this.transform.parent.gameObject);

            //Audio

            /*if (!lampeTorcheAttirance.isPlaying)
            {
                lampeTorcheAttirance.PlayOneShot(lampeTorcheAttirance.clip, 1);
            }*/

        }
    }

}
