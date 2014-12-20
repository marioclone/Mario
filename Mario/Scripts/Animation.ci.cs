public class ScriptAnimation : Script
{
    public ScriptAnimation()
    {
        t = 0;
        constAnimSpeed = 1;
        constAnimCount = 0;
        deleteAfter = false;
    }

    float t;

    internal float constAnimSpeed;
    internal int constAnimCount;
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
        int stage = (game.platform.FloatToInt(t * constAnimSpeed) % constAnimCount);
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
