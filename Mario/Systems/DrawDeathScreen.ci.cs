public class DrawDeathScreen : GameSystem
{
    public DrawDeathScreen()
    {
        deathScreenWorld = null;
        deathScreenWorldValue = null;
        deathScreenDrawMario = null;
        deathScreenLifesLeft = null;
        deathScreen = false;
        deathScreenTime = 0;
    }

    Entity deathScreenWorld;
    Entity deathScreenWorldValue;
    Entity deathScreenDrawMario;
    Entity deathScreenLifesLeft;
    bool deathScreen;
    float deathScreenTime;

    public override void Update(Game game, float dt)
    {
        // Show death screen
        if (game.gameShowDeathScreen)
        {
            game.gameShowDeathScreen = false;

            game.lifesLeft--;
            deathScreen = true;
            deathScreenTime = 0;

            for (int i = 0; i < game.entitiesCount; i++)
            {
                if (game.entities[i] != null)
                {
                    if (game.entities[i].draw != null)
                    {
                        game.entities[i].draw.hidden = true;
                    }
                }
            }
            game.entitiesCount = 0;

            game.backgroundColor = Misc.ColorFromArgb(255, 0, 0, 0);

            deathScreenWorld = new Entity();
            ScriptDrawText world = CreateText(game, 5, deathScreenWorld);
            world.text[0] = CharType.CharW;
            world.text[1] = CharType.CharO;
            world.text[2] = CharType.CharR;
            world.text[3] = CharType.CharL;
            world.text[4] = CharType.CharD;
            world.x = 100;
            world.y = 100;

            deathScreenWorldValue = new Entity();
            ScriptDrawText worldValue = CreateText(game, 3, deathScreenWorldValue);
            worldValue.text[0] = CharType.Char1;
            worldValue.text[1] = CharType.CharMinus;
            worldValue.text[2] = CharType.Char0 + 1; // (game.level + 1);
            worldValue.x = 164;
            worldValue.y = 100;

            deathScreenDrawMario = new Entity();
            deathScreenDrawMario.draw = new EntityDraw();
            deathScreenDrawMario.draw.x = 100;
            deathScreenDrawMario.draw.y = 132;
            deathScreenDrawMario.draw.sprite = "CharactersPlayerNormalNormalNormalNormal";
            deathScreenDrawMario.draw.absoluteScreenPosition = true;
            game.AddEntity(deathScreenDrawMario);

            deathScreenLifesLeft = new Entity();
            ScriptDrawText lifesLeft = CreateText(game, 4, deathScreenLifesLeft);
            lifesLeft.text[0] = CharType.CharTimes;
            lifesLeft.text[1] = CharType.CharSpace;
            lifesLeft.text[2] = CharType.CharSpace;
            lifesLeft.text[3] = CharType.Char0 + (game.lifesLeft);
            lifesLeft.x = 164;
            lifesLeft.y = 132;
        }

        // Hide death screen
        if (deathScreen)
        {
            deathScreenTime += dt;
            if (deathScreenTime > 3)
            {
                deathScreen = false;
                game.DeleteEntity_(deathScreenWorld);
                game.DeleteEntity_(deathScreenWorldValue);
                game.DeleteEntity_(deathScreenDrawMario);
                game.DeleteEntity_(deathScreenLifesLeft);
                game.restart = true;
                game.gameStarted = false;
                game.gamePaused = true;
            }
        }
    }

    ScriptDrawText CreateText(Game game, int length, Entity e)
    {
        ScriptDrawText s = new ScriptDrawText();
        s.text = new int[length];
        s.textLength = length;
        e.scripts[e.scriptsCount++] = s;
        game.AddEntity(e);
        return s;
    }
}
