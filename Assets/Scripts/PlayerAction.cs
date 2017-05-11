using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public abstract class PlayerAction : NetworkBehaviour
{
    [SyncVar(hook = "OnChangeState")]
    public bool activated = false;

	public GameObject player;

    public void OnTriggerEnter2D(Collider2D objet)
    {
        if (objet.CompareTag("Player"))
        {
            if (!objet.gameObject.GetComponent<Player>().usable.Contains(this.gameObject))
                objet.gameObject.GetComponent<Player>().usable.Add(this.gameObject);
        }
    }

    public void OnTriggerExit2D(Collider2D objet)
    {
        if (objet.CompareTag("Player"))
        {
            objet.gameObject.GetComponent<Player>().usable.Remove(this.gameObject);
        }
    }

    abstract public void OnChangeState(bool act);

    virtual public void Use(GameObject target, bool OnOff)
    {
		player = target;
        activated = OnOff;
    }




}
