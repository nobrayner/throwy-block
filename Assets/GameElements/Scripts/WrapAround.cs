using UnityEngine;
using System.Collections;

public class WrapAround : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(Mathf.Abs(this.transform.position.x) > 14)
        {
            this.transform.position = new Vector3(-this.transform.position.x, this.transform.position.y, 1);
            
        }
	}
}
