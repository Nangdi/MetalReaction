using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
[System.Serializable]
public class JsonData
{
    public Vector3 flamePos;
}
public class MovePosManager : MonoBehaviour
{

    public float speed = 5.0f;  // �̵� �ӵ�
    private string filePath;
    public JsonData data;
    private void Awake
        ()
    {
        filePath = Path.Combine(Application.persistentDataPath, "gameData.json");
        LoadData();
       
    }
    private void Start()
    {
        transform.position = data.flamePos;
    }


    void Update()
    {
        // Ű �Է¿� ���� x, y ���� �̵��� ���
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float moveZ = Input.GetAxis("Mouse ScrollWheel") * speed * Time.deltaTime; 
        // ������Ʈ ��ġ �̵�
        transform.Translate(moveX, moveY, moveZ);
    }
    public void SaveData()
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, data);
        }
    }
    private void OnApplicationQuit()
    {
        data.flamePos = transform.position;
        SaveData();
    }
}
