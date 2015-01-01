// Goomba, Mushroom
public class ScriptMoving : Script
{
    public ScriptMoving()
    {
        t = 0;
        constSpeed = 30;
        direction = -1;
        velY = 1;
        constGravity = one / 20;
        constBumpVel = -one * 15 / 10;
    }

    float t;
    float constSpeed;
    float velY; // bump
    float constGravity; // bump
    float constBumpVel;
    internal float direction;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        if (game.gamePaused) { return; }
        if (game.gamePausedByGrowthShrink) { return; }
        if (!IsActiveHelper.IsActive(game, e.draw.x)) { return; }
        if (direction == 0) { return; }
        t += dt;
        float oldx = e.draw.x;
        float oldy = e.draw.y;
        float newx = e.draw.x + constSpeed * dt * direction;
        float newy = e.draw.y + velY;
        velY += constGravity;
        if (velY > 1)
        {
            velY = 1;
        }

        // Move horizontally
        if (CollisionHelper.IsEmpty(game, entity, newx, oldy, e.draw.width, e.draw.height, true, false))
        {
            e.draw.x = newx;
        }
        else
        {
            // Bounce from walls
            // Also fixes falling down of mushroom on 1x1 gap
            direction = -direction;
        }

        // Move vertically (down)
        if (CollisionHelper.IsEmpty(game, entity, oldx, newy, e.draw.width, e.draw.height, true, false))
        //  && CollisionHelper.IsEmpty(game, entity, oldx, newy + 1, e.draw.width, e.draw.height, true))
        {
            e.draw.y = newy;
        }

        // Bump mushroom by brick
        if (e.attackableBump != null && e.attackableBump.bumped != BumpType.None)
        {
            if ((direction > 0 && e.attackableBump.bumped == BumpType.Right)
                || (direction < 0 && e.attackableBump.bumped == BumpType.Left))
            {
                direction = -direction;
            }
            e.attackableBump.bumped = BumpType.None;
            velY = constBumpVel;
        }
    }
}

public class IsActiveHelper
{
    public static bool IsActive(Game game, float x)
    {
        return x >= game.scrollx - 32 && x < game.scrollx + 256 + 64;
    }
}
