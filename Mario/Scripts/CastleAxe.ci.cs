public class ScriptCastleAxe : Script
{
    public ScriptCastleAxe()
    {
        deleteTime = -1;
        constDeleteSpeed = 120;
        targetLevelOrEntrance = null;
    }

    float deleteTime;
    float constDeleteSpeed;
    internal string targetLevelOrEntrance;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        if (e.attackableTouch.touched && deleteTime == -1)
        {
            deleteTime = 0;
        }
        if (deleteTime != -1)
        {
            deleteTime += dt;
            for (int i = 0; i < game.entitiesCount; i++)
            {
                Entity e2 = game.entities[i];
                if (e2 == null) { continue; }
                if (e2.draw == null) { continue; }
                if (e2.draw.x >= e.draw.x - deleteTime * constDeleteSpeed)
                {
                    if (e2.IsCastleBridge)
                    {
                        if (e2 == e)
                        {
                            e.draw.hidden = true;
                        }
                        else
                        {
                            game.AudioPlay("Bridgebreak");
                            game.DeleteEntity(i);
                        }
                    }
                }
            }
        }

        bool bridgeDestroyed = true;
        for (int i = 0; i < game.entitiesCount; i++)
        {
            Entity e2 = game.entities[i];
            if (e2 == null) { continue; }
            if (e2 == e) { continue; }
            if (e2.draw == null) { continue; }
            if (e2.IsCastleBridge)
            {
                bridgeDestroyed = false;
            }
        }

        game.scrollBlock = false;
        if (IsActiveHelper.IsActive(game, e.draw.x + 96))
        {
            if (!bridgeDestroyed)
            {
                game.scrollBlock = true;
            }
        }

        if (deleteTime > 10)
        {
            TransportHelper.Transport(game, targetLevelOrEntrance);
        }
    }
}

public class SpawnCastleAxe
{
    public static Entity Spawn(Game game, int x, int y, string transport)
    {
        Entity e = SystemSpawn.Spawn(game, "SolidsCastleAxeNormal", x, y);
        e.attackableTouch = new EntityAttackableTouch();
        ScriptCastleAxe script = new ScriptCastleAxe();
        script.targetLevelOrEntrance = transport;
        e.scripts[e.scriptsCount++] = script;
        return e;
    }
}
