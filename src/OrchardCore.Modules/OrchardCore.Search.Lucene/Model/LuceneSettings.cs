using Lucene.Net.Util;
using OrchardCore.Contents.Indexing;

namespace OrchardCore.Search.Lucene.Model
{
    public class LuceneSettings
    {
        public static readonly string[] FullTextField = new string[] { IndexingConstants.FullTextKey };

        public const string StandardAnalyzer = "standardanalyzer";

        public static LuceneVersion DefaultVersion = LuceneVersion.LUCENE_48;

        public string SearchIndex { get; set; }

        public string[] DefaultSearchFields { get; set; } = FullTextField;

        public bool AllowLuceneQueriesInSearch { get; set; } = false;
    }
}
