using ThrowyBlock.Core;
using ThrowyBlock.Mechanics;

namespace ThrowyBlock.Events {
    /// <summary>
    /// Fired when the player performs a Jump.
    /// </summary>
    /// <typeparam name="PlayerJumped"></typeparam>
    public class PlayerJumped : Simulation.Event<PlayerJumped> {
        public CharacterMovement Player;

        public override void Execute() {
            //if(Player.audioSource && Player.jumpAudio)
            //    Player.audioSource.PlayOneShot(Player.jumpAudio);
        }
    }
}