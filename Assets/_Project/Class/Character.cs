using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour
{
    public int maxHitPoints;
    public int startingHitPoints;
    public int shield;
    public virtual void KillCharacter()
    {
        Destroy(gameObject);
    }
    public abstract void ResetCharacter();
    public abstract IEnumerator DamageCharacter(int damage);
}
