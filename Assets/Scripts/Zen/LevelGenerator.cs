using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zen {

	public class LevelGenerator : Game.LevelCreator {

		private const int minSize = 7;
		private const int maxSize = 20;

		private Vector2 _startPoint;
		private Vector2 _endPoint;

		public LevelGenerator() {

		}

		public void generateNewLevel() {

			_width = Random.Range(minSize, maxSize + 1);
			_height = Random.Range(minSize, maxSize + 1);

			_levelData = initLevel();
			_collisionMap = new Game.TileType[_width, _height];

			_startPoint.x = (Random.value > 0.5f) ? Random.Range(0, 2) : Random.Range(_width - 2, _width);
			_startPoint.y = (Random.value > 0.5f) ? Random.Range(0, 2) : Random.Range(_height - 2, _height);
			_endPoint = _startPoint;
			
			// Do path map last for now.
			_levelData = generatePath();
			//createCollisionMap();
		}

		private int[,] generatePath() {

			PathGenerator pathGenerator = new PathGenerator();

			return pathGenerator.generate(_width, _height, _startPoint);
		}

		private int[,] initLevel() {

			int[,] level = new int[_width, _height];

			for (int y = 0; y < _height; y++) {

				for (int x = 0; x < _width; x++) {

						level[x, y] = 1;
				}
			}

			return level;
		}

		public override Vector2 getStartingPoint() {

            return _startPoint;
        }
	}
}