using UnityEngine;

public interface IDamageable
{
    // amount: damage amount; source: who caused the damage
    void TakeDamage(float amount, GameObject source);
}
