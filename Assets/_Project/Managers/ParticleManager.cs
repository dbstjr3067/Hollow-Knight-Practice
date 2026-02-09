using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public ParticleSystem GenerateIn(Vector3 position, ParticleSystem particlePrefab, float rotation = 0f, bool flipX = false)
    {
        if (particlePrefab == null)
        {
            Debug.LogError("파티클 생성 실패: 파티클 프리팹이 null입니다.");
            return null;
        }
        ParticleSystem particle = Instantiate(particlePrefab, position, Quaternion.Euler(0f, 0f, 0f));
        if(flipX)
            particle.transform.localScale = new Vector3(-1,1,1);
        particle.startRotation = rotation * Mathf.Deg2Rad;
        particle.Play();

        return particle;
    }

    public ParticleSystem GenerateToTarget(Transform targetObject, ParticleSystem particlePrefab, Vector3? localPosition = null, Quaternion? localRotation = null, bool autoDestroy = true)
    {
        if (targetObject == null)
        {
            Debug.LogError("파티클 생성 실패: 타겟 오브젝트가 null입니다.");
            return null;
        }

        if (particlePrefab == null)
        {
            Debug.LogError("파티클 생성 실패: 파티클 프리팹이 null입니다.");
            return null;
        }

        ParticleSystem particle = Instantiate(particlePrefab, targetObject);
        
        particle.transform.localPosition = localPosition ?? Vector3.zero;
        particle.transform.localRotation = localRotation ?? Quaternion.identity;
        
        particle.Play();

        if (autoDestroy)
        {
            float duration = particle.main.duration + particle.main.startLifetime.constantMax;
            Destroy(particle.gameObject, duration);
        }

        return particle;
    }
}