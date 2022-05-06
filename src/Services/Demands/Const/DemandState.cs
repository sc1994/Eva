namespace Eva.Demands.Const;

public enum DemandState
{
    /// <summary>
    ///     待审核
    /// </summary>
    Pending = 0,

    /// <summary>
    ///     审核通过
    /// </summary>
    Approved = 1,

    /// <summary>
    ///     审核不通过
    /// </summary>
    Rejected = 2,

    /// <summary>
    ///     已撤销
    /// </summary>
    Canceled = 3,

    /// <summary>
    ///     已完成
    /// </summary>
    Completed = 4,

    /// <summary>
    ///     已关闭
    /// </summary>
    Closed = 5,

    /// <summary>
    ///     已暂停
    /// </summary>
    Suspended = 7,

    /// <summary>
    ///     已挂起
    /// </summary>
    SuspendedByUser = 9,

    /// <summary>
    ///     进行中
    /// </summary>
    Underway = 10
}