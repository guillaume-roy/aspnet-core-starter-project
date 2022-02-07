namespace ServerDomain.Kernel.BusinessRules;

public class BusinessRuleValidationException : Exception
{
    public IBusinessRule BrokenRule { get; }

    public override string Message => this.ToString();

    public string ErrorCode => BrokenRule.ErrorCode;

    public BusinessRuleValidationException(IBusinessRule brokenRule) : base(brokenRule.ErrorCode)
    {
        BrokenRule = brokenRule;
    }

    public override string ToString()
    {
        return $"{BrokenRule.GetType().FullName}: [{BrokenRule.ErrorCode}]";
    }
}
