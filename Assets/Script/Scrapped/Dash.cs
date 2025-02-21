using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private bool isDashing = false;
    private bool canDash = true;
    private NewMovement movement;
    private Rigidbody2D rb;

    void Start()
    {
        movement = GetComponent<NewMovement>();
        rb = movement.GetRigidbody();
    }

    void OnDash()
    {
        if (canDash && movement.GetMovementInput() != Vector2.zero)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;
        float startTime = Time.time;
        Vector2 dashDirection = movement.GetMovementInput().normalized;

        while (Time.time < startTime + dashDuration)
        {
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
            yield return null;
        }

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
