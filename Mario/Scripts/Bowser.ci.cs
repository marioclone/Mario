public class ScriptBowser : Script
{
    public ScriptBowser()
    {
        fireTime = 0;
        constFireTimeMax = 2;
        fallSoundPlayed = false;
        afterFallTime = -1;
        castleEndPlayed = false;
    }

    float fireTime;
    float constFireTimeMax;
    bool fallSoundPlayed;
    float afterFallTime;
    bool castleEndPlayed;

    public override void Update(Game game, int entity, float dt)
    {
        if (game.gamePaused) { return; }

        Entity e = game.entities[entity];

        if (!IsActiveHelper.IsActiveBowser(game, e.draw.x))
        {
            return;
        }

        float oldx = e.draw.x;
        float oldy = e.draw.y;

        float vely = 100;

        float newx = e.draw.x;
        float newy = e.draw.y + vely * dt;

        // Move vertically (down)
        if (CollisionHelper.IsEmpty(game, entity, oldx, newy, e.draw.width, e.draw.height, true, false))
        {
            e.draw.y = newy;
        }

        // Fire
        fireTime += dt;
        if (fireTime > constFireTimeMax && afterFallTime == -1)
        {
            fireTime = 0;
            SpawnBowserFire.Spawn(game, e.draw.x - 16, e.draw.y + 8);
            game.AudioPlay("Fire");
        }

        if (e.draw.y > 200)
        {
            if (!fallSoundPlayed)
            {
                game.AudioPlay("BowserFall");
                fallSoundPlayed = true;
                afterFallTime = 0;
            }
        }
        if (afterFallTime != -1)
        {
            afterFallTime += dt;
            if (afterFallTime > 1)
            {
                if (!castleEndPlayed)
                {
                    castleEndPlayed = true;
                    game.audio.audioPlayMusic = "";
                    game.AudioPlay("CastleEnd");
                }
            }
        }

        HelperAttackWithTouch.Update(game, e);
    }
}

public class ScriptBowserFire : Script
{
    public ScriptBowserFire()
    {
        constSpeed = 100;
    }

    float constSpeed;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        e.draw.x -= constSpeed * dt;
        HelperAttackWithTouch.Update(game, e);
    }
}

public class SpawnBowserFire
{
    public static Entity Spawn(Game game, float realx, float realy)
    {
        Entity e = new Entity();
        e.draw = new EntityDraw();
        e.draw.sprite = "CharactersBowserFire";
        e.draw.x = realx;
        e.draw.y = realy;
        e.draw.width = 24;
        e.draw.height = 8;
        e.scripts[e.scriptsCount++] = new ScriptBowserFire();
        game.AddEntity(e);
        return e;
    }
}

public class SpawnBowser
{
    public static void Spawn(Game game, int x, int y)
    {
        Entity e = SystemSpawn.Spawn(game, "CharactersBowserNormal", x, y);
        e.draw.width = 32;
        e.draw.height = 32;
        e.scripts[e.scriptsCount++] = new ScriptBowser();
    }
}
