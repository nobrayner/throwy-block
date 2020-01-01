using UnityEngine;
using ThrowyBlock.Model;
using ThrowyBlock.Core;
using ThrowyBlock.UnityEditor;
using UnityEngine.Tilemaps;

namespace ThrowyBlock.Mechanics {
    public class Projectile : MonoBehaviour {
        [Header("Projectile Properties")]
        public float KnockBackForce;
        [ReadOnly] public bool IsGroundAbove;

        [Header("Components - Loaded on Start")]
        [ReadOnly] public Sprite BlockSprite;

        readonly MapModel model = Simulation.GetModel<MapModel>();

        void Start() {
            BlockSprite = GetComponent<SpriteRenderer>().sprite;
        }

        void FixedUpdate() {
            IsGroundAbove = false;

            var groundAbove = Raycast(new Vector2(), Vector2.up, (transform.localScale.y / 2) + 0.1f, model.GroundLayer);

            if (groundAbove) {
                IsGroundAbove = true;
            }
        }

        void OnCollisionEnter2D(Collision2D collision) {
            var collidedWithLayer = 1 << collision.gameObject.layer;

            if (IsGroundAbove)
                return;

            if (collidedWithLayer == model.GroundLayer.value) {
                var collisionTilePosition = model.GroundMap.WorldToCell(transform.position);

                model.GroundMap.SetTile(collisionTilePosition, GetTile());
            } else if (collidedWithLayer == model.PlayerLayer.value) {
                // TODO: Knockback player
                Debug.Log("Player was hit!");
            }

            Destroy(transform.gameObject);
        }

        Tile GetTile() {
            var tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = BlockSprite;

            return tile;
        }

        RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask) {
            //Record the player's position
            Vector2 pos = transform.position;

            //Send out the desired raycasr and record the result
            RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

            //If we want to show debug raycasts in the scene...
            if (model.ShowRaycasts) {
                //...determine the color based on if the raycast hit...
                Color color = hit ? Color.red : Color.green;
                //...and draw the ray in the scene view
                Debug.DrawRay(pos + offset, rayDirection * length, color);
            }

            //Return the results of the raycast
            return hit;
        }
    }
}
