using System.Linq;
using System.Threading.Tasks;
using Fluid;
using Fluid.Values;
using OrchardCore.Liquid;

namespace OrchardCore.ContentLocalization.Liquid
{
    public class ContentLocalizationFilter : ILiquidFilter
    {
        private readonly IContentLocalizationManager _contentLocalizationManager;

        public ContentLocalizationFilter(IContentLocalizationManager contentLocalizationManager)
        {
            _contentLocalizationManager = contentLocalizationManager;
        }

        public async ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext context)
        {
            var locale = arguments.At(0).ToStringValue();

            if (arguments.At(0).IsNil())
            {
                locale = context.CultureInfo.Name;
            }

            if (input.Type == FluidValues.Array)
            {
                // List of content item ids

                var localizationSets = input.Enumerate(context).Select(x => x.ToStringValue()).ToArray();

                return FluidValue.Create(await _contentLocalizationManager.GetItemsForSetsAsync(localizationSets, locale), context.Options);
            }
            else
            {
                var localizationSet = input.ToStringValue();

                return FluidValue.Create(await _contentLocalizationManager.GetContentItemAsync(localizationSet, locale), context.Options);
            }
        }
    }
}
