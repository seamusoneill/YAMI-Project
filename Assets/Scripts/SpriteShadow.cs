using UnityEngine;
using System.Collections;

public class SpriteShadow : MonoBehaviour {

	public UnityEngine.Rendering.ShadowCastingMode castShadows;
	public bool receiveShadows;
    SpriteRenderer rend;


	// Use this for initialization
	void Start () {

		GetComponent<Renderer>().shadowCastingMode = castShadows;
		GetComponent<Renderer>().receiveShadows = receiveShadows;
        rend = GetComponent<SpriteRenderer>();
        rend.sortingOrder = (int)(transform.position.y*10000);
    }

	// Update is called once per frame
	void FixedUpdate () {
        
	}
}
