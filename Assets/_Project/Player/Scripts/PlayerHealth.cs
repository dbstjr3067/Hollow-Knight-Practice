using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float invulnerabilityTime = 1f;

    private float health;
    private float invulnerableUntil = 0f;
    private Animator animator;
    private PlayerMovement movement;

    void Awake()
    {
        health = maxHealth;
        animator = GetComponentInChildren<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    public void TakeDamage(float amount, GameObject source)
    {
        if (Time.time < invulnerableUntil) return;
        invulnerableUntil = Time.time + invulnerabilityTime;

        health -= amount;
        Debug.Log($"Player took {amount} damage from {source.name}. Remaining HP: {health}");

        if (animator != null)
            animator.SetTrigger("Hurt");

        if (health <= 0f)
            Die();
    }

    private void Die()
    {
        Debug.Log("Player died");
        if (movement != null) movement.enabled = false;
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;
        // could add respawn or game over logic here
    }

    // Optional: expose current HP
    public float GetHealth() => health;
}
