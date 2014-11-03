using UnityEngine;
using System.Collections;

public abstract class ChunkGenerator : MonoBehaviour
{
    public abstract Block GenerateBlock(float x, float y);

}
