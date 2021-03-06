﻿public class DictionaryStringBitmap
{
    public DictionaryStringBitmap()
    {
        keys = new string[max];
        for (int i = 0; i < max; i++)
        {
            keys[i] = null;
        }
        values = new BitmapCi[max];
        for (int i = 0; i < max; i++)
        {
            values[i] = null;
        }
    }
    internal string[] keys;
    internal BitmapCi[] values;
    internal const int max = 1024;

    internal BitmapCi GetById(int id)
    {
        return values[id];
    }

    internal bool Contains(string key)
    {
        int id = GetId(key);
        return id != -1;
    }

    internal int GetId(string key)
    {
        for (int i = 0; i < max; i++)
        {
            if (keys[i] == key)
            {
                return i;
            }
        }
        return -1;
    }

    internal int Set(string key, BitmapCi bmp)
    {
        int id = GetId(key);
        if (id != -1)
        {
            values[id] = bmp;
            return id;
        }
        for (int i = 0; i < max; i++)
        {
            if (keys[i] == null)
            {
                keys[i] = key;
                values[i] = bmp;
                return i;
            }
        }
        return -1;
    }
}

public class DictionaryStringAudioData
{
    public DictionaryStringAudioData()
    {
        keys = new string[max];
        for (int i = 0; i < max; i++)
        {
            keys[i] = null;
        }
        values = new AudioData[max];
        for (int i = 0; i < max; i++)
        {
            values[i] = null;
        }
    }
    internal string[] keys;
    internal AudioData[] values;
    internal const int max = 1024;

    internal AudioData GetById(int id)
    {
        return values[id];
    }

    internal bool Contains(string key)
    {
        int id = GetId(key);
        return id != -1;
    }

    internal int GetId(string key)
    {
        for (int i = 0; i < max; i++)
        {
            if (keys[i] == key)
            {
                return i;
            }
        }
        return -1;
    }

    internal int Set(string key, AudioData bmp)
    {
        int id = GetId(key);
        if (id != -1)
        {
            values[id] = bmp;
            return id;
        }
        for (int i = 0; i < max; i++)
        {
            if (keys[i] == null)
            {
                keys[i] = key;
                values[i] = bmp;
                return i;
            }
        }
        return -1;
    }
}
