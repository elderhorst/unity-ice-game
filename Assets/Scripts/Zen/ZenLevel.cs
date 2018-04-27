using UnityEngine;
using System.Collections.Generic;

namespace Zen {

    public struct LevelData {
        public Game.Tile[,] grid;
    }

    public class ZenLevel : Game.Level {

        private List<GameObject> _levels;
        private int _currentLevelIndex;
        private int _previousLevelIndex;

        private void Start() {

            _levels = new List<GameObject> () { null, null };
            _currentLevelIndex = 0;

            load(_currentLevel);
        }

        public override void load(int level) {

            LevelGenerator levelCreator = new LevelGenerator();
            levelCreator.generateNewLevel();

            if (_levels[_currentLevelIndex] == null) {

                createLevelContianer();
            }
            else {

                _previousLevelIndex = _currentLevelIndex;
                _currentLevelIndex = (_currentLevelIndex == 0) ? 1 : 0;

                if (_levels[_currentLevelIndex] != null) {

                    _player.transform.parent = _tileContainer;

                    Destroy(_levels[_currentLevelIndex]);
                }

                createLevelContianer();
            }

            _tiles = levelCreator.createTileGameObjects(_levels[_currentLevelIndex].transform, _tileSprites);
            _collisionMap = levelCreator.updateCollisionMap();

            // Position Player.
            Vector2 gridStart = levelCreator.getStartingPoint();
            _player.transform.parent = _levels[_currentLevelIndex].transform;
            _player.transform.localPosition = new Vector3(0.5f * gridStart.x, -0.5f * gridStart.y, -0.5f);
            _player.setToTransparent();

            // Transition in level if it is not the first level.
            if (_currentLevel != 1) {

                float duration = 1.25f;
                Vector3 levelOffset = new Vector3(16, 0, 0);
                _levels[_currentLevelIndex].transform.localPosition += levelOffset;

                Vector3 previousStart = _levels[_previousLevelIndex].transform.localPosition;
                Vector3 previousEnd = _levels[_previousLevelIndex].transform.localPosition - levelOffset;
                Vector3 currentStart = _levels[_currentLevelIndex].transform.localPosition;
                Vector3 currentEnd = _levels[_currentLevelIndex].transform.localPosition - levelOffset;

                StartCoroutine(Actions.ActionManager.translateObject(_levels[_previousLevelIndex], previousStart, previousEnd, duration));
                StartCoroutine(Actions.ActionManager.translateObject(_levels[_currentLevelIndex], currentStart, currentEnd, duration, () => {

                    handleUiFinishedEnterTransition();
                }));
            }
        }

        public override void handleUiFinishedEnterTransition() {

            _player.fade(true, () => {

                _activeLevel = true;
            });
        }

        public override void handleFinishedLevel() {

            _activeLevel = false;

            _player.fade(false, goToNextLevel);
        }

        protected override void goToNextLevel() {

            _player.reset();
            _currentLevel++;

            load(_currentLevel);
        }

        private void createLevelContianer() {

            GameObject obj = new GameObject();
            obj.transform.parent = _tileContainer;
            obj.transform.localPosition = Vector3.zero;
            obj.name = "Level" + _currentLevel.ToString();

            _levels[_currentLevelIndex] = obj;
        }
    }
}