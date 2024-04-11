using UnityEngine;

namespace IceGame
{
	public class ZenLevelGenerator : LevelCreator
	{
		private const int minSize = 7;
		private const int maxSize = 20;

		private Vector2 _startPoint;
		
		public void GenerateNewLevel()
		{
			_width = Random.Range(minSize, maxSize + 1);
			_height = Random.Range(minSize, maxSize + 1);

			_levelData = InitLevel();
			_collisionMap = new TileType[_width, _height];

			_startPoint.x = (Random.value > 0.5f) ? Random.Range(0, 2) : Random.Range(_width - 2, _width);
			_startPoint.y = (Random.value > 0.5f) ? Random.Range(0, 2) : Random.Range(_height - 2, _height);
			
			// Do path map last for now.
			_levelData = GeneratePath();
			//createCollisionMap();
		}

		private int[,] GeneratePath()
		{
			PathGenerator pathGenerator = new PathGenerator();

			return pathGenerator.Generate(_width, _height, _startPoint);
		}

		private int[,] InitLevel()
		{
			int[,] level = new int[_width, _height];

			for (int y = 0; y < _height; y++) {

				for (int x = 0; x < _width; x++) {

						level[x, y] = 1;
				}
			}

			return level;
		}

		public override Vector2 GetStartingPoint()
		{
            return _startPoint;
        }
	}
}