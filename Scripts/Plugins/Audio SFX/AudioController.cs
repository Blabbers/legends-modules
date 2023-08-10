using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using static Animancer.Validate;

namespace Blabbers.Game00
{
    public class AudioController : MonoBehaviour, ISingleton
    {
        public static AudioController Instance => Singleton.Get<AudioController>();

        public AudioSource musicSource;
        public AudioSource gameplaySource;
        public bool isMuted = false;

        private float baseMusicVolume;
        private float baseGameplayVolume;

        [BoxGroup("Game Audio")]
        //Music
        public AudioClip gameplayMusic, lobbyMusic;

        private void Awake()
        {
            baseMusicVolume = musicSource.volume;
            baseGameplayVolume = gameplaySource.volume;
        }

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
                Instance.musicSource.volume = baseMusicVolume;
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

        public void FadeResetMusicVolume(float duration = 0.5f)
        {
            Instance.musicSource.DOFade(baseMusicVolume, duration);
        }

        public void FadeGameplayVolume(float targetVolume, float duration = 0.5f)
        {
            Instance.gameplaySource.DOFade(targetVolume, duration);
        }

        public void FadeResetGameplayVolume(float duration = 0.5f)
        {
            Instance.gameplaySource.DOFade(baseGameplayVolume, duration);
        }


        //Gameplay
        public void ResetGameplayPitch()
        {
            Instance.gameplaySource.pitch = 1f;
        }

        public Dictionary<AudioSFX, float> EndTimeBySFX = new Dictionary<AudioSFX, float>();
        public void PlayGameplayClip(AudioSFX sfx, AudioClip clip, float volumeScale = 1f)
        {
            if (!EndTimeBySFX.ContainsKey(sfx) || Time.time > EndTimeBySFX[sfx])
            {
                EndTimeBySFX[sfx] = Time.time + clip.length;
            }
            else if (sfx.onlyPlayOnceEachTime)
            {
                // We shouldnt play this SFX if one from the same type was recently played 
                //Debug.Log("We should not play yet");
                return;
            }
            Instance.gameplaySource.PlayOneShot(clip, volumeScale);
        }

        public void Mute(bool value)
        {
            //Debug.Log($"AudioController.Mute({value})");

            isMuted = value;

			Instance.gameplaySource.mute = value;
            Instance.musicSource.mute = value;
        }


        public void MuteMusic(bool value)
        {
            Instance.musicSource.mute = value;
			MuteCheck();
		}

		public void MuteSFX(bool value)
		{
			Instance.gameplaySource.mute = value;
            MuteCheck();
		}


        void MuteCheck()
        {

            if(Instance.gameplaySource.mute && Instance.musicSource.mute)
            {
				isMuted = true;
            }
            else
            {
                isMuted = false;
            }
        }
	}
}