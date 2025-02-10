using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 主界面，点击start但是在游戏开始前的准备状态（此时显示游戏引导），游戏中状态，游戏结束状态，暂停
public enum GameState {Start, Ready, Playing, Finished, Paused }

public class GameStateManager
{
    private static readonly Lazy<GameStateManager> instance = new Lazy<GameStateManager>(() => new GameStateManager());

    public static GameStateManager Instance => instance.Value;

    GameStateManager() { }

    public bool isStart;
    public bool isReady;
    public bool isPlaying;
    public bool isFinish;
    public bool isPaused;

    private MainPanel mainPanel;

    public void Setup(MainPanel panel)
    {
        mainPanel = panel;
    }

    public void SetState(GameState state)
    {
        switch (state)
        {
            case GameState.Start:
                isStart = true; isReady = false; isPlaying = false; isFinish = false; isPaused = false; break;
            case GameState.Ready:
                isStart = false ; isReady = true; isPlaying = false; isFinish = false; isPaused = false; break;
            case GameState.Playing:
                isStart = false; isReady = false; isPlaying = true; isFinish = false; isPaused = false; break;
            case GameState.Finished:
                isStart = false; isReady = false; isPlaying = false; isFinish = true; isPaused = false;
                mainPanel.ShowGameOverUI();
                break;
            case GameState.Paused:
                isPaused = true;
                Time.timeScale = 0;
                break;
        }
    }

    // 游戏恢复
    public void Resume()
    {
        SetState(GameState.Playing);  // 恢复游戏状态
        Time.timeScale = 1;  // 恢复游戏速度
    }

    // 重启游戏
    public void Restart()
    {
        SetState(GameState.Start);  // 设置为开始状态
        Time.timeScale = 1;  // 恢复时间缩放
    }

    // 获取并更新分数
    public void GetScore()
    {
        int currentScore;
        if (int.TryParse(mainPanel.CurrentScore.text, out currentScore))
        {
            mainPanel.CurrentScore.text = (currentScore + 1).ToString();
        }
    }
}
