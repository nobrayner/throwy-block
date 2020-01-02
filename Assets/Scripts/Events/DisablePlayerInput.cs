using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;
using ThrowyBlock.Model;

namespace ThrowyBlock.Events {
    public class DisablePlayerInput : Simulation.Event<DisablePlayerInput> {
        PlayerActions Player;

        public override void Execute() {
            Player.ControlEnabled = false;
        }

        public void SetPlayer(PlayerActions player) {
            Player = player;
        }
    }
}