using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class MainPanel : MonoBehaviour
{
    // 开始界面
    public Button StartButton;
    public Button RestartButton;
    public Image Title;
    public GameObject UIPipe;
    public CanvasGroup StartUI;

    // 准备界面
    public Button ReadyToStart;    
    public CanvasGroup Tutorial;

    // 游戏界面
    public Text CurrentScore;

    // 结束界面
    public GameObject GameOverUI;
    public Text FinalScore;
    public Text BestScore;
    public Image Medal;
    public List<Sprite> Medals;
    public Image NewIcon;

    public BirdController birdController;
    public PipeCreate pipeCreate;

    private float FadeTime = 0.4f;
    private bool isClickStart;

    // Start is called before the first frame update
    void Start()
    {
        StartButton.onClick.AddListener(OnClickStart);
        RestartButton.onClick.AddListener(OnClickRestart);
        ReadyToStart.onClick.AddListener(GameStart);

        GameOverUI.gameObject.SetActive(false);
        ReadyToStart.gameObject.SetActive(false);

        NewIcon.gameObject.SetActive(false);
        isClickStart = false;

        CurrentScore.text = "0";

        GameStateManager.Instance.Setup(this);
    }

    private void OnClickStart()
    {
        // 点击过快会触发两次点击，从而出现鸟跳跃被打断重新回到ready状态，在Restart恢复
        if (isClickStart) return;
        isClickStart = true;

        GameStateManager.Instance.Ready();
        // 恢复初始UI状态
        NewIcon.gameObject.SetActive(false);
        Medal.gameObject.SetActive(true);

        ShowReadyUI();
    }

    public void ShowStartUI()
    {
        // 已被隐藏且关闭，先启用，再渐显
        Title.gameObject.SetActive(true);
        StartButton.gameObject.SetActive(true);
        isClickStart = false;
        StartUI.DOFade(1, FadeTime);

        UIPipe.gameObject.SetActive(true);
        CurrentScore.gameObject.SetActive(false);

        birdController.Restart();
        pipeCreate.Restart();
    }

    public void ShowReadyUI()
    {
        // 隐藏UI，然后关闭UI
        StartUI.DOFade(0, FadeTime).onComplete = () =>
        {
            StartButton.gameObject.SetActive(false);
            Title.gameObject.SetActive(false);
        };
        UIPipe.gameObject.SetActive(false);

        CurrentScore.gameObject.SetActive(true);
        CurrentScore.text = "0";


        Tutorial.gameObject.SetActive(true);
        Tutorial.DOFade(1, FadeTime);

        ReadyToStart.gameObject.SetActive(true);
    }

    private void GameStart()
    {
        GameStateManager.Instance.Start();
        birdController.JumpOnce();
        Tutorial.DOFade(0, FadeTime).onComplete = () =>
        {
            Tutorial.gameObject.SetActive(false);
        };
        ReadyToStart.gameObject.SetActive(false);
    }

    private void OnClickRestart()
    {
        GameStateManager.Instance.Restart();
        GameOverUI.gameObject.SetActive(false);
        ShowStartUI();
    }

    //public void ShowRestart()
    //{
    //    RestartButton.gameObject.SetActive(true);
    //}

    public void ShowGameOverUI()
    {
        int score = int.Parse(CurrentScore.text);
        CurrentScore.gameObject.SetActive(false);
        GameOverUI.gameObject.SetActive(true);

        // 最高分处理
        if (score > PlayerPrefs.GetInt("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", score);
            NewIcon.gameObject.SetActive(true);
        }

        if(score <10)
        {
            Medal.gameObject.SetActive(false);
        }
        else if(score <20 && score >=10)
        {
            Medal.sprite = Medals[0];
        }
        else if(score < 50&& score >= 20)
        {
             Medal.sprite = Medals[1];
        }
        else if(score < 100 && score >=50)
        {
            Medal.sprite = Medals[2];
        }
        else
        {
            Medal.sprite = Medals[3];
        }

        FinalScore.text = score.ToString();
        BestScore.text = PlayerPrefs.GetInt("BestScore").ToString();

    }

}
