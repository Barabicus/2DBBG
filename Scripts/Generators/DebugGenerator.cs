using UnityEngine;
using System.Collections;

public class DebugGenerator : ChunkGenerator {

    public override Block GenerateBlock(float x, float y)
    {
        return new StandardBlock();
    }
}
