using Blabbers.Game00;
using System;
using UnityEngine;
using System.Collections;

namespace Fungus
{
	[CommandInfo("Blabbers",
				 "Clickblock",
				 "Toggles Clickblock.")]
	[AddComponentMenu("")]
	public class ToggleClickblock : Command
	{
		public bool active = true;
		[SerializeField] protected float delay = 0;
		[SerializeField] protected bool waitUntilFinished = true;
		[SerializeField] protected bool ignoreTimeScale = true;

		#region Public members

		public override void OnEnter()
		{
			if(delay == 0)
			{
				UI_Clickblock.Instance.ToggleClickBlock(active);
				Continue();
			}
			else
			{
				Wait(delay, () => PostDelay());
			}		
		}

		void PostDelay()
		{
			UI_Clickblock.Instance.ToggleClickBlock(active);
			Continue();
		}


		void Wait(float duration, Action callback)
		{
			StartCoroutine(_Wait());
			IEnumerator _Wait()
			{
				if (ignoreTimeScale)
				{
					yield return new WaitForSecondsRealtime(duration);
					callback?.Invoke();
				}
				else
				{

					var t = duration;
					while (t > 0)
					{
						t -= Time.deltaTime;
						yield return null;
					}

					callback?.Invoke();
				}
			}
		}


		public override Color GetButtonColor()
		{
			return new Color32(232, 130, 130, 255);
		}

		public override string GetSummary()
		{
			string namePrefix = "";
			string nameSuffix = "";

			if (active) {
				namePrefix = $"Clickblock: Enabled";
			}
			else
			{
				namePrefix = $"Clickblock: Disabled";
			}

			if(delay > 0)
			{
				nameSuffix = $"(delayed by {delay}s)";
			}

			return namePrefix + " "+ nameSuffix;
		}


		#endregion
	}
}