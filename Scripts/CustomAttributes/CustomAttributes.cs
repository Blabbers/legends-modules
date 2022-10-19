using UnityEngine;
using System;

// Use case: 
//[PercentBox]
[AttributeUsage(AttributeTargets.Field)]
public class PercentBoxAttribute : PropertyAttribute
{
}

// Use case:
//[TimeSpanBox("{0:mm} min {0:ss} sec")]
[AttributeUsage(AttributeTargets.Field)]
public class TimeSpanBoxAttribute : PropertyAttribute
{
	public TimeSpanBoxAttribute(string format)
	{
		Format = format;
	}

	public string Format { get; }
}