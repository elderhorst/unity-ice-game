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
			int maxAttempts = 500;

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

					break;
				}
			}

			return true;
		}

		private Path attemptToGenerateLine(Path existingPath) {

			Path path;

			List<Direction> directions = existingPath.getRandomOrderOfDirections(existingPath.LastDirection);

			for (int i = 0; i < directions.Count; i++) {

				path = existingPath;
				path.LastDirection = path.CurrentDirection;
				path.CurrentDirection = directions[i];

				Path lineAttempt = makeLine(path);

				if (!lineAttempt.MarkedAsFailed) {

					path = lineAttempt;
					path.LastDirection = path.CurrentDirection;

					return path;
				}
			}

			return existingPath;
		}

		private Path makeLine(Path path) {

			Vector2 step = getStepForDirection(path.CurrentDirection);

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
	}
}