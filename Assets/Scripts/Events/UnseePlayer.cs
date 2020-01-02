using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;
using ThrowyBlock.Model;

namespace ThrowyBlock.Events {
    public class UnseePlayer : Simulation.Event<UnseePlayer> {
        CharacterActions Player;

        public override void Execute() {
            Player.SpriteRenderer.enabled = false;
        }

        public void SetPlayer(CharacterActions player) {
            Player = player;
        }
    }
}