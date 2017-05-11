using UnityEngine;
using System.Collections;

public class CarWheel : MonoBehaviour {

    CarPush push_script;

	// Use this for initialization
	void Start () {
        push_script = transform.parent.GetComponent<CarPush>();
	}
	
	// Update is called once per frame
	void Update () {
        if (push_script != null)
        {
            transform.Rotate(0, 0, -push_script.getSpeed());
        }
	}
}
