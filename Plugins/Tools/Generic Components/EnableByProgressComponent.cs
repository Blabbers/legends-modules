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
			this.gameObject.SetActive(enableOnCondition);
		}
	}

	bool CheckCondition()
	{
		int currentLevel = ProgressController.GameProgress.currentLevelId;
		int reachedLevel = ProgressController.GameProgress.reachedLevel;

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



