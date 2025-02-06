using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager
{
    public static bool isStart;
    public static bool isFinish;

    private static MainPanel mainPanel;

    public static void Setup(MainPanel panel)
    {
        mainPanel = panel;
    }

    public static void Start()
    {
        isStart = true;
        isFinish = false;
    }

    public static void Finish()
    {
        isFinish = true;
        isStart = false;

        mainPanel.ShowRestart();
    }

    public static void Restart()
    {
        isStart = isFinish = false;
    }
}
