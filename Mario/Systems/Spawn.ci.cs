// Reads Things in the Map, and spawn them as Entities.
public class SystemSpawn : GameSystem
{
    public SystemSpawn()
    {
        Restart();
    }

    void Restart()
    {
        loaded = false;
        wasGameStarted = false;
    }

    bool loaded;
    const int spawnedMax = 1024;
    Entity player;
    bool wasGameStarted;

    public override void Update(Game game, float dt)
    {
        Map map = game.maps.map;

        if (game.assetsLoaded.value != 1)
        {
            return;
        }

        if (!loaded)
        {
            loaded = true;

            // Level setting
            for (int i = 0; i < game.maps.map.levelsCount; i++)
            {
                Level l = game.maps.map.levels[i];
                if (l.level == game.level)
                {
                    game.setting = l.setting;
                }
            }

            // Spawn things
            for (int i = 0; i < map.thingsCount; i++)
            {
                Thing t = map.things[i];
                if (t.level == game.level)
                {
                    SpawnThing(game, map, t);
                }
            }

            game.scrollxMax = MaxX(game);

            // Player
            int spawnX = game.restartPositionX;
            int spawnY = game.restartPositionY;
            if (spawnX == 0 && spawnY == 0)
            {
                int playerHeight = 16;
                if (player != null)
                {
                    playerHeight = player.draw.height;
                }

                spawnX = 32 - 8;
                if (game.setting == SettingType.Castle)
                {
                    spawnY = 208 - playerHeight - 16 * 6;
                }
                else
                {
                    spawnY = 208 - playerHeight;
                }
            }
            
            game.restartPositionX = 0;
            game.restartPositionY = 0;

            if (player == null)
            {
                player = new Entity();
                player.draw = new EntityDraw();
                player.draw.z = 2;
                player.growable = new EntityGrowable();
                player.attackableTouch = new EntityAttackableTouch();
                player.scripts[player.scriptsCount++] = new ScriptPlayer();
            }
            player.draw.x = spawnX;
            player.draw.y = spawnY;
            player.draw.hidden = false;
            game.AddEntity(player);

            game.scrollx = 0;

            // Main menu
            //{
            //    Entity logo = new Entity();
            //    logo.draw = new EntityDraw();
            //    logo.draw.x = 40;
            //    logo.draw.y = 32;
            //    logo.draw.width = 176;
            //    logo.draw.height = 88;
            //    logo.draw.sprite = "TitleLogo";
            //    game.AddEntity(logo);

            //    Entity singleplayer = new Entity();
            //    ScriptDrawText singleplayerText = new ScriptDrawText();
            //    singleplayerText.x = 96;
            //    singleplayerText.y = 144;
            //    singleplayerText.text = new int[13];
            //    singleplayerText.textLength = 13;
            //    singleplayerText.text[0] = CharType.Char1;
            //    singleplayerText.text[1] = CharType.CharSpace;
            //    singleplayerText.text[2] = CharType.CharP;
            //    singleplayerText.text[3] = CharType.CharL;
            //    singleplayerText.text[4] = CharType.CharA;
            //    singleplayerText.text[5] = CharType.CharY;
            //    singleplayerText.text[6] = CharType.CharE;
            //    singleplayerText.text[7] = CharType.CharR;
            //    singleplayerText.text[8] = CharType.CharSpace;
            //    singleplayerText.text[9] = CharType.CharG;
            //    singleplayerText.text[10] = CharType.CharA;
            //    singleplayerText.text[11] = CharType.CharM;
            //    singleplayerText.text[12] = CharType.CharE;
            //    singleplayerText.absoluteScreenPosition = false;
            //    singleplayer.scripts[singleplayer.scriptsCount++] = singleplayerText;
            //    game.AddEntity(singleplayer);

            //    Entity twoplayer = new Entity();
            //    ScriptDrawText twoplayerText = new ScriptDrawText();
            //    twoplayerText.x = 96;
            //    twoplayerText.y = 160;
            //    twoplayerText.text = new int[13];
            //    twoplayerText.textLength = 13;
            //    twoplayerText.text[0] = CharType.Char2;
            //    twoplayerText.text[1] = CharType.CharSpace;
            //    twoplayerText.text[2] = CharType.CharP;
            //    twoplayerText.text[3] = CharType.CharL;
            //    twoplayerText.text[4] = CharType.CharA;
            //    twoplayerText.text[5] = CharType.CharY;
            //    twoplayerText.text[6] = CharType.CharE;
            //    twoplayerText.text[7] = CharType.CharR;
            //    twoplayerText.text[8] = CharType.CharSpace;
            //    twoplayerText.text[9] = CharType.CharG;
            //    twoplayerText.text[10] = CharType.CharA;
            //    twoplayerText.text[11] = CharType.CharM;
            //    twoplayerText.text[12] = CharType.CharE;
            //    twoplayerText.absoluteScreenPosition = false;
            //    twoplayer.scripts[twoplayer.scriptsCount++] = twoplayerText;
            //    game.AddEntity(twoplayer);

            //    Entity copyright = new Entity();
            //    ScriptDrawText copyrightText = new ScriptDrawText();
            //    copyrightText.x = 104;
            //    copyrightText.y = 120;
            //    copyrightText.text = new int[14];
            //    copyrightText.textLength = 14;
            //    copyrightText.text[0] = CharType.CharCopyright;
            //    copyrightText.text[1] = CharType.Char1;
            //    copyrightText.text[2] = CharType.Char9;
            //    copyrightText.text[3] = CharType.Char8;
            //    copyrightText.text[4] = CharType.Char5;
            //    copyrightText.text[5] = CharType.CharSpace;
            //    copyrightText.text[6] = CharType.CharN;
            //    copyrightText.text[7] = CharType.CharI;
            //    copyrightText.text[8] = CharType.CharN;
            //    copyrightText.text[9] = CharType.CharT;
            //    copyrightText.text[10] = CharType.CharE;
            //    copyrightText.text[11] = CharType.CharN;
            //    copyrightText.text[12] = CharType.CharD;
            //    copyrightText.text[13] = CharType.CharO;
            //    copyrightText.absoluteScreenPosition = false;
            //    copyright.scripts[copyright.scriptsCount++] = copyrightText;
            //    game.AddEntity(copyright);
            //}

            if (game.setting == SettingType.Underworld
                || game.setting == SettingType.Castle)
            {
                game.backgroundColor = Misc.ColorFromArgb(255, 0, 0, 0);
            }
            else
            {
                game.backgroundColor = game.constBlueBackgroundColor;
            }
        }

        if (game.gameStarted && (!wasGameStarted))
        {
            wasGameStarted = true;
            game.gamePaused = false;
            if (game.setting == SettingType.Underworld)
            {
                game.audio.audioPlayMusic = "Underground";
            }
            else
            {
                game.audio.audioPlayMusic = "Overworld";
            }
            game.time = 0;
        }
        if (!game.gamePaused)
        {
            game.time += dt;
        }
        if (game.restart)
        {
            Restart();
            game.restart = false;
            for (int i = 0; i < game.entitiesCount; i++)
            {
                if (game.entities[i] == null) { continue; }
                if (game.entities[i] == player)
                {
                    // Keep player, only remove from list
                    game.entities[i] = null;
                }
                else
                {
                    // Delete entity memory
                    game.DeleteEntity(i);
                }
            }
            game.entitiesCount = 0;
        }
    }

