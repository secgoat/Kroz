using System;

namespace Kroz
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Kroz game = new Kroz())
            {
                game.Run();
            }
        }
    }
#endif
}

