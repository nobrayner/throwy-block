using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;
using ThrowyBlock.Model;

namespace ThrowyBlock.Events {
    public class UnseePlayer : Simulation.Event<UnseePlayer> {
        PlayerActions Player;

        public override void Execute() {
            Player.Character.SpriteRenderer.enabled = false;
        }

        public void SetPlayer(PlayerActions player) {
            Player = player;
        }
    }
}