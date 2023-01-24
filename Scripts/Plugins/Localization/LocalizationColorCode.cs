using Blabbers.Game00;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu]
public class LocalizationColorCode : ScriptableObject
{

	public LocalizedString localization;
	public List<string> tags = new List<string>();
}
