using System.Collections.Generic;
//interface represents a contract. A set of public methods any implementing class has to have. Technically, the interface only governs syntax, i.e. what methods are there, what arguments they get and what they return
public interface IDamageable
{   
    int currentHealth { get; set; }
    int maxHealth { get; set; }
    bool inHitStun { get; set; }

    public int receiveDamage(Dictionary<string, int> damageValues);
    public void Knockback(float force);
    public void feint();
    public void updateHealthBar();
}

