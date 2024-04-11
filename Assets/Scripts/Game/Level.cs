using UnityEngine;

namespace IceGame
{
	public abstract class Level : MonoBehaviour
	{
		[SerializeField] protected Player _player;
        [SerializeField] protected Sprite[] _tileSprites;
        [SerializeField] protected Transform _tileContainer;

		protected InputManager _inputManager;

		protected Tile[,] _tiles;
        protected TileType[,] _collisionMap;

		protected int _currentLevel = 1;

        protected bool _activeLevel = false;

		protected void Awake()
		{

			_inputManager = new InputManager();
		}

		protected void Update()
		{
            if (!_activeLevel || _player.IsMoving)
			{
                return;
            }
            else if (_player.IsOnLadder)
			{
                HandleFinishedLevel();
                return;
            }

            Movement movement = _inputManager.CheckForInput();

            if (movement != Movement.None)
			{
                _player.HandleMovement(movement, _collisionMap);
            }
        }

		public abstract void Load(int level);
        public abstract void HandleUiFinishedEnterTransition();
        public abstract void HandleFinishedLevel();

		protected abstract void GoToNextLevel();
	}
}