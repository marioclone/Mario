// Draws background
// Draws all EntityDraw
public class SystemDraw : GameSystem
{
    public SystemDraw()
    {
        spritesOrig = new DictionaryStringBitmap();
        sprites = new DictionaryStringBitmap();
        spritesMirrored = new DictionaryStringBitmap();
        oldScale = 2;
    }

    DictionaryStringBitmap spritesOrig;
    DictionaryStringBitmap sprites;
    DictionaryStringBitmap spritesMirrored;

    float oldScale;
    public override void Render(Game game, float dt)
    {
        int canvasWidth = game.platform.GetCanvasWidth();
        int canvasHeight = game.platform.GetCanvasHeight();
        float scale = one * canvasHeight / 240;
        // Round to one pixel, to avoid gaps. After scaling tile size must be an integer.
        scale = (one * game.platform.FloatToInt(scale * 8)) / 8;
        if (scale < 1) { scale = 1; }
        if (scale != oldScale)
        {
            Rescale(game, game.platform);
            oldScale = scale;
        }
        DrawBackground(game);
        int addY = game.platform.FloatToInt(canvasHeight - 240 * scale); // align to screen bottom
        LoadOrigSprites(game);
        for (int z = 0; z < 4; z++)
        {
            for (int i = 0; i < game.entitiesCount; i++)
            {
                Entity e = game.entities[i];
                if (e == null) { continue; }
                if (e.draw == null) { continue; }
                if (e.draw.sprite == null) { continue; }
                if (e.draw.z != z)
                {
                    continue;
                }
                PrepareSprite(game, e.draw, scale);
                if (e.draw.hidden) { continue; }
                for (int x = 0; x < e.draw.xrepeat; x++)
                {
                    for (int y = 0; y < e.draw.yrepeat; y++)
                    {
                        int dx = game.platform.FloatToInt(e.draw.x + e.draw.xOffset + x * e.draw.width);
                        int dy = game.platform.FloatToInt(e.draw.y + e.draw.yOffset + y * e.draw.height);
                        int dw = e.draw.width;
                        int dh = e.draw.height;
                        if (e.draw.loadedSprite == -1)
                        {
                            continue;
                        }
                        DictionaryStringBitmap d;
                        if (e.draw.mirrorx)
                        {
                            d = spritesMirrored;
                        }
                        else
                        {
                            d = sprites;
                        }
                        int finalX = game.platform.FloatToInt(dx * scale);
                        int finalY = game.platform.FloatToInt(dy * scale + addY);
                        if (!e.draw.absoluteScreenPosition)
                        {
                            finalX -= game.platform.FloatToInt(game.scrollx * scale);
                        }
                        BitmapCi bmp = d.GetById(e.draw.loadedSprite);
                        DrawBitmap(game.platform, bmp, 0, 0, game.platform.FloatToInt(e.draw.width * scale), game.platform.FloatToInt(e.draw.height * scale),
                            finalX, finalY, game.platform.FloatToInt(dw * scale), game.platform.FloatToInt(dh * scale), canvasWidth, canvasHeight);
                    }
                }
            }
        }
        game.gameScreenWidth = game.platform.GetCanvasWidth() / scale;
        game.gameScreenHeight = game.platform.GetCanvasHeight() / scale;
    }

    void Rescale(Game game, GamePlatform platform)
    {
        for (int i = 0; i < DictionaryStringBitmap.max; i++)
        {
            if (sprites.keys[i] != null)
            {
                platform.BitmapDelete(sprites.values[i]);
            }
            sprites.keys[i] = null;
            sprites.values[i] = null;

            if (spritesMirrored.keys[i] != null)
            {
                platform.BitmapDelete(spritesMirrored.values[i]);
            }
            spritesMirrored.keys[i] = null;
            spritesMirrored.values[i] = null;
        }
        for (int i = 0; i < game.entitiesCount; i++)
        {
            Entity e = game.entities[i];
            if (e == null) { continue; }
            if (e.draw == null) { continue; }
            e.draw.loadedSprite = -1;
            e.draw.loadedSpriteName = null;
            e.draw.loadedMirrorX = false;
        }
    }

    void DrawBitmap(GamePlatform platform, BitmapCi bmp, int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh, int canvasWidth, int canvasHeight)
    {
        if (Misc.RectIntersect(0, 0, canvasWidth, canvasHeight,
            dx, dy, dw, dh))
        {
            platform.DrawBitmap(bmp, sx, sy, sw, sh, dx, dy, dw, dh);
        }
    }

    void DrawBackground(Game game)
    {
        game.platform.FillRect(0, 0,
            game.platform.GetCanvasWidth(), game.platform.GetCanvasHeight(),
            Misc.ColorR(game.backgroundColor),
            Misc.ColorG(game.backgroundColor),
            Misc.ColorB(game.backgroundColor));
    }

