public abstract class GamePlatform
{
    // Primitive
    public abstract int FloatToInt(float value);
    public abstract float MathSqrt(float p);
    public abstract int StringLength(string a);

    public abstract string StringTrim(string s);
    public abstract bool StringStartsWithIgnoreCase(string a, string b);
    public abstract string StringReplace(string s, string from, string to);
    public abstract string[] StringSplit(string value, string separator, IntRef returnLength);
    public abstract bool FloatTryParse(string s, FloatRef ret);
    public abstract string StringFromUtf8ByteArray(byte[] value, int valueLength);

    // Window
    public abstract void AddEventHandler(WindowEventHandler handler);
    public abstract int GetCanvasWidth();
    public abstract int GetCanvasHeight();

    // Bitmap
    public abstract BitmapCi BitmapCreate(int width, int height);
    public abstract void BitmapSetPixelsArgb(BitmapCi bmp, int[] pixels);
    public abstract float BitmapGetWidth(BitmapCi bmp);
    public abstract float BitmapGetHeight(BitmapCi bmp);
    public abstract void BitmapDelete(BitmapCi bmp);
    public abstract BitmapCi BitmapCreateFromPng(byte[] data, int dataLength);
    public abstract bool BitmapIsLoaded(BitmapCi bmp);
    public abstract void BitmapGetPixelsArgb(BitmapCi bitmap, int[] bmpPixels);
    
    // 2D
    public abstract void FillRect(int x, int y, int width, int height, int r, int g, int b);
    public abstract void DrawBitmap(BitmapCi bmp, int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh);

    // Audio
    public abstract AudioData AudioDataCreate(byte[] data, int dataLength);
    public abstract AudioCi AudioCreate(AudioData data);
    public abstract void AudioPlay(AudioCi audio);
    public abstract void AudioPause(AudioCi audio);
    public abstract bool AudioFinished(AudioCi audio);
    public abstract void LoadAssetsAsyc(AssetList list, FloatRef progress);
}

public class Asset
{
    internal string name;
    internal string md5;
    internal byte[] data;
    internal int dataLength;

    public string GetName() { return name; } public void SetName(string value) { name = value; }
    public string GetMd5() { return md5; }public void SetMd5(string value) { md5 = value; }
    public byte[] GetData() { return data; } public void SetData(byte[] value) { data = value; }
    public int GetDataLength() { return dataLength; } public void SetDataLength(int value) { dataLength = value; }
}

public class AssetList
{
    internal Asset[] items;
    internal int count;

    public Asset[] GetItems() { return items; } public void SetItems(Asset[] value) { items = value; }
    public int GetCount() { return count; } public void SetCount(int value) { count = value; }
}

public abstract class AudioData
{
}

public abstract class AudioCi
{
}

public class IntRef
{
    public static IntRef Create(int value_)
    {
        IntRef intref = new IntRef();
        intref.value = value_;
        return intref;
    }
    internal int value;
    public int GetValue() { return value; }
    public void SetValue(int value_) { value = value_; }
}

public class FloatRef
{
    public static FloatRef Create(float value_)
    {
        FloatRef f = new FloatRef();
        f.value = value_;
        return f;
    }
    internal float value;

    public float GetValue() { return value; }
    public void SetValue(float value_) { value = value_; }
}

public class BitmapCi
{
}

public class KeyEventArgs
{
    int keyCode;
    public int GetKeyCode() { return keyCode; }
    public void SetKeyCode(int value) { keyCode = value; }
    bool handled;
    public bool GetHandled() { return handled; }
    public void SetHandled(bool value) { handled = value; }
}

public class KeyPressEventArgs
{
    int keyChar;
    public int GetKeyChar() { return keyChar; }
    public void SetKeyChar(int value) { keyChar = value; }
    bool handled;
    public bool GetHandled() { return handled; }
    public void SetHandled(bool value) { handled = value; }
}

public class GlKeys
{
    public const int Left = 37;
    public const int Up = 38;
    public const int Right = 39;
    public const int Down = 40;
    public const int Enter = 13;
    public const int Escape = 27;
    public const int Space = 32;
    public const int Comma = 188;
    public const int Period = 190;
}

public class WindowEventHandler
{
    public virtual void OnNewFrame(NewFrameEventArgs args) { }
    public virtual void OnKeyDown(KeyEventArgs e) { }
    public virtual void OnKeyPress(KeyPressEventArgs e) { }
    public virtual void OnKeyUp(KeyEventArgs e) { }
    public virtual void OnMouseDown(MouseEventArgs e) { }
    public virtual void OnMouseUp(MouseEventArgs e) { }
    public virtual void OnMouseMove(MouseEventArgs e) { }
    public virtual void OnMouseWheel(MouseWheelEventArgs e) { }
    public virtual void OnTouchStart(TouchEventArgs e) { }
    public virtual void OnTouchMove(TouchEventArgs e) { }
    public virtual void OnTouchEnd(TouchEventArgs e) { }
    public virtual void OnFocusChanged(bool focus) { }
}

public abstract class ImageOnLoadHandler
{
    public abstract void OnLoad();
}

public class MouseEventArgs
{
    int x;
    int y;
    int movementX;
    int movementY;
    int button;
    public int GetX() { return x; } public void SetX(int value) { x = value; }
    public int GetY() { return y; } public void SetY(int value) { y = value; }
    public int GetMovementX() { return movementX; } public void SetMovementX(int value) { movementX = value; }
    public int GetMovementY() { return movementY; } public void SetMovementY(int value) { movementY = value; }
    public int GetButton() { return button; } public void SetButton(int value) { button = value; }
    bool handled;
    public bool GetHandled() { return handled; }
    public void SetHandled(bool value) { handled = value; }
}

public class MouseWheelEventArgs
{
    int delta;
    float deltaPrecise;
    public int GetDelta() { return delta; } public void SetDelta(int value) { delta = value; }
    public float GetDeltaPrecise() { return deltaPrecise; } public void SetDeltaPrecise(float value) { deltaPrecise = value; }
}

public class MouseButtonEnum
{
    public const int Left = 0;
    public const int Middle = 1;
    public const int Right = 2;
}

public class TouchEventArgs
{
    int x;
    int y;
    int id;
    bool handled;
    public int GetX() { return x; } public void SetX(int value) { x = value; }
    public int GetY() { return y; } public void SetY(int value) { y = value; }
    public int GetId() { return id; } public void SetId(int value) { id = value; }
    public bool GetHandled() { return handled; } public void SetHandled(bool value) { handled = value; }
}

public class NewFrameEventArgs
{
    float dt;
    public float GetDt()
    {
        return dt;
    }
    public void SetDt(float p)
    {
        this.dt = p;
    }
}
