using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Extensions.Features;
using OrchardCore.Environment.Shell;
using OrchardCore.Features.Models;

namespace OrchardCore.Features.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IExtensionManager _extensionManager;
        private readonly IShellFeaturesManager _shellFeaturesManager;
        private readonly INotifier _notifier;
        private readonly IHtmlLocalizer H;

        public ModuleService(
                IExtensionManager extensionManager,
                IShellFeaturesManager shellFeaturesManager,
                IHtmlLocalizer<ModuleService> htmlLocalizer,
                INotifier notifier)
        {
            _notifier = notifier;
            _extensionManager = extensionManager;
            _shellFeaturesManager = shellFeaturesManager;
            H = htmlLocalizer;
        }

       
        public async Task<IEnumerable<ModuleFeature>> GetAvailableFeaturesAsync()
        {
            var enabledFeatures =
                await _shellFeaturesManager.GetEnabledFeaturesAsync();

            var availableFeatures = _extensionManager.GetFeatures();

            return availableFeatures
                .Select(f => AssembleModuleFromDescriptor(f, enabledFeatures
                    .Any(sf => sf.Id == f.Id)));
        }

      
        public Task EnableFeaturesAsync(IEnumerable<string> featureIds)
        {
            return EnableFeaturesAsync(featureIds, false);
        }

     
  
        public async Task EnableFeaturesAsync(IEnumerable<string> featureIds, bool force)
        {
            var featuresToEnable = _extensionManager
                .GetFeatures()
                .Where(x => featureIds.Contains(x.Id));

            var enabledFeatures = await _shellFeaturesManager.EnableFeaturesAsync(featuresToEnable, force);
            foreach (var enabledFeature in enabledFeatures)
            {
                await _notifier.SuccessAsync(H["{0} was enabled.", enabledFeature.Name]);
            }
        }

        public Task DisableFeaturesAsync(IEnumerable<string> featureIds)
        {
            return DisableFeaturesAsync(featureIds, false);
        }


        public async Task DisableFeaturesAsync(IEnumerable<string> featureIds, bool force)
        {
            var featuresToDisable = _extensionManager
                .GetFeatures()
                .Where(x => featureIds.Contains(x.Id));

            var features = await _shellFeaturesManager.DisableFeaturesAsync(featuresToDisable, force);
            foreach (var feature in features)
            {
                await _notifier.SuccessAsync(H["{0} was disabled.", feature.Name]);
            }
        }

        private static ModuleFeature AssembleModuleFromDescriptor(IFeatureInfo featureInfo, bool isEnabled)
        {
            return new ModuleFeature
            {
                Descriptor = featureInfo,
                IsEnabled = isEnabled
            };
        }

    }
}
