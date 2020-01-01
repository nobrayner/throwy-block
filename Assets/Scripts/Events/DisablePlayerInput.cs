using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;
using ThrowyBlock.Model;

namespace ThrowyBlock.Events {
    public class DisablePlayerInput : Simulation.Event<DisablePlayerInput> {
        CharacterActions Player;

        public override void Execute() {
            Player.ControlEnabled = false;
        }

        public void SetPlayer(CharacterActions player) {
            Player = player;
        }
    }
}