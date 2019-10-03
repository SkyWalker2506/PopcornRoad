using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerPrefab;
    [HideInInspector]
    public GameObject PlayerObj;
    [Range(1,100)]
    [SerializeField]
    int currentLevelIndex = 1;
    [SerializeField]
    List<Level> levels;

    public static GameManager Instance;

    void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
        currentLevelIndex = PlayerPrefs.GetInt("CurrentLevel", 1);
        PrepareLevel(currentLevelIndex);
    }

    private void OnEnable()
    {
        StartTrigger.PlayerPassed += SetLevel;
    }

    private void OnDisable()
    {
        if(StartTrigger.PlayerPassed!=null)
            StartTrigger.PlayerPassed -= SetLevel;
    }

    Level GetLevel(int index)
    {
        FindAllLevels();
        return levels.Find(l => l.LevelIndex == index);
    } 

    void PrepareLevel(int levelIndex)
    {
      //  
        var level = GetLevel(levelIndex);
        if (!level)
            level = LevelCreator.Instance.CreateLevel(levelIndex).GetComponent<Level>();
        PlayerObj = Instantiate(playerPrefab, level.StartPosition.position + level.StartPosition.forward.normalized*4, level.StartPosition.rotation);
        var player = PlayerObj.GetComponent<Player>();
        player.DoMove = false;
        UIManager.Instance.UpdateLevelText(levelIndex);
        ActivateNeededLevels();
        GetLevel(levelIndex - 1).gameObject.SetActive(false);

    }

    public void StartGame()
    {
        var player = PlayerObj.GetComponent<Player>();
        player.DoMove = true;
    }

    [ContextMenu("Find All Levels")]
    void FindAllLevels()
    {
           levels = FindObjectsOfType<Level>().OrderBy(l => l.LevelIndex).ToList();
    }


    void SetLevel(Level level)
    {
        UpdateCurrentLevel(level.LevelIndex);
        CreateNextLevelsIfNeeded();
        ActivateNeededLevels();
        PositionLevels();
        UIManager.Instance.UpdateLevelText(currentLevelIndex);
    }

    [ContextMenu("Reset Current Level")]
    public void ResetCurrentLevel()
    {
        UpdateCurrentLevel(1);
    }

    void UpdateCurrentLevel(int levelIndex)
    {
        currentLevelIndex = levelIndex;
        PlayerPrefs.SetInt("CurrentLevel", currentLevelIndex);
    }

    void CreateNextLevelsIfNeeded()
    {
        FindAllLevels();
        if (!GetLevel(currentLevelIndex + 1))
            LevelCreator.Instance.CreateLevel(currentLevelIndex + 1);
        if (!GetLevel(currentLevelIndex + 2))
            LevelCreator.Instance.CreateLevel(currentLevelIndex + 2);
        FindAllLevels();
    }


    void PositionLevels()
    {
        var level = levels.Find(l => l.LevelIndex == currentLevelIndex);
        var level1 = levels.Find(l => l.LevelIndex == currentLevelIndex + 1);
        var level2 = levels.Find(l => l.LevelIndex == currentLevelIndex + 2);
        level1.transform.position = level.transform.position + level.transform.forward * (level.RoadLength + level1.RoadLength) * .5f;
        level2.transform.position = level1.transform.position + level1.transform.forward * (level1.RoadLength + level2.RoadLength) * .5f;
    }

    void ActivateNeededLevels()
    {
        levels.ForEach(l => l.gameObject.SetActive(l.LevelIndex >= currentLevelIndex-1 && l.LevelIndex <= currentLevelIndex + 2));
    }

}
