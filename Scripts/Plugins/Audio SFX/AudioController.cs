using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Blabbers.Game00
{
    public class AudioController : MonoBehaviour, ISingleton
    {
        public static AudioController Instance => Singleton.Get<AudioController>();

        public AudioSource musicSource;
        public AudioSource gameplaySource;
        public bool isMuted = false;
        public bool playLobbyMusicOnStart = false;

        private static float baseMusicVolume;
        private static float baseGameplayVolume;

        public static float MusicVolumeFull => baseMusicVolume;
        public static float MusicVolumeHalf => baseMusicVolume * 0.5f;
        public static float MusicVolumeLow => baseMusicVolume * 0.1f;

        [BoxGroup("Game Audio")]
        //Music
        public AudioClip gameplayMusic,
            lobbyMusic;
        public Action<bool> OnMuteSFX;

        private void Awake()
        {
            baseMusicVolume = musicSource.volume;
            baseGameplayVolume = gameplaySource.volume;
        }

        void Start()
        {
            if (playLobbyMusicOnStart)
            {
                PlayLobbyMusic();
            }
        }

        void ISingleton.OnCreated() { }

        public void PlayGameplayMusic()
        {
            Instance.PlayMusic(Instance.gameplayMusic);
        }

        public void PlayLobbyMusic()
        {
            Instance.PlayMusic(Instance.lobbyMusic);
        }

        public void PlayMusic(AudioClip musicClip)
        {
            if (musicClip.Equals(Instance.musicSource.clip))
                return;

            Instance.musicSource.Stop();
            Instance.musicSource.volume = baseMusicVolume;
            Instance.musicSource.clip = musicClip;
            Instance.musicSource.loop = true;
            Instance.musicSource.Play();
        }

        public void StopFadeOutMusic()
        {
            Instance
                .musicSource.DOFade(0f, 0.5f)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    Instance.musicSource.Stop();
                    Instance.musicSource.clip = null;
                });
        }

        public void FadeMusicVolume(float targetVolume, float duration = 0.5f)
        {
            Instance.musicSource.DOFade(targetVolume, duration).SetUpdate(true);
        }

        public void FadeResetMusicVolume(float duration = 0.5f)
        {
            Instance.musicSource.DOFade(baseMusicVolume, duration).SetUpdate(true);
        }

        public void FadeGameplayVolume(float targetVolume, float duration = 0.5f)
        {
            Instance.gameplaySource.DOFade(targetVolume, duration).SetUpdate(true);
        }

        public void FadeResetGameplayVolume(float duration = 0.5f)
        {
            Instance.gameplaySource.DOFade(baseGameplayVolume, duration).SetUpdate(true);
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
            OnMuteSFX?.Invoke(value);
            MuteCheck();
        }

        void MuteCheck()
        {
            if (Instance.gameplaySource.mute && Instance.musicSource.mute)
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
