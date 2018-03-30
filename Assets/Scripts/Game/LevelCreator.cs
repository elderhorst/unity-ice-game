using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {

	public abstract class LevelCreator {

		protected int[,] _levelData;
		protected Game.TileType[,] _collisionMap;

		protected int _width;
		protected int _height;

		public Game.Tile[,] createTileGameObjects(Transform container, Sprite[] sprites) {

            Game.Tile[,] tiles = new Game.Tile[_width, _height];
            
            for (int x = 0; x < _width; x++) {

                for (int y = 0; y < _height; y++) {

                    GameObject obj = new GameObject();
                    obj.transform.parent = container;
                    obj.transform.localPosition = new Vector3(x / 2f, -y / 2f, 0);
                    obj.name = "Tile " + x + " " + y;

                    tiles[x,y] = obj.AddComponent<Game.Tile>();
                    tiles[x,y].init(sprites[_levelData[x, y]]);
                }
            }

			container.transform.localPosition = new Vector3((_width - 1) / -4f, (_height - 1) / 4f, 0);

			return tiles;
		}

		public Game.TileType[,] updateCollisionMap() {

            for (int x = 0; x < _width; x++) {

                for (int y = 0; y < _height; y++) {

                    int tile = _levelData[x, y];

                    if (tile == 1) {

                        // Ice.
                        _collisionMap[x, y] = Game.TileType.Ice;
                    }
                    else if (tile == 2) {

                        _collisionMap[x, y] = Game.TileType.Ladder;
                    }
                    else if (tile == 4) {

                        _collisionMap[x, y] = Game.TileType.Walkable;
                    }
                    else {

                        // Anything else.
                        _collisionMap[x, y] = Game.TileType.Solid;
                    }
                }
            }

			return _collisionMap;
		}

		public abstract Vector2 getStartingPoint();
	}
}