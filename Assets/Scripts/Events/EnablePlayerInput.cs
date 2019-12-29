using ThrowyBlock.Core;
using ThrowyBlock.Model;

namespace ThrowyBlock.Events {
    public class EnablePlayerInput : Simulation.Event<EnablePlayerInput> {
        readonly MapModel model = Simulation.GetModel<MapModel>();

        public override void Execute() {
            foreach(var player in model.Players) {
                player.ControlEnabled = true;
            }
        }
    }
}