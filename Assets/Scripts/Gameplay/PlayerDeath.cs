using System.Collections;
using System.Collections.Generic;
using ThrowyBlock.Core;
using ThrowyBlock.Model;
using UnityEngine;

namespace ThrowyBlock.Gameplay {
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    /// <typeparam name="PlayerDeath"></typeparam>
    public class PlayerDeath : Simulation.Event<PlayerDeath> {
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute() {
            var player = model.player;
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