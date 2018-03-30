using UnityEngine;

namespace Zen {

    public struct LevelData {
        public Game.Tile[,] grid;
    }

    public class ZenLevel : Game.Level {

        private void Start() {

            load(_currentLevel);
        }

        public override void load(int level) {

            LevelGenerator levelCreator = new LevelGenerator();
            levelCreator.generateNewLevel();

            _tiles = levelCreator.createTileGameObjects(_tileContainer, _tileSprites);
            _collisionMap = levelCreator.updateCollisionMap();

            // Position Player.
            Vector2 gridStart = levelCreator.getStartingPoint();
            _player.transform.localPosition = new Vector3(0.5f * gridStart.x, -0.5f * gridStart.y, -0.5f);
        }

        protected override void goToNextLevel() {

            foreach (Game.Tile tile in _tiles) {

                Destroy(tile.gameObject);
            }

            _player.reset();
            _currentLevel++;

            load(_currentLevel);
        }
    }
}