// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

﻿using UnityEngine;
using System.Collections;
using Blabbers.Game00;

namespace Fungus
{
    /// <summary>
    /// The block will execute when the game starts playing.
    /// </summary>
    [EventHandlerInfo("",
					  "Gameplay/Before Level Victory Screen",
                      "The block will execute right before the level victory screen would show up. The victory screen will wait for this block to be finished so it can appear.")]
    [AddComponentMenu("")]
    public class BeforeLevelVictoryScreen : EventHandler
    {
        protected virtual void Awake()
        {
            Singleton.Get<GameplayController>().OnBeforeVictoryScreenShown += HandleBlockStart;
        }

		private void OnDestroy()
		{
            var gameplay = Singleton.Get<GameplayController>();
            if(gameplay != null)
            {
			    gameplay.OnBeforeVictoryScreenShown -= HandleBlockStart;
            }
		}

        private void HandleBlockStart()
        {
			ExecuteBlock();

			StartCoroutine(WaitForBlockToFinish());
			IEnumerator WaitForBlockToFinish()
			{
				yield return new WaitUntil(()=> !parentBlock.IsExecuting());
				// When we remove this from the event, the gameplay controller moves on
				Singleton.Get<GameplayController>().OnBeforeVictoryScreenShown -= HandleBlockStart;
			}
        }
	}
}
