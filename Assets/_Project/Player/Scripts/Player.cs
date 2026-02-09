using UnityEngine;

public class Player : Character
{
    public HealthBar healthBarPrefab;
    [SerializeField] HealthBar healthBar;
    public void Start()
    {
        hitPoints = maxHitPoints;
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;
    }
    public override void TakeDamage(int damage)
    {
        hitPoints -= damage;
        healthBar.OnHurt(hitPoints, shield);
        var pm = GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.StartCoroutine(pm.HurtRoutine(0.33f));
            pm.StartCoroutine(pm.StunRoutine(0.53f));
        }
        if (hitPoints <= 0) KillCharacter();
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
        hitPoints = maxHitPoints;
    }
}