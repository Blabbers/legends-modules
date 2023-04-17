using Blabbers.Game00;
using UnityEngine;

namespace Fungus
{
    [CommandInfo("Blabbers",
				 "GameObjectSetActive",
				 "Sets an object enabled/disabled and checks if it has UI")]
    [AddComponentMenu("")]
    public class GameObjectSetActive : Command
    {

		[SerializeField] GameObject target;
		[SerializeField] bool active = true;
  //      [Header("Key that will be saved on GameData.Progress")]
  //      [SerializeField] string choiceKey;
		//[VariableProperty(typeof(IntegerVariable))]
		//[SerializeField] private IntegerVariable selectedChoiceValue;

		#region Public members

		public override void OnEnter()
        {

			target.SetActive(active);

			if (target.GetComponent<UI_PopupWindow>() != null)
			{
				if(active) target.GetComponent<UI_PopupWindow>().ShowPopup();
				else target.GetComponent<UI_PopupWindow>().HidePopup();

			}else if (target.GetComponent<UI_TutorialWindowBase>() != null)
			{
				if (active) target.GetComponent<UI_TutorialWindowBase>().ShowScreen();
				else target.GetComponent<UI_TutorialWindowBase>().HideScreen();
			}

			



   //         Choice choice = new Choice();

   //         choice.key = choiceKey;
			//choice.selectedId = selectedChoiceValue.Value;

			//GameData.Instance.Progress.AddChoice(choice);

            //TODO: Implement PAUSE call.
            Continue();
        }

		public override Color GetButtonColor()
        {
            return new Color32(216, 228, 170, 255);
        }

		public override string GetSummary()
		{
			//string namePrefix = $"\"{choiceKey}\" = ";
			//return namePrefix + (selectedChoiceValue?.Key);

			string namePrefix = $"\t {(active ? "Enable" : "Disable")}: [{target.name}] GameObject";
			return namePrefix;
		}

		#endregion
	}
}