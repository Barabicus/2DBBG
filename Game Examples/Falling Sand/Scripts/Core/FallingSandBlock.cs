using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class FallingSandBlock : FrequentBlock
{

    /// <summary>
    /// Returns the default block of the inherited 
    /// </summary>
    public abstract FallingSandBlock NewBlock
    {
        get;
    }

    protected bool IsOfType(Block block, Type[] types)
    {
        if (block == null)
            return false;

        Type t = block.GetType();
        foreach (Type type in types)
            if (t.IsSubclassOf(type) || t == type)
                return true;

        return false;
    }

}
