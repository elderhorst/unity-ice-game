using UnityEngine;
using System.Collections.Generic;

namespace Zen {

	public enum Direction {North, East, South, West, Null}

	public class PathGenerator {

		GenerationStatus _status;

		public PathGenerator() {

		}

		public int[,] generate(int width, int height, Vector2 startPoint) {

			Path? path;
			_status = new GenerationStatus();
			
			int attempts = 0;
			int maxAttempts = 1000;

			while (true) {
			
				path = attemptToGeneratePath(width, height, startPoint);

				attempts++;

				if (path != null || attempts >= maxAttempts) {

					break;
				}
			}

			if (path == null) {

				Debug.LogWarning("RECURSION");

				return generate(width, height, startPoint);
			}

			return path.Value.TileMap;
		}

		private Path? attemptToGeneratePath(int width, int height, Vector2 startPoint) {

			// Create a copy of _path. As iterations are successfull path will be copied back to _path.
			Path masterPath = new Path(width, height, startPoint);
			bool finishedGeneratingPath = false;

			_status.reset();

			while (!finishedGeneratingPath) {

				if (_status.NewPathAttempts >= _status.PathAttempLimit) {

					break;
				}

				masterPath = attemptToGenerateLine(masterPath);

				if (_status.CurrentPathCount >= _status.MinPathCount && Random.value <= 0.20f) {

					masterPath.TileMap[(int)masterPath.EndPoint.x, (int)masterPath.EndPoint.y] = 2;

					break;
				}
			}

			bool isValid = isPathValid(masterPath);

			if (isValid) {

				return masterPath;
			}

			return null;
		}

		private Path attemptToGenerateLine(Path existingPath) {

			Path path;

			List<Direction> directions = existingPath.getRandomOrderOfDirections(existingPath.LastDirection);

			for (int i = 0; i < directions.Count; i++) {

				path = existingPath;
				path.LastDirection = path.CurrentDirection;
				path.CurrentDirection = directions[i];

				Path lineAttempt = makeLine(path);

				_status.NewPathAttempts++;

				if (!lineAttempt.MarkedAsFailed) {

					path = lineAttempt;
					path.LastDirection = path.CurrentDirection;

					_status.CurrentPathCount++;

					return path;
				}
			}

			return existingPath;
		}

		private Path makeLine(Path path) {

			Vector2 step = getStepForDirection(path.CurrentDirection);

			bool atEndOfLine = false;

			while (!atEndOfLine) {

				path.PathMap[(int)path.CurrentPosition.x, (int)path.CurrentPosition.y] = 1;

				path.EndPoint = path.CurrentPosition;
				path.CurrentPosition += step;
				Vector2 nextPosition = path.CurrentPosition + step;

				if (checkIfNextStepIsSolid(path, path.CurrentPosition)) {

					path.CurrentPosition -= step;
					atEndOfLine = true;
				}
				else if (Random.value < 0.20f) {

					bool nextStepIsSolid = checkIfNextStepIsSolid(path, nextPosition);

					if (!nextStepIsSolid && path.PathMap[(int)(nextPosition.x), (int)(nextPosition.y)] != 1) {

						path.TileMap[(int)(nextPosition.x), (int)(nextPosition.y)] = 3;

						atEndOfLine = true;
					}
					else if (nextStepIsSolid) {

						atEndOfLine = true;
					}
				}
			}

			return path;
		}

		private Vector2 getStepForDirection(Direction direction) {

			Vector2 step = Vector2.zero;

			if (direction == Direction.North || direction == Direction.South) {

				step.y = (direction == Direction.South) ? 1 : -1;
			}
			else if (direction == Direction.West || direction == Direction.East) {

				step.x = (direction == Direction.East) ? 1 : -1;
			}

			return step;
		}

		private bool checkIfNextStepIsSolid(Path path, Vector2 nextPosition) {

			if (nextPosition.x < 0 || nextPosition.y < 0 || nextPosition.x >= path.Width || nextPosition.y >= path.Height) {

				return true;
			}

			if (path.TileMap[(int)nextPosition.x, (int)nextPosition.y] == 3) {

				return true;
			}

			return false;
		}

		private bool isPathValid(Path path) {

			// Check if there is something placed on the start position.
			if (path.StartPoint == path.EndPoint ||
				 _status.NewPathAttempts > _status.PathAttempLimit ||
				 path.TileMap[(int)path.StartPoint.x, (int)path.StartPoint.y] == 2 ||
				 path.TileMap[(int)path.StartPoint.x, (int)path.StartPoint.y] == 3) {

				return false;
			}

			// Check if the path can be solved in two or less moves.
			if (isSolvableInLessThanTwoMoves(path)) {

				return false;
			}

			return true;
		}

		private bool isStraightLineToEnd(Path path, Vector2 startPoint, Vector2 endPoint) {

			if (startPoint.x == endPoint.x || startPoint.y == endPoint.y) {

				int start = (startPoint.x == endPoint.x) ? (int)startPoint.x : (int)startPoint.y;
				int end = (startPoint.x == endPoint.x) ? (int)endPoint.x : (int)endPoint.y;
				
				Vector2 position = startPoint;
				Vector2 delta = (startPoint.x == endPoint.x) ? new Vector2(1, 0) : new Vector2(0, 1);

				int increment = (start <= end) ? 1 : -1;
				delta *= (start <= end) ? 1 : -1;

				bool obstacleInWay = false;

				for (int i = start; i != end; i += increment) {

					if (path.TileMap[(int)position.x, (int)position.y] == 3) {

						obstacleInWay = true;
						break;
					}

					position += delta;
				}

				if (!obstacleInWay) {

					return true;
				}
			}

			return false;
		}

		private bool isSolvableInLessThanTwoMoves(Path path) {

			// Up, Right, Down, Left
			Vector2[] deltas = { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };

			// Branch out in lines from the starting point.
			foreach (Vector2 firstDelta in deltas) {

				Vector2 currentPosition = path.StartPoint;

				while (true) {

					Vector2 nextPosition = currentPosition + firstDelta;
					bool nextStepIsSolid = checkIfNextStepIsSolid(path, nextPosition);

					if (nextStepIsSolid && currentPosition != path.StartPoint) {
						
						// Check if end can be gotten to from the end of this line.
						if (isStraightLineToEnd(path, currentPosition, path.EndPoint)) {

							return true;
						}

						break;
					}
					else if (nextStepIsSolid) {

						break;
					}
					else if (path.TileMap[(int)nextPosition.x, (int)nextPosition.y] == 2) {

						return true;
					}
					else {

						currentPosition = nextPosition;
					}
				}
			}

			return false;
		}
	}
}