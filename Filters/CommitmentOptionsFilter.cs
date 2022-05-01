using Etch.OrchardCore.Lever.Utilities;
using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Http;
using OrchardCore.Liquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Lever.Filters
{
    public class CommitmentOptionsFilter : ILiquidFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommitmentOptionsFilter(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext context)
        {
            var commitments = new List<string>();

            foreach (var value in input.Enumerate(context))
            {
                commitments.AddRange((await value.GetValueAsync("LeverPostingPart.Commitment", context))
                    .ToStringValue().Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim()).ToList()
                );
            }

            return new StringValue(StringUtils.GetOptions(commitments.Distinct().OrderBy(x => x).ToList(), _httpContextAccessor.HttpContext.Request.Query["commitment"].FirstOrDefault()));
        }
    }
}
