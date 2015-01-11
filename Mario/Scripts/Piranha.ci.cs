public class ScriptPiranha : Script
{
    public ScriptPiranha()
    {
        t = 0;
        constAnimSpeed = 4;
        startY = -1;
        dead = false;
        constCycleSeconds = 5;
        d = new DeadFromFireballOrBump();
    }

    float t;
    DeadFromFireballOrBump d;
    float constAnimSpeed;
    float startY;
    float constCycleSeconds;
    bool dead;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        if (Misc.Abs(game.playerx - e.draw.x) < 50)
        {
            // Don't emerge when player is near
            if (!IsHiddenInsidePipe())
            {
                t += dt;
            }
        }
        else
        {
            t += dt;
        }

        if (startY == -1)
        {
            startY = e.draw.y;
        }
        if (!dead)
        {
            e.draw.y = startY + GetY(t / constCycleSeconds) * 24;
        }
        if (t >= constCycleSeconds)
        {
            t = 0;
        }

        // Animation
        if ((game.platform.FloatToInt(t * constAnimSpeed) % 2) == 1)
        {
            e.draw.sprite = "CharactersPiranhaNormalNormal";
        }
        else
        {
            e.draw.sprite = "CharactersPiranhaNormalTwo";
        }
        if (!IsHiddenInsidePipe())
        {
            HelperAttackWithTouch.Update(game, e);
        }
        dead = d.Update(game, entity, dt, Game.ScorePiranha);
    }

    bool IsHiddenInsidePipe()
    {
        return !(t >= (one / 4) * constCycleSeconds);
    }

    float GetY(float time)
    {
        if (time < one * 1 / 4) { return 1; }
        if (time < one * 2 / 4) { return 1 - (time - one * 1 / 4) * 4; }
        if (time < one * 3 / 4) { return 0; }
        return (time - one * 3 / 4) * 4;
    }
}

public class SpawnPiranha
{
    public static void Spawn(Game game, int x, int y)
    {
        Entity e = SystemSpawn.Spawn(game, "CharactersPiranhaNormalNormal", x, y);
        e.draw.height = 24;
        e.attackableFireball = new EntityAttackableFireball();
        e.scripts[e.scriptsCount++] = new ScriptPiranha();
    }
}
