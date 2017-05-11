using UnityEngine;
using System.Collections;

public class PollygonFollowMouse : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (this.GetComponentInParent<Player>().isLocalPlayer)
        {

            Vector3 mouse = GetWorldPositionOnPlane(Input.mousePosition, transform.position.z);
            Vector3 point = new Vector3(transform.position.x, transform.position.y, 0);
            float angle = Mathf.Atan((mouse.y - point.y) / (mouse.x - point.x)) * Mathf.Rad2Deg;
            if (mouse.x - point.x < 0) angle += 180;
            transform.localEulerAngles = new Vector3(45, 0, 0);
            transform.Rotate(0, 0, angle, Space.World);

        }

    }

    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
}
