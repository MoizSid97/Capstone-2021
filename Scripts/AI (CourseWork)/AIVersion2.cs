using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVersion2 : MonoBehaviour
{
    //https://www.youtube.com/watch?v=jUdx_Nj4Xk0&ab_channel=SebastianLague

    public Transform path;
    public float speed = 5f;
    public float waitTime = 0.3f;
    public float turnSpeed = 90;

    private void Start()
    {
        Vector3[] waypoints = new Vector3[path.childCount];
        for(int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = path.GetChild(i).position;

            //Keeps mesh above ground
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }

        StartCoroutine(FollowPath(waypoints));
    }

    IEnumerator FollowPath (Vector3[] waypoints)
    {
        transform.position = waypoints[0];

        int targetWayPointIndex = 1;
        Vector3 targetWayPoint = waypoints[targetWayPointIndex];

        transform.LookAt(targetWayPoint);

        while(true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWayPoint, speed * Time.deltaTime);
            if(transform.position == targetWayPoint)
            {
                targetWayPointIndex = (targetWayPointIndex + 1) % waypoints.Length;
                targetWayPoint = waypoints[targetWayPointIndex];
                yield return new WaitForSeconds(waitTime);

                yield return StartCoroutine(TurnToFace(targetWayPoint));
            }
            yield return null;
        }
    }    

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while(Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        //Drawing gizmos
        Vector3 startPosition = path.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in path)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }

        Gizmos.DrawLine(previousPosition, startPosition);
    }
}
