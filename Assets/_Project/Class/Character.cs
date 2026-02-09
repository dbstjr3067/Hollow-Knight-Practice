using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour, IDamageable
{
    public int maxHitPoints;
    public int hitPoints;
    public int shield;
    public virtual void KillCharacter()
    {
        Destroy(gameObject);
    }
    public abstract void ResetCharacter();
    public abstract void TakeDamage(int damage);
}
