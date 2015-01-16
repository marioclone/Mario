public class ScriptPlayer : Script
{
    public ScriptPlayer()
    {
        t = 0;
        velX = 0;
        velY = 0;
        lookLeft = false;
        onGround = true;
        jumptime = -1;
        jumpSpringboard = false;
        growth = 0;
        dead = false;
        deadT = 0;
        invulnerable = 0;
        crouching = false;
        topPipeEnter = -1;
        leftPipeEnter = -1;
        fireballTime = 999;
        growthAnimationTime = -1;
        shrinkAnimationTime = -1;
        controlsFirePreviously = false;
        wasScrollBlock = false;
        flagpoleClimbingPlayerDone = false;
        previousY = 0;

        controlsTemp = new Controls();

        constAcceleration = one * 50 / 10;
        constAccelerationInAir = one * 30 / 10;
        constGravity = one * 50 / 10;
        constGravityUnderwater = one * 20 / 10;
        constMaxVel = 100;
        constAnimSpeed = 10;
        constMaxJumpTime = one * 2 / 10;
        constJumpVelocity = -200;
        constJumpVelocityUnderwater = -120;
        constInvulnerableTime = one * 20 / 10;
        constRunningMultiplier = one * 16 / 10;
        constFireballFrequency = one * 50 / 10;
        constGrowthShrinkAnimationLength = one * 10 / 10;
        constGrowthShrinkAnimationSpeed = 10;
        constScrollSpeed = 140;
    }

    float t;
    float velX;
    float velY;
    bool lookLeft;
    bool onGround;
    float jumptime;
    bool jumpSpringboard;
    int growth;
    bool dead;
    float deadT;
    float invulnerable;
    bool crouching;
    float topPipeEnter;
    float leftPipeEnter;
    float fireballTime;
    float growthAnimationTime;
    float shrinkAnimationTime;
    bool wasScrollBlock;
    float previousY;

    Controls controlsTemp;

    float constAcceleration;
    float constAccelerationInAir;
    float constGravity;
    float constGravityUnderwater;
    float constMaxVel;
    float constAnimSpeed;
    float constMaxJumpTime;
    float constJumpVelocity;
    float constJumpVelocityUnderwater;
    float constInvulnerableTime;
    float constRunningMultiplier;
    float constFireballFrequency;
    float constGrowthShrinkAnimationLength;
    float constGrowthShrinkAnimationSpeed;
    float constScrollSpeed;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];

        Controls controls = controlsTemp;
        if (game.controlsOverrideActive)
        {
            controls.CopyFrom(game.controlsOverride);
        }
        else
        {
            controls.CopyFrom(game.controls);
        }
        game.controlsOverrideActive = false;
        game.controlsOverride.Clear();
        game.playerx = e.draw.x;
        game.playery = e.draw.y;
        game.playerGrowth = growth;

        FlagPoleClimbing(game, e);
        UpdateSprite(game, dt, e, controls);

        // Scroll
        float newScrollX = e.draw.x - 256 / 2 + 8;
        if (!game.scrollBlock && (!wasScrollBlock))
        {
            if (newScrollX >= game.scrollxMax - 256)
            {
                newScrollX = game.scrollxMax - 256;
            }
            if (newScrollX > game.scrollx)
            {
                game.scrollx = newScrollX;
            }
        }
        if (!game.scrollBlock && wasScrollBlock)
        {
            if (newScrollX >= game.scrollx)
            {
                game.scrollx += constScrollSpeed * dt;
                if (game.scrollx > newScrollX)
                {
                    game.scrollx = newScrollX;
                    wasScrollBlock = false;
                }
            }
        }
        else
        {
            wasScrollBlock = game.scrollBlock;
        }

        if (game.gamePaused) { return; }
        if (e.flagpoleClimbing != null) { return; }
        if (growthAnimationTime != -1 && growthAnimationTime < constGrowthShrinkAnimationLength)
        {
            game.gamePausedByGrowthShrink = true;
            return;
        }
        if (shrinkAnimationTime != -1 && shrinkAnimationTime < constGrowthShrinkAnimationLength)
        {
            game.gamePausedByGrowthShrink = true;
            return;
        }
        game.gamePausedByGrowthShrink = false;
        if (growthAnimationTime > constGrowthShrinkAnimationLength)
        {
            growthAnimationTime = -1;
        }

        bool crouchingSlide = crouching && onGround;

        float acceleration;
        if (onGround)
        {
            acceleration = constAcceleration;
        }
        else
        {
            acceleration = constAccelerationInAir;
        }

        if (controls.left && (!crouchingSlide))
        {
            velX -= acceleration;
        }
        if (controls.right && (!crouchingSlide))
        {
            velX += acceleration;
        }

        velX = Misc.MakeCloserToZero(velX, one / 1);
        velY = Misc.MakeCloserToZero(velY, one / 1);

        float maxVel = constMaxVel;
        if (controls.fire)
        {
            maxVel = maxVel * constRunningMultiplier;
        }

        if (velX > maxVel) { velX = maxVel; }
        if (velX < -maxVel) { velX = -maxVel; }

        float currentJumpVelocity;
        float currentGravity;
        if (game.setting == SettingType.Underwater)
        {
            currentJumpVelocity = constJumpVelocityUnderwater;
            currentGravity = constGravityUnderwater;
            if (e.draw.y > 48)
            {
                onGround = true;
            }
        }
        else
        {
            currentJumpVelocity = constJumpVelocity;
            currentGravity = constGravity;
        }

        velY += currentGravity;

        // Jump
        {
            if (IsOnSpringboard(game, e))
            {
                jumpSpringboard = true;
            }
            if (controls.jump && onGround && (!dead))
            {
                if (jumptime == -1)
                {
                    if (growth == 0)
                    {
                        game.AudioPlay("Jump");
                    }
                    else
                    {
                        game.AudioPlay("JumpBig");
                    }
                }
                jumptime = 0;
                jumpSpringboard = false;
            }

            if (controls.jump && (onGround || (jumptime >= 0 && jumptime < constMaxJumpTime)))
            {
                if (jumpSpringboard)
                {
                    velY = Min(velY, currentJumpVelocity * one * 235 / 100);
                }
                else
                {
                    velY = Min(velY, currentJumpVelocity);
                }
                jumpSpringboard = false;
            }

            if (!controls.jump)
            {
                jumptime = -1;
            }
            if (jumptime != -1)
            {
                jumptime += dt;
            }
        }

        if (dead)
        {
            velX = 0;
            velY = 0;
            jumptime = -1;
        }

        if (topPipeEnter != -1)
        {
            velY = constMaxVel / 2;
            velX = 0;
            topPipeEnter += dt;
        }
        if (leftPipeEnter != -1)
        {
            velX = constMaxVel / 2;
            leftPipeEnter += dt;
        }
        if (topPipeEnter > 1) { topPipeEnter = -1; }
        if (leftPipeEnter > 1) { leftPipeEnter = -1; }

        e.draw.collisionOffsetX = 2;
        e.draw.collisionOffsetY = 0;
        e.draw.collisionOffsetWidth = -4;
        e.draw.collisionOffsetHeight = 0;

        // Move
        float oldx = e.draw.x;
        float oldy = e.draw.y;

        float newx = e.draw.x + velX * dt;
        float newy = e.draw.y + velY * dt;

        PushSide side = AttackHelper.Attack(game, e.draw.x + e.draw.collisionOffsetX, e.draw.y + e.draw.collisionOffsetY, newx + e.draw.collisionOffsetX, newy + e.draw.collisionOffsetY, e.draw.width + e.draw.collisionOffsetWidth, e.draw.height + e.draw.collisionOffsetHeight, growth > 0, controls.down);
        if (side == PushSide.TopJumpOnEnemy)
        {
            // Bump
            jumptime = 0;
            velY = currentJumpVelocity;
        }

        if (side == PushSide.TopPipeKeyDown)
        {
            topPipeEnter = 0;
        }

        if (side == PushSide.LeftPipeEnter)
        {
            leftPipeEnter = 0;
        }


        if (CollisionHelper.IsEmpty(game, -1, newx + e.draw.collisionOffsetX, e.draw.y + e.draw.collisionOffsetY, e.draw.width + e.draw.collisionOffsetWidth, e.draw.height + e.draw.collisionOffsetHeight, false, true)
            && newx >= game.scrollx)
        {
            e.draw.x = newx;
        }

        if (CollisionHelper.IsEmpty(game, -1, e.draw.x + e.draw.collisionOffsetX, newy + e.draw.collisionOffsetY, e.draw.width + e.draw.collisionOffsetWidth, e.draw.height + e.draw.collisionOffsetHeight, false, true))
        {
            e.draw.y = newy;
        }

        onGround = e.draw.y == oldy && velY >= 0;
        bool onCeiling = e.draw.y == oldy && velY < 0;

        if (onCeiling)
        {
            velY = 0;
            jumptime = constMaxJumpTime;
        }

        if (onGround)
        {
            velY = 0;
        }

        if (e.draw.x - game.platform.FloatToInt(e.draw.x) < one * 10 / 100)
        {
            e.draw.x = game.platform.FloatToInt(e.draw.x);
        }
        if (e.draw.x - game.platform.FloatToInt(e.draw.x) > one * 90 / 100)
        {
            e.draw.x = game.platform.FloatToInt(e.draw.x) + 1;
        }
        if (e.draw.y - game.platform.FloatToInt(e.draw.y) < one * 10 / 100)
        {
            e.draw.y = game.platform.FloatToInt(e.draw.y);
        }
        if (e.draw.y - game.platform.FloatToInt(e.draw.y) > one * 90 / 100)
        {
            e.draw.y = game.platform.FloatToInt(e.draw.y) + 1;
        }

        // Growth
        if (e.growable.grow)
        {
            game.AudioPlay("MushroomEat");
            e.growable.grow = false;
            if (growth == 0)
            {
                e.draw.y -= 16;
            }
            growth++;
            if (growth <= 2)
            {
                growthAnimationTime = 0;
            }
            if (growth > 2)
            {
                growth = 2;
            }
        }

        // If player is stuck colliding with an entity, disable collision with that entity
        // Example: player can get stuck when he grows and there is a block above him
        for (int i = 0; i < game.entitiesCount; i++)
        {
            if (i == entity) { continue; }
            if (game.entities[i] == null) { continue; }
            Entity e2 = game.entities[i];
            if (e2.collider != null)
            {
                if (Misc.RectIntersect(e.draw.x + e.draw.collisionOffsetX, e.draw.y + e.draw.collisionOffsetY, e.draw.width + e.draw.collisionOffsetWidth, e.draw.height + e.draw.collisionOffsetHeight,
                    e2.draw.x, e2.draw.y, e2.draw.width * e2.draw.xrepeat, e2.draw.height * e2.draw.yrepeat))
                {
                    e2.collider.playerStuck = true;
                }
                else
                {
                    e2.collider.playerStuck = false;
                }
            }
        }

        // Don't get killed by piranha when entering pipe
        if (topPipeEnter != -1)
        {
            e.attackableTouch.touched = false;
        }

        // Death
        if (invulnerable > 0)
        {
            invulnerable -= dt;
            e.attackableTouch.touched = false;
        }
        if (invulnerable <= 0)
        {
            if (e.attackableTouch.touched)
            {
                if (growth == 0)
                {
                    if (!dead)
                    {
                        game.AudioPlay("Death");
                        game.audio.audioPlayMusic = null;
                    }
                    dead = true;
                }
                else
                {
                    growth--;
                    shrinkAnimationTime = 0;
                    crouching = false;
                    invulnerable = constInvulnerableTime;
                    game.AudioPlay("Shrink");
                }
                e.attackableTouch.touched = false;
            }
        }

        // Death from falling down into hole
        if (e.draw.y > 240)
        {
            growth = 0;
            game.AudioPlay("Death");
            game.audio.audioPlayMusic = null;
            game.gameShowDeathScreen = true;
        }

        // Death animation
        if (dead)
        {
            if (!game.gamePaused)
            {
                deadT += dt;
            }
            e.draw.yOffset = DeathAnimation(deadT * 4);
            if (deadT > 3)
            {
                game.gameShowDeathScreen = true;
                dead = false;
                e.draw.yOffset = 0;
            }
        }

        // Touch coins
        for (int i = 0; i < game.entitiesCount; i++)
        {
            Entity e2 = game.entities[i];
            if (e2 == null) { continue; }
            if (e2 == e) { continue; }
            if (e2.attackableTouch == null) { continue; }
            if (!e2.attackableTouch.attackableByPlayer) { continue; }
            if (Misc.RectIntersect(e.draw.x, e.draw.y, e.draw.width * e.draw.xrepeat, e.draw.height * e.draw.yrepeat,
                e2.draw.x, e2.draw.y, e2.draw.width * e2.draw.xrepeat, e2.draw.height * e2.draw.yrepeat))
            {
                e2.attackableTouch.touched = true;
                e2.attackableTouch.touchedEntity = entity;
                return;
            }
        }

        // Fire
        fireballTime += dt;
        if (growth == 2
            && controls.fire
            && (fireballTime > one / constFireballFrequency)
            && (!controlsFirePreviously))
        {
            fireballTime = 0;
            Entity fireball = new Entity();
            fireball.draw = new EntityDraw();
            fireball.draw.x = e.draw.x;
            fireball.draw.y = e.draw.y;
            fireball.draw.width = 8;
            fireball.draw.height = 8;
            fireball.draw.z = 3;
            ScriptFireball script = new ScriptFireball();
            script.dirLeft = lookLeft;
            fireball.scripts[fireball.scriptsCount++] = script;
            game.AddEntity(fireball);
            game.AudioPlay("Fireball");
        }
        controlsFirePreviously = controls.fire;

        // Skidding
        if (onGround && (!crouching)
            && (((controls.left) && (!controls.right) && (velX > 0))
            || ((controls.right) && (!controls.left) && (velX < 0))))
        {
            if (growth == 0)
            {
                e.draw.sprite = "CharactersPlayerNormalNormalNormalRunningSkidding";
            }
            if (growth == 1)
            {
                e.draw.sprite = "CharactersPlayerNormalLargeNormalRunningSkidding";
            }
            if (growth == 2)
            {
                e.draw.sprite = "CharactersPlayerNormalFieryNormalRunningSkidding";
            }
        }
    }

    static float Min(float a, float b)
    {
        if (a <= b)
        {
            return a;
        }
        else
        {
            return b;
        }
    }

    bool IsOnSpringboard(Game game, Entity player)
    {
        for (int i = 0; i < game.entitiesCount; i++)
        {
            Entity e = game.entities[i];
            if (e == null) { continue; }
            if (!e.IsSpringboard) { continue; }
            if (Misc.RectIntersect(e.draw.x + e.draw.collisionOffsetX, e.draw.y + 1 + e.draw.collisionOffsetY, e.draw.width + e.draw.collisionOffsetWidth, e.draw.height + e.draw.collisionOffsetHeight,
                player.draw.x + player.draw.collisionOffsetX, player.draw.y + player.draw.collisionOffsetY, player.draw.width + player.draw.collisionOffsetWidth, player.draw.height + player.draw.collisionOffsetHeight))
            {
                return true;
            }
        }
        return false;
    }

    bool controlsFirePreviously;

    float DeathAnimation(float time)
    {
        if (time < 1)
        {
            return 0;
        }
        if (time < 2)
        {
            return -(time - 1) * 32;
        }
        else
        {
            return -(1 - (time - 2)) * 32;
        }
    }

    void UpdateSprite(Game game, float dt, Entity e, Controls controls)
    {
        int currentGrowth = growth;
        float currentVelX = velX;

        // Growth animation
        if (growthAnimationTime != -1)
        {
            growthAnimationTime += dt;
            if (game.platform.FloatToInt(growthAnimationTime * constGrowthShrinkAnimationSpeed) % 2 == 0)
            {
                currentGrowth = growth;
                e.draw.yOffset = 0;
            }
            else
            {
                currentGrowth = growth - 1;
                if (currentGrowth == 0)
                {
                    e.draw.yOffset = 16;
                }
            }
            currentVelX = 0;
        }
        else
        {
            e.draw.yOffset = 0;
        }

        // Shrink animation
        if (shrinkAnimationTime != -1)
        {
            shrinkAnimationTime += dt;
            if (game.platform.FloatToInt(shrinkAnimationTime * constGrowthShrinkAnimationSpeed) % 2 == 0)
            {
                currentGrowth = growth;
                e.draw.yOffset = 0;
            }
            else
            {
                currentGrowth = growth + 1;
                if (currentGrowth == 1)
                {
                    e.draw.yOffset = -16;
                }
            }
        }
        if (shrinkAnimationTime != -1)
        {
            if (game.platform.FloatToInt(shrinkAnimationTime * constGrowthShrinkAnimationSpeed * 2) % 2 == 0)
            {
                e.draw.hidden = true;
            }
            else
            {
                e.draw.hidden = false;
            }
        }
        if (shrinkAnimationTime > constGrowthShrinkAnimationLength)
        {
            shrinkAnimationTime = -1;
            e.draw.hidden = false;
        }

        bool swimming = game.setting == SettingType.Underwater;
        // Sprite
        if (e.flagpoleClimbing != null)
        {
            if (!flagpoleClimbingPlayerDone)
            {
                t += dt;
            }
            float stagesCount = 2;
            int stage = game.platform.FloatToInt(t * constAnimSpeed % stagesCount);
            if (currentGrowth == 0)
            {
                if (stage == 0) { e.draw.sprite = "CharactersPlayerNormalNormalNormalClimbingNormal"; }
                if (stage == 1) { e.draw.sprite = "CharactersPlayerNormalNormalNormalClimbingTwo"; }
            }
            if (currentGrowth == 1)
            {
                if (stage == 0) { e.draw.sprite = "CharactersPlayerNormalLargeNormalClimbingNormal"; }
                if (stage == 1) { e.draw.sprite = "CharactersPlayerNormalLargeNormalClimbingTwo"; }
            }
            if (currentGrowth == 2)
            {
                if (stage == 0) { e.draw.sprite = "CharactersPlayerNormalFieryNormalClimbingNormal"; }
                if (stage == 1) { e.draw.sprite = "CharactersPlayerNormalFieryNormalClimbingTwo"; }
            }
        }
        else if (currentVelX == 0)
        {
            if (currentGrowth == 0)
            {
                e.draw.sprite = "CharactersPlayerNormalNormalNormalNormal";
            }
            if (currentGrowth == 1)
            {
                e.draw.sprite = "CharactersPlayerNormalLargeNormalNormal";
            }
            if (currentGrowth == 2)
            {
                e.draw.sprite = "CharactersPlayerNormalFieryNormalNormal";
            }
            t = 0;
        }
        else
        {
            if (!game.gamePaused)
            {
                if (controls.fire)
                {
                    t += dt * constRunningMultiplier;
                }
                else
                {
                    t += dt;
                }
            }
            float stagesCount = 4;
            int stage = game.platform.FloatToInt(t * constAnimSpeed % stagesCount);
            if (currentGrowth == 0)
            {
                if (stage == 0) { e.draw.sprite = "CharactersPlayerNormalNormalNormalRunningNormalTwo"; }
                if (stage == 1) { e.draw.sprite = "CharactersPlayerNormalNormalNormalRunningNormalNormal"; }
                if (stage == 2) { e.draw.sprite = "CharactersPlayerNormalNormalNormalRunningNormalTwo"; }
                if (stage == 3) { e.draw.sprite = "CharactersPlayerNormalNormalNormalRunningNormalThree"; }
            }
            if (currentGrowth == 1)
            {
                if (stage == 0) { e.draw.sprite = "CharactersPlayerNormalLargeNormalRunningNormalTwo"; }
                if (stage == 1) { e.draw.sprite = "CharactersPlayerNormalLargeNormalRunningNormalNormal"; }
                if (stage == 2) { e.draw.sprite = "CharactersPlayerNormalLargeNormalRunningNormalTwo"; }
                if (stage == 3) { e.draw.sprite = "CharactersPlayerNormalLargeNormalRunningNormalThree"; }
            }
            if (currentGrowth == 2)
            {
                if (stage == 0) { e.draw.sprite = "CharactersPlayerNormalFieryNormalRunningNormalTwo"; }
                if (stage == 1) { e.draw.sprite = "CharactersPlayerNormalFieryNormalRunningNormalNormal"; }
                if (stage == 2) { e.draw.sprite = "CharactersPlayerNormalFieryNormalRunningNormalTwo"; }
                if (stage == 3) { e.draw.sprite = "CharactersPlayerNormalFieryNormalRunningNormalThree"; }
            }
        }
        if (!onGround && e.flagpoleClimbing == null)
        {
            if (swimming)
            {
                if (previousY < e.draw.y)
                {
                    if (currentGrowth == 0)
                    {
                        e.draw.sprite = "CharactersPlayerNormalNormalNormalPaddlingNormalNormal";
                    }
                    if (currentGrowth == 1)
                    {
                        e.draw.sprite = "CharactersPlayerNormalLargeNormalPaddlingNormalNormal";
                    }
                    if (currentGrowth == 2)
                    {
                        e.draw.sprite = "CharactersPlayerNormalFieryNormalPaddlingNormalNormal";
                    }
                }
                else
                {
                    float stagesCount = 4;
                    int stage = game.platform.FloatToInt(t * constAnimSpeed % stagesCount);
                    if (currentGrowth == 0)
                    {
                        if (stage == 0) { e.draw.sprite = "CharactersPlayerNormalNormalNormalPaddlingNormalPaddle2"; }
                        if (stage == 1) { e.draw.sprite = "CharactersPlayerNormalNormalNormalPaddlingNormalPaddle1"; }
                        if (stage == 2) { e.draw.sprite = "CharactersPlayerNormalNormalNormalPaddlingNormalPaddle2"; }
                        if (stage == 3) { e.draw.sprite = "CharactersPlayerNormalNormalNormalPaddlingNormalPaddle3"; }
                    }
                    if (currentGrowth == 1)
                    {
                        if (stage == 0) { e.draw.sprite = "CharactersPlayerNormalLargeNormalPaddlingNormalPaddle2"; }
                        if (stage == 1) { e.draw.sprite = "CharactersPlayerNormalLargeNormalPaddlingNormalPaddle1"; }
                        if (stage == 2) { e.draw.sprite = "CharactersPlayerNormalLargeNormalPaddlingNormalPaddle2"; }
                        if (stage == 3) { e.draw.sprite = "CharactersPlayerNormalLargeNormalPaddlingNormalPaddle3"; }
                    }
                    if (currentGrowth == 2)
                    {
                        if (stage == 0) { e.draw.sprite = "CharactersPlayerNormalFieryNormalPaddlingNormalPaddle2"; }
                        if (stage == 1) { e.draw.sprite = "CharactersPlayerNormalFieryNormalPaddlingNormalPaddle1"; }
                        if (stage == 2) { e.draw.sprite = "CharactersPlayerNormalFieryNormalPaddlingNormalPaddle2"; }
                        if (stage == 3) { e.draw.sprite = "CharactersPlayerNormalFieryNormalPaddlingNormalPaddle3"; }
                    }
                }
                previousY = e.draw.y;
            }
            else
            {
                if (currentGrowth == 0)
                {
                    e.draw.sprite = "CharactersPlayerNormalNormalJumping";
                }
                if (currentGrowth == 1)
                {
                    e.draw.sprite = "CharactersPlayerNormalLargeJumping";
                }
                if (currentGrowth == 2)
                {
                    e.draw.sprite = "CharactersPlayerNormalFieryJumping";
                }
            }
        }
        if (currentGrowth == 0)
        {
            e.draw.width = 16;
            e.draw.height = 16;
        }
        else
        {
            e.draw.width = 16;
            e.draw.height = 32;
        }
        if (dead)
        {
            e.draw.sprite = "CharactersPlayerDead";
        }
        {
            if (crouching)
            {
                if (currentGrowth == 1)
                {
                    e.draw.sprite = "CharactersPlayerNormalLargeNormalCrouching";
                }
                if (currentGrowth == 2)
                {
                    e.draw.sprite = "CharactersPlayerNormalFieryNormalCrouching";
                }
                e.draw.height = 32 - 8;
            }
            if (controls.down && growth > 0 && (!crouching) && onGround)
            {
                e.draw.y += 8;
                crouching = true;
            }
            if ((!controls.down) && crouching)
            {
                e.draw.y -= 9;
                crouching = false;
            }
            //if (jumptime != -1)
            //{
            //    crouching = false;
            //}
        }
        if (velX < 0)
        {
            lookLeft = true;
        }
        if (velX > 0)
        {
            lookLeft = false;
        }
        if (lookLeft)
        {
            e.draw.mirror = MirrorType.MirrorX;
        }
        else
        {
            e.draw.mirror = MirrorType.None;
        }
    }

    bool flagpoleClimbingPlayerDone;
    void FlagPoleClimbing(Game game, Entity e)
    {
        if (e.flagpoleClimbing != null)
        {
            if (e.draw.y < e.flagpoleClimbing.endY)
            {
                e.draw.y += one / 2;
            }
            else
            {
                flagpoleClimbingPlayerDone = true;
            }
            if (e.flagpoleClimbing.flagDone)
            {
                flagpoleClimbingPlayerDone = false;
                e.flagpoleClimbing = null;
            }
        }
    }
}

