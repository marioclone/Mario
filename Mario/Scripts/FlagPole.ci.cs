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
        scoreDoneTime = -1;
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
    float scoreDoneTime;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];

        if (e.attackableTouch == null)
        {
            e.attackableTouch = new EntityAttackableTouch();
            e.attackableTouch.attackableByPlayer = true;
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
                game.audio.audioPlayMusic = "";
                game.AudioPlay("LevelEnd");
                game.entities[playerEntityId].flagpoleClimbing = new EntityFlagpoleClimbing();
                game.entities[playerEntityId].flagpoleClimbing.startY = game.platform.FloatToInt(flagTouchY);
                game.entities[playerEntityId].flagpoleClimbing.endY = e.draw.y + e.draw.height * e.draw.yrepeat - 16 - game.entities[playerEntityId].draw.height;
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
        }

        if (playerEntityId != -1)
        {
            if (game.entities[playerEntityId].flagpoleClimbing != null)
            {
                game.entities[playerEntityId].flagpoleClimbing.flagDone = flagDone;
            }
        }

        if (!flagDone && t != -1)
        {
            game.controlsOverrideActive = true;
            game.controlsOverride.Clear();
        }

        if (flagDone)
        {
            if (t > 1)
            {
                game.controlsOverrideActive = true;
                game.controlsOverride.right = true;
            }

            if (t > 2 - one / 3)
            {
                game.controlsOverride.Clear();
            }
            if (t > 2 - one / 4)
            {
                game.entities[playerEntityId].draw.hidden = true;

                if (game.timeLeft > 0)
                {
                    game.timeLeft -= 1;
                    game.score += 50;
                    if (game.platform.FloatToInt(game.timeLeft) % 5 == 0)
                    {
                        game.AudioPlay("Scorering");
                    }
                }
                else
                {
                    if (scoreDoneTime == -1)
                    {
                        scoreDoneTime = t;
                    }
                }
            }

            if (scoreDoneTime != -1 && t > scoreDoneTime + 2)
            {
                game.controlsOverrideActive = false;
                t = -1;
                TransportHelper.Transport(game, targetLevelOrEntrance);
            }
        }
    }
}
