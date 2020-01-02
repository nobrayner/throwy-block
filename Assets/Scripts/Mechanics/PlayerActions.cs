using ThrowyBlock.Characters;
using ThrowyBlock.Core;
using ThrowyBlock.Events;
using ThrowyBlock.Model;
using ThrowyBlock.UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ThrowyBlock.Mechanics {
    public class PlayerActions : MonoBehaviour {
        public bool ControlEnabled = true;

        [Header("Status - Movement")]
        [ReadOnly] public bool IsGrounded = false;
        [ReadOnly] public bool IsJumping = false;
        [ReadOnly] public bool CanDoubleJump = false;
        [ReadOnly] public Vector2 TargetVelocity;
        [ReadOnly] public JumpState JumpState = JumpState.None;
        [ReadOnly] public int Facing = 1;


        [Header("Status - Block")]
        [ReadOnly] public bool IsHoldingBlock = false;
        [ReadOnly] public bool IsValidPickupBlock = false;
        [ReadOnly] public Vector3Int? PickupBlockPosition;
        [ReadOnly] public Sprite HeldBlockSprite;


        //[Header("Audio")]
        //public AudioClip jumpAudio;
        //public AudioClip respawnAudio;
        //public AudioClip ouchAudio;
        //public AudioSource audioSource;


        [Header("Movement Properties")]
        public float Speed = 4f;
        public float JumpForce = 22f;
        public float DoubleJumpForce = 12f;

        [Tooltip("The multiplier added to fall speed when pressing down")]
        public float FallSpeedModifier = 1.5f;
        public float MaxFallSpeed = -25f;

        [Tooltip("The amount of force to throw the held block with")]
        public float ThrowForce = 8f;
        public Vector3 ThrowOffset = new Vector3(.4f, .4f);
        public float ThrowKnockbackMultiplier = 2f;
        public float ThrowKnockbackStunDuration = .25f;


        [Header("Environment Check Properties"), Tooltip("X Offset for Foot Check Raycasts")]
        public float FootOffset = .4f;

        [Tooltip("Distance of raycast for Block-checking")]
        public float BlockCheckDistance = 0.8f;

        [Tooltip("The distance from the ground that counts as being grounded")]
        public float GroundDistance = .2f;
        [ReadOnly] public LayerMask GroundLayer;


        [Header("Components - Loaded on Start")]
        [ReadOnly] public PlayerInput Input;
        [ReadOnly] public Character Character;
        [ReadOnly] public Rigidbody2D Rigidbody;


        [Header("Components - Inspector Set")]
        public GameObject CharacterObject;
        public GameObject Projectile;


        readonly MapModel model = Simulation.GetModel<MapModel>();
        float originalXScale;
        float footOffsetFromCenter;

        void Start() {
            originalXScale = transform.localScale.x;
            footOffsetFromCenter = -(GetComponent<CapsuleCollider2D>().size.y / 2);

            GroundLayer = model.GroundLayer;

            //audioSource = GetComponent<AudioSource>();
            Character = CharacterObject.GetComponent<Character>();
            Rigidbody = GetComponent<Rigidbody2D>();
            Input = GetComponent<PlayerInput>();
        }

        void Update() {
            Character.SetSpeedParam(Mathf.Abs(TargetVelocity.x));
            //Character.SetGroundedParam(IsGrounded);
        }

        void FixedUpdate() {
            TargetVelocity = Rigidbody.velocity;

            // Physics check first
            PhysicsCheck();

            if (ControlEnabled) {
                // Apply Player Input as actions and movements
                Actions();
                XMovement();
                JumpMovement();
            }
            //else {
            //    Rigidbody.velocity = new Vector2(0f, Rigidbody.velocity.y);
            //}

            Rigidbody.velocity = TargetVelocity;
        }

        void PhysicsCheck() {
            // Assume the player isn't grounded, block in range is not a valid block, there is no block at the pickup position
            IsGrounded = false;
            IsValidPickupBlock = false;
            PickupBlockPosition = null;


            // Check Left and Right foot for grounding
            RaycastHit2D leftCheck = Raycast(new Vector2(-FootOffset, footOffsetFromCenter), Vector2.down, GroundDistance);
            RaycastHit2D rightCheck = Raycast(new Vector2(FootOffset, footOffsetFromCenter), Vector2.down, GroundDistance);

            if (leftCheck || rightCheck) {
                IsGrounded = true;
                IsJumping = false;
                CanDoubleJump = true;
            }

            // Check for the pick-up-able block
            var modifier = 1f + ((Mathf.Abs(Input.DirectionVector.x) + Mathf.Abs(Input.DirectionVector.y)) / 4);
            float checkDistance = BlockCheckDistance * modifier;
            RaycastHit2D blockCheck = Raycast(Vector2.zero, Input.NormalizedDirection, checkDistance);

            if (blockCheck) {
                var remainingRay = ((Input.NormalizedDirection * checkDistance) * (1f - blockCheck.fraction));
                var pickupBlockPoint = blockCheck.point + remainingRay;
                PickupBlockPosition = model.GroundMap.WorldToCell(new Vector3(pickupBlockPoint.x, pickupBlockPoint.y));
                IsValidPickupBlock = true;
            }
        }

        void Actions() {
            // Throw Block Action
            if (Input.ThrowPressed && IsHoldingBlock && Input.NormalizedDirection.magnitude > 0) {
                var throwForce = Input.NormalizedDirection * ThrowForce;

                // Knockback player from the throw
                if (!IsGrounded || !IsValidPickupBlock) {
                    TargetVelocity = Vector2.zero;
                    AddForce(-throwForce * ThrowKnockbackMultiplier);

                    Simulation.Schedule<SpawnThrownBlock>(0).SetSpawnDetails(Projectile, throwForce, Input.NormalizedDirection, transform, HeldBlockSprite);
                    Simulation.Schedule<DisablePlayerInput>(0).SetPlayer(this);
                    Simulation.Schedule<EnablePlayerInput>(ThrowKnockbackStunDuration).SetPlayer(this);
                }

                IsHoldingBlock = false;
                HeldBlockSprite = null;
            }
            // Pickup Block Action
            if (Input.PickupPressed && !IsHoldingBlock && IsValidPickupBlock) {
                var tile = model.GroundMap.GetTile<Tile>((Vector3Int) PickupBlockPosition);

                if (tile != null) {
                    IsHoldingBlock = true;
                    HeldBlockSprite = tile.sprite;

                    // Clear the block from the map
                    model.GroundMap.SetTile((Vector3Int) PickupBlockPosition, null);
                }
            }

            // Punch Action
            if (Input.PunchPressed) {
                // Do Punch Action
            }
        }

        void XMovement() {
            var inputSpeed = Speed * Input.DirectionVector.x;
            float targetXVelocity;

            if (ControlEnabled && Mathf.Abs(TargetVelocity.x) < Speed) {
                targetXVelocity = inputSpeed;

                if (inputSpeed * Facing < 0f) {
                    ChangeDirection();
                }
            } else {
                targetXVelocity = TargetVelocity.x;
            }

            TargetVelocity = new Vector2(targetXVelocity, TargetVelocity.y);
        }

        void JumpMovement() {
            if (Input.JumpPressed) {
                if ((IsGrounded && !IsJumping) || (JumpState == JumpState.Falling && !IsJumping)) {
                    IsGrounded = false;
                    IsJumping = true;
                    JumpState = JumpState.Takeoff;
                    AddForce(new Vector2(0f, JumpForce));
                } else if (!IsGrounded && CanDoubleJump && (JumpState == JumpState.Jumping || JumpState == JumpState.Falling)) {
                    CanDoubleJump = false;
                    JumpState = JumpState.DoubleJumping;
                    SetForce(new Vector2(TargetVelocity.x, DoubleJumpForce)); // Override original jump
                }
            }

            // TODO Wall Jump

            // Increase fall speed when pressing down
            if (JumpState == JumpState.Falling && Input.DirectionVector.y <= -0.7f) {
                TargetVelocity = new Vector2(TargetVelocity.x, TargetVelocity.y * FallSpeedModifier);
            }

            UpdateJumpState();

            // Cap to Max Fall Speed
            if (TargetVelocity.y < MaxFallSpeed) {
                TargetVelocity = new Vector2(TargetVelocity.x, MaxFallSpeed);
            }
        }

        void UpdateJumpState() {
            switch (JumpState) {
                case JumpState.Takeoff:
                    if (!IsGrounded) {
                        JumpState = JumpState.Jumping;
                    }
                    break;
                case JumpState.None:
                case JumpState.Jumping:
                case JumpState.DoubleJumping:
                    if (TargetVelocity.y < 0f) {
                        JumpState = JumpState.Falling;
                    }
                    break;
                case JumpState.Falling:
                    if (IsGrounded) {
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
            Facing *= -1;

            Vector3 scale = transform.localScale;
            scale.x = originalXScale * Facing;

            transform.localScale = scale;
        }

        public void Teleport(Vector3 position) {
            Rigidbody.position = position;
            TargetVelocity *= 0;
        }

        public void AddForce(Vector2 force) {
            TargetVelocity += force;
        }

        public void SetForce(Vector2 force) {
            TargetVelocity = force;
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
            if (model.ShowRaycasts) {
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