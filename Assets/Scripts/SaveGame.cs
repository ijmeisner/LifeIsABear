//
// Data structures and functions for saving and loading the game
//

using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// TODO:
// Learn a lot more about this so that I can actually do it right

public class SaveGame : MonoBehaviour {

	[Serializable]
	public struct playerPos
	{
		public float x;
		public float y;
		public float z;
	}

	[Serializable]
	public class GameData
	{
		public playerPos m_playerPosition {get; set;}
		public TerrainControl.chunkData[] m_terrainList {get; set;}
	}

	public static playerPos playerPosition;
	public static TerrainControl.chunkData[] terrainList;

	// ---
	// --------------------
	// ---

	public static void save()
	{
		GameObject player = GameObject.Find("Player");

		terrainList = TerrainControl.terrainList;
		BinaryFormatter bf = new BinaryFormatter();
		GameData data = new GameData();

		Debug.Log(Application.persistentDataPath);
		FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat"); // or whatever file name

		// DATA TO BE SAVED:
		playerPosition.x = player.transform.position.x;
		playerPosition.y = player.transform.position.y;
		playerPosition.z = player.transform.position.z;
		data.m_playerPosition = playerPosition;

		data.m_terrainList = terrainList;
		// ----------

		bf.Serialize(file, data);
		file.Close();

		Debug.Log("Data Saved, hopefully");
	}

	public static void load()
	{
		if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
			GameObject player = GameObject.Find("Player");

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			GameData data  = (GameData)bf.Deserialize(file); // check casting

			// DATA TO BE LOADED:
			player.transform.position = new Vector3(data.m_playerPosition.x,
			                                        data.m_playerPosition.y,
			                                        data.m_playerPosition.z);

			TerrainControl.terrainList = data.m_terrainList;
			// ----------

			file.Close();
			Debug.Log("Successfully Loaded Save Data, I think");
		}
		else
		{
			Debug.Log("No saved data found");
		}
	}
}
