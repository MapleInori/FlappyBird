using System.Collections.Generic;
using UnityEngine;

public class PipeCreate : MonoBehaviour
{

    public BgMovement bgMove;

    private List<GameObject> pipeList;
    private float Distance = 5f;
    private float CameraHalfWidth;

    private GameObject pipePrefab;
    // Start is called before the first frame update
    void Start()
    {
        pipePrefab = Resources.Load<GameObject>("Prefab/PipeObj"); // 只加载一次

        CameraHalfWidth = Screen.width * 1f / Screen.height * Camera.main.orthographicSize;
        //Debug.Log(CameraHalfWidth);
        pipeList = new List<GameObject>();
        
        // 也能通过协程的方式不断生成
        for(int i =0; i< 5;i++)
        {
            var go = GameObject.Instantiate(pipePrefab);
            go.transform.position = new Vector3(i * Distance, Random.Range(-3f,4.5f), 0) + new Vector3(8,0,0);
            go.transform.SetParent(this.transform,true);

            pipeList.Add(go);
        }
    }

    public void Restart()
    {
        for (int i = 0; i < 5; i++)
        {
            var go = pipeList[i];
            go.transform.position = new Vector3(i * Distance, Random.Range(-3f, 4.5f), 0) + new Vector3(8, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStateManager.Instance.isPlaying)
        {
            foreach (var item in pipeList)
            {
                item.transform.position += new Vector3(-Time.deltaTime * bgMove.moveSpeed, 0, 0);
                if (item.transform.position.x < -CameraHalfWidth - 1)
                {
                    var lastPipe = pipeList[pipeList.Count - 1];
                    item.transform.position = lastPipe.transform.position + new Vector3(Distance, 0, 0);
                    item.transform.position = new Vector3(lastPipe.transform.position.x + Distance, Random.Range(-3f, 4.5f), 0);

                    pipeList.Remove(item);
                    pipeList.Add(item);
                    return;
                }

            }

        }

    }

}
