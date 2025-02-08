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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.Instance.isFinish) return;
        GroundMone();
        BackGroundMove();
    }

    private void GroundMone()
    {
        ground_1.transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        ground_2.transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);

        if (ground_1.transform.position.x < -BG_WIDTH)
        {
            ground_1.transform.position = ground_2.transform.position + new Vector3(BG_WIDTH, 0, 0);
        }
        if (ground_2.transform.position.x < -BG_WIDTH)
        {
            ground_2.transform.position = ground_1.transform.position + new Vector3(BG_WIDTH, 0, 0);
        }
    }

    private void BackGroundMove()
    {
        backGround_1.transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        backGround_2.transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);

        if (backGround_1.transform.position.x < -BG_WIDTH)
        {
            backGround_1.transform.position = backGround_2.transform.position + new Vector3(BG_WIDTH, 0, 0);
        }
        if (backGround_2.transform.position.x < -BG_WIDTH)
        {
            backGround_2.transform.position = backGround_1.transform.position + new Vector3(BG_WIDTH, 0, 0);
        }
    }
}
