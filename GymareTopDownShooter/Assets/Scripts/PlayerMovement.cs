using UnityEngine;
using System;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float lookOffsetSize;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashLength;
    [SerializeField] float dashCool;

    [SerializeField] Transform lowerBody;
    [SerializeField] Transform upperBody;

    Camera cam;
    PlayerHealth health;

    public bool canMove = true;
    public bool canTurn = true;
    Rigidbody2D rb;
    Vector2 MoveDir => InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
    Vector2 LookDir => InputSystem.actions.FindAction("Aim").ReadValue<Vector2>();
    bool dashDown => InputSystem.actions.FindAction("Dash").ReadValue<float>() > 0.5f ? true : false;
    bool dashHeld = false;
    bool canDash = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        health = GetComponent<PlayerHealth>();
    }
    IEnumerator Dash()
    {
        canMove = false;
        canDash = false;
        health.SetInvincible(true);
        Vector2 dashDir = MoveDir;
        Vector2 startPos = transform.position;
        Vector2 finalPos = startPos + MoveDir.normalized * dashLength;
        float lerpAmount = Time.deltaTime * dashSpeed;
        while (lerpAmount < 1)
        {
            print(lerpAmount);
            rb.MovePosition(Vector2.Lerp(startPos, finalPos, lerpAmount));
            lerpAmount += Time.deltaTime * dashSpeed;
            yield return null;
        }
        health.SetInvincible(false);
        canMove = true;
        yield return new WaitForSeconds(dashCool);
        canDash = true;
        print("can dash");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            rb.MovePosition((Vector2)transform.position + MoveDir * moveSpeed * Time.fixedDeltaTime);
            lowerBody.rotation = MoveDir == Vector2.zero ? lowerBody.rotation :
                Quaternion.Euler(0, 0, Mathf.LerpAngle(lowerBody.eulerAngles.z, Mathf.Atan2(MoveDir.y, MoveDir.x) * Mathf.Rad2Deg - 90, 0.5f));
        }

        
    }

    private void Update()
    {
        Vector2 res = new Vector2(Screen.width, Screen.height);
        Vector2 mouseWorld = cam.ScreenToWorldPoint(LookDir);
        Vector2 camOffset = (LookDir - res / 2) / res * lookOffsetSize;

        if (canTurn)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, transform.position + new Vector3(camOffset.x, camOffset.y, cam.transform.position.z), 0.7f);
            Vector2 lookVector = (mouseWorld - (Vector2)transform.position).normalized;
            upperBody.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(lookVector.y, lookVector.x) * Mathf.Rad2Deg - 90);
        }

        if (dashDown && !dashHeld && canDash)
        {
            print("dash");
            StartCoroutine(Dash());
        }

        dashHeld = dashDown;
    }


}
