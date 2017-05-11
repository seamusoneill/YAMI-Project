using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
	public int maxHealth;
    [SyncVar(hook = "healthChanged")]
	int currentHealth;
	// Use this for initialization
	public virtual void Start () {
		currentHealth = maxHealth;
	}

	// Update is called once per frame
	void Update () {

	}

	public int getCurrentHealth(){
		return currentHealth;
	}

    public void restoreHealth(float percent)
    {
        currentHealth = (int)(maxHealth * percent);
    }

	public virtual void getDamage(int amount){
		currentHealth-=amount;
        if (isServer)
        {
            RpctakeDamage();
        }
    }

	public void restoreHealth(int amount = 0){
		if(amount>0)
			currentHealth+=amount;
		if(currentHealth>maxHealth || amount==0)
			currentHealth = maxHealth;
	}

    [ClientRpc]
    public virtual void RpctakeDamage()
    {

    }

    public void healthChanged(int h)
    {

        
        currentHealth = h;
    }
}
