using System;

namespace Tutorial.Game
{
    [Flags]
    public enum RuleEnum
    {
        None = 0
        ,lt = 2
        ,rt = 4
        ,rb = 8
        ,lb = 16
    }
}