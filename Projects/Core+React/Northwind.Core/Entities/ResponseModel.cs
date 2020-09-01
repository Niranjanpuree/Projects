using System;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Core.Entities
{
    public enum ResponseStatus
    {
        success,
        info,
        warn,
        error
    }
}