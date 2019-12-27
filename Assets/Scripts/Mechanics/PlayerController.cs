using UnityEngine;

namespace ThrowyBlock.Mechanics {
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject {
        //public AudioClip jumpAudio;
        //public AudioClip respawnAudio;
        //public AudioClip ouchAudio;

        private float jumpModifier = 1.0f;
        private float jumpDeceleration = 0f;

        public int PlayerNumber = 1;

        public float maxSpeed = 4;
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;

        public Collider2D collider2d;
        //public AudioSource audioSource;
        //public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        //readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake() {
            //health = GetComponent<Health>();
            //audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        protected override void Update() {
            if(controlEnabled) {
                move.x = Input.GetAxis("Horizontal" + PlayerNumber);
                if(jumpState == JumpState.Grounded && Input.GetButtonDown("Jump" + PlayerNumber)) {
                    jumpState = JumpState.PrepareToJump;
                    stopJump = true;
                    //Schedule<PlayerStopJump>().player = this;
                }
            } else {
                move.x = 0;
            }
            UpdateJumpState();
            base.Update();
        }

        void UpdateJumpState() {
            jump = false;
            switch(jumpState) {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if(!IsGrounded) {
                        //Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if(IsGrounded) {
                        //Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity() {
            if(jump && IsGrounded) {
                velocity.y = jumpTakeOffSpeed * jumpModifier;
                jump = false;
            } else if(stopJump) {
                stopJump = false;
                if(velocity.y > 0) {
                    velocity.y = velocity.y * jumpDeceleration;
                }
            }

            if(move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if(move.x < -0.01f)
                spriteRenderer.flipX = true;

            //animator.SetBool("grounded", IsGrounded);
            //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            animator.SetBool("Moving", Mathf.Abs(velocity.x) > 0.01);

            targetVelocity = move * maxSpeed;
        }

        public enum JumpState {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}