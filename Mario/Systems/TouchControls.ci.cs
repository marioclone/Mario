﻿public class SystemTouchControls : GameSystem
{
    public SystemTouchControls()
    {
        enabled = false;
        loaded = false;
        left = null;
        right = null;
        jump = null;
        fire = null;
        down = null;
    }

    bool enabled;
    bool loaded;
    Entity left;
    Entity right;
    Entity jump;
    Entity fire;
    Entity down;

    public override void Render(Game game, float dt)
    {
        if (!enabled)
        {
            return;
        }

        bool found = false;
        for (int i = 0; i < game.entitiesCount; i++)
        {
            if (left != null && game.entities[i] == left)
            {
                found = true;
            }
        }

        if ((!loaded) || (!found))
        {
            loaded = true;
            left = AddButton(game, "ButtonLeft");
            right = AddButton(game, "ButtonRight");
            jump = AddButton(game, "ButtonJump");
            fire = AddButton(game, "ButtonFire");
            down = AddButton(game, "ButtonDown");
        }

        int touchBelow = 0;
        if (game.platform.GetCanvasHeight() > game.platform.GetCanvasWidth())
        {
            // In vertical screen orientation, place touch buttons below game.
            touchBelow = 64;
        }

        left.draw.x = 0 * 16;
        left.draw.y = 240 - 4 * 16 + touchBelow;
        right.draw.x = 4 * 16;
        right.draw.y = 240 - 4 * 16 + touchBelow;
        jump.draw.x = game.gameScreenWidth - 4 * 16;
        jump.draw.y = 240 - 4 * 16 + touchBelow;
        fire.draw.x = game.gameScreenWidth - 4 * 16;
        fire.draw.y = 240 - 8 * 16 + touchBelow;
        down.draw.x = game.gameScreenWidth - 8 * 16;
        down.draw.y = 240 - 4 * 16 + touchBelow;
    }

    public const int TouchButtonsHeight = 128;

    Entity AddButton(Game game, string sprite)
    {
        Entity e = new Entity();
        e.draw = new EntityDraw();
        e.draw.absoluteScreenPosition = true;
        e.draw.sprite = sprite;
        e.draw.width = 64;
        e.draw.height = 64;
        e.draw.z = 3;
        game.entities[game.entitiesCount++] = e;
        return e;
    }

    int touchLeft;
    int touchRight;
    int touchJump;
    int touchFire;
    int touchDown;

    public override void OnTouchStart(Game game, TouchEventArgs e)
    {
        enabled = true;
        StartTouch(game, e);
    }

    void StartTouch(Game game, TouchEventArgs e)
    {
        if (ButtonPressed(game, e.GetX(), e.GetY(), left))
        {
            game.keysDown[GlKeys.Left] = true;
            touchLeft = e.GetId();
        }
        if (ButtonPressed(game, e.GetX(), e.GetY(), right))
        {
            game.keysDown[GlKeys.Right] = true;
            touchRight = e.GetId();
        }
        if (ButtonPressed(game, e.GetX(), e.GetY(), jump))
        {
            game.keysDown[GlKeys.Up] = true;
            touchJump = e.GetId();
        }
        if (ButtonPressed(game, e.GetX(), e.GetY(), fire))
        {
            game.keysDown[GlKeys.Comma] = true;
            touchFire = e.GetId();
        }
        if (ButtonPressed(game, e.GetX(), e.GetY(), down))
        {
            game.keysDown[GlKeys.Down] = true;
            touchDown = e.GetId();
        }
    }

    static bool ButtonPressed(Game game, int x, int y, Entity e)
    {
        return PointInRect(x, y - game.addY, e.draw.x * game.scale, e.draw.y * game.scale, e.draw.width * game.scale, e.draw.height * game.scale);
    }

    static bool PointInRect(float x, float y, float rx, float ry, float rw, float rh)
    {
        return x >= rx && x < rx + rw
            && y >= ry && y < ry + rh;
    }

    public override void OnTouchMove(Game game, TouchEventArgs e)
    {
        EndTouch(game, e);
        StartTouch(game, e);
    }

    void EndTouch(Game game, TouchEventArgs e)
    {
        if (e.GetId() == touchLeft)
        {
            game.keysDown[GlKeys.Left] = false;
            touchLeft = -1;
        }
        if (e.GetId() == touchRight)
        {
            game.keysDown[GlKeys.Right] = false;
            touchRight = -1;
        }
        if (e.GetId() == touchJump)
        {
            game.keysDown[GlKeys.Up] = false;
            touchJump = -1;
        }
        if (e.GetId() == touchFire)
        {
            game.keysDown[GlKeys.Comma] = false;
            touchFire = -1;
        }
        if (e.GetId() == touchDown)
        {
            game.keysDown[GlKeys.Down] = false;
            touchDown = -1;
        }
    }

    public override void OnTouchEnd(Game game, TouchEventArgs e)
    {
        EndTouch(game, e);
    }
}