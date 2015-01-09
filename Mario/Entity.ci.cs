public class Entity
{
    public Entity()
    {
        draw = null;
        collider = null;
        enemyCollider = false;
        attackablePush = null;
        attackableTouch = null;
        growable = null;
        attackableBump = null;
        IsCastleBridge = false;
        flagpoleClimbing = null;

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
    internal bool enemyCollider;
    // Pushed from any of 4 sides
    // Examples:
    // - Brick destroyed when player jumps from bottom
    // - Enemy destroyed when player jumps on top
    // - Pipe walked into by player
    internal EntityAttackablePush attackablePush;
    // Touched from any side by enemy
    // Example: Player dies when touched by enemies
    internal EntityAttackableTouch attackableTouch;
    internal EntityAttackableFireball attackableFireball;
    // Touched by mushroom
    internal EntityGrowable growable;
    internal EntityAttackableBump attackableBump;
    internal bool IsCastleBridge;

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
        if (attackableFireball != null)
        {
            attackableFireball.Delete();
#if CITO
            delete attackableFireball;
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

    internal EntityFlagpoleClimbing flagpoleClimbing;
}

public class EntityFlagpoleClimbing
{
    internal int startY;
    internal float endY;
    internal bool flagDone;
}

public class EntityAttackableFireball
{
    public EntityAttackableFireball()
    {
        attacked = false;
    }

    internal bool attacked;

    public void Delete()
    {
    }
}

public class EntityAttackableTouch
{
    public EntityAttackableTouch()
    {
        attackableByEnemy = false;
        attackableByPlayer = false;
        touched = false;
        touchedEntity = -1;
    }

    internal bool attackableByEnemy;
    internal bool attackableByPlayer;
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
    BottomBrickDestroy,
    LeftRightKoopa
}

public class EntityAttackablePush
{
    public EntityAttackablePush()
    {
        pushSide = PushSide.None;
        pushed = PushType.None;
        pushSideLeftRight = PushSideLeftRight.Left;
    }
    internal PushSide pushSide;
    internal PushType pushed;
    internal PushSideLeftRight pushSideLeftRight;

    public void Delete()
    {
    }
}

public enum PushSideLeftRight
{
    Left,
    Right
}

public enum PushType
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

public class EntityAttackableBump
{
    public EntityAttackableBump()
    {
        bumped = BumpType.None;
    }
    internal BumpType bumped;

    public void Delete()
    {
    }
}

public enum BumpType
{
    None,
    Left,
    Right
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
        mirror = MirrorType.None;
        xOffset = 0;
        yOffset = 0;
        absoluteScreenPosition = false;
        xrepeat = 1;
        yrepeat = 1;
        hidden = false;
        collisionOffsetX = 0;
        collisionOffsetY = 0;
        collisionOffsetWidth = 0;
        collisionOffsetHeight = 0;
        
        loadedSprite = -1;
        loadedSpriteName = null;
        loadedMirror = MirrorType.None;
    }
    internal float x;
    internal float y;
    internal int width;
    internal int height;
    internal string sprite;
    internal int z;
    internal MirrorType mirror;
    internal float xOffset;
    internal float yOffset;
    internal bool absoluteScreenPosition;
    internal int xrepeat;
    internal int yrepeat;
    internal bool hidden;
    internal int collisionOffsetX;
    internal int collisionOffsetY;
    internal int collisionOffsetWidth;
    internal int collisionOffsetHeight;

    internal int loadedSprite;
    internal string loadedSpriteName;
    internal MirrorType loadedMirror;

    public void Delete()
    {
    }
}

public enum MirrorType
{
    None,
    MirrorX,
    MirrorY
}

public class EntityCollider
{
    public void Delete()
    {
    }

    internal bool playerStuck;
}
