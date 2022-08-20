using System.Collections.Generic;
using UnityEngine;

namespace Shared
{
	public class DebugDrawHelper : MonoBehaviour
	{
		static private DebugDrawHelper instance;

		private Queue<DebugDrawInfo> cache;
		private Queue<DebugDrawInfo> debugDrawInfo;

		public enum DebugDrawType { Ray, Line }

		public class DebugDrawInfo
		{
			private DebugDrawType drawType;

			private Vector2 a;
			private Vector2 b;

			private Color color;

			private float duration;
			private bool depthTest;

			public DebugDrawInfo(DebugDrawType drawType, Vector2 a, Vector2 b, Color color, float duration, bool depthTest)
			{
				this.drawType = drawType;
				this.a = a;
				this.b = b;
				this.color = color;
				this.duration = duration;
				this.depthTest = depthTest;
			}

			public void Set(DebugDrawType drawType, Vector2 a, Vector2 b, Color color, float duration, bool depthTest)
			{
				this.drawType = drawType;
				this.a = a;
				this.b = b;
				this.color = color;
				this.duration = duration;
				this.depthTest = depthTest;
			}

			public void Draw()
			{
				switch (drawType)
				{
					case DebugDrawType.Ray:
						Debug.DrawRay(a, b, color, duration, depthTest);
						break;
					case DebugDrawType.Line:
						Debug.DrawLine(a, b, color, duration, depthTest);
						break;
				}
			}
		}

		private void Awake()
		{
			debugDrawInfo = new Queue<DebugDrawInfo>();
			cache = new Queue<DebugDrawInfo>();

			instance = this;

			DontDestroyOnLoad(gameObject);
		}

		private void OnDestroy()
		{
			instance = null;

			cache.Clear();
			debugDrawInfo.Clear();
		}

		private void LateUpdate()
		{
			while (debugDrawInfo.Count > 0)
			{
				var info = debugDrawInfo.Dequeue();
				info.Draw();

				cache.Enqueue(info);
			}
		}

		private void Add(DebugDrawType drawType, Vector2 a, Vector2 b, Color color, float duration, bool depthTest)
		{
			if (cache.Count > 0)
			{
				var info = cache.Dequeue();
				info.Set(drawType, a, b, color, duration, depthTest);
				debugDrawInfo.Enqueue(info);
			}
			else
			{
				debugDrawInfo.Enqueue(new DebugDrawInfo(drawType, a, b, color, duration, depthTest));
			}
		}

		static private void Create()
		{
			var go = new GameObject("DebugDrawHelper");
			go.AddComponent<DebugDrawHelper>();
		}

		static public void DrawLine(Vector2 origin, Vector2 end, Color color, float duration = 0f, bool depthTest = false)
		{
			if (instance == null)
				Create();

			instance.Add(DebugDrawType.Line, origin, end, color, duration, depthTest);
		}

		static public void DrawRay(Vector2 origin, Vector2 direction, Color color, float duration = 0f, bool depthTest = false)
		{
			if (instance == null)
				Create();

			instance.Add(DebugDrawType.Ray, origin, direction, color, duration, depthTest);
		}

		static public void DrawBox(Vector2 center, Vector2 extends, Color color, float duration = 0f, bool depthTest = false)
		{
			var topLeft = new Vector2(center.x - extends.x, center.y + extends.y);
			var topRight = new Vector2(center.x + extends.x, center.y + extends.y);
			var bottomLeft = new Vector2(center.x - extends.x, center.y - extends.y);
			var bottomRight = new Vector2(center.x + extends.x, center.y - extends.y);

			DrawBox(topLeft, topRight, bottomLeft, bottomRight, color, duration, depthTest);
		}

		static public void DrawBox(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight, Color color, float duration = 0f, bool depthTest = false)
		{
			if (instance == null)
				Create();

			instance.Add(DebugDrawType.Line, topLeft, topRight, color, duration, depthTest);
			instance.Add(DebugDrawType.Line, topLeft, bottomLeft, color, duration, depthTest);
			instance.Add(DebugDrawType.Line, topRight, bottomRight, color, duration, depthTest);
			instance.Add(DebugDrawType.Line, bottomLeft, bottomRight, color, duration, depthTest);
		}

		static public void DrawCircle(Vector2 center, float radius, int iterations, Color color, float duration = 0f, bool depthTest = false)
		{
			if (instance == null)
				Create();

			if (iterations < 4)
				iterations = 4;

			if (iterations > 360)
				iterations = 360;

			var debugDraw = instance.debugDrawInfo;

			var points = new Vector2[iterations];
			var angle = (2f * Mathf.PI) / iterations;
			var currentAngle = 0f;
			var direction = Vector2.zero;

			for (int i = 0; i < iterations; i++)
			{
				currentAngle += angle;
				direction = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));
				points[i] = center + (direction * radius);

				if (i > 0)
				{
					instance.Add(DebugDrawType.Line, points[i - 1], points[i], color, duration, depthTest);
				}
			}

			instance.Add(DebugDrawType.Line, points[points.Length - 1], points[0], color, duration, depthTest);
		}

		static public void DrawPolygon(Vector2[] points, Color color, float duration = 0f, bool depthTest = false)
		{
			if (instance == null)
				Create();

			for (int i = 0; i < points.Length; i++)
			{
				if (i > 0)
				{
					instance.Add(DebugDrawType.Line, points[i - 1], points[i], color, duration, depthTest);
				}
			}

			instance.Add(DebugDrawType.Line, points[points.Length - 1], points[0], color, duration, depthTest);
		}
	}
}