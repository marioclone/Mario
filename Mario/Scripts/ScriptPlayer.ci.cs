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
        growth = 0;
        dead = false;
        deadT = 0;
        invulnerable = 0;
        crouching = false;
        topPipeEnter = -1;
        leftPipeEnter = -1;

        constAcceleration = 50 * one / 10;
        constGravity = 50 * one / 10;
        constMaxVel = 100;
        constAnimSpeed = 10;
        constMaxJumpTime = 2 * one / 10;
        constJumpVelocity = -200;
        constInvulnerableTime = 3;
    }

    float t;
    float velX;
    float velY;
    bool lookLeft;
    bool onGround;
    float jumptime;
    int growth;
    bool dead;
    float deadT;
    float invulnerable;
    bool crouching;
    float topPipeEnter;
    float leftPipeEnter;

    float constAcceleration;
    float constGravity;
    float constMaxVel;
    float constAnimSpeed;
    float constMaxJumpTime;
    float constJumpVelocity;
    float constInvulnerableTime;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];

        UpdateSprite(game, dt, e);

        if (!game.levelStatic)
        {
            // Scroll
            float scrollX = e.draw.x - 256 / 2 + 8;
            if (scrollX >= game.scrollx)
            {
                game.scrollx = scrollX;
            }
        }

        if (game.gamePaused) { return; }

        bool crouchingSlide = crouching && onGround;

        if (game.keysDown[GlKeys.Left] && (!crouchingSlide))
        {
            velX -= constAcceleration;
        }
        if (game.keysDown[GlKeys.Right] && (!crouchingSlide))
        {
            velX += constAcceleration;
        }

        velX = Misc.MakeCloserToZero(velX, one / 1);
        velY = Misc.MakeCloserToZero(velY, one / 1);

        if (velX > constMaxVel) { velX = constMaxVel; }
        if (velX < -constMaxVel) { velX = -constMaxVel; }

        velY += constGravity;

        // Jump
        {
            bool jumpKeyPressed = game.keysDown[GlKeys.Period] || game.keysDown[GlKeys.Up];

            if (jumpKeyPressed && onGround && (!dead))
            {
                jumptime = 0;
                if (growth == 0)
                {
                    game.AudioPlay("Jump");
                }
                else
                {
                    game.AudioPlay("JumpBig");
                }
            }

            if (jumpKeyPressed && (onGround || (jumptime >= 0 && jumptime < constMaxJumpTime)))
            {
                velY = constJumpVelocity;
            }

            if (!jumpKeyPressed)
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
        }
        if (leftPipeEnter != -1)
        {
            velX = constMaxVel / 2;
        }

        // Move
        float oldx = e.draw.x;
        float oldy = e.draw.y;

        float newx = e.draw.x + velX * dt;
        float newy = e.draw.y + velY * dt;

        bool keyDownPressed = game.keysDown[GlKeys.Down];

        PushSide side = AttackHelper.Attack(game, e.draw.x, e.draw.y, newx, newy, e.draw.width, e.draw.height, growth > 0, keyDownPressed);
        if (side == PushSide.TopJumpOnEnemy)
        {
            // Bump
            jumptime = 0;
            velY = constJumpVelocity;
        }

        if (side == PushSide.TopPipeKeyDown)
        {
            topPipeEnter = 0;
        }

        if (side == PushSide.LeftPipeEnter)
        {
            leftPipeEnter = 0;
        }


        if (CollisionHelper.IsEmpty(game, -1, newx, e.draw.y, e.draw.width, e.draw.height)
            && newx >= game.scrollx)
        {
            e.draw.x = newx;
        }

        if (CollisionHelper.IsEmpty(game, -1, e.draw.x, newy, e.draw.width, e.draw.height))
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
            if (growth > 2)
            {
                growth = 2;
            }
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
            }
        }

        // Touch coins
        for (int i = 0; i < game.entitiesCount; i++)
        {
            Entity e2 = game.entities[i];
            if (e2 == null) { continue; }
            if (e2 == e) { continue; }
            if (e2.attackableTouch == null) { continue; }
            if (Misc.RectIntersect(e.draw.x, e.draw.y, e.draw.width, e.draw.height,
                e2.draw.x, e2.draw.y, e2.draw.width, e2.draw.height))
            {
                e2.attackableTouch.touched = true;
                return;
            }
        }
    }

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

    void UpdateSprite(Game game, float dt, Entity e)
    {
        // Sprite
        if (velX == 0)
        {
            if (growth == 0)
            {
                e.draw.sprite = "CharactersPlayerNormalNormalNormalNormal";
            }
            if (growth == 1)
            {
                e.draw.sprite = "CharactersPlayerNormalLargeNormalNormal";
            }
            if (growth == 2)
            {
                e.draw.sprite = "CharactersPlayerNormalFieryNormalNormal";
            }
            t = 0;
        }
        else
        {
            if (!game.gamePaused)
            {
                t += dt;
            }
            float stagesCount = 4;
            int stage = game.platform.FloatToInt(t * constAnimSpeed % stagesCount);
            if (growth == 0)
            {
                if (stage == 0) { e.draw.sprite = "CharactersPlayerNormalNormalNormalRunningNormalTwo"; }
                if (stage == 1) { e.draw.sprite = "CharactersPlayerNormalNormalNormalRunningNormalNormal"; }
                if (stage == 2) { e.draw.sprite = "CharactersPlayerNormalNormalNormalRunningNormalTwo"; }
                if (stage == 3) { e.draw.sprite = "CharactersPlayerNormalNormalNormalRunningNormalThree"; }
            }
            if (growth == 1)
            {
                if (stage == 0) { e.draw.sprite = "CharactersPlayerNormalLargeNormalRunningNormalTwo"; }
                if (stage == 1) { e.draw.sprite = "CharactersPlayerNormalLargeNormalRunningNormalNormal"; }
                if (stage == 2) { e.draw.sprite = "CharactersPlayerNormalLargeNormalRunningNormalTwo"; }
                if (stage == 3) { e.draw.sprite = "CharactersPlayerNormalLargeNormalRunningNormalThree"; }
            }
            if (growth == 2)
            {
                if (stage == 0) { e.draw.sprite = "CharactersPlayerNormalFieryNormalRunningNormalTwo"; }
                if (stage == 1) { e.draw.sprite = "CharactersPlayerNormalFieryNormalRunningNormalNormal"; }
                if (stage == 2) { e.draw.sprite = "CharactersPlayerNormalFieryNormalRunningNormalTwo"; }
                if (stage == 3) { e.draw.sprite = "CharactersPlayerNormalFieryNormalRunningNormalThree"; }
            }
        }
        if (!onGround)
        {
            if (growth == 0)
            {
                e.draw.sprite = "CharactersPlayerNormalNormalJumping";
            }
            if (growth == 1)
            {
                e.draw.sprite = "CharactersPlayerNormalLargeJumping";
            }
            if (growth == 2)
            {
                e.draw.sprite = "CharactersPlayerNormalFieryJumping";
            }
        }
        if (growth == 0)
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
                if (growth == 1)
                {
                    e.draw.sprite = "CharactersPlayerNormalLargeNormalCrouching";
                }
                if (growth == 2)
                {
                    e.draw.sprite = "CharactersPlayerNormalFieryNormalCrouching";
                }
                e.draw.height = 32 - 8;
            }
            if (game.keysDown[GlKeys.Down] && growth > 0 && (!crouching))
            {
                e.draw.y += 8;
                crouching = true;
            }
            if (!game.keysDown[GlKeys.Down] && crouching)
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
        e.draw.mirrorx = lookLeft;
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
                            e.attackablePush.pushed = pushType;
                            side = PushSide.BottomBrickDestroy;
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
                    }
                    bool right = x > oldx;
                    if (right && x + w >= e.draw.x && x + w < e.draw.x + 3)
                    {
                        if (e.attackablePush.pushSide == PushSide.LeftPipeEnter)
                        {
                            e.attackablePush.pushed = pushType;
                            side = PushSide.LeftPipeEnter;
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
                    }
                }
            }
        }
        return side;
    }
}