    static int MaxX(Game game)
    {
        float maxX = 0;
        for (int i = 0; i < game.entitiesCount; i++)
        {
            Entity e = game.entities[i];
            if (e == null) { continue; }
            if (e.draw == null) { continue; }
            if (e.draw.x > maxX)
            {
                maxX = e.draw.x;
            }
        }
        return game.platform.FloatToInt(maxX);
    }

    void SpawnThing(Game game, Map map, Thing t)
    {
        if (t.width == 0) { t.width = 1; }
        if (t.height == 0) { t.height = 1; }

        // Solids

        if (t.type == ThingType.Floor)
        {
            Entity e = Spawn(game, "SolidsFloorNormal", t.x, t.y);
            e.collider = new EntityCollider();
            e.draw.xrepeat = t.width;
            if (game.setting == SettingType.Overworld)
            {
                e.draw.yrepeat = 2;
            }
            else
            {
                e.draw.yrepeat = 5;
            }
        }
        if (t.type == ThingType.Ceiling)
        {
            t.type = ThingType.Brick;
            t.y = 8 * 11;
        }
        if (t.type == ThingType.Pattern)
        {
            int patternWidth = 0;
            for (int x = 0; x < t.width; x++)
            {
                for (int k = 0; k < map.patternsCount; k++)
                {
                    Pattern p = map.patterns[k];
                    if (p.patternName == t.pattern)
                    {
                        Thing t2 = new Thing();
                        t2.x = t.x + p.x + x * patternWidth;
                        t2.y = t.y + p.y;
                        t2.type = p.thingType;
                        t2.width = p.width;
                        t2.height = p.height;
                        t2.transport = t.transport;
                        SpawnThing(game, map, t2);

                        if (p.x > patternWidth)
                        {
                            patternWidth = p.x;
                        }
                    }
                }
            }
        }
        for (int x = 0; x < t.width; x++)
        {
            for (int y = 0; y < t.height; y++)
            {
                if (t.type == ThingType.Block)
                {
                    Entity e = Spawn(game, "SolidsBlockNormalNormalNormal", t.x + x * 8, t.y + y * 8);
                    if (t.hidden)
                    {
                        e.draw.hidden = true;
                    }
                    e.collider = new EntityCollider();
                    e.attackablePush = new EntityAttackablePush();
                    e.attackablePush.pushSide = PushSide.BottomBrickDestroy;
                    ScriptQuestionBlock script = new ScriptQuestionBlock();
                    ActionSpawnThing action = new ActionSpawnThing();
                    int contents = t.contents;
                    if (contents == 0)
                    {
                        contents = ThingType.CoinAnimation;
                    }
                    action.thingType = contents;
                    action.x = t.x + x * 8;
                    action.y = t.y + y * 8 + 8;
                    script.onUse = action;
                    e.scripts[e.scriptsCount++] = script;
                    e.scripts[e.scriptsCount++] = new ScriptBrickBumpAnimation();
                }
                if (t.type == ThingType.Brick)
                {
                    Entity e = Spawn(game, "SolidsBrickNormalNormal", t.x + x * 8, t.y + y * 8);
                    e.collider = new EntityCollider();
                    e.attackablePush = new EntityAttackablePush();
                    e.attackablePush.pushSide = PushSide.BottomBrickDestroy;
                    e.scripts[e.scriptsCount++] = new ScriptBrickBumpAnimation();
                    e.scripts[e.scriptsCount++] = new ScriptBrickDestroy();
                }
                if (t.type == ThingType.Stone)
                {
                    Entity e = Spawn(game, "SolidsStoneNormal", t.x + x * 8, t.y - y * 8);
                    e.collider = new EntityCollider();
                }
                if (t.type == ThingType.Coin)
                {
                    Entity e = Spawn(game, "CharactersCoinNormalNormalNormal", t.x + x * 8, t.y - y * 8);
                    e.scripts[e.scriptsCount++] = new ScriptCoinStatic();
                }
                if (t.type == ThingType.CastleBridge)
                {
                    Entity e = Spawn(game, "SolidsCastleBridge", t.x + x * 8, t.y + y * 8);
                    e.collider = new EntityCollider();
                }
            }
        }
        if (t.type == ThingType.Pipe)
        {
            Entity e = Spawn(game, "SolidsPipeNormalTop", t.x, t.y + t.pipeHeight);
            e.draw.width = 32;
            e.draw.height = 16;
            e.draw.z = 3;
            e.collider = new EntityCollider();
            ScriptPipeTransport transport = null;
            if (t.transport != null && t.transport != "")
            {
                transport = new ScriptPipeTransport();
                transport.targetLevelOrEntrance = t.transport;
                e.scripts[e.scriptsCount++] = transport;
            }

            Entity e2 = Spawn(game, "SolidsPipeNormalMiddle", t.x, t.y + t.pipeHeight - 8);
            e2.draw.width = 32;
            e2.draw.height = 16;
            e2.draw.yrepeat = t.pipeHeight / 8 - 1;
            e2.draw.z = 3;
            e2.collider = new EntityCollider();

            if (transport != null)
            {
                transport.pipeMiddle = e2;
            }
        }
        if (t.type == ThingType.PipeHorizontal)
        {
            Entity e = Spawn(game, "SolidsPipeHorizontal", t.x, t.y);
            e.draw.width = 48;
            e.draw.height = 32;
            e.draw.z = 3;
            e.collider = new EntityCollider();
            ScriptPipeTransport transport = new ScriptPipeTransport();
            transport.horizontal = true;
            transport.targetLevelOrEntrance = t.transport;
            e.scripts[e.scriptsCount++] = transport;
        }
        if (t.type == ThingType.PipeVertical)
        {
            Entity e = Spawn(game, "SolidsPipeVerticalNormal", t.x, t.y);
            e.draw.width = 16;
            e.draw.height = 16;
            e.draw.z = 3;
            e.draw.yrepeat = t.height;
            e.collider = new EntityCollider();
        }
        if (t.type == ThingType.SceneryFlagPole)
        {
            Entity e = Spawn(game, "SceneryFlagPole", t.x, t.y);
            e.draw.width = 2;
            e.draw.height = 16;
            e.draw.yrepeat = 10;
            e.draw.z = 0;
            ScriptFlagPole script = new ScriptFlagPole();
            script.targetLevelOrEntrance = t.transport;
            script.flag = Spawn(game, "SceneryFlagNormal", t.x, t.y);
            script.flag.draw.z = 2;
            e.scripts[e.scriptsCount++] = script;
        }
        if (t.type == ThingType.SceneryFlagTop)
        {
            Entity e = Spawn(game, "SceneryFlagTop", t.x - one / 2, t.y);
            e.draw.width = 8;
            e.draw.height = 8;
            e.draw.z = 2;
        }
        if (t.type == ThingType.SceneryBrickPlain)
        {
            Entity e = Spawn(game, "SceneryBrickPlainNormal", t.x, t.y);
            e.draw.xrepeat = t.width;
            e.draw.yrepeat = t.height;
        }
        if (t.type == ThingType.SceneryCastleRailing)
        {
            Entity e = Spawn(game, "SceneryCastleRailingNormal", t.x, t.y);
            e.draw.xrepeat = t.width;
            e.draw.yrepeat = t.height;
        }
        if (t.type == ThingType.SceneryCastleTop)
        {
            Entity e = Spawn(game, "SceneryCastleTopNormal", t.x, t.y);
            e.draw.width = 8;
            e.draw.height = 8;
            e.draw.xrepeat = t.width;
            e.draw.yrepeat = t.height;
            e.draw.z = 0;
        }
        if (t.type == ThingType.SceneryCastleWindow)
        {
            Entity e = Spawn(game, "SceneryCastleWindow", t.x, t.y);
            e.draw.width = 8;
            e.draw.height = 16;
            e.draw.z = 0;
        }
        if (t.type == ThingType.SceneryCastleDoor)
        {
            Entity e = Spawn(game, "SceneryCastleDoor", t.x, t.y);
            e.draw.width = 16;
            e.draw.height = 24;
            e.draw.z = 1;
        }
        if (t.type == ThingType.CastleOutside)
        {
        }
        if (t.type == ThingType.PlatformGenerator)
        {
            Entity e = Spawn(game, "", t.x, t.y);
            PlatformGenerator script = new PlatformGenerator();
            script.direction = t.direction;
            e.scripts[e.scriptsCount++] = script;
        }
        if (t.type == ThingType.Platform)
        {
            if (t.platformType == PlatformDirection.Floating)
            {
                SpawnPlatform.Spawn(game, t.x, t.y, t.platformType, GetY(t.platformStart), GetY(t.platformEnd));
            }
            else
            {
                SpawnPlatform.Spawn(game, t.x, t.y, t.platformType, GetX(t.platformStart), GetX(t.platformEnd));
            }
        }
        if (t.type == ThingType.Tree)
        {
            Entity topLeft = Spawn(game, "SolidsTreeTopNormalLeft", t.x, t.y);
            topLeft.collider = new EntityCollider();
            topLeft.draw.z = 1;
            Entity topMiddle = Spawn(game, "SolidsTreeTopNormalMiddle", t.x + 8, t.y);
            topMiddle.draw.xrepeat = t.width - 2;
            topMiddle.collider = new EntityCollider();
            topMiddle.draw.z = 1;
            Entity topRight = Spawn(game, "SolidsTreeTopNormalRight", t.x + (t.width - 1) * 8, t.y);
            topRight.collider = new EntityCollider();
            topRight.draw.z = 1;
            Entity trunk = Spawn(game, "SceneryTreeTrunkNormal", t.x + 8, t.y - 8);
            trunk.draw.width = 8;
            trunk.draw.height = 8;
            trunk.draw.xrepeat = (t.width - 2) * 2;
            trunk.draw.yrepeat = 16 * 2;
            trunk.draw.z = 0;
        }
        if (t.type == ThingType.CastleBlock)
        {
            SpawnCastleBlock.Spawn(game, t.x, t.y, t.castleBlockFireballs, t.castleBlockSpeed, t.castleBlockDirection);
        }

        // Characters

        if (t.type == ThingType.Goomba)
        {
            SpawnGoomba.Spawn(game, t.x, t.y);
        }

        if (t.type == ThingType.Koopa)
        {
            SpawnKoopa.Spawn(game, t.x, t.y);
        }

        if (t.piranha)
        {
            SpawnPiranha.Spawn(game, t.x + 4, t.y + t.pipeHeight + 12);
        }

        // Scenery
        if (t.type == ThingType.HillLarge)
        {
            SpawnScenery(game, "SceneryHillLarge", t.x, t.y, 80, 35);
        }
        if (t.type == ThingType.HillSmall)
        {
            SpawnScenery(game, "SceneryHillSmall", t.x, t.y, 48, 19);
        }
        if (t.type == ThingType.Cloud1)
        {
            SpawnScenery(game, "SceneryCloud1", t.x, t.y, 32, 24);
        }
        if (t.type == ThingType.Cloud2)
        {
            SpawnScenery(game, "SceneryCloud2", t.x, t.y, 48, 24);
        }
        if (t.type == ThingType.Cloud3)
        {
            SpawnScenery(game, "SceneryCloud3", t.x, t.y, 64, 24);
        }
        if (t.type == ThingType.Bush1)
        {
            SpawnScenery(game, "SceneryBush1", t.x, t.y, 32, 16);
        }
        if (t.type == ThingType.Bush2)
        {
            SpawnScenery(game, "SceneryBush2", t.x, t.y, 48, 16);
        }
        if (t.type == ThingType.Bush3)
        {
            SpawnScenery(game, "SceneryBush3", t.x, t.y, 64, 16);
        }
        if (t.type == ThingType.Water)
        {
            Entity top = SpawnScenery(game, "SceneryWaterNormalTop", t.x, t.y, 8, 8);
            top.draw.xrepeat = t.width * 2;

            Entity middle = SpawnScenery(game, "SceneryWaterNormalMiddle", t.x, t.y - 4, 8, 8);
            middle.draw.xrepeat = t.width * 2;
            middle.draw.yrepeat = 20;
        }
    }

