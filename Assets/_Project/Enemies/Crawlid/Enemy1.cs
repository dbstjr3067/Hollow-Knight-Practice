using UnityEngine;
using System.Collections;

public class Enemy1 : Enemy, IKnockbackable
{
    [SerializeField] private Rigidbody2D rb;
    private float knockbackTimer = 0;
    private DamageFlash _damageFlash;
    public override void TakeDamage(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0) KillCharacter();
        else{
            BugParticle bugParticle = GetComponent<BugParticle>();
            if (bugParticle != null) bugParticle.PlayBugHit(transform.position);
        }
        _damageFlash.CallDamageFlash();
    }
    void Start()
    {
        _damageFlash = GetComponent<DamageFlash>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void ApplyKnockback(float duration, float force, int direction)
    {
        StartCoroutine(KnockbackRoutine(duration, force, direction));
    }
    public IEnumerator KnockbackRoutine(float duration, float force, int direction)
    {
        knockbackTimer = 0;
        while (knockbackTimer < duration)
        {
            if(direction == 0){
                rb.linearVelocity = new Vector2(1 * force, rb.linearVelocity.y);
            }
            else if(direction == 1){
                rb.linearVelocity = new Vector2(-1 * force, rb.linearVelocity.y);
            }
            else if(direction == 2){
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0.7f * force);
            }
            else if(direction == 3){
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -0.7f * force);
            }
            knockbackTimer += 0.03f;
            yield return new WaitForSeconds(0.03f);
        }
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }
}
