using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BirdController : MonoBehaviour
{
    [Range(1, 10)]
    public float R = 5f;
    [Range(0, 1)]
    public float IdleHeight = 0.5f;

    private float Radian = 0;
    private Vector3 orgPosition;

    public float Gravity = -9.81f;
    public float JumpHeight = 1.3f;
    public Vector3 Velocity = Vector3.zero;

    public float MaxVelocity = -15f;

    private float RotationZ = 0;
    public float RotateSpeed = 8; // 弧度
    private float JumpVelocity;


    void Start()
    {
        orgPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.Instance.isFinish == false && GameStateManager.Instance.isStart == false)
        {
            Idle();
        }
        else if (GameStateManager.Instance.isStart)
        {
            CustomGravity();
        }
        else if(GameStateManager.Instance.isFinish)
        {
            HandleBirdDie();
        }

    }

    private void Idle()
    {
        Radian += R * Time.deltaTime;
        var height = Mathf.Sin(Radian) * IdleHeight;
        transform.position = orgPosition + new Vector3(0, height, 0);
    }

    private void CustomGravity()
    {
        if (Input.GetMouseButtonDown(0) && GameStateManager.Instance.isStart)
        {
            Velocity.y = MathF.Sqrt(JumpHeight * -2 * Gravity);
            JumpVelocity = Velocity.y;
            RotationZ = 30;
        }
        Velocity.y += Gravity * Time.deltaTime;
        if (Velocity.y < MaxVelocity) Velocity.y = MaxVelocity;
        transform.position += Velocity * Time.deltaTime;

        if (Velocity.y < -JumpVelocity * 0.1f)
        {
            RotationZ -= RotateSpeed * Time.deltaTime * 1000 * Mathf.Deg2Rad; // 放大1000倍才显得正常，帧率太高导致deltaTime过低？
            //Debug.Log(RotateSpeed * Time.deltaTime * 1000 * Mathf.Deg2Rad);
            RotationZ = Mathf.Max(-90, RotationZ);
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
            GameStateManager.Instance.Finish();
        }
        if (collision.CompareTag("sky"))
        {
            Velocity = new Vector3(0, -2, 0);
        }
        if (collision.CompareTag("checkPoint"))
        {
            GameStateManager.Instance.GetScore();
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
        float distance = -Camera.main.orthographicSize + 1.28f + 0.64f * 0.5f;
        if(transform.position.y > distance)
        {
            CustomGravity();
        }
    }

    public void Restart()
    {
        transform.position = orgPosition;
        transform.eulerAngles = Vector3.zero;
        RotationZ = 0;
        Velocity = Vector3.zero;
    }

    public void JumpOnce()
    {
        // 只跳一下应该没理由超过最大值，上边复制过来应该可以删掉if
        Velocity.y = MathF.Sqrt(JumpHeight * -2 * Gravity);
        JumpVelocity = Velocity.y;
        RotationZ = 30;

        Velocity.y += Gravity * Time.deltaTime;
        transform.position += Velocity * Time.deltaTime;

        RotationZ -= RotateSpeed * Time.deltaTime * 1000 * Mathf.Deg2Rad;

        transform.eulerAngles = new Vector3(0, 0, RotationZ);
    }
}