    bool IsOnScreen(Game game, int x)
    {
        return x * 2 > game.scrollx + game.gameScreenWidth;
    }

    Entity SpawnScenery(Game game, string p, int x, int y, int width, int height)
    {
        Entity e = Spawn(game, p, x, y + height / 2);
        e.draw.width = width;
        e.draw.height = height;
        return e;
    }

    public static Entity Spawn(Game game, string p, float x, float y)
    {
        Entity e = new Entity();
        e.draw = new EntityDraw();
        e.draw.sprite = p;
        e.draw.x = x * 2;
        e.draw.y = 240 - y * 2 - 16 * 2;
        e.draw.z = 1;
        game.AddEntity(e);
        return e;
    }

    public static float GetX(float mapx)
    {
        return mapx * 2;
    }

    public static float GetY(float mapy)
    {
        return 240 - mapy * 2 - 16 * 2;
    }
}

public class SettingApply
{
    public static string Apply(Game game, string sprite)
    {
        SettingType setting = game.setting;
        if (setting == SettingType.Underworld)
        {
            if (sprite == "SolidsBrickNormalNormal")
            {
                sprite = "SolidsBrickUnderworldNormal";
            }
            if (sprite == "SolidsBrickNormalUsed")
            {
                sprite = "SolidsBrickUnderworldUsed";
            }
            if (sprite == "CharactersCoinNormalNormalNormal")
            {
                sprite = "CharactersCoinNormalUnderworldNormal";
            }
            if (sprite == "CharactersCoinNormalNormalTwo")
            {
                sprite = "CharactersCoinNormalUnderworldTwo";
            }
            if (sprite == "CharactersCoinNormalNormalThree")
            {
                sprite = "CharactersCoinNormalUnderworldThree";
            }
            if (sprite == "CharactersGoombaNormal")
            {
                sprite = "CharactersGoombaUnderworld";
            }
            if (sprite == "SolidsDeadGoombaNormal")
            {
                sprite = "SolidsDeadGoombaUnderworld";
            }
            if (sprite == "SolidsBlockNormalNormalNormal")
            {
                sprite = "SolidsBlockUnderworldNormalNormal";
            }
            if (sprite == "SolidsBlockNormalNormalTwo")
            {
                sprite = "SolidsBlockUnderworldNormalTwo";
            }
            if (sprite == "SolidsBlockNormalNormalThree")
            {
                sprite = "SolidsBlockUnderworldNormalThree";
            }
            if (sprite == "SolidsFloorNormal")
            {
                sprite = "SolidsFloorUnderworld";
            }
            if (sprite == "SolidsStoneNormal")
            {
                sprite = "SolidsStoneUnderworld";
            }
            if (sprite == "SolidsBlockNormalUsed")
            {
                sprite = "SolidsBlockUnderworldUsed";
            }
            if (sprite == "CharactersFireFlowerNormalNormal")
            {
                sprite = "CharactersFireFlowerUnderworldNormal";
            }
            if (sprite == "CharactersFireFlowerNormalTwo")
            {
                sprite = "CharactersFireFlowerUnderworldTwo";
            }
            if (sprite == "CharactersFireFlowerNormalThree")
            {
                sprite = "CharactersFireFlowerUnderworldThree";
            }
            if (sprite == "CharactersFireFlowerNormalFour")
            {
                sprite = "CharactersFireFlowerUnderworldFour";
            }
            if (sprite == "CharactersBrickShard1")
            {
                sprite = "CharactersBrickShardUnderworld1";
            }
            if (sprite == "CharactersBrickShard2")
            {
                sprite = "CharactersBrickShardUnderworld2";
            }
            if (sprite == "CharactersBrickShard3")
            {
                sprite = "CharactersBrickShardUnderworld3";
            }
            if (sprite == "CharactersBrickShard4")
            {
                sprite = "CharactersBrickShardUnderworld4";
            }
            if (sprite == "CharactersKoopaStoppedNormalNormal")
            {
                sprite = "CharactersKoopaStoppedUnderworldNormal";
            }
            if (sprite == "CharactersKoopaStoppedNormalTwo")
            {
                sprite = "CharactersKoopaStoppedUnderworldTwo";
            }
            if (sprite == "CharactersKoopaNormalNormalNormalNormalNormal")
            {
                sprite = "CharactersKoopaNormalNormalNormalUnderworldNormal";
            }
            if (sprite == "CharactersKoopaNormalNormalNormalNormalTwo")
            {
                sprite = "CharactersKoopaNormalNormalNormalUnderworldTwo";
            }
            if (sprite == "CharactersPiranhaNormalNormal")
            {
                sprite = "CharactersPiranhaUnderworldNormal";
            }
            if (sprite == "CharactersPiranhaNormalTwo")
            {
                sprite = "CharactersPiranhaUnderworldTwo";
            }
        }
        if (setting == SettingType.Castle)
        {
            if (sprite == "CharactersPiranhaNormalNormal")
            {
                sprite = "CharactersPiranhaCastleNormal";
            }
            if (sprite == "CharactersPiranhaNormalTwo")
            {
                sprite = "CharactersPiranhaCastleTwo";
            }
            if (sprite == "SolidsFloorNormal")
            {
                sprite = "SolidsCastleStoneNormal";
            }
            if (sprite == "SolidsStoneNormal")
            {
                sprite = "SolidsCastleStoneNormal";
            }
            if (sprite == "SceneryWaterNormalMiddle")
            {
                sprite = "SceneryWaterCastleNormalMiddle";
            }
            if (sprite == "SceneryWaterNormalTop")
            {
                sprite = "SceneryWaterCastleNormalTop";
            }
            if (sprite == "SolidsBlockNormalUsed")
            {
                sprite = "SolidsCastleBlock";
            }
        }
        if (setting == SettingType.Underwater)
        {
            if (sprite == "SceneryWaterNormalMiddle")
            {
                sprite = "SceneryWaterUnderwaterMiddle";
            }
            if (sprite == "SceneryWaterNormalTop")
            {
                sprite = "SceneryWaterUnderwaterTop";
            }
        }
        return sprite;
    }
}

