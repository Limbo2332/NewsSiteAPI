using NewsSite.UI.Endpoints;
using NewsSite.UI.Filters;

namespace NewsSite.UI.Extensions;

public static class WebApplicationExtensions
{
    public static void RegisterRouteGroups(this WebApplication app)
    {
        var groups = new List<RouteGroupBuilder>
        {
            app.MapAuthEndpoints(),
            app.MapTagsEndpoints(),
            app.MapRubricsEndpoints(),
            app.MapNewsEndpoints(),
            app.MapAuthorsEndpoints()
        };

        groups.ForEach(g => g.AddEndpointFilter<ErrorExceptionFilter>());
    }
}