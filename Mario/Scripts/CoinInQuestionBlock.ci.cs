public class ScriptCoinInQuestionBlock : Script
{
    public ScriptCoinInQuestionBlock()
    {
        t = 0;
        coinSpeed = 2;
        constAnimSpeed = 20;
        constCoinMaxHeight = 3;
    }

    float t;
    float constAnimSpeed;
    float constCoinMaxHeight;
    float coinSpeed;

    public override void Update(Game game, int entity, float dt)
    {
        if (game.gamePaused) { return; }
        t += dt;
        Entity e = game.entities[entity];
        int stage = (game.platform.FloatToInt(game.t * constAnimSpeed) % 5);
        if (stage == 0) { e.draw.sprite = "CharactersCoinAnimNormal"; }
        if (stage == 1) { e.draw.sprite = "CharactersCoinAnimNormal"; }
        if (stage == 2) { e.draw.sprite = "CharactersCoinAnimAnim2"; }
        if (stage == 3) { e.draw.sprite = "CharactersCoinAnimAnim3"; }
        if (stage == 4) { e.draw.sprite = "CharactersCoinAnimAnim4"; }
        e.draw.yOffset = -Anim(t * coinSpeed) * constCoinMaxHeight * 16;
        if (t > one / coinSpeed)
        {
            Spawn_.Score(game, e.draw.x, e.draw.y, Game.ScoreCoin);
            game.DeleteEntity(entity);
        }
    }

    float Anim(float time)
    {
        if (time < one / 2)
        {
            return time * 2;
        }
        else
        {
            return 1 - (time - one / 2) * 2;
        }
    }
}
