using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LightFollowMouse : NetworkBehaviour
{
    

	public float lightZ;
	public float distanceFromSource;
    private Light _lightSource;

    public float MaxReduction;
    public float MaxIncrease;
    public float RateDamping;
    public float Strength;
    public bool StopFlickering;
    private float _baseIntensity;
    private bool _flickering;

	bool Ison;

    // Use this for initialization
    void Start () {
        Ison = true;
        _lightSource = GetComponent<Light>();
    }

	// Update is called once per frame
	void FixedUpdate() {
    

		if (this.GetComponentInParent<Player> ().isLocalPlayer) {
        
			//transform.LookAt(GetWorldPositionOnPlane(Input.mousePosition, transform.position.z));
			Vector3 mouse = GetWorldPositionOnPlane (Input.mousePosition, transform.position.z);
			Vector3 point = new Vector3 (transform.position.x, transform.position.y, 0);
			float angle = Mathf.Atan ((mouse.y - point.y) / (float)(mouse.x - point.x)) * Mathf.Rad2Deg;

			if (mouse.x - point.x < 0)
				angle += 180;

		
			GetComponentInParent<Animator> ().SetFloat ("PlayerRotation", angle); //This line rotates the sprite

			transform.eulerAngles = new Vector3 (0, 90, 0);
			transform.Rotate (0, 0, angle, Space.World);
		
		}
    }

	public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z) {
     Ray ray = Camera.main.ScreenPointToRay(screenPosition);
     Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
     float distance;
     xy.Raycast(ray, out distance);
     return ray.GetPoint(distance);
 }



    public void SwitchLight()
    {
        _lightSource.enabled = !_lightSource.enabled;
    }

    public void ActualiseDraw()
    {
        _lightSource.enabled = Ison;
    }
     
      






}



