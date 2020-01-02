using ThrowyBlock.UnityEditor;
using UnityEngine;

namespace ThrowyBlock.Characters {
    public abstract class Character : MonoBehaviour {
        public SpriteRenderer SpriteRenderer;

        protected int speedParamHash = Animator.StringToHash("Speed");
        //protected int groundedParamHash = Animator.StringToHash("Grounded");

        public void Start() {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public abstract void SetSpeedParam(float speed);
    }
}
