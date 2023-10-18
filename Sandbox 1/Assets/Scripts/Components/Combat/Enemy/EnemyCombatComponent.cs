public class EnemyCombatComponent : CombatComponent
{
    public override void Feint()
    {
        Destroy(gameObject);
    }
}
