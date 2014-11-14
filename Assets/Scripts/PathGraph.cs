using UnityEngine;
using System.Collections;

/* Current Implementation Issues: What about the possibility of a bridge? Currently,
   there would be no way of placing two CellNodes at the same x and z coords but
   different y's.
*/
public class PathGraph : MonoBehaviour
{
  Vector2                 localOrigin;           // Origin that this is specified from in World Space.
  Terrain                 sceneTerrain;          // The terrain of the scene of this path graph.
  float[,]                vertices;              // All the different shared corners of the different cells
  CellNode[]              cells;                 // The Graph starts at the bottom-left cell
  int                     numVerts;              // Current Number of Verts.
  int                     numCells;              // Current Number of Cells.
  int                     xSize;                 // Size in x direction.
  int                     zSize;                 // Size in z direction.
  int                     xMax;                  // TODO: decide the x size of terrains
  int                     zMax;                  // TODO: decide the z size of terrains
  int                     spaceIncrement;        // The density of terrain points to use for pathgraph generation
  float                   spaceIncrementInverse; // 1/spaceIncrement
  public static Vector2   activeLocalOrigin;     // Holds the local origin of the active pathgraph
  public static PathGraph activeGraph;           // Holds a handle to the active PathGraph

  public PathGraph()
  {
  }
  public Vector3 GetCurrentCellNormalVec( Vector3 currentPos )
  {
    Vector3 norm = CurrentCell(currentPos).Normal();
    return norm;
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
        Vector3 bottomLeft = new Vector3( i, vertices[i,k], k);
        Vector3 topRight   = new Vector3( i+1, vertices[i+1,k+1], k+1);
        cells[index] = new CellNode( bottomLeft, new Vector3( i+1, vertices[i+1,k], k), topRight, index);
        index += 1;
        cells[index] = new CellNode( bottomLeft, new Vector3( i, vertices[i,k+1], k+1), topRight, index);
      }
    }
  }

  void MakeEdges()
  {
    int index = 0;
    while( index < xSize*zSize*2 )
    {
      /*
      // DEBUG
      Vector3 cellPos = cells[index].GetCenter ();
      Debug.Log ( "Current Cell: " + index );
      Debug.Log ( "Cell Center: " + cellPos.x + " " + cellPos.y + " " + cellPos.z );
      // END DEBUG
      */
      if( index%(zSize*2) == 0 && index != (xSize-1)*zSize*2 ) {// bottom edge
        cells[index].SetEdge1 ( index+1 );
        cells[index].SetEdge2 ( index+((2*zSize)+1) );
      }
      else if(index%(zSize*2) == (zSize*2)-1 && index != (zSize*2-1)) {// top edge
        cells[index].SetEdge1 ( index-1 );
        cells[index].SetEdge2 ( index-((2*zSize)+1) );
      }
      else if( index > 0 && index < (zSize*2-1) && index%2 == 1 || (index > (xSize-1)*zSize*2) && index%2 == 0 ) {// Left of Right Edge (Not including top left/bottom right corners because they are special cases
        cells[index].SetEdge1 ( index+1 );
        cells[index].SetEdge2 ( index-1 );
      }
      else if( index == (zSize*2-1) ) {// Top left corner (1 edge)
        cells[index].SetEdge1 ( index-1 );
      }
      else if( index == ((xSize-1)*zSize*2) ) {// Bottom right corner(1 edge)
        cells[index].SetEdge1 ( index+1 );
      }
      else if( index == xSize*zSize*2 - 1 ) // special case index==max-1
      {
        cells[index].SetEdge1 ( index - 1 );
        cells[index].SetEdge2 ( index - (2*zSize+1));
      }
      else if( index == 0 ) // special case index==0
      {
        cells[index].SetEdge1 ( index + 1 );
        cells[index].SetEdge2 ( index + (zMax*2+1) );
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
      /*
      // DEBUG
      int[] edges = cells[index].Edges ();
      Debug.Log ( " edges[0]: " + edges[0] );
      if( edges[0] > 0 )
      {
        Vector3 edge1Pos = cells[edges[0]].GetCenter ();
        Debug.Log ( "Edge 1 Center: " + edge1Pos.x + " " + edge1Pos.y + " " + edge1Pos.z );
      }

      Debug.Log ( " edges[1]: " + edges[1] );
      if( edges[1] > 0 )
      {
        Vector3 edge2Pos = cells[edges[1]].GetCenter ();
        Debug.Log ( " Edge 2 Center: " + edge2Pos.x + " " + edge2Pos.y + " " + edge2Pos.z );
      }

      Debug.Log ( " edges[2]: " + edges[2] );
      if( edges[2] > 0 )
      {
        Vector3 edge3Pos = cells[edges[2]].GetCenter ();
        Debug.Log ( "Edge 3 Center: " + edge3Pos.x + " " + edge3Pos.y + " " + edge3Pos.z );
      }
      // END DEBUG
      */
      index ++;
    }
  }
  
  public int[] Astar( Vector3 currentPos, Vector3 targetPos ) // Causes crash currently
  {
    int numCells = (int)(xSize * spaceIncrementInverse * zSize * spaceIncrementInverse * 2); // number of cells.
    int openSetIndex = 0;
    int closedSetIndex = 0;
    int start = CurrentCell (currentPos).GetIndex(); // Index of starting node
    int current; // index of current Node in path algorithm
    int goal = CurrentCell ( targetPos ).GetIndex (); // Index of goal node
    int kickoutCounter = 0; // after too many loops, algorithm is halted ( for debugging so that log will work)
    int kickoutThreshold = 200; // max number of loops for right now.

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
    /*// DEBUG
    Debug.Log ( " Goal position: " + targetPos.x + " " + targetPos.y + " " + targetPos.z );
    Debug.Log ( " Goal index: " + goal );
    Debug.Log ( " Start position: " + currentPos.x + " " + currentPos.y + " " + currentPos.z );
    Debug.Log ( " Start index: " + start );
    // END DEBUG */
    while (openSet[0] > -1 ) // set isnt empty
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
        // Debug.Log ( "ReconstructPath()!!" );
         return ReconstructPath ( cameFrom, goal );
      }
      closedSet[closedSetIndex] = current;
      closedSetIndex ++;
      int[] edgeIndices = new int[3];        // holds the neighbor verts cells indices of current
      edgeIndices = cells[current].Edges ();

      for ( int i = 0; i < 3 && edgeIndices[i] > -1; i++ )
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
  public int[] ReconstructPath( int[] cameFrom, int currentNodeIndex )
  {
    Debug.Log ( "ReconstructPath" );
    int countIndex = currentNodeIndex;
    int totalNodes = 0; // total nodes along final path
    int[] pathIndices;
    if( cameFrom[countIndex] > -1)
    {
      while( cameFrom[countIndex] > -1 ) // structured in such a way as to leave the starting node off the path bc it is superfluous
      {
        countIndex = cameFrom[countIndex];
        totalNodes++;
      }
      pathIndices = new int[totalNodes];
      pathIndices [totalNodes - 1] = currentNodeIndex;
      for( countIndex = totalNodes-2; countIndex >= 0; countIndex--)
      {
        pathIndices[countIndex] = cameFrom[pathIndices[countIndex+1]];
      }
    }
    else
    {
      // Debug.Log ( " cameFrom[currentNodeIndex]: " + cameFrom[currentNodeIndex] );
      pathIndices = new int[1];
      pathIndices[0] = -3; // -3 is Error Code ( meaning the goal was where the agent started)
    }
    return pathIndices;
  }

  // Use this for initialization
  void Start()
  {
    activeGraph = this; // TEMPORARY FIX!!!!!!
    xSize = 100;
    zSize = 100;
    localOrigin = new Vector2 (PlayerController.playerPos.x - 0.5f * (float)xSize, PlayerController.playerPos.z - 0.5f * (float)zSize );
    activeLocalOrigin = localOrigin; // TEMPORARY FIX!!!!!!
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
    float xDec = position.x - localOrigin.x;
    float zDec = position.z - localOrigin.y;
    int i = (int)xDec;
    int k = (int)zDec;
    xDec -= i; // gets decimal value of x
    zDec -= k;  // gets decimal value of z

    int index;
    if (zDec / xDec >= 1)
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
