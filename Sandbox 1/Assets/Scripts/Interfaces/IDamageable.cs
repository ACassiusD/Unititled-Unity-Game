using System.Collections.Generic;
using UnityEngine;
//interface represents a contract. A set of public methods any implementing class has to have. Technically, the interface only governs syntax, i.e. what methods are there, what arguments they get and what they return
public interface IDamageable
{   
    public float receiveDamage(float damageAmount, int knockBackForce, Vector3 direction);
    public void feint();
    public void updateHealthBar();
}

