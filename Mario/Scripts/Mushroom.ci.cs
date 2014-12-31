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
                if (mushroomType == MushroomType.Mushroom
                    || mushroomType == MushroomType.FireFlower)
                {
                    Spawn_.Score(game, e.draw.x, e.draw.y, Game.ScoreMushroom);
                }
                else if (mushroomType == MushroomType.Mushroom1Up)
                {
                    Spawn_.Score(game, e.draw.x, e.draw.y, Spawn_.Score1Up);
                }
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

public class SpawnMushroom
{
    public static void Spawn(Game game, int x, int y, int thingType)
    {
        int spawnX = x * 2;
        int spawnY = 240 - y * 2 - 16 * 2;

        Entity e = new Entity();
        e.draw = new EntityDraw();
        e.draw.x = spawnX;
        e.draw.y = spawnY;
        e.attackableBump = new EntityAttackableBump();
        ScriptMushroom script = new ScriptMushroom();

        if (thingType == ThingType.Mushroom)
        {
            if (game.playerGrowth == 0)
            {
                script.mushroomType = MushroomType.Mushroom;
                e.draw.sprite = "CharactersMushroom";
            }
            else
            {
                script.mushroomType = MushroomType.FireFlower;
                e.draw.sprite = "CharactersFireFlowerNormalNormal";
                ScriptAnimation animation = new ScriptAnimation();
                animation.constAnimCount = 4;
                animation.constAnimSpeed = 20;
                animation.constAnims = new string[4];
                animation.constAnims[0] = "CharactersFireFlowerNormalNormal";
                animation.constAnims[1] = "CharactersFireFlowerNormalTwo";
                animation.constAnims[2] = "CharactersFireFlowerNormalThree";
                animation.constAnims[3] = "CharactersFireFlowerNormalFour";
                animation.constGlobalTime = true;
                e.scripts[e.scriptsCount++] = animation;
            }
        }
        if (thingType == ThingType.Mushroom1Up)
        {
            script.mushroomType = MushroomType.Mushroom1Up;
            e.draw.sprite = "CharactersMushroom1Up";
        }
        if (thingType == ThingType.MushroomDeathly)
        {
            script.mushroomType = MushroomType.MushroomDeathly;
            e.draw.sprite = "CharactersMushroomDeathly";
        }

        e.scripts[e.scriptsCount++] = script;
        game.AddEntity(e);
        game.AudioPlay("MushroomAppear");
    }
}
