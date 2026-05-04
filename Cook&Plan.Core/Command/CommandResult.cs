using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Plan.Core.Command
{
    public class CommandResult<T>
    {
        public T? Value { get; private set; }

        public void SetValue(T value)
        {
            Value = value;
        }
    }
}
