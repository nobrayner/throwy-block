using UnityEngine;
using ThrowyBlock.Gameplay;
using static ThrowyBlock.Core.Simulation;

namespace ThrowyBlock.Mechanics {
    /// <summary>
    /// DeathZone components mark a collider which will schedule a
    /// PlayerEnteredDeathZone event when the player enters the trigger.
    /// </summary>
    public class DeathZone : MonoBehaviour {
        void OnTriggerEnter2D(Collider2D collider) {
            var p = collider.gameObject.GetComponent<PlayerMovement>();
            if(p != null) {
                var ev = Schedule<PlayerEnteredDeathZone>();
                ev.deathzone = this;
            }
        }
    }
}