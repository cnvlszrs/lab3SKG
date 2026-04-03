using System;
using System.Windows.Forms;

namespace WindowsFormsApp1  // ← должно быть ТОЧНО ТАК ЖЕ, как в Form1.cs
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}