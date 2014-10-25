using UnityEngine;
using System.Collections;
using System;

public struct Vector2IndexWrapper
{
    public float x;
    public float y;

    public Vector2 Vector2
    {
        get { return new Vector2(x, y); }
    }

    public Vector2IndexWrapper(float x, float y)
    {
        this.x = x;
        this.y = y;
        TruncateValues();
    }

    public Vector2IndexWrapper(Vector2 vector)
    {
        this.x = vector.x;
        this.y = vector.y;
        TruncateValues();
    }

    private void TruncateValues()
    {
        x = Truncate(x, 2);
        y = Truncate(y, 2);
    }

    private float Truncate(float value, int digits)
    {
        double mult = Math.Pow(10.0, digits);
        double result = Math.Truncate(mult * value) / mult;
        return (float)result;
    }

    public static Vector2IndexWrapper operator +(Vector2IndexWrapper v1, Vector2 v2)
    {
        return new Vector2IndexWrapper(v1.x + v2.x, v1.y + v2.y);
    }

    public static Vector2IndexWrapper operator -(Vector2IndexWrapper v1, Vector2 v2)
    {
        return new Vector2IndexWrapper(v1.x - v2.x, v1.y - v2.y);
    }

    public static Vector2IndexWrapper operator *(Vector2IndexWrapper v1, Vector2 v2)
    {
        return new Vector2IndexWrapper(v1.x * v2.x, v1.y * v2.y);
    }

    public static Vector2IndexWrapper operator /(Vector2IndexWrapper v1, Vector2 v2)
    {
        return new Vector2IndexWrapper(v1.x / v2.x, v1.y / v2.y);
    }

    public static implicit operator Vector2(Vector2IndexWrapper vw)
    {
        return new Vector2(vw.x, vw.y);
    }

    public static implicit operator Vector2IndexWrapper(Vector2 vector)
    {
        return new Vector2IndexWrapper(vector);
    }

    public override int GetHashCode()
    {
        return new Vector2(x, y).GetHashCode();
    }

    public override string ToString()
    {
        return new Vector2(x, y).ToString();
    }
}