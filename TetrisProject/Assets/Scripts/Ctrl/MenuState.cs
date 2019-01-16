using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : FSMState
{
    private void Awake()
    {
        stateID = StateID.Menu;
        //注册StartBtuuonClick，绑定layState
        AddTransition(Transition.StartBtuuonClick, StateID.Play);
    }

    /// <summary>
    /// 设置默认状态时，会调用DoBeforeEntering(进入之前调用) 
    /// </summary>
    public override void DoBeforeEntering()
    {
        //显示菜单面板
        ctrl.view.ShowMenu();
        //动效
        ctrl.cameraManager.ZoomOut();
    }


    /// <summary>
    /// 离开该状态之前
    /// </summary>
    public override void DoBeforeLeaving()
    {
        ctrl.view.HideMenu();
    }

    public void OnStartButtonClick()
    {
        ctrl.audioManager.PlayCursor();
        //跳转至PlayState
        fsm.PerformTransition(Transition.StartBtuuonClick);
    }

    public void OnRankButtonClick()
    {
        ctrl.audioManager.PlayCursor();
        ctrl.view.ShowRankUI(ctrl.model.HighScore,ctrl.model.Score,ctrl.model.NumberScore);
    }

    public void OnDestroyButtonClick()
    {
        ctrl.model.ClearData();
        OnRankButtonClick();
    }

    public void OnRestartButtonClick()
    {
        ctrl.model.Restart();
        ctrl.gameManager.ClearShape();
        fsm.PerformTransition(Transition.StartBtuuonClick);
    }


}
