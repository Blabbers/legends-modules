using System;
using UnityEngine;

namespace Shared.Utils
{
    [Serializable]
    public abstract class Filter
    {
        public abstract bool Check(GameObject gameObject);
    }
}