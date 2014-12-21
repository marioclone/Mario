public class SystemControlsKeyboard : GameSystem
{
    public SystemControlsKeyboard()
    {
    }

    public override void OnKeyDown(Game game, KeyEventArgs e)
    {
        if (e.GetKeyCode() < 0 || e.GetKeyCode() > 255)
        {
            return;
        }
        game.keysDown[e.GetKeyCode()] = true;
        if (e.GetKeyCode() == GlKeys.Enter)
        {
            game.gamePaused = !game.gamePaused;
            game.AudioPlay("Pause");
        }
        game.gameStarted = true;
        SetControls(game);
    }

    public override void OnKeyUp(Game game, KeyEventArgs e)
    {
        if (e.GetKeyCode() < 0 || e.GetKeyCode() > 255)
        {
            return;
        }
        game.keysDown[e.GetKeyCode()] = false;
        SetControls(game);
    }

    void SetControls(Game game)
    {
        game.controls.left = game.keysDown[GlKeys.Left];
        game.controls.right = game.keysDown[GlKeys.Right];
        game.controls.jump = game.keysDown[GlKeys.Period] || game.keysDown[GlKeys.Up];
        game.controls.fire = game.keysDown[GlKeys.Comma];
        game.controls.down = game.keysDown[GlKeys.Down];
    }
}
