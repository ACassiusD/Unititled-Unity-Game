using UnityEngine;

public class EnemyCombatComponent : CombatComponent
{

    //An event to let the movement controller know that it should be put in stun.
    public event System.Action<int?, Vector3?> OnAttacked = delegate { };

    //Enemys should get knocked back.
    public override float ReceiveDamage(float damageAmount, int? knockBackForce, Vector3? direction = null)
    {
        float newCurrentHealthValue = base.ReceiveDamage(damageAmount, knockBackForce, direction);

        //Raise onAttacked Event
        OnAttacked(knockBackForce, direction);

        return newCurrentHealthValue;
    }

    public override void Feint()
    {
        Destroy(gameObject);
    }
}
