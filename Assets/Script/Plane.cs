using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Plane : MonoBehaviour
{
    float timer = 0f;        //设置加数
    float interval = 0.2f;   //循环一次的时间
    float speed = 1.5f;      //加的速度
   
    public GameObject FoodPrefab;//食物预制体
    public GameObject body;//身体预设体
    List<GameObject> snakeBody = new List<GameObject>();//一定记住设对象数组的格式
    GameObject canv;
   // GameObject Food;



    void Start()
    {
        canv= GameObject.Find("Canvas");
      //  Food= GameObject.Find("蛇头");
        for (int i = 0; i < 3; ++i)
        {
            Vector3 SnakeHeadPos = transform.position;
            SnakeHeadPos.z = 0;
           // Debug.Log("SnakeHeadPos:"+SnakeHeadPos);
            GameObject newbodynext = Instantiate<GameObject>(body,
            SnakeHeadPos - (i + 1) * new Vector3(0, 40f, 0f),/*40f是因为蛇头中心点与蛇身中心点的距离为40f*/ Quaternion.identity);

           // Debug.Log("蛇身："+newbodynext.transform.position);
            newbodynext.transform.SetParent(canv.transform, false);//再将它设为canvas的子物体
            snakeBody.Add(newbodynext);
        }

        CreateFood();
        CreateFood();


    }

    void Update()
    {
        
        timer += speed * Time.deltaTime;
        if (timer > interval)
        {
            timer = 0;
            Vector3 tmpPosition = transform.position;    //记录头部变化前的位置
            List<Vector3> tmpList = new List<Vector3>(); //记录身体变化前的位置 


            for (int i = 0; i < snakeBody.Count; ++i)
            {
                tmpList.Add(snakeBody[i].transform.position);
            }

            transform.Translate(0, 0.5f, 0);//定时移动一定的距离（蛇头中心点到蛇身中心点的距离）

            snakeBody[0].transform.position = tmpPosition;//将0移到头部之前的位置

            for (int i = 1; i < snakeBody.Count; ++i)//依次前移身体的位置
            {
                snakeBody[i].transform.position = tmpList[i - 1];
            }
        }

        //从中心圆获取Rotation的z值
        float thlta_z = GameObject.Find("中心圆").GetComponent<CenterCircle>().thlta;
        //Debug.Log("转角："+thlta_z);
        transform.rotation = Quaternion.Euler(0, 0, thlta_z);

    }
    void CreateFood()
    {
        float x = Random.Range(-500f, 500f);
        float y = Random.Range(-260f, 260f);
        float z = 0;
     
        GameObject food=Instantiate<GameObject>(FoodPrefab, new Vector3(x, y, z), Quaternion.identity);
        food.transform.SetParent(canv.transform, false);//再将它设为canvas的子物体
        Debug.Log("位置：" + food.transform.position + "父节点：" + food.transform.parent);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Food"))
        {
            Destroy(collision.gameObject);
            GameObject newbodynext = Instantiate<GameObject>(body,
            snakeBody[snakeBody.Count - 1].transform.position,
            Quaternion.identity);
            newbodynext.transform.SetParent(canv.transform, false);//再将它设为canvas的子物体
            snakeBody.Add(newbodynext);
            CreateFood();
        }
        else if (collision.tag.Equals("Wall"))
        {
            Debug.Log("游戏结束");
        }

    }

}

