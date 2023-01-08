using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blabbers.Game00;

public static class HashExtension
{
    public static void DisplaySet(this HashSet<string> collection)
    {
        string debug;

        debug = "{";
        foreach (string i in collection)
        {
            debug = debug + $" {i}";
        }
        debug = debug + "}";

        Debug.Log(debug);

    }

}
