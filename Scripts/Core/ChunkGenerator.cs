using UnityEngine;
using System.Collections;

public abstract class ChunkGenerator : MonoBehaviour
{
    /// <summary>
    /// Using the world position X and Y the generator outputs a block.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public abstract Block GenerateBlock(float x, float y);

}
