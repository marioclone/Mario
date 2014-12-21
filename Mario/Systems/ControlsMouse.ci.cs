public class SystemControlsMouse : GameSystem
{
    public SystemControlsMouse()
    {
        downTime = -1;
    }

    float downTime;

    public override void OnMouseDown(Game game, MouseEventArgs e)
    {
        if (e.GetButton() == MouseButtonEnum.Right)
        {
            game.controls.jump = true;
        }
        if (e.GetButton() == MouseButtonEnum.Left)
        {
            if (e.GetX() > one * (game.playerx + 8 - game.scrollx) * game.scale)
            {
                game.controls.left = false;
                game.controls.right = true;
            }
            else
            {
                game.controls.left = true;
                game.controls.right = false;
            }
        }
        if (e.GetButton() == MouseButtonEnum.Middle)
        {
            game.controls.fire = true;
        }
        game.gameStarted = true;
    }

    public override void OnMouseUp(Game game, MouseEventArgs e)
    {
        if (e.GetButton() == MouseButtonEnum.Right)
        {
            game.controls.jump = false;
        }
        if (e.GetButton() == MouseButtonEnum.Left)
        {
            game.controls.left = false;
            game.controls.right = false;
        }
        if (e.GetButton() == MouseButtonEnum.Middle)
        {
            game.controls.fire = false;
        }
    }

    public override void OnMouseWheel(Game game, MouseWheelEventArgs e)
    {
        if (e.GetDelta() < 0)
        {
            game.controls.down = true;
            downTime = 0;
        }
    }

    public override void Update(Game game, float dt)
    {
        if (downTime != -1)
        {
            downTime += dt;
        }
        if (downTime > one / 2)
        {
            downTime = -1;
            game.controls.down = false;
        }
    }
}
