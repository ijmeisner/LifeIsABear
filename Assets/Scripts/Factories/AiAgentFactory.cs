using UnityEngine;
using System.Collections;

public class AiAgentFactory {

  static AiAgentFactory instance;
  AiAgentFactory()
  {
  }

  AiAgentFactory GetInstance()
  {
    if( instance == null )
    {
      instance = new AiAgentFactory();
    }
    else
    {
    }
    return instance;
  }

  IAiAgent CreateAiAgent( AIType type, Vector3 position ) // so far only fox exists
  {
    GameObject agent = null;
    IAiAgent iface = null; // interface but cant use keyword
    switch (type)
    {
    case AIType.FOX:
      agent = new GameObject();
      agent.transform.position = position;
      agent.AddComponent ( "Fox" );
      iface = (IAiAgent)agent.GetComponent ( "Fox" );
      break;
    case AIType.DEER:
     // agent = new Deer();
      break;
    case AIType.RABBIT:
      //agent = new Rabbit();
      break;
    case AIType.SQUIRREL:
     // agent = new Squirrel();
      break;
    case AIType.HIKER:
     // agent = new Hiker();
      break;
    case AIType.BEAR:
     // agent = new Bear();
      break;
    default:
      break;
    }

    return iface;
  }

}
