namespace GHY.EF.Core.Info
{
    /// <summary>
    /// 内容状态枚举。
    /// </summary>
    public enum InfoState
    {
        // 待审核
        PendingAudit,

        // 审核通过
        Audited,

        // 撤销
        Revoked,

        // 不解释
        DoNotExplain
    };
}
