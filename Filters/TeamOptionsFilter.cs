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
    public class TeamOptionsFilter : ILiquidFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TeamOptionsFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, TemplateContext ctx)
        {
            var teams = new List<string>();

            foreach (var value in input.Enumerate())
            {
                teams.Add((await value.GetValueAsync("LeverPostingPart.Team", ctx)).ToStringValue());
            }

            return new StringValue(StringUtils.GetOptions(teams.Distinct().OrderBy(x => x).ToList(), _httpContextAccessor.HttpContext.Request.Query["team"].FirstOrDefault()));
        }
    }
}
