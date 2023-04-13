using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

public class LegendsBuildPostProcess : IPreprocessBuildWithReport
{
	public int callbackOrder => -100000;

	public void OnPreprocessBuild(BuildReport report)
	{
		Debug.Log($"Pre build hook process.");
#if !UNITY_CLOUD_BUILD
		// Se a build é local (fora do Cloud), fazer aparecer um popup de contexto perguntando se as infos do GAME DATA estao corretas.        
		if (EditorUtility.DisplayDialog($"Legends of Learning platform. Is the GameData correct?",
				$"AppID: {GameData.Instance.applicationID}" +
				$"\nMaxProgress: {GameData.Instance.maxProgress}" +
				$"\nTotal Levels: {GameData.Instance.totalLevels}",
				"No, it is wrong. I will fix it now.", "Yes it looks right!"))
		{
			EditorGUIUtility.PingObject(GameData.Instance);
			Selection.activeObject = GameData.Instance;
			throw new UnityEditor.Build.BuildFailedException("Go fix the GameData values!");
		}
#endif
	}

	[PostProcessBuild(1)]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
	{
		Debug.Log($"Post build hook process.");
		// Later I will make the ZIP file again, because the behaviour below is not necessary anymore.
		//TransferLolSpecFile(pathToBuiltProject);
#if !UNITY_CLOUD_BUILD
		EditBuildIndexFile(pathToBuiltProject);
#endif
	}

	private static void TransferLolSpecFile(string pathToBuiltProject)
	{
		Debug.Log($"Post build process: Will try to transfer lol_spec file.");
		var originalPath = $"{pathToBuiltProject}/StreamingAssets/lol_spec.json";
		var rootBuildPath = $"{pathToBuiltProject}/lol_spec.json";
		// Moves the file
		try
		{
			System.IO.Directory.Move(originalPath, rootBuildPath);
			Debug.Log($"Post build process: Success on transferring lol_spec file.");
		}
		catch
		{
			Debug.Log($"<color=red>Post build process: The file 'lol_spec.json' was not found on the build directory.</color>");
		}
	}

	/*[MenuItem("Blabbers/Test Write on Index File")]
	private static void Test()
	{
		EditBuildIndexFile("D:\\Blabbers\\methodical-firefighter\\docs\\WebGL-Testando" + (UnityEngine.Random.Range(0, 102401024)));
	}*/
	private static void EditBuildIndexFile(string pathToBuiltProject)
	{
		var appPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "docs");
		appPath = appPath.Replace('/', Path.DirectorySeparatorChar);		
		var fileFullPath = Path.Combine(appPath, "index.html");
		fileFullPath = fileFullPath.Replace('/', Path.DirectorySeparatorChar);		

		// Fow now, if the index file does not exit, we just return
		if (!File.Exists(fileFullPath))
		{
			return;
		}

		var lines = new List<string>();
		var removedFolders = new List<string>();
		var gameBuildListLineIndex = -1;
		using (StreamReader reader = new StreamReader(fileFullPath))
		{
			var foundGameBuildList = false;
			var foundAllObjects = false;
			var line = "";
			var i = 0;
			while ((line = reader.ReadLine()) != null)
			{
				lines.Add(line);
				// Found the beginning of the object list
				if (!foundGameBuildList && line.Contains("let gameBuildFolderNames = ["))
				{
					foundGameBuildList = true;
					gameBuildListLineIndex = i;
					continue;
				}

				if (foundGameBuildList && !foundAllObjects)
				{
					// Found the end of the object list
					if (line.Contains("];"))
					{
						foundAllObjects = true;
						continue;
					}

					var directories = Directory.GetDirectories(appPath);
					var shouldBeRemoved = true;
					// Looks for folders that dont exist anymore
					foreach (var item in directories)
					{
						var savedFolderName = new DirectoryInfo(item).Name;
						if (line.Contains(savedFolderName))
						{
							shouldBeRemoved = false;
							break;
						}
					}
					
					// If we now know that it doesnt exist anymore, schedule it for removal
					if (shouldBeRemoved)
					{
						removedFolders.Add(line);
					}
				}

				i++;
			}
			reader.Close();
		}

		// We remove every line object with a build that had its folder removed
		foreach (var removedFolder in removedFolders)
		{		
			lines.Remove(removedFolder);
		}

		// Now we add the line for this new build
		var folder = new DirectoryInfo(pathToBuiltProject).Name;
		var date = DateTime.Now;
		var name = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Environment.UserName.ToLower());
		var newLine = "        {" + $"folder: '{folder}', date: '{date}', person: '{name}'" + "},";
		if (gameBuildListLineIndex >= 0)
		{
			lines.Insert(gameBuildListLineIndex + 1, newLine);
		}
		
		// Now we write it back to the file		
		using (StreamWriter writer = new StreamWriter(fileFullPath))
		{
			foreach (var line in lines)
			{
				writer.WriteLine(line);				
			}
		}

		Debug.Log($"[{folder}] build was added to index file.\n{newLine}");
	}
}