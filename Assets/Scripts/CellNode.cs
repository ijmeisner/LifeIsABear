using UnityEngine;
using System.Collections;

public class CellNode
{
  Vector2[]   vertIndices;     // Indices into vector3 array that are the verts of the cell.
  Vector3     normal;          // Vector normal to this face of the graph. (Not necessary for
                               // Graph where all Nodes are level)
  int[]       linkedNodes;     // The indices of the (possibly 3) connected Nodes
  Vector3     cellCenter;      // Center of this Triangle
  int         clearanceHeight; // How tall something can be and still use this polygon.
  public bool navigable;       // Is this polygon navigable?
  bool        preferable;      // Is this node difficult to traverse
  int         index;           // Index into 'cells' of PathGraph container

  public CellNode(){
    vertIndices = new Vector2[3];
    vertIndices [0] = new Vector2 (0.0f, 0.0f);
    vertIndices[1] = new Vector2( 0.0f, 0.0f);
    vertIndices [2] = new Vector2 (0.0f, 0.0f);
    normal = new Vector3 (0.0f, 1.0f, 0.0f);
    clearanceHeight = 5;
    navigable = true;
    cellCenter.x = vertIndices [0].x;
    cellCenter.x += vertIndices [1].x;
    cellCenter.x += vertIndices [2].x;
    cellCenter.x /= 3;

    cellCenter.z = vertIndices [0].y;
    cellCenter.z += vertIndices [1].y;
    cellCenter.z += vertIndices [2].y;
    cellCenter.z /= 3;
  } // ** Only need this as temporary place holder in CurrentCell function in PathGraph class.

  public CellNode( Vector3 vert1, Vector3 vert2, Vector3 vert3, int srcIndex)
  {
    Vector3 a; // used to calculate normal vector
    Vector3 b; // used to calculate normal vector
    vertIndices = new Vector2[3];
    vertIndices [0] = new Vector2(vert1.x,vert1.z);
    vertIndices [1] = new Vector2(vert2.x,vert2.z);
    vertIndices [2] = new Vector2(vert3.x,vert3.z);
    a = new Vector3 (vert1.x - vert2.x, vert1.y - vert2.y, vert1.z - vert2.z);
    b = new Vector3 (vert1.x - vert3.x, vert1.y - vert3.y, vert1.z - vert3.z);
    normal = Vector3.Cross ( a, b);
    normal = Vector3.Normalize (normal);
    navigable = Vector3.Angle (normal, new Vector3 (0.0f, 1, 0.0f)) < 45; // SAMPLE** 45 degree angle is too steep to navigate.
    clearanceHeight = 5; // Not specified, so assume 5 meters clearance.
    cellCenter.x = vertIndices [0].x;
    cellCenter.x += vertIndices [1].x;
    cellCenter.x += vertIndices [2].x;
    cellCenter.x /= 3;
    cellCenter.y = vert1.y + vert2.y + vert3.y;
    cellCenter.y /= 3;
    cellCenter.z = vertIndices [0].y;
    cellCenter.z += vertIndices [1].y;
    cellCenter.z += vertIndices [2].y;
    cellCenter.z /= 3;
    index = srcIndex;

  }
  public Vector3 GetCenter()
  {
    return cellCenter;
  }
  public Vector3 Normal()
  {
    return normal;
  }
  

  public int[] Edges()
  {
    return linkedNodes;
  }
  public void SetEdge1( int index)
  {
    linkedNodes = new int[3];
    linkedNodes [0] = index;
  }
  public void SetEdge2( int index)
  {
    linkedNodes [1] = index;
  }
  public void SetEdge3( int index)
  {
    linkedNodes [2] = index;
  }

  public float Heuristic( Vector2 target)
  {
    float distanceEstSqrd; // square root is unnecessary and costly
    float xDiff;
    float yDiff;
    xDiff = target.x - cellCenter.x;
    yDiff = target.y - cellCenter.y;
    distanceEstSqrd = xDiff * xDiff + yDiff * yDiff;
    return distanceEstSqrd;
  }

  public int GetIndex()
  {
    return index;
  }
  // Use this for initialization

}
