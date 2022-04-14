using UnityEngine.AI;
using Polyperfect.Animals;


public class EnemyMovementComponent : MoveComponent
{
    public EnemyStandingState standing;
    public EnemyChasingState chasing;
    public NavMeshAgent naveMeshAgent;
    public Animal_WanderScript wanderscript;

    void Start()
    {
        playerScript = PlayerManager.Instance.getPlayerScript();
        getNavMesh();
        getWanderScript();
        standing = new EnemyStandingState(stateMachine, this);
        chasing = new EnemyChasingState(stateMachine, this);
        stateMachine.Initialize(standing);
    }

    Animal_WanderScript getWanderScript()
    {
        if(wanderscript == null)
        {
            wanderscript = this.GetComponent<Animal_WanderScript>();
        }
        return wanderscript;
    }

    NavMeshAgent getNavMesh()
    {
        if (wanderscript == null)
        {
            naveMeshAgent = this.GetComponent<NavMeshAgent>();
        }
        return naveMeshAgent;
    }
}
