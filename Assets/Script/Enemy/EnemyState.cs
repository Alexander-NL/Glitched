using UnityEngine;

public class EnemyAnimationStateMachine : MonoBehaviour
{
    // Reference to the Animator component
    private Animator animator;

    // Reference to other enemy scripts
    public EnemyPatrol enemyPatrol;
    public EnemyChase enemyChase;
    public EnemyAttack enemyAttack;
    public EnemyStats ES;

    private Vector2 attackDirection;
    private Vector2 ChaseDirection;
    // Enum to represent enemy states
    private enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Dead,
        Damaged
    }

    private EnemyState currentState;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Determine the current state based on priority
        if (ES.Dead)
        {
            currentState = EnemyState.Dead;
        }
        else if (enemyAttack.AttackingPlayer)
        {
            currentState = EnemyState.Attack;

        }
        else if (enemyPatrol.isWaiting)
        {
            currentState = EnemyState.Idle;
        }
        else if (enemyAttack.isStunned)
        {
            currentState = EnemyState.Damaged;
        }
        else
        {
            currentState = EnemyState.Chase;
        }

            // Use a switch statement to handle animations based on the state
            switch (currentState)
            {
                case EnemyState.Dead:
                    UpdateAnimation();
                    break;

                case EnemyState.Attack:
                    UpdateAnimation();
                    break;

                case EnemyState.Idle:
                    UpdateAnimation();
                    break;

                case EnemyState.Chase:
                    UpdateAnimation();
                break;

                case EnemyState.Damaged:
                     UpdateAnimation();
                break;

            default:
                    Debug.LogWarning("Unknown enemy state.");
                    break;
            }
    }

    // Method to update the animation based on the current state
    private void UpdateAnimation()
    {
        switch (currentState)
        {
            case EnemyState.Dead:
                animator.Play("Dead");
                break;

            case EnemyState.Attack:
                attackDirection = enemyAttack.CalculateAttackDirection();
                animator.SetFloat("AttackDirection", enemyAttack.GetDirectionValue(attackDirection));
                animator.SetTrigger("Attack");

                break;

            case EnemyState.Idle:
                animator.Play("Idle_Side");
                break;

            case EnemyState.Chase:
                ChaseDirection = enemyChase.CalculateDirection();
                animator.SetFloat("ChaseDirection", enemyChase.GetDirectionValue(ChaseDirection));
                animator.SetTrigger("Chase");
                break;

            case EnemyState.Damaged:
                animator.Play("Damaged");
                break;

            default:
                Debug.LogWarning("Unknown enemy state.");
                break;
        }
    }
}