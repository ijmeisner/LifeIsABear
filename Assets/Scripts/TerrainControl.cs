//
// Manages Dynamic Terrain Loading and Unloading
//

using UnityEngine;
using System.Collections;
using System;

public class TerrainControl : MonoBehaviour {
	
	// TODO:
	// figure out how active terrains works and possible use it
	// completely redo memory structure so that it makes more sense without being huge
	// make sure stuff on disabled terrain gets disabled first (easiest way to make them child of it) * It is currently being destroyed, so no problem yet
	// add random terrain prefab selection once we have prefabs and know this works
	// replace terrainData array with two-key dictionary for more efficiency?
	// rename chunksLoaded to chunksCreated
	// Fix instantiate object to be smarter about instantiated height
	
	// potential for slowdown and/or high memory use when map gets generated really big
	// (at least in similar but more extreme cases of dynamic terrain, don't know how much of impact in this game):
	//   consider storing megachunks in files and only keeping one megachunk of data in the game memory
	//   but will have to keep several megachunks loaded so that it is seamless moving between them
	
	// Unity has problems with Unity units going too high. Have player recentered at 0 with new terrain. This could work with mega chunk structure
	
	// Terrain is not actually unloaded, only disabled, system is currently too basic to unload without losing the data, this causes high memory usage
	
	[Serializable]
	public struct chunkData
	{
		public int data;
		public int x, z;
	}
	
	// Public:
	public Camera playerCamera;
	public Terrain[] terrainPrefabs;
	public GameObject[] objects;
	public Transform environment; // so terrains are set as childs of it
	// -
	
	// Public Static:
	public static chunkData[] terrainList;
	public static Terrain playerTerrain;
	// -
	
	// Private
	private Terrain[] terrainData;
	private GameObject[] terrainObjects;
	private int m_terrainSize;
	private float m_farClip;
	private float m_loadRange; // make at least as big as farClip
	private float m_unloadRange; // make at least as big as farclip
	private int m_xPlayerChunk, m_zPlayerChunk;
	
	private Transform m_cameraTransform;
	
	private int i;
	// private Terrain m_terrainRef;
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
		
