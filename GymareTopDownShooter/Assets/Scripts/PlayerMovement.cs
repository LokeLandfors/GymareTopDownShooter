using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Rigidbody2D rb;
    Vector2 MoveDir => InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
    Vector2 LookDir => InputSystem.actions.FindAction("Aim").ReadValue<Vector2>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition((Vector2)transform.position + MoveDir * moveSpeed * Time.fixedDeltaTime);
        print(LookDir);
    }
}
