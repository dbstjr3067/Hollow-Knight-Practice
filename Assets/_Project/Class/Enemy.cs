using UnityEngine;
using System.Collections;

public class Enemy : Character
{
    public int damageStrength;
    public override void TakeDamage(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0) KillCharacter();
    }
    public override void ResetCharacter()
    {
        hitPoints = maxHitPoints;
    }
    private void OnEnable()
    {
        ResetCharacter();
    }
    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.CompareTag("Player")){
            IDamageable dmg = collider.GetComponent<IDamageable>();
            if (dmg != null) dmg.TakeDamage(damageStrength);
        }
    }
}
