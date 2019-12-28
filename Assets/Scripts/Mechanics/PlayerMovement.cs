using ThrowyBlock.UnityEditor;
using UnityEngine;

namespace ThrowyBlock.Mechanics {
    public class PlayerMovement : MonoBehaviour {
        public bool showRaycasts = true;
        public bool ControlEnabled = true;

        [Header("Movement Properties")]
        [ReadOnly] public bool IsGrounded;
        [ReadOnly] public bool IsHeadBlocked;
        [ReadOnly] public bool CanDoubleJump;
        public float Speed = 4f;
        public float JumpForce = 18f;
        public float MaxFallSpeed = -25f;
        [ReadOnly] public Vector2 Velocity;


        [Header("Environment Check Properties"), Tooltip("X Offset for Foot Check Raycasts")]
        public float FootOffset = .4f;
        
        [Tooltip("Space needed above the player's head")]
        public float HeadClearance = .5f;
        
        [Tooltip("The distance from the ground that counts as being grounded")]
        public float GroundDistance = .2f;
        public LayerMask GroundLayer;


        [Header("Components")]
        public Rigidbody2D Rigidbody;
        public BoxCollider2D BodyCollider;
        public PlayerInput Input;

        //[Header("Audio")]
        //public AudioClip jumpAudio;
        //public AudioClip respawnAudio;
        //public AudioClip ouchAudio;
        //public AudioSource audioSource;

        //public Health health;
        public Bounds Bounds => BodyCollider.bounds;

        SpriteRenderer SpriteRenderer;
        internal Animator Animator;

        //readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        float originalXScale;                   //Original scale on X axis
        [ReadOnly] public int direction = 1;	//Direction player is facing

        // Animator Parameter Hashes
        int movingParamHash;

        void Start() {
            movingParamHash = Animator.StringToHash("Moving");

            originalXScale = transform.localScale.x;

            //health = GetComponent<Health>();
            //audioSource = GetComponent<AudioSource>();
            Rigidbody = GetComponent<Rigidbody2D>();
            BodyCollider = GetComponent<BoxCollider2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Animator = GetComponent<Animator>();

            Input = GetComponent<PlayerInput>();
        }

        void Update() {
            //animator.SetBool("grounded", IsGrounded);
            //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            Animator.SetBool(movingParamHash, Mathf.Abs(Input.Horizontal) > 0.01f);
        }

        void FixedUpdate() {
            // Physics check first
            PhysicsCheck();

            // Apply Player Input as movements
            GroundMovement();
            JumpMovement();

            Velocity = Rigidbody.velocity;
        }

        void PhysicsCheck() {
            // Assumbe the player isn't grounded, their head is not blocked, and they can't double-jump
            IsGrounded = false;
            IsHeadBlocked = false;
            CanDoubleJump = false;

            // Check Left and Right foot for grounding
            RaycastHit2D leftCheck = Raycast(new Vector2(-FootOffset, -(BodyCollider.size.y / 2)), Vector2.down, GroundDistance);
            RaycastHit2D rightCheck = Raycast(new Vector2(FootOffset, -(BodyCollider.size.y / 2)), Vector2.down, GroundDistance);

            if (leftCheck || rightCheck) {
                IsGrounded = true;
            }

            // Check above the player's head
            RaycastHit2D headCheck = Raycast(new Vector2(0f, (BodyCollider.size.y / 2) - 0.08f), Vector2.up, HeadClearance);

            if (headCheck) {
                IsHeadBlocked = true;
            }

            // TODO: Double-Jump checks
        }

        void GroundMovement() {
            var xVelocity = Speed * Input.Horizontal;

            if(xVelocity * direction < 0f) {
                ChangeDirection();
            }

            Rigidbody.velocity = new Vector2(xVelocity, Rigidbody.velocity.y);
        }

        void JumpMovement() {
            if(IsGrounded && Input.JumpPressed) {
                IsGrounded = false;
                Rigidbody.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
            }

            // Cap to Max Fall Speed
            if(Rigidbody.velocity.y < MaxFallSpeed) {
                Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, MaxFallSpeed);
            }
        }

        void ChangeDirection() {
            //Turn the character by flipping the direction
            direction *= -1;

            //Record the current scale
            Vector3 scale = transform.localScale;

            //Set the X scale to be the original times the direction
            scale.x = originalXScale * direction;

            //Apply the new scale
            transform.localScale = scale;
        }

        public void Teleport(Vector3 position) {
            Rigidbody.position = position;
            Rigidbody.velocity *= 0;
        }

        //These two Raycast methods wrap the Physics2D.Raycast() and provide some extra
        //functionality
        RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length) {
            //Call the overloaded Raycast() method using the ground layermask and return 
            //the results
            return Raycast(offset, rayDirection, length, GroundLayer);
        }

        RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask) {
            //Record the player's position
            Vector2 pos = transform.position;

            //Send out the desired raycasr and record the result
            RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

            //If we want to show debug raycasts in the scene...
            if(showRaycasts) {
                //...determine the color based on if the raycast hit...
                Color color = hit ? Color.red : Color.green;
                //...and draw the ray in the scene view
                Debug.DrawRay(pos + offset, rayDirection * length, color);
            }

            //Return the results of the raycast
            return hit;
        }
    }
}