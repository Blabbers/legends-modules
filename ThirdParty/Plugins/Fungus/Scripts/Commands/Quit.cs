// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Events;

namespace Fungus
{
    /// <summary>
    /// Quits the application. Does not work in Editor or Webplayer builds. Shouldn't generally be used on iOS.
    /// </summary>
    [CommandInfo("Flow", 
                 "Quit", 
                 "Quits the application. Does not work in Editor or Webplayer builds. Shouldn't generally be used on iOS.")]
    [AddComponentMenu("")]
    public class Quit : Command 
    {
        #region Public members

        public override void OnEnter()
        {
            Application.Quit();

            // On platforms that don't support Quit we just continue onto the next command
            Continue();
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

        #endregion
    }
}