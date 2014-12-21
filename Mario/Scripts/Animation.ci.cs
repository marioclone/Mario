public class ScriptAnimation : Script
{
    public ScriptAnimation()
    {
        t = 0;
        constAnimSpeed = 1;
        constAnimCount = 0;
        constGlobalTime = false;
        deleteAfter = false;
    }

    float t;

    internal float constAnimSpeed;
    internal int constAnimCount;
    internal bool constGlobalTime;
    internal string[] constAnims;
    internal bool deleteAfter;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        t += dt;
        if (constAnimCount == 0)
        {
            return;
        }
        float time;
        if (constGlobalTime)
        {
            time = game.t;
        }
        else
        {
            time = t;
        }
        int stage = (game.platform.FloatToInt(time * constAnimSpeed) % constAnimCount);
        e.draw.sprite = constAnims[stage];
        if (deleteAfter)
        {
            if (t > constAnimCount / constAnimSpeed)
            {
                game.DeleteEntity(entity);
            }
        }
    }
}
