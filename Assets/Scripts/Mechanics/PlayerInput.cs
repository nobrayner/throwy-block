using ThrowyBlock.UnityEditor;
using UnityEngine;

namespace ThrowyBlock.Mechanics {
    [DefaultExecutionOrder(-100)]
    public class PlayerInput : MonoBehaviour {
        [ReadOnly] public Vector2 DirectionVector;
        [ReadOnly] public Vector2 NormalizedDirection;
        [ReadOnly] public bool JumpPressed;
        [ReadOnly] public bool PickupPressed;
        [ReadOnly] public bool ThrowPressed;
        [ReadOnly] public bool PunchPressed;
        [HideInInspector] public PlayerInfo PlayerInfo;

        float Horizontal;
        float Vertical;

        bool readyToClear;

        void Start() {
            PlayerInfo = GetComponent<PlayerInfo>();
        }

        void Update() {
            ClearInput();

            // Game Over clause?

            ProcessInputs();

            Horizontal = Mathf.Clamp(Horizontal, -1f, 1f);
            Vertical = Mathf.Clamp(Vertical, -1f, 1f);
            DirectionVector = Vector2.ClampMagnitude(DirectionVector, 1f);
            NormalizedDirection = DirectionVector.normalized;
        }

        void FixedUpdate() {
            readyToClear = true;
        }

        void ClearInput() {
            if(!readyToClear) {
                return;
            }

            // Clear inputs
            Horizontal = 0f;
            Vertical = 0f;
            DirectionVector = Vector2.zero;

            JumpPressed = false;
            PickupPressed = false;
            ThrowPressed = false;
            PunchPressed = false;

            // Reset clear flag
            readyToClear = false;
        }

        void ProcessInputs() {
            Horizontal += Input.GetAxis("Horizontal" + PlayerInfo.PlayerNumber);
            Vertical += Input.GetAxis("Vertical" + PlayerInfo.PlayerNumber);

            if (Horizontal == 0f) {
                Horizontal += Input.GetAxis("HorizontalKeyboardInput");
            }
            if (Vertical == 0f) {
                Vertical += Input.GetAxis("VerticalKeyboardInput");
            }

            DirectionVector = new Vector2(Horizontal, Vertical);

            JumpPressed = JumpPressed || Input.GetButtonDown("Jump" + PlayerInfo.PlayerNumber);
            PickupPressed = PickupPressed || Input.GetButtonDown("PickUp" + PlayerInfo.PlayerNumber);
            ThrowPressed = ThrowPressed || Input.GetButtonDown("Throw" + PlayerInfo.PlayerNumber);
            PunchPressed = PunchPressed || Input.GetButtonDown("Punch" + PlayerInfo.PlayerNumber);
        }
    }
}
