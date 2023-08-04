using Animancer;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class StreamingAssetsManager : MonoBehaviour
{
	#region Init

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	static void Init()
	{
		_instance = null;
	}

	#endregion


	[SerializeField] int languageId;
	//[SerializeField] string folderPath;
	[SerializeField] List<string> audioFileNames = new List<string>();
	[SerializeField] List<string> audioLoadKeys = new List<string>();

	[SerializeField] List<AudioClip> audioClips = new List<AudioClip>();


	Dictionary<string, int> keyValuePairs = new Dictionary<string, int>();

	#region Awake
	private void Awake()
	{
		if (!_instance)
		{
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(this.gameObject);
			return;
		}


		//GetLanguageId
		GetAudioClips();
	} 

	void GetAudioClips()
	{
		string basePath = Application.streamingAssetsPath + "/Audio/EN/";

		if(languageId == 0)
		{
			basePath = Application.streamingAssetsPath + "/Audio/EN/";
		}
		else
		{
			basePath = Application.streamingAssetsPath + "/Audio/ES/";
		}

		string fullPath;

		//string fullPath = basePath + key + ".mp3"; //or ES for spanish, grab the language code needed from the start game payload.
		//string fullPath = Application.streamingAssetsPath + "/Audio/EN/" + key + ".mp3"; //or ES for spanish, grab the language code needed from the start game payload.


		for (int i = 0; i < audioFileNames.Count; i++)
		{
			fullPath = basePath + audioFileNames[i] + ".mp3";
			//keyValuePairs.Add(audioFileNames[i], i);
			keyValuePairs.Add(audioLoadKeys[i], i);

			StartCoroutine(LoadAudio(fullPath, audioFileNames[i]));
		}

		
	}



	private IEnumerator LoadAudio(string path, string key)
	{
		UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG); // Create the UnityWebRequest
		yield return www.SendWebRequest(); // Send the request


		if (www.result == UnityWebRequest.Result.Success) // Check if the request was successful
		{
			var _audioClip = DownloadHandlerAudioClip.GetContent(www); // Get the audio clip from the loaded data
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
		if(clip!=null) audioClips.Add(clip);
	}


	#endregion


	#region Instance
	private static StreamingAssetsManager _instance = null;
	public static StreamingAssetsManager Instance
	{
		get
		{
			if (!_instance)
			{
				_instance = Instantiate(Resources.Load<StreamingAssetsManager>("StreamingAssets_Manager"));
				_instance.gameObject.name = "--StreamingAssets_Manager";
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}
	#endregion

	#region MyRegion

	//[Button]
	//void LoadAllKeysOnFolder()
	//{


	//	if (Directory.Exists(folderPath))
	//	{
	//		audioFileNames.Clear();

	//		// Get the names of all the files in the folder
	//		string[] fileNames = Directory.GetFiles(folderPath);

	//		// Display the names of the files in the Unity console
	//		foreach (string fileName in fileNames)
	//		{
	//			Debug.Log("File Name: " + Path.GetFileName(fileName));
	//			string completeFileName = Path.GetFileName(fileName);


	//			//string fileNameOnly = Path.GetFileNameWithoutExtension(fileName);

	//			if (!completeFileName.Contains("meta"))
	//			{
	//				string fileNameOnly = Path.GetFileNameWithoutExtension(fileName);
	//				audioFileNames.Add(fileNameOnly);
	//			}


	//		}
	//	}
	//	else
	//	{
	//		Debug.LogError("Folder path is invalid or does not exist.");
	//	}
	//}

	#endregion


	//Editor
	public void SetUpDictionaries(List<string> loadNames, List<string> loadKeys)
	{
		audioFileNames = loadNames;
		audioLoadKeys = loadKeys;		
	}



	public AudioClip GetClipByKey(string key)
	{
		int id = 0;

		if (keyValuePairs.TryGetValue(key, out int value))
		{
			id = value;
		}
		else
		{
			return null;
		}
		return audioClips[id];
	}
}

