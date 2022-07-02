using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public static class Drawing
    {
        private static ConsoleColors ConsoleDefaultColor { get; set; } = new ConsoleColors();

        private static Stack<Position> Buffer { get; set; } = new Stack<Position>();

        public static void InitializeConsole(ConsoleColors? colors = null)
        {
            Drawing.ConsoleDefaultColor = (colors == null) ? new ConsoleColors(Console.BackgroundColor, Console.ForegroundColor) : colors;

            Console.CursorVisible = false;
            Drawing.ResetColorPreset();
            Console.Clear();
        }

        public static void Write(Position pos)
        {
            Drawing.Write(pos, " ", null, null);
        }

        public static void Write(Position pos, string text)
        {
            Drawing.Write(pos, text, null, null);
        }

        public static void Write(Position pos, string text, ConsoleColors colors)
        {
            if (colors == null)
                Drawing.Write(pos, text, null, null);
            else
                Drawing.Write(pos, text, colors.ForegroundColor, colors.BackgroundColor);
        }

        public static void Write(Position pos, string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            Drawing.Buffer.Push(pos);
            Drawing.DrawElement(pos, text, foregroundColor, backgroundColor);
        }

        public static void Clear()
        {
            while (Drawing.Buffer.Count != 0)
                Drawing.DrawElement(Drawing.Buffer.Pop()); // Draws " " char in every changed place with default colors
        }

        // reset whole console into desirable color 
        public static void ResetConsole(ConsoleColors colors = null)
        {
            if (colors != null)
                Drawing.ConsoleDefaultColor = colors;
            Drawing.ResetColorPreset();

            Drawing.Clear();
        }

        // Draws element with custom background 
        private static void DrawElement(Position pos, string text = " ", ConsoleColor? forCol = null, ConsoleColor? bacCol = null)
        {
            Console.SetCursorPosition(pos.X, pos.Y);

            if (forCol == null && bacCol == null)
                Drawing.ResetColorPreset();
            else if (forCol == null)
            {
                Console.ForegroundColor = Drawing.ConsoleDefaultColor.ForegroundColor;
                Console.BackgroundColor = (ConsoleColor)bacCol;
            }
            else if (bacCol == null)
            {
                Console.ForegroundColor = (ConsoleColor)forCol;
                Console.BackgroundColor = Drawing.ConsoleDefaultColor.BackgroundColor;
            }
            else
            {
                Console.BackgroundColor = (ConsoleColor)forCol;
                Console.ForegroundColor = (ConsoleColor)bacCol;
            }
            Console.Write(text);
        }

        private static void ResetColorPreset()
        {
            Console.BackgroundColor = Drawing.ConsoleDefaultColor.BackgroundColor;
            Console.ForegroundColor = Drawing.ConsoleDefaultColor.ForegroundColor;
        }
    }

    public class ConsoleColors
    {
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public ConsoleColors()
        {
        }

        public ConsoleColors(ConsoleColor BackgroundColor, ConsoleColor ForegroundColor)
        {
            this.BackgroundColor = BackgroundColor;
            this.ForegroundColor = ForegroundColor;
        }
    }

    public class Position
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Position(Position pos)
        {
            (this.X, this.Y) = pos.Get();
        }

        public Position(int x, int y)
        {
            (this.X, this.Y) = (x, y);
        }

        public Position((int x, int y) pos)
        {
            this.Set(pos);
        }

        public (int, int) Get()
        {
            return (this.X, this.Y);
        }

        public void Set((int x, int y) pos)
        {
            (this.X, this.Y) = pos;
        }

        public static bool operator ==(Position x, Position y)
        {
            return x.Get() == y.Get();
        }

        public static bool operator !=(Position x, Position y)
        {
            return x.Get() != y.Get();
        }

        public static Position operator +(Position x, Position y)
        {
            return new Position(x.X + y.X, x.Y + y.Y);
        }

        public static Position operator -(Position x, Position y)
        {
            return new Position(x.X - y.X, x.Y - y.Y);
        }
    }
}