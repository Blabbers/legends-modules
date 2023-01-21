using System;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Blabbers.Game00
{
	[CreateAssetMenu]
	public class AudioSFX : ScriptableObject
	{
		[SerializeField, ReorderableList]
		private List<AudioClip> audioClips = new List<AudioClip>();
		[SerializeField, Range(0f, 1f)]
		private float volumeScale = 1f;
		public bool onlyPlayOnceEachTime;
		/// <summary>
		/// Randomizes audioclip play between variations
		/// </summary>
		public void Play()
		{
			if (audioClips.Count > 0)
			{
				var clip = audioClips[Random.Range(0, audioClips.Count)];
				Singleton.Get<AudioController>().PlayGameplayClip(this, clip, volumeScale);
			}
		}

		/// <summary>
		/// Selects a single clip (in a given index) to be played
		/// </summary>		
		public void PlaySelectedIndex(int index)
		{
			if (audioClips.Count > index)
			{
				var clip = audioClips[index];
				Singleton.Get<AudioController>().PlayGameplayClip(this, clip, volumeScale);
			}
		}

        #if UNITY_EDITOR
        [Button()]
        public void Preview()
        {
            var clip = audioClips[Random.Range(0, audioClips.Count)];
            EditorPlayClip(clip);
        }
        [Button()]
        public void Stop()
        {
            EditorStopAllClips();
        }

        
        public static void EditorPlayClip(AudioClip clip, int startSample = 0, bool loop = false)
        {
            System.Reflection.Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            System.Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            System.Reflection.MethodInfo method = audioUtilClass.GetMethod(
                "PlayClip",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
                null,
                new System.Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
                null
            );
            method.Invoke(
                null,
                new object[] { clip, startSample, loop }
            );
        }
        
        public static void EditorStopAllClips()
        {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass =
                unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod(
                "StopAllClips",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new System.Type[]{},
                null
            );
            method.Invoke(
                null,
                new object[] {}
            );
        }
        #endif
	}
}