using UnityEngine;

namespace Fungus
{
    [CommandInfo("Blabbers",
                 "Load Choice",
				 "Loads the Choice.")]
    [AddComponentMenu("")]
    public class LoadChoice : Command
    {
        [Header("Key that will be loaded from GameData.Progress.choices")]
        [SerializeField] string choiceKey;
		[VariableProperty(typeof(IntegerVariable))]
		[SerializeField] private IntegerVariable setChoiceIdTo;

		#region Public members

		public override void OnEnter()
        {
			var choice = GameData.Instance.Progress.GetChoice(choiceKey);
			int id;

			if(choice == null)
			{
				id = 0;
			}
			else
			{
				id = choice.selectedId;
			}

			setChoiceIdTo?.Apply(SetOperator.Assign, id);
			//TODO: Implement PAUSE call.
			Continue();
        }

		public override Color GetButtonColor()
        {
            return new Color32(216, 228, 170, 255);
        }

		public override string GetSummary()
		{
			string namePrefix = $"{(setChoiceIdTo? "":setChoiceIdTo?.Key)} = ";
			return namePrefix + $"\"{choiceKey}\" from GameData";
		}

		#endregion
	}
}