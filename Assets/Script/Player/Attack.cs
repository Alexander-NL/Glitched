using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    [Header("Stats Script")]
    public Stats S;  // Reference to the Stats script

    [Header("Hitboxes")]
    public GameObject[] basicAttackHitboxes; // Array of hitboxes for basic attacks (one for each stage)
    public GameObject[] chargedAttackHitboxes; // Array of hitboxes for charged attacks (one for each stage)

    [Header("Hitbox Duration")]
    public float hitboxDuration = 0.2f; // Duration the hitbox stays active (editable in Inspector)

    public float damage;
    private float chargeTimer = 0f;
    private bool isCharging = false;
    private int currentStage = 0;
    private int basicAttackCount = 0; // Counter for basic attacks
    private bool isBasicAttackOnCooldown = false; // Cooldown flag for basic attacks
    private float lastBasicAttackTime = 0f; // Time since the last basic attack
    private float comboResetTime = 0.5f; // Time before the combo resets

    private PlayerInput playerInput;
    private InputAction attackAction;

    public Animator animator; // Reference to the Animator component
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    private Vector2 attackDirection; // Store the attack direction when the attack is initiated
    public AudioPlayer AP;

    private void Awake()
    {
        // Get the PlayerInput component
        playerInput = GetComponent<PlayerInput>();

        // Get the attack action
        attackAction = playerInput.actions["Attack"]; // Ensure this action is bound to Mouse 1

        // Ensure all hitboxes are initially disabled
        DisableAllHitboxes();
    }

    private void OnEnable()
    {
        // Enable the attack action
        attackAction.Enable();
    }

    private void OnDisable()
    {
        // Disable the attack action
        attackAction.Disable();
    }

    private void Update()
    {
        HandleAttack();

        // Check if the combo should reset
        if (Time.time - lastBasicAttackTime > comboResetTime && basicAttackCount > 0)
        {
            basicAttackCount = 0;
            Debug.Log("Combo Reset!");
            RevertToIdle();
        }
    }

    private void HandleAttack()
    {
        // Check if the attack button is pressed and basic attacks are not on cooldown
        if (attackAction.IsPressed() && !isBasicAttackOnCooldown)
        {
            if (!isCharging)
            {
                // Start charging
                isCharging = true;
                chargeTimer = 0f;
                currentStage = 0;

                // Store the attack direction when the attack is initiated
                attackDirection = CalculateAttackDirection();

                // Set the AttackDirection parameter in the Animator
                animator.SetFloat("AttackDirection", GetDirectionValue(attackDirection));

                // Flip the sprite based on the attack direction
                if (attackDirection.x < 0)
                    spriteRenderer.flipX = true; // Flip the sprite to face left
                else if (attackDirection.x > 0)
                    spriteRenderer.flipX = false; // Ensure the sprite faces right
            }

            // Increment the charge timer
            chargeTimer += Time.deltaTime;

            // Check for stage progression
            if (chargeTimer >= S.M1_ChargeTime * (currentStage + 1))
            {
                // Limit the current stage to a maximum of 3
                if (currentStage < 3)
                {
                    currentStage++;
                    Debug.Log("Charged to Stage " + currentStage);

                    string animationTrigger = "ChargedAttack" + currentStage;
                    animator.SetTrigger(animationTrigger);

                    // Optional: Trigger visual/audio feedback for reaching a new stage
                    OnChargeStageReached(currentStage);
                }
            }
        }
        else if (isCharging)
        {
            // Button released, perform the appropriate attack based on the current stage
            if (currentStage > 0)
            {
                PerformChargedAttack(currentStage);
            }
            else
            {
                StartCoroutine(PerformBasicAttack());
            }

            // Reset charging state
            isCharging = false;
            chargeTimer = 0f;
            currentStage = 0;
        }
    }

    IEnumerator PerformBasicAttack()
    {
        basicAttackCount++;
        lastBasicAttackTime = Time.time;

        // Trigger the appropriate basic attack animation
        string animationTrigger = "BasicAttack" + basicAttackCount;
        animator.SetTrigger(animationTrigger); // Set the trigger

        Debug.Log("Basic Attack " + basicAttackCount + "! Damage: " + S.M1_Damage);

        // Activate the appropriate hitbox based on direction and stage
        int directionIndex = GetDirectionValue(attackDirection);
        GameObject hitbox = GetHitboxForBasicAttack(directionIndex, basicAttackCount);

        if (hitbox != null)
        {
            OnBasicAttackReached(basicAttackCount);
            hitbox.SetActive(true);
            yield return new WaitForSeconds(hitboxDuration); // Use the editable hitbox duration
            hitbox.SetActive(false);
        }

        if (basicAttackCount >= S.M1_ComboMax)
        {
            isBasicAttackOnCooldown = true;
            basicAttackCount = 0; // Reset the counter
            yield return new WaitForSeconds(S.M1_Delay); // Apply the delay
            isBasicAttackOnCooldown = false; // End the cooldown
        }
    }

    public void PerformChargedAttack(int stage)
    {
        string animationTrigger = "C_Attack" + stage;
        animator.SetTrigger(animationTrigger);

        // Activate the appropriate hitbox based on direction and stage
        int directionIndex = GetDirectionValue(attackDirection);
        GameObject hitbox = GetHitboxForChargedAttack(directionIndex, stage);

        if (hitbox != null)
        {
            AP.ChargedAttackPlay();
            hitbox.SetActive(true);
            StartCoroutine(DeactivateHitboxAfterDelay(hitbox, hitboxDuration)); // Use the editable hitbox duration
        }
    }

    private IEnumerator DeactivateHitboxAfterDelay(GameObject hitbox, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (hitbox != null)
            hitbox.SetActive(false);
    }

    private void RevertToIdle()
    {
        // Reset the basic attack counter and revert to Idle
        basicAttackCount = 0;
        animator.SetTrigger("RevertToIdle"); // Trigger the Idle state
        Debug.Log("Reverted to Idle due to inactivity.");
    }

    private Vector2 CalculateAttackDirection()
    {
        // Example: Use mouse position to determine attack direction
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
        return direction;
    }

    private int GetDirectionValue(Vector2 direction)
    {
        // Determine the direction index based on the angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle >= 45 && angle < 135) // Up
            return 0;
        else if (angle >= -135 && angle < -45) // Down
            return 1;
        else if (angle >= -135 && angle < 135) //Left
            return 2;
        else //Right
            return 3;
    }

    private GameObject GetHitboxForBasicAttack(int directionIndex, int stage)
    {
        // Calculate the index for the hitbox array
        int index = directionIndex * S.M1_ComboMax + (stage - 1);
        if (index >= 0 && index < basicAttackHitboxes.Length)
            return basicAttackHitboxes[index];
        return null;
    }

    private GameObject GetHitboxForChargedAttack(int directionIndex, int stage)
    {
        // Calculate the index for the hitbox array
        int index = directionIndex * 3 + (stage - 1); // Assuming 3 stages for charged attacks
        if (index >= 0 && index < chargedAttackHitboxes.Length)
            return chargedAttackHitboxes[index];
        return null;
    }

    private void DisableAllHitboxes()
    {
        // Disable all basic attack hitboxes
        foreach (var hitbox in basicAttackHitboxes)
        {
            if (hitbox != null)
                hitbox.SetActive(false);
        }

        // Disable all charged attack hitboxes
        foreach (var hitbox in chargedAttackHitboxes)
        {
            if (hitbox != null)
                hitbox.SetActive(false);
        }
    }
    private void OnBasicAttackReached(int basicAttackCount)
    {
        switch (basicAttackCount)
        {
            case 1:
                AP.BasicAttack1Play();
                Debug.Log("Attack1 Sound Play");
                break;
            case 2:
                AP.BasicAttack2Play();
                Debug.Log("Attack2s Sound Play");
                break;
            case 3:
                AP.BasicAttack3Play();
                break;
        }
    }
    private void OnChargeStageReached(int stage)
    {
        switch (stage)
        {
            case 1:
                AP.ChargedRamp1Play();
                Debug.Log("Test");
                break;
            case 2:
                AP.ChargedRamp2Play();
                break;
            case 3:
                AP.ChargedRamp3Play();
                break;
        }
    }
}