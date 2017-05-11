using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CarHealth : Health {

    AudioSource source;
    AudioSource source2;
    AudioSource sourceAlarme;
    public AudioClip alarme;
    public AudioClip damaged1;
    public AudioClip damaged2;
    public AudioClip damaged3;
    public AudioClip health1;
    public AudioClip health2;
    public AudioClip health3;
    public AudioClip detruite;
    int state = 0;
    float dmgTimer = 0;
    public float alarmTime;

    // Use this for initialization
    public override void Start () {
        base.Start();
        source = GetComponent<AudioSource>();
        source2 = GetComponent<AudioSource>();
        sourceAlarme = Camera.main.gameObject.AddComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<Animator>().SetInteger("Health", getCurrentHealth());
        dmgTimer += Time.deltaTime;
    }

    [ClientRpc]
    public override void RpctakeDamage()
    {
        if (dmgTimer > alarmTime)
        {
            sourceAlarme.PlayOneShot(alarme);
        }
        dmgTimer = 0;
        int m = Random.Range(0, 2);
        switch (m)
        {
            case 0:
                source.PlayOneShot(damaged1);
                break;
            case 1:
                source.PlayOneShot(damaged2);
                break;
            case 2:
                source.PlayOneShot(damaged3);
                break;
        }
        if (getCurrentHealth() < 75 && state == 0)
        {
            source2.PlayOneShot(health1);
            state++;
        }
        if (getCurrentHealth() < 50 && state == 1)
        {
            source2.PlayOneShot(health1);
            state++;
        }
        if (getCurrentHealth() < 25 && state == 2)
        {
            source2.PlayOneShot(health1);
            state++;
        }
        if (getCurrentHealth() < 0 && state == 3)
        {
            source2.PlayOneShot(detruite);
            state++;
        
            foreach (GameObject cur in GameObject.FindGameObjectsWithTag("Player"))
            {

                cur.GetComponent<Player>().CarDestroyed();

            }





        }

    }
}
