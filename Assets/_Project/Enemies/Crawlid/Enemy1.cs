using UnityEngine;
using System.Collections;

public class Enemy1 : Enemy
{
    const int FLASH_FRAMES = 9; // 30fps 기준
    Material mat;
    Coroutine flashCo;
    IEnumerator FlashRoutine()
    {
        mat.SetFloat("_Flash", 1f);

        for (int i = 0; i < FLASH_FRAMES; i++)
        {
            float t = (float)i / FLASH_FRAMES;
            float value = Mathf.Lerp(1f, 0f, t);
            mat.SetFloat("_Flash", value);

            yield return new WaitForSeconds(0.03f);
        }

        mat.SetFloat("_Flash", 0f);
        flashCo = null;
    }
    public override IEnumerator DamageCharacter(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0) KillCharacter();

        if (flashCo != null)
        {
            StopCoroutine(flashCo);
        }
        flashCo = StartCoroutine(FlashRoutine());

        yield return null;
    }
    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }
}
