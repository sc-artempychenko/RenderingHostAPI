using System.Text.RegularExpressions;

namespace POCRenderingHostAPI
{
    public static class QueryHelper
    {
        public static string BuildQuery(string queryName)
        {
            var query = GetQueryFromResources(queryName);
            return query;
        }

        private static string GetQueryFromResources(string queryName)
        {
            var assembly = typeof(QueryHelper).Assembly;

            var queryFullName = $"{assembly.GetName().Name}.Queries.{queryName}.graphql";
            var stream = assembly.GetManifestResourceStream(queryFullName);
            if (stream == null)
            {
                throw new FileNotFoundException($"Unable to resolve resource {queryFullName}");
            }

            using var reader = new StreamReader(stream);
            var query = reader.ReadToEnd();
            var importQueries = Regex.Matches(query, "#import\\s+\"(.+)\";").OfType<Match>().Select(m => m.Groups[1].Value).ToList();
            var importedQueries = "";
            if (importQueries.Any())
            {
                importedQueries = string.Join("\n", importQueries.Select(GetQueryFromResources));
            }

            return $"{query}\n{importedQueries}";
        }
    }
}