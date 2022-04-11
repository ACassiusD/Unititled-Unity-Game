using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Polyperfect.Animals;
using Polyperfect.Common;

public class Enemy : MonoBehaviour, IDamageable
{
    EnemyMovementComponent moveComponent;
    EnemyAnimatorComponent enemyAnimator;
    HealthBar healthBarScript;
    Animal_WanderScript wanderscript;
    IdleState[] idleStates;
    MovementState[] movementStates;
    AIState[] attackingStates;
    private AIState[] deathStates;
    protected float walkAnimationSpeed = 1;
    protected float runAnimaitonSpeed = 2;
    public float distanceToPlayer = 0;
    public bool isWandering = false;
    public int currentHealth { get; set; } = 50;
    public int maxHealth { get; set; } = 100;
    public bool inHitStun { get; set; } = false;

    protected virtual void Start()
    {
        enemyAnimator = this.GetComponent<EnemyAnimatorComponent>();
        healthBarScript = this.GetComponentInChildren<HealthBar>();
        moveComponent = this.GetComponent<EnemyMovementComponent>();
        if (!moveComponent)
            Debug.LogError(this.name + " is missing a EnemyMoveComponent!");
        if (!enemyAnimator)
            Debug.LogError(this.name + " is missing a EnemyAnimatorComponent!");
        if (!healthBarScript)
            Debug.LogError(this.name + " is missing a HealthBarScript!");
    }

    public int receiveDamage(Dictionary<string, int> dmgVals)
    {
        var damageAmount = dmgVals["damage"];
        var knockBackForce = dmgVals["knockback"];

        currentHealth -= damageAmount;
        updateHealthBar();
        if (currentHealth <= 0)
        {
            feint();
            return 0;
        }
        Knockback(knockBackForce);
        return currentHealth;
    }

    public void Knockback(float knockBackForce)
    {
        inHitStun = true;

        //Add Impact
        Vector3 direction = this.transform.forward * -1; //Need to make this direction
        Vector3 up = this.transform.up;
        up.Normalize();
        direction.Normalize();
        direction.y = up.y;
        var impact = Vector3.zero;
        impact += direction.normalized * knockBackForce;

        //Apply vector to object
        moveComponent.characterController.Move(impact * Time.deltaTime);
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

    private void updateAnimations()
    {
        enemyAnimator.ClearAnimation();

        if (moveComponent.isBeingControlled)
        {
            if (moveComponent.isMoving)
            {
                //Running
                if (moveComponent.isRunning)
                {
                    enemyAnimator.setRunningAnimation();
                }
                else//walking
                {
                    enemyAnimator.setWalkingAnimation();
                }
            }
        }
    }
}
