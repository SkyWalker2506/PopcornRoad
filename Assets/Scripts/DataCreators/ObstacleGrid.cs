using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Grid", order = 1)]
public class ObstacleGrid : ScriptableObject
{
    public List<Column> Grid = new List<Column>();

    public ObstacleGrid(int row, int column)
    {
        for (int i = 0; i < row; i++)
        {
            Grid.Add (new Column(column));
        }
    }

    public void AddEmptyRow()
    {
        for (int i = 0; i < Grid.Count; i++)
        {
            Grid[i].column.Add(0);
        }                
    }

    public void AddSameSizeRowGrid(List<Column> extraGrid)
    {
        for (int i = 0; i < Grid.Count; i++)
        {
                Grid[i].column.AddRange(extraGrid[i].column);
        }
    }
}

[Serializable]
public class Column
{
    public List<int> column = new List<int>();

    public Column(int length)
    {
        for (int i = 0; i < length; i++)
        {
            column.Add(0);
        }
    }
}
