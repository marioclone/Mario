// Goomba, Mushroom
public class ScriptMoving : Script
{
    public ScriptMoving()
    {
        t = 0;
        constSpeed = 30;
        direction = -1;
    }

    float t;
    float constSpeed;
    internal float direction;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        if (game.gamePaused) { return; }
        if (!IsActiveHelper.IsActive(game, e.draw.x)) { return; }
        if (direction == 0) { return; }
        t += dt;
        float oldx = e.draw.x;
        float oldy = e.draw.y;
        float newx = e.draw.x + constSpeed * dt * direction;
        float newy = e.draw.y + 1;

        // Move horizontally
        if (CollisionHelper.IsEmpty(game, entity, newx, oldy, e.draw.width, e.draw.height))
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
        if (CollisionHelper.IsEmpty(game, entity, oldx, newy, e.draw.width, e.draw.height)
            && CollisionHelper.IsEmpty(game, entity, oldx, newy + 1, e.draw.width, e.draw.height))
        {
            e.draw.y = newy;
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
