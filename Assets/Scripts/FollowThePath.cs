using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowThePath : MonoBehaviour
{
    public Transform[] waypoints;

    [SerializeField]
    private float moveSpeed = 1f;

    [HideInInspector]
    public int waypointIndex = 0;

    public bool moveAllowed = false;

    // Use this for initialization
    
    private void Start()
    {
        // Place the object at the first waypoint at start
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (moveAllowed)
            Move();
    }

    private void Move()
    {
        // Only move if we haven't reached the end of the waypoint array
        if (waypointIndex <= waypoints.Length - 1)
        {
            // Move towards the current target waypoint
            transform.position = Vector2.MoveTowards(
                transform.position,
                waypoints[waypointIndex].transform.position,
                moveSpeed * Time.deltaTime
            );

            // Once we arrive exactly at the waypoint, advance to the next one
            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex += 1;
            }
        }
    }
}