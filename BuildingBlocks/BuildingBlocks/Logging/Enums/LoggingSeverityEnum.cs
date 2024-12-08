using System.ComponentModel;

namespace BuildingBlocks.Logging.Enums;

public enum LoggingSeverityEnum
{
    [Description("error")]
    Error = 1,
    
    [Description("warning")]
    Warning = 2,

    [Description("information")]
    Information = 3,

    [Description("debug")]
    Debug = 4,

}