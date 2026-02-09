using UnityEngine;
using System.Collections;

public class MenuSet : MonoBehaviour
{
    [SerializeField] private float duration;
    public GameObject InitialMenu;

    private CanvasGroup canvasGroup;
    private Coroutine currentCoroutine;

    private void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    public void MenuEnable()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(FadeIn(0.5f));
        InitialMenu.GetComponent<Menu>().StartCoroutine("SelectAnimation");
    }

    public void MenuDisable()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(FadeOut());
    }

    private IEnumerator Fade(float from, float to, float delay, bool disableAtEnd = false)
    {
        float elapsed = 0f;

        canvasGroup.alpha = from;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        yield return new WaitForSeconds(delay);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = to;

        bool visible = to > 0.9f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
        currentCoroutine = null;
    }
    private IEnumerator FadeIn(float delay = 0f)
    {
        float elapsed = 0f;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        yield return new WaitForSeconds(delay);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        currentCoroutine = null;
    }
    private IEnumerator FadeOut(float delay = 0f)
    {
        float elapsed = 0f;

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        yield return new WaitForSeconds(delay);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        currentCoroutine = null;
    }
}
