public class ScriptBrickDestroy : Script
{
    public ScriptBrickDestroy()
    {
    }

    public override void Update(Game game, int entity, float dt)
    {
        if (game.gamePaused) { return; }
        Entity e = game.entities[entity];

        if (e.attackablePush.pushed == PushType.BigMario)
        {
            Spawn_.BrickShards(game, e.draw.x + 4, e.draw.y + 4);
            game.AudioPlay("Blockbreak");
            game.DeleteEntity(entity);
        }
    }
}
