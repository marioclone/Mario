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
            game.AudioPlay("Blockbreak");
            game.DeleteEntity(entity);
            // Todo: Animation
            // "CharactersBrickShardNormal"
        }
    }
}
