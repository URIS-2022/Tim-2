using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;

namespace OrchardCore.ContentManagement
{
    public class ContentItemMetadata
    {
        [Obsolete("This property will be removed in a future version. Use ContentItem.DisplayText instead.")]
        public string DisplayText { get; set; }

        public RouteValueDictionary DisplayRouteValues { get; set; }
        public RouteValueDictionary EditorRouteValues { get; set; }
        public RouteValueDictionary CreateRouteValues { get; set; }
        public RouteValueDictionary RemoveRouteValues { get; set; }
        public RouteValueDictionary AdminRouteValues { get; set; }

        private IList<GroupInfo> DisplayGroupInfo = new List<GroupInfo>();
        private IList<GroupInfo> EditorGroupInfo = new List<GroupInfo>();
    }
}
