using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Modules;

namespace OrchardCore.ContentTypes.Editors
{
    public class ContentDefinitionDisplayCoordinator : IContentDefinitionDisplayHandler
    {
        private readonly IEnumerable<IContentTypeDefinitionDisplayDriver> _typeDisplayDrivers;
        private readonly IEnumerable<IContentTypePartDefinitionDisplayDriver> _typePartDisplayDrivers;
        private readonly IEnumerable<IContentPartDefinitionDisplayDriver> _partDisplayDrivers;
        private readonly IEnumerable<IContentPartFieldDefinitionDisplayDriver> _partFieldDisplayDrivers;
        private readonly ILogger _logger;

        public ContentDefinitionDisplayCoordinator(
            IEnumerable<IContentTypeDefinitionDisplayDriver> typeDisplayDrivers,
            IEnumerable<IContentTypePartDefinitionDisplayDriver> typePartDisplayDrivers,
            IEnumerable<IContentPartDefinitionDisplayDriver> partDisplayDrivers,
            IEnumerable<IContentPartFieldDefinitionDisplayDriver> partFieldDisplayDrivers,
            ILogger<IContentDefinitionDisplayHandler> logger)
        {
            _partFieldDisplayDrivers = partFieldDisplayDrivers;
            _partDisplayDrivers = partDisplayDrivers;
            _typePartDisplayDrivers = typePartDisplayDrivers;
            _typeDisplayDrivers = typeDisplayDrivers;
            _logger = logger;
        }

        public Task BuildTypeEditorAsync(ContentTypeDefinition definition, BuildEditorContext context)
        {
            return _typeDisplayDrivers.InvokeAsync(async (contentDisplay, model, context) =>
            {
                var result = await contentDisplay.BuildEditorAsync(model, context);
                if (result != null)
                    await result.ApplyAsync(context);
            }, definition, context, _logger);
        }

        public Task UpdateTypeEditorAsync(ContentTypeDefinition definition, UpdateTypeEditorContext context)
        {
            return _typeDisplayDrivers.InvokeAsync(async (contentDisplay, model, context) =>
            {
                var result = await contentDisplay.UpdateEditorAsync(model, context);
                if (result != null)
                    await result.ApplyAsync(context);
            }, definition, context, _logger);
        }

        public Task BuildTypePartEditorAsync(ContentTypePartDefinition definition, BuildEditorContext context)
        {
            return _typePartDisplayDrivers.InvokeAsync(async (contentDisplay, model, context) =>
            {
                var result = await contentDisplay.BuildEditorAsync(model, context);
                if (result != null)
                    await result.ApplyAsync(context);
            }, definition, context, _logger);
        }

        public Task UpdateTypePartEditorAsync(ContentTypePartDefinition definition, UpdateTypePartEditorContext context)
        {
            return _typePartDisplayDrivers.InvokeAsync(async (contentDisplay, model, context) =>
            {
                var result = await contentDisplay.UpdateEditorAsync(model, context);
                if (result != null)
                    await result.ApplyAsync(context);
            }, definition, context, _logger);
        }

        public Task BuildPartEditorAsync(ContentPartDefinition definition, BuildEditorContext context)
        {
            return _partDisplayDrivers.InvokeAsync(async (contentDisplay, model, context) =>
            {
                var result = await contentDisplay.BuildEditorAsync(model, context);
                if (result != null)
                    await result.ApplyAsync(context);
            }, definition, context, _logger);
        }

        public Task UpdatePartEditorAsync(ContentPartDefinition definition, UpdatePartEditorContext context)
        {
            return _partDisplayDrivers.InvokeAsync(async (contentDisplay, model, context) =>
            {
                var result = await contentDisplay.UpdateEditorAsync(model, context);
                if (result != null)
                    await result.ApplyAsync(context);
            }, definition, context, _logger);
        }

        public async Task BuildPartFieldEditorAsync(ContentPartFieldDefinition definition, BuildEditorContext context)
        {
            await _partFieldDisplayDrivers.InvokeAsync(async (contentDisplay, model, context) =>
            {
                var result = await contentDisplay.BuildEditorAsync(model, context);
                if (result != null)
                    await result.ApplyAsync(context);
            }, definition, context, _logger);
        }

        public async Task UpdatePartFieldEditorAsync(ContentPartFieldDefinition definition, UpdatePartFieldEditorContext context)
        {
            await _partFieldDisplayDrivers.InvokeAsync(async (contentDisplay, model, context) =>
            {
                var result = await contentDisplay.UpdateEditorAsync(model, context);
                if (result != null)
                    await result.ApplyAsync(context);
            }, definition, context, _logger);
        }
    }
}