    bool loadedOrigSprites;
    void LoadOrigSprites(Game game)
    {
        if (!loadedOrigSprites)
        {
            loadedOrigSprites = true;
            for (int k = 0; k < game.assets.count; k++)
            {
                string s = game.assets.name[k];
                string sprite = game.platform.StringReplace(s, ".png", "");
                if (s == sprite)
                {
                    continue;
                }
                if (!spritesOrig.Contains(sprite))
                {
                    int assetId = AssetsHelper.GetAssetId(game.platform, game.assets, sprite);
                    BitmapCi bmp;
                    if (assetId != -1)
                    {
                        bmp = game.platform.BitmapCreateFromPng(game.assets.data[assetId], game.assets.length[assetId]);
                    }
                    else
                    {
                        // Asset not found
                        bmp = game.platform.BitmapCreate(16, 16);
                        int[] invalid = new int[16 * 16];
                        for (int i = 0; i < 16 * 16; i++)
                        {
                            invalid[i] = Misc.ColorFromArgb(255, 255, 0, 0);
                        }
                        game.platform.BitmapSetPixelsArgb(bmp, invalid);
                    }
                    spritesOrig.Set(sprite, bmp);
                }
            }
        }
    }

    void PrepareSprite(Game game, EntityDraw draw, float scale)
    {
        if (draw.loadedSprite != -1
            && draw.loadedSpriteName == draw.sprite
            && draw.loadedMirrorX == draw.mirrorx)
        {
            return;
        }

        BitmapCi origSprite = null;
        int origId = spritesOrig.GetId(draw.sprite);
        if (origId == -1)
        {
            return;
        }
        origSprite = spritesOrig.GetById(origId);
        if (!game.platform.BitmapIsLoaded(origSprite))
        {
            return;
        }

        DictionaryStringBitmap d;
        if (draw.mirrorx)
        {
            d = spritesMirrored;
        }
        else
        {
            d = sprites;
        }
        if (!d.Contains(draw.sprite))
        {
            int assetId = AssetsHelper.GetAssetId(game.platform, game.assets, draw.sprite);
            BitmapCi bmp = origSprite;

            BitmapCi bmp2 = ScaleBitmap(game.platform, bmp, scale);
            if (draw.mirrorx)
            {
                BitmapCi bmp3 = MirrorXBitmap(game.platform, bmp2);
                game.platform.BitmapDelete(bmp2);
                bmp2 = bmp3;
            }
            bmp = bmp2;

            d.Set(draw.sprite, bmp);
        }
        draw.loadedSprite = d.GetId(draw.sprite);
        draw.loadedSpriteName = draw.sprite;
        draw.loadedMirrorX = draw.mirrorx;
    }

    BitmapCi ScaleBitmap(GamePlatform platform, BitmapCi a, float scale)
    {
        int width = platform.FloatToInt(platform.BitmapGetWidth(a));
        int height = platform.FloatToInt(platform.BitmapGetHeight(a));

        int[] aPixels = new int[width * height];
        platform.BitmapGetPixelsArgb(a, aPixels);
        BitmapCi b = platform.BitmapCreate(platform.FloatToInt(width * scale), platform.FloatToInt(height * scale));

        int[] bPixels = resizePixels(aPixels, width, height, platform.FloatToInt(width * scale), platform.FloatToInt(height * scale));

        platform.BitmapSetPixelsArgb(b, bPixels);
        return b;
    }

    // http://tech-algorithm.com/articles/nearest-neighbor-image-scaling/
    public int[] resizePixels(int[] pixels, int w1, int h1, int w2, int h2)
    {
        int[] temp = new int[w2 * h2];
        // EDIT: added +1 to account for an early rounding problem
        int x_ratio = ((w1 << 16) / w2) + 1;
        int y_ratio = ((h1 << 16) / h2) + 1;
        //int x_ratio = (int)((w1<<16)/w2) ;
        //int y_ratio = (int)((h1<<16)/h2) ;
        int x2;
        int y2;
        for (int i = 0; i < h2; i++)
        {
            for (int j = 0; j < w2; j++)
            {
                x2 = ((j * x_ratio) >> 16);
                y2 = ((i * y_ratio) >> 16);
                temp[(i * w2) + j] = pixels[(y2 * w1) + x2];
            }
        }
        return temp;
    }

    BitmapCi MirrorXBitmap(GamePlatform platform, BitmapCi a)
    {
        int width = platform.FloatToInt(platform.BitmapGetWidth(a));
        int height = platform.FloatToInt(platform.BitmapGetHeight(a));

        int[] aPixels = new int[width * height];
        platform.BitmapGetPixelsArgb(a, aPixels);
        BitmapCi b = platform.BitmapCreate(width, height);
        int[] bPixels = new int[width * height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bPixels[x + y * width] = aPixels[(width - x - 1) + y * width];
            }
        }
        platform.BitmapSetPixelsArgb(b, bPixels);
        return b;
    }
}
