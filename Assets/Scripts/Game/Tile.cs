using UnityEngine;

namespace IceGame
{
    public class Tile : MonoBehaviour
	{
        public void Init(Sprite sprite)
		{
            SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
        }
    }
}