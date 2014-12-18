public class GameSystem
{
    public GameSystem()
    {
        one = 1;
    }
    internal float one;
    public virtual void Render(Game game, float dt) { }
    public virtual void Update(Game game, float dt) { }
    
    public virtual void OnKeyDown(Game game, KeyEventArgs e) { }
    public virtual void OnKeyPress(Game game, KeyPressEventArgs e) { }
    public virtual void OnKeyUp(Game game, KeyEventArgs e) { }
    public virtual void OnMouseDown(Game game, MouseEventArgs e) { }
    public virtual void OnMouseUp(Game game, MouseEventArgs e) { }
    public virtual void OnMouseMove(Game game, MouseEventArgs e) { }
    public virtual void OnMouseWheel(Game game, MouseWheelEventArgs e) { }
    public virtual void OnTouchStart(Game game, TouchEventArgs e) { }
    public virtual void OnTouchMove(Game game, TouchEventArgs e) { }
    public virtual void OnTouchEnd(Game game, TouchEventArgs e) { }
    public virtual void OnFocusChanged(Game game, bool focus) { }
}

public abstract class Script
{
    public Script()
    {
        one = 1;
    }
    internal float one;
    public abstract void Update(Game game, int entity, float dt);
    public virtual void Delete(Game game) { }
}

public class ArrayUtil
{
    public static int Index2d(int x, int y, int sizex)
    {
        return x + y * sizex;
    }
}

public class Misc
{
    public static byte IntToByte(int a)
    {
#if CITO
        return a.LowByte;
#else
        return (byte)a;
#endif
    }

    public static int ColorFromArgb(int a, int r, int g, int b)
    {
        int iCol = (a << 24) | (r << 16) | (g << 8) | b;
        return iCol;
    }

    public static int ColorA(int color)
    {
        byte a = IntToByte(color >> 24);
        return a;
    }

    public static int ColorR(int color)
    {
        byte r = IntToByte(color >> 16);
        return r;
    }

    public static int ColorG(int color)
    {
        byte g = IntToByte(color >> 8);
        return g;
    }

    public static int ColorB(int color)
    {
        byte b = IntToByte(color);
        return b;
    }

    public static float GetPi()
    {
        float a = 3141592;
        return a / 1000000;
    }

    public static bool RectIntersect(float x1, float y1, float w1, float h1,
        float x2, float y2, float w2, float h2)
    {
        float aRight = x1 + w1;
        float aBottom = y1 + h1;
        float bRight = x2 + w2;
        float bBottom = y2 + h2;
        return (x1 < bRight &&
                x2 < aRight &&
                y1 < bBottom &&
                y2 < aBottom);
    }

    public static float MakeCloserToZero(float a, float b)
    {
        if (a > 0)
        {
            float c = a - b;
            if (c < 0)
            {
                c = 0;
            }
            return c;
        }
        else
        {
            float c = a + b;
            if (c > 0)
            {
                c = 0;
            }
            return c;
        }
    }

    public static bool StringEquals(string strA, string strB)
    {
        if (strA == null && strB == null)
        {
            return true;
        }
        if (strA == null || strB == null)
        {
            return false;
        }
        return strA == strB;
    }
}

public class CollisionHelper
{
    public static bool IsEmpty(Game game, int exceptEntity, float x, float y, float w, float h)
    {
        for (int i = 0; i < game.entitiesCount; i++)
        {
            if (i == exceptEntity)
            {
                continue;
            }
            if (game.entities[i] == null)
            {
                continue;
            }
            Entity e = game.entities[i];
            if (e.collider == null)
            {
                continue;
            }
            if (Misc.RectIntersect(x, y, w, h, e.draw.x, e.draw.y, e.draw.width * e.draw.xrepeat, e.draw.height * e.draw.yrepeat))
            {
                return false;
            }
        }
        //if (y > 240 - h)
        //{
        //    return false;
        //}
        if (x < 0)
        {
            return false;
        }
        return true;
    }
}

public class AssetsHelper
{
    public static int GetAssetId(GamePlatform platform, Assets assets, string nameNoExtension)
    {
        for (int i = 0; i < assets.count; i++)
        {
            if (i == 152)
            {
            }
            if (FileNameEquals(platform, assets.name[i], nameNoExtension))
            {
                return i;
            }
        }
        return -1;
    }

    public static bool FileNameEquals(GamePlatform platform, string aWithExtension, string bNoExtension)
    {
        int aLength = platform.StringLength(aWithExtension);
        int bLength = platform.StringLength(bNoExtension);
        if (aLength != bLength + 4)
        {
            return false;
        }
        for (int i = 0; i < aLength - 4; i++)
        {
            if (aWithExtension[i] != bNoExtension[i])
            {
                return false;
            }
        }
        return true;
    }
}
