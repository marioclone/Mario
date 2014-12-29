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
            PlatformDirection dir;
            if (direction >= 0)
            {
                dir = PlatformDirection.Down;
            }
            else
            {
                dir = PlatformDirection.Up;
            }
            SpawnPlatform.Spawn(game, e.draw.x / 2, 0, dir, 0, 0);
        }
    }
}
