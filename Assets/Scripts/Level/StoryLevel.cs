using UnityEngine;

namespace Level {

    public struct LevelData {
        public Game.Tile[,] grid;
    }

    public class StoryLevel : Game.Level {
        
        private const int _lastLevel = 6;

        private void Start() {

            load(_currentLevel);
        }

        public override void load(int level) {

            StoryLevelLoader levelLoader = new StoryLevelLoader(level);

            _tiles = levelLoader.createTileGameObjects(_tileContainer, _tileSprites);
            _collisionMap = levelLoader.updateCollisionMap();

            // Position Player.
            Vector2 gridStart = levelLoader.getStartingPoint();
            _player.transform.localPosition = new Vector3(0.5f * gridStart.x, -0.5f * gridStart.y, -0.5f);
        }

        public override void handleUiFinishedEnterTransition() {

            _activeLevel = true;
        }

        public override void handleFinishedLevel() {

            Game.SoundManager.Instance.playEffect("LevelComplete");

            goToNextLevel();
        }

        protected override void goToNextLevel() {

            foreach (Game.Tile tile in _tiles) {

                Destroy(tile.gameObject);
            }

            _player.reset();
            _currentLevel++;

            if (_currentLevel > _lastLevel) {

                _currentLevel = 1;
            }

            load(_currentLevel);
        }
    }
}