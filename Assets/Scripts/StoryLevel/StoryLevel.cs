using UnityEngine;

namespace IceGame {

    public struct LevelData {
        public Tile[,] grid;
    }

    public class StoryLevel : Level {
        
        private const int _lastLevel = 6;

        private void Start() {

            Load(_currentLevel);
        }

        public override void Load(int level) {

            StoryLevelLoader levelLoader = new StoryLevelLoader(level);

            _tiles = levelLoader.CreateTileGameObjects(_tileContainer, _tileSprites);
            _collisionMap = levelLoader.UpdateCollisionMap();

            // Position Player.
            Vector2 gridStart = levelLoader.GetStartingPoint();
            _player.transform.localPosition = new Vector3(0.5f * gridStart.x, -0.5f * gridStart.y, -0.5f);
        }

        public override void HandleUiFinishedEnterTransition() {

            _activeLevel = true;
        }

        public override void HandleFinishedLevel() {

            SoundManager.Instance.PlayEffect("LevelComplete");

            GoToNextLevel();
        }

        protected override void GoToNextLevel() {

            foreach (Tile tile in _tiles) {

                Destroy(tile.gameObject);
            }

            _player.Reset();
            _currentLevel++;

            if (_currentLevel > _lastLevel) {

                _currentLevel = 1;
            }

            Load(_currentLevel);
        }
    }
}