using UnityEngine;
using System.Collections;

public class Punch : MonoBehaviour {

    public GameObject Monocle;
    public GameObject Projectile;

    public bool ObjectInFront;
    public bool ProjectileInFront;

    private GameObject Block;

    public float ProjectileSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Punch" + GetComponentInParent<Char>().PlayerNumber))
        {
            Monocle.GetComponent<Animator>().SetTrigger("Action");

            if(GetComponentInParent<Char>().I_AM_HOLDING_A_BLOCK)
            {
                Destroy(GetComponentInParent<Char>().BLOCK_THAT_I_AM_HOLDING);
                GetComponentInParent<Char>().I_AM_HOLDING_A_BLOCK = false;
                if (!this.ObjectInFront)
                {
                    GameObject projectile = Instantiate(Projectile);
                    projectile.transform.position = this.transform.position;


                    if (GetComponentInParent<Char>().FacingRight)
                    {
                        projectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(ProjectileSpeed, 0));
                        projectile.transform.localScale = new Vector3(-0.5f, 0.5f, 1);
                    }
                    else
                    {
                        projectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(-ProjectileSpeed, 0));
                    }
                }
            }

            if (this.ObjectInFront)
            {
                float multiplier = 1.0f;
                if (this.ProjectileInFront)
                {
                    multiplier = 4f;
                }

                GameObject projectile = Instantiate(Projectile);
                projectile.transform.position = Block.transform.position;
             
                Destroy(Block);
                this.ProjectileInFront = false;
                this.ObjectInFront = false;


                if (GetComponentInParent<Char>().FacingRight)
                {
                    projectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(ProjectileSpeed*multiplier, 0));
                    projectile.transform.localScale = new Vector3(-0.5f,0.5f,1);
                }
                else
                {
                    projectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(-ProjectileSpeed*multiplier, 0));
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Floor" || coll.gameObject.tag == "Projectile")
        {
            if(coll.gameObject.tag == "Projectile")
            {
                this.ProjectileInFront = true;
                Debug.Log("projectile");
            }
            this.ObjectInFront = true;
            Block = coll.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Floor" || coll.gameObject.tag == "Projectile")
        {
            if (coll.gameObject.tag == "Projectile")
            {
                this.ProjectileInFront = false;
            }
            this.ObjectInFront = false;
        }
    }


}
