using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    public const int MAX_ROWS = 23;
    public const int MAX_COLUMNS = 10;
    public const int NORMAL_ROWS = 20;

    public bool isDataUpdate = false;

    private Transform[,] map = new Transform[MAX_COLUMNS, MAX_ROWS];

    private Dictionary<int, int> rowInfos = new Dictionary<int, int>();

    private int score = 0;
    private int highScore = 0;
    private int numberGame = 0;

    

    private int currentRow = 0;

    private void Awake()
    {
        LoadData();
    }

    public int Score {
        get { return score; }
    }

    public int HighScore
    {
        get { return highScore; }
    }

    public int NumberScore
    {
        get { return numberGame; }
    }

    /// <summary>
    /// 检测是否在有效位置
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public bool IsVaildMapPosition(Transform t)
    {
        foreach (Transform child in t)
        {
            if (child.tag != "Block") continue;
            Vector2 pos = child.position.Round();
            
            //边界范围内
            if (!IsInsideMap(pos)) return false;

            //该位置已经有Block
            if (map[(int) pos.x, (int) pos.y] != null) return false;
        }

        return true;
    }

    /// <summary>
    /// 是否在地图内
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool IsInsideMap(Vector2 pos)
    {
        return pos.x >= 0 && pos.x <= MAX_COLUMNS - 1 && pos.y >= 0; 
    }

    /// <summary>
    /// 放置Block
    /// </summary>
    /// <param name="t"></param>
    public bool PlaceShape(Transform t)
    {
        int count = 0;
        foreach (Transform child in t)
        {
            if (child.tag != "Block") continue;
            //worldPosition
            Vector2 pos = child.position.Round();
               //列          行
            map[(int) pos.x, (int) pos.y] = child;

            RecordCurretRowNumber((int)pos.y);

            ////检查每行是否已满
            //if (rowInfos.ContainsKey((int) pos.y))
            //{
            //    //Debug.Log("pos.y:" + (int)pos.y);
            //    rowInfos[(int) pos.y] += 1;
            //    //Debug.Log("rowInfos[(int) pos.y]:" + rowInfos[(int)pos.y]);
            //    //该行已满
            //    if (rowInfos[(int)pos.y] >= 10)
            //    {
            //        Debug.LogError(" full (int)pos.y:" + (int)pos.y);
            //        count++;
            //        DeleteRow((int)pos.y);
            //        MoveDownRowsAbove((int)pos.y + 1);

            //        Debug.Log("currentRow:" + currentRow);
            //        Debug.Log("(int)pos.y:" + (int)pos.y);
            //        for (int index = (int)pos.y; index < currentRow + 1; index++)
            //        {
            //            Debug.LogError("pre  rindex:" + index);
            //            Debug.LogError("pre  rowInfos[index]:" + rowInfos[index]);
            //            rowInfos[index] = rowInfos[index + 1];
            //            Debug.LogError("rindex:" + index);
            //            Debug.LogError("rowInfos[index]:" + rowInfos[index]);
            //        }
            //    }
            //}
            //else
            //{
            //    rowInfos[(int)pos.y] = 1;
            //}
            //CheckMapByRow((int)pos.y);
        }

        //if (count > 0)
        //{
        //    score += (count * 100);
        //    if (score > highScore)
        //    {
        //        highScore = score;
        //    }
        //    isDataUpdate = true;
        //}


        //return count > 0 ? true : false;

        return CheckMap();
    }

    /// <summary>
    /// 记录当前的最大行数
    /// </summary>
    /// <param name="row"></param>
    private void RecordCurretRowNumber(int row)
    {
        currentRow = currentRow > row ? currentRow : row;
    }

    /// <summary>
    /// 检测当前放下的这个Block所占的几行是否已满
    /// </summary>
    /// <param name="row"></param>
    private void CheckMapByRow(int row)
    {
        bool isFull = CheckIsRowFull(row);
        if (isFull)
        {
            DeleteRow(row);
            MoveDownRowsAbove(row + 1);
        }
    }


    /// <summary>
    /// 检查地图是否需要消除
    /// </summary>
    private bool CheckMap()
    {
        int count = 0;
        for (int i = 0; i < MAX_ROWS; i++)
        {
            bool isFull =  CheckIsRowFull(i);
            if (isFull)
            {
                count++;
                DeleteRow(i);
                MoveDownRowsAbove(i + 1);
                i--;
            }
        }

        if (count > 0)
        {
            score += (count*100);
            if (score > highScore)
            {
                highScore = score;
            }
            isDataUpdate = true;
        }


        return count > 0 ? true : false;
    }


    /// <summary>
    /// 检测该行的每列是否已满
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    private bool CheckIsRowFull(int row)
    {
        //每列
        for (int i = 0; i < MAX_COLUMNS; i++)
        {
            if (map[i, row] == null) return false;
        }
        return true;
    }

    /// <summary>
    /// 删除该行的每列元素
    /// </summary>
    /// <param name="row"></param>
    private void DeleteRow(int row)
    {
        for (int i = 0; i < MAX_COLUMNS; i++)
        {
            Destroy(map[i, row].gameObject);
            map[i, row] = null;
        }
    }


    /// <summary>
    /// 将前一行及以上全部移下来
    /// </summary>
    /// <param name="row"></param>
    private void MoveDownRowsAbove(int row)
    {
        for (int i = row; i <= currentRow; i++)
        {
            MoveDownRow(i);
        }
        currentRow = currentRow - 1;
    }

    private void MoveDownRow(int row)
    {
        for (int i = 0; i < MAX_COLUMNS; i++)
        {
            if (map[i, row] != null)
            {
                map[i, row - 1] = map[i, row];
                map[i, row] = null;
                map[i, row - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    private void LoadData()
    {
        highScore =  PlayerPrefs.GetInt("HighScore", 0);
        numberGame =  PlayerPrefs.GetInt("NumberGame", 0);
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("HighScore",highScore);
        PlayerPrefs.SetInt("NumberGame", numberGame);
    }


    public bool IsGameOver()
    {
        for (int i = NORMAL_ROWS; i < MAX_ROWS; i++)
        {
            for (int j = 0; j < MAX_COLUMNS; j++)
            {
                if (map[j, i] != null)
                {
                    numberGame++;
                    SaveData();
                    return true;
                }
            }
        }

        return false;
    }

    public void Restart()
    {
        for (int i = 0; i < MAX_COLUMNS; i++)
        {
            for (int j = 0; j < MAX_ROWS; j++)
            {
                if (map[i, j] != null) 
                {
                    Destroy(map[i, j].gameObject);
                    map[i, j] = null;
                }
            }
        }

        score = 0;
    }

    public void ClearData()
    {
        score = 0;
        highScore = 0;
        numberGame = 0;
        SaveData();
    }

}
