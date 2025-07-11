using System.ComponentModel;

namespace BoletoNet.Enums
{
    public enum Bancos
    {
        [Description("001")]
        BancoBrasil = 1,
        [Description("041")]
        Banrisul = 041,
        Bradesco = 237,
        Caixa = 104,
        CECRED = 85,
        HSBC = 399,
        Itau = 341,
        [Description("033")]
        Santander = 33,
        Sicredi = 748,
        Semear = 743,

        [Description("336")]
        C6Bank = 336,

        [Description("389")]
        BancoMercantil = 389,
    }
}
