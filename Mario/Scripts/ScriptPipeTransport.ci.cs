public class ScriptPipeTransport : Script
{
    public ScriptPipeTransport()
    {
        targetLevel = null;
        enteringTime = -1;
    }

    internal string targetLevel;
    float enteringTime;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        if (e.attackablePush == null)
        {
            e.attackablePush = new EntityAttackablePush();
            e.attackablePush.pushSide = PushSide.TopPipeKeyDown;
        }
        if (e.attackablePush.pushed != PushType.None)
        {
            // Enter pipe
            if (enteringTime == -1)
            {
                enteringTime = 0;
                game.AudioPlay("Pipe");
                game.level = targetLevel;
                game.restart = true;
            }
        }
    }
}
