using UnityEngine;
using System.Collections.Generic;

namespace IceGame
{
	public struct Path
	{
		public int Width;
		public int Height;

		public int[,] PathMap;
		public int[,] TileMap;

		public Vector2 StartPoint;
		public Vector2 EndPoint;
		public Vector2 CurrentPosition;

		public Direction CurrentDirection;
		public Direction LastDirection;

		public bool MarkedAsFailed;

		private System.Random _randomGenerator;

		public Path(int width, int height, Vector2 startPoint)
		{
			Width = width;
			Height = height;

			PathMap = new int[Width, Height];
			TileMap = new int[Width, Height];

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					TileMap[x, y] = 1;
				}
			}

			StartPoint = startPoint;
			EndPoint = StartPoint;
			CurrentPosition = StartPoint;

			CurrentDirection = Direction.Null;
			LastDirection = Direction.Null;

			MarkedAsFailed = false;

			_randomGenerator = new System.Random();
		}

		public Direction ChooseRandomPathDirection(Direction ignore)
		{
			List<Direction> directions = new List<Direction>();

			// Get possible directions from the current position.
			if (CurrentPosition.y > 0 && ignore != Direction.North)
			{
				directions.Add(Direction.North);
			}
			if (CurrentPosition.y < Height - 1 && ignore != Direction.South)
			{
				directions.Add(Direction.South);
			}
			if (CurrentPosition.x > 0 && ignore != Direction.West)
			{
				directions.Add(Direction.West);
			}
			if (CurrentPosition.x < Width - 1 && ignore != Direction.East)
			{
				directions.Add(Direction.East);
			}

			if (directions.Count == 0)
			{
				return Direction.Null;
			}

			return directions[Random.Range(0, directions.Count)];
		}

		public List<Direction> GetRandomOrderOfDirections(Direction ignore)
		{
			List<Direction> directions = new List<Direction>();

			// Get possible directions from the current position.
			if (CurrentPosition.y > 0 && ignore != Direction.North)
			{
				directions.Add(Direction.North);
			}
			if (CurrentPosition.y < Height - 1 && ignore != Direction.South)
			{
				directions.Add(Direction.South);
			}
			if (CurrentPosition.x > 0 && ignore != Direction.West)
			{
				directions.Add(Direction.West);
			}
			if (CurrentPosition.x < Width - 1 && ignore != Direction.East)
			{
				directions.Add(Direction.East);
			}

			directions = Shuffle(directions);

			return directions;
		}

		private List<Direction> Shuffle(List<Direction> list)
		{
			int n = list.Count;

			while (n > 1)
			{  
				n--;  
				int k = _randomGenerator.Next(n + 1);  
				Direction value = list[k];  
				list[k] = list[n];  
				list[n] = value;  
			}

			return list;
		}
	}
}