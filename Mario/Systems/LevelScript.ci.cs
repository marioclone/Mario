﻿public class SystemLevelScript : GameSystem
{
    public override void Update(Game game, float dt)
    {
        if (game.level == "1-2-0")
        {
            game.gameStarted = true;
            game.controlsOverrideActive = true;
            game.controlsOverride.right = true;
        }
    }
}
