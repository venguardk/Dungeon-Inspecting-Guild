using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
    // This script is for converting key value pair into a serializable format for JSON file, and converting it back
    public TKey key;
    public TValue value;

    public SerializableKeyValuePair(TKey key, TValue value)
    {
        this.key = key;
        this.value = value;
    }
}
