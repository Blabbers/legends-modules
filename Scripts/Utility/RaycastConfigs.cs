using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace Raycast.Utility
{
    [Serializable]
    public class RaycastData
    {

        public string Name;
        public float Range;
        public float HeightOffset;
        public Vector2 offsetDir;
        public Transform Origin;

        [Header("Output")]
        public Vector2 finalOrigin;
        public Vector2 direction = Vector2.down;

        public LayerMask Mask;
        public Color color = Color.green;


        public Vector2 GetOrigin()
        {
            //return new Vector2(Origin.transform.position.x, Origin.transform.position.y + HeightOffset);
            return finalOrigin;
        }

        public Vector2 GetBaseorigin()
		{
            return new Vector2(Origin.transform.position.x, Origin.transform.position.y);
        }


		public void SetOrigin()
        {
            finalOrigin = new Vector2(Origin.transform.position.x, Origin.transform.position.y + HeightOffset);
        }

        public void SetRelativeOrigin()
        {
            finalOrigin = new Vector2(Origin.transform.position.x, Origin.transform.position.y) + direction.normalized * HeightOffset;
        }

        public void SetForwardedOrigin(Vector2 currentDir)
        {
            finalOrigin = new Vector2(Origin.transform.position.x, Origin.transform.position.y) + (currentDir * offsetDir.x);
        }

        public void SetRaisedOrigin(Vector2 currentDir)
        {
            finalOrigin = new Vector2(Origin.transform.position.x, Origin.transform.position.y) + (currentDir * offsetDir.y);
        }

        public void SetLocalOffsetOrigin(Vector2 forwardDir, Vector2 upDir)
        {
            finalOrigin = new Vector2(Origin.transform.position.x, Origin.transform.position.y) + (upDir * offsetDir.y) + (forwardDir * offsetDir.x);
        }


        public void SetOffsetOrigin()
        {
            finalOrigin = new Vector2(Origin.transform.position.x, Origin.transform.position.y) + offsetDir * HeightOffset;
        }


    }
}


