using Etch.OrchardCore.Lever.Utilities;
using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Http;
using OrchardCore.Liquid;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Lever.Filters
{
    public class LocationOptionsFilter : ILiquidFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocationOptionsFilter(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, TemplateContext ctx)
        {
            var locations = new List<string>();

            foreach (var value in input.Enumerate())
            {
                locations.Add((await value.GetValueAsync("LeverPostingPart.Location", ctx)).ToStringValue());
            }

            return new StringValue(StringUtils.GetOptions(locations.Distinct().OrderBy(x => x).ToList(), _httpContextAccessor.HttpContext.Request.Query["location"].FirstOrDefault()));
        }
    }
}
