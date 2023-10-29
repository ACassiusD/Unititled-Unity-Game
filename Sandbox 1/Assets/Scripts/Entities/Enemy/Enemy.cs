using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyMovementComponent moveComponent;
    public EnemyAnimatorComponent enemyAnimator;
    public EnemyCombatComponent enemyCombatComponent;
    private EnemyStats EnemyStats;
    private HealthBar healthBarScript;

    //Comaat vars to be moved into compat component
    public bool attacked = false; // TODO: State machine.
    public float distanceToPlayer = 0;

    void Awake()
    {
        enemyAnimator = this.GetComponent<EnemyAnimatorComponent>();
        healthBarScript = this.GetComponentInChildren<HealthBar>();
        moveComponent = this.GetComponent<EnemyMovementComponent>();
        EnemyStats = this.GetComponent<EnemyStats>();
        enemyCombatComponent = this.GetComponent<EnemyCombatComponent>();
        enemyCombatComponent.Initialize(EnemyStats);

        //Subscribe to component events.
        enemyCombatComponent.OnAttacked += onRecDam;
    }

    protected virtual void Start()
    {
        if (!moveComponent)
            Debug.LogError(this.name + " is missing a EnemyMoveComponent! ");

        if (!enemyAnimator)
            Debug.LogError(this.name + " is missing a EnemyAnimatorComponent!");

        if (!healthBarScript)
            Debug.LogError(this.name + " is missing a HealthBarScript!");
    }

    public void onRecDam(int? knockBackForce, Vector3? direction)
    {
        // set attacked state to true
        attacked = true;

        // Check if knockback is applicable
        if (knockBackForce.HasValue && direction.HasValue)
        {
            // Set Knockback to the movement component.
            moveComponent.DoKnockback(knockBackForce.Value, direction.Value);
        }
    }

    //TODO: State code goes in state machines.
    public void resetAttackState()
    {
        attacked = false;
    }
}
