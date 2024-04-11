using UnityEngine;
using System.Collections.Generic;

namespace IceGame
{
	public enum Direction {North, East, South, West, Null}

	public class PathGenerator
	{
		GenerationStatus _status;

		public int[,] Generate(int width, int height, Vector2 startPoint)
		{
			Path? path;
			_status = new GenerationStatus();
			
			int attempts = 0;
			int maxAttempts = 1000;

			while (true)
			{
				path = AttemptToGeneratePath(width, height, startPoint);

				attempts++;

				if (path != null || attempts >= maxAttempts)
				{
					break;
				}
			}

			if (path == null)
			{
				Debug.LogWarning("Exceeded max number of attempts to create a path. Trying again.");

				return Generate(width, height, startPoint);
			}

			path = AddRandomObstacles(path.Value);

			return path.Value.TileMap;
		}

		private Path? AttemptToGeneratePath(int width, int height, Vector2 startPoint)
		{
			Path masterPath = new Path(width, height, startPoint);
			bool finishedGeneratingPath = false;

			_status.Reset();

			while (!finishedGeneratingPath)
			{
				if (_status.NewPathAttempts >= _status.PathAttempLimit)
				{
					break;
				}

				masterPath = AttemptToGenerateLine(masterPath);

				if (_status.CurrentPathCount >= _status.MinPathCount && Random.value <= 0.20f)
				{
					masterPath.TileMap[(int)masterPath.EndPoint.x, (int)masterPath.EndPoint.y] = 2;

					break;
				}
			}

			bool isValid = IsPathValid(masterPath);

			if (isValid)
			{
				return masterPath;
			}

			return null;
		}

		private Path AttemptToGenerateLine(Path existingPath)
		{
			Path path;

			List<Direction> directions = existingPath.GetRandomOrderOfDirections(existingPath.LastDirection);

			for (int i = 0; i < directions.Count; i++)
			{
				path = existingPath;
				path.LastDirection = path.CurrentDirection;
				path.CurrentDirection = directions[i];

				Path lineAttempt = MakeLine(path);

				_status.NewPathAttempts++;

				if (!lineAttempt.MarkedAsFailed)
				{
					path = lineAttempt;
					path.LastDirection = path.CurrentDirection;

					_status.CurrentPathCount++;

					return path;
				}
			}

			return existingPath;
		}

		private Path MakeLine(Path path)
		{
			Vector2 step = GetStepForDirection(path.CurrentDirection);

			bool atEndOfLine = false;

			while (!atEndOfLine)
			{
				path.PathMap[(int)path.CurrentPosition.x, (int)path.CurrentPosition.y] = 1;

				path.EndPoint = path.CurrentPosition;
				path.CurrentPosition += step;
				Vector2 nextPosition = path.CurrentPosition + step;

				if (CheckIfNextStepIsSolid(path, path.CurrentPosition))
				{
					path.CurrentPosition -= step;
					atEndOfLine = true;
				}
				else if (Random.value < 0.20f)
				{
					bool nextStepIsSolid = CheckIfNextStepIsSolid(path, nextPosition);

					if (!nextStepIsSolid && path.PathMap[(int)(nextPosition.x), (int)(nextPosition.y)] != 1)
					{
						path.TileMap[(int)(nextPosition.x), (int)(nextPosition.y)] = 3;

						atEndOfLine = true;
					}
					else if (nextStepIsSolid)
					{
						atEndOfLine = true;
					}
				}
			}

			return path;
		}

		private Vector2 GetStepForDirection(Direction direction)
		{
			Vector2 step = Vector2.zero;

			if (direction == Direction.North || direction == Direction.South)
			{
				step.y = (direction == Direction.South) ? 1 : -1;
			}
			else if (direction == Direction.West || direction == Direction.East)
			{
				step.x = (direction == Direction.East) ? 1 : -1;
			}

			return step;
		}

		private bool CheckIfNextStepIsSolid(Path path, Vector2 nextPosition)
		{
			if (nextPosition.x < 0 || nextPosition.y < 0 || nextPosition.x >= path.Width || nextPosition.y >= path.Height)
			{
				return true;
			}

			if (path.TileMap[(int)nextPosition.x, (int)nextPosition.y] == 3)
			{
				return true;
			}

			return false;
		}

		private bool IsPathValid(Path path)
		{
			// Check if there is something placed on the start position.
			if (path.StartPoint == path.EndPoint ||
				 _status.NewPathAttempts > _status.PathAttempLimit ||
				 path.TileMap[(int)path.StartPoint.x, (int)path.StartPoint.y] == 2 ||
				 path.TileMap[(int)path.StartPoint.x, (int)path.StartPoint.y] == 3)
			{

				return false;
			}

			// Check if the path can be solved in six or less moves.
			PathSolver solver = new PathSolver();

			if (solver.SolveInLessThan(path, 6))
			{
				return false;
			}

			return true;
		}

		private Path AddRandomObstacles(Path path) {

			int minRocks = 8;
			int maxRocks = 40;
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
	}
}