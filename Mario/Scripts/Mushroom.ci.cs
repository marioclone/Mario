public class ScriptMushroom : Script
{
    public ScriptMushroom()
    {
        t = 0;
        done = false;
        constGrowTime = one / 2;
        mushroomType = MushroomType.Mushroom;
    }

    float t;
    bool done;
    float constGrowTime;
    const int constScoreMushroom = 1000;
    internal MushroomType mushroomType;

    public override void Update(Game game, int entity, float dt)
    {
        if (game.gamePaused) { return; }
        t += dt;
        Entity e = game.entities[entity];
        e.draw.z = 0; // Draw growing mushroom behind question block

        // Grow mushroom
        float growProgress = t / constGrowTime;
        if (growProgress < 1)
        {
            e.draw.yOffset = 16 - growProgress * 16;
        }
        else
        {
            e.draw.yOffset = 0;
            if (!done)
            {
                done = true;
                if (mushroomType == MushroomType.Mushroom
                    || mushroomType == MushroomType.Mushroom1Up
                    || mushroomType == MushroomType.MushroomDeathly)
                {
                    ScriptMoving moving = new ScriptMoving();
                    moving.direction = 1;
                    e.scripts[e.scriptsCount++] = moving;
                }
            }
        }

        // Grow player who is touching the mushroom.
        for (int i = 0; i < game.entitiesCount; i++)
        {
            Entity e2 = game.entities[i];
            if (e2 == null) { continue; }
            if (e2.growable == null) { continue; }
            if (Misc.RectIntersect(e.draw.x, e.draw.y, e.draw.width, e.draw.height,
                e2.draw.x, e2.draw.y, e2.draw.width, e2.draw.height))
            {
                e2.growable.grow = true;
                game.score += constScoreMushroom;
                game.DeleteEntity(entity);
                return;
            }
        }
    }
}

public enum MushroomType
{
    Mushroom,
    Mushroom1Up,
    MushroomDeathly,
    FireFlower
}
