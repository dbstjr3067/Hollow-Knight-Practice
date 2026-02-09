using UnityEngine;

public class BugParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem Bug_Hit;
    [SerializeField] private ParticleSystem Bug_Splash;
    [SerializeField] private ParticleSystem Bug_Particle;
    public void PlayBugHit(Vector3 position)
    {
        ParticleManager.Instance.GenerateIn(position, Bug_Hit);
        ParticleManager.Instance.GenerateIn(position, Bug_Splash);
        ParticleManager.Instance.GenerateIn(position, Bug_Particle);
    }
}