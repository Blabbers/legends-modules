namespace UnityEngine
{
    public static class DebugExtensions
    {
        //public static void LogColored(this string str, string color = "cyan")
        //{
        //    //Debug.Log("<color=cyan> ToggleLockedInput() →  </color>" + sokobanMiniGame.robot.transform.position);
        //    Debug.Log($"<color={color}>{str}</color>");
        //}


        public static string Colored(this string str, string color = "cyan")
        {
            return ($"<color={color}>{str}</color>");
        }
    }
}