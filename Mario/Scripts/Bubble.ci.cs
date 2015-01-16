public class ScriptBubble : Script
{
    public ScriptBubble()
    {
        t = 0;
    }

    float t;

    public override void Update(Game game, int entity, float dt)
    {
        t += dt;
        Entity e = game.entities[entity];
        e.draw.y -= Misc.Abs(game.platform.MathSin(t * 10)) / 4 + 20 * dt;
        if (e.draw.y < WaterLevel.WaterSurfaceY)
        {
            game.DeleteEntity(entity);
        }
    }
}

public class ScriptSpawnBubble : Script
{
    public ScriptSpawnBubble()
    {
        t = 0;
        n = 0;
        float one = 1;
        constSpawnTime = one * 12 / 10;
        constSpawnTime2 = one * 20 / 10;
    }

    float t;
    float constSpawnTime;
    float constSpawnTime2;
    int n;

    public override void Update(Game game, int entity, float dt)
    {
        if (game.setting != SettingType.Underwater)
        {
            return;
        }
        float spawnTime;
        if (n % 3 == 0)
        {
            spawnTime = constSpawnTime2;
        }
        else
        {
            spawnTime = constSpawnTime;
        }
        Entity e = game.entities[entity];
        t += dt;
        while (t > spawnTime)
        {
            n++;
            t = 0;
            Entity e2 = new Entity();
            e2.draw = new EntityDraw();
            e2.draw.sprite = "CharactersBubble";
            e2.draw.x = e.draw.x + e.draw.width / 2 + 2;
            e2.draw.y = e.draw.y;
            e2.draw.width = 4;
            e2.draw.height = 4;
            e2.draw.z = 3;
            e2.scripts[e2.scriptsCount++] = new ScriptBubble();
            game.AddEntity(e2);
        }
    }
}
