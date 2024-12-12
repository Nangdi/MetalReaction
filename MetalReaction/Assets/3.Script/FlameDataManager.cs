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
    public JsonData jsondata;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        filePath = Path.Combine(Application.persistentDataPath, "gameData.json");
        LoadData();
        InvisbleMouseCursor();

    }
    public void SaveData()
    {
        string json = JsonUtility.ToJson(jsondata);
        File.WriteAllText(filePath, json);
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, jsondata);
        }
    }
    private void OnApplicationQuit()
    {
        jsondata.flamePos = flameMovement.transform.position;
        jsondata.spacing = reactionFlameMovement.spacing;
        jsondata.reactionFlamePos = reactionFlameMovement.transform.transform.localPosition ;
        SaveData();
    }
    private void InvisbleMouseCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
