using Blabbers.Game00;
using UnityEngine;

namespace Fungus
{
    [CommandInfo("Blabbers",
				 "Get GameObject Properties",
				 "Gets properties from a GameObject")]
    [AddComponentMenu("")]
    public class GetGameObjectProperty : Command
	{

		public enum PropertyOptions {
			activeSelf
		}


		[SerializeField] GameObject sourceObj;
		[SerializeField] PropertyOptions property;
		//[SerializeField] protected AnyVariableAndDataPair targetVar = new AnyVariableAndDataPair();

		[Tooltip("Variable to store the value in.")]
		[VariableProperty(typeof(BooleanVariable),
					 typeof(IntegerVariable),
					 typeof(FloatVariable),
					 typeof(StringVariable))]
		[SerializeField] protected Variable variable;


		#region Public members

		public override void OnEnter()
        {
			if (sourceObj == null) return;
			if (variable == null) return;

			if (CheckForVariableMissmatch()) return;



            Continue();
        }

		bool CheckForVariableMissmatch()
		{

			System.Type variableType = variable.GetType();

			Debug.Log(variableType.ToString());

			if (property == PropertyOptions.activeSelf)
			{

				if(variableType == typeof(BooleanVariable))
				{

					BooleanVariable booleanVariable = variable as BooleanVariable;
					booleanVariable.Value = sourceObj.activeSelf;

					return false;
				}

				return true;
			}


			return true;
		}


		public override Color GetButtonColor()
        {
            return new Color32(216, 228, 170, 255);
        }

		public override string GetSummary()
		{
			string prefix = $"Get [{property}] from";
			string mid = $"[{(sourceObj == null ? "NULL".Colored("red") : sourceObj.name)}] GameObject";
			//string suffix = $"-> Save at [{(targetVar == null ? "null" : targetVar.GetDataDescription())}]";
			string suffix = "";


			if (variable == null)
			{
				suffix = "-> Save at [NULL]".Colored("Red");
			}
			else
			{
				suffix = $"-> Save at [{variable.Key}]";
			}

			return $"\t{prefix} {mid} {suffix}";
		}

		#endregion
	}
}