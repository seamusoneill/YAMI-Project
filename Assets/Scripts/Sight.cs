using UnityEngine;
using System.Collections;

public class Sight : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        this.transform.parent.gameObject.GetComponent<Enemy0>().OnSight(other.gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        this.transform.parent.gameObject.GetComponent<Enemy0>().ExitSight(other.gameObject);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
