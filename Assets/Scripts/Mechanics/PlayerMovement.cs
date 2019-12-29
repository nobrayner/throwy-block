using ThrowyBlock.UnityEditor;
using UnityEngine;

namespace ThrowyBlock.Mechanics {
    public class PlayerMovement : MonoBehaviour {
        public bool showRaycasts = true;
        public bool ControlEnabled = true;

        [Header("Status - Readonly")]
        [ReadOnly] public bool IsGrounded = false;
        [ReadOnly] public bool IsHeadBlocked = false;
        [ReadOnly] public bool IsJumping = false;
        [ReadOnly] public bool CanDoubleJump = false;
        [ReadOnly] public Vector2 Velocity;
        [ReadOnly] public JumpState JumpState = JumpState.None;

        [Header("Movement Properties")]
        public float Speed = 4f;
        public float JumpForce = 22f;
        public float DoubleJumpForce = 12f;
        public float MaxFallSpeed = -25f;


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
        int speedParamHash;
        int groundedParamHash;

        void Start() {
            movingParamHash = Animator.StringToHash("Moving");
            speedParamHash = Animator.StringToHash("Speed");
            groundedParamHash = Animator.StringToHash("Grounded");

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
            var speed = Mathf.Abs(Input.Horizontal);

            Animator.SetFloat(speedParamHash, speed);
            Animator.SetBool(movingParamHash, speed > 0f);
            Animator.SetBool(groundedParamHash, IsGrounded);
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
            // Assumbe the player isn't grounded, their head is not blocked
            IsGrounded = false;
            IsHeadBlocked = false;

            // Check Left and Right foot for grounding
            RaycastHit2D leftCheck = Raycast(new Vector2(-FootOffset, -(BodyCollider.size.y / 2)), Vector2.down, GroundDistance);
            RaycastHit2D rightCheck = Raycast(new Vector2(FootOffset, -(BodyCollider.size.y / 2)), Vector2.down, GroundDistance);

            if (leftCheck || rightCheck) {
                IsGrounded = true;
                IsJumping = false;
                CanDoubleJump = true;
            }

            // Check above the player's head
            RaycastHit2D headCheck = Raycast(new Vector2(0f, (BodyCollider.size.y / 2) - 0.08f), Vector2.up, HeadClearance);

            if (headCheck) {
                IsHeadBlocked = true;
            }
        }

        void GroundMovement() {
            var xVelocity = Speed * Input.Horizontal;

            if(xVelocity * direction < 0f) {
                ChangeDirection();
            }

            Rigidbody.velocity = new Vector2(xVelocity, Rigidbody.velocity.y);
        }

        void JumpMovement() {
            if(Input.JumpPressed) {
                if((IsGrounded && !IsJumping) || (JumpState == JumpState.Falling && !IsJumping)) {
                    IsGrounded = false;
                    IsJumping = true;
                    JumpState = JumpState.Takeoff;
                    Rigidbody.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
                } else if(!IsGrounded && CanDoubleJump && (JumpState == JumpState.Jumping || JumpState == JumpState.Falling)) {
                    CanDoubleJump = false;
                    JumpState = JumpState.DoubleJumping;
                    Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0); // We want the double-jump force to override the original jump
                    Rigidbody.AddForce(new Vector2(0f, DoubleJumpForce), ForceMode2D.Impulse);
                }
            }

            UpdateJumpState();

            // Cap to Max Fall Speed
            if(Rigidbody.velocity.y < MaxFallSpeed) {
                Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, MaxFallSpeed);
            }
        }

        void UpdateJumpState() {
            switch(JumpState) {
                case JumpState.Takeoff:
                    if(!IsGrounded) {
                        JumpState = JumpState.Jumping;
                    }
                    break;
                case JumpState.None:
                case JumpState.Jumping:
                case JumpState.DoubleJumping:
                    if (Rigidbody.velocity.y < 0f) {
                        JumpState = JumpState.Falling;
                    }
                    break;
                case JumpState.Falling:
                    if(IsGrounded) {
                        //Schedule<PlayerLanded>().player = this;
                        JumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    JumpState = JumpState.None;
                    break;
            }
        }

        void ChangeDirection() {
            direction *= -1;

            Vector3 scale = transform.localScale;
            scale.x = originalXScale * direction;

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