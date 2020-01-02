using UnityEngine;
using ThrowyBlock.Events;
using static ThrowyBlock.Core.Simulation;

namespace ThrowyBlock.Mechanics {
    public class DeathZone : MonoBehaviour {
        void OnTriggerEnter2D(Collider2D collider) {
            var player = collider.gameObject.GetComponent<PlayerActions>();
            if(player != null) {
                Schedule<PlayerDied>().SetPlayer(player);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            var player = collision.gameObject.GetComponent<PlayerActions>();
            if (player != null) {
                Schedule<PlayerDied>().SetPlayer(player);
            }
        }
    }
}