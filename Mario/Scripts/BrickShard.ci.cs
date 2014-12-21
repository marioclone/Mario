public class ScriptBrickShard : Script
{
    public ScriptBrickShard()
    {
        t = 0;
        velX = 0;
        velY = 0;
        constGravity = 5;
    }

    float t;
    internal float velX;
    internal float velY;
    float constGravity;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        t += dt;
        velY += constGravity;
        e.draw.x += velX * dt;
        e.draw.y += velY * dt;
        if (e.draw.y > 240 * 2)
        {
            game.DeleteEntity(entity);
        }
    }
}
