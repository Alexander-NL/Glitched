using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ShootScrapped : MonoBehaviour
{
    [Header("Shooting Settings")]
    public Stats S; // Reference to the Stats script
    public GameObject bulletPrefab; // Prefab of the bullet
    public float bulletSpeed = 10f; // Speed of the bullet
    public Transform firePoint; // Point where the bullet spawns
    public float shootCooldown = 1.5f; // Cooldown time in seconds

    private InputAction shootAction; // Reference to the shoot action
    private float lastShootTime; // Time when the player last shot

    private Animator animator; // Reference to the Animator component
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    public AudioPlayer AP;
    private void Awake()
    {
        // Get the shoot action from the Input Action Asset
        var playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions["Shoot"];

        // Get the Animator and SpriteRenderer components
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // Enable the shoot action and subscribe to the performed event
        shootAction.performed += OnShootPerformed;
        shootAction.Enable();
    }

    private void OnDisable()
    {
        // Disable the shoot action and unsubscribe from the performed event
        shootAction.performed -= OnShootPerformed;
        shootAction.Disable();
    }

    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        // Check if enough time has passed since the last shot
        if (S.M2_Ammo > 0)
        {
            if (Time.time - lastShootTime >= shootCooldown)
            {
                Debug.Log("Shoot action performed!");
                Shooting();
                lastShootTime = Time.time; // Update the last shoot time
            }
            else
            {
                Debug.Log("Shoot is on cooldown!");
            }
        }
        else
        {
            Debug.Log("Ammo Empty");
        }
    }

    private void Shooting()
    {
        AP.ShootSoundPlay();
        // Get the mouse position in world coordinates
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Calculate the direction from the player to the mouse
        Vector2 shootDirection = (mousePosition - (Vector2)firePoint.position).normalized;

        // Determine the shooting direction (Up, Down, Side)
        string direction = GetDirectionString(shootDirection);

        // Store the original flip state
        bool originalFlipState = spriteRenderer.flipX;

        // Flip the character if shooting to the left
        if (shootDirection.x < 0)
        {
            spriteRenderer.flipX = true; // Flip the sprite horizontally
        }
        else if (shootDirection.x > 0)
        {
            spriteRenderer.flipX = false; // Reset the flip
        }

        // Trigger the appropriate shooting animation
        animator.SetTrigger("Shoot_" + direction);

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Set the bullet's velocity
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = shootDirection * bulletSpeed;

        // Optional: Rotate the bullet to face the shooting direction
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Temporarily disable the bullet's collider to avoid immediate collisions
        Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
        if (bulletCollider != null)
        {
            bulletCollider.enabled = false;
            StartCoroutine(EnableColliderAfterDelay(bulletCollider, 0.1f)); // Re-enable after 0.1 seconds
        }

        // Decrease ammo
        S.M2_Ammo--;

        // Wait for the animation to complete before reverting the flip state
        StartCoroutine(ResetFlipStateAfterAnimation(originalFlipState));
    }

    private IEnumerator ResetFlipStateAfterAnimation(bool originalFlipState)
    {
        // Wait for the duration of the shooting animation
        yield return new WaitForSeconds(0.5f); // Adjust this value to match your animation's length

        // Revert the sprite's flip state to its original value
        spriteRenderer.flipX = originalFlipState;
    }

    private IEnumerator EnableColliderAfterDelay(Collider2D collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
    }

    private string GetDirectionString(Vector2 direction)
    {
        // Determine the direction string based on the angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle >= 45 && angle < 135) // Up
            return "Up";
        else if (angle >= -135 && angle < -45) // Down
            return "Down";
        else // Side (left or right)
            return "Side";
    }
}