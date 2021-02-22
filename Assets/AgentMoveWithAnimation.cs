using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMoveWithAnimation : MonoBehaviour
{
    // 路径索引和目标数组
    public int moveIndex = 0;
    public List<Vector3> moveDestinations;
    float turnSpeed = 10f;
    public float distance;

    Animator animator;
    int velocity = Animator.StringToHash("Velocity");
    
    // 速度和移动到下一点所需的时间
    public float speed = 0f;
    float moveTime = 0.05f;   
    public Vector3 originalPosition;

    public int ID;
    public int startFrame;
    public int lifeTime;
    public bool active;

    

    private void Start()
    {
        animator = GetComponent<Animator>();
        transform.position = originalPosition;       
    }

    // Update is called once per frame
    void Update()
    {
        // 走完就销毁人物
        if (moveIndex >= moveDestinations.Count - 1) gameObject.SetActive(false);

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


        distance = Vector3.Distance(transform.position, moveDestinations[moveIndex]);
        // 如果人物和目标距离很近，则换到下一个目标
        if (distance <= 0.1f)
        {              
            if(moveIndex < moveDestinations.Count)
                moveIndex++;                     
        }


        if (Vector3.Distance(transform.position, moveDestinations[moveIndex]) > 10f)
        {
            transform.position = moveDestinations[moveIndex];
            moveIndex++;
            return;
        }


        // 人物速度和距离的关系
        if (moveIndex == 0)
        {
            speed = Vector3.Distance(moveDestinations[0],originalPosition) / moveTime;
        }
        else
        {
            speed = Vector3.Distance(moveDestinations[moveIndex], moveDestinations[moveIndex - 1]) / moveTime;
        }

        if (speed >= 6f) speed = 6f;
        animator.SetFloat(velocity, speed);
        MoveToExample(moveIndex);

    }

    void MoveToExample(int index)
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", moveDestinations[index], "time", moveTime, "easetype", iTween.EaseType.linear));
    }
}
