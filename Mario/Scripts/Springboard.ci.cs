public class ScriptSpringboard : Script
{
    public ScriptSpringboard()
    {
        t = -1;
        constSpeed = 10;
    }

    float t;
    float constSpeed;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        if (t != -1)
        {
            t += dt;
        }
        if (e.attackablePush.pushed != PushType.None)
        {
            if (t == -1)
            {
                t = 0;
            }
            e.attackablePush.pushed = PushType.None;
        }
        int stage = 0;
        if (t > (one * 0 / 10) / constSpeed) { stage = 1; }
        if (t > (one * 10 / 10) / constSpeed) { stage = 2; }
        if (t > (one * 20 / 10) / constSpeed) { stage = 1; }
        if (t > (one * 33 / 10) / constSpeed) { stage = 0; t = -1; }

        if (stage == 0)
        {
            e.draw.sprite = "SolidsSpringboardOne";
            if (e.draw.height == 24)
            {
                e.draw.y -= 8;
            }
            e.draw.height = 32;
        }
        if (stage == 1)
        {
            e.draw.sprite = "SolidsSpringboardTwo";
            if (e.draw.height == 32)
            {
                e.draw.y += 8;
            }
            if (e.draw.height == 16)
            {
                e.draw.y -= 8;
            }
            e.draw.height = 24;
        }
        if (stage == 2)
        {
            e.draw.sprite = "SolidsSpringboardThree";
            if (e.draw.height == 24)
            {
                e.draw.y += 8;
            }
            e.draw.height = 16;
        }
    }
}

public class SpawnSpringboard
{
    public static void Spawn(Game game, int x, int y)
    {
        Entity e = SystemSpawn.Spawn(game, "SolidsSpringboardOne", x, y + 2);
        e.draw.height = 32;
        e.collider = new EntityCollider();
        e.attackablePush = new EntityAttackablePush();
        e.attackablePush.pushSide = PushSide.TopJumpOnEnemy;
        e.IsSpringboard = true;
        e.scripts[e.scriptsCount++] = new ScriptSpringboard();
    }
}
