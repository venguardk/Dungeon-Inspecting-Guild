using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SerializableVector2
{
    // This script is for converting Vector2 into a serializable format for JSON file, and converting it back
    public float x;
    public float y;

    public SerializableVector2(Vector2 vector)
    {
        this.x = vector.x;
        this.y = vector.y;
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }
}

