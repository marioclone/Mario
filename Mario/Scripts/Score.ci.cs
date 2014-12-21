public class ScriptScore : Script
{
    public ScriptScore()
    {
        t = 0;
        score = 100;
    }

    float t;

    internal int score;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        t += dt;

        float maxTime = one * 25 / 100;

        if (t < maxTime)
        {
            e.draw.y -= dt * 16 / maxTime;
        }
        else
        {
            game.DeleteEntity(entity);
        }
    }
}
