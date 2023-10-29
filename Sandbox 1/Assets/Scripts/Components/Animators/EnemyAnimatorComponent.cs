//This animator is overriding the mount animatior because they are both using polyperfect models.
public class EnemyAnimatorComponent : MountAnimatorComponent
{
    //Use the default idle animation as the attack animation instead.
    public void setAOEAttackAnimation()
    {
        setIdleAnimation();
    }
}
