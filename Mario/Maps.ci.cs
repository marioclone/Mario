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
    public const int CoinAnimation = 20;
    public const int PipeHorizontal = 21;
    public const int PipeVertical = 22;
    public const int CastleOutside = 23;
    public const int SceneryCastleTop = 24;
    public const int SceneryCastleWindow = 25;
    public const int SceneryCastleDoor = 26;
    public const int SceneryCastleRailing = 27;
    public const int SceneryFlagTop = 28;
    public const int SceneryFlagPole = 29;
    public const int SceneryBrickPlain = 30;
    public const int MushroomDeathly = 31;
    public const int Ceiling = 32;
    public const int PlatformGenerator = 33;
    public const int Koopa = 34;
    public const int Piranha = 35;
    public const int Tree = 36;
    public const int Platform = 37;
    public const int StartInsideCastle = 38;
    public const int CastleBridge = 39;
    public const int Water = 40;
    public const int CastleChain = 41;
    public const int CastleAxe = 42;
    public const int CastleNpc = 43;
    public const int Bowser = 44;
    public const int CastleBlock = 45;
    public const int WarpWorld = 46;
    public const int Fence = 47;
    public const int PlantSmall = 48;
    public const int PlantLarge = 49;
    public const int Springboard = 50;
    public const int Coral = 51;
    public const int Blooper = 52;
    public const int CheepCheep = 53;
}

public class Map
{
    public Map()
    {
        things = new Thing[1024 * 8];
        patterns = new Pattern[1024];
        levels = new Level[128];
    }
    internal Thing[] things;
    internal int thingsCount;
    internal Pattern[] patterns;
    internal int patternsCount;
    internal Level[] levels;
    internal int levelsCount;
}

public class Thing
{
    public Thing()
    {
        level = null;
        type = 0;
        x = 0;
        y = 0;
        width = 0;
        height = 0;
        contents = 0;
        pipeHeight = 0;
        pattern = null;
        transport = null;
        entrance = null;
        hidden = false;
        direction = 0;
        piranha = false;
        platformType = PlatformDirection.None;
        platformStart = 0;
        platformEnd = 0;
        platformSpeed = 0;
        platformNoCollideChar = false;
        castleBlockFireballs = 0;
        castleBlockSpeed = 0;
        castleBlockDirection = 0;
        koopaType = KoopaType.Normal;
    }
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
    internal string entrance;
    internal bool hidden;
    internal int direction;
    internal bool piranha;
    internal PlatformDirection platformType;
    internal float platformStart;
    internal float platformEnd;
    internal float platformSpeed;
    internal bool platformNoCollideChar;
    internal int castleBlockFireballs;
    internal float castleBlockSpeed;
    internal float castleBlockDirection;
    internal KoopaType koopaType;
}

public enum KoopaType
{
    Normal,
    Floating,
    Jumping
}

public enum PlatformType
{
    None,
    Floating,
    Sliding,
    Falling
}

public class Pattern
{
    public Pattern()
    {
        width = 1;
        height = 1;
    }
    internal string patternName;
    internal int thingType;
    internal int x;
    internal int y;
    internal int width;
    internal int height;
}

public class Level
{
    internal string level;
    internal SettingType setting;
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
            if (column == "entrance") { k.entrance = value; }
            if (column == "hidden") { k.hidden = value != null && value != ""; }
            if (column == "direction") { k.direction = IntParse(value); }
            if (column == "piranha") { k.piranha = value != null && value != ""; }
            if (column == "platformType") { k.platformType = PlatformTypeParse(value); }
            if (column == "platformStart") { k.platformStart = FloatParse(value); }
            if (column == "platformEnd") { k.platformEnd = FloatParse(value); }
            if (column == "platformSpeed") { k.platformSpeed = FloatParse(value); }
            if (column == "platformNoCollideChar") { k.platformNoCollideChar = value != null && value != ""; }
            if (column == "castleBlockFireballs") { k.castleBlockFireballs = IntParse(value); }
            if (column == "castleBlockSpeed") { k.castleBlockSpeed = FloatParse(value); }
            if (column == "castleBlockDirection") { k.castleBlockDirection = FloatParse(value); }
            if (column == "koopaType") { k.koopaType = GetKoopaType(value); }
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
            if (column == "width") { k.width = IntParse(value); }
            if (column == "height") { k.height = IntParse(value); }
        }
        if (table == "levels")
        {
            if (index >= m.levelsCount) { m.levelsCount = index + 1; }
            if (m.levels[index] == null) { m.levels[index] = new Level(); }
            Level k = m.levels[index];
            if (column == "level") { k.level = value; }
            if (column == "setting") { k.setting = GetSetting(value); }
        }
    }

    KoopaType GetKoopaType(string value)
    {
        if (value == "floating")
        {
            return KoopaType.Floating;
        }
        if (value == "jumping")
        {
            return KoopaType.Jumping;
        }
        return KoopaType.Normal;
    }

    PlatformDirection PlatformTypeParse(string value)
    {
        if (value == "sliding")
        {
            return PlatformDirection.Sliding;
        }
        if (value == "floating")
        {
            return PlatformDirection.Floating;
        }
        if (value == "falling")
        {
            return PlatformDirection.Falling;
        }
        return PlatformDirection.None;
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
        if (value == "PipeHorizontal") { return ThingType.PipeHorizontal; }
        if (value == "PipeVertical") { return ThingType.PipeVertical; }
        if (value == "CastleOutside") { return ThingType.CastleOutside; }
        if (value == "SceneryCastleTop") { return ThingType.SceneryCastleTop; }
        if (value == "SceneryCastleWindow") { return ThingType.SceneryCastleWindow; }
        if (value == "SceneryCastleDoor") { return ThingType.SceneryCastleDoor; }
        if (value == "SceneryCastleRailing") { return ThingType.SceneryCastleRailing; }
        if (value == "SceneryFlagTop") { return ThingType.SceneryFlagTop; }
        if (value == "SceneryFlagPole") { return ThingType.SceneryFlagPole; }
        if (value == "SceneryBrickPlain") { return ThingType.SceneryBrickPlain; }
        if (value == "Ceiling") { return ThingType.Ceiling; }
        if (value == "PlatformGenerator") { return ThingType.PlatformGenerator; }
        if (value == "Koopa") { return ThingType.Koopa; }
        if (value == "Tree") { return ThingType.Tree; }
        if (value == "Platform") { return ThingType.Platform; }
        if (value == "StartInsideCastle") { return ThingType.StartInsideCastle; }
        if (value == "CastleBridge") { return ThingType.CastleBridge; }
        if (value == "Water") { return ThingType.Water; }
        if (value == "Bowser") { return ThingType.Bowser; }
        if (value == "CastleChain") { return ThingType.CastleChain; }
        if (value == "CastleAxe") { return ThingType.CastleAxe; }
        if (value == "CastleNpc") { return ThingType.CastleNpc; }
        if (value == "CastleBlock") { return ThingType.CastleBlock; }
        if (value == "WarpWorld") { return ThingType.WarpWorld; }
        if (value == "Fence") { return ThingType.Fence; }
        if (value == "PlantSmall") { return ThingType.PlantSmall; }
        if (value == "PlantLarge") { return ThingType.PlantLarge; }
        if (value == "Springboard") { return ThingType.Springboard; }
        if (value == "Coral") { return ThingType.Coral; }
        if (value == "Blooper") { return ThingType.Blooper; }
        if (value == "CheepCheep") { return ThingType.CheepCheep; }
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
