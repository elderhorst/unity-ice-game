using UnityEngine;
using UnityEngine.UI;

namespace IceGame
{
	public class SettingsMenu : SubMenu
	{
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _effectsSlider;

        private void Awake()
		{
            _musicSlider.value = SoundManager.Instance.MusicVolume;
            _effectsSlider.value = SoundManager.Instance.EffectVolume;
        }

        public override void EnableButtons()
		{
            base.EnableButtons();

            _musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
            _effectsSlider.onValueChanged.AddListener(OnEffectsValueChanged);
        }

        public override void DisableButtons()
		{
            base.DisableButtons();

            _musicSlider.onValueChanged.RemoveListener(OnMusicValueChanged);
            _effectsSlider.onValueChanged.RemoveListener(OnEffectsValueChanged);
        }

        private void OnMusicValueChanged(float value)
		{
            SoundManager.Instance.MusicVolume = value;
        }

        private void OnEffectsValueChanged(float value)
		{
            SoundManager.Instance.EffectVolume = value;
        }
    }
}