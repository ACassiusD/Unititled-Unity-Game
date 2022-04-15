using System.Collections.Generic;
//interface represents a contract. A set of public methods any implementing class has to have. Technically, the interface only governs syntax, i.e. what methods are there, what arguments they get and what they return
public interface IDamageable
{   
    public int receiveDamage(Dictionary<string, int> damageValues);
    public void feint();
    public void updateHealthBar();
}

