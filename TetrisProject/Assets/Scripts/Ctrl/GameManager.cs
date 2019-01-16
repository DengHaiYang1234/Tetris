using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isPause = true; //游戏是否暂停

    private Shape currentShape = null;

    private Transform blockHodler;

    public Shape[] shapes;

    public Color[] colors;

    private Ctrl ctrl;

    private void Awake()
    {
        ctrl = GetComponent<Ctrl>();
        blockHodler = transform.Find("BlockHolder");
    }

    private void Update()
    {
        if (isPause) return;
        if (currentShape == null)
        {
            SpawnShape();
        }
    }


    /// <summary>
    /// 方块生成
    /// </summary>
    void SpawnShape()
    {
        int index = Random.Range(0, shapes.Length - 1);

        int indexColor = Random.Range(0, colors.Length - 1);

        currentShape = GameObject.Instantiate(shapes[index]);
        currentShape.transform.SetParent(blockHodler);
        //初始化
        currentShape.Init(colors[indexColor], ctrl,this);
    }

    public void StartGame()
    {
        isPause = false;
        if (currentShape != null)
            currentShape.Resume();
    }

    public void PauseGame()
    {
        isPause = true;
        if (currentShape != null)
            currentShape.Pause();
    }

    public void FallDown()
    {
        currentShape = null;
        if (ctrl.model.isDataUpdate)
            ctrl.view.UpdateGameUI(ctrl.model.Score,ctrl.model.HighScore);

        foreach (Transform t in blockHodler)
        {
            if (t.childCount <= 1)
            {
                Destroy(t.gameObject);
            }
        }

        if (ctrl.model.IsGameOver())
        {
            PauseGame();
            ctrl.view.ShowGameOver(ctrl.model.Score);
        }
    }

    public void ClearShape()
    {
        if (currentShape != null)
        {
            Destroy(currentShape.gameObject);
            currentShape = null;
        }
    }
}
