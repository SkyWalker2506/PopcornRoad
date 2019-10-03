using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Range(1,100)]
    public int LevelIndex;
    public int RoadLength;
    public Transform StartPosition;
}
