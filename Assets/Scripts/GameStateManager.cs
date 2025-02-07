using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager
{
    public static bool isStart;
    public static bool isFinish;
    public static bool isReady;

    private static MainPanel mainPanel;

    public static void Setup(MainPanel panel)
    {
        mainPanel = panel;
    }

    public static void Ready()
    {
        isReady = true;
        isStart = false;
        isFinish = false;
    }

    public static void Start()
    {
        isReady = false;
        isStart = true;
        isFinish = false;
    }

    public static void Finish()
    {
        isReady = false;
        isStart = false;
        isFinish = true;

        mainPanel.ShowRestart();
    }

    public static void Restart()
    {
        isStart = isFinish = isReady = false;
    }
}
