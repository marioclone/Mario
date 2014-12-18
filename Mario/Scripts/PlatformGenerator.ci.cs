public class PlatformGenerator : Script
{
    public PlatformGenerator()
    {
        t = 0;
        constGenerateEverySeconds = 2;
    }

    float t;
    float constGenerateEverySeconds;
    internal int direction;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        t += dt;
        if (t > constGenerateEverySeconds)
        {
            t = 0;
            Entity p = new Entity();
            p.draw = new EntityDraw();
            p.draw.x = e.draw.x;
            p.draw.xrepeat = 6;
            if (direction >= 0)
            {
                p.draw.y = 0;
            }
            else
            {
                p.draw.y = 240;
            }
            p.draw.sprite = "SolidsPlatformNormal";
            p.draw.width = 8;
            p.draw.height = 8;
            p.collider = new EntityCollider();
            ScriptPlatform script = new ScriptPlatform();
            script.direction = direction;
            p.scripts[p.scriptsCount++] = script;
            game.AddEntity(p);
        }
    }
}
