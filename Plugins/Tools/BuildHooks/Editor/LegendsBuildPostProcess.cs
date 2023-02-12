using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class LegendsBuildPostProcess
{
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        // Not needed anymore, we're using the one that comes with the lolsdk editor demo
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