using UnityEngine;
using System.Collections;

public class Enemy : Character
{
    public int hitPoints;
    public int damageStrength;
    public override IEnumerator DamageCharacter(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0) KillCharacter();
        yield return null;
    }
    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
    }
    private void OnEnable()
    {
        ResetCharacter();
    }
    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Player")){
            Player player = collision.gameObject.GetComponent<Player>();
            StartCoroutine( player.DamageCharacter(damageStrength) );
        }
    }
}
