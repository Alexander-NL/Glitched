using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
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
        // Get the mouse position in world coordinates
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Calculate the direction from the player to the mouse
        Vector2 shootDirection = (mousePosition - (Vector2)firePoint.position).normalized;

        // Determine the shooting direction (Up, Down, Side)
        string direction = GetDirectionString(shootDirection);

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
        if (direction == "Left" || direction == "Right")
        {
            animator.SetTrigger("Shoot_Side"); // Use the same animation for both left and right
        }
        else
        {
            animator.SetTrigger("Shoot_" + direction); // Use Up or Down animations
        }

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Set the bullet's velocity
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = shootDirection * bulletSpeed;

        // Optional: Rotate the bullet to face the shooting direction
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Decrease ammo
        S.M2_Ammo--;
    }

    private string GetDirectionString(Vector2 direction)
    {
        // Determine the direction string based on the angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle >= 45 && angle < 135) // Up
            return "Up";
        else if (angle >= -135 && angle < -45) // Down
            return "Down";
        else if (angle >= -45 && angle < 45) // Right
            return "Right";
        else // Left
            return "Left";
    }
}