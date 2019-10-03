using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [Header("General Settings")]
    [Range(50, 500)]
    public int RoadLength = 100;
    [Range(10, 25)]
    public int RoadWidth = 10;
    [Range(5, 200)]
    public int FirstObstacleStartPosition = 10;
    [Range(1,10)]
    public int LevelDificulty = 1;
    [Range(.01f, .1f)]
    public float ObstacleChanceConstant = .05f;
    [Range(.1f, .9f)]
    public float PopcornChance = .25f;
    public GameObject RoadBlock;
    public GameObject SideBlock;
    public GameObject StartTrigger;
    public GameObject PopCorn;
    public GameObject Ice;
    public List<ObstacleGrid> ObstaclePatterns;
    [Header("Multiple Level Settings")]
    [Range(1, 100)]
    public int FirstLevel = 1;
    [Range(1, 100)]
    public int TotalLevels = 10;

    public static LevelCreator Instance;

    void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }

    [ContextMenu("Create Road")]
    public Transform CreateRoad()
    {
        var road = new GameObject("Road").transform;
        var roadBlock = Instantiate(RoadBlock, road);
        var sideBlockLeft = Instantiate(SideBlock, road);
        var sideBlockRight = Instantiate(SideBlock, road);
        var startTrigger = Instantiate(StartTrigger, road);
        roadBlock.transform.localScale = new Vector3(RoadWidth, 1, RoadLength);
        sideBlockLeft.transform.localScale = new Vector3(1, 1, RoadLength);
        sideBlockRight.transform.localScale = new Vector3(1, 1, RoadLength);
        startTrigger.transform.localScale = new Vector3(RoadWidth, 1, 1);
        sideBlockLeft.transform.position = new Vector3((RoadWidth+ SideBlock.transform.lossyScale.x)*-.5f, 0.5f, 0);
        sideBlockLeft.transform.rotation = Quaternion.Euler(0, 180, 0);
        sideBlockRight.transform.position = new Vector3((RoadWidth+ SideBlock.transform.lossyScale.x)*.5f, 0.5f, 0);
        startTrigger.transform.position = new Vector3(0, 1, RoadLength * -.5f);
        return road;
    }

    ObstacleGrid CreateProperObstaclePattern(ObstacleGrid obstacle)
    {
        var obs = obstacle;
        if (RoadWidth > obs.Grid.Count)
        {
            var emptyColumn = new Column(obs.Grid[0].column.Count);
            var emptyColumnCount = RoadWidth - obs.Grid.Count;
            for (int i = 0; i < emptyColumnCount * .5f; i++)
            {
                obs.Grid.Insert(0, emptyColumn);
            }

            while (RoadWidth > obs.Grid.Count)
            {
                obs.Grid.Insert(obs.Grid.Count, emptyColumn);
            }
        }

        else if (RoadWidth < obs.Grid.Count)
        {
            for (int i = 0; i < (obs.Grid[0].column.Count - RoadWidth) * .5f; i++)
            {
                obstacle.Grid.RemoveAt(0);
            }

            while (RoadWidth < obs.Grid.Count)
            {
                obs.Grid.RemoveAt(obs.Grid.Count - 1);
            }
        }
        return obs;
    }

    ObstacleGrid MapAllRoad(int difficulty)
    {
        var Map = new ObstacleGrid(RoadWidth, FirstObstacleStartPosition);
        var obstacle = GetRandomObstacle();
        Map.AddSameSizeRowGrid(CreateProperObstaclePattern(obstacle).Grid);
        var currentColumn = FirstObstacleStartPosition + obstacle.Grid[0].column.Count + 1;
        while (currentColumn < RoadLength - 5)
        {
            var ObstacleChance = difficulty * ObstacleChanceConstant;
            var putObstacle = Random.Range(0, 1f) < ObstacleChance;
            if (putObstacle)
            {
                obstacle = GetRandomObstacle();
                Map.AddSameSizeRowGrid(CreateProperObstaclePattern(obstacle).Grid);
                currentColumn +=  obstacle.Grid[0].column.Count;
            }
                Map.AddEmptyRow();
                currentColumn++;
        }

        while(Map.Grid[0].column.Count<RoadLength)
        {
            Map.AddEmptyRow();
        }

        return Map;
    } 

    public void CreateLevelObjects(Transform road, int difficulty)
    {
        var roadStartCorner = new Vector3(1+RoadWidth * -.5f, 0, 1+RoadLength * -.5f);
        var map = MapAllRoad(difficulty);
        for (int j = 5; j < RoadLength-2 ; j++)
        {
            for (int i = 0; i < RoadWidth-1; i++)
            {
                GameObject gameObj = null;
                if (map.Grid[i].column[j] > 0)
                {
                    gameObj = Instantiate(Ice, road);
                }
                else if(Random.Range(0, 1f) < PopcornChance)
                {
                    gameObj = Instantiate(PopCorn, road);
                }
                if(gameObj!=null)
                gameObj.transform.localPosition = roadStartCorner + new Vector3(i, 1, j);
            }
        }
    }

    ObstacleGrid GetRandomObstacle()
    {
        return Instantiate(ObstaclePatterns[Random.Range(0, ObstaclePatterns.Count)]);
    }

    [ContextMenu("Create Level")]
    public void CreateLevel()
    {
        var levelObj = new GameObject
        {
            name = "Level "
        };

        var road = CreateRoad();
        road.name = RoadWidth + "x" + RoadLength + " LD " + LevelDificulty;
        CreateLevelObjects(road,LevelDificulty);
        road.parent = levelObj.transform;
        var level = levelObj.AddComponent<Level>();
        level.LevelIndex = 1;
        var startTrigger = road.GetComponentInChildren<StartTrigger>();
        level.StartPosition = startTrigger.transform;
        level.RoadLength = RoadLength;
        startTrigger.BelongedLevel = level;
    }

    public Transform CreateLevel(int Level)
    {
        var difficulty = Mathf.Min(Level, 10);
        var levelObj = new GameObject
        {
            name = "Level " + Level
        };
  
        var road = CreateRoad();
        road.name= RoadWidth + "x" + RoadLength + " LD " + difficulty;
        CreateLevelObjects(road, difficulty);
        road.parent = levelObj.transform;
        var level = levelObj.AddComponent<Level>();
        level.LevelIndex = Level;
        var startTrigger = road.GetComponentInChildren<StartTrigger>();
        level.StartPosition = startTrigger.transform;
        level.RoadLength = RoadLength;
        startTrigger.BelongedLevel = level;
        return levelObj.transform;
    }

    [ContextMenu("Create Multiple Level")]
    public void CreateLevels()
    {
        for (int i = 0; i < TotalLevels; i++)
        {
          var level = CreateLevel(i + FirstLevel);
          level.position += level.forward* RoadLength * i;
        }
    } 

}