using UnityEngine;
using System.Collections.Generic;

namespace Zen {

	//public enum Direction {North, East, South, West, Null}

	public class PathGeneratorOld {

		Path _path;
		GenerationStatus _status;

		public PathGeneratorOld() {

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

			//_status.reset();

			while (!finishedGeneratingPath) {

				if (_status.NewPathAttempts > _status.PathAttempLimit) {

					break;
				}

				Direction direction = masterPath.chooseRandomPathDirection(masterPath.LastDirection);
				Vector2 step = getStepForDirection(direction);

				if (direction == Direction.Null) {

					_status.NewPathAttempts++;
					continue;
				}
				else {

					masterPath.LastDirection = direction;
				}

				Path progressPath = attemptToAddLine(masterPath, step);

				if (isNewLineValid(progressPath, step)) {

					masterPath = progressPath;

					_status.CurrentPathCount++;
					_status.NewPathAttempts = 0;

					finishedGeneratingPath = changeTileAtEndOfLine(masterPath, step);
				}
				else {

					_status.NewPathAttempts++;
				}
			}

			return isPathValid(masterPath);
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

		private Path attemptToAddLine(Path existingPath, Vector2 step) {

			Path path = existingPath;
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
				/*else if (!checkIfNextStepIsSolid(path, nextPosition) &&
					path.PathMap[(int)(nextPosition.x), (int)(nextPosition.y)] == 1) {

					Debug.Log("Crossing paths");
					//continue;
				}*/
				else if (_status.CurrentPathCount > _status.MinPathCount && Random.value < 0.20f) {

					atEndOfLine = true;

					if (!checkIfNextStepIsSolid(path, nextPosition)) {

						path.TileMap[(int)(nextPosition.x), (int)(nextPosition.y)] = 3;
					}
				}
			}

			return path;
		}

		private bool isNewLineValid(Path path, Vector2 step) {

			Vector2 nextPosition = path.CurrentPosition + step;
			bool keepChanges = false;

			// If the next step is out of bounds, the path can't create a conflict.
			// If the next step is an existing path, placing a solid object will break the puzzle.
			if ((nextPosition.x >= 0 && nextPosition.x < path.Width &&
					nextPosition.y >= 0 && nextPosition.y < path.Height) &&
				(path.TileMap[(int)nextPosition.x, (int)nextPosition.y] != 3)) {

				keepChanges = true;
			}

			if (keepChanges) {

				_status.NewPathAttempts = 1;
			}
			else {

				_status.NewPathAttempts++;
				_status.CurrentPathCount--;
			}

			return keepChanges;
		}

		private bool changeTileAtEndOfLine(Path path, Vector2 step) {

			bool placedLadder = false;

			// End puzzle and place ladder, or try to place solid object and continue.
			if (/*_status.CurrentPathCount >= _status.MinPathCount && */Random.value < 0.20f) {

				Debug.Log("DONEEEEEEEEEEE");

				path.TileMap[(int)path.EndPoint.x, (int)path.EndPoint.y] = 2;

				placedLadder = true;
			}
			else {

				if (!checkIfNextStepIsSolid(path, path.CurrentPosition + step)) {

					path.TileMap[(int)(path.CurrentPosition.x + step.x), (int)(path.CurrentPosition.y + step.y)] = 3;
				}

				_status.CurrentPathCount++;
			}

			return placedLadder;
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

		private bool checkIfNextStepIsSolid(Path path, Vector2 nextPosition) {

			if (nextPosition.x < 0 || nextPosition.y < 0 || nextPosition.x >= path.Width || nextPosition.y >= path.Height) {

				return true;
			}

			if (path.TileMap[(int)nextPosition.x, (int)nextPosition.y] == 3) {

				return true;
			}

			return false;
		}
	}
}