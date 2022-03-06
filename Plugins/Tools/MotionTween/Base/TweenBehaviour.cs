using NaughtyAttributes;
using UnityEngine;

namespace Blabbers.Game00
{
	public abstract class TweenBehaviour : ScriptableObject
	{
		public float duration = 0.3f;
		public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
		public abstract void Play(MotionTweenPlayer tweenPlayer);

		[Button]
		public void DestroyBehaviour()
		{
			DestroyImmediate(this, true);
			//UnityEditor.AssetDatabase.SaveAssets();
			GUI.changed = true;
		}
	}
}
