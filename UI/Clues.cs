using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Clues : MonoBehaviour
{

    [SerializeField] private GameObject[] clueObject;

    [SerializeField] private ButtonFlashAnimator anim = null;

    private GameData gameData = null;

    private void Awake()
    {
        gameData = LoadFile();
        for (int i = 0; i < gameData.clues.Length; i++)
        {
            clueObject[i].gameObject.SetActive(gameData.clues[i]);
        }
    }

    private void OnEnable()
    {
        EventManager.onUnlockClue += UnlockClue;

    }

    private void OnDisable()
    {
        EventManager.onUnlockClue -= UnlockClue;

    }

    private void UnlockClue(int clueID)
    {
        gameData.clues[clueID] = true;
        clueObject[clueID].SetActive(true);
        anim?.StartAnimation();
        SaveFile();
    }

    public static void ResetClues()
    {
        DeleteSaveFile();
    }

    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination))
            file = File.OpenWrite(destination);
        else
            file = File.Create(destination);

        var data = gameData;

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public GameData LoadFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination))
            file = File.OpenRead(destination);
        else
        {
            Debug.Log("Save not found");
            return new GameData();
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData)bf.Deserialize(file);
        file.Close();

        return data;

    }

    public static void DeleteSaveFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";

        if (File.Exists(destination))
        {
            File.Delete(destination);
        }
    }
}

[Serializable]
public class GameData
{
    public bool[] clues;

    public GameData(bool[] clues)
    {
        this.clues = clues;
    }

    public GameData()
    {
        this.clues = new bool[4];
    }

}