		// initialize data
		terrainList = new chunkData[512];
		terrainObjects = new GameObject[512];
		// check if created already or not, if negative data index, has not (CHECK TO MAKE SURE NO NEG INDEX HAPPENS!)
		{
			for(i=0; i<512; i++)
			{
				terrainList[i].data = -1;
			}
		}
		terrainData = new Terrain[512];
	}
	
	void Start()
	{
		playerTerrain = createTerrain(0, m_xPlayerChunk, m_zPlayerChunk);
		loadTerrains();
		StartCoroutine(checkTerrain());
	}
	
	// ---
	// --------------------
	// ---
	
	// load terrain when it comes into range
	void loadTerrains() // TODO make more efficient, change to use m_loadRange
	{
		int j=0;
		
		// holds if chunks around yours are created (so only search once), 0 starts at top, then clockwise
		bool[] chunksLoaded = new bool[] {false, false, false, false, false, false, false, false};
		
		// get values for bool chunksLoaded, and enable if disabled and already created
		for(i=0; i<512 && terrainList[i].data>=0; i++)
		{
			if(terrainList[i].x==m_xPlayerChunk && terrainList[i].z==m_zPlayerChunk)
			{
				playerTerrain = terrainData[i];
			}
			else if(terrainList[i].x==m_xPlayerChunk && terrainList[i].z==m_zPlayerChunk+1)
			{
				chunksLoaded[0] = true;
				if(!terrainData[i].gameObject.activeSelf)
				{
					terrainData[i].gameObject.SetActive(true);
				}
			}
			else if(terrainList[i].x==m_xPlayerChunk+1 && terrainList[i].z==m_zPlayerChunk+1)
			{
				chunksLoaded[1] = true;
				if(!terrainData[i].gameObject.activeSelf)
				{
					terrainData[i].gameObject.SetActive(true);
				}
			}
			else if(terrainList[i].x==m_xPlayerChunk+1 && terrainList[i].z==m_zPlayerChunk)
			{
				chunksLoaded[2] = true;
				if(!terrainData[i].gameObject.activeSelf)
				{
					terrainData[i].gameObject.SetActive(true);
				}
			}
			else if(terrainList[i].x==m_xPlayerChunk+1 && terrainList[i].z==m_zPlayerChunk-1)
			{
				chunksLoaded[3] = true;
				if(!terrainData[i].gameObject.activeSelf)
				{
					terrainData[i].gameObject.SetActive(true);
				}
			}
			else if(terrainList[i].x==m_xPlayerChunk && terrainList[i].z==m_zPlayerChunk-1)
			{
				chunksLoaded[4] = true;
				if(!terrainData[i].gameObject.activeSelf)
				{
					terrainData[i].gameObject.SetActive(true);
				}
			}
			else if(terrainList[i].x==m_xPlayerChunk-1 && terrainList[i].z==m_zPlayerChunk-1)
			{
				chunksLoaded[5] = true;
				if(!terrainData[i].gameObject.activeSelf)
				{
					terrainData[i].gameObject.SetActive(true);
				}
			}
			else if(terrainList[i].x==m_xPlayerChunk-1 && terrainList[i].z==m_zPlayerChunk)
			{
				chunksLoaded[6] = true;
				if(!terrainData[i].gameObject.activeSelf)
				{
					terrainData[i].gameObject.SetActive(true);
				}
			}
			else if(terrainList[i].x==m_xPlayerChunk-1 && terrainList[i].z==m_zPlayerChunk+1)
			{
				chunksLoaded[7] = true;
				if(!terrainData[i].gameObject.activeSelf)
				{
					terrainData[i].gameObject.SetActive(true);
				}
			}
		}
		
		j=i;
		
		// create terrains around you (createTerrain function checks if already done)
		if(!chunksLoaded[0] && j<512)
		{
			createTerrain(j, m_xPlayerChunk, m_zPlayerChunk+1);
			j++;
		}
		if(!chunksLoaded[1] && j<512)
		{
			createTerrain(j, m_xPlayerChunk+1, m_zPlayerChunk+1);
			j++;
		}
		if(!chunksLoaded[2] && j<512)
		{
			createTerrain(j, m_xPlayerChunk+1, m_zPlayerChunk);
			j++;
		}
		if(!chunksLoaded[3] && j<512)
		{
			createTerrain(j, m_xPlayerChunk+1, m_zPlayerChunk-1);
			j++;
		}
		if(!chunksLoaded[4] && j<512)
		{
			createTerrain(j, m_xPlayerChunk, m_zPlayerChunk-1);
			j++;
		}
		if(!chunksLoaded[5] && j<512)
		{
			createTerrain(j, m_xPlayerChunk-1, m_zPlayerChunk-1);
			j++;
		}
		if(!chunksLoaded[6] && j<512)
		{
			createTerrain(j, m_xPlayerChunk-1, m_zPlayerChunk);
			j++;
		}
		if(!chunksLoaded[7] && j<512)
		{
			createTerrain(j, m_xPlayerChunk-1, m_zPlayerChunk+1);
			j++;
		}
		
		// now set the neighbors of terrains
		for(i=0; i<512 && terrainList[i].data>=0; i++)
		{
			setChunkNeighbors(terrainList[i], terrainData);
		}
		
		populateObjects();
	}
	
	// unload terrain when it gets far away
	void unloadTerrains() // TODO make it actually unload the data and not just disable, not needed right now
	{
		for(i=0; i<512 && terrainList[i].data>=0; i++)
		{
			if(terrainList[i].x*m_terrainSize > (m_cameraTransform.position.x+m_unloadRange)
			   || terrainList[i].x*m_terrainSize < (m_cameraTransform.position.x-m_unloadRange)
			   || terrainList[i].z*m_terrainSize > (m_cameraTransform.position.z+m_unloadRange)
			   || terrainList[i].z*m_terrainSize < (m_cameraTransform.position.z-m_unloadRange)
			   && terrainData[i].gameObject.activeSelf)
			{
				terrainData[i].gameObject.SetActive(false);
				Debug.Log("Some Terrain Unloaded");
			}
		}
	}
	
	// set the neighbors of a chunk
	void setChunkNeighbors(chunkData chunk, Terrain[] terrains) // TODO this needs to be more efficient, try dictionary
	{
		Terrain left, right, up, down;
		left = right = up = down = null;
		
		// left = new Terrain(); right = new Terrain(); up = new Terrain(); down = new Terrain();
		for(i=0; i<512 && terrainList[i].data>=0; i++)
		{
			if(terrainList[i].x==chunk.x+1 && terrainList[i].z==chunk.z)
			{
				right = terrainData[i];
			}
			else if(terrainList[i].x==chunk.x-1 && terrainList[i].z==chunk.z)
			{
				left = terrainData[i];
			}
			else if(terrainList[i].x==chunk.x && terrainList[i].z==chunk.z+1)
			{
				up = terrainData[i];
			}
			else if(terrainList[i].x==chunk.x && terrainList[i].z==chunk.z-1)
			{
				down = terrainData[i];
			}
			
			if(left!=null && up!=null && right!=null && down!=null)
			{
				terrainData[i].SetNeighbors(left, up, right, down);
			}
			left = right = up = down = null;
		}
	}
	
	// instantiate terrain and add to list
	Terrain createTerrain(int i, int x, int z)
	{
		Terrain terrainRef = null;
		terrainList[i].x = x;
		terrainList[i].z = z;
		if(terrainData[i]!=null)
		{
			return terrainData[i];
		}
		terrainList[i].data = UnityEngine.Random.Range(0, terrainPrefabs.Length);
		terrainRef = Instantiate(terrainPrefabs[terrainList[i].data],
		                         new Vector3(terrainList[i].x*m_terrainSize-(m_terrainSize/2),
		            						 0.0f,
		            						 terrainList[i].z*m_terrainSize-(m_terrainSize/2)),
		                         Quaternion.identity) as Terrain;
		
		terrainRef.transform.parent = environment;
		terrainData[i] = terrainRef;
		
		return terrainRef;
	}
	
	void populateObjects()
	{
		GameObject objectRef;
		
		// Delete old objects on terrain you aren't on anymore
		for(i=0; i<512; i++)
		{
			Destroy(terrainObjects[i]);
			terrainObjects[i] = null;
		}
		
		// Create random objects with random locations on the terrain
		for(i=0; i<512; i++)
		{
			objectRef = Instantiate(objects[UnityEngine.Random.Range(0, objects.Length)],
			                        new Vector3(m_xPlayerChunk*m_terrainSize+UnityEngine.Random.Range(-m_terrainSize,
			                                                                  						  m_terrainSize),
			            						0.5f,
			            						m_zPlayerChunk*m_terrainSize+UnityEngine.Random.Range(-m_terrainSize,
			                                                      									  m_terrainSize)),
			                        Quaternion.identity) as GameObject;
			terrainObjects[i] = objectRef;
		}
	}
	
	// ---
	// --------------------
	// ---
	
	// Check if need to load or unload
	IEnumerator checkTerrain()
	{
		int xNew, zNew;
		while(true)
		{
			// don't need to do anything if player is still in same chunk
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
