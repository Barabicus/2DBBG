using UnityEngine;
using System.Collections;

public interface ITickable
{
    void Update(Chunk chunk);
}
