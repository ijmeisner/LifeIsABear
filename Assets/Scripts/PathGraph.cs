using UnityEngine;
using System.Collections;

/* Current Implementation Issues: What about the possibility of a bridge? Currently,
   there would be no way of placing two CellNodes at the same x and z coords but
   different y's.
*/
public class PathGraph : MonoBehaviour
{
	Vector2        localOrigin;           // Origin that this is specified from in World Space.
	Terrain        sceneTerrain;          // The terrain of the scene of this path graph.
	float[,]       vertices;              // All the different shared corners of the different cells
	CellNode[]     cells;                 // The Graph starts at the bottom-left cell
	int            numVerts;              // Current Number of Verts.
	int            numCells;              // Current Number of Cells.
	int            xSize;                 // Size in x direction.
	int            zSize;                 // Size in z direction.
	int            xMax;                  // TODO: decide the x size of terrains
	int            zMax;                  // TODO: decide the z size of terrains
	int            spaceIncrement;        // The density of terrain points to use for pathgraph generation
	float          spaceIncrementInverse; // 1/spaceIncrement
	public static PathGraph activeGraph;  // Holds a handle to the active PathGraph

	public PathGraph()
	{
	}
	private void GetVerts (Terrain sceneTerrain) // private for now
	{
		// Loops for getting the y-component of each vertex for path graph
		for ( int i = 0; i <= xSize; i += spaceIncrement)
		{
			for( int k = 0; k <= zSize; k += spaceIncrement)
			{
				vertices[i,k] = sceneTerrain.terrainData.GetHeight((int)(i + localOrigin.x),(int)(k + localOrigin.y));
			}
		}
	}
	private void MakeCells() // private for now
	{
		for (int i = 0; i < xSize; i += spaceIncrement)
		{
			for( int k = 0; k < zSize; k += spaceIncrement)
			{
				int index = ((i*(zSize)+k)*2);
				Vector3 bottomLeft = new Vector3(i,k,vertices[i,k]);
				Vector3 topRight   = new Vector3(i+1,k+1,vertices[i+1,k+1]);
				cells[index] = new CellNode( bottomLeft, new Vector3(i+1,k,vertices[i+1,k]), topRight, index);
				index += 1;
				cells[index] = new CellNode( bottomLeft, new Vector3(i,k+1,vertices[i,k+1]), topRight, index);
			}
		}
	}

	void MakeEdges()
	{
		int index = 0;
		while( index < xSize*zSize*2 )
		{
			if( index%(zSize*2) == 0 && index != (xSize-1)*zSize*2 ) {// bottom edge
				cells[index].SetEdge1 ( index+1 );
				cells[index].SetEdge2 ( index+((2*zSize)+1) );
			}
			else if(index%(zSize*2) == (zSize*2)-1 && index != (zSize*2-1)) {// top edge
				cells[index].SetEdge1 ( index-1 );
				cells[index].SetEdge2 ( index-((2*zSize)+1) );
			}
			else if( index >= 0 && index < (zSize*2-1) && index%2 == 1 || (index > (xSize-1)*zSize*2) && index%2 == 0 ) {// Left of Right Edge (Not including top left/bottom right corners because they are special cases
				cells[index].SetEdge1 ( index+1 );
				cells[index].SetEdge2 ( index-1 );
			}
			else if( index == (zSize*2-1) ) {// Top left corner (1 edge)
				cells[index].SetEdge1 ( index-1 );
			}
			else if( index == ((xSize-1)*zSize*2) ) {// Bottom right corner(1 edge)
				cells[index].SetEdge1 ( index+1 );
			}
			else{
				cells[index].SetEdge1 ( index+1 );
				cells[index].SetEdge2 ( index-1 );
				if( index%2 == 0 ){
					cells[index].SetEdge3 ( index+((2*zSize)+1) );
				}
				else{
					cells[index].SetEdge3 ( index-((2*zSize)+1) );
				}
			}
			index ++;
		}
	}
	
