using System;
using System.Collections.Generic;
using Shared;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Shared.Utils
{
    #region Comparer
    
    public class RaycastHitComparer : IComparer<RaycastHit>
    {
        public int Compare(RaycastHit a, RaycastHit b)
        {
            var aDistance = a.collider != null ? a.distance : float.MaxValue;
            var bDistance = b.collider != null ? b.distance : float.MaxValue;
            
            return aDistance.CompareTo(bDistance);
        }
    }
    
    public class ColliderComparer : IComparer<Collider>
    {
        private static readonly Vector3 Vector3Max = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        public Vector3 Reference;
        
        public int Compare(Collider a, Collider b)
        {
            var distanceX = Vector2.Distance(a != null ? a.transform.position : Vector3Max, Reference);
            var distanceY = Vector2.Distance(b != null ? b.transform.position : Vector3Max, Reference);

            return distanceX.CompareTo(distanceY);
        }
    }
    
    public class RaycastHit2DComparer : IComparer<RaycastHit2D>
    {
        public int Compare(RaycastHit2D a, RaycastHit2D b)
        {
            var distanceComparison = a.distance.CompareTo(b.distance);
            if (distanceComparison != 0) return distanceComparison;
            
            return a.fraction.CompareTo(b.fraction);
        }
    }

    public class Collider2DComparer : IComparer<Collider2D>
    {
        private static readonly Vector2 Vector2Max = new Vector2(float.MaxValue, float.MaxValue);
        public Vector3 Reference;
        
        public int Compare(Collider2D a, Collider2D b)
        {
            var distanceA = Vector2.Distance((a != null ? (Vector2)a.transform.position : Vector2Max), Reference);
            var distanceB = Vector2.Distance((b != null ? (Vector2)b.transform.position : Vector2Max), Reference);

            return distanceA.CompareTo(distanceB);
        }
    }
    
    #endregion
    
    public static class Physics2DHelper
    {
        private static readonly HashSet<RaycastHit2D> RaycastHitSet = new HashSet<RaycastHit2D>();
        private static readonly HashSet<Collider2D> ColliderSet = new HashSet<Collider2D>();
        
        private static readonly RaycastHit2DComparer RaycastHit2DComparer = new RaycastHit2DComparer();
        private static readonly Collider2DComparer ColliderComparer = new Collider2DComparer();

        public static readonly RaycastHit2D[] Results = new RaycastHit2D[30];
        public static readonly Collider2D[] Colliders = new Collider2D[30];
        
        public static RaycastHit2D FirstRaycastHit2D => Results[0];
        public static Collider2D FirstCollider2D => Colliders[0];
        
        #region Raycast
        
        public static int RaycastNonAllocSorted(Vector3 start, Vector3 direction, float distance, int layerMask, bool draw = false, Color drawColor = new Color())
        {
            return RaycastNonAllocSorted(start, direction, Results, distance, layerMask, draw, drawColor);
        }
        
        public static int RaycastNonAllocSorted(Vector3 start, Vector3 direction, RaycastHit2D[] results, float distance, int layerMask, bool draw = false, Color drawColor = new Color())
        {
            var hits = Physics2D.RaycastNonAlloc(start, direction, results, distance, layerMask);

            if (draw)
            {
                DebugDrawHelper.DrawLine(start, start + (direction * distance), drawColor);
            }

            if (hits > 0)
            {
                Array.Sort(results, 0, hits, RaycastHit2DComparer);
            }

            return hits;
        }

        public static int RaycastNonAllocSortedFiltered(Vector3 start, Vector2 direction, float distance,
            int layerMask, Filter[] filters)
        {
            return RaycastNonAllocSortedFiltered(start, direction, Results, distance, layerMask, filters);
        }

        public static int RaycastNonAllocSortedFiltered(Vector2 start, Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, Filter[] filters, bool draw = false, Color drawColor = new Color())
        {
            var hits = RaycastNonAllocSorted(start, direction, results, distance, layerMask, draw, drawColor);

            if (hits <= 0)
            {
                return 0;
            }
            
            RaycastHitSet.Clear();
            
            for (var i = 0; i < hits; i++)
            {
                var otherGameObject = results[i].transform.gameObject;
                var add = true;
                
                foreach (var filter in filters)
                {
                    if (!filter.Check(otherGameObject))
                    {
                        add = false; 
                        break;
                    }
                }

                if (add)
                {
                    RaycastHitSet.Add(results[i]);
                }
            }

            if (RaycastHitSet.Count == 0)
            {
                return 0;
            }
            
            Array.Clear(results, 0, RaycastHitSet.Count);
            RaycastHitSet.CopyTo(results);

            return RaycastHitSet.Count;
        }
        
        #endregion
        
        #region Overlaps

        public static int OverlapCircleNonAllocSorted(Vector2 center, float radius, int layerMask)
        {
            var hits = Physics2D.OverlapCircleNonAlloc(center, radius, Colliders, layerMask);

            if (hits <= 0)
            {
                return 0;
            }
            
            ColliderComparer.Reference = center;
            Array.Sort(Colliders, 0, hits, ColliderComparer);

            return hits;
        }

        public static int OverlapCircleNonAllocSortedFiltered(Vector2 center, float radius, int layerMask, Filter[] filters)
        {
            var hits = OverlapCircleNonAllocSorted(center, radius, layerMask);

            if (hits <= 0)
            {
                return 0;
            }
            
            ColliderSet.Clear();
            
            for (var i = 0; i < hits; i++)
            {
                var otherGameObject = Colliders[i].transform.gameObject;
                var add = true;
                
                foreach (var filter in filters)
                {
                    if (!filter.Check(otherGameObject))
                    {
                        add = false; 
                        break;
                    }
                }

                if (add)
                {
                    ColliderSet.Add(Colliders[i]);
                }
            }

            if (ColliderSet.Count == 0)
            {
                return 0;
            }
            
            Array.Clear(Colliders, 0, ColliderSet.Count);
            ColliderSet.CopyTo(Colliders);

            return ColliderSet.Count;
        }
        
        public static int OverlapBoxNonAllocSorted(Vector3 center, Vector3 halfExtents, float angle, int layerMask)
        {
            var hits = Physics2D.OverlapBoxNonAlloc(center, halfExtents, angle, Colliders, layerMask);

            if (hits <= 0)
            {
                return 0;
            }
            
            ColliderComparer.Reference = center;
            Array.Sort(Colliders, 0, hits, ColliderComparer);

            return hits;
        }

        public static int OverlapBoxNonAllocSortedFiltered(Vector3 center, Vector3 halfExtents, float angle, int layerMask, Filter[] filters)
        {
            var hits = OverlapBoxNonAllocSorted(center, halfExtents, angle, layerMask);

            if (hits <= 0)
            {
                return 0;
            }
            
            ColliderSet.Clear();
            
            for (var i = 0; i < hits; i++)
            {
                var otherGameObject = Colliders[i].transform.gameObject;
                var add = true;
                
                foreach (var filter in filters)
                {
                    if (!filter.Check(otherGameObject))
                    {
                        add = false; 
                        break;
                    }
                }

                if (add)
                {
                    ColliderSet.Add(Colliders[i]);
                }
            }

            if (ColliderSet.Count == 0)
            {
                return 0;
            }
            
            Array.Clear(Colliders, 0, ColliderSet.Count);
            ColliderSet.CopyTo(Colliders);

            return ColliderSet.Count;
        }
        
        #endregion
    }
    
    public static class PhysicsHelper
    {
        private static readonly HashSet<RaycastHit> RaycastHitSet = new HashSet<RaycastHit>();
        private static readonly HashSet<Collider> ColliderSet = new HashSet<Collider>();
        
        private static readonly RaycastHitComparer RaycastHitComparer = new RaycastHitComparer();
        private static readonly ColliderComparer ColliderComparer = new ColliderComparer();

        public static readonly RaycastHit[] Results = new RaycastHit[30];
        public static readonly Collider[] Colliders = new Collider[30];
        
        public static RaycastHit FirstRaycastHit => Results[0];
        public static Collider FirstCollider => Colliders[0];
        
        #region Raycast
        
        public static int RaycastNonAllocSorted(Vector3 start, Vector3 direction, float distance, int layerMask, bool draw = false, Color drawColor = new Color())
        {
            return RaycastNonAllocSorted(start, direction, Results, distance, layerMask, draw, drawColor);
        }
        
        public static int RaycastNonAllocSorted(Vector3 start, Vector3 direction, RaycastHit[] results, float distance, int layerMask, bool draw = false, Color drawColor = new Color())
        {
            var hits = Physics.RaycastNonAlloc(start, direction, results, distance, layerMask);

            if (draw)
            {
                DebugDrawHelper.DrawLine(start, start + (direction * distance), drawColor);
            }

            if (hits > 0)
            {
                Array.Sort(results, 0, hits, RaycastHitComparer);
            }

            return hits;
        }

        public static int RaycastNonAllocSortedFiltered(Vector3 start, Vector2 direction, float distance,
            int layerMask, Filter[] filters, bool draw = false, Color drawColor = new Color())
        {
            return RaycastNonAllocSortedFiltered(start, direction, Results, distance, layerMask, filters, draw, drawColor);
        }

        public static int RaycastNonAllocSortedFiltered(Vector2 start, Vector2 direction, RaycastHit[] results, float distance, int layerMask, Filter[] filters,  bool draw = false, Color drawColor = new Color())
        {
            var hits = RaycastNonAllocSorted(start, direction, results, distance, layerMask, draw, drawColor);

            if (hits <= 0)
            {
                return 0;
            }
            
            RaycastHitSet.Clear();
            
            for (var i = 0; i < hits; i++)
            {
                var otherGameObject = results[i].transform.gameObject;
                var add = true;
                
                foreach (var filter in filters)
                {
                    if (!filter.Check(otherGameObject))
                    {
                        add = false; 
                        break;
                    }
                }

                if (add)
                {
                    RaycastHitSet.Add(results[i]);
                }
            }

            if (RaycastHitSet.Count == 0)
            {
                return 0;
            }
            
            Array.Clear(results, 0, RaycastHitSet.Count);
            RaycastHitSet.CopyTo(results);

            return RaycastHitSet.Count;
        }
        
        #endregion
        
        #region Overlaps

        public static int OverlapSphereNonAllocSorted(Vector2 center, float radius, int layerMask)
        {
            var hits = Physics.OverlapSphereNonAlloc(center, radius, Colliders, layerMask);

            if (hits <= 0)
            {
                return 0;
            }
            
            ColliderComparer.Reference = center;
            Array.Sort(Colliders, 0, hits, ColliderComparer);

            return hits;
        }

        public static int OverlapSphereNonAllocSortedFiltered(Vector2 center, float radius, int layerMask, Filter[] filters)
        {
            var hits = OverlapSphereNonAllocSorted(center, radius, layerMask);

            if (hits <= 0)
            {
                return 0;
            }
            
            ColliderSet.Clear();
            
            for (var i = 0; i < hits; i++)
            {
                var otherGameObject = Colliders[i].transform.gameObject;
                var add = true;
                
                foreach (var filter in filters)
                {
                    if (!filter.Check(otherGameObject))
                    {
                        add = false; 
                        break;
                    }
                }

                if (add)
                {
                    ColliderSet.Add(Colliders[i]);
                }
            }

            if (ColliderSet.Count == 0)
            {
                return 0;
            }
            
            Array.Clear(Colliders, 0, ColliderSet.Count);
            ColliderSet.CopyTo(Colliders);

            return ColliderSet.Count;
        }
        
        public static int OverlapBoxNonAllocSorted(Vector3 center, Vector3 halfExtents, Quaternion orientation, int layerMask)
        {
            var hits = Physics.OverlapBoxNonAlloc(center, halfExtents, Colliders, orientation, layerMask);

            if (hits <= 0)
            {
                return 0;
            }
            
            ColliderComparer.Reference = center;
            Array.Sort(Colliders, 0, hits, ColliderComparer);

            return hits;
        }

        public static int OverlapBoxNonAllocSortedFiltered(Vector3 center, Vector3 halfExtents, Quaternion orientation, int layerMask, Filter[] filters)
        {
            var hits = OverlapBoxNonAllocSorted(center, halfExtents, orientation, layerMask);

            if (hits <= 0)
            {
                return 0;
            }
            
            ColliderSet.Clear();
            
            for (var i = 0; i < hits; i++)
            {
                var otherGameObject = Colliders[i].transform.gameObject;
                var add = true;
                
                foreach (var filter in filters)
                {
                    if (!filter.Check(otherGameObject))
                    {
                        add = false; 
                        break;
                    }
                }

                if (add)
                {
                    ColliderSet.Add(Colliders[i]);
                }
            }

            if (ColliderSet.Count == 0)
            {
                return 0;
            }
            
            Array.Clear(Colliders, 0, ColliderSet.Count);
            ColliderSet.CopyTo(Colliders);

            return ColliderSet.Count;
        }
        
        #endregion
    }
}