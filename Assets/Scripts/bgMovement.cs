using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMovement : MonoBehaviour
{
    private float BG_WIDTH = 9.8f;

    public Transform ground_1;
    public Transform ground_2;

    public Transform backGround_1;
    public Transform backGround_2;

    [Range(0,10)]
    public float moveSpeed = 3f;

    void Update()
    {
        if (GameStateManager.Instance.isFinish || GameStateManager.Instance.isPaused ) return;
        MoveBackground(ground_1, ground_2);
        MoveBackground(backGround_1, backGround_2);

    }

    private void MoveBackground(Transform obj1, Transform obj2)
    {
        obj1.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        obj2.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);

        if (obj1.position.x < -BG_WIDTH)
        {
            obj1.position = obj2.position + new Vector3(BG_WIDTH, 0, 0);
        }
        if (obj2.position.x < -BG_WIDTH)
        {
            obj2.position = obj1.position + new Vector3(BG_WIDTH, 0, 0);
        }
    }
}
