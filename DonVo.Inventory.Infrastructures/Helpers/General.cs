namespace DonVo.Inventory.Infrastructures.Helpers
{
    public static class General
    {
        public const string ASCENDING = "asc";
        public const string DESCENDING = "desc";
        public const string JsonMediaType = "application/json";

        //public static string BuildSearch(List<string> SearchAttributes)
        //{
        //    string SearchQuery = String.Empty;
        //    foreach (string Attribute in SearchAttributes)
        //    {
        //        if(Attribute.Contains("."))
        //        {
        //            var Key = Attribute.Split(".");
        //            SearchQuery = string.Concat(SearchQuery, Key[0], $".Any({Key[1]}.Contains(@0)) OR ");
        //        }
        //        else
        //        {
        //            SearchQuery = string.Concat(SearchQuery, Attribute, ".Contains(@0) OR ");
        //        }
        //    }

        //    SearchQuery = SearchQuery.Remove(SearchQuery.Length - 4);

        //    return SearchQuery;
        //}
    }
}
