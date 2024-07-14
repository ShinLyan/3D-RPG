using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float WaypointGizmosRadius = 0.3f;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWaypoint(i), WaypointGizmosRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNextIndex(int currentIndex)
        {
            return currentIndex + 1 == transform.childCount ? 0 : currentIndex + 1;
        }

        public Vector3 GetWaypoint(int index)
        {
            return transform.GetChild(index).position;
        }

        #region CreateWaypoints
        [SerializeField, Range(2, 5)] private int _waypointsCount = 0;

        private void OnValidate()
        {
            CreateWaypoints();
        }

        private void CreateWaypoints()
        {
            if (_waypointsCount < transform.childCount)
            {
                _waypointsCount = transform.childCount;
                return;
            }

            int currentChildCount = transform.childCount;
            int waypointsToCreate = _waypointsCount - currentChildCount;

            // Create new waypoints
            for (int i = 0; i < waypointsToCreate; i++)
            {
                var newWaypoint = new GameObject($"Waypoint{i + currentChildCount + 1}");
                newWaypoint.transform.position = transform.position;
                newWaypoint.transform.parent = transform;
            }
        }
        #endregion
    }
}
