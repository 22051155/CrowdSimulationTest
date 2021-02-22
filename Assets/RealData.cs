using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RealData : MonoBehaviour
{   
    string[] positionList;
    string fileName;
    string filePath;
    
    float coordinateTransformcoefficient = 3f; // 坐标系放大比率

    public GameObject[] prefab;
    public List<GameObject> objectPool;
    

    public float timer = -2f;

    // Start is called before the first frame update
    void Start()
    {
        fileName = "realdata.txt";
        filePath = Application.dataPath + "/" + fileName;
        ReadFromFile();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        foreach (GameObject agent in objectPool)
        {
            if (!agent.GetComponent<AgentMoveWithAnimation>().active && agent.GetComponent<AgentMoveWithAnimation>().startFrame / 4 < timer)
            {
                // Debug.Log("生成第" + agent.GetComponent<AgentMoveWithAnimation>().ID + "号");
                agent.SetActive(true);
                agent.GetComponent<AgentMoveWithAnimation>().active = true;
            }
        }       
    }

    void ReadFromFile()
    {
        positionList = File.ReadAllLines(filePath);
        
        int i = 0;

        // 储存每帧位置信息
        List<Vector3> positionListForOneAgent = new List<Vector3>();        

        foreach (string line in positionList)
        {           
            string[] data = line.Split(' ');
            
            if(data.Length != 2)
            {                  
                if (positionListForOneAgent.Count > 0)
                {
                    prefab[i].GetComponent<AgentMoveWithAnimation>().moveDestinations = positionListForOneAgent;
                    prefab[i].GetComponent<AgentMoveWithAnimation>().originalPosition = positionListForOneAgent[0];
                    prefab[i].transform.position = positionListForOneAgent[0];
                    GameObject obj = Instantiate(prefab[i]);
                    obj.SetActive(false);
                    objectPool.Add(obj);                    
                    
                    positionListForOneAgent.Clear();
                    i++;
                    if (i > 5) i = 0;
                }

                prefab[i].GetComponent<AgentMoveWithAnimation>().ID = int.Parse(data[0]);
                prefab[i].GetComponent<AgentMoveWithAnimation>().lifeTime = int.Parse(data[1]);
                prefab[i].GetComponent<AgentMoveWithAnimation>().startFrame = int.Parse(data[2]);
                prefab[i].GetComponent<AgentMoveWithAnimation>().active = false;              
            }
            else
            {
                float x = float.Parse(data[0]) * coordinateTransformcoefficient;
                float y = float.Parse(data[1]) * coordinateTransformcoefficient;
                positionListForOneAgent.Add(new Vector3(x, -0.5f, y));               
            }

            
        }


    }
      
}
