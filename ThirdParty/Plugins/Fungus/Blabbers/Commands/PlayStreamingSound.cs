using Animancer;
using Blabbers.Game00;
using Fungus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;


[CommandInfo("Blabbers",
			 "Play Stream",
			 "Plays a Sound from the Streaming Assets")]
[AddComponentMenu("")]

public class PlayStreamingSound : Command
{
	//public string key;
	public List<string> keys;

	[Range(0, 1)]
	[Tooltip("Volume level of the sound effect")]
	[SerializeField] protected float volume = 1;

	[Tooltip("Wait until the sound has finished playing before continuing execution.")]
	[SerializeField] protected bool waitUntilFinished = true;
	[SerializeField] protected bool ignoreTimeScale = true;

	AudioClip _audioClip;
	AudioSource _audioSource;
	//string audioFilePath;

	public override void OnEnter()
	{
		if (keys == null) return;

		if(keys.Count > 0)
		{
			var id = Random.Range(0, keys.Count);

			Debug.Log($"PlayStreamingSound.OnEnter({keys[id]})");
			PostRequest(StreamingAssetsManager.Instance.GetClipByKey(keys[id]));
		}


	}

	//void Awake()
	//{
	//	string fullPath = Application.streamingAssetsPath + "/Audio/EN/" + audioFilePath; //or ES for spanish, grab the language code needed from the start game payload.
	//	StartCoroutine(LoadAudio(fullPath));
	//}


	private IEnumerator LoadAudio(string path, string key)
	{
		UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG); // Create the UnityWebRequest
		yield return www.SendWebRequest(); // Send the request


		if (www.result == UnityWebRequest.Result.Success) // Check if the request was successful
		{
			_audioClip = DownloadHandlerAudioClip.GetContent(www); // Get the audio clip from the loaded data
			_audioClip.name = key; // Set the name of the audio clip

			//_audioSource.clip = _audioClip; // Set the audio clip on the AudioSource component
			PostRequest(_audioClip);
		}
		else
		{
			Debug.LogError("Error loading audio file: " + www.error); // Log an error message if the request failed
			PostRequest(null);
		}

		www.Dispose(); // Clean up the UnityWebRequest object
	}


	void PostRequest(AudioClip clip)
	{
		if(clip == null)
		{
			Debug.LogError($"PlayStreamingSound: Clip not found\nFaltando nova implementacao");
			Continue();
		}
		else
		{
			var duration = clip.length;

			Debug.Log($"PlayStreamingSound.PostRequest({clip.name})");

			Fungus_StreamingAudioPlayer.Instance.PlayAudio(clip, volume);

			if (waitUntilFinished)
			{
				StartCoroutine(_Wait());
				IEnumerator _Wait()
				{
					if (ignoreTimeScale)
					{
						yield return new WaitForSecondsRealtime(duration);
						Continue();
					}
					else
					{

						var t = duration;
						while (t > 0)
						{
							t -= Time.deltaTime;
							yield return null;
						}

						Continue();
					}
				}
			}
			else
			{
				Continue();
			}
		}
	}

	#region Default
	public override Color GetButtonColor()
	{
		return new Color32(216, 228, 170, 255);
	}

	public override string GetSummary()
	{
		string namePrefix = $"Play Sound:";
		string suffix = "";

		if (keys == null || keys.Count==0)
		{
			namePrefix = "<color=red><b>*INSERT ONE OR MORE KEYS TO PLAY SOUND*</b></color>";
			return namePrefix;
		}
		else
		{
			if (string.IsNullOrEmpty(keys[0]))
			{
				namePrefix = "<color=red><b>*INSERT ONE OR MORE KEYS TO PLAY SOUND*</b></color>";
				return namePrefix;
			}
			else
			{
				//namePrefix = $"Play Sound: {keys[0]}";
				suffix = keys[0];
				return $"{namePrefix} {suffix}";
			}
		}


		////suffix = keys[0];

		if (keys.Count > 1)
		{

			for (int i = 1; i < keys.Count; i++)
			{
				suffix += $" or {keys[i]}";
			}
		}


		return $"{namePrefix} {suffix}";
	} 
	#endregion

}
