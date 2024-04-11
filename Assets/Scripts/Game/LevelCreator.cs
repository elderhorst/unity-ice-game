using UnityEngine;

namespace IceGame
{
	public abstract class LevelCreator
	{
		protected int[,] _levelData;
		protected TileType[,] _collisionMap;

		protected int _width;
		protected int _height;
		
		public abstract Vector2 GetStartingPoint();

		public Tile[,] CreateTileGameObjects(Transform container, Sprite[] sprites)
		{
            Tile[,] tiles = new Tile[_width, _height];
            
            for (int x = 0; x < _width; x++)
			{
                for (int y = 0; y < _height; y++)
				{
                    GameObject obj = new GameObject();
                    obj.transform.parent = container;
                    obj.transform.localPosition = new Vector3(x / 2f, -y / 2f, 0);
                    obj.name = "Tile " + x + " " + y;

                    tiles[x,y] = obj.AddComponent<Tile>();
                    tiles[x,y].Init(sprites[_levelData[x, y]]);
                }
            }

			container.transform.localPosition = new Vector3((_width - 1) / -4f, (_height - 1) / 4f, 0);

			return tiles;
		}

		public TileType[,] UpdateCollisionMap()
		{
            for (int x = 0; x < _width; x++)
			{
                for (int y = 0; y < _height; y++)
				{
                    int tile = _levelData[x, y];

                    if (tile == 1)
					{
                        // Ice.
                        _collisionMap[x, y] = TileType.Ice;
                    }
                    else if (tile == 2)
					{
                        _collisionMap[x, y] = TileType.Ladder;
                    }
                    else if (tile == 4)
					{
                        _collisionMap[x, y] = TileType.Walkable;
                    }
                    else
					{
                        // Anything else.
                        _collisionMap[x, y] = TileType.Solid;
                    }
                }
            }

			return _collisionMap;
		}
	}
}