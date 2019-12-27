using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{

  public GameObject Block;
  public float KnockBack;

  public GameObject Projectil;

  // Use this for initialization
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {

  }

  void OnTriggerEnter2D(Collider2D coll)
  {
    if (coll.gameObject.tag == "Floor" || coll.gameObject.tag == "Projectile")
    {
      GameObject block = Instantiate(Block);
      int Direction = 0;

      if (GetComponentInParent<Rigidbody2D>().velocity.x < 0)
        Direction = 1;
      else
        Direction = -1;

      block.transform.position = coll.transform.position + new Vector3(Direction, 0, 0);

      Destroy(this.transform.parent.gameObject);
    }
    else if (coll.gameObject.tag == "Player")
    {
      int Direction = 0;

      if (GetComponentInParent<Rigidbody2D>().velocity.x < 0)
        Direction = -1;
      else
        Direction = 1;

      coll.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(KnockBack * Direction, 15), ForceMode2D.Impulse);

      Vector3 pos = coll.gameObject.transform.position;
      coll.gameObject.transform.position = new Vector3(pos.x, pos.y + 0.5f, 0);

      Destroy(this.Projectil);
    }
  }

  //void OnTriggerExit2D(Collider2D coll)
  //{
  //    if (coll.gameObject.tag == "Floor" || coll.gameObject.tag == "Projectile")
  //    {

  //    }
  //}
}
