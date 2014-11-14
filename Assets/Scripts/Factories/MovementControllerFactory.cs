using UnityEngine;
using System.Collections;

public class MovementControllerFactory // singleton factory
{
  static MovementControllerFactory instance;

  private MovementControllerFactory() // do nothing
  {
  }

  public static MovementControllerFactory GetInstance()
  {
    if( instance == null )
    {
      instance = new MovementControllerFactory();
    }
    else
    {
    }
    return instance;
  }
  public IMovementController CreateController( AIType type )
  {
    IMovementController movementCont = null;
    switch (type)
    {
      case AIType.FOX:
        movementCont = new FoxController();
        break;
      /*case AIType.DEER:
        movementCont = new DeerController();
        break;
      case AIType.SQUIRREL:
        movementCont = new SquirrelController();
        break;
      case AIType.RABBIT:
        movementCont = new RabbitController();
        break;
      case AIType.HIKER:
        movementCont = new HikerController();
        break;
      case AIType.BEAR:
        movementCont = new BearController();
        break;
        */
      default:
        break;
    }
    return movementCont;
  }

}

