using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

   
	public bool followPlayer = true;

	public GameObject player;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

        if (player != null)
        {

      
            if (followPlayer){
			transform.position = new Vector3(player.transform.position.x,
							player.transform.position.y+transform.position.z*0.7f,
							 transform.position.z);
		}
        }
       
    }


    public void SetPlayer(GameObject play)
    {
        player = play;
    }





}
