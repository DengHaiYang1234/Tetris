using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    private Ctrl ctrl;

    private GameManager gameManager;

    private Transform pivot;

    private bool isPause = false;

    private float timer = 0;

    private float stepTime = 0.8f;

    private int multiple = 10;

    private bool isSpeed = false;

    private void Awake()
    {
        pivot = transform.Find("Piovt");
    }

    private void Update()
    {
        if (isPause) return;
        timer += Time.deltaTime;
        if (timer > stepTime)
        {
            timer = 0;
            Fall();
        }
        InputControl();
    }

    /// <summary>
    /// 方块初始化
    /// </summary>
    /// <param name="color"></param>
    /// <param name="ctrl"></param>
    /// <param name="gameManager"></param>
    public void Init(Color color,Ctrl ctrl,GameManager gameManager)
    {
        foreach (Transform t in transform)
        {
            if (t.tag == "Block")
            {
                t.GetComponent<SpriteRenderer>().color = color;
            }
        }
        this.ctrl = ctrl;
        this.gameManager = gameManager;
    }

    /// <summary>
    /// 方块掉落判断
    /// </summary>
    public void Fall()
    {
        Vector3 pos = transform.position;
        pos.y -= 1;
        transform.position = pos;
        
        //到达指定位置
        if (ctrl.model.IsVaildMapPosition(this.transform) == false)
        {
            //位置跳转，此时不能再往下掉落
            pos.y += 1;
            transform.position = pos;
            //该方块停止掉落
            isPause = true;
            //整行消除
            bool isLineClear =  ctrl.model.PlaceShape(this.transform);
            //播放音效
            if (isLineClear) ctrl.audioManager.PlayClearLine();
            //掉落检测
            gameManager.FallDown();
            return;
        }

        ctrl.audioManager.PlayDrop();
    }

    private void InputControl()
    {

        #region 左右滑动
        float h = 0;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            h = -1f;
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            h = 1f;
        }

        if (h != 0)
        {
            Vector3 pos = transform.position;
            pos.x += h;
            transform.position = pos;
            
            //控制Block左右滑动 但不超过地图有效位置  若超过，则将位置还原
            if (ctrl.model.IsVaildMapPosition(this.transform) == false)
            {
                pos.x -= h;
                transform.position = pos;
            }
            else
            {
                ctrl.audioManager.PlayControl();
            }
        }

        #endregion

        #region 旋转
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(pivot.position, Vector3.forward, -90);
            //控制Block加速下滑 但不超过地图有效位置
            if (ctrl.model.IsVaildMapPosition(this.transform) == false)
            {
                transform.RotateAround(pivot.position, Vector3.forward, 90);
            }
            else
            {
                ctrl.audioManager.PlayControl();
            }
        }

        #endregion

        #region 加速下滑
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isSpeed = true;
            stepTime /= multiple;
        }
        else if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            isSpeed = false;
            stepTime = 0.8f;
        }
        #endregion
    }

    public void Pause()
    {
        isPause = true;
    }

    public void Resume()
    {
        isPause = false;
    }
}