	public int[] Astar( Vector3 currentPos, Vector3 targetPos ) // Causes crash currently
	{
		int numCells = (int)(xSize * spaceIncrementInverse * zSize * spaceIncrementInverse * 2); // number of cells.
		int openSetIndex = 0;
		int closedSetIndex = 0;
		int start = CurrentCell (currentPos).GetIndex();
		int current; // index of current Node in path algorithm
		int goal = CurrentCell ( targetPos ).GetIndex (); // Index of goal node
		int kickoutCounter = 0; // after too many loops, algorithm is halted ( for debugging so that log will work)

		int[] closedSet = new int[ numCells];
		int[] openSet   = new int[ numCells]; // set of indices of tentative nodes to be evaluated
		int[] cameFrom  = new int[ numCells ];      // each array entry holds the index of the previous node in the best path to it so far
		float[] gScore  = new float[ numCells ];    // NOTE: not sure how big this array needs to be so I played it safe; represents the distance of the path to this node
		float[] fScore  = new float[ numCells ];    // Same as above; represents the distance of the path to this node plus the heuristic to the goal

		for (int i = 0; i < numCells; i++) // Initializing loop
		{
			// -2 and -1.0f because they are invalid values
			openSet[i] = -2;
			closedSet[i] = -2;
			cameFrom[i] = -2;
			gScore[i] = 0.0f;
			fScore[i] = -2.0f;
		}

		openSet [openSetIndex] = start; // 1 indicates that the index node is in the set
		openSetIndex++;
		gScore [start] = 0;
		fScore [start] = gScore [start] + cells [start].Heuristic ( targetPos );

		current = start;
		bool isStart = true;
		bool isInClosedSet = false;
		bool isInOpenSet = false;
		while (openSet[0] > -1 && kickoutCounter < 50) // set isnt empty
		{

			kickoutCounter++;
			if( !isStart ) // start is special case and doesnt need to find lowest fScore
			{
				current = openSet[0];
				int currentOpenSetIndex = 0;
				for( int i = 1; i < openSetIndex; i++) // find lowest fScore
				{
					if( fScore[openSet[i]] < fScore[current] )
					{
						current = openSet[i];
						currentOpenSetIndex = i;
					}
				}
				openSetIndex--;
				openSet[ currentOpenSetIndex ] = openSet[ openSetIndex];
				openSet[ openSetIndex ] = -2;
			}
			if( current == goal)
			{
				return ReconstructPath ( cameFrom, goal );
			}
			closedSet[closedSetIndex] = current;
			closedSetIndex ++;
			int[] edgeIndices = new int[3];        // holds the neighbor verts cells indices of current
			edgeIndices = cells[current].Edges ();

			for ( int i = 0; i < 3 && edgeIndices[i] > -1 && kickoutCounter < 50; i++ )
			{
				float tentativeGScore = 0; // Temporary score that only matters if it is less than the current gscore for a node or the node had no previous gscore
				isInClosedSet = false; // is a neighbor already in the closedSet
				for( int j = 0; j < closedSetIndex && !isInClosedSet; j++)
				{
					if( closedSet[j] == edgeIndices[i])
					{
						isInClosedSet = true;
					}
				}
				tentativeGScore = gScore[ current ] + 1; // + distances between current and neighbor ( no distance the way that i have implemented it so this is irrelevant currently)
				if( isInClosedSet )
				{
				}
				isInOpenSet = false;
				for( int j = 0; j < openSetIndex && !isInOpenSet; j++)
				{
					if( openSet[j] == edgeIndices[i])
					{
						isInOpenSet = true;
					}
				}
				if( (!isInOpenSet && !isInClosedSet )|| tentativeGScore < gScore[edgeIndices[i]] )
				{
					if( isStart )
					{
						isStart = false;
					}
					cameFrom[ edgeIndices[i] ] = current;
					gScore[ edgeIndices[i] ] = tentativeGScore;
					fScore[ edgeIndices[i] ] = gScore[ edgeIndices[i] ] + cells[ edgeIndices[i] ].Heuristic ( targetPos );
					if( !isInOpenSet && !isInClosedSet )
					{
						openSet[openSetIndex] =  edgeIndices[i];
						openSetIndex ++;
					}
				}

			}
		}
		return null; // return failure

	}
	int[] ReconstructPath( int[] cameFrom, int currentNodeIndex )
	{
		int countIndex = currentNodeIndex;
		int totalNodes = 0; // total nodes along final path
		int[] path;
		if( cameFrom[countIndex] > -1)
		{
			while( cameFrom[countIndex] > -1 ) // structured in such a way as to leave the starting node off the path bc it is superfluous
			{
				countIndex = cameFrom[countIndex];
				totalNodes++;
			}
			path = new int[totalNodes];
			path [totalNodes - 1] = currentNodeIndex;
			for( countIndex = totalNodes-2; countIndex >= 0; countIndex--)
			{
				path[countIndex] = cameFrom[path[countIndex+1]];
			}
		}
		else
		{
			path = new int[1];
			path[0] = -3; // -3 is Error Code ( meaning the goal was where the agent started)
		}
		return path;

	}

	// Use this for initialization
	void Start ()
	{
		activeGraph = this;
		localOrigin = new Vector2 (PlayerController.playerPos.x - 0.5f * (float)xSize, PlayerController.playerPos.z - 0.5f * (float)zSize );
		xSize = 200; // Set this variable to be whatever works best for our game.
		zSize = 200; // Same as above.
		xMax = (int)localOrigin.x + xSize;
		zMax = (int)localOrigin.y + zSize;
		spaceIncrement = 1; // GetHeight() function in unity terrainData takes ints as arguments
		spaceIncrementInverse = ((float)1)/spaceIncrement;
		sceneTerrain = Terrain.activeTerrain;
		vertices = new float[(int)(xSize*spaceIncrementInverse+1),(int)(zSize*spaceIncrementInverse+1)];

		// Test below
		GetVerts ( sceneTerrain);
		cells = new CellNode[(int)(xSize*spaceIncrementInverse)*(int)(zSize*spaceIncrementInverse)*2]; 
		MakeCells ();
		MakeEdges ();
	}
	
	// Update is called once per frame
	void Update () { // define a circle for the player that once exited, a new PathGraph is made and upon completion becomes the activeGraph
	}
	CellNode CurrentCell( Vector3 position)
	{
		float x = position.x - localOrigin.x;
		float z = position.z - localOrigin.y;
		int i = (int)x;
		int k = (int)z;
		x -= i; // gets decimal value of x
		z -= k;  // gets decimal value of z

		int index;
		if (z / x >= 1)
		{
			index = ((i * zSize + k) * 2) + 1;
		}
		else
		{
			index = ((i * zSize + k) * 2);
		}
		return cells[index];
	}
	public CellNode CurrentCell( int index )
	{
		if (index > ((int)xSize * spaceIncrementInverse) * ((int)zSize * spaceIncrementInverse) * 2 - 1)
		{
			return null; // null for invalid index
		}
		else // Do nothing
		{
		}
		return cells [index];
	}
}
