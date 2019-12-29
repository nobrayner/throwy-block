﻿using ThrowyBlock.UnityEditor;
using UnityEngine;

namespace ThrowyBlock.Mechanics {
    [DefaultExecutionOrder(-100)]
    public class PlayerInput : MonoBehaviour {
        [ReadOnly] public Vector2 DirectionVector;
        [ReadOnly] public bool JumpPressed;
        [ReadOnly] public bool PickupBlockPressed;
        [ReadOnly] public bool DeflectPressed;
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
            PickupBlockPressed = false;
            DeflectPressed = false;

            // Reset clear flag
            readyToClear = false;
        }

        void ProcessInputs() {
            Horizontal += Input.GetAxis("Horizontal" + PlayerInfo.PlayerNumber);
            Vertical += Input.GetAxis("Vertical" + PlayerInfo.PlayerNumber);
            DirectionVector = new Vector2(Horizontal, Vertical);

            JumpPressed = JumpPressed || Input.GetButtonDown("Jump" + PlayerInfo.PlayerNumber);
            PickupBlockPressed = PickupBlockPressed || Input.GetButtonDown("PickUp" + PlayerInfo.PlayerNumber);
            DeflectPressed = DeflectPressed || Input.GetButtonDown("Block" + PlayerInfo.PlayerNumber);
        }
    }
}
