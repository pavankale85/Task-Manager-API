using FluentValidation;

namespace TaskMgnrAPI.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> IsInEnumOrString<T>(this IRuleBuilder<T, string> ruleBuilder, params string[] allowed)
        {
            return ruleBuilder.Must(x => allowed.Contains(x)).WithMessage($"Status must be one of: {string.Join(", ", allowed)}");
        }
    }
}
