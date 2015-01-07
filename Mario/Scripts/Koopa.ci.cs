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
        isDead = false;
        velY = 0;

        constWalkSpeed = 30;
        constSlideSpeed = 150;
        constReviveTime1 = 2;
        constReviveTime2 = 3;
        constReviveBlinkingSpeed = 5;
        constUpDownSpeed = one / 3;
        constGravity = one;
        constJumpVel = one * 100;

        deadFromFireball = new DeadFromFireballOrBump();

        type = KoopaType.Normal;
        floatingStart = 0;
        floatingEnd = 0;
    }

    float t;
    float reviveTime;
    float constAnimSpeed;
    float direction;
    bool slide;
    float slideTime;
    bool isDead;
    float velY;

    float constWalkSpeed;
    float constSlideSpeed;
    float constReviveTime1;
    float constReviveTime2;
    float constReviveBlinkingSpeed;
    float constUpDownSpeed;
    float constGravity;
    float constJumpVel;

    DeadFromFireballOrBump deadFromFireball;

    internal KoopaType type;
    internal float floatingStart;
    internal float floatingEnd;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        if (game.gamePaused) { return; }
        if (game.gamePausedByGrowthShrink) { return; }
        if (!IsActiveHelper.IsActive(game, e.draw.x)) { return; }
        t += dt;

        if (e.attackablePush.pushed != PushType.None)
        {
            if (!slide)
            {
                if (direction != 0)
                {
                    if (type == KoopaType.Floating
                        || type == KoopaType.Jumping)
                    {
                        type = KoopaType.Normal;
                        e.attackablePush.pushed = PushType.None;
                        velY = 0;
                        game.AudioPlay("Stomp");
                        Spawn_.Score(game, e.draw.x, e.draw.y, Game.ScoreKoopaJumping);
                        return;
                    }
                    else
                    {
                        // Player jumps on walking Koopa
                        direction = 0;
                        e.draw.y += 10;
                        game.AudioPlay("Stomp");
                        Spawn_.Score(game, e.draw.x, e.draw.y, Game.ScoreKoopa);
                        e.attackablePush.pushSide = PushSide.LeftRightKoopa;
                    }
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

        isDead = deadFromFireball.Update(game, entity, dt, Game.ScoreKoopa);

        if (isDead)
        {
            type = KoopaType.Normal;
        }

        if (slide)
        {
            slideTime += dt;
        }

        float velX = 0;

        if (type == KoopaType.Normal || type == KoopaType.Jumping)
        {
            if (slide)
            {
                velX = direction * constSlideSpeed;
            }
            else
            {
                velX = direction * constWalkSpeed;
            }

            // Walk and slide
            if (!isDead)
            {
                float oldx = e.draw.x;
                float oldy = e.draw.y;

                bool isOnGroundOld = !CollisionHelper.IsEmpty(game, entity, oldx + e.draw.collisionOffsetX + e.draw.width / 2, oldy + e.draw.collisionOffsetY + 1, 1, e.draw.height + e.draw.collisionOffsetHeight + 1, !slide, false);
                if (!isOnGroundOld)
                {
                    velY += constGravity;
                }

                float newx = e.draw.x + velX * dt;
                float newy = e.draw.y + velY * dt;
                bool isOnGroundNew = !CollisionHelper.IsEmpty(game, entity, newx + e.draw.collisionOffsetX + e.draw.width / 2, oldy + e.draw.collisionOffsetY + 1, 1, e.draw.height + e.draw.collisionOffsetHeight + 1, !slide, false);

                // Move horizontally
                if (CollisionHelper.IsEmpty(game, entity, newx + e.draw.collisionOffsetX, oldy + e.draw.collisionOffsetY, e.draw.width + e.draw.collisionOffsetWidth, e.draw.height + e.draw.collisionOffsetHeight, !slide, false)
                    && (!(isOnGroundOld && (!isOnGroundNew) && (!slide))))
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
                if (CollisionHelper.IsEmpty(game, entity, oldx + e.draw.collisionOffsetX, newy + e.draw.collisionOffsetY, e.draw.width + e.draw.collisionOffsetWidth, e.draw.height + e.draw.collisionOffsetHeight, !slide, false)
                    && CollisionHelper.IsEmpty(game, entity, oldx + e.draw.collisionOffsetX, newy + 1 + e.draw.collisionOffsetY, e.draw.width + e.draw.collisionOffsetWidth, e.draw.height + e.draw.collisionOffsetHeight, !slide, false))
                {
                    e.draw.y = newy;
                }

                if (type == KoopaType.Jumping && isOnGroundNew)
                {
                    velY = -constJumpVel;
                }
            }
        }
        if (type == KoopaType.Floating)
        {
            e.draw.y = ScriptPlatform.PlatformPosition(game, t, floatingEnd, floatingStart, constUpDownSpeed);
        }

        if (direction == 0 || slide)
        {
            e.draw.sprite = "CharactersKoopaStoppedNormalNormal";
            e.draw.height = 14;
        }
        else
        {
            int anim = game.platform.FloatToInt(t * constAnimSpeed) % 2;
            if (isDead)
            {
                anim = 0;
            }
            if (type == KoopaType.Normal)
            {
                if (anim == 1)
                {
                    e.draw.sprite = "CharactersKoopaNormalNormalNormalNormalNormal";
                }
                else
                {
                    e.draw.sprite = "CharactersKoopaNormalNormalNormalNormalTwo";
                }
            }
            if (type == KoopaType.Floating)
            {
                if (anim == 1)
                {
                    e.draw.sprite = "CharactersKoopaNormalNormalFlyingNormal";
                }
                else
                {
                    e.draw.sprite = "CharactersKoopaNormalNormalFlyingTwo";
                }
            }
            if (type == KoopaType.Jumping)
            {
                if (anim == 1)
                {
                    e.draw.sprite = "CharactersKoopaNormalNormalJumpingNormal";
                }
                else
                {
                    e.draw.sprite = "CharactersKoopaNormalNormalJumpingTwo";
                }
            }
        }

        if (!isDead)
        {
            if (direction <= 0)
            {
                e.draw.mirror = MirrorType.None;
            }
            else
            {
                e.draw.mirror = MirrorType.MirrorX;
            }
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
    public static void Spawn(Game game, int x, int y, KoopaType type, float floatingStart, float floatingEnd)
    {
        Entity e = SystemSpawn.Spawn(game, GetImage(type), x, y);
        e.draw.height = 24;
        e.attackablePush = new EntityAttackablePush();
        e.attackablePush.pushSide = PushSide.TopJumpOnEnemy;
        e.attackableFireball = new EntityAttackableFireball();
        e.draw.collisionOffsetY = 8;
        e.draw.collisionOffsetHeight = -8;
        e.enemyCollider = true;
        ScriptKoopa script = new ScriptKoopa();
        script.type = type;
        script.floatingStart = floatingStart;
        script.floatingEnd = floatingEnd;
        e.scripts[e.scriptsCount++] = script;
    }

    static string GetImage(KoopaType type)
    {
        if (type == KoopaType.Floating)
        {
            return "CharactersKoopaNormalNormalFlyingNormal";
        }
        if (type == KoopaType.Jumping)
        {
            return "CharactersKoopaNormalNormalJumpingNormal";
        }
        return "CharactersKoopaNormalNormalNormalNormalNormal";
    }
}

public class DeadFromFireballOrBump
{
    public DeadFromFireballOrBump()
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

        // If touched by fireball, entity dies
        if (e.attackableFireball != null)
        {
            if (e.attackableFireball.attacked
                && (!dead))
            {
                Die(game, score, e);
            }
        }
        // If bumped, entity dies
        if (e.attackableBump != null)
        {
            if (e.attackableBump.bumped != BumpType.None && (!dead))
            {
                Die(game, score, e);
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

    void Die(Game game, int score, Entity e)
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
