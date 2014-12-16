public class Entity
{
    public Entity()
    {
        draw = null;
        collider = null;
        attackablePush = null;
        attackableTouch = null;
        growable = null;

        scripts = new Script[scriptsMax];
        for (int i = 0; i < scriptsMax; i++)
        {
            scripts[i] = null;
        }
        scriptsCount = 0;
    }

    internal EntityDraw draw;
    // Can stand on. Can't walk through. Example: floor, brick.
    internal EntityCollider collider;
    // Pushed from any of 4 sides
    // Examples:
    // - Brick destroyed when player jumps from bottom
    // - Enemy destroyed when player jumps on top
    // - Pipe walked into by player
    internal EntityAttackablePush attackablePush;
    // Touched from any side by enemy
    // Example: Player dies when touched by enemies
    internal EntityAttackableTouch attackableTouch;
    // Touched by mushroom
    internal EntityGrowable growable;

    internal Script[] scripts;
    internal int scriptsCount;
    internal const int scriptsMax = 16;

    internal void Delete(Game game)
    {
        if (draw != null)
        {
            draw.Delete();
#if CITO
            delete draw;
#endif
        }
        if (collider != null)
        {
            collider.Delete();
#if CITO
            delete collider;
#endif
        }
        if (attackablePush != null)
        {
            attackablePush.Delete();
#if CITO
            delete attackablePush;
#endif
        }
        if (attackableTouch != null)
        {
            attackableTouch.Delete();
#if CITO
            delete attackableTouch;
#endif
        }
        if (growable != null)
        {
            growable.Delete();
#if CITO
            delete growable;
#endif
        }
        for (int i = 0; i < scriptsMax; i++)
        {
            if (scripts[i] != null)
            {
                scripts[i].Delete(game);
#if CITO
                delete scripts[i];
#endif
            }
        }
        scriptsCount = 0;
#if CITO
        delete scripts;
#endif
    }
}

public class EntityAttackableTouch
{
    public EntityAttackableTouch()
    {
        touched = false;
        touchedEntity = -1;
    }
    internal bool touched;
    internal int touchedEntity;

    public void Delete()
    {
    }
}

public enum PushSide
{
    None,
    LeftPipeEnter,
    Right,
    // Player jumps after attack
    TopJumpOnEnemy,
    // Player must press down key to enter
    TopPipeKeyDown,
    BottomBrickDestroy
}

public class EntityAttackablePush
{
    public EntityAttackablePush()
    {
        pushSide = PushSide.None;
        pushed = PushType.None;
    }
    internal PushSide pushSide;
    internal PushType pushed;

    public void Delete()
    {
    }
}

enum PushType
{
    None,
    SmallMario,
    BigMario
}

public class EntityGrowable
{
    public EntityGrowable()
    {
        grow = false;
    }
    internal bool grow;

    public void Delete()
    {
    }
}

public class EntityDraw
{
    public EntityDraw()
    {
        x = 0;
        y = 0;
        width = 16;
        height = 16;
        sprite = null;
        z = 0;
        mirrorx = false;
        xOffset = 0;
        yOffset = 0;
        absoluteScreenPosition = false;
        xrepeat = 1;
        yrepeat = 1;
        hidden = false;
        
        loadedSprite = -1;
        loadedSpriteName = null;
        loadedMirrorX = false;
    }
    internal float x;
    internal float y;
    internal int width;
    internal int height;
    internal string sprite;
    internal int z;
    internal bool mirrorx;
    internal float xOffset;
    internal float yOffset;
    internal bool absoluteScreenPosition;
    internal int xrepeat;
    internal int yrepeat;
    internal bool hidden;

    internal int loadedSprite;
    internal string loadedSpriteName;
    internal bool loadedMirrorX;

    public void Delete()
    {
    }
}

public class EntityCollider
{
    public void Delete()
    {
    }
}
