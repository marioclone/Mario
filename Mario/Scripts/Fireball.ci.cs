public class ScriptFireball : Script
{
    public ScriptFireball()
    {
        dirLeft = false;

        t = 0;
        dirY = 1;
        bounced = false;

        constSpeed = 200;
        constAnimSpeed = 20;
        constBounceTime = one * 1 / 10;
        constGravity = 10;
        constBounceVel = 200;
    }

    internal bool dirLeft;

    float t;
    int dirY;
    float velY;
    bool bounced;

    float constSpeed;
    float constAnimSpeed;
    float constBounceTime;
    float constGravity;
    float constBounceVel;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];

        t += dt;

        // Animation
        float stagesCount = 4;
        int stage = game.platform.FloatToInt(t * constAnimSpeed % stagesCount);
        if (stage == 0) { e.draw.sprite = "CharactersFireballNormal"; }
        if (stage == 1) { e.draw.sprite = "CharactersFireballTwo"; }
        if (stage == 2) { e.draw.sprite = "CharactersFireballThree"; }
        if (stage == 3) { e.draw.sprite = "CharactersFireballFour"; }


        // Movement
        if (!bounced)
        {
            velY = constSpeed * dirY;
        }
        else
        {
            velY += constGravity;
        }
        int dirx = 1;
        if (dirLeft)
        {
            dirx = -1;
        }
        float newx = e.draw.x + dt * constSpeed * dirx;
        float newy = e.draw.y + dt * velY;
        if (CollisionHelper.IsEmpty(game, entity, newx, e.draw.y, e.draw.width, e.draw.height, false, false))
        {
            e.draw.x = newx;
        }
        else
        {
            Explosion(game, entity);
            game.AudioPlay("Blockhit");
            return;
        }
        if (CollisionHelper.IsEmpty(game, entity, e.draw.x, newy, e.draw.width, e.draw.height, false, false))
        {
            e.draw.y = newy;
        }
        else
        {
            bounced = true;
            velY = -constBounceVel;
        }

        bool explosion = HelperAttackWithFireball.Update(game, e);
        if (explosion)
        {
            Explosion(game, entity);
        }
    }

    static void Explosion(Game game, int entity)
    {
        Entity e = game.entities[entity];
        Entity firework = new Entity();
        firework.draw = new EntityDraw();
        firework.draw.x = e.draw.x - e.draw.width / 2;
        firework.draw.y = e.draw.y - e.draw.height / 2;
        firework.draw.height = 16;
        firework.draw.width = 16;
        ScriptAnimation script = new ScriptAnimation();
        script.constAnims = new string[3];
        script.constAnims[script.constAnimCount++] = "SolidsFireworkNormal";
        script.constAnims[script.constAnimCount++] = "SolidsFireworkN2";
        script.constAnims[script.constAnimCount++] = "SolidsFireworkN3";
        script.deleteAfter = true;
        script.constAnimSpeed = 20;
        firework.scripts[firework.scriptsCount++] = script;
        game.AddEntity(firework);

        game.DeleteEntity(entity);
    }
}

public class HelperAttackWithFireball
{
    public static bool Update(Game game, Entity e)
    {
        bool explosion = false;
        for (int i = 0; i < game.entitiesCount; i++)
        {
            Entity e2 = game.entities[i];
            if (e2 == null) { continue; }
            if (e2 == e) { continue; }
            if (e2.attackableFireball == null) { continue; }
            if (Misc.RectIntersect(e.draw.x, e.draw.y, e.draw.width, e.draw.height,
                e2.draw.x, e2.draw.y, e2.draw.width * e2.draw.xrepeat, e2.draw.height * e2.draw.yrepeat))
            {
                e2.attackableFireball.attacked = true;
                explosion = true;
            }
        }
        return explosion;
    }
}
