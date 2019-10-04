using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popcorn : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player&& player.isPlayerHealthFull)
        {
            PlayerHitMe();
        }
    }

    void PlayerHitMe()
    {
        GetComponent<Animator>().SetTrigger("Pop");
    }

    void DestroyPopcorn()
    {
        Destroy(gameObject);
    }
}
