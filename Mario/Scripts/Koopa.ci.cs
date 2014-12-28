public class ScriptKoopa : Script
{
    public ScriptKoopa()
    {
        t = 0;
        reviveTime = 0;
        constAnimSpeed = 4;
        direction = -1;
        slide = false;
        slideTime = 0;
        constWalkSpeed = 30;
        constSlideSpeed = 150;
        constReviveTime1 = 2;
        constReviveTime2 = 3;
        constReviveBlinkingSpeed = 5;
        deadFromFireball = new DeadFromFireball();
    }

    float t;
    float reviveTime;
    float constAnimSpeed;
    float direction;
    bool slide;
    float slideTime;
    float constWalkSpeed;
    float constSlideSpeed;
    float constReviveTime1;
    float constReviveTime2;
    float constReviveBlinkingSpeed;
    DeadFromFireball deadFromFireball;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        if (game.gamePaused) { return; }
        if (!IsActiveHelper.IsActive(game, e.draw.x)) { return; }
        t += dt;

        if (e.attackablePush.pushed != PushType.None)
        {
            if (!slide)
            {
                if (direction != 0)
                {
                    // Player jumps on walking Koopa
                    direction = 0;
                    e.draw.y += 10;
                    game.AudioPlay("Stomp");
                    Spawn_.Score(game, e.draw.x, e.draw.y, Game.ScoreKoopa);
                    e.attackablePush.pushSide = PushSide.LeftRightKoopa;
                }
                else
                {
                    // Player jumps on stopped Koopa
                    if (e.attackablePush.pushSideLeftRight == PushSideLeftRight.Left)
                    {
                        direction = 1;
                    }
                    else
                    {
                        direction = -1;
                    }
                    slide = true;
                    slideTime = 0;
                }
            }
            e.attackablePush.pushed = PushType.None;
        }

        deadFromFireball.Update(game, entity, dt, Game.ScoreKoopa);

        if (slide)
        {
            slideTime += dt;
        }

        float velX = 0;
        if (slide)
        {
            velX = direction * constSlideSpeed;
        }
        else
        {
            velX = direction * constWalkSpeed;
        }

        // Walk and slide
        {
            float oldx = e.draw.x;
            float oldy = e.draw.y;
            float newx = e.draw.x + velX * dt;
            float newy = e.draw.y + 1;

            // Move horizontally
            if (CollisionHelper.IsEmpty(game, entity, newx + e.draw.collisionOffsetX, oldy + e.draw.collisionOffsetY, e.draw.width + e.draw.collisionOffsetWidth, e.draw.height + e.draw.collisionOffsetHeight))
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
            if (CollisionHelper.IsEmpty(game, entity, oldx + e.draw.collisionOffsetX, newy + e.draw.collisionOffsetY, e.draw.width + e.draw.collisionOffsetWidth, e.draw.height + e.draw.collisionOffsetHeight)
                && CollisionHelper.IsEmpty(game, entity, oldx + e.draw.collisionOffsetX, newy + 1 + e.draw.collisionOffsetY, e.draw.width +e.draw.collisionOffsetWidth, e.draw.height + e.draw.collisionOffsetHeight))
            {
                e.draw.y = newy;
            }
        }

        if (direction == 0 || slide)
        {
            e.draw.sprite = "CharactersKoopaStoppedNormalNormal";
            e.draw.height = 14;
        }
        else
        {
            // Walk animation
            if ((game.platform.FloatToInt(t * constAnimSpeed) % 2) == 1)
            {
                e.draw.sprite = "CharactersKoopaNormalNormalNormalNormalNormal";
            }
            else
            {
                e.draw.sprite = "CharactersKoopaNormalNormalNormalNormalTwo";
            }
        }

        if (direction <= 0)
        {
            e.draw.mirror = MirrorType.None;
        }
        else
        {
            e.draw.mirror = MirrorType.MirrorX;
        }

        if (direction == 0)
        {
            reviveTime += dt;

            if (reviveTime > constReviveTime1)
            {
                int q = game.platform.FloatToInt(reviveTime * constReviveBlinkingSpeed) % 2;
                if (q == 0)
                {
                    e.draw.sprite = "CharactersKoopaStoppedNormalTwo";
                }
                else
                {
                    e.draw.sprite = "CharactersKoopaStoppedNormalNormal";
                }
            }

            if (reviveTime > constReviveTime2)
            {
                direction = -1;
                e.draw.height = 24;
                e.draw.y -= 10;
                reviveTime = -1;
            }
        }
        if (slide)
        {
            HelperAttackWithFireball.Update(game, e);
        }
        if (!slide || (slide && slideTime > one / 2))
        {
            HelperAttackWithTouch.Update(game, e);
        }
    }
}

public class SpawnKoopa
{
    public static void Spawn(Game game, int x, int y)
    {
        Entity e = SystemSpawn.Spawn(game, "CharactersKoopaNormalNormalNormalNormalNormal", x, y);
        e.draw.height = 24;
        e.attackablePush = new EntityAttackablePush();
        e.attackablePush.pushSide = PushSide.TopJumpOnEnemy;
        e.attackableFireball = new EntityAttackableFireball();
        e.draw.collisionOffsetY = 8;
        e.draw.collisionOffsetHeight = -8;
        e.scripts[e.scriptsCount++] = new ScriptKoopa();
    }
}

public class DeadFromFireball
{
    public DeadFromFireball()
    {
        float one = 1;
        constDeadTime = one / 2;
    }

    internal bool dead;
    internal bool deadFromFireball;
    internal float t;
    internal float constDeadTime;

    public bool Update(Game game, int entity, float dt, int score)
    {
        Entity e = game.entities[entity];

        // If touched by fireball, the goomba dies
        if (e.attackableFireball != null)
        {
            if (e.attackableFireball.attacked
                && (!dead))
            {
                if (e.collider != null)
                {
#if CITO
                    delete e.collider;
#endif
                    e.collider = null;
                }
                dead = true;
                deadFromFireball = true;
                t = 0;
                game.AudioPlay("Shot");
                Spawn_.Score(game, e.draw.x, e.draw.y, score);
            }
        }

        if (deadFromFireball)
        {
            e.draw.mirror = MirrorType.MirrorY;
            e.draw.y += dt * 100;
        }

        // Remove dead goomba
        if (dead && t > constDeadTime)
        {
            game.DeleteEntity(entity);
        }

        return deadFromFireball;
    }
}
