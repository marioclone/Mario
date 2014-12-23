public class SystemDrawLoading : GameSystem
{
    public override void Render(Game game, float dt)
    {
        if (game.assetsLoaded.value == 1)
        {
            if (textEntity != null)
            {
                game.DeleteEntity_(textEntity);
                textEntity = null;
            }
            return;
        }
        if (text == null)
        {
            textEntity = new Entity();
            text = new ScriptDrawText();
            text.text = new int[7];
            text.textLength = 7;
            text.text[0] = CharType.CharL;
            text.text[1] = CharType.CharO;
            text.text[2] = CharType.CharA;
            text.text[3] = CharType.CharD;
            text.text[4] = CharType.CharI;
            text.text[5] = CharType.CharN;
            text.text[6] = CharType.CharG;
            textEntity.scripts[textEntity.scriptsCount++] = text;
            game.AddEntity(textEntity);
        }
        text.x = game.platform.FloatToInt(game.gameScreenWidth) / 2 - 8 * 4;
        text.y = game.platform.FloatToInt(game.gameScreenHeight / 2 - 4 - game.addY);
    }
    ScriptDrawText text;
    Entity textEntity;
}
