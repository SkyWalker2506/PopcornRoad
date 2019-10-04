using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text levelText;
   

    public static UIManager Instance;

    void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void UpdateLevelText(int level)
    {
        levelText.text = level.ToString();
    }

}
