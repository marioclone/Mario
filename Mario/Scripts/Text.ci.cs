public class ScriptDrawText : Script
{
    public ScriptDrawText()
    {
        x = 0;
        y = 0;
        text = null;
        textLength = 0;
        loadedText = new int[constMaxLength];
        for (int i = 0; i < constMaxLength; i++)
        {
            loadedText[i] = -1;
        }
        loadedEntities = new int[constMaxLength];
        for (int i = 0; i < constMaxLength; i++)
        {
            loadedEntities[i] = -1;
        }
        loadedTextLength = 0;
        absoluteScreenPosition = true;
    }

    internal int x;
    internal int y;
    internal int[] text; // CharType[]
    internal int textLength;
    internal bool absoluteScreenPosition;
    int loadedTextLength;
    int[] loadedText;
    int[] loadedEntities;
    const int constMaxLength = 32;

    public override void Delete(Game game)
    {
        for (int i = 0; i < constMaxLength; i++)
        {
            if (loadedEntities[i] != -1)
            {
                if (game.entities[loadedEntities[i]] != null)
                {
                    game.DeleteEntity(loadedEntities[i]);
                }
            }
        }
    }

    public override void Update(Game game, int entity, float dt)
    {
        if (Changed())
        {
            Redraw(game);
        }

        // Update position
        for (int i = 0; i < loadedTextLength; i++)
        {
            int charEntityId = loadedEntities[i];
            Entity charEntity = game.entities[charEntityId];
            charEntity.draw.x = x + i * 8;
            charEntity.draw.y = y;
        }
    }

    void Redraw(Game game)
    {
        for (int i = 0; i < loadedTextLength; i++)
        {
            if (loadedEntities[i] != -1)
            {
                game.DeleteEntity(loadedEntities[i]);
                loadedEntities[i] = -1;
            }
            loadedText[i] = -1;
        }
        loadedTextLength = 0;

        // Spawn entity for each letter of text
        for (int i = 0; i < textLength; i++)
        {
            loadedText[i] = text[i];
            Entity e = new Entity();
            e.draw = new EntityDraw();
            e.draw.x = x + i * 8;
            e.draw.y = y;
            e.draw.sprite = GetImage(text[i]);
            e.draw.z = 1;
            e.draw.width = 8;
            e.draw.height = 8;
            e.draw.absoluteScreenPosition = absoluteScreenPosition;
            loadedEntities[i] = game.AddEntity(e);
        }

        loadedTextLength = textLength;
    }

    bool Changed()
    {
        if (textLength != loadedTextLength)
        {
            return true;
        }
        for (int i = 0; i < textLength; i++)
        {
            if (text[i] != loadedText[i])
            {
                return true;
            }
        }
        return false;
    }

    string GetImage(int p)
    {
        switch (p)
        {
            case CharType.CharSpace: return "FontSpace";
            case CharType.Char0: return "Font0";
            case CharType.Char1: return "Font1";
            case CharType.Char2: return "Font2";
            case CharType.Char3: return "Font3";
            case CharType.Char4: return "Font4";
            case CharType.Char5: return "Font5";
            case CharType.Char6: return "Font6";
            case CharType.Char7: return "Font7";
            case CharType.Char8: return "Font8";
            case CharType.Char9: return "Font9";
            case CharType.CharA: return "FontA";
            case CharType.CharB: return "FontB";
            case CharType.CharC: return "FontC";
            case CharType.CharD: return "FontD";
            case CharType.CharE: return "FontE";
            case CharType.CharF: return "FontF";
            case CharType.CharG: return "FontG";
            case CharType.CharH: return "FontH";
            case CharType.CharI: return "FontI";
            case CharType.CharJ: return "FontJ";
            case CharType.CharK: return "FontK";
            case CharType.CharL: return "FontL";
            case CharType.CharM: return "FontM";
            case CharType.CharN: return "FontN";
            case CharType.CharO: return "FontO";
            case CharType.CharP: return "FontP";
            case CharType.CharQ: return "FontQ";
            case CharType.CharR: return "FontR";
            case CharType.CharS: return "FontS";
            case CharType.CharT: return "FontT";
            case CharType.CharU: return "FontU";
            case CharType.CharV: return "FontV";
            case CharType.CharW: return "FontW";
            case CharType.CharX: return "FontX";
            case CharType.CharY: return "FontY";
            case CharType.CharZ: return "FontZ";
            case CharType.CharCopyright: return "FontCopyright";
            case CharType.CharExclamation: return "FontExclamation";
            case CharType.CharMinus: return "FontMinus";
            case CharType.CharTimes: return "FontTimes";
            default: return "";
        }
    }
}

public class CharType
{
    public const int CharSpace = 32;
    public const int Char0 = 48;
    public const int Char1 = 49;
    public const int Char2 = 50;
    public const int Char3 = 51;
    public const int Char4 = 52;
    public const int Char5 = 53;
    public const int Char6 = 54;
    public const int Char7 = 55;
    public const int Char8 = 56;
    public const int Char9 = 57;
    public const int CharA = 58;
    public const int CharB = 59;
    public const int CharC = 60;
    public const int CharD = 61;
    public const int CharE = 62;
    public const int CharF = 63;
    public const int CharG = 64;
    public const int CharH = 65;
    public const int CharI = 66;
    public const int CharJ = 67;
    public const int CharK = 68;
    public const int CharL = 69;
    public const int CharM = 70;
    public const int CharN = 71;
    public const int CharO = 72;
    public const int CharP = 73;
    public const int CharQ = 74;
    public const int CharR = 75;
    public const int CharS = 76;
    public const int CharT = 77;
    public const int CharU = 78;
    public const int CharV = 79;
    public const int CharW = 80;
    public const int CharX = 81;
    public const int CharY = 82;
    public const int CharZ = 83;
    public const int CharCopyright = 37;
    public const int CharMinus = 45;
    public const int CharTimes = 42;
    public const int CharExclamation = 33;
}
