using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitSceenSize : MonoBehaviour
{
    public RectTransform StartUI;
    public RectTransform GamingUI;
    public RectTransform FinishUI;

    private int screenWidth;
    private int screenHeight;

    public RectTransform pauseButtonRectTransform;

    private static float left;
    private static float right;
    private static float top;
    private static float bottom;

    void Start()
    {
        screenHeight = UnityEngine.Screen.height;
        screenWidth = UnityEngine.Screen.width;
        //int screen_min = Mathf.Min(screenHeight, screenWidth);
        //int screen_max = Mathf.Max(screenWidth, screenHeight);

        //GetComponent<CanvasScaler>().scaleFactor = screen_min / (float)1080;

        StartUI.sizeDelta = new Vector2(screenWidth, screenHeight);
        GamingUI.sizeDelta = new Vector2 (screenWidth, screenHeight);
        FinishUI.sizeDelta = new Vector2 (screenWidth, screenHeight);

        // 获取暂停按钮的世界坐标
        Vector3 buttonWorldPosition = pauseButtonRectTransform.position;

        // 获取暂停按钮的尺寸（宽度和高度）
        Vector2 buttonSize = pauseButtonRectTransform.rect.size;
        // 计算暂停按钮的四个边界位置
        left = buttonWorldPosition.x - buttonSize.x / 2;
        right = buttonWorldPosition.x + buttonSize.x / 2;
        top = buttonWorldPosition.y + buttonSize.y / 2;
        bottom = buttonWorldPosition.y - buttonSize.y / 2;

        //Debug.Log(pauseButtonRectTransform.position.y + "+" + pauseButtonRectTransform.rect.size.y / 2 + "=" + top);
    }

    public static bool ClickPause(Vector3 position)
    {
        if (position.x >= left && position.x <= right && position.y >= bottom && position.y <= top)
        {
            return true;
        }
        return false;
    }
}
