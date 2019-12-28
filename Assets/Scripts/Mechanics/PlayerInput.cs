using ThrowyBlock.UnityEditor;
using UnityEngine;

namespace ThrowyBlock.Mechanics {
    [DefaultExecutionOrder(-100)]
    public class PlayerInput : MonoBehaviour {
        [ReadOnly] public float Horizontal;
        [ReadOnly] public bool JumpPressed;
        [HideInInspector] public PlayerInfo PlayerInfo;

        bool readyToClear;

        void Start() {
            PlayerInfo = GetComponent<PlayerInfo>();
        }

        void Update() {
            ClearInput();

            // Game Over clause?

            ProcessInputs();

            Horizontal = Mathf.Clamp(Horizontal, -1f, 1f);
        }

        void FixedUpdate() {
            readyToClear = true;
        }

        void ClearInput() {
            if(!readyToClear) {
                return;
            }

            Horizontal = 0f;
            JumpPressed = false;

            readyToClear = false;
        }

        void ProcessInputs() {
            Horizontal += Input.GetAxis("Horizontal" + PlayerInfo.PlayerNumber);

            JumpPressed = JumpPressed || Input.GetButtonDown("Jump" + PlayerInfo.PlayerNumber);
        }
    }
}
