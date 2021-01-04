using System;

namespace taleOfDungir.Helpers
{
    public static class DebugHelper
    {
        public static void WriteError(string text)
        {
            Write(text, ConsoleColor.Red);
        }

        public static void WriteSuccess(string text)
        {
            Write(text, ConsoleColor.Green);
        }

        private static void Write(string text, ConsoleColor color)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = oldColor;
        }
    }
}