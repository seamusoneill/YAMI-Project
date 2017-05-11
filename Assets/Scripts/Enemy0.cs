using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Enemy0 : NetworkBehaviour
{
    
    GameObject player;
    AudioSource Sound;
    AudioSource loopSound;
    public List<AudioClip> cris;
    public AudioClip monstreEclaire;
    public AudioClip monstreEclaire2;
    public AudioClip monstreMort;
    public AudioClip monstreAttire;

    public float speed;
    public float SpeedAugmntationFactor;
    bool playerInRange = false;
    float attackTimer = 0;
    public int damage;
    public float timeBetweenAttack = 1;
    public float expositionTimer = 1;
    [SyncVar]
    int lighten = 0;
    [SyncVar]
    bool isDead = false;
    float lightenTimer = 0;
    float changeTargetTimer = 0;

    float pathTimer = 0;
    PathFinding path;
    Vector2 target;

    Rigidbody2D rigid;

    bool screamed = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            print("in range");
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            playerInRange = false;
        }
    }

    // Use this for initialization
    void Start()
    {
        Sound = gameObject.AddComponent<AudioSource>();
        loopSound = gameObject.AddComponent<AudioSource>();
        player = null;
        rigid = GetComponent<Rigidbody2D>();
        path = GetComponent<PathFinding>();
        
    }


    // Update is called once per frame
    [Server]
    void FixedUpdate()
    {
        if (!isServer)
            return;

        if (!screamed)
        {
            RpcPlayScream();
            screamed = true;
        }

        if (isDead)
        {
            if (!Sound.isPlaying)
                Rpckill();
            return;
        }

        attackTimer += Time.deltaTime;
        lightenTimer += Time.deltaTime;
        changeTargetTimer += Time.deltaTime;
        pathTimer += Time.deltaTime;
		if (player != null && !playerInRange) {
            if (pathTimer > 0.5f)
            {
                target = path.getNearest(this.gameObject, player.gameObject);
                target -= new Vector2(this.transform.position.x, this.transform.position.y);
                target.Normalize();
                pathTimer = 0;
            }
            rigid.velocity = target * speed * Time.deltaTime * 50 ;

			//Rotate enemy sprite to face player. If there is a target variable added just change player to target and it will work fine
			float angle = Mathf.Atan((transform.position.y - player.transform.position.y)/(float)(transform.position.x - player.transform.position.x)) * Mathf.Rad2Deg;
			if (player.transform.position.x - transform.position.x < 0)
				angle += 180;
			GetComponent<Animator>().SetFloat("EnemyRotation",angle); 
		}
        else if (player != null && attackTimer > timeBetweenAttack)
            Attack();


        if (lighten >= 2 && lightenTimer > expositionTimer)
        {
            //Dies
            print("Dead." + lighten);
            //Destroy(this.gameObject);
            isDead = true;
            RpcPlayDead();
            RpcDies();
           
        }
    }

    void Attack()
    {
        attackTimer = 0;
        player.GetComponent<Health>().getDamage(damage);
    }

    public void OnLit(GameObject target)
    {
		if (target.CompareTag("Player") && target!=player && changeTargetTimer > 2.0)
        {
            if (isServer)
                RpcMonstreAtirre();
            player = target;
            changeTargetTimer = 0;
            playerInRange = false;
            
        }
        print("light++");
        lighten++;
        speed = speed * SpeedAugmntationFactor;

        if (lighten == 2) lightenTimer = 0;

        if (isServer)
        {
            if (lighten == 1)
                RpcPlayLit1();
            if (lighten == 2)
                RpcPlayLit2();
        }
    }

    public void OnUnlit(GameObject target)
    {
        print("light--");
        lighten--;
        speed = 7;

        if (isServer)
        {
            if (lighten == 0)
            {
                RpcStopLit();
            }
            if (lighten == 1)
            {
                RpcPlayLit1();
            }
        }
    }

    public void OnSight(GameObject target)
    {
        if (target.CompareTag("Player") || target.CompareTag("Voiture") )
        {
            if (player != null && player.CompareTag("Voiture"))
                return;
            player = target;
            if(!Sound.isPlaying)
            {
                Sound.PlayOneShot(Sound.clip, 1);

            }
            playerInRange = false;
        }
    }

    public void ExitSight(GameObject target)
    {
        if (target == player) player = null;
    }

    [ClientRpc]
    void Rpckill()
    {
        this.gameObject.SetActive(false);
    }

    [ClientRpc]
    void RpcDies()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    [ClientRpc]
    void RpcPlayScream()
    {
        if (cris.Count == 0)
            return;
        int r = Random.Range(0, cris.Count-1);
        Sound.PlayOneShot(cris[r]);
    }

    [ClientRpc]
    void RpcPlayDead()
    {
        Sound.PlayOneShot(monstreMort);
    }

    [ClientRpc]
    void RpcPlayLit1()
    {
        loopSound.PlayOneShot(monstreEclaire);
    }

    [ClientRpc]
    void RpcPlayLit2()
    {
        loopSound.PlayOneShot(monstreEclaire2);
    }

    [ClientRpc]
    void RpcStopLit()
    {
        loopSound.Stop();
    }

    [ClientRpc]
    void RpcMonstreAtirre()
    {
        Sound.PlayOneShot(monstreAttire);
    }
}
