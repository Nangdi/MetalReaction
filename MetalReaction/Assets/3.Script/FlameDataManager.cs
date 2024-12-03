using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class JsonData
{
    public Vector3 flamePos;
    public Vector3 reactionFlamePos;
    public float spacing= 0.5f;
}
public class FlameDataManager : MonoBehaviour
{
   public static FlameDataManager Instance;
    [SerializeField]
    FlameMovement flameMovement;
    [SerializeField]
    ReactionFlameMovement reactionFlameMovement;


    private string filePath;
    public JsonData data;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        filePath = Path.Combine(Application.persistentDataPath, "gameData.json");
        LoadData();

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
        data.flamePos = flameMovement.transform.position;
        data.spacing = reactionFlameMovement.spacing;
        data.reactionFlamePos = reactionFlameMovement.transform.transform.localPosition ;
        SaveData();
    }
}
