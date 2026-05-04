using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Command
{
    public class CommandInvoker
    {
        private ICommandAction? _command;

        public void SetCommand(ICommandAction command)
        {
            _command = command;
        }

        public void Run()
        {
            if (_command == null)
                throw new InvalidOperationException("Команда не назначена.");

            _command.Execute();
        }
    }
}
