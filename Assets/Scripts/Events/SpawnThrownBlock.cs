using ThrowyBlock.Core;
using ThrowyBlock.Model;
using UnityEngine;

namespace ThrowyBlock.Events {
    public class SpawnThrownBlock : Simulation.Event<SpawnThrownBlock> {
        GameObject Projectile;
        Vector2 ThrowForce;
        Vector2 ThrowDirection;
        Transform Character;
        Sprite BlockSprite;

        public override void Execute() {
            var spawnPosition = Character.transform.position;

            var projectile = GameObject.Instantiate(Projectile);
            projectile.transform.position = spawnPosition + new Vector3(ThrowDirection.x, ThrowDirection.y);
            projectile.GetComponent<SpriteRenderer>().sprite = BlockSprite;

            // Throw block
            projectile.GetComponent<Rigidbody2D>().AddForce(ThrowForce, ForceMode2D.Impulse);
        }

        public void SetSpawnDetails(GameObject projectile, Vector2 throwForce, Vector2 throwDirection, Transform character, Sprite blockSprite) {
            Projectile = projectile;
            ThrowForce = throwForce;
            ThrowDirection = throwDirection;
            Character = character;
            BlockSprite = blockSprite;
        }
    }
}