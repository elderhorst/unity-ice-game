using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace IceGame
{
    public class RandomLevelUi : GameUi
	{
        [SerializeField] private Button _restartButton;
		[SerializeField] private RandomLevel _randomLevel;
		[SerializeField] private TMPro.TextMeshProUGUI _levelText;
		
		public async Task FadeLevelText(bool fadeIn, float duration)
		{
			await Animate.FadeText(fadeIn, 0.3f, _levelText);
		}
		
		public void UpdateLevelText(int number)
		{
			_levelText.text = $"Level {number}";
		}
		
		public void SetLevelTextToTransparent()
		{
            Color color = _levelText.color;
            color.a = 0;

            _levelText.color = color;
        }

        protected override void OnFinishedEnterTransition()
		{
			base.OnFinishedEnterTransition();

            _restartButton.onClick.AddListener(OnClickRestartButton);
		}
        
        private void OnClickRestartButton()
		{
			SoundManager.Instance.PlayEffect(StringKey.ButtonClickEffect);

			_randomLevel.RestartLevel();
		}
    }
}