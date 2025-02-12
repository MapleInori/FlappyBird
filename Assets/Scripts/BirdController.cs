using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class BirdController : MonoBehaviour
{
    [Range(1, 10)]
    public float R = 5f; // 控制小鸟上下浮动的速度
    [Range(0, 1)]
    public float IdleHeight = 0.5f; // 小鸟在待机状态时的浮动高度

    private float Radian = 0; // 当前浮动的角度（弧度）
    private Vector3 orgPosition; // 初始位置

    public float Gravity = -9.81f; // 重力加速度
    public float JumpHeight = 1.3f; // 跳跃高度
    public Vector3 Velocity = Vector3.zero; // 当前速度

    public float MaxVelocity = -15f; // 最大下落速度

    private float RotationZ = 0; // 小鸟的旋转角度
    public float RotateSpeed = 8; // 旋转速度（弧度）
    private float JumpVelocity; // 跳跃的初始速度

    private Collider2D preCollieder; // 上次碰撞的物体

    void Start()
    {
        orgPosition = transform.position; // 记录小鸟的初始位置
        preCollieder = null; // 初始化碰撞器
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.Instance.isPaused) return;
        if (GameStateManager.Instance.isStart || GameStateManager.Instance.isReady)
        {
            Idle();
        }
        else if (GameStateManager.Instance.isPlaying)
        {
            CustomGravity();
        }
        else if(GameStateManager.Instance.isFinish)
        {
            HandleBirdDie();
        }

    }

    // 控制小鸟在待机状态时的上下浮动
    private void Idle()
    {
        Radian += R * Time.deltaTime; 
        var height = Mathf.Sin(Radian) * IdleHeight; // 根据角度计算当前浮动高度
        transform.position = orgPosition + new Vector3(0, height, 0); 
    }

    // 控制小鸟的重力和跳跃
    private void CustomGravity()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0) )
        {
            Vector2 clickPosition;

            // 获取点击位置
            if (Input.touchCount > 0)
            {
                clickPosition = Input.GetTouch(0).position; // 触摸位置
            }
            else
            {
                clickPosition = Input.mousePosition; // 鼠标位置
            }

            if (!InitSceenSize.ClickPause(clickPosition))
            {
                // 如果点击位置不在暂停按钮区域内，触发跳跃
                Jump();
            }
            
            //if (GameStateManager.Instance.isPlaying) Jump();
        }
        Velocity.y += Gravity * Time.deltaTime; // 受重力影响更新垂直速度
        if (Velocity.y < MaxVelocity) Velocity.y = MaxVelocity; // 限制最大下落速度
        transform.position += Velocity * Time.deltaTime; // 更新小鸟的位置

        // 控制小鸟的旋转角度，使其根据下落速度旋转
        if (Velocity.y < -JumpVelocity * 0.1f)
        {
            RotationZ -= RotateSpeed * Time.deltaTime * 1000 * Mathf.Deg2Rad; // 放大1000倍才显得正常，帧率太高导致deltaTime过低？

            RotationZ = Mathf.Max(-90, RotationZ);// 限制旋转角度不超过-90°
        }

        transform.eulerAngles = new Vector3(0, 0, RotationZ);
    }

    // 鸟作为触发器去接触其他物体，要么是其他物体都设置为触发器检测是否碰到鸟，那么需要给每个会碰到鸟的物体写脚本。
    // 如果碰到后那些物体需要各种处理的话，也许其他作为触发器好点，毕竟还要给它们写其他东西。
    // 如果东西多的话，鸟要接触的东西太多了，集中在这里看起来也许不如分散到各自身上处理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.tag);

        if(collision.CompareTag("ground") || collision.CompareTag("pipe"))
        {
            //GameStateManager.Instance.Finish();
            GameStateManager.Instance.SetState(GameState.Finished);
        }
        if (collision.CompareTag("sky"))
        {
            Velocity = new Vector3(0, -2, 0);
        }

        // 避免刚好接触时死亡，触发两次加分的情况
        if (collision.CompareTag("checkPoint") && ((preCollieder==null)|| preCollieder.GetComponent<Transform>().position != collision.GetComponent<Transform>().position))
        {
            GameStateManager.Instance.GetScore();
            preCollieder = collision;
        }
    }

    // 防止卡进去然后穿过去
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("sky"))
        {
            Velocity = new Vector3(0, -2, 0);
        }
    }

    private void HandleBirdDie()
    {
        float distance = -Camera.main.orthographicSize + 1.28f + 0.64f * 0.5f; // 通过手动计算得到的地面高度
        if(transform.position.y > distance)
        {
            CustomGravity();
        }
    }

    // 重启游戏时重置小鸟的位置和状态
    public void Restart()
    {
        transform.position = orgPosition; // 恢复初始位置
        transform.eulerAngles = Vector3.zero; // 恢复旋转角度
        RotationZ = 0; // 恢复旋转值
        Velocity = Vector3.zero; // 恢复速度
    }

    // 使小鸟跳跃
    public void Jump()
    {
        Velocity.y = MathF.Sqrt(JumpHeight * -2 * Gravity); // 计算跳跃的初始速度
        JumpVelocity = Velocity.y; // 记录跳跃速度
        RotationZ = 30; // 跳跃时调整旋转角度
    }
}
