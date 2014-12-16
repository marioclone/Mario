public class ScriptPipeTransport : Script
{
    public ScriptPipeTransport()
    {
        targetLevelOrEntrance = null;
        horizontal = false;
        pipeMiddle = null;
        enteringTime = -1;
    }

    internal string targetLevelOrEntrance;
    internal bool horizontal;
    internal Entity pipeMiddle;
    float enteringTime;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        if (e.attackablePush == null)
        {
            e.attackablePush = new EntityAttackablePush();
            if (horizontal)
            {
                e.attackablePush.pushSide = PushSide.LeftPipeEnter;
            }
            else
            {
                e.attackablePush.pushSide = PushSide.TopPipeKeyDown;
            }
        }

        if (e.attackablePush.pushed != PushType.None
            && enteringTime == -1)
        {
            // Enter pipe
            game.AudioPlay("Pipe");
            enteringTime = 0;
            e.collider = null;
            if (pipeMiddle != null)
            {
                pipeMiddle.collider = null;
            }
        }

        if (enteringTime != -1)
        {
            enteringTime += dt;
        }

        if (enteringTime >= 1)
        {
            enteringTime = -1;
            TransportHelper.Transport(game, targetLevelOrEntrance);
        }
    }
}

public class TransportHelper
{
    public static void Transport(Game game, string targetLevelOrEntrance)
    {
        game.restart = true;

        // Target is level name
        for (int i = 0; i < game.maps.map.levelsCount; i++)
        {
            if (game.maps.map.levels[i].level == targetLevelOrEntrance)
            {
                game.level = targetLevelOrEntrance;
            }
        }

        // Target is entrance name
        for (int i = 0; i < game.maps.map.thingsCount; i++)
        {
            Thing t = game.maps.map.things[i];
            if (t.entrance == targetLevelOrEntrance)
            {
                game.level = t.level;
                game.restartPositionX = t.x * 2;
                if (t.x == 0)
                {
                    game.restartPositionX = 32;
                }
                game.restartPositionY = 240 - t.y * 2 - 16 * 5;
            }
        }
    }
}
