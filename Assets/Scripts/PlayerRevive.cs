using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerRevive : PlayerAction {

    [SyncVar]
    float reviveTimer;


    bool reviving = false;
    public float reviveTime;
    public RectTransform ReviveBar;
    GameObject localplayer;

    AudioSource audioSource;
    public AudioClip revivingSound;
    public AudioClip revivedSound;

    public override void OnChangeState(bool act)
    {
        if (act)
        {
            reviveTimer = 0;
           
            reviving = true;
            if(isServer)
                RpcPlayReviving(); 
        }
        else
        {
            if (isServer)
                RpcStopReviving();
            reviving = false;
        }
    }

    new void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.CompareTag("Player") && (other != localplayer))
        {
            reviving = false;
        }
    }


    // Use this for initialization
    void Start () {
        localplayer = this.transform.parent.gameObject;
        audioSource = localplayer.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    [ServerCallback]
	void Update () {
        if (reviving)
        {
            reviveTimer += Time.deltaTime;
            if (reviveTimer > reviveTime)
            {
                localplayer.GetComponent<Player>().playerState = 0;
                localplayer.GetComponent<Health>().restoreHealth(0.5f);
                RpcPlayRevived();
                reviveTimer = 0;
                activated = false;
            }
        }
	}

    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }

    [ClientRpc]
    void RpcPlayReviving()
    {
        audioSource.clip = revivingSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    [ClientRpc]
    void RpcStopReviving()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
    [ClientRpc]
    void RpcPlayRevived()
    {
        audioSource.PlayOneShot(revivedSound);
    }
}
