using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Polyperfect.Animals;
using Polyperfect.Common;

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyMovementComponent  moveComponent;
    public EnemyAnimatorComponent  enemyAnimator;
    HealthBar               healthBarScript;
    Animal_WanderScript     wanderscript;
    IdleState[]             idleStates;
    MovementState[]         movementStates;
    AIState[]               attackingStates;
    private AIState[]       deathStates;
    public float            walkAnimationSpeed = 1;
    public float             runAnimaitonSpeed = 2;
    public float            distanceToPlayer = 0;
    public bool             isWandering = false;
    //Move to a StatPage class
    public int              currentHealth = 90;
    public int              maxHealth  = 100;

    void Awake()
    {
        enemyAnimator = this.GetComponent<EnemyAnimatorComponent>();
        healthBarScript = this.GetComponentInChildren<HealthBar>();
        moveComponent = this.GetComponent<EnemyMovementComponent>();
    }
    protected virtual void Start()
    {
        if (!moveComponent)
            Debug.LogError(this.name + " is missing a EnemyMoveComponent!");
        if (!enemyAnimator)
            Debug.LogError(this.name + " is missing a EnemyAnimatorComponent!");
        if (!healthBarScript)
            Debug.LogError(this.name + " is missing a HealthBarScript!");
    }

    public int receiveDamage(Dictionary<string, int> dmgVals)
    {
        int damageAmount = dmgVals["damage"];
        int knockBackForce = dmgVals["knockback"];

        currentHealth -= damageAmount;
        updateHealthBar();
        if (currentHealth <= 0)
        {
            feint();
            return 0;
        }
        moveComponent.inHitStun = true;
        moveComponent.knockBackForce = knockBackForce;
        //Knockback(knockBackForce);
        return currentHealth;
    }

    //Update floating healthbar in world space.
    public void updateHealthBar()
    {
         healthBarScript.setHealth(currentHealth, maxHealth);
    }

    //Kill/death command, despawn and drop loot.
    public void feint()
    {
        Destroy(gameObject);
    }

}
