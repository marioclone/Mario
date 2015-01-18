public class ScriptCheepCheepFlying : Script
{
    public ScriptCheepCheepFlying()
    {
        velX = 0;
        velY = 0;
        gravity = one * 80 / 100;
        jumpOnEnemy = new JumpOnEnemy();
    }

    internal float velX;
    internal float velY;
    internal float gravity;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        if (game.gamePaused) { return; }
        //if (!IsActiveHelper.IsActive(game, e.draw.x)) { return; }
        jumpOnEnemy.Update(game, entity, Game.ScoreGoomba, dt);
        if (jumpOnEnemy.dead)
        {
            return;
        }
        HelperAttackWithTouch.Update(game, e);
        e.draw.x += velX * dt;
        e.draw.y += velY * dt;
        velY += gravity;
        if (velX > 0)
        {
            e.draw.mirror = MirrorType.MirrorX;
        }
        else
        {
            e.draw.mirror = MirrorType.None;
        }
    }
    JumpOnEnemy jumpOnEnemy;
}

public class JumpOnEnemy
{
    public JumpOnEnemy()
    {
        t = 0;
        float one = 1;
        constDeadTime = one / 2;
    }
    internal bool dead;
    float constDeadTime;
    float t;
    public void Update(Game game, int entity, int score, float dt)
    {
        Entity e = game.entities[entity];

        // If player jumps on enemy, the enemy dies
        if (e.attackablePush != null)
        {
            if ((e.attackablePush.pushed != PushType.None)
                && (!dead))
            {
                e.draw.y += 8;
                if (e.collider != null)
                {
#if CITO
                    delete e.collider;
#endif
                    e.collider = null;
                }
                dead = true;
                t = 0;
                game.AudioPlay("Stomp");
                Spawn_.Score(game, e.draw.x, e.draw.y, score);
            }
        }

        if (dead)
        {
            e.draw.mirror = MirrorType.MirrorY;
            e.draw.y += dt * 100;
        }

        // Remove dead enemy
        if (dead && t > constDeadTime)
        {
            game.DeleteEntity(entity);
        }
    }
}

public class ScriptCheepCheepZone : Script
{
    public ScriptCheepCheepZone()
    {
        stop = 0;
        t = 0;
    }

    internal float stop;
    float t;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        t += dt;
        if (t > 3)
        {
            float spawnx = game.playerx - 96;
            if (spawnx < e.draw.x)
            {
                return;
            }
            if (spawnx > stop)
            {
                return;
            }
            t = 0;
            Entity e2 = new Entity();
            e2.attackablePush = new EntityAttackablePush();
            e2.attackablePush.pushSide = PushSide.TopJumpOnEnemy;
            e2.draw = new EntityDraw();
            e2.draw.x = spawnx + 64 + game.rnd.Next() % 64;
            e2.draw.y = 240;
            ScriptCheepCheepFlying script = new ScriptCheepCheepFlying();
            script.velX = 50 + game.rnd.Next() % 75;
            if (game.rnd.Next() % 100 < 50)
            {
                script.velX = -script.velX;
                e2.draw.x += 192;
            }
            script.velY = -200;
            e2.scripts[e2.scriptsCount++] = script;
            e2.scripts[e2.scriptsCount++] = CheepCheepAnimation.Create(true);
            game.AddEntity(e2);
        }
    }
}

public class RandomCi
{
    internal int _state;

    public int Next()
    {
        _state = (1103515245 * _state + 12345) & 2147483647;
        return _state;
    }
}
