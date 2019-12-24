using UnityEngine;
using System.Collections;

public class DestroyOnBoundary : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Boundary")
        {
            Destroy(this.gameObject);
        }
    }
}
