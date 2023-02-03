using Polyperfect.Animals;
using Polyperfect.Common;

public class BreedableCreatureMoveComponent : MoveComponent
{
    public BreedableCreatureWanderingState wandering;
    //public New_WanderScript wanderscript;
    public Animal_WanderScript wanderscript;

    private void Start()
    {
        isEnabled = true;
        //wanderscript = this.GetComponent<New_WanderScript>();
        wanderscript = this.GetComponent<Animal_WanderScript>();
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
