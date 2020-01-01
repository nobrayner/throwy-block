using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;
using ThrowyBlock.Model;

namespace ThrowyBlock.Events {
    public class SeePlayer : Simulation.Event<SeePlayer> {
        CharacterActions Player;

        public override void Execute() {
            Player.SpriteRenderer.enabled = true;
        }

        public void SetPlayer(CharacterActions player) {
            Player = player;
        }
    }
}