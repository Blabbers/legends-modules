using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

public class LegendsBuildPostProcess: IPreprocessBuildWithReport
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
    }

    private static void TransferLolSpecFile(string pathToBuiltProject)
    {
        Debug.Log($"Post build process: Will try to transfer lol_spec file.");
        var originalPath = $"{pathToBuiltProject}/StreamingAssets/lol_spec.json" ;
        var rootBuildPath = $"{pathToBuiltProject}/lol_spec.json";
        // Moves the file
        try
        {
            Directory.Move(originalPath, rootBuildPath);
            Debug.Log($"Post build process: Success on transferring lol_spec file.");
        }
        catch
        {
            Debug.Log($"<color=red>Post build process: The file 'lol_spec.json' was not found on the build directory.</color>");
        }
    }
}