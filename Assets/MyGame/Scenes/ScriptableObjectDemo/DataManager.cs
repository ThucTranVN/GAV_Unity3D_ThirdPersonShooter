using System.IO;
using TPS;
using UnityEngine;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    public DataSO PlayerData;
    private string dataFilePath;

    public override void OnInited()
    {
        base.OnInited();
        dataFilePath = Application.persistentDataPath + "/playerdata.json";
        Debug.Log(dataFilePath);
        this.LoadPlayerData();
    }

    private void WritePlayerDataSO()
    {
        string toJson = JsonUtility.ToJson(PlayerData);
        File.WriteAllText(dataFilePath, toJson);
    }

    private string ReadPlayerDataSO()
    {
        if (File.Exists(dataFilePath))
        {
            return File.ReadAllText(dataFilePath);
        }
        return null;
    }

    public void LoadPlayerData()
    {
        string fromJson = ReadPlayerDataSO();
        if(fromJson == null)
        {
            WritePlayerDataSO();
            fromJson = ReadPlayerDataSO();
        }
        JsonUtility.FromJsonOverwrite(fromJson, PlayerData);
    }

    public void SavePlayerData()
    {
        WritePlayerDataSO();
    }
}
