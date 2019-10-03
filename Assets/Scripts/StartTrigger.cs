using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    public Level BelongedLevel;
    public static Action<Level> PlayerPassed;

    private void OnTriggerExit(Collider other)
    {
        print(BelongedLevel.LevelIndex + " Passed");

        if (other.tag == "Player")
        {
            print(BelongedLevel.LevelIndex);
            PlayerPassed(BelongedLevel);
        }
    }
}