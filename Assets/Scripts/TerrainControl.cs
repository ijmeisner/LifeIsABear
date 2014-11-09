
// Manages Dynamic Terrain Loading and Unloading
// *Terrain is not actually unloaded, only disabled, system is currently too basic to unload without losing the data

using UnityEngine;
using System.Collections;

public class TerrainControl : MonoBehaviour {

	// TODO:
	// figure out how active terrains works and possible use it
	// cmopletely redo memory structure so that it makes more sense without being huge
	// make sure stuff on disabled terrain gets disabled first (easiest way to make them child of it)
	// add random terrain prefab selection once we have prefabs and know this works
	// adjust to have initial terrain be instantiated also

	// potential for slowdown and/or high memory use when map gets generated really big
	// (at least in similar but more extreme cases of dynamic terrain, don't know how much of impact in this game):
	//   consider storing megachunks in files and only keeping one megachunk of data in the game memory
	//   but will have to keep several megachunks loaded so that it is seamless moving between them

	public struct chunkData
	{
		public Terrain data;
		public int x,z;
	}

	// Public:
	public Camera playerCamera;
	public Terrain[] terrainPrefabs;
	public Transform environment; // so terrains are set as childs of it
	// -

	// Public Static:
	public static chunkData[] terrainList;
	// -

	// Private
	private int m_terrainSize;
	private float m_farClip;
	private float m_loadRange; // make at least as big as farClip
	private float m_unloadRange; // make at least as big as farclip
	private int m_xPlayerChunk, m_zPlayerChunk;
	private Transform m_cameraTransform;

	private int i;
	// -
	
	void Awake()
	{
		m_farClip = playerCamera.farClipPlane;
		m_loadRange = m_farClip + 1000;
		m_unloadRange = m_farClip + 2500;
		m_terrainSize = 2000; // distance along a side, coords should be in center
		m_xPlayerChunk = 0;
		m_zPlayerChunk = 0;

		m_cameraTransform = playerCamera.transform;

		terrainList = new chunkData[512];

		loadTerrains();
		StartCoroutine(checkTerrain());
	}

	/*
	void Update()
	{
	
	}
	*/

	// load terrain when it comes into range
	void loadTerrains() // TODO make more efficient, in desperate need of it, also doesn't use m_loadRange
	{
		// holds if chunks around yours are loaded (so only search once), 0 starts at top, then clockwise
		bool[] chunksLoaded = new bool[] {false, false, false, false, false, false, false, false};

		// get values for bool chunksLoaded
		for(i=0; i<512 && terrainList[i].data!=null; i++)
		{
			if(terrainList[i].x==m_xPlayerChunk && terrainList[i].z==m_zPlayerChunk+1)
			{
				chunksLoaded[0] = true;
			}
			if(terrainList[i].x==m_xPlayerChunk+1 && terrainList[i].z==m_zPlayerChunk+1)
			{
				chunksLoaded[1] = true;
			}
			if(terrainList[i].x==m_xPlayerChunk+1 && terrainList[i].z==m_zPlayerChunk)
			{
				chunksLoaded[2] = true;
			}
			if(terrainList[i].x==m_xPlayerChunk+1 && terrainList[i].z==m_zPlayerChunk-1)
			{
				chunksLoaded[3] = true;
			}
			if(terrainList[i].x==m_xPlayerChunk && terrainList[i].z==m_zPlayerChunk-1)
			{
				chunksLoaded[4] = true;
			}
			if(terrainList[i].x==m_xPlayerChunk-1 && terrainList[i].z==m_zPlayerChunk-1)
			{
				chunksLoaded[5] = true;
			}
			if(terrainList[i].x==m_xPlayerChunk-1 && terrainList[i].z==m_zPlayerChunk)
			{
				chunksLoaded[6] = true;
			}
			if(terrainList[i].x==m_xPlayerChunk-1 && terrainList[i].z==m_zPlayerChunk+1)
			{
				chunksLoaded[7] = true;
			}
		}

		// take edges not existing and instantiate then add to list
		if(i<512)
		{
			if(!chunksLoaded[0])
			{
				terrainList[i].x = m_xPlayerChunk;
				terrainList[i].z = m_zPlayerChunk+1;
				terrainList[i].data = Instantiate(terrainPrefabs[0],
				                                  new Vector3(terrainList[i].x*m_terrainSize-1000,
				            								  0,
				            								  terrainList[i].z*m_terrainSize-1000),
				                                  Quaternion.identity) as Terrain;

				terrainList[i].data.transform.parent = environment;
				i++;
			}
			if(!chunksLoaded[1])
			{
				terrainList[i].x = m_xPlayerChunk+1;
				terrainList[i].z = m_zPlayerChunk+1;
				terrainList[i].data = Instantiate(terrainPrefabs[0],
				                                  new Vector3(terrainList[i].x*m_terrainSize-1000,
				            								0,
				            								terrainList[i].z*m_terrainSize-1000),
				                                  Quaternion.identity) as Terrain;
				terrainList[i].data.transform.parent = environment;
				i++;
			}
			if(!chunksLoaded[2])
			{
				terrainList[i].x = m_xPlayerChunk+1;
				terrainList[i].z = m_zPlayerChunk;
				terrainList[i].data = Instantiate(terrainPrefabs[0],
				                                  new Vector3(terrainList[i].x*m_terrainSize-1000,
				            								0,
				            								terrainList[i].z*m_terrainSize-1000),
				                                  Quaternion.identity) as Terrain;
				terrainList[i].data.transform.parent = environment;
				i++;
			}
			if(!chunksLoaded[3])
			{
				terrainList[i].x = m_xPlayerChunk+1;
				terrainList[i].z = m_zPlayerChunk-1;
				terrainList[i].data = Instantiate(terrainPrefabs[0],
				                                  new Vector3(terrainList[i].x*m_terrainSize-1000,
				            								0,
				            								terrainList[i].z*m_terrainSize-1000),
				                                  Quaternion.identity) as Terrain;
				terrainList[i].data.transform.parent = environment;
				i++;
			}
			if(!chunksLoaded[4])
			{
				terrainList[i].x = m_xPlayerChunk;
				terrainList[i].z = m_zPlayerChunk-1;
				terrainList[i].data = Instantiate(terrainPrefabs[0],
				                                  new Vector3(terrainList[i].x*m_terrainSize-1000,
				            								0,
				            								terrainList[i].z*m_terrainSize-1000),
				                                  Quaternion.identity) as Terrain;
				terrainList[i].data.transform.parent = environment;
				i++;
			}
			if(!chunksLoaded[5])
			{
				terrainList[i].x = m_xPlayerChunk-1;
				terrainList[i].z = m_zPlayerChunk-1;
				terrainList[i].data = Instantiate(terrainPrefabs[0],
				                                  new Vector3(terrainList[i].x*m_terrainSize-1000,
				            								0,
				            								terrainList[i].z*m_terrainSize-1000),
				                                  Quaternion.identity) as Terrain;
				terrainList[i].data.transform.parent = environment;
				i++;
			}
			if(!chunksLoaded[6])
			{
				terrainList[i].x = m_xPlayerChunk-1;
				terrainList[i].z = m_zPlayerChunk;
				terrainList[i].data = Instantiate(terrainPrefabs[0],
				                                  new Vector3(terrainList[i].x*m_terrainSize-1000,
				            								0,
				            								terrainList[i].z*m_terrainSize-1000),
				                                  Quaternion.identity) as Terrain;
				terrainList[i].data.transform.parent = environment;
				i++;
			}
			if(!chunksLoaded[7])
			{
				terrainList[i].x = m_xPlayerChunk-1;
				terrainList[i].z = m_zPlayerChunk+1;
				terrainList[i].data = Instantiate(terrainPrefabs[0],
				                                  new Vector3(terrainList[i].x*m_terrainSize-1000,
				            								0,
				            								terrainList[i].z*m_terrainSize-1000),
				                                  Quaternion.identity) as Terrain;
				terrainList[i].data.transform.parent = environment;
				i++;
			}
		}




		for(i=0; i<512 && terrainList[i].data!=null; i++)
		{
			setChunkNeighbors(terrainList[i]);
		}
	}

