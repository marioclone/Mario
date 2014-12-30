public class ScriptCastleBlockFireball : Script
{
    public ScriptCastleBlockFireball()
    {
        startx = 0;
        starty = 0;
        direction = -1;
        r = 0;
        constSpeed = one * 18 / 10;
    }

    internal float startx;
    internal float starty;
    internal float direction;
    internal float r;
    float constSpeed;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];

        e.draw.x = startx + game.platform.MathSin(game.t * -1 * direction * constSpeed) * r * 8;
        e.draw.y = starty + game.platform.MathSin(Misc.GetPi() / 2 - game.t * -1 * direction * constSpeed) * r * 8;

        HelperAttackWithTouch.Update(game, e);
    }
}

public class SpawnCastleBlock
{
    public static void Spawn(Game game, int x, int y, int count, float speed, float direction)
    {
        Entity castleBlock = SystemSpawn.Spawn(game, "SolidsCastleBlock", x, y);
        castleBlock.collider = new EntityCollider();

        for (int i = 0; i < count; i++)
        {
            Entity fireball = SystemSpawn.Spawn(game, "CharactersFireballNormal", x + 2, y - 2);
            fireball.draw.width = 8;
            fireball.draw.height = 8;
            fireball.draw.z = 2;
            ScriptCastleBlockFireball script = new ScriptCastleBlockFireball();
            script.startx = fireball.draw.x;
            script.starty = fireball.draw.y;
            //if (speed != 0)
            //{
            //    script.speed = speed;
            //}
            if (direction != 0)
            {
                script.direction = direction;
            }
            script.r = i;
            fireball.scripts[fireball.scriptsCount++] = script;
            fireball.scripts[fireball.scriptsCount++] = FireballAnimation();
        }
    }
    
    public static ScriptAnimation FireballAnimation()
    {
        ScriptAnimation script = new ScriptAnimation();
        script.constAnimCount = 4;
        script.constAnims = new string[4];
        script.constAnims[0] = "CharactersFireballNormal";
        script.constAnims[1] = "CharactersFireballTwo";
        script.constAnims[2] = "CharactersFireballThree";
        script.constAnims[3] = "CharactersFireballFour";
        script.constAnimSpeed = 20;
        script.constGlobalTime = true;
        return script;
    }
}
