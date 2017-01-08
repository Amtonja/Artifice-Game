using UnityEngine;

namespace Artifice.Data {
    /// <summary>
    /// Manages all the high-level game desisions
    /// </summary>
    public class GameManager : MonoBehaviour {
        // Instance variable to set up singleton
        private static GameManager instance;

        public delegate void SpriteOrderUpdate();
        /// <summary>
        /// Event for when to tell all sprites to reorder themselves
        /// </summary>
        public static event SpriteOrderUpdate UpdateSpriteOrder;

        /// <summary>
        /// How many units before resorting needs to occur on sprites
        /// </summary>
        public const int PLAYER_RESET_THRESHOLD = 200;
        private int playerChunk = 0;

        private void Awake() {
            if(instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else if(instance != this) {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Calls the Update Sprite Event
        /// </summary>
        public void CallUpdateSpriteOrder() {
            if (UpdateSpriteOrder != null) UpdateSpriteOrder();
        }

        public int PlayerChunk {
            get { return playerChunk; }
            set {
                playerChunk = value;
                CallUpdateSpriteOrder();
            }
        }

        /// <summary>
        /// Returns the current chunk the player is in (for sprite sorting)
        /// </summary>
        /// <returns>The current chunk the player is in (for sprite sorting)</returns>
        public int GetChunkOffset() {
            return playerChunk * PLAYER_RESET_THRESHOLD;
        }

        public static GameManager Instance {
            get {
                if(instance == null) { instance = new GameObject("Game Manager").AddComponent<GameManager>(); }
                return instance;
            }
        }
    } 
}
