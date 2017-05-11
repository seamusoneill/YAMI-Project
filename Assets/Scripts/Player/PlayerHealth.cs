using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerHealth : Health {

    AudioSource playerSource;
    Player player;

    public AudioClip hit1;
    public AudioClip hit2;
    public AudioClip hit3;

    // Use this for initialization
    public override void Start () {
        base.Start();
        playerSource = GetComponent<AudioSource>();
        player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    [ClientRpc]
    public override void RpctakeDamage()
    {
        int r = Random.Range(0, 2);
        switch (r)
        {
            case 0:
                Camera.main.GetComponent<AudioSource>().PlayOneShot(hit1);
                break;
            case 1:
                Camera.main.GetComponent<AudioSource>().PlayOneShot(hit1);
                break;
            case 2:
                Camera.main.GetComponent<AudioSource>().PlayOneShot(hit1);
                break;
        }

    }

}
