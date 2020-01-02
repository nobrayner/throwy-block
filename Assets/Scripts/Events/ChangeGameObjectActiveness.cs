using ThrowyBlock.Core;
using UnityEngine;

namespace ThrowyBlock.Events {
    public class ChangeGameObjectActiveness : Simulation.Event<ChangeGameObjectActiveness> {
        GameObject GameObject;
        bool Active;
        
        public override void Execute() {
            GameObject.SetActive(Active);
        }

        public void SetGameObject(GameObject gameObject, bool active) {
            GameObject = gameObject;
            Active = active;
        }
    }
}
