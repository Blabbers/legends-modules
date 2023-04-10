/* The Preview audio method constantly needs to be changed since we access it through reflection and Untiy sometimes changes its name
 * If it stopped working, follow this thread to see if there was any updated and change it accordingly
 * https://forum.unity.com/threads/way-to-play-audio-in-editor-using-an-editor-script.132042/
 */
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
			Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;

			Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
			MethodInfo method = audioUtilClass.GetMethod(
				"PlayPreviewClip",
				BindingFlags.Static | BindingFlags.Public,
				null,
				new Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
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

			Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
			MethodInfo method = audioUtilClass.GetMethod(
				"StopAllPreviewClips",
				BindingFlags.Static | BindingFlags.Public,
				null,
				new Type[] { },
				null
			);

			method.Invoke(
				null,
				new object[] { }
			);
		}
        #endif
	}
}