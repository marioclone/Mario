public class ScriptGoomba : Script
{
    public ScriptGoomba()
    {
        t = 0;
        dead = false;
        constAnimSpeed = 4;
        constDeadTime = one / 2;
    }

    float t;
    bool dead;
    float constAnimSpeed;
    float constDeadTime;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        if (game.gamePaused) { return; }
        if (!IsActiveHelper.IsActive(game, e.draw.x)) { return; }
        
        t += dt;
        // If player jumps on goomba, the goomba dies
        if (e.attackablePush != null)
        {
            if ((e.attackablePush.pushed != PushType.None)
                && (!dead))
            {
                e.draw.sprite = "SolidsDeadGoombaNormal";
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
            }
        }

        // Remove dead goomba
        if (dead && t > constDeadTime)
        {
            game.DeleteEntity(entity);
        }

        if (!dead)
        {
            // Attack player who is touching goomba
            for (int i = 0; i < game.entitiesCount; i++)
            {
                Entity e2 = game.entities[i];
                if (e2 == null) { continue; }
                if (e2.attackableTouch == null) { continue; }
                if (Misc.RectIntersect(e.draw.x, e.draw.y, e.draw.width, e.draw.height,
                    e2.draw.x, e2.draw.y, e2.draw.width, e2.draw.height))
                {
                    e2.attackableTouch.touched = true;
                    return;
                }
            }

            // Walk animation
            e.draw.mirrorx = (game.platform.FloatToInt(t * constAnimSpeed) % 2) == 1;
        }
    }
}
