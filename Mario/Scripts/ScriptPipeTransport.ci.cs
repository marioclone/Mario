public class ScriptPipeTransport : Script
{
    public ScriptPipeTransport()
    {
        targetLevelOrEntrance = null;
        enteringTime = -1;
        horizontal = false;
    }

    internal string targetLevelOrEntrance;
    internal bool horizontal;
    float enteringTime;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        if (e.attackablePush == null)
        {
            e.attackablePush = new EntityAttackablePush();
            if (horizontal)
            {
                e.attackablePush.pushSide = PushSide.Left;
            }
            else
            {
                e.attackablePush.pushSide = PushSide.TopPipeKeyDown;
            }
        }
        if (e.attackablePush.pushed != PushType.None)
        {
            // Enter pipe
            if (enteringTime == -1)
            {
                enteringTime = 0;
                game.AudioPlay("Pipe");
                
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
                        game.restartPositionY = 240 - t.y * 2 - 16 * 5;
                    }
                }
            }
        }
    }
}
