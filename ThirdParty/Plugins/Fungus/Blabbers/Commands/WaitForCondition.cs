// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using System.Collections;

namespace Fungus
{
	/// <summary>
	/// Waits for a number of frames before executing the next command in the block.
	/// </summary>
	[CommandInfo("Flow",
				 "Wait Until",
				 "Waits a boolean condition before continuing to the next command.")]
	[AddComponentMenu("")]
	[ExecuteInEditMode]
	public class WaitForCondition : Command
	{
		[Tooltip("Condition to wait for")]
		[SerializeField]
		[VariableProperty("<Value>", typeof(BooleanVariable))]
		private BooleanVariable condition;

		//[SerializeField] protected CompareOperator comparison;

		[Tooltip("Value the condition needs to meet")]
		[SerializeField]
		private BooleanData value;

		#region Public members

		public override void OnEnter()
		{
			StartCoroutine(Routine());
			IEnumerator Routine()
			{
				yield return new WaitUntil(() => condition.Value == value);
				Continue();
			}
		}

		public override string GetSummary()
		{
			return $": {(condition != null ? condition.Key : "<Value>")} " +
				"is" +
				$" {(value.booleanRef != null ? value.booleanRef.Key : value.Value.ToString())}";
		}

		public override Color GetButtonColor()
		{
			return new Color32(235, 191, 217, 255);
		}

		//public override bool HasReference(Variable variable)
		//{
		//	return frameCount.integerRef == variable || base.HasReference(variable);
		//}

		#endregion
	}
}