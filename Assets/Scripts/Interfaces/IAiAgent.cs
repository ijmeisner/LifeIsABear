using UnityEngine;
using System.Collections;

public interface IAiAgent { // going to be used with factory
  Vector3 GetPosition();
  AIType GetAIType();
  void Live(); // function to simulate the entire ai agent ( possibly a better name for this? )
}
