using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mario
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            PlatformWindows platform = new PlatformWindows();
            Form1 form = new Form1();
            platform.form = form;
            Game game = new Game();
            game.Start(platform);
            platform.Start();
            Application.Run(form);
        }
    }
    class Form1 : Form
    {
        public Form1()
        {
            DoubleBuffered = true;
        }
    }
}
