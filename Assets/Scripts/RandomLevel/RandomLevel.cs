using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IceGame
{
    public class RandomLevel : Level
	{
		[SerializeField] private RandomLevelUi _levelUi;
		
        private RandomLevelGenerator _levelCreator;

        private List<GameObject> _levels;
        private int _currentLevelIndex;
        private int _previousLevelIndex;

        private int _previousLevel;

        private void Start()
		{
            _levelCreator = new RandomLevelGenerator();

            _levels = new List<GameObject> () { null, null };
            _currentLevelIndex = 0;

            _previousLevel = _currentLevel - 1;
            
            _player.SetToTransparent();
			_levelUi.SetLevelTextToTransparent();
			_levelUi.UpdateLevelText(_currentLevel);

            Load(_currentLevel);
        }

        public override void Load(int level)
		{
            if (level != _previousLevel)
			{
                _levelCreator.GenerateNewLevel();

                _previousLevel = level;
            }
			
			if (_levels[_currentLevelIndex] == null)
			{
                CreateCurrentLevelContianer();
            }
            else
			{
                _previousLevelIndex = _currentLevelIndex;
                _currentLevelIndex = (_currentLevelIndex == 0) ? 1 : 0;

                if (_levels[_currentLevelIndex] != null)
				{
                    _player.transform.parent = _tileContainer;

                    Destroy(_levels[_currentLevelIndex]);
                }

                CreateCurrentLevelContianer();
            }

            _tiles = _levelCreator.CreateTileGameObjects(_levels[_currentLevelIndex].transform, _tileSprites);
            _collisionMap = _levelCreator.UpdateCollisionMap();

            PositionPlayer();
            TransitionToLevel();
        }
		
		private void CreateCurrentLevelContianer()
		{
            GameObject obj = new GameObject();
            obj.transform.parent = _tileContainer;
            obj.transform.localPosition = Vector3.zero;
            obj.name = "Level" + _currentLevel.ToString();

            _levels[_currentLevelIndex] = obj;
        }
		
		private void PositionPlayer()
		{
			Vector2 gridStart = _levelCreator.GetStartingPoint();
            _player.transform.parent = _levels[_currentLevelIndex].transform;
            _player.transform.localPosition = new Vector3(0.5f * gridStart.x, -0.5f * gridStart.y, -0.5f);
            _player.SetToTransparent();
		}
		
		private async void TransitionToLevel()
		{
			float duration = 1.25f;
            Vector3 levelOffset = new Vector3(16, 0, 0);
            _levels[_currentLevelIndex].transform.localPosition += levelOffset;

            Vector3 previousStart = _levels[_previousLevelIndex].transform.localPosition;
            Vector3 previousEnd = _levels[_previousLevelIndex].transform.localPosition - levelOffset;
            Vector3 currentStart = _levels[_currentLevelIndex].transform.localPosition;
            Vector3 currentEnd = _levels[_currentLevelIndex].transform.localPosition - levelOffset;

            Task moveOldLevel = Animate.TranslateObject(_levels[_previousLevelIndex], previousStart, previousEnd, duration);
            Task moveNewLevel = Animate.TranslateObject(_levels[_currentLevelIndex], currentStart, currentEnd, duration);
			Task animateText = AnimateLevelText(duration);
			
			await Task.WhenAll(moveOldLevel, moveNewLevel, animateText);
			
			HandleLevelFinishedEnterTransition();
		}
		
		private async Task AnimateLevelText(float duration)
		{
			await _levelUi.FadeLevelText(false, duration / 2f);
			_levelUi.UpdateLevelText(_currentLevel);
			await _levelUi.FadeLevelText(true, duration / 2f);
		}

        public override void HandleUiFinishedEnterTransition()
		{
            _activeLevel = true;
        }
        
        public override async void HandleFinishedLevel()
		{
            SoundManager.Instance.PlayEffect("LevelComplete");

            _activeLevel = false;

            await _player.Fade(false);
			
			GoToNextLevel();
        }

        public async void HandleLevelFinishedEnterTransition()
		{
			await _player.Fade(true);
			
			_levelUi.UpdateLevelText(_currentLevel);
			
			_activeLevel = true;
        }

        public async void RestartLevel()
		{
			await _player.Fade(false);
			
			_player.Reset();

            Load(_currentLevel);
        }

        protected override void GoToNextLevel()
		{
            _player.Reset();
            _currentLevel++;

            Load(_currentLevel);
        }
    }
}