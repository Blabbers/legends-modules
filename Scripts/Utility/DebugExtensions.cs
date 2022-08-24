namespace UnityEngine
{
    public static class DebugExtensions
    {
        public static string Colored(this string str, string color = "cyan")
        {
            color = color.ToLower();
            return ($"<color={color}>{str}</color>");
        }
    }
}