public class ScriptBrickBump : Script
{
    public ScriptBrickBump()
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

        // Brick bump animation
        if (bumpTime != -1)
        {
            bumpTime += dt;
        }
        if (bumpTime != -1)
        {
            e.draw.yOffset = -Bump(bumpTime * constBumpSpeed) * constBumpForce;
        }
        
        if (e.attackablePush.pushed == PushType.SmallMario
            || e.attackablePush.pushed == PushType.BigMario)
        {
            // Bump entities above brick
            for (int i = 0; i < game.entitiesCount; i++)
            {
                Entity e2 = game.entities[i];
                if (e2 == null) { continue; }
                if (e2.attackableBump == null) { continue; }
                if (Misc.RectIntersect(e.draw.x, e.draw.y - 4, e.draw.width, e.draw.height,
                    e2.draw.x, e2.draw.y, e2.draw.width, e2.draw.height))
                {
                    if (e.draw.x + e.draw.width / 2 > e2.draw.x + e2.draw.width / 2)
                    {
                        e2.attackableBump.bumped = BumpType.Right;
                    }
                    else
                    {
                        e2.attackableBump.bumped = BumpType.Left;
                    }
                }
            }
        }

        if (e.attackablePush.pushed == PushType.SmallMario)
        {
            // Start animation
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
