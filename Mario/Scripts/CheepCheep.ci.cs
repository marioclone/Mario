public class ScriptCheepCheep : Script
{
    public override void Update(Game game, int entity, float dt)
    {
        Entity e = game.entities[entity];
        HelperAttackWithTouch.Update(game, e);
    }
}

public class SpawnCheepCheep
{
    public static void Spawn(Game game, int x, int y)
    {
        Entity e = SystemSpawn.Spawn(game, "CharactersCheepCheepNormalNormal", x, y);
        e.scripts[e.scriptsCount++] = new ScriptCheepCheep();
    }
}
