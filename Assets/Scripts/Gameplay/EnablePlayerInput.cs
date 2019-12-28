using ThrowyBlock.Core;
using ThrowyBlock.Model;

namespace ThrowyBlock.Gameplay {
    /// <summary>
    /// This event is fired when user input should be enabled.
    /// </summary>
    public class EnablePlayerInput : Simulation.Event<EnablePlayerInput> {
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute() {
            var player = model.player;
            player.ControlEnabled = true;
        }
    }
}