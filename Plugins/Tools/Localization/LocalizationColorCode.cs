using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LocalizationColorCode : ScriptableObject
{
    public string key;
    public List<string> extraKeys;

    public Color color;
    public bool includePlural = true;
}
