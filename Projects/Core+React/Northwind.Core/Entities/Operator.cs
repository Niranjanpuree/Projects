using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{

    //This names must match names in database at all times.
    public enum OperatorName
    {
        StringEquals = 1,
        StringGreaterThan = 2,
        StringLessThan = 3,
        StringLike = 4,
        StringIn = 5,
        StringNotEquals = 6,
        DateTimeEquals = 8,
        DateTimeLessThan = 9,
        DateTimeLessThanOrEquals = 10,
        DateTimeGreaterThan = 11,
        DateTimeGreaterThanOrEquals = 12,
        DateTimeBetween = 13,
        DateTimeIn = 14,
        DateTimeNotEquals = 15,
        IntegerEquals = 16,
        IntegerLessThan = 17,
        IntegerLessThanorEquals = 18,
        IntegerGreaterThan = 19,
        IntegerGreaterThanOrEquals = 20,
        IntegerBetween = 21,
        IntegerIn = 22,
        IntegerNotEquals = 23,
        DecimalEquals = 24,
        DecimalLessThan = 25,
        DecimalLessThanOrEquals = 26,
        DecimalGreaterThan = 27,
        DecimalGreaterThanOrEquals = 28,
        DecimalBetween = 29,
        DecimalIn = 30,
        DecimalNotEquals = 31,
        ComboEquals = 32,
        ComboIn = 33,
        MultiCheckboxEquals = 34,
        MultiCheckboxIn = 35,
        RadioEquals = 36,
        StringLikeStartWith = 37,
        IsNull =38,
        IsNotNull =39,
        IsEmpty=40,
    }
}
