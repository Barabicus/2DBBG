using UnityEngine;
using System.Collections;

public class IgnoreCollisions : MonoBehaviour {

    public int[] ignoreList;

    void Start()
    {
        for (int i = 0; i < ignoreList.Length / 2; i++)
        {
            int i1, i2;
            LayersAtIndex(i, out i1, out i2);
            Physics2D.IgnoreLayerCollision(i1, i2);
        }
    }

    void AddLayer(int layer1, int layer2)
    {
        if (ignoreList == null)
        {
            ignoreList = new int[2];
        }
        else
        {
            int[] tempList = ignoreList;
            ignoreList = new int[tempList.Length + 1];
            for (int i = 0; i < tempList.Length; i++)
            {
                ignoreList[i] = tempList[i];
            }
            ignoreList[ignoreList.Length - 2] = layer1;
            ignoreList[ignoreList.Length - 1] = layer2;
        }
    }

    void LayersAtIndex(int index, out int layer1, out int layer2)
    {
        index *= 2;
        if (index < 0 || index > ignoreList.Length - 2)
        {
            Debug.LogError("Invalid Index");
            layer1 = -1;
            layer2 = -1;
            return;
        }

        layer1 = ignoreList[index];
        layer2 = ignoreList[index + 1];
    }
	
}
