using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;
using ThrowyBlock.Model;

namespace ThrowyBlock.Events {
    public class SeePlayer : Simulation.Event<SeePlayer> {
        PlayerActions Player;

        public override void Execute() {
            Player.Character.SpriteRenderer.enabled = true;
        }

        public void SetPlayer(PlayerActions player) {
            Player = player;
        }
    }
}