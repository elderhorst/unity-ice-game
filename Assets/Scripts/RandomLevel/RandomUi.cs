using UnityEngine;
using UnityEngine.UI;

namespace IceGame
{
    public class RandomLevelUi : GameUi
	{
        [SerializeField] private Button _restartButton;
		[SerializeField] private RandomLevel _randomLevel;

        protected override void OnFinishedEnterTransition()
		{
			base.OnFinishedEnterTransition();

            _restartButton.onClick.AddListener(OnClickRestartButton);
		}
        
        private void OnClickRestartButton()
		{
			SoundManager.Instance.PlayEffect("ButtonClick");

			_randomLevel.RestartLevel();
		}
    }
}