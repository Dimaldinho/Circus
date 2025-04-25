using UnityEngine;

public class FollowThePath : MonoBehaviour
{
    public Transform[] waypoints;
    [SerializeField] private float moveSpeed = 1f;
    [HideInInspector] public int waypointIndex = 0;
    public bool moveAllowed = false;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // Snap to the first waypoint
        transform.position = waypoints[waypointIndex].position;

        // Cache components
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError($"{name} is missing an Animator component!");

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogError($"{name} is missing a SpriteRenderer component!");
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

        Vector3 targetPos = waypoints[waypointIndex].position;
        Vector3 currentPos = transform.position;

        // 1) Flip sprite based on horizontal direction
        float deltaX = targetPos.x - currentPos.x;
        if (Mathf.Abs(deltaX) > 0.01f)  // only if there’s noticeable horizontal movement
            spriteRenderer.flipX = (deltaX < 0);

        // 2) Move toward the next waypoint
        transform.position = Vector2.MoveTowards(
            currentPos,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        // Once we arrive, advance the index
        if ((Vector2)transform.position == (Vector2)targetPos)
            waypointIndex++;
    }
}
