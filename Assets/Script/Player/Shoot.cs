using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [Header("Shooting Settings")]
    public Stats S; //Put Stat.cs here
    public GameObject bulletPrefab; // Prefab of the bullet
    public float bulletSpeed = 10f; // Speed of the bullet
    public Transform firePoint; // Point where the bullet spawns
    public float shootCooldown = 1.5f; // Cooldown time in seconds

    private InputAction shootAction; // Reference to the shoot action
    private float lastShootTime; // Time when the player last shot

    private void Awake()
    {
        // Get the shoot action from the Input Action Asset
        var playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions["Shoot"];
    }

    private void OnShoot()
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

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Set the bullet's velocity
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = shootDirection * bulletSpeed;

        // Optional: Rotate the bullet to face the shooting direction
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        S.M2_Ammo--;
    }
}