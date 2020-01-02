using UnityEngine;

namespace ThrowyBlock.Characters {
    public class BillCharacter : Character {
        public Animator BillAnimator;
        public Animator LeftArmAnimator;
        public Animator RightArmAnimator;

        new void Start() {
            base.Start();
            BillAnimator = GetComponent<Animator>();    
        }

        public override void SetSpeedParam(float speed) {
            BillAnimator.SetFloat(speedParamHash, speed);
        }
    }
}
