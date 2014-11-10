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
	int            i;                     // Temporary variable

	public PathGraph()
	{
	}
	private void GetVerts (Terrain sceneTerrain) // private for now
	{
		// Loops for getting the y-component of each vertex for path graph
		for ( int i = (int)localOrigin.x; i <= xMax; i += spaceIncrement)
		{
			for( int k = (int)localOrigin.y; k <= zMax; k += spaceIncrement)
			{
				vertices[i,k] = sceneTerrain.terrainData.GetHeight((int)i,(int)k);
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
			if( index%(zSize*2) == 0 && index != (xMax-1)*zMax*2 ) {// bottom edge
				cells[index].SetEdge1 ( index+1 );
				cells[index].SetEdge2 ( index+((2*zMax)+1) );
			}
			else if(index%(zSize*2) == (zSize*2)-1 && index != (zMax*2-1)) {// top edge
				cells[index].SetEdge1 ( index-1 );
				cells[index].SetEdge2 ( index-((2*zMax)+1) );
			}
			else if( index >= 0 && index < (zMax*2-1) && index%2 == 1 || (index > (xMax-1)*zMax*2) && index%2 == 0 ) {// Left of Right Edge (Not including top left/bottom right corners because they are special cases
				cells[index].SetEdge1 ( index+1 );
				cells[index].SetEdge2 ( index-1 );
			}
			else if( index == (zMax*2-1) ) {// Top left corner (1 edge)
				cells[index].SetEdge1 ( index-1 );
			}
			else if( index == ((xMax-1)*zMax*2) ) {// Bottom right corner(1 edge)
				cells[index].SetEdge1 ( index+1 );
			}
			else{
				cells[index].SetEdge1 ( index+1 );
				cells[index].SetEdge2 ( index-1 );
				if( index%2 == 0 ){
					cells[index].SetEdge3 ( index+((2*zMax)+1) );
				}
				else{
					cells[index].SetEdge3 ( index-((2*zMax)+1) );
				}
			}
			index ++;
		}
	}
	
	public CellNode Astar( Vector3 currentPos, Vector3 targetPos )
	{
		int[] path;
		CellNode current = CurrentCell (currentPos);
		int[] edgeIndices = new int[3];
		edgeIndices = current.Edges();
		float heuristic1 = -2.0f; // impossible values to start
		float heuristic2 = -2.0f;
		float heuristic3 = -2.0f;
		heuristic1 =  cells[edgeIndices[0]].Heuristic ( targetPos );
		if ( edgeIndices[1] > -1 )
		{
			heuristic2 = cells[edgeIndices[1]].Heuristic (targetPos);
		}
		if ( edgeIndices[2] > -1 )
		{
			heuristic3 = cells[edgeIndices[2]].Heuristic ( targetPos );
		}
		return new CellNode (); // *****place holder to stop compiler errors

	}

	// Use this for initialization
	void Start ()
	{
		xSize = 25; // Set this variable to be whatever works best for our game.
		zSize = 25; // Same as above.
		xMax = (int)localOrigin.x + xSize;
		zMax = (int)localOrigin.y + zSize;

		spaceIncrement = 1; // GetHeight() function in unity terrainData takes ints as arguments
		spaceIncrementInverse = ((float)1)/spaceIncrement;
		sceneTerrain = Terrain.activeTerrain;
		vertices = new float[(int)(xMax*spaceIncrementInverse+1),(int)(zMax*spaceIncrementInverse+1)];
	}
	
	// Update is called once per frame
	void Update () {
		if( i == 1 ) // psuedo-code:
		{
			GetVerts ( sceneTerrain);
			cells = new CellNode[(int)(xMax*spaceIncrementInverse)*(int)(zMax*spaceIncrementInverse)*2]; 
			MakeCells ();
			MakeEdges ();
			i = 0;
		}
	
	}
	CellNode CurrentCell( Vector3 position)
	{
		float x = position.x - localOrigin.x;
		float z = position.z - localOrigin.y;
		int i = (int)x;
		int k = (int)z;
		x -= i;
		z -= k; 

		int index;
		if (z / x >= 1)
		{
			index = ((i * (zMax - 1) + k) * 2);
		}
		else
		{
			index = ((i * (zMax - 1) + k) * 2)+1;
		}
		return cells[index];
	}
	// TODO: EVERYTHING BELOW THIS

	CellNode CurrentCell( CellNode lastCell)
	{
		return new CellNode();
	}
}