public class ActionSpawnThing
{
    public ActionSpawnThing()
    {
        thingType = -1;
        x = 0;
        y = 0;
    }

    internal int thingType;
    internal int x;
    internal int y;

    public void Spawn(Game game)
    {
        int spawnX = x * 2;
        int spawnY = 240 - y * 2 - 16 * 2;
        if (thingType == ThingType.Mushroom
            || thingType == ThingType.Mushroom1Up
            || thingType == ThingType.MushroomDeathly)
        {
            Entity e = new Entity();
            e.draw = new EntityDraw();
            e.draw.x = spawnX;
            e.draw.y = spawnY;
            ScriptMushroom script = new ScriptMushroom();

            if (thingType == ThingType.Mushroom)
            {
                if (game.playerGrowth == 0)
                {
                    script.mushroomType = MushroomType.Mushroom;
                    e.draw.sprite = "CharactersMushroom";
                }
                else
                {
                    script.mushroomType = MushroomType.FireFlower;
                    e.draw.sprite = "CharactersFireFlowerNormalNormal";
                    ScriptAnimation animation = new ScriptAnimation();
                    animation.constAnimCount = 4;
                    animation.constAnimSpeed = 20;
                    animation.constAnims = new string[4];
                    animation.constAnims[0] = "CharactersFireFlowerNormalNormal";
                    animation.constAnims[1] = "CharactersFireFlowerNormalTwo";
                    animation.constAnims[2] = "CharactersFireFlowerNormalThree";
                    animation.constAnims[3] = "CharactersFireFlowerNormalFour";
                    animation.constGlobalTime = true;
                    e.scripts[e.scriptsCount++] = animation;
                }
            }
            if (thingType == ThingType.Mushroom1Up)
            {
                script.mushroomType = MushroomType.Mushroom1Up;
                e.draw.sprite = "CharactersMushroom1Up";
            }
            if (thingType == ThingType.MushroomDeathly)
            {
                script.mushroomType = MushroomType.MushroomDeathly;
                e.draw.sprite = "CharactersMushroomDeathly";
            }

            e.scripts[e.scriptsCount++] = script;
            game.AddEntity(e);
            game.AudioPlay("MushroomAppear");
        }
        if (thingType == ThingType.CoinAnimation)
        {
            Entity e = new Entity();
            e.draw = new EntityDraw();
            e.draw.sprite = "CharactersCoinAnimNormal";
            e.draw.x = spawnX;
            e.draw.y = spawnY;
            e.scripts[e.scriptsCount++] = new ScriptCoinInQuestionBlock();
            game.AddEntity(e);
            
            game.coins++;
            game.AudioPlay("Coin");
        }
        if (thingType == ThingType.Score)
        {
        }
    }
}

