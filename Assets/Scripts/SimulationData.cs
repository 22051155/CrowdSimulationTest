using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SimulationData : MonoBehaviour
{
    string[] positionList;    
    string fileName;
    string filePath;

    public GameObject[] prefab;

    public List<GameObject> objectPool;
       
    float coordinateTransformCoefficient = 3f;      
     
    // Start is called before the first frame update
    void Start()
    {
        fileName = "street.txt";
        filePath = Application.dataPath + "/" + fileName;
        ReadFromFile();
        SpawnAgents();
    }

    // 去读文件，并分配给agent相应每一帧的坐标
    void ReadFromFile()
    {       
        positionList = File.ReadAllLines(filePath);

        List<Vector3> positionListForOneAgent = new List<Vector3>();

        

        // 生成的人物样式（1-6种）
        int i = 0;

        foreach (string line in positionList)
        {
            string[] data = line.Split(' ');

            if (data.Length == 1)
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

            if (data.Length > 1)
            {                
                string[] pos = line.Split(' ');
                if (pos.Length >= 2)
                {
                    float x = float.Parse(pos[0]) * coordinateTransformCoefficient;
                    float y = float.Parse(pos[1]) * coordinateTransformCoefficient;
                    Vector3 agentPosition = new Vector3(x, -0.5f, y);
                    positionListForOneAgent.Add(agentPosition);
                }
            }

        }
    }

    // 生成一个单位，并传给他要移动的坐标
    void SpawnAgents()
    {
        foreach (GameObject agent in objectPool)
        {
            agent.SetActive(true);
        }
    }
}
