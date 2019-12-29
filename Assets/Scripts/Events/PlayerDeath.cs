using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;
using ThrowyBlock.Model;

namespace ThrowyBlock.Events {
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    /// <typeparam name="PlayerDeath"></typeparam>
    public class PlayerDeath : Simulation.Event<PlayerDeath> {
        public PlayerInfo PlayerInfo;

        readonly MapModel model = Simulation.GetModel<MapModel>();

        public override void Execute() {
            var player = model.GetPlayer(PlayerInfo);
            //if(player.health.IsAlive) {
            //    player.health.Die();
                player.ControlEnabled = false;

                //if(player.audioSource && player.ouchAudio)
                //    player.audioSource.PlayOneShot(player.ouchAudio);
                //player.animator.SetTrigger("hurt");
                player.Animator.SetBool("dead", true);
                Simulation.Schedule<PlayerSpawn>(2);
            //}
        }
    }
}