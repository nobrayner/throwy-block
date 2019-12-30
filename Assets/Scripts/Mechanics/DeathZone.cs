using UnityEngine;
using ThrowyBlock.Events;
using static ThrowyBlock.Core.Simulation;

namespace ThrowyBlock.Mechanics {
    public class DeathZone : MonoBehaviour {
        void OnTriggerEnter2D(Collider2D collider) {
            var p = collider.gameObject.GetComponent<CharacterActions>();
            if(p != null) {
                var ev = Schedule<PlayerEnteredDeathZone>();
                ev.DeathZone = this;
            }
        }
    }
}