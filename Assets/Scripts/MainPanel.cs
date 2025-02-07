using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class MainPanel : MonoBehaviour
{
    public Button StartButton;
    public Button RestartButton;
    public Button ReadyToStart;

    public Image Title;
    public GameObject UIPipe;
    public BirdController birdController;
    public PipeCreate pipeCreate;

    public CanvasGroup StartUI;

    public Text Score;
    public Image Tutorial;

    private float FadeTime = 0.4f;
    private bool isClickStart;

    // Start is called before the first frame update
    void Start()
    {
        StartButton.onClick.AddListener(OnClickStart);
        RestartButton.onClick.AddListener(OnClickRestart);
        ReadyToStart.onClick.AddListener(GameStart);

        RestartButton.gameObject.SetActive(false);
        ReadyToStart.gameObject.SetActive(false);

        isClickStart = false;

        GameStateManager.Setup(this);
    }

    private void OnClickStart()
    {
        // 点击过快会触发两次点击，从而出现鸟跳跃被打断重新回到ready状态，在Restart恢复
        if (isClickStart) return;
        isClickStart = true;

        GameStateManager.Ready();

        // 隐藏UI，然后关闭UI
        StartUI.DOFade(0, FadeTime).onComplete = () =>
        {
            StartButton.gameObject.SetActive(false);
            Title.gameObject.SetActive(false);
        };
        UIPipe.gameObject.SetActive(false);

        Score.gameObject.SetActive(true);
        Tutorial.gameObject.SetActive(true);
        ReadyToStart.gameObject.SetActive(true);
    }

    private void GameStart()
    {
        GameStateManager.Start();
        birdController.JumpOnce();
        Tutorial.gameObject.SetActive(false);
        ReadyToStart.gameObject.SetActive(false);
    }

    private void OnClickRestart()
    {
        GameStateManager.Restart();
        RestartButton.gameObject.SetActive(false);

        // 已被隐藏且关闭，先启用，再渐显
        Title.gameObject.SetActive(true);
        StartButton.gameObject.SetActive(true);
        isClickStart = false;
        StartUI.DOFade(1, FadeTime);

        UIPipe.gameObject.SetActive(true);
        Score.gameObject.SetActive(false);

        birdController.Restart();
        pipeCreate.Restart();
    }

    public void ShowRestart()
    {
        RestartButton.gameObject.SetActive(true);
    }

}
