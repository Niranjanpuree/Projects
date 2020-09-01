namespace AutoCare.Product.Vcdb.Model
{
    public enum ChangeType : short
    {
        None = 0,
        Delete = 1,
        Add = 2,
        Modify = 3,
        Replace = 4
    }

    public enum ChangeRequestStatus : short
    {
        Submitted = 0,
        Deleted = 1,
        PreliminaryApproved = 2,
        Approved = 3,
        Rejected = 4
    }
    public enum LikeStatusType:short
    {
        Like =1,
        Unlike=0
    }

    public enum FileStatus : short
    {
        Pending = 0,
        Uploaded = 1,
        Failed = 2,
        Deleted = 3
    }
}
