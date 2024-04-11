using UnityEngine;
using System.Collections;

namespace IceGame
{
    public class Player : MonoBehaviour
	{
        [SerializeField] private SpriteRenderer _sprite;

        private bool _isMoving;
        private bool _isOnLadder;

        public bool IsMoving
		{
            get { return _isMoving; }
        }

        public bool IsOnLadder
		{
            get { return _isOnLadder; }
        }
		
        public void Reset()
		{
            _isMoving = false;
            _isOnLadder = false;
        }

        public void SetToTransparent()
		{
            Color color = _sprite.color;
            color.a = 0;

            _sprite.color = color;
        }

        public void Fade(bool fadeIn, System.Action onDone = null)
		{
            _isMoving = true;

            StartCoroutine(ActionManager.FadeTransition(fadeIn, _sprite, () =>
			{
                _isMoving = false;

                if (onDone != null)
				{
                    onDone();
                }
            }));
        }

        public void HandleMovement(Movement direction, TileType[,] collisionMap)
		{
            int mapX = (int)(transform.localPosition.x * 2f);
            int mapY = (int)(transform.localPosition.y * -2f);

            if (direction == Movement.Left || direction == Movement.Right)
			{
                HandleHorizontalDirection(mapX, mapY, direction, collisionMap);
            }
            else if (direction == Movement.Up || direction == Movement.Down)
			{
                HandleVerticalDirection(mapX, mapY, direction, collisionMap);
            }
        }

        private void HandleHorizontalDirection(int startX, int startY, Movement direction, TileType[,] collisionMap)
		{
            int maxX = collisionMap.GetLength(0) - 1;
            int step = (direction == Movement.Right) ? 1 : -1;
            int destinationX = startX;

            for (int i = startX + step; i >= 0 && i <= maxX; i += step)
			{
                if (collisionMap[i, startY] != TileType.Solid)
				{
                    destinationX = i;

                    if (collisionMap[i, startY] == TileType.Ladder)
					{
                        _isOnLadder = true;
                        break;
                    }
                    else if (collisionMap[i, startY] == TileType.Walkable)
					{
                        break;
                    }
                }
                else
				{
                    break;
                }
            }

            Move(destinationX, startY);
        }

        private void HandleVerticalDirection(int startX, int startY, Movement direction, TileType[,] collisionMap)
		{
            int maxY = collisionMap.GetLength(1) - 1;
            int step = (direction == Movement.Down) ? 1 : -1;
            int destinationY = startY;

            for (int i = startY + step; i >= 0 && i <= maxY; i += step) {

                if (collisionMap[startX, i] != TileType.Solid) {

                    destinationY = i;

                    if (collisionMap[startX, i] == TileType.Ladder) {

                        _isOnLadder = true;
                        break;
                    }
                    else if (collisionMap[startX, i] == TileType.Walkable) {

                        break;
                    }
                }
                else {

                    break;
                }
            }

            Move(startX, destinationY);
        }

        private void Move(int destinationX, int destinationY)
		{
            _isMoving = true;
            float z = transform.localPosition.z;

            StartCoroutine(MoveObject(new Vector3(destinationX / 2f, destinationY / -2f, z)));
        }

        private IEnumerator MoveObject(Vector3 destination)
		{
            Vector3 start = transform.localPosition;
            float duration = (destination - transform.localPosition).magnitude / 5f;
            float currentTime = 0;

            while (currentTime < duration)
			{
                currentTime = Mathf.Min(currentTime + Time.deltaTime, duration);
                transform.localPosition = Vector3.Lerp(start, destination, currentTime / duration);

                yield return null;
            }

            _isMoving = false;
        }
    }
}