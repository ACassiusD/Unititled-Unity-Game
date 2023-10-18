using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyMovementComponent moveComponent;
    public EnemyAnimatorComponent enemyAnimator;
    private EnemyCombatComponent enemyCombatComponent;
    private EnemyStats EnemyStats;
    protected GameObject activeAOEObject = null;
    public GameObject floatingDmgText;
    public GameObject aoeObject;
    private HealthBar healthBarScript;

    //Comaat vars to be moved into compat component
    public bool attacked = false; // TODO: State machine.
    public bool attackOnCooldown = false;
    public float distanceToPlayer = 0;
    public float attackCooldown = 5f;
    public float attackCooldownTimer = 5f;

    void Awake()
    {
        enemyAnimator = this.GetComponent<EnemyAnimatorComponent>();
        healthBarScript = this.GetComponentInChildren<HealthBar>();
        moveComponent = this.GetComponent<EnemyMovementComponent>();
        EnemyStats = this.GetComponent<EnemyStats>();
        enemyCombatComponent = this.GetComponent<EnemyCombatComponent>();
        enemyCombatComponent.Initialize(EnemyStats);
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

    public float ReceiveDamage(float damageAmount, int knockBackForce, Vector3 direction = new Vector3())
    {
        // set attacked state to true
        attacked = true;

        // Update the health using the StatsComponent's method
        enemyCombatComponent.TakeDamage(damageAmount);

        //Update Healthbar in UI
        UpdateFloatingHealthBarUI();

        // Set Knockback to the movement component.
        moveComponent.DoKnockback(knockBackForce, direction);

        return EnemyStats.currentHealth;
    }


    //Update floating healthbar in world space.
    public void UpdateFloatingHealthBarUI()
    {
        if (floatingDmgText)
        {
            var floatingDamageText = Instantiate(floatingDmgText, transform.position, this.transform.rotation, transform);
            floatingDamageText.GetComponent<TextMeshPro>().text = EnemyStats.currentHealth.ToString();
        }
         healthBarScript.setHealth(EnemyStats.currentHealth, EnemyStats.maxHealth);
    }

    //Kill/death command, despawn and drop loot.
    public void Feint()
    {
        Destroy(gameObject);
    }

    //TODO: State code goes in state machines.
    public void resetAttackState()
    {
        attacked = false;
    }

    public void TestFunction()
    {
        if(attackOnCooldown == false)
        {
            activeAOEObject = Instantiate(aoeObject);
            activeAOEObject.transform.position = this.transform.position;
            attackOnCooldown = true;
        }
    }
}
