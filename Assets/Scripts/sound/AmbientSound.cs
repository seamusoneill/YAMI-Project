using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class AmbientSound : Action
{


    AudioSource Emmiter;
    public List<AudioClip> Sounds;
    public AudioClip Endgame;

    [SyncVar]
    private int rang;

    // Use this for initialization
    void Start()
    {

        Emmiter = GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {

        if (!Emmiter.isPlaying)
        {
            Emmiter.PlayOneShot(Sounds[rang], 1);
        }
    }


    public override void Use()
    {
        CallBackUse();
    }



    [ServerCallback]
    public void CallBackUse()
    {
        RpcUse();

    }
    [ClientRpc]
    void RpcUse()
    {
        if (rang != Sounds.Count)
        {

            rang = rang + 1;
            Emmiter.Stop();
        }
        else
        {
            rang = 0;
        }

    }

    public void Endthegame()
    {
        ServerEndGame();


    }

    [ServerCallback]
    public void ServerEndGame()
    {
        RpcEndGame();

    }

    void RpcEndGame()
    {
        Emmiter.Stop();

        if (!Emmiter.isPlaying)
        {
            Emmiter.PlayOneShot(Endgame, 1);
        }

    }
}
