using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] Transform[] points;
    [SerializeField] Transform lowerBody;
    [SerializeField] float speed = 2f;
    [SerializeField] float waitTime = 1f;
    [SerializeField] float arriveDistance = 0.1f;

    Rigidbody2D rb;
    int index;
    float waitTimer;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void FixedUpdate()
    {
        if (points.Length == 0) return;

        // Waiting at a point
        if (waitTimer > 0f)
        {
            waitTimer -= Time.fixedDeltaTime;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 toTarget = (Vector2)points[index].position - rb.position;

        if (toTarget.magnitude <= arriveDistance)
        {
            index = (index + 1) % points.Length;
            waitTimer = waitTime;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 dir = toTarget.normalized;
        rb.linearVelocity = dir * speed;

        // Face movement direction (-90 if your sprite points up)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        lowerBody.rotation = Quaternion.Euler(0, 0, angle);
    }
}