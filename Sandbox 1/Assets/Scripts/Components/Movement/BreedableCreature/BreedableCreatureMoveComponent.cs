
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


/// <summary>
/// Movement Component contains logic to move entity in 3D space.
/// Possibly also contains state machine and states relevant to movement.
/// </summary>
public class BreedableCreatureMoveComponent : MoveComponent
{
    public BreedableCreatureWanderingState wandering;
    public BreedableCreatureIdleState idle;

    private NavMeshAgent navMeshAgent;
    public const float CONTINGENCY_DISTANCE = 1f;
    Vector3 wanderTarget;
    public Vector3 targetPosition;
    public float wanderZone = 10f; //"How far away from it's origin this animal will wander by itself.
    Vector3 startPosition;
    public float idleTimeMin = 5f;
    public float idleTimeMax = 15f;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        wandering = new BreedableCreatureWanderingState(movementStateMachine, this);
        idle = new BreedableCreatureIdleState(movementStateMachine, this);
        movementStateMachine.Initialize(idle);
        startPosition = transform.position;
    }

    public void Reset()
    {
        this.Start();
    }

    protected void Update()
    {
        base.Update();
    }

    public void handleBeginWander()
    {
        var rand = Random.insideUnitSphere * wanderZone;
        var targetPos = startPosition + rand;
        ValidatePosition(ref targetPos);

        wanderTarget = targetPos;
        SetMoveSlow();
    }

    public void wander()
    {
        //Make the entity wander.
        var position = transform.position;

        targetPosition = wanderTarget;
        Debug.DrawLine(position, targetPosition, Color.yellow);
        FaceDirection((targetPosition - position).normalized);

        if (navMeshAgent)
        {
            navMeshAgent.destination = targetPosition;
            navMeshAgent.speed = currentSpeed;
            navMeshAgent.angularSpeed = rotationSpeed;
            navMeshAgent.autoBraking = false;
            navMeshAgent.acceleration = 100;
        }
        else
        {
            characterController.SimpleMove(currentSpeed * Vector3.ProjectOnPlane(targetPosition - position, Vector3.up).normalized);
        }
    }

    /**
     * Checked weather the passed targeted position is valid
     * Disabled the component if not
     **/
    void ValidatePosition(ref Vector3 targetPos)
    {
        if (navMeshAgent)
        {
            NavMeshHit hit;
            if (!NavMesh.SamplePosition(targetPos, out hit, Mathf.Infinity, 1 << NavMesh.GetAreaFromName("Walkable")))
            {
                Debug.LogError("Unable to sample nav mesh. Please ensure there's a Nav Mesh layer with the name Walkable");
                enabled = false;
                return;
            }

            targetPos = hit.position;
        }
    }
    void FaceDirection(Vector3 facePosition)
    {
        /*        transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(Vector3.RotateTowards(transform.forward,
                    facePosition, rotationSpeed * Time.deltaTime * Mathf.Deg2Rad, 0f), Vector3.up), Vector3.up);*/
    }

    /**
     * Sets the move speed for wonder state, can probably be added to the onEnter for wanderstate.
     **/
    void SetMoveSlow()
    {
        var minSpeed = float.MaxValue;

        if (currentSpeed < minSpeed)
        {
            minSpeed = currentSpeed;
        }

        currentSpeed = minSpeed;
    }
}
