namespace Adverthouse.Utility.Validation
{
    public interface IRuleOptions
    {
        IRuleBuilder NotNull();
        IRuleBuilder GreaterThen();
    }
}
