using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    public Button StartButton;
    public Button RestartButton;

    public GameObject UIPipe;
    public BirdController birdController;
    public PipeCreate pipeCreate;
    // Start is called before the first frame update
    void Start()
    {
        StartButton.onClick.AddListener(OnClickStart);
        RestartButton.onClick.AddListener(OnClickRestart);

        GameStateManager.Setup(this);
    }

    private void OnClickStart()
    {
        GameStateManager.Start();

        StartButton.gameObject.SetActive(false);
        UIPipe.gameObject.SetActive(false);
    }

    private void OnClickRestart()
    {
        GameStateManager.Restart();
        RestartButton.gameObject.SetActive(false);
        StartButton.gameObject.SetActive(true);
        UIPipe.gameObject.SetActive(true);

        birdController.Restart();
        pipeCreate.Restart();
    }

    public void ShowRestart()
    {
        RestartButton.gameObject.SetActive(true);
    }
}
