using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyMovementComponent moveComponent;
    public EnemyAnimatorComponent enemyAnimator;
    HealthBar healthBarScript;
    public float walkAnimationSpeed = 1;
    public float runAnimaitonSpeed = 2;
    public float distanceToPlayer = 0;
    public bool isWandering = false;
    //Move to a StatPage class
    public int currentHealth = 90;
    public int maxHealth = 100;
    public bool attacked = false;
    protected GameObject activeAOEObject = null;
    public GameObject aoeObject;
    public float attackCooldown = 5f;
    public  float attackCooldownTimer = 5f;
    public bool attackOnCooldown = false;
    public GameObject floatingDmgText;

    void Awake()
    {
        enemyAnimator = this.GetComponent<EnemyAnimatorComponent>();
        healthBarScript = this.GetComponentInChildren<HealthBar>();
        moveComponent = this.GetComponent<EnemyMovementComponent>();
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

    public int receiveDamage(int damageAmount, int knockBackForce, Vector3 direction = new Vector3())
    {
        attacked = true;
        currentHealth -= damageAmount;
        updateHealthBar();
        if (currentHealth <= 0)
        {
            feint();
            return 0;
        }
        moveComponent.knockBackForce = knockBackForce;
        moveComponent.knockBackDirection = direction;
        moveComponent.stateMachine.ChangeState(moveComponent.knockback);
        return currentHealth;
    }

    //Update floating healthbar in world space.
    public void updateHealthBar()
    {
        if (floatingDmgText)
        {
            var floatingDamageText = Instantiate(floatingDmgText, transform.position, this.transform.rotation, transform);
            floatingDamageText.GetComponent<TextMeshPro>().text = currentHealth.ToString();
        }
         healthBarScript.setHealth(currentHealth, maxHealth);
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
