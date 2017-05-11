
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ActiverVoiture : PlayerAction
{


    // Use this for initialization
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnChangeState(bool act)
    {
 
        this.GetComponentInParent<CarPush>().TriggerPush(act);
    }

     public override void Use(GameObject target, bool OnOff)
    {    
        if(target.CompareTag("Player"))
        {
            print("UEHUEHUEHUEH");
            activated = OnOff;
        }
  
    }
}

