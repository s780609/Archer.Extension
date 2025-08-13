using System.ComponentModel;

namespace Archer.Extension.Models
{
    /// <summary>
    /// 實體類型：自然人或法人
    /// </summary>
    public enum EntityType
    {
        /// <summary>
        /// 自然人（個人）
        /// </summary>
        [Description("自然人")]
        NaturalPerson = 1,

        /// <summary>
        /// 法人（公司）
        /// </summary>
        [Description("法人")]
        LegalEntity = 2,

        /// <summary>
        /// 未知或無效
        /// </summary>
        [Description("未知")]
        Unknown = 0
    }
}