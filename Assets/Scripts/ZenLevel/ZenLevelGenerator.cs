using UnityEngine;

namespace IceGame
{
	public class ZenLevelGenerator : LevelCreator
	{
		private const int minSize = 7;
		private const int maxWidth = 26;
		private const int maxHeight = 16;

		private Vector2 _startPoint;
		
		public void GenerateNewLevel()
		{
			_width = Random.Range(minSize, maxWidth + 1);
			_height = Random.Range(minSize, maxHeight + 1);

			_levelData = InitLevel();
			_collisionMap = new TileType[_width, _height];

			_startPoint.x = (Random.value > 0.5f) ? Random.Range(0, 2) : Random.Range(_width - 2, _width);
			_startPoint.y = (Random.value > 0.5f) ? Random.Range(0, 2) : Random.Range(_height - 2, _height);
			
			PathGenerator pathGenerator = new PathGenerator();
			Path path = pathGenerator.Generate(_width, _height, _startPoint);
			path = AddRandomObstaclesToGrid(path);
			
			_levelData = path.TileMap;
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
		
		private Path AddRandomObstaclesToGrid(Path path)
		{
			int minRocks = 8 + (int)Mathf.Lerp(0, 20, (_width * _height) / (maxWidth * maxHeight));
			int maxRocks = 40 + (int)Mathf.Lerp(0, 80, (_width * _height) / (maxWidth * maxHeight));
			int minSize = 49;
			int maxSize = 400;
			int size = path.Width * path.Height;

			float scaledValue = (size - minSize) / (maxSize - minSize);
			int numberOfRocks = Mathf.RoundToInt(Mathf.Lerp(minRocks, maxRocks, scaledValue));

			for (int i = 0; i < numberOfRocks; i++)
			{
				path = PlaceRandomObstacle(path);
			}

			return path;
		}

		private Path PlaceRandomObstacle(Path path)
		{
			while (true)
			{
				int randomX = Random.Range(0, path.Width);
				int randomY = Random.Range(0, path.Height);

				if (path.PathMap[randomX, randomY] == 0 && path.TileMap[randomX, randomY] == 1)
				{
					path.TileMap[randomX, randomY] = 3;
					break;
				}
			}

			return path;
		}

		public override Vector2 GetStartingPoint()
		{
            return _startPoint;
        }
	}
}