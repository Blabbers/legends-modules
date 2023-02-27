// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using Animancer;
using Blabbers.Game00;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Writes text in a dialog box.
    /// </summary>
    [CommandInfo("Blabbers",
                 "Say",
                 "Writes text in a dialog box.")]
    [AddComponentMenu("")]
    public class Say : Command
    {
		// Removed this tooltip as users's reported it obscures the text box
		//[TextArea(5,10)]
		[SerializeField]
		[LocalizedStringOptions(hasBigTextArea: true)]
		protected LocalizedString storyText;
		[SerializeField] 
        protected bool applyKeyCodes = true;
                
		[Tooltip("Plays Text-to-Speech when this command starts")]
		[SerializeField] protected bool playTTS = true;

		[Tooltip("Always show this Say text when the command is executed multiple times")]
		[SerializeField] protected bool showAlways = true;

		[Tooltip("Number of times to show this Say text when the command is executed multiple times")]
		[HideIf(nameof(showAlways))]
		[SerializeField] protected int showCount = 1;

		[Tooltip("Character that is speaking")]
        [OnValueChanged("AddPortraitIfNone")]
        [SerializeField] protected Character character;
        
		[Tooltip("Portrait that represents speaking character")]
        //[ShowAssetPreview]
        [HideIf(nameof(HasNoCharacter))]
        [Dropdown(nameof(GetPortraits))]		
		[SerializeField] protected Sprite portrait;
        private List<Sprite> GetPortraits() => character != null ? character.Portraits : null;
        private bool HasNoCharacter() => character == null;
		
		//[Tooltip("Voiceover audio to play when writing the text")]
        //protected AudioClip voiceOverClip;

		//[Tooltip("Type this text in the previous dialog box.")]
		//[SerializeField] protected bool extendPrevious = false;

		//[Tooltip("Fade out the dialog box when writing has finished and not waiting for input.")]
		//[SerializeField] protected bool fadeWhenDone = true;

		//[Tooltip("Wait for player to click before continuing.")]
		//[SerializeField] protected bool waitForClick = true;

		//[Tooltip("Stop playing voiceover when text finishes writing.")]
		//[SerializeField] protected bool stopVoiceover = true;

		//[Tooltip("Wait for the Voice Over to complete before continuing")]
		//[SerializeField] protected bool waitForVO = false;        

		protected int executionCount;

        #region Public members

        /// <summary>
        /// Character that is speaking.
        /// </summary>
        public virtual Character _Character { get { return character; } }

        /// <summary>
        /// Portrait that represents speaking character.
        /// </summary>
        public virtual Sprite Portrait { get { return portrait; } set { portrait = value; } }

		public override void OnEnter()
        {
            if (!showAlways && executionCount >= showCount)
            {
                Continue();
                return;
            }

            executionCount++;

            var sayDialog = SayDialog.GetSayDialog();
            if (sayDialog == null)
            {
                Continue();
                return;
            }
    
            var flowchart = GetFlowchart();

            sayDialog.SetActive(true);

            sayDialog.SetCharacter(character);
            sayDialog.SetCharacterImage(portrait);

			string displayText = storyText.GetLocalizedText(applyKeyCodes);

            var activeCustomTags = CustomTag.activeCustomTags;
            for (int i = 0; i < activeCustomTags.Count; i++)
            {
                var ct = activeCustomTags[i];
                displayText = displayText.Replace(ct.TagStartSymbol, ct.ReplaceTagStartWith);
                if (ct.TagEndSymbol != "" && ct.ReplaceTagEndWith != "")
                {
                    displayText = displayText.Replace(ct.TagEndSymbol, ct.ReplaceTagEndWith);
                }
            }

            string subbedText = flowchart.SubstituteVariables(displayText);

			LocalizationExtensions.PlayTTS(storyText.Key);

			sayDialog.Say(subbedText, true, true, true, true, false, null, delegate {
                Continue();
            });
		}

        public void Update()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			// Cheats
			if (Input.GetKeyDown(KeyCode.PageDown))
			{
				var sayDialog = SayDialog.GetSayDialog();
                sayDialog.Clear();
				sayDialog.Stop();
				// Skip this dialogue                
				Continue();
			}
#endif			
		}

        public override string GetSummary()
        {
            string namePrefix = "";
            if (character != null) 
            {
                namePrefix = character.NameText + ": ";
            }
            //if (extendPrevious)
            //{
            //    namePrefix = "EXTEND" + ": ";
            //}
			if (storyText.HasUnsavedChanges())
			{
                namePrefix = "<color=red><b>* UNSAVED CHANGES *</b></color> " + namePrefix;
            }
            return namePrefix + "\"" + storyText.GetRawText() + "\"";
        }

        public override Color GetButtonColor()
        {
            return new Color32(184, 210, 235, 255);
        }

        public override void OnReset()
        {
            executionCount = 0;
        }

        public override void OnStopExecuting()
        {
            var sayDialog = SayDialog.GetSayDialog();
            if (sayDialog == null)
            {
                return;
            }

            sayDialog.Stop();
        }

		public override void OnCommandAdded(Block parentBlock)
		{
            if(storyText != null)
            {
                storyText.OverrideLocKey(LocalizedString.GenerateLocKey());
            }

			AddPortraitIfNone();
		}

        private void AddPortraitIfNone()
        {
			if (portrait == null && character != null && character.Portraits.Count > 0)
			{
				portrait = character.Portraits[0];
			}
		}
		#endregion
	}
}