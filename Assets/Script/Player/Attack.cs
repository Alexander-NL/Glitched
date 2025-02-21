using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    [Header("Stats Script")]
    public Stats S;  // Put Stat.cs here

    private float chargeTimer = 0f;
    private bool isCharging = false;
    private int currentStage = 0;
    private int basicAttackCount = 0; // Counter for basic attacks
    private bool isBasicAttackOnCooldown = false; // Cooldown flag for basic attacks
    private float lastBasicAttackTime = 0f; // Time since the last basic attack
    private float comboResetTime = 2f; // Time before the combo resets

    private PlayerInput playerInput;
    private InputAction attackAction;

    private Animator animator; // Reference to the Animator component

    private void Awake()
    {
        // Get the PlayerInput component
        playerInput = GetComponent<PlayerInput>();

        // Get the attack action
        attackAction = playerInput.actions["Attack"]; // Ensure this action is bound to Mouse 1

        // Get the Animator component
        animator = GetComponent<Animator>();
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

        string animationTrigger = "BasicAttack" + basicAttackCount;
        //animator.SetTrigger(animationTrigger);

        Debug.Log("Basic Attack " + basicAttackCount + "! Damage: " + S.M1_Damage);

        if (basicAttackCount >= S.M1_ComboMax)
        {
            isBasicAttackOnCooldown = true;
            basicAttackCount = 0; // Reset the counter
            yield return new WaitForSeconds(S.M1_Delay); // Apply the delay
            isBasicAttackOnCooldown = false; // End the cooldown
        }
    }

    private void PerformChargedAttack(int stage)
    {
        float damage = 0f;
        switch (stage)
        {
            case 1:
                damage = 2 * S.M1_Damage;
                break;
            case 2:
                damage = 2 * S.M1_Damage * 1.5f;
                break;
            case 3:
                damage = 2 * S.M1_Damage * 2;
                break;
        }

        Debug.Log("Charged Attack Stage " + stage + "! Damage: " + damage);
        //check if enemy is in player range
        //if enemy in player range deal damage and run animation
        //if not still run player attack and maybe stop player movement
    }

    private void OnChargeStageReached(int stage)
    {
        // Optional: Add visual/audio feedback for reaching a new stage
        Debug.Log("Reached Stage " + stage);

        // Example: Play a sound or update a UI charge bar
        // AudioManager.Instance.Play("ChargeStage" + stage);
        // UIManager.Instance.UpdateChargeBar(stage);
    }
}