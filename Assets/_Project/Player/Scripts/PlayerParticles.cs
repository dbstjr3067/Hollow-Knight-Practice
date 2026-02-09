using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem Slash_Hit_1;
    [SerializeField] private ParticleSystem Slash_Hit_2;
    public void PlaySlashHit(Vector3 position, int direction = 0)
    {
        int directionAngle = 0;
        if(direction == 0){
            directionAngle = 0; // 오른쪽
        }
        else if(direction == 1){ // 왼쪽
            ParticleManager.Instance.GenerateIn(position, Slash_Hit_1, directionAngle + Random.Range(0, 180));
            ParticleManager.Instance.GenerateIn(position, Slash_Hit_2, directionAngle + Random.Range(-5, 15), true);
            return;
        }
        else if(direction == 2){
            directionAngle = -90; // 위
        }
        else if(direction == 3){
            directionAngle = 90; // 아래
        }
        ParticleManager.Instance.GenerateIn(position, Slash_Hit_1, directionAngle + Random.Range(0, 180));
        ParticleManager.Instance.GenerateIn(position, Slash_Hit_2, directionAngle + Random.Range(-15, 5)); //스프라이트 자체가 5도 휘어진걸 그림;
    }
}
