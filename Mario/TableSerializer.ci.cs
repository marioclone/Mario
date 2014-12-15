public abstract class TableBinding
{
    public abstract void Set(string table, int index, string column, string value);
    public abstract void Get(string table, int index, DictionaryStringString items);
}

public class TableSerializer
{
    public void Deserialize(GamePlatform p, string data, TableBinding b)
    {
        IntRef linesCount = new IntRef();
        string[] lines = p.StringSplit(data, "\n", linesCount);
        string[] header = null;
        IntRef headerLength = new IntRef();
        string current = "";
        int currentI = 0;
        for (int i = 0; i < linesCount.value; i++)
        {
            string s = p.StringTrim(lines[i]);
            if (s == "")
            {
                continue;
            }
            if (p.StringStartsWithIgnoreCase(s, "//")
                || p.StringStartsWithIgnoreCase(s, "#"))
            {
                continue;
            }
            if (p.StringStartsWithIgnoreCase(s, "section="))
            {
                current = p.StringReplace(s, "section=", "");

                string sHeader = p.StringTrim(lines[i + 1]);
                header = p.StringSplit(sHeader, "\t", headerLength);
                i++; // header
                currentI = 0;
                continue;
            }
            {
                if (header == null)
                {
                    continue;
                }
                IntRef ssLength = new IntRef();
                string[] ss = p.StringSplit(s, "\t", ssLength);
                for (int k = 0; k < ssLength.value; k++)
                {
                    b.Set(current, currentI, header[k], ss[k]);
                }

                currentI++;
            }
        }
    }
}

public class DictionaryStringString
{
    public DictionaryStringString()
    {
        Start(64);
    }

    public void Start(int count_)
    {
        items = new KeyValueStringString[count_];
        count = count_;
    }

    internal KeyValueStringString[] items;
    internal int count;

    public void Set(string key, string value)
    {
        for (int i = 0; i < count; i++)
        {
            if (items[i] == null)
            {
                continue;
            }
            if (Misc.StringEquals(items[i].key, key))
            {
                items[i].value = value;
                return;
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (items[i] == null)
            {
                items[i] = new KeyValueStringString();
                items[i].key = key;
                items[i].value = value;
                return;
            }
        }
    }

    internal bool ContainsKey(string key)
    {
        for (int i = 0; i < count; i++)
        {
            if (items[i] == null)
            {
                continue;
            }
            if (Misc.StringEquals(items[i].key, key))
            {
                return true;
            }
        }
        return false;
    }

    internal string Get(string key)
    {
        for (int i = 0; i < count; i++)
        {
            if (items[i] == null)
            {
                continue;
            }
            if (Misc.StringEquals(items[i].key, key))
            {
                return items[i].value;
            }
        }
        return null;
    }

    internal void Remove(string key)
    {
        for (int i = 0; i < count; i++)
        {
            if (items[i] == null)
            {
                continue;
            }
            if (Misc.StringEquals(items[i].key, key))
            {
                items[i] = null;
            }
        }
    }
}

public class KeyValueStringString
{
    internal string key;
    internal string value;
}

