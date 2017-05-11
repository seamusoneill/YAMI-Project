using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{

	private Rigidbody2D m_Rigidbody2D;
	public float m_MaxSpeed;
    public GameObject MainCamera;
    public RuntimeAnimatorController Player2;
    
    public float reanimTime;


    // Références aux affichages
    private GameObject Canvas;
    Canvas Dead;
    Canvas OtherDead;
    Canvas CarDead;




    public List<GameObject> usable = new List<GameObject>();
    public List<AudioClip> dyingSound;
    public AudioClip revivingSound;
    public AudioClip revivedSound;
    public AudioClip gameOver;
    public bool CanActivate;
    
    public AudioClip heartbeat;
    static AudioSource source = null;

    private float reanimTimer;
  
    float lastTimeSinceActivation;

	public Pickup.PickupType pickupType;

    Health health;
    [SyncVar(hook = "OnStateChange")]
	public int playerState = 0;

    [SyncVar(hook = "OnSwitchLight")]
    public bool lightOn = true;

    PlayerRevive revive;

    public void OnTriggerEnter2D(Collider2D objet)
    {
		if (objet.CompareTag ("Activable")) {
			if (!usable.Contains (objet.gameObject)){
				usable.Add (objet.gameObject);
			GameObject.FindGameObjectWithTag ("PromptManager").GetComponent<Prompt> ().PlayPrompt (objet.gameObject.transform.position);
		}
        }
    }

    public void OnTriggerExit2D(Collider2D objet)
    {

        if (objet.CompareTag("Activable"))
        {

            usable.Remove(objet.gameObject);
			GameObject.FindGameObjectWithTag ("PromptManager").GetComponent<Prompt> ().StopPrompt ();
        }
        
    }

	// Use this for initialization
	void Start () {
        
       
        pickupType = Pickup.PickupType.Null;
        CanActivate = true;
        Canvas = GameObject.FindGameObjectsWithTag("MainCanvas")[0];

        
        
        lastTimeSinceActivation = Time.time;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
		health = GetComponent<PlayerHealth>();
        MainCamera = Camera.main.gameObject;
        if(source == null || MainCamera != source.gameObject)
            source = MainCamera.AddComponent<AudioSource>();
        CarDead = Canvas.transform.FindChild("CarDestroyed").gameObject.GetComponent<Canvas>();
        Dead = Canvas.transform.FindChild("Died").gameObject.GetComponent<Canvas>();
        OtherDead = Canvas.transform.FindChild("OtherDied").gameObject.GetComponent<Canvas>();
        OnSwitchLight(lightOn);
        if (isLocalPlayer)
        {
            if (FindObjectsOfType<Player>().Length > 1)
            {
               this.gameObject.GetComponent<Animator>().runtimeAnimatorController = Player2;
                CmdSwitchAnimation();
            }

            Camera.main.gameObject.GetComponent<CameraScript>().SetPlayer(this.gameObject);
        }
        revive = GetComponentInChildren<PlayerRevive>();
        revive.gameObject.SetActive(false);
	}

    
    void Update()
    {
        print(pickupType);
        if (!isLocalPlayer)
        {
            return;
        }


        if (Input.GetButtonDown("Fire1"))
        {
            CmdSwitchLight();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            CmdSwitchLight();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			m_MaxSpeed = m_MaxSpeed * 1.5F;
		}
    
        if (Input.GetKeyDown(KeyCode.E) && (Time.time - lastTimeSinceActivation > 0.5F ))
        {
            if (usable != null)
            {
                CmdActiver(true);
                lastTimeSinceActivation = Time.time;
            }
        }
        if (Input.GetKeyUp(KeyCode.E) )
        {
            if (usable != null && CanActivate)
            {
                CmdActiver(false);
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            m_MaxSpeed = 10;
        }


    }

    [Command]
    void CmdActiver(bool OnOff)
    {
        if (usable != null)

            foreach (GameObject ObjectToUse in usable)
				ObjectToUse.GetComponent<PlayerAction>().Use(this.gameObject, OnOff);
		if (OnOff)
			GameObject.FindGameObjectWithTag ("PromptManager").GetComponent<Prompt> ().StopPrompt ();
    }

    [Command]
    void CmdSwitchLight()
    {
        lightOn = !lightOn;
    }

    [Command]
    public void CmdForceSwitchLight(bool OnOff)
    {
        lightOn = OnOff;
    }

    [Command]
    public void CmdSwitchAnimation()
    {
        RpcSwitchAnimation();
    }

    [ClientRpc]
    public void RpcSwitchAnimation()
    {
        this.gameObject.GetComponent<Animator>().runtimeAnimatorController = Player2;
    }




    void OnSwitchLight(bool light)
    {
        GetComponentInChildren<Light>().enabled = light;
        GetComponentInChildren<LightCollision>().TurnOffCollision();
        GetComponentInChildren<PolygonCollider2D>().enabled = light;
    }

    void OnStateChange(int state)
    {
        playerState = state;
        GetComponent<Animator>().SetInteger("PlayerState", playerState);
        if(playerState == 0)
        {
            CanActivate = true;
            if (isServer)
            {
                RpcStopHeartbeat();
            }
            m_Rigidbody2D.isKinematic = false;
            revive.SetActive(false);
        }
        if(playerState == 1)
        {
            m_Rigidbody2D.velocity = new Vector2(0, 0);
            m_Rigidbody2D.isKinematic = true;
            CanActivate = false;
            if (isServer)
            {
                RpcLaunchSound();
            }
            revive.SetActive(true);
        }
        if (playerState == 2)
        {
            this.gameObject.SetActive(false);
            RpcStopHeartbeat();
            if (isLocalPlayer)
            {
               // Camera.main.enabled = false;
                Dead.enabled = true;
                Dead.GetComponent<Canvas>().enabled = true;
                Cursor.visible = true;
                if(isServer)
                    RpcGameOver();

            }
            else
            {
                GameObject OtherPlayer = null;
                foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player")){

                    OtherPlayer = cur;
                }

				OtherPlayer.SetActive(false);
		
                OtherDead.enabled = true;
                OtherDead.GetComponent<Canvas>().enabled = true;
                Cursor.visible = true;
                if (isServer)
                    RpcGameOver();

            }
        }
    }
    

    // Update is called once per frame
    
    void FixedUpdate () {
        if (isServer)
        {
            reanimTimer += Time.deltaTime;
            if (playerState==0 && health.getCurrentHealth() <= 0)
            {
                reanimTimer = 0;
                playerState =1;
          
            }
            if(playerState==1 && reanimTimer > reanimTime)
            {
                playerState = 2;
            }
        }

        if (!isLocalPlayer)
        {
            return;
        }

        if (playerState == 0)
        {
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");

            //if(!isDead)
            m_Rigidbody2D.velocity = new Vector3(hor * m_MaxSpeed, ver * m_MaxSpeed, 0);
        }
	}

    [ClientRpc]
    void RpcLaunchSound()
    {
        
        source.clip = heartbeat;
        source.loop = true;
        source.Play();
        if (dyingSound.Count == 0)
            return;
        int chosen = Random.Range(0, dyingSound.Count-1);
        source.PlayOneShot(dyingSound[chosen]);
    }

    [ClientRpc]
    void RpcStopHeartbeat()
    {
        if (source.isPlaying)
            source.Stop();
    }

    public void CarDestroyed()
    {
        if (isLocalPlayer)
        {
            this.gameObject.SetActive(false);
       
            CarDead.enabled = true;
            CarDead.GetComponent<Canvas>().enabled = true;
            Cursor.visible = true;
            if (isServer)
                RpcGameOver();
        }


    }

    [ClientRpc]
    void RpcGameOver()
    {
        source.Stop();
        source.PlayOneShot(gameOver);
    }

}
