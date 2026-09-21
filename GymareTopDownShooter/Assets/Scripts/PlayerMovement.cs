using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float lookOffsetSize;

    [SerializeField] Transform lowerBody;
    [SerializeField] Transform upperBody;

    [SerializeField] Camera cam;

    public bool canMove = true;
    public bool canTurn = true;
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
        lowerBody.rotation = MoveDir == Vector2.zero ? lowerBody.rotation : Quaternion.Euler(0, 0, Mathf.LerpAngle(lowerBody.eulerAngles.z, Mathf.Atan2(MoveDir.y, MoveDir.x) * Mathf.Rad2Deg - 90, 0.3f));

        Vector2 res = new Vector2(Screen.width, Screen.height);

        Vector2 mouseWorld = cam.ScreenToWorldPoint(LookDir);

        Vector2 camOffset = (LookDir - res / 2) / res * lookOffsetSize;
        print(camOffset);
        cam.transform.position = transform.position + new Vector3(camOffset.x, camOffset.y, cam.transform.position.z);
        //Vector2 lookVector = (LookDir - res / 2) + (Vector2)(cam.transform.position - transform.position).normalized;

        Vector2 lookVector = (mouseWorld - (Vector2)transform.position).normalized;



        //print(res);
        upperBody.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(lookVector.y, lookVector.x) * Mathf.Rad2Deg - 90);
    }
}