public class AttackHelper
{
    public static PushSide Attack(Game game, float oldx, float oldy, float x, float y, float w, float h, bool bigMario, bool keyDownPressed)
    {
        PushSide side = PushSide.None;
        for (int i = 0; i < game.entitiesCount; i++)
        {
            Entity e = game.entities[i];
            if (e == null)
            {
                continue;
            }
            if (e.draw == null)
            {
                continue;
            }
            //if (e.collider == null)
            //{
            //    continue;
            //}
            PushType pushType = bigMario ? PushType.BigMario : PushType.SmallMario;
            if (Misc.RectIntersect(x, y, w, h, e.draw.x, e.draw.y, e.draw.width, e.draw.height))
            {
                if (e.attackablePush != null)
                {
                    bool up = y < oldy;
                    if (up && y < e.draw.y + e.draw.height && y > e.draw.y + e.draw.height - 3)
                    {
                        if (e.attackablePush.pushSide == PushSide.BottomBrickDestroy)
                        {
                            if (IsEmptyBelow(game, i))
                            {
                                e.attackablePush.pushed = pushType;
                                side = PushSide.BottomBrickDestroy;
                            }
                        }
                    }
                    bool down = y > oldy;
                    if (down && y + h >= e.draw.y && y + h < e.draw.y + 3)
                    {
                        if (e.attackablePush.pushSide == PushSide.TopJumpOnEnemy)
                        {
                            e.attackablePush.pushed = pushType;
                            side = PushSide.TopJumpOnEnemy;
                        }
                        if (e.attackablePush.pushSide == PushSide.TopPipeKeyDown && keyDownPressed)
                        {
                            // Only on center of pipe
                            if (x + w / 2 > e.draw.x + e.draw.width / 3
                                && x + w / 2 < e.draw.x + e.draw.width - e.draw.width / 3)
                            {
                                e.attackablePush.pushed = pushType;
                                side = PushSide.TopPipeKeyDown;
                            }
                        }
                        if (e.attackablePush.pushSide == PushSide.LeftRightKoopa)
                        {
                            e.attackablePush.pushed = pushType;
                            if (x + w / 2 > e.draw.x + e.draw.width / 2)
                            {
                                e.attackablePush.pushSideLeftRight = PushSideLeftRight.Right;
                            }
                            else
                            {
                                e.attackablePush.pushSideLeftRight = PushSideLeftRight.Left;
                            }
                        }
                    }
                    bool right = x > oldx;
                    if (right && x + w >= e.draw.x && x + w < e.draw.x + 3)
                    {
                        if (e.attackablePush.pushSide == PushSide.LeftPipeEnter)
                        {
                            e.attackablePush.pushed = pushType;
                            side = PushSide.LeftPipeEnter;
                        }
                        if (e.attackablePush.pushSide == PushSide.LeftRightKoopa)
                        {
                            e.attackablePush.pushed = pushType;
                            e.attackablePush.pushSideLeftRight = PushSideLeftRight.Left;
                        }
                    }
                    bool left = x < oldx;
                    if (left && x < e.draw.x + e.draw.width && x > e.draw.x + e.draw.width - 3)
                    {
                        if (e.attackablePush.pushSide == PushSide.Right)
                        {
                            e.attackablePush.pushed = pushType;
                            side = PushSide.Right;
                        }
                        if (e.attackablePush.pushSide == PushSide.LeftRightKoopa)
                        {
                            e.attackablePush.pushed = pushType;
                            e.attackablePush.pushSideLeftRight = PushSideLeftRight.Right;
                        }
                    }
                }
            }
        }
        return side;
    }

    static bool IsEmptyBelow(Game game, int entity)
    {
        Entity e = game.entities[entity];
        for (int i = 0; i < game.entitiesCount; i++)
        {
            Entity e2 = game.entities[i];
            if (e2 == null) { continue; }
            if (e2.collider == null) { continue; }
            if (Misc.RectIntersect(e.draw.x, e.draw.y + e.draw.height,
                e.draw.width, 1,
                e2.draw.x, e2.draw.y, e2.draw.width * e2.draw.xrepeat, e2.draw.height * e2.draw.yrepeat)
                )
            {
                return false;
            }
        }
        return true;
    }
}

public class Controls
{
    public Controls()
    {
        Clear();
    }

    internal bool left;
    internal bool right;
    internal bool jump;
    internal bool fire;
    internal bool down;

    internal void Clear()
    {
        left = false;
        right = false;
        jump = false;
        fire = false;
        down = false;
    }

    internal void CopyFrom(Controls other)
    {
        left = other.left;
        right = other.right;
        jump = other.jump;
        fire = other.fire;
        down = other.down;
    }
}
