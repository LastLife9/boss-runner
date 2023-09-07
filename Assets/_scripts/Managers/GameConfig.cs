using System.IO;
using UnityEngine;

public class GameConfig : Singleton<GameConfig>
{
    [field: SerializeField] public GameParameters GameParameters { get; private set; }

    private string jsonFilePath;

    protected override void Awake()
    {
        base.Awake();
        jsonFilePath = Path.Combine(Application.persistentDataPath, "config.json");
        LoadParameters();
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        SaveParameters();
    }

    public void SaveParameters()
    {
        string json = JsonUtility.ToJson(GameParameters);
        File.WriteAllText(jsonFilePath, json);
    }

    public void LoadParameters()
    {
        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
            GameParameters = JsonUtility.FromJson<GameParameters>(json);
        }
        else
        {
            SaveParameters();
        }
    }
}
