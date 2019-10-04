using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    [SerializeField]
    [Range(0,100)]
    float iceDamage = 5;
    public static Action<float> HitToPlayer;


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>())
            PlayerHitMe();
    }

    void PlayerHitMe()
    {
        HitToPlayer(-iceDamage);
        GetComponent<Animator>().SetTrigger("DestroyIce");
    }

    void DestroyIce()
    {
        Destroy(gameObject);
    }
}
