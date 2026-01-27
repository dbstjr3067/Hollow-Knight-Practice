using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    const float speed = 0.57f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce = 5f;

    // Ground check
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.12f;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded = false;
    private Animator animator;
    [SerializeField] private float attackCooldownTime = 0.41f;
    [SerializeField] private float currentAttackCooldown = 0;
    [SerializeField] private string currentAnimation = "";
    [Header("Damage / Invulnerability")]
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private float hitStopDuration = 0.33f;
    private bool isInvulnerable = false;
    private bool isStunned = false;
    [Header("Hitbox")]
    [SerializeField] private Collider2D hitboxHorizontal;
    [SerializeField] private Collider2D hitboxUp;
    [SerializeField] private Collider2D hitboxDown;
    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 0.09f;
    [SerializeField] private float knockbackForce = 0.9f;
    private float knockbackTimer = 0f;
    private float knockbackVelocityX = 0f;
    // attack sequence: alternate between Slash_1 and Slash_2
    private int attackSequenceIndex = 0; // 0 -> Slash_1, 1 -> Slash_2
    private float attackSequenceTimer = 0f;
    private const float attackSequenceResetTime = 2f; // reset to Slash_1 after this many seconds of no attack

    void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Apply a short horizontal knockback to the player. This sets a temporary velocity
    // and a timer during which input-driven horizontal movement is ignored.
    private void ApplyKnockback()
    {
        // direction: transform.localScale.x is 1 or -1 depending on facing
        if (currentAnimation == "Slash_Down"){
            // vector little bit upwards
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 7f);
            StartCoroutine( DownSlashGravity() );
        }
        else if(currentAnimation == "Slash_Up"){
            if(rb.linearVelocity.y > 0) rb.linearVelocity = new Vector2(rb.linearVelocity.x, -2.3f);
            else rb.AddForce(Vector2.down * 2.3f, ForceMode2D.Impulse);
        }
        else{
            int dir = (int)Mathf.Sign(transform.localScale.x);
            knockbackVelocityX = dir * -1f * knockbackForce;
            knockbackTimer = knockbackDuration;
            if (rb != null)
                rb.linearVelocity = new Vector2(knockbackVelocityX, rb.linearVelocity.y);
        }
    }

    // Play Hurt animation and grant temporary invulnerability (ignores collisions with enemies by tag and/or layer)
    public IEnumerator HurtRoutine(float duration)
    {
        if (isInvulnerable) yield break;
        isInvulnerable = true;
        DisableHitbox();
        // Force Hurt animation and prevent CheckAnimation from cancelling it
        ChangeAnimation("Hurt");
        int dir = (int)transform.localScale.x;
        rb.linearVelocity = new Vector2(dir * -1, 1.73f) * 4;
        // Hit stop: pause global time briefly so the hurt hitstop is visible.
        if (hitStopDuration > 0f)
        {
            float prevTimeScale = Time.timeScale;
            float prevFixedDelta = Time.fixedDeltaTime;
            Time.timeScale = 0f;
            Time.fixedDeltaTime = 0f;
            yield return new WaitForSecondsRealtime(hitStopDuration);
            Time.timeScale = prevTimeScale;
            Time.fixedDeltaTime = prevFixedDelta;
        }
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
        yield return new WaitForSeconds(duration);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
        isInvulnerable = false;
        CheckAnimation();
    }

    // Stun the player for `duration` seconds. While stunned the player cannot move or jump.
    // This coroutine uses real-time waiting so it's unaffected by global timeScale hit-stops.
    public IEnumerator StunRoutine(float duration)
    {
        if (isStunned) yield break;
        isStunned = true;
        yield return new WaitForSecondsRealtime(duration);

        isStunned = false;
        CheckAnimation();
    }

    public IEnumerator DownSlashGravity()
    {
        yield return new WaitForSeconds(0.27f);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -2.3f);
    }
    // Enable or disable the hitbox collider (child polygon collider used for attack hits)
    public void SetHitboxEnabled(string direction)
    {
        if (direction == "Horizontal" && hitboxHorizontal != null)
            hitboxHorizontal.enabled = true;
        else if (direction == "Up" && hitboxUp != null)
            hitboxUp.enabled = true;
        else if (direction == "Down" && hitboxDown != null)
            hitboxDown.enabled = true;
    }

    // Toggle convenience
    public void DisableHitbox()
    {
        hitboxHorizontal.enabled = false;
        hitboxUp.enabled = false;
        hitboxDown.enabled = false;
    }

    // Called when any trigger on the player's Rigidbody2D (including child triggers) enters another collider.
    // We filter to only process hits coming from the configured hitboxHorizontal and then try to deliver damage.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (hitboxHorizontal == null) return;
        if (other == null) return;
        if (other.gameObject.tag == "Enemy"){
            other.gameObject.SendMessage("DamageCharacter", 1, SendMessageOptions.DontRequireReceiver);
            // apply a short knockback to the player when an attack successfully hits an enemy
            ApplyKnockback();
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentAttackCooldown -= Time.deltaTime;
        if (currentAttackCooldown < 0) currentAttackCooldown = 0;
        // update attack sequence timer (reset sequence after inactivity)
        attackSequenceTimer += Time.deltaTime;
        if (attackSequenceTimer >= attackSequenceResetTime)
        {
            attackSequenceIndex = 0; // next attack should start from Slash_1
            // clamp timer so it doesn't grow unbounded
            attackSequenceTimer = attackSequenceResetTime;
        }

        // update grounded state early so attack direction can be evaluated
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
        }

        // If stunned, prevent movement and attacking. Keep cooldown/timers updating.
        if (isStunned)
        {
            // keep in stun animation until stun ends
            CheckAnimation();
            return;
        }
        if(Input.GetKeyDown("d")) //데미지 테스트
        {
            StartCoroutine( gameObject.GetComponent<Player>().DamageCharacter(1) );
        }
        if(Input.GetKeyDown("x") && currentAttackCooldown == 0)
        {
            // decide attack animation based on vertical input and grounded state
            float v = Input.GetAxisRaw("Vertical");
            string slashAnim;
            if (v == 1f)
            {
                slashAnim = "Slash_Up";
            }
            else if (v == -1f && !isGrounded)
            {
                slashAnim = "Slash_Down";
            }
            else
            {
                // normal alternating slashes
                slashAnim = attackSequenceIndex == 0 ? "Slash_1" : "Slash_2";
            }

            ChangeAnimation(slashAnim);
            currentAttackCooldown = attackCooldownTime;

            // advance sequence only when using normal slashes
            if (slashAnim == "Slash_1" || slashAnim == "Slash_2")
                attackSequenceIndex = 1 - attackSequenceIndex;

            // reset sequence timer on any attack
            attackSequenceTimer = 0f;
        }

        // Jump = Z key; only when grounded
        if (Input.GetKeyDown("z") && isGrounded)
        {
            if(currentAnimation != "Slash_1" && currentAnimation != "Slash_2")
                ChangeAnimation("Jump_Ascend");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Variable jump height: if player releases jump before reaching apex,
        // invert upward velocity to force a quicker fall as requested.
        if (Input.GetKeyUp("z"))
        {
            if (rb != null && rb.linearVelocity.y > 0f)
            {
                CheckAnimation();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * -0.3f);
            }
        }

        // horizontal movement (use velocity)
        float h = Input.GetAxisRaw("Horizontal");
        if (knockbackTimer > 0f)
        {
            // during knockback, honor the knockback velocity and don't let player input overwrite it
            knockbackTimer -= Time.deltaTime;
            if (rb != null)
                rb.linearVelocity = new Vector2(knockbackVelocityX, rb.linearVelocity.y);
        }
        else
        {
            if (rb != null)
                rb.linearVelocity = new Vector2(h * speed * 6f, rb.linearVelocity.y);

            if (h == 1)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (h == -1)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        if (rb.linearVelocity.y < -9.8f) rb.linearVelocity = new Vector2(rb.linearVelocity.x, -9.8f);
        CheckAnimation();
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private void CheckAnimation()
    {
        if(currentAnimation == "Slash_Down" && !isGrounded) return;
        if (currentAnimation == "Hurt") return;
        if( currentAnimation == "Slash_1" || currentAnimation == "Slash_2" || currentAnimation == "Slash_Up" || currentAnimation == "Jump_Ascend" ) return;
        if (isGrounded)
        {
            if (rb.linearVelocity.x != 0)
            {
                ChangeAnimation("Run");
            }
            else
            {
                ChangeAnimation("Idle");
            }
        }
        else
        {
            ChangeAnimation("Jump_Descend");
        }
    }
    public void ChangeAnimation(string animation, float crossfade = 0, float time = 0)
    {
        string temp = currentAnimation;
        if (time > 0) StartCoroutine(Wait());
        else Validate();

        IEnumerator Wait()
        {
            
            yield return new WaitForSeconds(time - crossfade);
            Validate();
        }

        void Validate()
        {
            if (temp != currentAnimation) return;
            if (currentAnimation != animation){
                currentAnimation = animation;
                if (currentAnimation == "")
                    CheckAnimation();
                else
                animator.CrossFade(currentAnimation, crossfade);
            }
        }
    }
}
