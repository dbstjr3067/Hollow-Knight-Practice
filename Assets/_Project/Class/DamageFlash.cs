using UnityEngine;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float flashTime = 0.16f;

    private SpriteRenderer spriteRenderer;
    private Material _material;
    private Coroutine _damageFlashCoroutine;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _material = spriteRenderer.material;
    }
    public void CallDamageFlash()
    {
        _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }
    private IEnumerator DamageFlasher()
    {
        SetFlashColor();
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while(elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / flashTime));
            SetFlashAmount(currentFlashAmount);
            
            yield return null;
        }
    }
    private void SetFlashColor()
    {
        _material.SetColor("_FlashColor", _flashColor);
    }
    private void SetFlashAmount(float amount)
    {
        _material.SetFloat("_FlashAmount", amount);
    }
}
