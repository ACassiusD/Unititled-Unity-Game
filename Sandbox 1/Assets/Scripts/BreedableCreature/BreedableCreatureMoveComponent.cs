
using UnityEngine;
using Polyperfect.Animals;
using UnityEngine.AI;

public class BreedableCreatureMoveComponent : MoveComponent
{
    public BreedableCreatureWanderingState wandering;
    Vector3 velocity = new Vector3(); //gravity force
    public Animal_WanderScript wanderscript;
    public NavMeshAgent naveMeshAgent;

    private void Start()
    {
        isEnabled = true;
        wanderscript = this.GetComponent<Animal_WanderScript>();
        naveMeshAgent = this.GetComponent<NavMeshAgent>();
        wandering = new BreedableCreatureWanderingState(stateMachine, this); 
        stateMachine.Initialize(wandering);
    }

    public void Reset()
    {
        this.Start();
    }

    protected void Update()
    {
        base.Update();
    }
}
