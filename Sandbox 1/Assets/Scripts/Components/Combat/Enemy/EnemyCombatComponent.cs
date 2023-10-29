using UnityEngine;

public class EnemyCombatComponent : CombatComponent
{
    //Combat State Machine + States
    public StateMachine combatStateMachine;
    private EnemyIdleCombatState enemyIdleCombatState;
    private EnemyLeapAndCrashCombatState enemyLeapAndCrashCombatState;

    //For AOE attack
    public bool attackOnCooldown = false;
    public GameObject aoeObject;
    protected GameObject aoeObjectInstance = null;
    public float attackCooldown = 5f;
    public float attackCooldownTimer = 5f;

    void Start()
    {        
        //Intantiate the state machine.
        combatStateMachine = new StateMachine();

        //Initialize all combat states that will be used in the state machine.
        enemyIdleCombatState = new EnemyIdleCombatState(combatStateMachine);
        enemyLeapAndCrashCombatState = new EnemyLeapAndCrashCombatState(combatStateMachine);

        //Initialize state machine with its default state.
        combatStateMachine.Initialize(enemyIdleCombatState);
    }

    //Deal with cooldowns for aoe
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

    public void CastAOEAttack()
    {
        if (attackOnCooldown == false)
        {
            aoeObjectInstance = Instantiate(aoeObject);
            aoeObjectInstance.transform.position = this.transform.position;
            attackOnCooldown = true;
        }
    }

    public override void Feint()
    {
        Destroy(gameObject);
    }
}
