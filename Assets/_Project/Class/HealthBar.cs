using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour
{
    public int hitPoints;
    public List<Heart> hearts = new List<Heart>();
    public List<GameObject> emptyHearts = new List<GameObject>();
    public Heart heartPrefab;
    public GameObject emptyHeartPrefab;
    [HideInInspector]
    public Player character;
    public int maxHitPoints;
    public int shield;
    public int space;
    public Transform HeartFolder;
    public Transform EmptyHeartFolder;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHitPoints = character.maxHitPoints;
        shield = character.shield;
        hitPoints = character.hitPoints.value;
        for(int i=0; i<maxHitPoints+shield; i++){
            Heart newHeart = Instantiate(heartPrefab, HeartFolder);
            newHeart.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * space, 0);
            hearts.Add(newHeart);
            if(i<maxHitPoints){
                if(i<hitPoints){
                    newHeart.heartType = Heart.HeartType.Filled;
                } else {
                    newHeart.heartType = Heart.HeartType.Empty;
                }
            } else {
                newHeart.heartType = Heart.HeartType.Shield;
            }
        }
        for(int i=0; i<maxHitPoints; i++){
            GameObject newEmptyHeart = Instantiate(emptyHeartPrefab, EmptyHeartFolder);
            newEmptyHeart.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * space, 0);
            emptyHearts.Add(newEmptyHeart);
        }
    }
    public void OnHurt(HitPoints curHitPoints, int curShield){ //5+3 -> 5+2
        if(curHitPoints.value<0) curHitPoints.value = 0;
        if(curShield<0) curShield = 0;
        for(int i=hitPoints; i>curHitPoints.value; i--){
            hearts[i-1].heartType = Heart.HeartType.Empty;
            hearts[i-1].gameObject.GetComponent<Image>().color = new Color(1,1,1,0f);
        }
        for(int i=shield; i>curShield; i--){
            hearts[i-1].heartType = Heart.HeartType.Empty;
            hearts[i-1].gameObject.GetComponent<Image>().color = new Color(1,1,1,0f);
        }
        hitPoints = curHitPoints.value;
        shield = curShield;
    }
    public void OnHeal(HitPoints curHitPoints, int curShield){ //4 -> 5
        for(int i=hitPoints; i<curHitPoints.value; i++){
            hearts[i-1].heartType = Heart.HeartType.Filled;
            hearts[i-1].gameObject.GetComponent<Image>().color = new Color(1,1,1,1f);
        }
        for(int i=shield; i<curShield; i++){
            hearts[i-1].heartType = Heart.HeartType.Filled;
            hearts[i-1].gameObject.GetComponent<Image>().color = new Color(1,1,1,1f);
        }
        hitPoints = curHitPoints.value;
        shield = curShield;
    }
}