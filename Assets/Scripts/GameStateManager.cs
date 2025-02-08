using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager
{
    static GameStateManager instance;

    public static GameStateManager Instance
    {
        get 
        { 
            if (instance == null)
                instance = new GameStateManager();
            return instance; 
        }
    }

    GameStateManager() { }

    public bool isStart;
    public bool isFinish;
    public bool isReady;

    private MainPanel mainPanel;

    public void Setup(MainPanel panel)
    {
        mainPanel = panel;
    }

    public void Ready()
    {
        isReady = true;
        isStart = false;
        isFinish = false;
    }

    public void Start()
    {
        isReady = false;
        isStart = true;
        isFinish = false;
    }

    public void Finish()
    {
        isReady = false;
        isStart = false;
        isFinish = true;

        mainPanel.ShowGameOverUI();
    }

    public void Restart()
    {
        isStart = isFinish = isReady = false;
    }

    public void GetScore()
    {
        mainPanel.CurrentScore.text = (int.Parse(mainPanel.CurrentScore.text)+1).ToString();
    }
}
