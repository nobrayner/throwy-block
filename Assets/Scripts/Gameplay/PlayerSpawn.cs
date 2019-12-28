using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;
using ThrowyBlock.Model;

namespace ThrowyBlock.Gameplay {
    /// <summary>
    /// Fired when the player is spawned after dying.
    /// </summary>
    public class PlayerSpawn : Simulation.Event<PlayerSpawn> {
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute() {
            var player = model.player;
            player.BodyCollider.enabled = true;
            player.ControlEnabled = false;
            //if(player.audioSource && player.respawnAudio)
            //    player.audioSource.PlayOneShot(player.respawnAudio);
            //player.health.Increment();
            player.Teleport(model.spawnPoint.transform.position);
            //player.CharacterState = PlayerMovement.CharacterState.Grounded;
            //player.Animator.SetBool("dead", false);
            Simulation.Schedule<EnablePlayerInput>(2f);
        }
    }
}