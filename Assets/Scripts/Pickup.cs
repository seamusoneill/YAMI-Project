using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//Script attached to all pickup objects.
//Equips item upon button press from player disabling torch.
//If player already has a pickup it drops the other one. 

public class Pickup : PlayerAction
{

    public Sprite State1;
    public Sprite State2;
    public bool destroyOnPickup;
    [SyncVar(hook = "Destroy")]
    bool Used;


    public AudioClip pickupSound;
    AudioSource source;



    public enum PickupType
    {
        Null,
        Axe,
        Plank
    }

    public PickupType pickupType = PickupType.Null;

    void Start()
    {

        source = GetComponent<AudioSource>();

        gameObject.GetComponent<SpriteRenderer>().sprite = State1;
        if (pickupType == PickupType.Null && !gameObject.CompareTag("Player"))
        {
            Debug.Log("This pickup does not have a type " + gameObject.ToString());
        }
        source = GetComponent<AudioSource>();
    }

    public override void OnChangeState(bool act)
    {





        if ((pickupType == PickupType.Plank) && (player.GetComponent<Player>().pickupType == PickupType.Plank))
        {
            return;
        }
        else
        {


            player.GetComponent<Player>().pickupType = pickupType; //Player is now holding this pickup
                                                                   /*
                                                                   if (!destroyOnPickup)
                                                                   {
                                                                       gameObject.GetComponent<SpriteRenderer>().sprite = State2;
                                                                       this.enabled = false;
                                                                       this.gameObject.tag = "Untagged";
                                                                   }
                                                                   else
                                                                   {
                                                                       this.enabled = false;
                                                                       this.gameObject.tag = "Untagged";
                                                                       this.gameObject.SetActive(false);

                                                                   }
                                                                          */
            Disable();
                                                            

        }



    }

    [ServerCallback]
    private void Disable()
    {
        RpcDisable();
    }

    [ClientRpc]
    private void RpcDisable()
    {
        if(source != null)
        {

            if (pickupSound != null)
            {
                source.PlayOneShot(pickupSound);

            }
        }


        Used = true;

    }

    void Destroy(bool useless)
    {
        if (!destroyOnPickup)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = State2;
            this.enabled = false;
            this.gameObject.tag = "Untagged";
            player.GetComponent<Player>().usable.Remove(this.gameObject);
        }
        else
        {

            this.gameObject.SetActive(false);
            this.gameObject.tag = "Untagged";
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
            player.GetComponent<Player>().usable.Remove(this.gameObject);
        }

    }

}
