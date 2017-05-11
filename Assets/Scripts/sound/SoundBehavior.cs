using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundBehavior : MonoBehaviour {


    AudioSource Emitter;
    public List<AudioClip> Clips;
 
    public bool Loop;
    private bool Hasfinished;
    private int Track;

	// Use this for initialization
	void Start () {
    Emitter =  GetComponent<AudioSource>();
        Track = 0;
        Hasfinished = false;

    }
	
	// Update is called once per frame
	void FixedUpdate () {


        if ( !Emitter.isPlaying)
        {
   
            if (Track >= ( Clips.Count) )
            {

                if (Loop == false)
                {
                    Destroy(this.gameObject);
                    return;

                }
                else
                {
                    Track = 0;
                }
           }
                Emitter.PlayOneShot(Clips[Track], 1);
                ++Track;

            
    
        }
  







	
	}
}
