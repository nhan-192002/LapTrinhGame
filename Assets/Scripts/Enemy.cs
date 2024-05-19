using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float flipSpeed = 2f;

    void Update()
    {
        MoveToWaypoint();
        FlipTowardsWaypoint();
    }

    void MoveToWaypoint()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * moveSpeed);
    }
    

    void FlipTowardsWaypoint()
    {
        Vector3 direction = waypoints[currentWaypointIndex].transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle > 90 || angle < -90)
        {
            // Flip the enemy if the waypoint is behind it
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            // Restore the enemy's original scale if the waypoint is in front of it
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
