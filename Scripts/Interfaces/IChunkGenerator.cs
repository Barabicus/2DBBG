using UnityEngine;
using System.Collections;

public interface IChunkGenerator {

    Block GenerateBlock(Vector2 position);

}
