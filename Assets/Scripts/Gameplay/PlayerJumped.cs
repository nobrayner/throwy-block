using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;

namespace ThrowyBlock.Gameplay {
    /// <summary>
    /// Fired when the player performs a Jump.
    /// </summary>
    /// <typeparam name="PlayerJumped"></typeparam>
    public class PlayerJumped : Simulation.Event<PlayerJumped> {
        public PlayerMovement player;

        public override void Execute() {
            //if(player.audioSource && player.jumpAudio)
            //    player.audioSource.PlayOneShot(player.jumpAudio);
        }
    }
}