using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class CarPush : NetworkBehaviour
{

	
   public List<Transform> path = new List<Transform>();


    public float MaximumSpeed = 10.0f;
    public float Acceleration ;
    public float Deceleration = 8.0f;

    
    public float TravelTime;
	
    [SyncVar]
    private float speed = 0.0f;
    public int currentPoint = 0;
    public int NumberOfPlayer = 0;
	public  bool isPushed = false;
	Animator anim;

	AudioSource SoundCar;

	// Use this for initialization
	void Start () {
        if (isServer)
        {  
            NetworkServer.Spawn(this.gameObject);
			
        }
        SoundCar = GetComponent<AudioSource>();


        //anim = GameObject.FindGameObjectWithTag ("Canvas").GetComponent<Animator> ();
    }
	
	// Update is called once per frame
    [ServerCallback]
	void FixedUpdate () {
        if(NumberOfPlayer == 0)
        {
            isPushed = false;
        }

        if (isPushed == true && speed < MaximumSpeed)
        {
           
                speed = speed + ( Acceleration * Time.fixedDeltaTime * NumberOfPlayer);
        }
        if (isPushed == false && speed > 0)
        {

            speed = speed -   (Deceleration * Time.fixedDeltaTime);

        }


        float dist = Vector3.Distance(path[currentPoint].position, transform.position);
        if (dist < 1)
        {
       
           currentPoint += 1;
    


        }
		if (currentPoint >= path.Count)
        {
            return;
		}

        transform.position = Vector3.MoveTowards(transform.position, path[currentPoint].position,  Time.fixedDeltaTime * speed);
    }

    void OnDrawGizmos()
	{
		for (int i = 0; i < path.Count; ++i) {
			if (path [i] != null) {
				Gizmos.DrawSphere (path [i].position, 1.0f);
			}
		}
	}

    public void TriggerPush(bool Activated)
    {
    

        if(Activated == true)
        {
            print("ON");
            isPushed = Activated;
            ++NumberOfPlayer;
			//Car pushed Sound, (sound already set on loop in the editor) Stops when players are away
			SoundCar.Play();

        }
        else
        {
            if (NumberOfPlayer > 0)
                 --NumberOfPlayer;
                  if (NumberOfPlayer == 0 && SoundCar.isPlaying)
                   {
                SoundCar.Stop();
                  }
	
                
			
        }

  
      

    }


    public void CarStuck()
    {
        speed = 0;
    }

    public float getSpeed()
    {
        return speed;
    }
}
