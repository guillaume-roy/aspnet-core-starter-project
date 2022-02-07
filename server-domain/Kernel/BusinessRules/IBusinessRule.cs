namespace ServerDomain.Kernel.BusinessRules;

public interface IBusinessRule
{
    public string ErrorCode { get; }
    public Task<bool> IsBroken();
}