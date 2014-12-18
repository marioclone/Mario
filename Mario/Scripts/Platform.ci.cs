public class ScriptPlatform : Script
{
    public ScriptPlatform()
    {
        constSpeed = 64;
    }

    float constSpeed;
    internal int direction;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        if (direction >= 0)
        {
            // down
            float newy = e.draw.y + constSpeed * dt;
            e.draw.y = newy;
        }
        else
        {
            // up
            float newy = e.draw.y - constSpeed * dt;
            for (int i = 0; i < game.entitiesCount; i++)
            {
                if (i == entity) { continue; }
                Entity e2 = game.entities[i];
                if (e2 == null) { continue; }
                if (e2.draw == null) { continue; }
                if (Misc.RectIntersect(e.draw.x, newy, e.draw.width * e.draw.xrepeat, e.draw.height * e.draw.yrepeat,
                    e2.draw.x, e2.draw.y, e2.draw.width * e2.draw.xrepeat, e2.draw.height * e2.draw.yrepeat))
                {
                    e2.draw.y -= constSpeed * dt + one / 2;
                }
            }
            e.draw.y = newy;
        }
        if (e.draw.y < -8)
        {
            game.DeleteEntity(entity);
        }
    }
}
