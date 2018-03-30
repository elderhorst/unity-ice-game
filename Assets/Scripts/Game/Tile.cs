using UnityEngine;

namespace Game {

    public class Tile : MonoBehaviour {

        public void init(Sprite sprite) {

            SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
        }
    }
}