using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;
using ThrowyBlock.Model;

namespace ThrowyBlock.Events {
    public class PlayerDied : Simulation.Event<PlayerDied> {
        CharacterActions Player;

        readonly MapModel model = Simulation.GetModel<MapModel>();

        public override void Execute() {
            Player.ControlEnabled = false;

            Simulation.Schedule<UnseePlayer>(0).SetPlayer(Player);
            Simulation.Schedule<SpawnPlayer>(0.3f).SetPlayer(Player);
        }

        public void SetPlayer(CharacterActions player) {
            Player = player;
        }
    }
}