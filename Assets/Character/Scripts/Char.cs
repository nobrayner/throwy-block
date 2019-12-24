using UnityEngine;
using System.Collections;

public class Char : MonoBehaviour
{
  public int MoveSpeed = 5;
  public int JumpHeight = 10;

  public GameObject Monocle;

  public int PlayerNumber;

  private Rigidbody2D Rigidbody;
  private Animator Animator;

  [HideInInspector]
  public bool TouchingGround;
  private bool DoubleJump;

  public bool FacingRight;
  private bool Moving;

  public GameObject collideBelow;
  public GameObject collideAbove;

  public bool I_AM_HOLDING_A_BLOCK;
  public GameObject BLOCK_THAT_I_AM_HOLDING;

  public GameObject block_gameobject;

  // Use this for initialization
  void Start()
  {
    this.Rigidbody = GetComponent<Rigidbody2D>();
    this.Animator = GetComponent<Animator>();
    this.DoubleJump = true;

    this.FacingRight = this.Animator.GetBool("FacingRight");
    Moving = false;
  }

  // Update is called once per frame
  void Update()
  {
    float jump = (this.Rigidbody.mass * 10) * this.JumpHeight;

    if (Input.GetButtonDown("Jump" + this.PlayerNumber) && this.TouchingGround)
    {
      this.Rigidbody.AddForce(new Vector2(this.Rigidbody.velocity.x, jump));
      this.TouchingGround = false;
    }
    else if (Input.GetButtonDown("Jump" + this.PlayerNumber) && this.DoubleJump)
    {
      this.Rigidbody.velocity = new Vector2(this.Rigidbody.velocity.x, 0);
      this.Rigidbody.AddForce(new Vector2(this.Rigidbody.velocity.x, jump * 1.15f));
      this.DoubleJump = false;
    }

    if (Input.GetButtonDown("PickUp" + this.PlayerNumber))
    {
      if (!I_AM_HOLDING_A_BLOCK)
      {
        if (collideAbove.GetComponent<PickUp>().Colliding)
        {
          collideAbove.GetComponent<PickUp>().Colliding = false;
          collideBelow.GetComponent<PickUp>().Colliding = false;

          Destroy(collideAbove.GetComponent<PickUp>().Block);
          I_AM_HOLDING_A_BLOCK = true;
        }
        else if (collideBelow.GetComponent<PickUp>().Colliding)
        {
          collideAbove.GetComponent<PickUp>().Colliding = false;
          collideBelow.GetComponent<PickUp>().Colliding = false;
          Destroy(collideBelow.GetComponent<PickUp>().Block);
          I_AM_HOLDING_A_BLOCK = true;
        }

        if (I_AM_HOLDING_A_BLOCK)
        {
          GameObject block = Instantiate(block_gameobject);
          block.transform.parent = this.transform;
          block.transform.transform.position = this.transform.position + new Vector3(0, 0.9f, 0);
          block.transform.localScale = new Vector3(0.5f, 0.5f, 1);

          BLOCK_THAT_I_AM_HOLDING = block;
        }
      }
    }

    if (Input.GetAxis("Horizontal" + this.PlayerNumber) < 0)
    {
      this.FacingRight = false;
      this.Moving = true;
    }
    else if (Input.GetAxis("Horizontal" + this.PlayerNumber) > 0)
    {
      this.FacingRight = true;
      this.Moving = true;
    }
    else
    {
      this.Moving = false;
    }

    this.Animator.SetBool("FacingRight", this.FacingRight);
    Monocle.GetComponent<Animator>().SetBool("FacingRight", this.FacingRight);
    this.Animator.SetBool("Moving", this.Moving);


    this.Rigidbody.AddForce(new Vector2(Input.GetAxis("Horizontal" + this.PlayerNumber) * (MoveSpeed / 100f), 0), ForceMode2D.Impulse);
  }

  void OnCollisionEnter2D(Collision2D coll)
  {
    if (coll.gameObject.tag == "Floor" && (this.transform.position.y > coll.transform.position.y))
    {
      this.TouchingGround = true;
      this.DoubleJump = true;
    }
  }
}