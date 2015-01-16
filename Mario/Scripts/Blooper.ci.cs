public class ScriptBlooper : Script
{
    public ScriptBlooper()
    {
        cycleProgress = 0;
        dirLeft = false;
        dirUp = true;
    }

    float cycleProgress;
    bool dirLeft;
    bool dirUp;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        HelperAttackWithTouch.Update(game, e);

        cycleProgress += dt / 2;
        if (cycleProgress >= 1)
        {
            dirLeft = game.playerx < e.draw.x;
            dirUp = game.playery < e.draw.y + 16;
        }
        cycleProgress = cycleProgress % one;
        if (cycleProgress >= one * 50 / 100 && cycleProgress < one * 70 / 100)
        {
            if (e.draw.height == 24)
            {
                e.draw.height = 16;
            }
            e.draw.sprite = "CharactersBlooperNormalSqueeze";
        }
        else
        {
            if (e.draw.height == 16)
            {
                if (CollisionHelper.IsEmpty(game, entity, e.draw.x, e.draw.y, e.draw.width, 24, false, false))
                {
                    e.draw.height = 24;
                }
            }
            e.draw.sprite = "CharactersBlooperNormalNormal";
        }
        float newx = e.draw.x;
        float newy = e.draw.y;
        float speedLeftRight = 40;
        float speedUp = 80;
        float speedDown = 20;
        if (cycleProgress < one * 25 / 100)
        {
            if (dirLeft)
            {
                newx -= dt * speedLeftRight;
            }
            else
            {
                newx += dt * speedLeftRight;
            }
            if (dirUp)
            {
                newy -= dt * speedUp;
            }
            else
            {
                newy += dt * speedDown;
            }
        }
        else
        {
            newy += dt * speedDown;
        }
        if (CollisionHelper.IsEmpty(game, entity, newx, e.draw.y, e.draw.width, 24, false, false))
        {
            e.draw.x = newx;
        }
        else
        {
            dirLeft = !dirLeft;
        }
        if (CollisionHelper.IsEmpty(game, entity, e.draw.x, newy, e.draw.width, 24, false, false)
            && newy >= WaterLevel.WaterSurfaceY)
        {
            e.draw.y = newy;
        }
    }
}

public class WaterLevel
{
    public const int WaterSurfaceY = 40;
}

public class SpawnBlooper
{
    public static void Spawn(Game game, int x, int y)
    {
        Entity e = SystemSpawn.Spawn(game, "CharactersBlooperNormalNormal", x, y);
        e.scripts[e.scriptsCount++] = new ScriptBlooper();
    }
}
