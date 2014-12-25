public class Game : WindowEventHandler
{
    public Game()
    {
        one = 1;
        {
            systemsCount = 0;
            systems = new GameSystem[systemsMax];
            for (int i = 0; i < systemsMax; i++)
            {
                systems[i] = null;
            }
            systems[systemsCount++] = new SystemDraw();
            systems[systemsCount++] = new SystemDrawScore();
            systems[systemsCount++] = new SystemSpawn();
            systems[systemsCount++] = new SystemAudio();
            systems[systemsCount++] = new SystemDrawDeathScreen();
            systems[systemsCount++] = new SystemDrawLoading();
            systems[systemsCount++] = new SystemLevelScript();
            systems[systemsCount++] = new SystemControlsTouch();
            systems[systemsCount++] = new SystemControlsMouse();
            systems[systemsCount++] = new SystemControlsKeyboard();
        }

        {
            entitiesCount = 0;
            entities = new Entity[entitiesMax];
            for (int i = 0; i < entitiesMax; i++)
            {
                entities[i] = null;
            }
        }

        {
            keysDown = new bool[keysDownMax];
            for (int i = 0; i < keysDownMax; i++)
            {
                keysDown[i] = false;
            }
        }

        assets = new Assets();
        assets2 = new AssetList();
        maps = new MapsLoader();
        accum = 0;
        backgroundColor = 0;
        level = "1-1-0";
        setting = SettingType.Overworld;
        scrollxMax = 1000 * 1000;
        time = 0;
        score = 0;
        coins = 0;
        lifesLeft = 3;
        scrollx = 0;
        gameScreenWidth = 0;
        gameStarted = false;
        gamePaused = true;
        gameShowDeathScreen = false;
        restart = false;
        restartPositionX = 0;
        restartPositionY = 0;
        controls = new Controls();
        controlsOverride = new Controls();
        controlsOverrideActive = false;
        constBlueBackgroundColor = Misc.ColorFromArgb(255, 92, 148, 252);
        backgroundColor = constBlueBackgroundColor;

        audio = new AudioControl();
    }

    float one;

    internal GamePlatform platform;

    internal GameSystem[] systems;
    const int systemsMax = 256;
    internal int systemsCount;

    internal bool[] keysDown;
    const int keysDownMax = 256;
    internal Assets assets;
    internal AssetList assets2; // music
    internal MapsLoader maps;

    internal Entity[] entities;
    const int entitiesMax = 1024;
    internal int entitiesCount;

    float accum;
    internal float t; // seconds from game start
    internal int backgroundColor;
    internal string level;
    internal SettingType setting;
    internal float time;
    internal int score;
    internal int coins;
    internal int lifesLeft;
    internal float scrollx; // in original pixels
    internal int scrollxMax; // in original pixels. Maximum x position of any entity on map
    internal float gameScreenWidth; // in original pixels
    internal float gameScreenHeight; // in original pixels
    internal float scale;
    internal float addY;
    internal bool gameStarted;
    internal bool gamePaused;
    internal bool gameShowDeathScreen;
    internal bool restart;
    internal int restartPositionX;
    internal int restartPositionY;
    internal Controls controls;
    internal Controls controlsOverride;
    internal bool controlsOverrideActive;
    internal float playerx; // for mouse controls
    internal int playerGrowth; // for FireFlower
    
    internal AudioControl audio;

    const int constFps = 120;
    const int maxDt = 1;
    internal int constBlueBackgroundColor;

    public void Start(GamePlatform p)
    {
        platform = p;
        platform.AddEventHandler(this);
        maps.Load(platform, assets.data[AssetsHelper.GetAssetId(platform, assets, "Map")], assets.length[AssetsHelper.GetAssetId(platform, assets, "Map")]);
        assetsLoaded = new FloatRef();
        platform.LoadAssetsAsyc(assets2, assetsLoaded);
    }

    internal FloatRef assetsLoaded;

    public override void OnNewFrame(NewFrameEventArgs args)
    {
        // Fixed timestep
        {
            float constDt = one / constFps;
            accum += args.GetDt();
            if (accum >= maxDt)
            {
                accum = maxDt;
            }
            while (accum >= constDt)
            {
                accum -= constDt;

                // Update systems
                for (int i = 0; i < systemsCount; i++)
                {
                    systems[i].Update(this, constDt);
                }
                // Update scripts
                for (int i = 0; i < entitiesCount; i++)
                {
                    if (entities[i] == null) { continue; }
                    Entity e = entities[i];
                    int scriptsCount = e.scriptsCount;
                    for (int k = 0; k < scriptsCount; k++)
                    {
                        if (entities[i] == null) { continue; }
                        if (e.scripts[k] == null)
                        {
                            continue;
                        }
                        e.scripts[k].Update(this, i, constDt);
                    }
                }
            }
        }
        // Render systems
        for (int i = 0; i < systemsCount; i++)
        {
            systems[i].Render(this, args.GetDt());
        }
        if (!gamePaused)
        {
            t += args.GetDt();
        }
    }

    public override void OnKeyDown(KeyEventArgs e)
    {
        for (int i = 0; i < systemsCount; i++)
        {
            systems[i].OnKeyDown(this, e);
        }
    }

    public override void OnKeyPress(KeyPressEventArgs e)
    {
        for (int i = 0; i < systemsCount; i++)
        {
            systems[i].OnKeyPress(this, e);
        }
    }

    public override void OnKeyUp(KeyEventArgs e)
    {
        for (int i = 0; i < systemsCount; i++)
        {
            systems[i].OnKeyUp(this, e);
        }
    }

    public override void OnTouchStart(TouchEventArgs e)
    {
        for (int i = 0; i < systemsCount; i++)
        {
            systems[i].OnTouchStart(this, e);
        }
    }

    public override void OnTouchMove(TouchEventArgs e)
    {
        for (int i = 0; i < systemsCount; i++)
        {
            systems[i].OnTouchMove(this, e);
        }
    }

    public override void OnTouchEnd(TouchEventArgs e)
    {
        for (int i = 0; i < systemsCount; i++)
        {
            systems[i].OnTouchEnd(this, e);
        }
    }

    public override void OnMouseDown(MouseEventArgs e)
    {
        for (int i = 0; i < systemsCount; i++)
        {
            systems[i].OnMouseDown(this, e);
        }
    }


    public override void OnMouseMove(MouseEventArgs e)
    {
        for (int i = 0; i < systemsCount; i++)
        {
            systems[i].OnMouseMove(this, e);
        }
    }

    public override void OnMouseWheel(MouseWheelEventArgs e)
    {
        for (int i = 0; i < systemsCount; i++)
        {
            systems[i].OnMouseWheel(this, e);
        }
    }

    public override void OnMouseUp(MouseEventArgs e)
    {
        for (int i = 0; i < systemsCount; i++)
        {
            systems[i].OnMouseUp(this, e);
        }
    }

    public override void OnFocusChanged(bool focus)
    {
        for (int i = 0; i < systemsCount; i++)
        {
            systems[i].OnFocusChanged(this, focus);
        }
    }

    internal void DeleteEntity(int entity)
    {
        entities[entity].Delete(this);
#if CITO
        delete entities[entity];
#endif
        entities[entity] = null;
    }

    internal void DeleteEntity_(Entity e)
    {
        for (int i = 0; i < entitiesCount; i++)
        {
            if (entities[i] == e)
            {
                DeleteEntity(i);
            }
        }
    }

    public int AddEntity(Entity e)
    {
        for (int i = 0; i < entitiesCount; i++)
        {
            if (entities[i] == null)
            {
                entities[i] = e;
                return i;
            }
        }
        int id = entitiesCount;
        entities[entitiesCount++] = e;
        return id;
    }

    const int timeSpeed = 2;
    internal int timeLeft()
    {
        return platform.FloatToInt(400 - time * timeSpeed);
    }

    public void AudioPlay(string p)
    {
        audio.audioPlaySounds[audio.audioPlaySoundsCount++] = p;
    }

    internal byte[] GetFile(string p)
    {
        for (int i = 0; i < assets2.count; i++)
        {
            if (AssetsHelper.FileNameEquals(platform, assets2.items[i].name, p))
            {
                return assets2.items[i].data;
            }
        }
        return null;
    }

    internal int GetFileLength(string p)
    {
        for (int i = 0; i < assets2.count; i++)
        {
            if (AssetsHelper.FileNameEquals(platform, assets2.items[i].name, p))
            {
                return assets2.items[i].dataLength;
            }
        }
        return 0;
    }

    public const int ScoreGoomba = 100;
    public const int ScoreCoin = 200;
    public const int ScoreKoopa = 500;
    public const int ScoreMushroom = 1000;
    public const int ScorePiranha = 500;
}

public class AudioControl
{
    public AudioControl()
    {
        audioPlayMusic = null;
        audioPlaySoundsCount = 0;
        audioPlaySounds = new string[audioPlaySoundsMax];
        for (int i = 0; i < audioPlaySoundsMax; i++)
        {
            audioPlaySounds[i] = null;
        }
    }

    internal string audioPlayMusic;
    internal string[] audioPlaySounds;
    internal int audioPlaySoundsCount;
    const int audioPlaySoundsMax = 64;

    internal void ClearSounds()
    {
        for (int i = 0; i < audioPlaySoundsCount; i++)
        {
            audioPlaySounds[i] = null;
        }
        audioPlaySoundsCount = 0;
    }
}
