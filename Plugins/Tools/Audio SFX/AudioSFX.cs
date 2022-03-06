using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Blabbers.Game00
{
	[CreateAssetMenu]
	public class AudioSFX : ScriptableObject
	{
		[SerializeField, ReorderableList]
		private List<AudioClip> audioClips = new List<AudioClip>();
		[SerializeField, Range(0f, 1f)]
		private float volumeScale = 1f;
		/// <summary>
		/// Randomizes audioclip play between variations
		/// </summary>
		public void Play()
		{
			if (audioClips.Count > 0)
			{
				var clip = audioClips[Random.Range(0, audioClips.Count)];
				Singleton.Get<AudioController>().PlayGameplayClip(clip, volumeScale);
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
				Singleton.Get<AudioController>().PlayGameplayClip(clip, volumeScale);
			}
		}
	}
}