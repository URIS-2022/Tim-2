using System.Threading.Tasks;
using GraphQL.Language.AST;
using GraphQL.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OrchardCore.Apis.GraphQL.ValidationRules
{
    public class MaxNumberOfResultsValidationRule : IValidationRule
    {
        private readonly int _maxNumberOfResults;
        private readonly MaxNumberOfResultsValidationMode _maxNumberOfResultsValidationMode;
        private readonly IStringLocalizer<MaxNumberOfResultsValidationRule> _localizer;
        private readonly ILogger<MaxNumberOfResultsValidationRule> _logger;

        public MaxNumberOfResultsValidationRule(IOptions<GraphQLSettings> options, IStringLocalizer<MaxNumberOfResultsValidationRule> localizer, ILogger<MaxNumberOfResultsValidationRule> logger)
        {
            var settings = options.Value;
            _maxNumberOfResults = settings.MaxNumberOfResults;
            _maxNumberOfResultsValidationMode = settings.MaxNumberOfResultsValidationMode;
            _localizer = localizer;
            _logger = logger;
        }

        public Task<INodeVisitor> ValidateAsync(ValidationContext context)
        {
            return Task.FromResult((INodeVisitor)new NodeVisitors(
            new MatchingNodeVisitor<Argument>((arg, visitorContext) =>
            {
                if ((arg.Name == "first" || arg.Name == "last") && arg.Value != null)
                {
                    int? value = null;

                    if (arg.Value is IntValue)
                    {
                        value = ((IntValue)arg.Value)?.Value;
                    }
                    else
                    {
                        if (context.Inputs.TryGetValue(arg.Value.ToString(), out var input))
                        {
                            value = (int?)input;
                        }
                    }

                    if (value.HasValue && value > _maxNumberOfResults)
                    {
                        var errorMessage = _localizer["'{0}' exceeds the maximum number of results for '{1}' ({2})", value.Value, arg.Name, _maxNumberOfResults];

                        if (_maxNumberOfResultsValidationMode == MaxNumberOfResultsValidationMode.Enabled)
                        {
                            context.ReportError(new ValidationError(
                            context.Document.OriginalQuery,
                                "ArgumentInputError",
                                errorMessage,
                                arg));
                        }
                        else
                        {
                            _logger.LogInformation(errorMessage);
                        }
                    }
                }
            })));
        }
    }
}
