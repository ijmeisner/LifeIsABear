using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveGame : MonoBehaviour {

	public static object player;

	public static void save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		PlayerData data = new PlayerData();

		Debug.Log(Application.persistentDataPath);
		FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat"); // or whatever file name

		// DATA TO BE SAVED:
		data.m_player = player;
		// ----------

		bf.Serialize(file, data);
		file.Close();

		Debug.Log("Data Saved");
	}

	public static void load()
	{
		if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			PlayerData data  = (PlayerData)bf.Deserialize(file); // check casting

			// DATA TO BE LOADED:
			// TODO assign player data
			// ----------

			file.Close();
			Debug.Log("Successfully Loaded Save Data");
		}
		else
		{
			Debug.Log("No saved data found");
		}
	}

	[Serializable]
	class PlayerData
	{
		public object m_player {get; set;}
		// public object m_terrain {get; set;}
	}
}
