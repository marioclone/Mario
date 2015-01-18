public class ScriptCheepCheep : Script
{
    const int constSpeed = 25;
    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        if (game.gamePaused) { return; }
        if (!IsActiveHelper.IsActive(game, e.draw.x)) { return; }
        HelperAttackWithTouch.Update(game, e);
        e.draw.x -= dt * constSpeed;
    }
}

public class SpawnCheepCheep
{
    public static void Spawn(Game game, int x, int y)
    {
        bool red = x % 16 == 0; // random
        Entity e = SystemSpawn.Spawn(game, "CharactersCheepCheepNormalNormal", x, y);
        e.scripts[e.scriptsCount++] = new ScriptCheepCheep();
        e.scripts[e.scriptsCount++] = CheepCheepAnimation.Create(red);
    }
}

public class CheepCheepAnimation
{
    public static ScriptAnimation Create(bool red)
    {
        ScriptAnimation anim = new ScriptAnimation();
        anim.constAnimCount = 2;
        anim.constAnims = new string[2];
        if (red)
        {
            anim.constAnims[0] = "CharactersCheepCheepRedNormal";
            anim.constAnims[1] = "CharactersCheepCheepRedTwo";
        }
        else
        {
            anim.constAnims[0] = "CharactersCheepCheepNormalNormal";
            anim.constAnims[1] = "CharactersCheepCheepNormalTwo";
        }
        anim.constAnimSpeed = 2;
        return anim;
    }
}
