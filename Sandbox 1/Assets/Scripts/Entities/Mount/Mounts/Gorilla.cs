public class Gorilla : Mount
{
    public float gorillaWalkSpeed = 4;
    public float gorillaRunSpeed = 15;

    // Start is called before the first frame update
    void Start()
    {
        moveComponent.walkSpeed = 4;
        moveComponent.runSpeed = 15;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        moveComponent.runSpeed = gorillaRunSpeed;
        moveComponent.walkSpeed = gorillaWalkSpeed;
    }
}
