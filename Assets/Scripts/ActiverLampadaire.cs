using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ActiverLampadaire : PlayerAction {

    
	// Use this for initialization
	void Start () {


    }
	
	// Update is called once per frame
	void Update () {

	}

    public override void OnChangeState(bool act)
    {
        if (act == true)
        {
            this.GetComponentInChildren<Light>().enabled = !this.GetComponentInChildren<Light>().enabled;
            this.GetComponentInChildren<VolumetricLight>().enabled = !this.GetComponentInChildren<VolumetricLight>().enabled;

        }
      
    }
}
