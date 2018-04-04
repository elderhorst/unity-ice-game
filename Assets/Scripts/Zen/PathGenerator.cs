using UnityEngine;
using System.Collections.Generic;

namespace Zen {

	public enum Direction {North, East, South, West, Null}

	public class PathGenerator {

		Path _path;
		GenerationStatus _status;

		public PathGenerator() {

		}

		public int[,] generate(int width, int height, Vector2 startPoint) {

			_path = new Path(width, height, startPoint);
			_status = new GenerationStatus();
			
			bool createdValidPath = false;
			int attempts = 0;
			int maxAttempts = 1000;

			while (!createdValidPath) {
			
				createdValidPath = attemptToGeneratePath();

				attempts++;

				if (attempts >= maxAttempts) {

					Debug.LogWarning(width + " : " + height + " : " + startPoint);
					break;
				}
			}

			return _path.TileMap;
		}

		private bool attemptToGeneratePath() {

			// Create a copy of _path. As iterations are successfull path will be copied back to _path.
			Path masterPath = _path;
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

				_path = masterPath;
			}

			return isValid;
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

			if (path.StartPoint == path.EndPoint ||
				 _status.NewPathAttempts > _status.PathAttempLimit ||
				 path.TileMap[(int)path.StartPoint.x, (int)path.StartPoint.y] == 2 ||
				 path.TileMap[(int)path.StartPoint.x, (int)path.StartPoint.y] == 3) {

				return false;
			}

			return true;
		}
	}
}