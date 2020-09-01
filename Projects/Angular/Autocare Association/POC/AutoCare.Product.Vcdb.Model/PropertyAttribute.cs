using System;

namespace AutoCare.Product.Vcdb.Model
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IdentityProperty : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ChangeRequestProperty : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DeletedOnProperty : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class InsertedOnProperty : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class UpdatedOnProperty : Attribute
    {
    }
}
