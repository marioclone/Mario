public class ScriptQuestionBlock : Script
{
    public ScriptQuestionBlock()
    {
        onUse = null;
        used = false;
        constAnimSpeed = 6;
    }

    internal ActionSpawnThing onUse;
    bool used;
    float constAnimSpeed;

    public override void Update(Game game, int entity, float dt)
    {
        if (game.gamePaused) { return; }
        Entity e = game.entities[entity];

        // Animation
        if (!used)
        {
            int stage = (game.platform.FloatToInt(game.t * constAnimSpeed) % 5);
            if (stage == 0) { e.draw.sprite = "SolidsBlockNormalNormalNormal"; }
            if (stage == 1) { e.draw.sprite = "SolidsBlockNormalNormalNormal"; }
            if (stage == 2) { e.draw.sprite = "SolidsBlockNormalNormalTwo"; }
            if (stage == 3) { e.draw.sprite = "SolidsBlockNormalNormalThree"; }
            if (stage == 4) { e.draw.sprite = "SolidsBlockNormalNormalTwo"; }
        }

        AttackEntitiesAboveBumpedBrick.Update(game, e);

        // When player jumps from bottom, spawn a Thing.
        if (!used)
        {
            if (e.attackablePush.pushed != PushType.None)
            {
                e.draw.sprite = "SolidsBlockNormalUsed";
                used = true;
                e.draw.hidden = false;
                onUse.Spawn(game);

                // remove ScriptBlock
                // remove ScriptBrickBumpAnimation
                for (int i = 0; i < e.scriptsCount; i++)
                {
                    if (e.scripts[i] != null)
                    {
                        e.scripts[i].Delete(game);
#if CITO
                        delete e.scripts[i];
#endif
                        e.scripts[i] = null;
                    }
                }
                e.scriptsCount = 0;
            }
        }
    }
}
