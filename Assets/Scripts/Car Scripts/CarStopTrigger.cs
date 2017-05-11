using UnityEngine;
using System.Collections;

public class CarStopTrigger : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy") || other.CompareTag("Block"))
        {
            GetComponentInParent<CarPush>().CarStuck();

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
    }
}