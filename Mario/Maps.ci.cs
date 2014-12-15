public class MapsLoader
{
    public MapsLoader()
    {
        map = null;
    }
    internal Map map;
    public void Load(GamePlatform p, byte[] data, int dataLength)
    {
        MapBinding b = new MapBinding();
        b.p = p;
        b.m = new Map();
        TableSerializer s = new TableSerializer();
        s.Deserialize(p, p.StringFromUtf8ByteArray(data, dataLength), b);
        map = b.m;
    }
}

public class ThingType
{
    public const int Block = 0;
    public const int Stone = 1;
    public const int Goomba = 2;
    public const int Pipe = 3;
    public const int Brick = 4;
    public const int Mushroom = 5;
    public const int Mushroom1Up = 6;
    public const int Star = 7;
    public const int Coin = 8;
    public const int Floor = 9;
    public const int Score = 10;
    public const int Pattern = 11;
    public const int HillLarge = 12;
    public const int HillSmall = 13;
    public const int Cloud1 = 14;
    public const int Cloud2 = 15;
    public const int Cloud3 = 16;
    public const int Bush1 = 17;
    public const int Bush2 = 18;
    public const int Bush3 = 19;
    public const int CoinAnimation = 9;
}

public class Map
{
    public Map()
    {
        things = new Thing[1024];
        patterns = new Pattern[1024];
        levels = new Level[128];
    }
    internal string name;
    internal Thing[] things;
    internal int thingsCount;
    internal Pattern[] patterns;
    internal int patternsCount;
    internal Level[] levels;
    internal int levelsCount;
}

public class Thing
{
    internal string level;
    internal int type;
    internal int x;
    internal int y;
    internal int width;
    internal int height;
    internal int contents;
    internal int pipeHeight;
    internal string pattern;
    internal string transport;
}

public class Pattern
{
    internal string patternName;
    internal int thingType;
    internal int x;
    internal int y;
}

public class Level
{
    internal string level;
    internal SettingType setting;
    internal bool static_;
}

public enum SettingType
{
    Overworld,
    Underworld,
    Castle,
    Sky,
    Underwater
}

public class MapBinding : TableBinding
{
    internal Map m;
    internal GamePlatform p;

    public override void Set(string table, int index, string column, string value)
    {
        if (table == "things")
        {
            if (index >= m.thingsCount) { m.thingsCount = index + 1; }
            if (m.things[index] == null) { m.things[index] = new Thing(); }
            Thing k = m.things[index];
            if (column == "level") { k.level = value; }
            if (column == "type") { k.type = GetType(value); }
            if (column == "x") { k.x = IntParse(value); }
            if (column == "y") { k.y = IntParse(value); }
            if (column == "width") { k.width = IntParse(value); }
            if (column == "height") { k.height = IntParse(value); }
            if (column == "contents") { k.contents = GetType(value); }
            if (column == "pipeHeight") { k.pipeHeight = IntParse(value); }
            if (column == "pattern") { k.pattern = value; }
            if (column == "transport") { k.transport = value; }
        }
        if (table == "patterns")
        {
            if (index >= m.patternsCount) { m.patternsCount = index + 1; }
            if (m.patterns[index] == null) { m.patterns[index] = new Pattern(); }
            Pattern k = m.patterns[index];
            if (column == "name") { k.patternName = value; }
            if (column == "type") { k.thingType = GetType(value); }
            if (column == "x") { k.x = IntParse(value); }
            if (column == "y") { k.y = IntParse(value); }
        }
        if (table == "levels")
        {
            if (index >= m.levelsCount) { m.levelsCount = index + 1; }
            if (m.levels[index] == null) { m.levels[index] = new Level(); }
            Level k = m.levels[index];
            if (column == "level") { k.level = value; }
            if (column == "setting") { k.setting = GetSetting(value); }
            if (column == "static") { k.static_ = (value == "1"); }
        }
    }

    SettingType GetSetting(string value)
    {
        if (value == "Overworld")
        {
            return SettingType.Overworld;
        }
        if (value == "Underworld")
        {
            return SettingType.Underworld;
        }
        if (value == "Castle")
        {
            return SettingType.Castle;
        }
        if (value == "Sky")
        {
            return SettingType.Sky;
        }
        if (value == "Underwater")
        {
            return SettingType.Underwater;
        }
        return SettingType.Overworld;
    }

    int GetType(string value)
    {
        if (value == "Block") { return ThingType.Block; }
        if (value == "Stone") { return ThingType.Stone; }
        if (value == "Goomba") { return ThingType.Goomba; }
        if (value == "Pipe") { return ThingType.Pipe; }
        if (value == "Brick") { return ThingType.Brick; }
        if (value == "Mushroom") { return ThingType.Mushroom; }
        if (value == "Mushroom1Up") { return ThingType.Mushroom1Up; }
        if (value == "Star") { return ThingType.Star; }
        if (value == "Coin") { return ThingType.Coin; }
        if (value == "Floor") { return ThingType.Floor; }
        if (value == "Pattern") { return ThingType.Pattern; }
        if (value == "HillLarge") { return ThingType.HillLarge; }
        if (value == "HillSmall") { return ThingType.HillSmall; }
        if (value == "Cloud1") { return ThingType.Cloud1; }
        if (value == "Cloud2") { return ThingType.Cloud2; }
        if (value == "Cloud3") { return ThingType.Cloud3; }
        if (value == "Bush1") { return ThingType.Bush1; }
        if (value == "Bush2") { return ThingType.Bush2; }
        if (value == "Bush3") { return ThingType.Bush3; }
        return -1;
    }

    public override void Get(string table, int index, DictionaryStringString items)
    {
    }

    int IntParse(string s)
    {
        return p.FloatToInt(FloatParse(s));
    }

    float FloatParse(string s)
    {
        FloatRef ret = new FloatRef();
        p.FloatTryParse(s, ret);
        return ret.value;
    }
}
