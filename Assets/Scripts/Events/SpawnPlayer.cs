using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;
using ThrowyBlock.Model;

namespace ThrowyBlock.Events {
    public class SpawnPlayer : Simulation.Event<SpawnPlayer> {
        PlayerActions Player;

        readonly MapModel model = Simulation.GetModel<MapModel>();

        public override void Execute() {
            Player.Teleport(model.RespawnPoint.transform.position);
            Simulation.Schedule<EnablePlayerInput>(0.05f).SetPlayer(Player);
            Simulation.Schedule<SeePlayer>(0.05f).SetPlayer(Player);
        }

        public void SetPlayer(PlayerActions player) {
            Player = player;
        }
    }
}