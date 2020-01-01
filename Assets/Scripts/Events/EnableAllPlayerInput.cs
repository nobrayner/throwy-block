using ThrowyBlock.Core;
using ThrowyBlock.Model;

namespace ThrowyBlock.Events {
    public class EnableAllPlayerInput : Simulation.Event<EnableAllPlayerInput> {
        readonly MapModel model = Simulation.GetModel<MapModel>();

        public override void Execute() {
            foreach(var player in model.Players) {
                player.ControlEnabled = true;
            }
        }
    }
}