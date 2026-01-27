using UnityEngine;
using System.Collections;

public class Player : Character
{
    public HitPoints hitPoints;
    public HealthBar healthBarPrefab;
    [SerializeField] HealthBar healthBar;
    public void Start()
    {
        hitPoints.value = startingHitPoints;
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;
    }
    public override IEnumerator DamageCharacter(int damage)
    {
        hitPoints.value -= damage;
        healthBar.OnHurt(hitPoints, shield);
        // Play Hurt animation and grant 1 second of invulnerability (ignore enemy collisions)
        var pm = GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.StartCoroutine(pm.HurtRoutine(0.33f));
            pm.StartCoroutine(pm.StunRoutine(0.53f));
        }
        if (hitPoints.value <= 0) KillCharacter();
        yield return null;
    }
    public override void KillCharacter()
    {
        base.KillCharacter();
        Destroy(healthBar.gameObject);
    }
    public override void ResetCharacter()
    {
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;
        hitPoints.value = startingHitPoints;
    }
    /*private void OnEnable()
    {
        ResetCharacter();
    }*/
}