public class Spawn_
{
    public const int Score1Up = 0;

    public static void Score(Game game, float x, float y, int score)
    {
        game.score += score;

        Entity e = new Entity();
        e.draw = new EntityDraw();
        e.draw.x = x;
        e.draw.y = y;
        e.draw.z = 3;
        ScriptScore script = new ScriptScore();
        script.score = score;
        e.scripts[e.scriptsCount++] = script;
        game.AddEntity(e);

        if (score == 100)
        {
            e.draw.sprite = "SceneryText100";
        }
        if (score == 200)
        {
            e.draw.sprite = "SceneryText200";
        }
        if (score == 400)
        {
            e.draw.sprite = "SceneryText400";
        }
        if (score == 500)
        {
            e.draw.sprite = "SceneryText500";
        }
        if (score == 800)
        {
            e.draw.sprite = "SceneryText800";
        }
        if (score == 1000)
        {
            e.draw.sprite = "SceneryText1000";
        }
        if (score == 2000)
        {
            e.draw.sprite = "SceneryText2000";
        }
        if (score == 4000)
        {
            e.draw.sprite = "SceneryText4000";
        }
        if (score == 5000)
        {
            e.draw.sprite = "SceneryText5000";
        }
        if (score == 8000)
        {
            e.draw.sprite = "SceneryText8000";
        }
        if (score == Score1Up)
        {
            e.draw.sprite = "SceneryText1Up";
        }
    }

