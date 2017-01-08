using UnityEngine;

namespace Artifice.Data {
    /// <summary>
    /// Updates a sprite's sorting layer based off of it's y coordinate
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class SortableSprite : MonoBehaviour {
        private float sortingOrder;

        public const int ORDER_MULTIPLIER = 100;            // Howw much to scale the y value by to get more precise ordering

        private Sprite sprite;
        private SpriteRenderer spriteRenderer;
        private Vector3 initialBounds;

        private void OnEnable() {
            GameManager.UpdateSpriteOrder += UpdateSortingOrder;
        }
        private void OnDisable() {
            GameManager.UpdateSpriteOrder -= UpdateSortingOrder;
        }

        private void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            sprite = spriteRenderer.sprite;
            
            // Destroy this script is there is no sprite to sort
            if(sprite == null) {
                Debug.LogError("Sprite on object: " + gameObject.name + " has no Sprite. Destroying script");
                Destroy(this);
            }

            // Sets the initial bounds to the bottom of the sprite
            initialBounds = new Vector3(sprite.bounds.center.x, sprite.bounds.min.y, 0);

            UpdateSortingOrder();
        }

        // DEBUG ONLY
        /*
        private void Update() {
            UpdateSortingOrder();
        }
        */

        /// <summary>
        /// Updates the sorting order of the sprite
        /// </summary>
        public void UpdateSortingOrder() {
            // To avoid errors with overflow
            // we use a "chunk" system to sort things
            // based off of the y val in combination to the chunk system
            sortingOrder = (GetBottomOfSprite().y + -GameManager.Instance.GetChunkOffset()) * ORDER_MULTIPLIER;
            //sortingOrder = GetBottomOfSprite().y * ORDER_MULTIPLIER;

            // Set the sorting order to the negative so that lower objects render on top
            spriteRenderer.sortingOrder = -(int)sortingOrder;
        }

        /// <summary>
        /// Gets the bottom of the sprite
        /// </summary>
        /// <returns>The point at the bottom of a sprite</returns>
        private Vector3 GetBottomOfSprite() {
            return transform.root.position + initialBounds;
        }
    } 
}
