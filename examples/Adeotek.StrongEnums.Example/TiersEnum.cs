// namespace Adeotek.StrongEnums.Example;
//
// public sealed class TiersEnum : StrongEnumBase<TiersEnum, int, decimal>
// {
//     public static readonly TiersEnum BASIC = new("Basic", 1, 0m);
//     public static readonly TiersEnum PREMIUM = new("Premium", 2, 0.1m);
//     public static readonly TiersEnum VIP = new("VIP", 3, 0.3m);
//
//     private TiersEnum(string name, int value, decimal discount)
//         : base(name, value, discount)
//     {
//     }
// }