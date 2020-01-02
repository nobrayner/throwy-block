using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;

namespace ThrowyBlock.Events {
    public class EnablePlayerInput : Simulation.Event<EnablePlayerInput> {
        PlayerActions Player;

        public override void Execute() {
            Player.ControlEnabled = true;
        }

        public void SetPlayer(PlayerActions player) {
            Player = player;
        }
    }
}
