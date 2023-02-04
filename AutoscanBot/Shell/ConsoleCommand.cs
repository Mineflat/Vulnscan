using AutoscanBot.Telegramm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoscanBot.Shell
{
    internal class ConsoleCommand
    {
        public static string? Name;
        public static string? Description;
        public delegate CommandExecutionResult LinkMethod();
        public static LinkMethod? LinkedTo;

    }
}


