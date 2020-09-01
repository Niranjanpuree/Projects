namespace AutoCare.Product.Infrastructure.ErrorMessage
{
    public class ErrorMessage
    {
        public class Make {
            public const string RequiredMakeName = "Make Name is required";
            public const string InvalidMakeId = "Invalid MakeId";
            public const string RequiredMakeInputModel = "MakeInputModel is required";
        }
        public class Model
        {
            public const string RequiredModelName = "Model Name is required";
            public const string InvalidModelId = "Invalid ModelId";
            public const string RequiredModelInputModel = "MadeInputModel is required";
        }

        public class Year {
            public const string RequiredYearInputModel = "YearInputModel is required";
            public const string InvalidYearLength = "Required 4 digit year";
            public const string RequiredYear = "Required Year";
        }

        public class Region
        {
            public const string RequiredRegionInputModel = "RegionInputModel is required";
            public const string InvalidRegionLength = "Required 4 digit Region";
            public const string RequiredRegion = "Required Region";
            public const string RegionAbbreviation = "Required Region Abbreviation";
        }

        public class TypeGroup
        {
            public const string RequiredTypeGroupInputModel = "TypeGroupInputModel is required";
            public const string InvalidTypeGroupLength = "Required 4 digit TypeGroup";
            public const string RequiredTypeGroup = "Required TypeGroup";
        }

        public class Type
        {
            public const string RequiredTypeInputModel = "TypeInputModel is required";
            public const string InvalidTypeLength = "Required 4 digit Type";
            public const string RequiredType = "Required Type";
        }




    }
}
