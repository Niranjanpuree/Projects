using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearch.Indexing.RepositoryServices
{
    public class ChangeRequestIndexRepositoryServices
    {
        private readonly ISearchServiceClient _searchServiceClient;

        public ChangeRequestIndexRepositoryServices(ISearchServiceClient searchServiceClient = null)
        {
            _searchServiceClient = searchServiceClient ??
                                   new SearchServiceClient("optimussearch",
                                       new SearchCredentials("24C77889585CFB6E756E7783DE693438"));
        }

        public void CreateIndex()
        {
            if (_searchServiceClient.Indexes.Exists("changerequests"))
            {
                _searchServiceClient.Indexes.Delete("changerequests");
            }

            var definition = new Index()
            {
                Name = "changerequests",
                Fields = new[]
                {
                    new Field("changeRequestId", DataType.String)
                    {
                        IsKey = true,
                        IsRetrievable = true,
                        IsFilterable = true,
                        IsFacetable = true,
                        IsSearchable = true,
                        IsSortable = true
                    },

                    new Field("changeType", DataType.String)
                    {
                        IsRetrievable = true,
                        IsFilterable = true,
                        IsFacetable = true,
                        IsSearchable = false
                    },

                    new Field("changeRequestTypeId", DataType.Int32)
                    {
                        IsRetrievable = true,
                        IsFilterable = true,
                        IsFacetable = true,
                        IsSearchable = false
                    },

                    new Field("status", DataType.Int32)
                    {
                        IsRetrievable = true,
                        IsFilterable = true,
                        IsFacetable = true,
                        IsSearchable = false
                    },

                    new Field("statusText", DataType.String)
                    {
                        IsRetrievable = true,
                        IsFilterable = true,
                        IsFacetable = true,
                        IsSearchable = false
                    },

                    new Field("requestedBy", DataType.String)
                    {
                        IsRetrievable = true,
                        IsFilterable = true,
                        IsFacetable = true,
                        IsSearchable = true
                    },

                    new Field("submittedDate", DataType.DateTimeOffset)
                    {
                        IsRetrievable = true,
                        IsFilterable = true,
                        IsFacetable = true,
                        IsSearchable = false,
                        IsSortable = true
                    },

                    new Field("updatedDate", DataType.DateTimeOffset)
                    {
                        IsRetrievable = true,
                        IsFilterable = true,
                        IsFacetable = true,
                        IsSearchable = false,
                        IsSortable = true
                    },

                    new Field("source", DataType.String)
                    {
                        IsRetrievable = true,
                        IsFilterable = true,
                        IsFacetable = true,
                        IsSearchable = true
                    },

                    new Field("likes", DataType.Int32)
                    {
                        IsRetrievable = true,
                        IsFilterable = true,
                        IsFacetable = true,
                        IsSearchable = false
                    },

                    new Field("commentExists", DataType.Boolean)
                    {
                        IsRetrievable = true,
                        IsFilterable = true,
                        IsFacetable = true,
                        IsSearchable = false
                    },

                    new Field("entity", DataType.String)
                    {
                        IsRetrievable = true,
                        IsFilterable = true,
                        IsFacetable = true,
                        IsSearchable = false
                    },

                    new Field("assignee", DataType.String)
                    {
                        IsRetrievable = true,
                        IsFilterable = true,
                        IsFacetable = true,
                        IsSearchable = false
                    },

                    new Field("changeContent", DataType.String)
                    {
                        IsRetrievable = true,
                        IsFilterable = true,
                        IsFacetable = true,
                        IsSearchable = false
                    },

                }
            };

            _searchServiceClient.Indexes.Create(definition);
        }
    }
}

