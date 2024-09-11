using DG.Tweening;
using System;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.Events;

namespace Blabbers.Game00
{
	public static class Extensions
	{
		public static void SilentInvoke(this Action action)
		{
			try
			{
				action?.Invoke();
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}

		public static void SilentInvoke<T1>(this Action<T1> action, T1 arg1)
		{
			try
			{
				action?.Invoke(arg1);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}

		public static void SilentInvoke<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
		{
			try
			{
				action?.Invoke(arg1, arg2);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}

		public static void SilentInvoke<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
		{
			try
			{
				action?.Invoke(arg1, arg2, arg3);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}

		public static void SilentInvoke(this Relay action)
		{
			try
			{
				action?.Invoke();
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}

		public static void SilentInvoke<T1>(this Relay<T1> action, T1 arg1)
		{
			try
			{
				action?.Invoke(arg1);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}

		public static void SilentInvoke<T1, T2>(this Relay<T1, T2> action, T1 arg1, T2 arg2)
		{
			try
			{
				action?.Invoke(arg1, arg2);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}

		public static void SilentInvoke<T1, T2, T3>(this Relay<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
		{
			try
			{
				action?.Invoke(arg1, arg2, arg3);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}

		public static void AddListenerOnce(this UnityEvent unityEvent, UnityAction callback)
		{
			unityEvent.RemoveListener(callback);
			unityEvent.AddListener(callback);
		}

		public static void SetActive(this GameObject gameObject, bool value, float delay)
		{
			var myFloat = 0f;
			DOTween.To(() => myFloat, x => myFloat = x, delay, delay).OnComplete(() =>
			{
				gameObject.SetActive(value);
			});
		}

		public static T GetComponentAboveAndBelow<T>(this GameObject gameObject) where T : Component
		{
			var component = gameObject.GetComponentInParent<T>();
			if (component == null)
				component = gameObject.GetComponentInChildren<T>();

			return component;
		}

		public static bool TryGetComponentAboveAndBelow<T>(this GameObject gameObject, out T component) where T : Component
		{
			component = gameObject.GetComponentInParent<T>();
			if (component == null)
				component = gameObject.GetComponentInChildren<T>();

			return component != null;
		}

		public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
		{
			if (x != null)
			{
				vector.x = x.Value;
			}
			if (y != null)
			{
				vector.y = y.Value;
			}
			if (z != null)
			{
				vector.z = z.Value;
			}
			return vector;
		}
	}
}