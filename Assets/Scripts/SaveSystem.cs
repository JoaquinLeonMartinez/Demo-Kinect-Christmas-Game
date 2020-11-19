using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


public static class SaveSystem 
{

    /*public static void SaveData()
    {
        //BinaryFormatter formatter = new BinaryFormatter(); //Cambiar por un json
        string path = Application.persistentDataPath + "/jsonFiles/Data.json";

        //FileStream stream = new FileStream(path, FileMode.Create); //en realidad no estoy muy seguro de que queramos crear el fichero aqui 

        GameData data = new GameData();

        //LE DAMOS EL FORMATO DE JSON:
        string gameDataJson = JsonUtility.ToJson(data);

        File.WriteAllText(path, gameDataJson);
        //formatter.Serialize(stream, data);
        //stream.Close();
    }*/
    /*
    public static void UpdateGameData()
    {
        string path = Application.dataPath + "/jsonFiles";

        if (!File.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path = path + "/Data.json";

        GameData data = new GameData();
        string gameDataJson = JsonUtility.ToJson(data);
        //File.WriteAllText(path, gameDataJson);

        using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(path))
        {
            file.WriteLine(gameDataJson);
        }
    }
    
    public static void SaveDataScore()
    {
        string path = Application.dataPath + "/jsonFiles";

        if (!File.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path = path + "/Score.json";

        ScoreData data = new ScoreData();
        string gameDataJson = JsonUtility.ToJson(data);
        using (System.IO.StreamWriter file =
        new System.IO.StreamWriter(path, true))
        {
            file.WriteLine(gameDataJson);
        }
    }

    public static GameData LoadData()
    {
        string path = Application.dataPath + "/jsonFiles";

        if (!File.Exists(path))
        {
            //Directory.CreateDirectory(path);
            //UpdateGameData(); //Metemos informacion en el fichero aunque sea la que hay por defecto (asegurarnos de que hay informacion)
            path = path + "/Data.json";
            //BinaryFormatter formatter = new BinaryFormatter();
            string fileDataJson = File.ReadAllText(path);
            //FileStream stream = new FileStream(path, FileMode.Open);
            GameData data = JsonUtility.FromJson<GameData>(fileDataJson);
            //GameData data = formatter.Deserialize(stream) as GameData;
            //stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("No existe el fichero en la ruta: " + path);
            return null;
        }


    }
    */

    /*
    public static void SaveDataPrices(int leftSmall, int leftMedium, int leftBig, System.DateTime lastCheck)
    {
        //LastCheck = 19/11/2020 0:00:00
        PlayerPrefs.SetInt("leftSmallPrices", leftSmall);
        PlayerPrefs.SetInt("leftMediumPrices", leftMedium);
        PlayerPrefs.SetInt("leftBigPrices", leftBig);
        PlayerPrefs.SetInt("lastCheckDay", lastCheck.Day);
        PlayerPrefs.SetInt("lastCheckMonth", lastCheck.Month);
        PlayerPrefs.SetInt("lastCheckDayYear", lastCheck.Year);
        //PlayerPrefs.Save();
    }
    */
}
