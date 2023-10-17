using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyMovementComponent moveComponent;
    public EnemyAnimatorComponent enemyAnimator;
    private EnemyStats stats;
    protected GameObject activeAOEObject = null;
    public GameObject floatingDmgText;
    public GameObject aoeObject;
    private HealthBar healthBarScript;

    //Comaat vars to be moved into compat component
    public bool attacked = false; //This needs to be refactored.
    public bool attackOnCooldown = false;
    public float distanceToPlayer = 0;
    public float attackCooldown = 5f;
    public float attackCooldownTimer = 5f;

    void Awake()
    {
        enemyAnimator = this.GetComponent<EnemyAnimatorComponent>();
        healthBarScript = this.GetComponentInChildren<HealthBar>();
        moveComponent = this.GetComponent<EnemyMovementComponent>();
        stats = this.GetComponentInChildren<EnemyStats>();
    }

    void Update()
    {
        if (attackOnCooldown)
        {
            attackCooldownTimer -= Time.deltaTime;
            if (attackCooldownTimer <= 0)
            {
                attackOnCooldown = false;
                attackCooldownTimer = attackCooldown;
            }
        }
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

    public float receiveDamage(float damageAmount, int knockBackForce, Vector3 direction = new Vector3())
    {
        // set attacked state to true
        attacked = true;

        // Update the health using the StatsComponent's method
        stats.TakeDamage(damageAmount);

        // Kill entity if health is now 0
        if (stats.IsDead())
        {
            feint();
            return 0;
        }
        else
        {
            // Update UI Health bar
            updateHealthBar();
        }

        // Set Knockback to the movement component.
        moveComponent.knockBackForce = knockBackForce;
        moveComponent.knockBackDirection = direction;
        moveComponent.movementStateMachine.ChangeState(moveComponent.knockback);

        return stats.currentHealth;
    }

    //Update floating healthbar in world space.
    public void updateHealthBar()
    {
        if (floatingDmgText)
        {
            var floatingDamageText = Instantiate(floatingDmgText, transform.position, this.transform.rotation, transform);
            floatingDamageText.GetComponent<TextMeshPro>().text = stats.currentHealth.ToString();
        }
         healthBarScript.setHealth(stats.currentHealth, stats.maxHealth);
    }

    //Kill/death command, despawn and drop loot.
    public void feint()
    {
        Destroy(gameObject);
    }

    public void resetAttackedState()
    {
        attacked = false;
    }

    public void testFunction()
    {
        if(attackOnCooldown == false)
        {
            activeAOEObject = Instantiate(aoeObject);
            activeAOEObject.transform.position = this.transform.position;
            attackOnCooldown = true;
        }
    }
}
