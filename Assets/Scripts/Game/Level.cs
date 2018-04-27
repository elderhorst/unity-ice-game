using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {

	public abstract class Level : MonoBehaviour {

		[SerializeField] protected Player _player;
        [SerializeField] protected Sprite[] _tileSprites;
        [SerializeField] protected Transform _tileContainer;

		protected Managers.InputManager _inputManager;

		protected Game.Tile[,] _tiles;
        protected Game.TileType[,] _collisionMap;

		protected int _currentLevel = 1;

        protected bool _activeLevel = false;

		protected void Awake() {

			_inputManager = new Managers.InputManager();
		}

		protected void Update() {

            if (!_activeLevel || _player.IsMoving) {

                return;
            }
            else if (_player.IsOnLadder) {

                handleFinishedLevel();
                return;
            }

            Movement movement = _inputManager.checkForInput();

            if (movement != Movement.None) {

                _player.handleMovement(movement, _collisionMap);
            }
        }

		public abstract void load(int level);
        public abstract void handleUiFinishedEnterTransition();
        public abstract void handleFinishedLevel();

		protected abstract void goToNextLevel();
	}
}