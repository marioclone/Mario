public class ScriptBrickBumpAnimation : Script
{
    public ScriptBrickBumpAnimation()
    {
        bumpTime = -1;
        constBumpSpeed = 10;
        constBumpForce = 5;
    }

    float bumpTime;
    float constBumpSpeed;
    float constBumpForce;

    public override void Update(Game game, int entity, float dt)
    {
        if (game.gamePaused) { return; }
        Entity e = game.entities[entity];

        if (bumpTime != -1)
        {
            bumpTime += dt;
        }

        if (bumpTime != -1)
        {
            e.draw.yOffset = - Bump(bumpTime * constBumpSpeed) * constBumpForce;
        }

        if (e.attackablePush.pushed == PushType.SmallMario)
        {
            game.AudioPlay("Blockhit");
            bumpTime = 0;
            e.attackablePush.pushed = PushType.None;
        }
    }

    float Bump(float t)
    {
        float one = 1;
        if (t < one / 2) { return t / 2; }
        if (t < 1) { return one - t / 2; }
        return 0;
    }
}
