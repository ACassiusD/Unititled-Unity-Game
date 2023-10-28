using UnityEngine;

public class EnemyCombatComponent : CombatComponent
{
    //Enemys should get knocked back.
    //public override float ReceiveDamage(float damageAmount, int? knockBackForce, Vector3? direction = null)
    //{
    //    float newCurrentHealthValue = base.ReceiveDamage(damageAmount, knockBackForce, direction);

    //    return newCurrentHealthValue;
    //}

    public override void Feint()
    {
        Destroy(gameObject);
    }
}
