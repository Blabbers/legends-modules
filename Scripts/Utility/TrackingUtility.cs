using UnityEngine;

public static class TrackingUtility
{
    public static void CreateTrackingPoint(Vector3 pos, Transform parent= null, string name = "Track")
    {
#if UNITY_EDITOR
        //CreateTrackingPoint(pos, "Track", parent);
        CreateTrackingPoint(pos, IconManagerAlternative.Icon.CircleBlue, parent, name);
#endif
    }

    public static void CreateTrackingPoint(Vector3 pos, string name, Transform parent = null)
    {
#if UNITY_EDITOR
        if (!Application.isEditor) return;
        GameObject temp = new GameObject(name);
        temp.transform.SetParent(parent);
        temp.transform.position = pos;

        IconManagerAlternative.DrawIcon(temp, IconManagerAlternative.Icon.CircleBlue);
#endif
    }

    public static void CreateAltTrackingPoint(Vector3 pos, Transform parent = null, string name = "TrackAlt")
    {
#if UNITY_EDITOR
        CreateTrackingPoint(pos, IconManagerAlternative.Icon.CircleOrange, parent, name);
#endif
    }

#if UNITY_EDITOR
    private static void CreateTrackingPoint(Vector3 pos, IconManagerAlternative.Icon icon, Transform parent = null, string name = "Track")
	{
        if (!Application.isEditor) return;
        GameObject temp = new GameObject(name);
        temp.transform.SetParent(parent);
        temp.transform.position = pos;

        IconManagerAlternative.DrawIcon(temp, icon);
    }
#endif
}

