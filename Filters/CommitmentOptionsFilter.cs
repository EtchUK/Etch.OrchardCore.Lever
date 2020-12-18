using Etch.OrchardCore.Lever.Utilities;
using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Http;
using OrchardCore.Liquid;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, TemplateContext ctx)
        {
            var commitments = new List<string>();
            foreach (var value in input.Enumerate())
            {
                var commitment = await value.GetValueAsync("LeverPostingPart.Commitment", ctx);

                if (!commitments.Any(x => x.Equals(commitment.ToStringValue(), System.StringComparison.InvariantCultureIgnoreCase)))
                {
                    commitments.Add(commitment.ToStringValue());
                }
            }

            
            return new StringValue(StringUtils.GetOptions(commitments, _httpContextAccessor.HttpContext.Request.Query["commitment"].FirstOrDefault()));
        }


    }
}
