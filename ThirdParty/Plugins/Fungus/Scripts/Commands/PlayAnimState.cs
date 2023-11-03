// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using System.Collections;
using UnityEngine;

namespace Fungus
{
	/// <summary>
	/// Plays a state of an animator according to the state name.
	/// </summary>
	[CommandInfo("Animation",
				 "Play Anim State",
				 "Plays a state of an animator according to the state name")]
	[AddComponentMenu("")]
	public class PlayAnimState : Command
	{
		[Tooltip("Reference to an Animator component in a game object")]
		[SerializeField] protected AnimatorData animator = new AnimatorData();

		[Tooltip("Name of the state you want to play")]
		[SerializeField] protected StringData stateName = new StringData();

		[Tooltip("Layer to play animation on")]
		[SerializeField] protected IntegerData layer = new IntegerData(-1);

		[Tooltip("Start time of animation")]
		[SerializeField] protected FloatData time = new FloatData(0f);

		[SerializeField] private bool waitUntilFinished = false;

		#region Public members

		public override void OnEnter()
		{
			var waitTime = 0f;
			if (animator.Value != null)
			{
				animator.Value.Play(stateName.Value, layer.Value, time.Value);

				if (waitUntilFinished)
				{
					var clip = FindAnimation(animator.Value, stateName.Value);
					waitTime = clip.length;
				}
			}

			StartCoroutine(Routine());
			IEnumerator Routine()
			{
				if (waitTime > 0f)
				{
					yield return new WaitForSeconds(waitTime);
				}
				Continue();
			}
		}

		public AnimationClip FindAnimation(Animator animator, string name)
		{
			foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
			{
				if (clip.name == name)
				{
					return clip;
				}
			}

			return null;
		}

		public override string GetSummary()
		{
			if (animator.Value == null)
			{
				return "Error: No animator selected";
			}

			return animator.Value.name + " (" + stateName.Value + ")";
		}

		public override Color GetButtonColor()
		{
			return new Color32(170, 204, 169, 255);
		}

		public override bool HasReference(Variable variable)
		{
			return animator.animatorRef == variable || stateName.stringRef == variable ||
				layer.integerRef == variable || time.floatRef == variable ||
				base.HasReference(variable);
		}

		#endregion
	}
}

