using UnityEngine;

public class FollowThePath : MonoBehaviour
{
    public Transform[] waypoints;
    [SerializeField] private float moveSpeed = 1f;
    [HideInInspector] public int waypointIndex = 0;
    public bool moveAllowed = false;

    private Animator animator;

    private void Start()
    {
        // Snap to the first waypoint
        transform.position = waypoints[waypointIndex].position;

        // Cache the Animator
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError($"{name} is missing an Animator component!");
    }

    private void Update()
    {
        // Drive the walk/idle animation
        animator.SetBool("isWalking", moveAllowed);

        // Only move the transform when flagged
        if (moveAllowed)
            MoveAlongPath();
    }

    private void MoveAlongPath()
    {
        if (waypointIndex >= waypoints.Length) return;

        // Move toward the next waypoint
        transform.position = Vector2.MoveTowards(
            transform.position,
            waypoints[waypointIndex].position,
            moveSpeed * Time.deltaTime
        );

        // Once we arrive, advance the index
        if ((Vector2)transform.position == (Vector2)waypoints[waypointIndex].position)
            waypointIndex++;
    }
}
