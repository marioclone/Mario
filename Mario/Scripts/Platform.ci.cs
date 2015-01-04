public class ScriptPlatform : Script
{
    public ScriptPlatform()
    {
        start = 0;
        end = 0;
        constUpDownSpeed = one / 3;
        constSpeed = 64;
    }

    float constUpDownSpeed;
    float constSpeed;
    internal PlatformDirection direction;
    internal float start;
    internal float end;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];

        float newx = e.draw.x;
        float newy = e.draw.y;
        if (direction == PlatformDirection.Down)
        {
            newy = e.draw.y + constSpeed * dt;
        }
        if (direction == PlatformDirection.Up)
        {
            newy = e.draw.y - constSpeed * dt;
        }
        if (direction == PlatformDirection.Floating)
        {
            newy = PlatformPosition(game, game.t, start, end, constUpDownSpeed);
        }
        if (direction == PlatformDirection.Sliding)
        {
            newx = PlatformPosition(game, game.t, start, end, constUpDownSpeed);
        }

        // Down
        if (newy > e.draw.y)
        {
        }
        // Up
        if (newy < e.draw.y)
        {
            for (int i = 0; i < game.entitiesCount; i++)
            {
                if (i == entity) { continue; }
                Entity e2 = game.entities[i];
                if (e2 == null) { continue; }
                if (e2.draw == null) { continue; }
                if (Misc.RectIntersect(e.draw.x, newy, e.draw.width * e.draw.xrepeat, e.draw.height * e.draw.yrepeat,
                    e2.draw.x, e2.draw.y, e2.draw.width * e2.draw.xrepeat, e2.draw.height * e2.draw.yrepeat))
                {
                    e2.draw.y -= constSpeed * dt + one;
                }
            }
            
        }
        // Left
        if (newx < e.draw.x)
        {
            for (int i = 0; i < game.entitiesCount; i++)
            {
                if (i == entity) { continue; }
                Entity e2 = game.entities[i];
                if (e2 == null) { continue; }
                if (e2.draw == null) { continue; }
                if (Misc.RectIntersect(newx, e.draw.y, e.draw.width * e.draw.xrepeat, e.draw.height * e.draw.yrepeat,
                    e2.draw.x, e2.draw.y + 1, e2.draw.width * e2.draw.xrepeat, e2.draw.height * e2.draw.yrepeat))
                {
                    e2.draw.x -= e.draw.x - newx;
                }
            }
        }
        // Right
        if (newx > e.draw.x)
        {
            for (int i = 0; i < game.entitiesCount; i++)
            {
                if (i == entity) { continue; }
                Entity e2 = game.entities[i];
                if (e2 == null) { continue; }
                if (e2.draw == null) { continue; }
                if (Misc.RectIntersect(newx, e.draw.y, e.draw.width * e.draw.xrepeat, e.draw.height * e.draw.yrepeat,
                    e2.draw.x, e2.draw.y + 1, e2.draw.width * e2.draw.xrepeat, e2.draw.height * e2.draw.yrepeat))
                {
                    e2.draw.x += newx - e.draw.x;
                }
            }
        }

        e.draw.x = newx;
        e.draw.y = newy;

        if (direction == PlatformDirection.Up || direction == PlatformDirection.Down)
        {
            if (e.draw.y < -8 || e.draw.y > 240 + 8)
            {
                game.DeleteEntity(entity);
            }
        }
    }

    public static float PlatformPosition(Game game, float t, float start_, float end_, float upDownSpeed_)
    {
        float maxY = end_ - start_;
        int a = game.platform.FloatToInt(t * upDownSpeed_);
        float b = Smoothstep(t * upDownSpeed_ - a) * maxY;
        if (a % 2 == 1)
        {
            b = maxY - b;
        }
        return start_ + b;
    }

    static float Smoothstep(float x)
    {
        return x * x * (3 - 2 * x);
    }
}

public enum PlatformDirection
{
    None,
    Up,
    Down,
    Floating,
    Sliding,
    Falling
}

public class SpawnPlatform
{
    internal static void Spawn(Game game, float x, float y, PlatformDirection direction, float start, float end)
    {
        Entity p = SystemSpawn.Spawn(game, "SolidsPlatformNormal", x, y);
        p.draw.xrepeat = 6;
        if (direction == PlatformDirection.Down)
        {
            p.draw.y = 0;
        }
        else if (direction == PlatformDirection.Up)
        {
            p.draw.y = 240;
        }
        if (direction == PlatformDirection.Floating)
        {
        }
        p.draw.width = 8;
        p.draw.height = 8;
        p.collider = new EntityCollider();
        ScriptPlatform script = new ScriptPlatform();
        script.direction = direction;
        script.start = start;
        script.end = end;
        p.scripts[p.scriptsCount++] = script;
    }
}
