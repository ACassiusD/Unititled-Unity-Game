using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    StatsComponent statComponent;
    CombatComponent combatComponent;

    public void Awake()
    {
        statComponent = GetComponent<StatsComponent>();
        combatComponent = GetComponent<CombatComponent>();
        combatComponent.Initialize(statComponent);
    }
}
