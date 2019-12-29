using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;
using ThrowyBlock.Model;

namespace ThrowyBlock.Events {
    /// <summary>
    /// Fired when the player is spawned after dying.
    /// </summary>
    public class PlayerSpawn : Simulation.Event<PlayerSpawn> {
        public PlayerInfo PlayerInfo;

        readonly MapModel model = Simulation.GetModel<MapModel>();

        public override void Execute() {
            var player = model.GetPlayer(PlayerInfo);
            player.BodyCollider.enabled = true;
            player.ControlEnabled = false;
            //if(player.audioSource && player.respawnAudio)
            //    player.audioSource.PlayOneShot(player.respawnAudio);
            //player.health.Increment();
            player.Teleport(model.RespawnPoint.transform.position);
            //player.CharacterState = PlayerMovement.CharacterState.Grounded;
            //player.Animator.SetBool("dead", false);
            Simulation.Schedule<EnablePlayerInput>(2f);
        }
    }
}