	// unload terrain when it gets far away
	void unloadTerrains() // TODO make it actually unload the data and not just disable, not needed right now
	{
		for(i=0; i<512 && terrainList[i].data!=null; i++)
		{
			if(terrainList[i].x*m_terrainSize > (m_cameraTransform.position.x+m_unloadRange)
			   || terrainList[i].x*m_terrainSize < (m_cameraTransform.position.x-m_unloadRange)
			   || terrainList[i].z*m_terrainSize > (m_cameraTransform.position.z+m_unloadRange)
			   || terrainList[i].z*m_terrainSize < (m_cameraTransform.position.z-m_unloadRange)
			   && terrainList[i].data.gameObject.activeSelf)
			{
				terrainList[i].data.gameObject.SetActive(false);
				Debug.Log("Something Unloaded");
			}
		}
	}

	// set the neighbors of a chunk
	void setChunkNeighbors(chunkData chunk) // TODO this needs to be more efficient
	{
		Terrain left, right, up, down;
		left = new Terrain(); right = new Terrain(); up = new Terrain(); down = new Terrain();
		for(i=0; i<512 && terrainList[i].data!=null; i++)
		{
			if(terrainList[i].x==chunk.x+1 && terrainList[i].z==chunk.z)
			{
				right = terrainList[i].data;
			}
			if(terrainList[i].x==chunk.x-1 && terrainList[i].z==chunk.z)
			{
				left = terrainList[i].data;
			}
			if(terrainList[i].x==chunk.x && terrainList[i].z==chunk.z+1)
			{
				up = terrainList[i].data;
			}
			if(terrainList[i].x==chunk.x && terrainList[i].z==chunk.z-1)
			{
				down = terrainList[i].data;
			}
		}
		if(left!=null && up!=null && right!=null && down!=null)
		{
			chunk.data.SetNeighbors(left, up, right, down);
		}
	}

	// Check if need to load or unload
	IEnumerator checkTerrain()
	{
		int xNew, zNew;
		while(true)
		{
			// don't need to do anything if player is in same chunk
			xNew = ((int)(m_cameraTransform.position.x + (m_terrainSize/2.0f)))/m_terrainSize;
			zNew = ((int)(m_cameraTransform.position.z + (m_terrainSize/2.0f)))/m_terrainSize;
			while(xNew==m_xPlayerChunk && zNew==m_zPlayerChunk)
			{
				yield return new WaitForSeconds(5);
				xNew = ((int)(m_cameraTransform.position.x + (m_terrainSize/2.0f)))/m_terrainSize;
				zNew = ((int)(m_cameraTransform.position.z + (m_terrainSize/2.0f)))/m_terrainSize;
			}

			// store new player chunk
			m_xPlayerChunk = xNew;
			m_zPlayerChunk = zNew;

			// Load terrain that needs loaded and unload those that don't need to be anymore
			loadTerrains();
			unloadTerrains();

			yield return new WaitForSeconds(5);
		}
	}
}
