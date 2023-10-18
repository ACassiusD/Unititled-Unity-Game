using UnityEngine;
/// <summary>
/// Interface represents a contract. 
/// A set of public methods any implementing class has to have. 
/// Technically, the interface only governs syntax, 
/// i.e. what methods are there, what arguments they get and what they return
/// </summary>
public interface IDamageable
{
    public float ReceiveDamage(float damageAmount, int knockBackForce, Vector3 direction);
    public void Feint();
    public void UpdateFloatingHealthBarUI();
}

