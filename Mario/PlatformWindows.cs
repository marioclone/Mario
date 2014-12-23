using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace Mario
{
    public class PlatformWindows : GamePlatform
    {
        public PlatformWindows()
        {
        }
        public Form form;

        public void Start()
        {
            form.Paint += new PaintEventHandler(form_Paint);
            form.KeyDown += new System.Windows.Forms.KeyEventHandler(form_KeyDown);
            form.KeyPress += new KeyPressEventHandler(form_KeyPress);
            form.KeyUp += new System.Windows.Forms.KeyEventHandler(form_KeyUp);
            form.MouseDown += new MouseEventHandler(form_MouseDown);
            form.MouseMove += new MouseEventHandler(form_MouseMove);
            form.MouseUp += new MouseEventHandler(form_MouseUp);
            form.MouseWheel += new MouseEventHandler(form_MouseWheel);

            form.Width = 256 * 2;
            form.Height = 240 * 2 + SystemInformation.CaptionHeight + 2;
        }

        void form_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            foreach (var h in handlers)
            {
                MouseEventArgs args = new MouseEventArgs();
                args.SetX(e.X);
                args.SetY(e.Y);
                args.SetButton(GetMouseButton(e.Button));
                h.OnMouseDown(args);
            }
        }
        
        void form_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            foreach (var h in handlers)
            {
                MouseEventArgs args = new MouseEventArgs();
                args.SetX(e.X);
                args.SetY(e.Y);
                args.SetButton(GetMouseButton(e.Button));
                h.OnMouseUp(args);
            }
        }
        
        void form_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            foreach (var h in handlers)
            {
                MouseEventArgs args = new MouseEventArgs();
                args.SetX(e.X);
                args.SetY(e.Y);
                args.SetButton(GetMouseButton(e.Button));
                h.OnMouseMove(args);
            }
        }
        
        void form_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            foreach (var h in handlers)
            {
                MouseWheelEventArgs args = new MouseWheelEventArgs();
                args.SetDelta(e.Delta);
                h.OnMouseWheel(args);
            }
        }
        
        int GetMouseButton(MouseButtons b)
        {
            if (b == MouseButtons.Left) { return MouseButtonEnum.Left; }
            if (b == MouseButtons.Right) { return MouseButtonEnum.Right; }
            if (b == MouseButtons.Middle) { return MouseButtonEnum.Middle; }
            return 0;
        }

        Stopwatch s = new Stopwatch();

        void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                form.Close();
            }
            foreach (var h in handlers)
            {
                KeyEventArgs args = new KeyEventArgs();
                args.SetKeyCode(ToGlKey(e.KeyCode));
                h.OnKeyDown(args);
            }
        }

        int ToGlKey(Keys keys)
        {
            if (keys == Keys.Left) { return GlKeys.Left; }
            if (keys == Keys.Right) { return GlKeys.Right; }
            if (keys == Keys.Up) { return GlKeys.Up; }
            if (keys == Keys.Down) { return GlKeys.Down; }
            if (keys == Keys.OemPeriod) { return GlKeys.Period; }
            // todo
            return (int)keys;
        }

        void form_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            foreach (var h in handlers)
            {
                KeyPressEventArgs args = new KeyPressEventArgs();
                args.SetKeyChar((int)e.KeyChar);
                h.OnKeyPress(args);
            }
        }

        void form_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            foreach (var h in handlers)
            {
                KeyEventArgs args = new KeyEventArgs();
                args.SetKeyCode(ToGlKey(e.KeyCode));
                h.OnKeyUp(args);
            }
        }

        Graphics graphics;
        void form_Paint(object sender, PaintEventArgs e)
        {
            float dt = (float)s.Elapsed.TotalSeconds;
            s.Reset();
            s.Start();

            graphics = e.Graphics;
            var args = new NewFrameEventArgs();
            args.SetDt(dt);
            foreach (var h in handlers)
            {
                h.OnNewFrame(args);
            }
            form.Invalidate();
        }

        public override BitmapCi BitmapCreate(int width, int height)
        {
            BitmapCiCs bmp = new BitmapCiCs();
            bmp.bmp = new Bitmap(width, height);
            return bmp;
        }

        public override void BitmapSetPixelsArgb(BitmapCi bmp, int[] pixels)
        {
            BitmapCiCs bmp_ = (BitmapCiCs)bmp;
            int width = bmp_.bmp.Width;
            int height = bmp_.bmp.Height;
            if (IsMono)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int color = pixels[x + y * width];
                        bmp_.bmp.SetPixel(x, y, Color.FromArgb(color));
                    }
                }
            }
            else
            {
                FastBitmap fastbmp = new FastBitmap();
                fastbmp.bmp = bmp_.bmp;
                fastbmp.Lock();
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        fastbmp.SetPixel(x, y, pixels[x + y * width]);
                    }
                }
                fastbmp.Unlock();
            }
        }

        public override BitmapCi BitmapCreateFromPng(byte[] data, int dataLength)
        {
            BitmapCiCs bmp = new BitmapCiCs();
            try
            {
                bmp.bmp = new Bitmap(new MemoryStream(data, 0, dataLength));
            }
            catch
            {
                bmp.bmp = new Bitmap(1, 1);
                bmp.bmp.SetPixel(0, 0, Color.Orange);
            }
            return bmp;
        }

        public bool IsMono = Type.GetType("Mono.Runtime") != null;

        public override void BitmapGetPixelsArgb(BitmapCi bitmap, int[] bmpPixels)
        {
            BitmapCiCs bmp = (BitmapCiCs)bitmap;
            int width = bmp.bmp.Width;
            int height = bmp.bmp.Height;
            if (IsMono)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        bmpPixels[x + y * width] = bmp.bmp.GetPixel(x, y).ToArgb();
                    }
                }
            }
            else
            {
                FastBitmap fastbmp = new FastBitmap();
                fastbmp.bmp = bmp.bmp;
                fastbmp.Lock();
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        bmpPixels[x + y * width] = fastbmp.GetPixel(x, y);
                    }
                }
                fastbmp.Unlock();
            }
        }

        public override float BitmapGetWidth(BitmapCi bmp)
        {
            BitmapCiCs bmp_ = (BitmapCiCs)bmp;
            return bmp_.bmp.Width;
        }

        public override float BitmapGetHeight(BitmapCi bmp)
        {
            BitmapCiCs bmp_ = (BitmapCiCs)bmp;
            return bmp_.bmp.Height;
        }

        public override void BitmapDelete(BitmapCi bmp)
        {
            BitmapCiCs bmp_ = (BitmapCiCs)bmp;
            bmp_.bmp.Dispose();
        }

        public override bool BitmapIsLoaded(BitmapCi bmp)
        {
            return true;
        }

        public override int GetCanvasWidth()
        {
            return form.Width;
        }

        public override int GetCanvasHeight()
        {
            return form.Height - (SystemInformation.CaptionHeight + 2);
        }

        public override void DrawBitmap(BitmapCi bmp, int sx, int sy, int sw, int sh, int dx, int dy, int dw, int dh)
        {
            Rectangle src = new Rectangle(sx, sy, sw, sh);
            Rectangle dest = new Rectangle(dx, dy, dw, dh);
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            graphics.DrawImage(((BitmapCiCs)bmp).bmp, dest, src, GraphicsUnit.Pixel);
        }

        public override int FloatToInt(float value)
        {
            return (int)value;
        }

        public override float MathSqrt(float p)
        {
            return (float)Math.Sqrt(p);
        }

        public override int StringLength(string a)
        {
            return a.Length;
        }

        List<WindowEventHandler> handlers = new List<WindowEventHandler>();
        public override void AddEventHandler(WindowEventHandler handler)
        {
            handlers.Add(handler);
        }

        public override string StringTrim(string s)
        {
            return s.Trim();
        }

        public override bool StringStartsWithIgnoreCase(string a, string b)
        {
            return a.StartsWith(b, StringComparison.InvariantCultureIgnoreCase);
        }

        public override string StringReplace(string s, string from, string to)
        {
            return s.Replace(from, to);
        }

        public override string[] StringSplit(string value, string separator, IntRef returnLength)
        {
            string[] ret = value.Split(new char[] { separator[0] });
            returnLength.value = ret.Length;
            return ret;
        }

        public override bool FloatTryParse(string s, FloatRef ret)
        {
            float f;
            if (float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out f))
            {
                ret.value = f;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string StringFromUtf8ByteArray(byte[] value, int valueLength)
        {
            string s = Encoding.UTF8.GetString(value, 0, valueLength);
            return s;
        }

        SolidBrush brush;
        public override void FillRect(int x, int y, int width, int height, int r, int g, int b)
        {
            if (brush == null)
            {
                brush = new SolidBrush(Color.FromArgb(r, g, b));
            }
            brush.Color = Color.FromArgb(r, g, b);
            graphics.FillRectangle(brush, x, y, width, height);
        }

        public override AudioData AudioDataCreate(byte[] data, int dataLength)
        {
            return null;
        }

        public override AudioCi AudioCreate(AudioData data)
        {
            return null;
        }

        public override void AudioPlay(AudioCi audio)
        {
        }

        public override void AudioPause(AudioCi audio)
        {
        }

        public override void LoadAssetsAsyc(AssetList list, FloatRef progress)
        {
            progress.value = 1;
        }

        public override bool AudioFinished(AudioCi audio)
        {
            return false;
        }

        public override bool AudioDataLoaded(AudioData data)
        {
            return true;
        }
    }

    public class BitmapCiCs : BitmapCi
    {
        public Bitmap bmp;
    }

    //Doesn't work on Ubuntu - pointer access crashes.
    public class FastBitmap
    {
        public Bitmap bmp { get; set; }
        BitmapData bmd;
        public void Lock()
        {
            if (bmd != null)
            {
                throw new Exception("Already locked.");
            }
            if (bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                bmp = new Bitmap(bmp);
            }
            bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
        }
        public int GetPixel(int x, int y)
        {
            if (bmd == null)
            {
                throw new Exception();
            }
            unsafe
            {
                int* row = (int*)((byte*)bmd.Scan0 + (y * bmd.Stride));
                return row[x];
            }
        }
        public void SetPixel(int x, int y, int color)
        {
            if (bmd == null)
            {
                throw new Exception();
            }
            unsafe
            {
                int* row = (int*)((byte*)bmd.Scan0 + (y * bmd.Stride));
                row[x] = color;
            }
        }
        public void Unlock()
        {
            if (bmd == null)
            {
                throw new Exception("Not locked.");
            }
            bmp.UnlockBits(bmd);
            bmd = null;
        }
    }
}
