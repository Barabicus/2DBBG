using UnityEngine;
using System.Collections;

public class MiningAsteroidBlock : Block
{

    public override Color BlockColor
    {
        get { return Color.gray; }
    }

    public override void OnDestroy(Chunk chunk)
    {
        base.OnDestroy(chunk);
        if (Random.Range(0, 100) == 0)
            MonoBehaviour.Instantiate(MiningGUIControl.Instance.orbPrefab, chunk.IndexToWorldPosition(indexX, indexY), Quaternion.identity);
    }
}
