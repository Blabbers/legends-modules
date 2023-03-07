using UnityEngine;

namespace Fungus
{
    [CommandInfo("Blabbers",
                 "Set Choice",
				 "Sets the Choice.")]
    [AddComponentMenu("")]
    public class SetChoice : Command
    {
        [Header("Key that will be saved on GameData.Progress")]
        [SerializeField] string choiceKey;
		[VariableProperty(typeof(IntegerVariable))]
		[SerializeField] private IntegerVariable selectedChoiceValue;

		#region Public members

		public override void OnEnter()
        {
            Choice choice = new Choice();

            choice.key = choiceKey;
			choice.selectedId = selectedChoiceValue.Value;

			GameData.Instance.Progress.AddChoice(choice);

            //TODO: Implement PAUSE call.
            Continue();
        }

		public override Color GetButtonColor()
        {
            return new Color32(216, 228, 170, 255);
        }

		public override string GetSummary()
		{
			string namePrefix = $"\"{choiceKey}\" = ";
			return namePrefix + (selectedChoiceValue?.Key);
		}

		#endregion
	}
}