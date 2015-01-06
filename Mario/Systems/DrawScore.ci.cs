public class SystemDrawScore : GameSystem
{
    public SystemDrawScore()
    {
        loaded = false;
        score = null;
        scoreValue = null;
        coin = null;
        coins = null;
        world = null;
        worldValue = null;
        time = null;
        timeValue = null;
        constAnimSpeed = 6;
        coinEntityId = -1;
    }

    bool loaded;
    ScriptDrawText score;
    ScriptDrawText scoreValue;
    Entity coin;
    ScriptDrawText coins;
    ScriptDrawText world;
    ScriptDrawText worldValue;
    ScriptDrawText time;
    ScriptDrawText timeValue;
    float constAnimSpeed;

    int coinEntityId; // used to detect level restart. (entity becomes null)

    public override void Render(Game game, float dt)
    {
        if (game.assetsLoaded.value != 1)
        {
            return;
        }
        bool worldRestarted = false;
        if (coinEntityId != -1)
        {
            if (game.entities[coinEntityId] != coin)
            {
                worldRestarted = true;
            }
        }

        if (!loaded || worldRestarted)
        {
            loaded = true;

            score = CreateText(game, 5);
            score.text[0] = CharType.CharM;
            score.text[1] = CharType.CharA;
            score.text[2] = CharType.CharR;
            score.text[3] = CharType.CharI;
            score.text[4] = CharType.CharO;

            scoreValue = CreateText(game, 6);

            {
                coin = new Entity();
                coin.draw = new EntityDraw();
                coin.draw.x = 80;
                coin.draw.y = 24;
                coin.draw.width = 8;
                coin.draw.height = 8;
                coin.draw.absoluteScreenPosition = true;
                coin.draw.sprite = "FontCoin";
                coinEntityId = game.AddEntity(coin);
            }


            coins = CreateText(game, 3);

            world = CreateText(game, 5);
            world.text[0] = CharType.CharW;
            world.text[1] = CharType.CharO;
            world.text[2] = CharType.CharR;
            world.text[3] = CharType.CharL;
            world.text[4] = CharType.CharD;

            worldValue = CreateText(game, 3);

            time = CreateText(game, 4);
            time.text[0] = CharType.CharT;
            time.text[1] = CharType.CharI;
            time.text[2] = CharType.CharM;
            time.text[3] = CharType.CharE;

            timeValue = CreateText(game, 3);
        }
        if (game.gameStarted)
        {
            SetIntCharTypes(game.timeLeft(), timeValue.text, 3);
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                timeValue.text[i] = CharType.CharSpace;
            }
        }
        SetIntCharTypes(game.score, scoreValue.text, 6);
        coins.text[0] = CharType.CharTimes;
        coins.text[1] = CharType.Char0 + ((game.coins / 10) % 10);
        coins.text[2] = CharType.Char0 + (game.coins) % 10;
        worldValue.text[0] = game.level[0];
        worldValue.text[1] = game.level[1];
        worldValue.text[2] = game.level[2];

        int stage = (game.platform.FloatToInt(game.t * constAnimSpeed) % 5);
        if (stage == 0) { coin.draw.sprite = "FontCoin"; }
        if (stage == 1) { coin.draw.sprite = "FontCoin"; }
        if (stage == 2) { coin.draw.sprite = "FontCoin1"; }
        if (stage == 3) { coin.draw.sprite = "FontCoin2"; }
        if (stage == 4) { coin.draw.sprite = "FontCoin1"; }

        int totalWidth = 256;
        int addToCenter = game.platform.FloatToInt(game.gameScreenWidth / 2 - totalWidth / 2);
        int addY = game.platform.FloatToInt(-(game.gameScreenHeight - 240));
        score.x = 24 + addToCenter;
        score.y = 16 + addY;
        scoreValue.x = 24 + addToCenter;
        scoreValue.y = 24 + addY;
        coin.draw.x = 96 + addToCenter;
        coin.draw.y = 24 + addY;
        coins.x = 104 + addToCenter;
        coins.y = 24 + addY;
        world.x = 144 + addToCenter;
        world.y = 16 + addY;
        worldValue.x = 152 + addToCenter;
        worldValue.y = 24 + addY;
        time.x = 200 + addToCenter;
        time.y = 16 + addY;
        timeValue.x = 208 + addToCenter;
        timeValue.y = 24 + addY;
    }

    void SetIntCharTypes(int value, int[] charTypes, int n)
    {
        for (int i = 0; i < n; i++)
        {
            int digit = (value / Pow(10, i)) % 10;
            charTypes[n - i - 1] = CharType.Char0 + digit;
        }
    }

    static int Pow(int b, int n)
    {
        if (n == 0) { return 1; }
        if (n == 1) { return b; }
        return Pow(b, n - 1) * b;
    }

    ScriptDrawText CreateText(Game game, int length)
    {
        ScriptDrawText s = new ScriptDrawText();
        s.text = new int[length];
        s.textLength = length;
        Entity e = new Entity();
        e.scripts[e.scriptsCount++] = s;
        game.AddEntity(e);
        return s;
    }
}
