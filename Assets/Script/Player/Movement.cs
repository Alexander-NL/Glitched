using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    enum PlayerState { Idle, Running, Dashing, DownRun, UpRun, DashUp, DashDown }
    PlayerState currentState;

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
    public bool canDash = true;
    private int enemyLayer;
    private int spikelayer;
    private int DeadBlocks;

    public AudioPlayer AP;

    // Reference to the Animator component
    private Animator animator;
    public SpriteRenderer spriteRenderer;

    // Store the default local scale for flipping
    private Vector3 defaultLocalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyLayer = LayerMask.NameToLayer("Enemy");
        spikelayer = LayerMask.NameToLayer("Spike");
        DeadBlocks = LayerMask.NameToLayer("DeadBlocks");

        // Get the Animator component
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject.");
        }

        // Store the default local scale for flipping
        defaultLocalScale = transform.localScale;

        // Initialize state
        currentState = PlayerState.Idle;
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        if (movementInput != Vector2.zero)
        {
            TryMove(movementInput);

            // Determine the state based on movement direction
            if (Mathf.Abs(movementInput.y) > Mathf.Abs(movementInput.x))
            {
                // Vertical movement is dominant
                if (movementInput.y > 0)
                {
                    currentState = PlayerState.UpRun;
                }
                else if (movementInput.y < 0)
                {
                    currentState = PlayerState.DownRun;
                }
            }
            else
            {
                // Horizontal movement is dominant
                currentState = PlayerState.Running;

                // Flip the sprite based on movement direction
                if (movementInput.x > 0) // Moving right
                {
                    spriteRenderer.flipX = false;
                }
                else if (movementInput.x < 0) // Moving left
                {
                    spriteRenderer.flipX = true;
                }
            }

            UpdateAnimation();
        }
        else
        {
            if (currentState != PlayerState.Idle)
            {
                currentState = PlayerState.Idle;
                UpdateAnimation();
            }
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
        AP.FootstepSoundPlay();
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
        AP.DashSoundPlay();

        // Determine the dash direction and set the appropriate state
        if (Mathf.Abs(movementInput.y) > Mathf.Abs(movementInput.x))
        {
            // Vertical dash
            if (movementInput.y > 0)
            {
                currentState = PlayerState.DashUp;
            }
            else if (movementInput.y < 0)
            {
                currentState = PlayerState.DashDown;
            }
        }
        else
        {
            // Horizontal dash
            currentState = PlayerState.Dashing;
        }

        UpdateAnimation();

        // Temporarily ignore collision with "Enemy" and "Spike" layers
        Physics2D.IgnoreLayerCollision(gameObject.layer, enemyLayer, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, spikelayer, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, DeadBlocks, true);

        rb.velocity = movementInput.normalized * dashSpeed;
        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero;
        isDashing = false;

        // Re-enable collision with "Enemy" and "Spike" layers
        Physics2D.IgnoreLayerCollision(gameObject.layer, enemyLayer, false);
        Physics2D.IgnoreLayerCollision(gameObject.layer, spikelayer, false);
        Physics2D.IgnoreLayerCollision(gameObject.layer, DeadBlocks, false);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

        // Return to Idle or Running state after dash
        currentState = movementInput != Vector2.zero ? PlayerState.Running : PlayerState.Idle;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        // Update the Animator based on the current state
        switch (currentState)
        {
            case PlayerState.Idle:
                animator.Play("Idle");
                break;
            case PlayerState.Running:
                animator.Play("Running");
                break;
            case PlayerState.Dashing:
                animator.Play("Dashing");
                break;
            case PlayerState.DownRun:
                animator.Play("DownRun");
                break;
            case PlayerState.UpRun:
                animator.Play("UpRun");
                break;
            case PlayerState.DashUp:
                animator.Play("DashUp");
                break;
            case PlayerState.DashDown:
                animator.Play("DashDown");
                break;
        }
    }

    //public void Reset()
    //{
    //    canDash = true;
    //}
}