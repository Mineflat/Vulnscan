using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoscanBot.Telegramm
{
    internal class CommandExecutionResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public CommandExecutionResult(bool success, string? message)
        {
            Success = success;
            Message = message;
        }
    }
}
