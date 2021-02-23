using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMoveWithAnimation : MonoBehaviour
{
    [Header("Agent attributes")]
    public int ID; // id，现在没什么用
    public int startFrame; // 出生的帧数
    public int lifeTime; // 存在的帧数
    public float speed = 0f; // 速度，和动画挂钩
    public float turnSpeed = 10f; // 角色转向速度
    public bool active; // 判断角色是否已经在场景中

    [Header("Path data")]
    // 路径索引和目标数组
    public int moveIndex = 0;
    public List<Vector3> moveDestinations;   
    public float distance; // 当前位置和下一个目标点的距离
    public Vector3 originalPosition; // 出生位置
         

    private float timePerFrame = 0.1f;// 移动到下一点所需的时间，模拟数据用0.1，真实数据用0.25比较合适

    Animator animator;
    int velocity = Animator.StringToHash("Velocity");

    private void Start()
    {
        animator = GetComponent<Animator>();
        transform.position = originalPosition;       
    }

    // Update is called once per frame
    void Update()
    {
        // 走完就销毁人物
        if (moveIndex >= moveDestinations.Count - 1)
        {
            gameObject.SetActive(false);
            return;
        } 

        // 旋转人物
        if (moveIndex < moveDestinations.Count)
        {
            Vector3 dir = moveDestinations[moveIndex] - transform.position;
            if (dir != Vector3.zero && dir!=transform.rotation.eulerAngles)
            {
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, turnSpeed).eulerAngles;
                transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            }
        }

        // 当前位置和下一个目标点位置的距离
        distance = Vector3.Distance(transform.position, moveDestinations[moveIndex]);

        // 如果人物和目标距离很近，则换到下一个目标
        if (distance <= 0.1f)
        {              
            if(moveIndex < moveDestinations.Count)
                moveIndex++;
            return;
        }

        // 用于模拟数据，距离大于10时说明人物要重生，直接移动坐标
        if (distance > 10f)
        {
            transform.position = moveDestinations[moveIndex];
            moveIndex++;
            return;
        }

        // 人物速度计算
        if (moveIndex == 0)
        {
            speed = Vector3.Distance(moveDestinations[0],originalPosition) / timePerFrame;
        }
        else
        {
            speed = Vector3.Distance(moveDestinations[moveIndex], moveDestinations[moveIndex - 1]) / timePerFrame;
        }

        if (speed >= 6f) speed = 6f;
        animator.SetFloat(velocity, speed);

        // 移动       
        MoveToExample(moveIndex);

    }

    void MoveToExample(int index)
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", moveDestinations[index], "time", timePerFrame, "easetype", iTween.EaseType.linear));
    }
}
