public class ScriptFlagPole : Script
{
    public ScriptFlagPole()
    {
        targetLevelOrEntrance = null;
        flag = null;
        flagY = 0;
        t = -1;
        flagTouchY = 5;
        playerEntityId = -1;
        flagDone = false;
        playerY = 0;
        playerDone = false;
    }

    internal string targetLevelOrEntrance;
    internal Entity flag;
    float flagY;
    float t;
    float flagTouchY;
    int playerEntityId;
    bool flagDone;
    float playerY;
    bool playerDone;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];

        if (e.attackableTouch == null)
        {
            e.attackableTouch = new EntityAttackableTouch();
        }
        flag.draw.x = e.draw.x - 16;
        flag.draw.y = e.draw.y + flagY;

        if (e.attackableTouch.touched)
        {
            if (t == -1)
            {
                t = 0;
                playerEntityId = e.attackableTouch.touchedEntity;
                flagTouchY = game.entities[playerEntityId].draw.y - e.draw.y;
                flagY = 0;
                playerY = flagTouchY;
            }
        }

        if (t != -1)
        {
            t += dt;

            if (!flagDone)
            {
                if (flagY < e.draw.height * e.draw.yrepeat - 32)
                {
                    flagY += one / 2;
                }
                else
                {
                    flagDone = true;
                    t = one / 2;
                }
            }
            if (!playerDone)
            {
                if (playerY < e.draw.height * e.draw.yrepeat - 16 - game.entities[playerEntityId].draw.height)
                {
                    playerY += one / 2;
                }
                else
                {
                    playerDone = true;
                }
            }
            if (!flagDone)
            {
                game.entities[playerEntityId].draw.x = flag.draw.x;
                game.entities[playerEntityId].draw.y = e.draw.y + playerY;
            }
        }

        if (!flagDone && t != -1)
        {
            game.controlsOverride.active = true;
            game.controlsOverride.Clear();
        }

        if (flagDone)
        {
            if (t > 1)
            {
                game.controlsOverride.active = true;
                game.controlsOverride.right = true;
                game.controlsOverride.jump = true;
            }
            if (t > 1 + one / 50)
            {
                game.controlsOverride.jump = false;
            }

            if (t > 2 - one / 4)
            {
                game.controlsOverride.Clear();
            }
            if (t > 2 + one / 4)
            {
                game.entities[playerEntityId].draw.hidden = true;
            }

            if (t > 3)
            {
                game.controlsOverride.active = false;
                t = -1;
                TransportHelper.Transport(game, targetLevelOrEntrance);
            }
        }
    }
}
