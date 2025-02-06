using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    [Range(1, 10)]
    public float R = 5f;
    [Range(0, 1)]
    public float IdleHeight = 0.5f;

    private float Radian = 0;
    private Vector3 orgPosition;

    public float Gravity = -9.81f;
    public float JumpHeigh = 1.3f;
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
        if (GameStateManager.isFinish == false && GameStateManager.isStart == false)
        {
            Idle();
        }
        else if (GameStateManager.isStart)
        {
            CustomGravity();
        }
        else if(GameStateManager.isFinish)
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
        if (Input.GetMouseButtonDown(0) && GameStateManager.isStart)
        {
            Velocity.y = MathF.Sqrt(JumpHeigh * -2 * Gravity);
            JumpVelocity = Velocity.y;
            RotationZ = 30;
        }
        Velocity.y += Gravity * Time.deltaTime;
        if (Velocity.y < MaxVelocity) Velocity.y = MaxVelocity;
        transform.position += Velocity * Time.deltaTime;

        if(Velocity.y < - JumpVelocity *0.5f)
        {
            RotationZ -= RotateSpeed * Time.deltaTime * 1000 * Mathf.Deg2Rad; // 帧率太高导致deltaTime过低？
            //Debug.Log(RotateSpeed * Time.deltaTime * 1000 * Mathf.Deg2Rad);
            RotationZ = Mathf.Max(-90, RotationZ);
        }

        transform.eulerAngles = new Vector3(0, 0, RotationZ);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.tag);

        if(collision.CompareTag("ground") || collision.CompareTag("pipe"))
        {
            GameStateManager.Finish();
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
}
