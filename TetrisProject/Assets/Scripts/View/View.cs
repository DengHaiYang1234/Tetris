using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class View : MonoBehaviour
{
    private Ctrl ctrl;
    private RectTransform logoName;
    private RectTransform menuUI;
    private RectTransform gameUI;
    private RectTransform gameOverUI;
    private GameObject restartBtn;
    private GameObject settingUI;
    private GameObject mute;
    private GameObject rankUI;
    private Text score;
    private Text highScore;
    private Text gameOverScore;
    private Text rankScore;
    private Text rankHighScore;
    private Text rankNumbersGame;

    private void Start()
    {
        ctrl = GameObject.FindGameObjectWithTag("Ctrl").GetComponent<Ctrl>();
        logoName = transform.Find("Canvas/LogoName") as RectTransform;
        menuUI = transform.Find("Canvas/MenuUI") as RectTransform;
        gameUI = transform.Find("Canvas/GameUI") as RectTransform;
        gameOverUI = transform.Find("Canvas/GameOverUI") as RectTransform;
        settingUI = transform.Find("Canvas/SettingUI").gameObject;
        restartBtn = menuUI.transform.Find("Button_Restart").gameObject;
        mute = transform.Find("Canvas/SettingUI/Button_Audio/Mute").gameObject;
        rankUI = transform.Find("Canvas/RankUI").gameObject;
        score = transform.Find("Canvas/GameUI/ScoreLable/Text").GetComponent<Text>();
        highScore = transform.Find("Canvas/GameUI/MaxScoreLable/Text").GetComponent<Text>();
        gameOverScore = transform.Find("Canvas/GameOverUI/Score").GetComponent<Text>();
        rankScore = transform.Find("Canvas/RankUI/ScoreLable/Score").GetComponent<Text>();
        rankHighScore = transform.Find("Canvas/RankUI/MaxScoreLable/Score").GetComponent<Text>();
        rankNumbersGame = transform.Find("Canvas/RankUI/NumberGamesLable/Score").GetComponent<Text>();
    }

    public void ShowMenu()
    {
        logoName.gameObject.SetActive(true);
        logoName.DOAnchorPosY(-90f, 0.5f);
        menuUI.gameObject.SetActive(true);
        menuUI.DOAnchorPosY(27.3f, 0.5f);
    }


    public void HideMenu()
    {

        logoName.DOAnchorPosY(50f, 0.5f).OnComplete(delegate
        {
            logoName.gameObject.SetActive(false);
        });


        menuUI.DOAnchorPosY(-30.5f, 0.5f).OnComplete(delegate ()
        {
            menuUI.gameObject.SetActive(false);
        });
        
    }

    public void UpdateGameUI(int score, int high)
    {
        this.score.text = score.ToString();
        this.highScore.text = high.ToString();
    }


    public void ShowGameUI(int score =0,int high = 0)
    {
        this.score.text = score.ToString();
        this.highScore.text = high.ToString();

        gameUI.gameObject.SetActive(true);
        gameUI.DOAnchorPosY(-100f, 0.5f);
    }

    public void HideGameUI()
    {
        gameUI.DOAnchorPosY(100f, 0.5f).OnComplete(delegate ()
        {
            gameUI.gameObject.SetActive(false);
        });
    }

    public void ShowRestartButton()
    {
        restartBtn.gameObject.SetActive(true);
    }

    public void OnHomeButtonClick()
    {
        ctrl.audioManager.PlayCursor();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowGameOver(int score = 0)
    {
        gameOverUI.gameObject.SetActive(true);
        gameOverScore.text = score.ToString();
    }

    public void HideGameOverUI()
    {
        ctrl.audioManager.PlayCursor();
        gameOverUI.gameObject.SetActive(false);
    }

    public void OnSettingButtonClick()
    {
        ctrl.audioManager.PlayCursor();
        settingUI.SetActive(true);
    }

    public void SetMuteActive(bool isActive)
    {
        mute.SetActive(isActive);
    }

    public void OnSettingUIClick()
    {
        ctrl.audioManager.PlayCursor();
        settingUI.SetActive(false);
    }



    public void ShowRankUI(int highScore,int score,int numberGame)
    {
        rankScore.text = score.ToString();
        rankHighScore.text = highScore.ToString();
        rankNumbersGame.text = numberGame.ToString();
        rankUI.SetActive(true);
    }

    public void HideRankUI()
    {
        rankUI.SetActive(false);
    }
}
