namespace AutoCare.Product.Infrastructure.Logging
{
    public enum LogType : short
    {
        None = 0,
        Trace = 1, //Begin method X, end method X etc
        Debug = 2, //Executed queries, user authenticated, session expired
        Info = 3, // Normal behaviour like mail sent, user updated profile etc
        Warn = 4, // Incorrect behaviour but the application can continue
        Error = 5, // For example, application crashes / exceptions
        Fatal = 6 // Highest level, important stuff down
    }
}
