public class ScriptCoinStatic : Script
{
    public ScriptCoinStatic()
    {
        constAnimSpeed = 6;
    }

    float constAnimSpeed;

    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];

        // Touched by player
        if (e.attackableTouch == null)
        {
            e.attackableTouch = new EntityAttackableTouch();
        }
        if (e.attackableTouch.touched)
        {
            game.score += Game.ScoreCoin;
            game.coins++;
            game.AudioPlay("Coin");
            game.DeleteEntity(entity);
        }

        // Animation
        int stage = (game.platform.FloatToInt(game.t * constAnimSpeed) % 5);
        if (stage == 0) { e.draw.sprite = "CharactersCoinNormalNormalNormal"; }
        if (stage == 1) { e.draw.sprite = "CharactersCoinNormalNormalNormal"; }
        if (stage == 2) { e.draw.sprite = "CharactersCoinNormalNormalTwo"; }
        if (stage == 3) { e.draw.sprite = "CharactersCoinNormalNormalThree"; }
        if (stage == 4) { e.draw.sprite = "CharactersCoinNormalNormalTwo"; }
    }
}
