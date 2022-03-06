using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Blabbers.Game00
{
	public class AudioController : MonoBehaviour, ISingleton
	{
		private AudioController Instance => Singleton.Get<AudioController>();

		public AudioSource musicSource;
		public AudioSource gameplaySource;

		[BoxGroup("Game Audio")]
		//Music
		public AudioClip gameplayMusic, lobbyMusic;

		void ISingleton.OnCreated()
		{
		}

		public void PlayGameplayMusic()
		{
			PlayMusic(gameplayMusic);
		}

		public void PlayLobbyMusic()
		{
			PlayMusic(lobbyMusic);
		}

		private void PlayMusic(AudioClip musicClip)
		{
			if (musicSource.clip && musicSource.isPlaying)
			{
				Instance.musicSource.DOFade(0f, 0.5f).OnComplete(Execute);
			}
			else
			{
				Execute();
			}

			void Execute()
			{
				Instance.musicSource.Stop();
				Instance.musicSource.volume = 0.2f;
				Instance.musicSource.clip = musicClip;
				Instance.musicSource.loop = true;
				Instance.musicSource.Play();
			}
		}

		public void StopFadeOutMusic()
		{
			Instance.musicSource.DOFade(0f, 0.5f).OnComplete(() =>
			{
				Instance.musicSource.Stop();
				Instance.musicSource.clip = null;
			});
		}

		public void FadeMusicVolume(float targetVolume, float duration = 0.5f)
		{
			Instance.musicSource.DOFade(targetVolume, duration);
		}

		//Gameplay
		public void ResetGameplayPitch()
		{
			Instance.gameplaySource.pitch = 1f;
		}

		public void PlayGameplayClip(AudioClip clip, float volumeScale = 1f)
		{
			Instance.gameplaySource.PlayOneShot(clip, volumeScale);
		}

		public void Mute(bool value)
		{
			Instance.gameplaySource.mute = value;
			Instance.musicSource.mute = value;
		}
	}
}