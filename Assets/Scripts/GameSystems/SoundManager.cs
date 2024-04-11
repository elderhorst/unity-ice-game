using System.Collections.Generic;
using UnityEngine;

namespace IceGame
{
	public class SoundManager : MonoBehaviour
	{
		private const string MusicVolumeKey = "MusicVolume";
		private const string EffectsVolumeKey = "EffectsVolume";

		private static SoundManager _instance;
		private AudioSource _musicSource;
		private List<AudioSource> _effects;

		private float _musicVolume;
		private float _effectVolume;

		public static SoundManager Instance
		{
			get
			{
				if (_instance == null)
				{
					GameObject obj = new GameObject();
					_instance = obj.AddComponent<SoundManager>();
				}
 
				return _instance;
			}
		}

		public float MusicVolume
		{
			get { return _musicVolume; }

			set
			{
				_musicVolume = Mathf.Clamp01(value);

				if (_musicSource != null) {

					_musicSource.volume = _musicVolume;
				}

				PlayerPrefs.SetFloat(MusicVolumeKey, _musicVolume);
			}
		}

		public float EffectVolume
		{
			get { return _effectVolume; }

			set
			{
				_effectVolume = Mathf.Clamp01(value);

				foreach (AudioSource effect in _effects)
				{
					effect.volume = _effectVolume;
				}

				PlayerPrefs.SetFloat(EffectsVolumeKey, _effectVolume);
			}
		}

		private void Awake()
		{
			gameObject.name = "SoundManager";
			DontDestroyOnLoad(this);

			_effects = new List<AudioSource>();

			_musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
			_effectVolume = PlayerPrefs.GetFloat(EffectsVolumeKey, 1f);
		}

		private void Update()
		{
			for (int i = 0; i < _effects.Count; i++)
			{
				if (!_effects[i].isPlaying)
				{
					Destroy(_effects[i]);
					_effects.RemoveAt(i);

					i--;
				}
			}
		}

		public void PlayMusic(string name)
		{
			AudioClip clip = Resources.Load<AudioClip>("Sounds/Music/" + name);
			
			if (_musicSource != null)
			{

				if (clip == _musicSource.clip)
				{
					return;
				}

				_musicSource.Stop();
			}
			else
			{
				_musicSource = gameObject.AddComponent<AudioSource>();
			}

			_musicSource.clip = clip;
			_musicSource.loop = true;
			_musicSource.volume = MusicVolume;
			_musicSource.Play();
		}

		public void PlayEffect(string name)
		{
			AudioClip clip = Resources.Load<AudioClip>("Sounds/Effects/" + name);
			AudioSource source = gameObject.AddComponent<AudioSource>();

			source.clip = clip;
			source.volume = EffectVolume;
			source.Play();

			_effects.Add(source);
		}
	}
}
