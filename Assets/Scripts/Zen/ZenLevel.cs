using UnityEngine;
using System.Collections.Generic;

namespace Zen {

    public struct LevelData {
        public Game.Tile[,] grid;
    }

    public class ZenLevel : Game.Level {

        private LevelGenerator _levelCreator;

        private List<GameObject> _levels;
        private int _currentLevelIndex;
        private int _previousLevelIndex;

        private int _previousLevel;

        private void Start() {

            _levelCreator = new LevelGenerator();

            _levels = new List<GameObject> () { null, null };
            _currentLevelIndex = 0;

            _previousLevel = _currentLevel - 1;
            
            _player.setToTransparent();

            load(_currentLevel);
        }

        public override void load(int level) {

            // If the game has progressed to the next level, generate a new level.
            if (level != _previousLevel) {

                _levelCreator.generateNewLevel();

                _previousLevel = level;
            }

            // If this is the first level, don't set it up to slide transition in.
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

            _tiles = _levelCreator.createTileGameObjects(_levels[_currentLevelIndex].transform, _tileSprites);
            _collisionMap = _levelCreator.updateCollisionMap();

            // Position Player.
            Vector2 gridStart = _levelCreator.getStartingPoint();
            _player.transform.parent = _levels[_currentLevelIndex].transform;
            _player.transform.localPosition = new Vector3(0.5f * gridStart.x, -0.5f * gridStart.y, -0.5f);
            _player.setToTransparent();

            // Transition in the level.
            float duration = 1.25f;
            Vector3 levelOffset = new Vector3(16, 0, 0);
            _levels[_currentLevelIndex].transform.localPosition += levelOffset;

            Vector3 previousStart = _levels[_previousLevelIndex].transform.localPosition;
            Vector3 previousEnd = _levels[_previousLevelIndex].transform.localPosition - levelOffset;
            Vector3 currentStart = _levels[_currentLevelIndex].transform.localPosition;
            Vector3 currentEnd = _levels[_currentLevelIndex].transform.localPosition - levelOffset;

            StartCoroutine(Actions.ActionManager.translateObject(_levels[_previousLevelIndex], previousStart, previousEnd, duration));
            StartCoroutine(Actions.ActionManager.translateObject(_levels[_currentLevelIndex], currentStart, currentEnd, duration, () => {

                handleLevelFinishedEnterTransition();
            }));
        }

        public override void handleUiFinishedEnterTransition() {

            _activeLevel = true;
        }
        
        public override void handleFinishedLevel() {

            Game.SoundManager.Instance.playEffect("LevelComplete");

            _activeLevel = false;

            _player.fade(false, goToNextLevel);
        }

        public void handleLevelFinishedEnterTransition() {

            _player.fade(true, () => {

                _activeLevel = true;
            });
        }

        public void restartLevel() {

            _player.fade(false, () => {

                _player.reset();

                load(_currentLevel);
            });
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