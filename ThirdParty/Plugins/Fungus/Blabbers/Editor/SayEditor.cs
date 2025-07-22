// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEditor;
using UnityEngine;

namespace Fungus.EditorUtils
{
    [CustomEditor(typeof(Say))]
    public class SayEditor : CommandEditor
    {
        protected SerializedProperty characterProp;
        protected SerializedProperty portraitProp;

        public override void OnEnable()
        {
            base.OnEnable();

            //characterProp = serializedObject.FindProperty("character");
            portraitProp = serializedObject.FindProperty("portrait");
        }

        public override void DrawCommandGUI()
        {
            base.DrawCommandGUI();

            var sprite = ((Sprite)portraitProp.objectReferenceValue);
            var texture = AssetPreview.GetAssetPreview(sprite);
            GUILayout.Label(texture);

            if (GUILayout.Button("Refresh Portrait"))
            {
                ((Say)target).RefreshPortrait();
            }
        }
    }
}