    public static void BrickShards(Game game, float x, float y)
    {
        for (int i = 0; i < 4; i++)
        {
            Entity shard = new Entity();
            shard.draw = new EntityDraw();
            shard.draw.x = x;
            shard.draw.y = y;
            shard.draw.z = 3;
            shard.draw.width = 8;
            shard.draw.height = 8;
            shard.draw.sprite = "CharactersBrickShard1";
            ScriptBrickShard script = new ScriptBrickShard();
            if (i == 0 || i == 1)
            {
                script.velX = -60;
            }
            else
            {
                script.velX = 60;
            }
            if (i == 1 || i == 3)
            {
                script.velY = -150;
            }
            else
            {
                script.velY = 0;
            }
            shard.scripts[shard.scriptsCount++] = script;
            ScriptAnimation animation = new ScriptAnimation();
            animation.constAnimCount = 4;
            animation.constAnims = new string[4];
            animation.constAnims[0] = "CharactersBrickShard1";
            animation.constAnims[1] = "CharactersBrickShard2";
            animation.constAnims[2] = "CharactersBrickShard3";
            animation.constAnims[3] = "CharactersBrickShard4";
            animation.constAnimSpeed = 10;
            shard.scripts[shard.scriptsCount++] = animation;
            game.AddEntity(shard);
        }
    }
}
