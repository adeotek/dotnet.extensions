namespace Adeotek.StrongEnums.Example;

public class CreditCard : TestStrongEnum<CreditCard>
{
    public static readonly CreditCard Standard = new(1, "Standard");
    public static readonly CreditCard Premium = new(2, "Premium");
    public static readonly CreditCard Platinum = new(3, "Platinum");
    
    private CreditCard(int value, string name) : base(value, name)
    {
    }
}