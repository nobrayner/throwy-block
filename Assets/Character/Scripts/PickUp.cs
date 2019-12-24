using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

    public bool Colliding;
    public GameObject Block;

    private bool entered_this_frame = false;
    
    void Update()
    {
        entered_this_frame = false;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Floor" || coll.gameObject.tag == "Projectile")
        {
            this.Colliding = true;
            Block = coll.gameObject;
            entered_this_frame = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Floor" || coll.gameObject.tag == "Projectile" && !entered_this_frame)
        {
            if (!GetComponentInParent<Char>().TouchingGround)
            {
                this.Colliding = false;
            }
        }
    }
}
