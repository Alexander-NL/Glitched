using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    [Header("Dash Settings")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Vector2 movementInput;
    private Rigidbody2D rb;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private bool isDashing = false;
    private bool canDash = true;
    private int enemyLayer;
    private int spikelayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyLayer = LayerMask.NameToLayer("Enemy");
        spikelayer = LayerMask.NameToLayer("Spike");
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        if (movementInput != Vector2.zero)
        {
            TryMove(movementInput);
        }
    }

    private bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);
        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        return false;
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnDash()
    {
        if (canDash && movementInput != Vector2.zero)
        {
            Debug.Log("DASHING");
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        // Temporarily ignore collision with "Enemy" layer
        Physics2D.IgnoreLayerCollision(gameObject.layer, enemyLayer, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, spikelayer, true);

        rb.velocity = movementInput.normalized * dashSpeed;
        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero;
        isDashing = false;

        // Re-enable collision with "Enemy" layer
        Physics2D.IgnoreLayerCollision(gameObject.layer, enemyLayer, false);
        Physics2D.IgnoreLayerCollision(gameObject.layer, spikelayer, false);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
