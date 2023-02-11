using System;
using UnityEngine;

namespace Blabbers
{
    [Serializable]
    public abstract class Filter
    {
        public abstract bool Check(GameObject gameObject);
    }
    
    [Serializable]
    public class TagFilter : Filter
    {
        public string[] AllowedTags;
        public override bool Check(GameObject gameObject)
        {
            var allowed = false;
            foreach (var allowedTag in AllowedTags)
            {
                if (gameObject.CompareTag(allowedTag))
                {
                    allowed = true;
                    break;
                }
            }

            return allowed;
        }
    }
}