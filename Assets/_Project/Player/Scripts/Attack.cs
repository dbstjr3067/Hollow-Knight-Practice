using UnityEngine;

public class Attack : MonoBehaviour
{
    public void HitBoxStart(string direction){
        gameObject.GetComponentInParent<PlayerMovement>().SetHitboxEnabled(direction);
    }

    public void HitBoxEnd(){
        gameObject.GetComponentInParent<PlayerMovement>().DisableHitbox();
    }
}
