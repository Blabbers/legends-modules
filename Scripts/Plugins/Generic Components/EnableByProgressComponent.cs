using Blabbers.Game00;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(-1000)]
public class EnableByProgressComponent : MonoBehaviour
{
	public EnableConditions condition;
	public int level = -1;
	public bool enableOnCondition = true;

	private void Awake()
	{
		if (CheckCondition())
		{
			Debug.Log($"EnableByProgressComponent - Condition: {condition} {level}\nSetting " +
				$"[{this.name}] ".Colored("white") +
				$"as " +
				$"[{enableOnCondition}]".Colored("orange"));
			this.gameObject.SetActive(enableOnCondition);
		}
	}

	bool CheckCondition()
	{
		int currentLevel = ProgressController.GameProgress.currentLevelId + 1;
		int reachedLevel = ProgressController.GameProgress.reachedLevel + 1;

		switch (condition)
		{
			case EnableConditions.IsCurrentLevel:
				if (currentLevel == level) return true;
				break;
			case EnableConditions.HasClearedLevel:
				if (reachedLevel > level) return true;
				break;
			default:
				break;
		}

		return false;
	}


}


public enum EnableConditions
{
	IsCurrentLevel, HasClearedLevel
}



