using UnityEngine;
using System.Collections.Generic;

namespace Zen {

    public class PathSolver {

        // Up, Right, Down, Left
		private readonly Vector2[] _deltas = { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };

		private bool _endFound;

        public PathSolver() {

        }

        public bool solveInLessThan(Path path, int maxMoves) {

            _endFound = false;

			List<Vector2> points = new List<Vector2>();
			points.Add(path.StartPoint);

			findNextPointsFromPoint(path, points, 0, maxMoves);

            return _endFound;
        }

        private void findNextPointsFromPoint(Path path, List<Vector2> startPoints, int moves, int maxMoves) {

            moves++;

			if (moves > maxMoves || _endFound) {

				return;
			}

            foreach (Vector2 startPoint in startPoints) {

                List<Vector2> nextPoints = new List<Vector2>();

                foreach (Vector2 delta in _deltas) {

                    Vector2? nextPoint = findNextPoint(path, startPoint, delta);

					if (nextPoint != null) {

						nextPoints.Add(nextPoint.Value);
					}
                    else if (_endFound) {

                        break;
                    }
                }

                if (_endFound) {

                    break;
                }

                findNextPointsFromPoint(path, nextPoints, moves, maxMoves);
            }
        }

		private Vector2? findNextPoint(Path path, Vector2 currentPosition, Vector2 delta) {

			while (true) {

				Vector2 nextPosition = currentPosition + delta;
				bool nextStepIsSolid = checkIfNextStepIsSolid(path, nextPosition);

				if (nextStepIsSolid) {
					
					if (currentPosition != path.StartPoint) {
					
						return currentPosition;
					}

					break;
				}
				else if (path.TileMap[(int)nextPosition.x, (int)nextPosition.y] == 2) {

					_endFound = true;

					break;
				}
				else {

					currentPosition = nextPosition;
				}
			}

			return null;
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