using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveAlongPath : MonoBehaviour
{
    int moveIndex = 0;
    public List<Vector3> moveDestinations;

    // Update is called once per frame
    void Update()
    {
        

        if (Vector3.Distance(transform.position, moveDestinations[moveIndex]) <= 0.001f)
        {
            moveIndex++;
            if (moveIndex >= moveDestinations.Count) Destroy(gameObject);
            MoveToExample(moveIndex);
            Vector3 dir = moveDestinations[moveIndex] - transform.position;
            if(dir != new Vector3(0,0,0))
            {
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                // Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime).eulerAngles;
                // 动作太快用平滑旋转反而会出现漂移现象
                Vector3 rotation = lookRotation.eulerAngles;
                transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            }            
        }
              
        
    }

    void MoveToExample(int index)
    {       
        iTween.MoveTo(this.gameObject, iTween.Hash("position", moveDestinations[index], "time", Time.deltaTime*10, "easetype", iTween.EaseType.linear));
    }
}
