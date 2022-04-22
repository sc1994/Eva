namespace Eva.ToolKit
{
    /// <summary>
    ///     映射到. 可在特性中直接指定映射类型.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MapToAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MapToAttribute" /> class.
        /// </summary>
        /// <param name="mapToType">映射到的类型.</param>
        /// <param name="isCanReverse">是否可以反转.</param>
        public MapToAttribute(Type mapToType, bool isCanReverse = true)
        {
            MapToType = mapToType;
            IsCanReverse = isCanReverse;
        }

        /// <summary>
        ///     Gets 映射到的类型.
        /// </summary>
        public Type MapToType { get; }

        /// <summary>
        ///     Gets a value indicating whether 是否可以反转.
        /// </summary>
        public bool IsCanReverse { get; }
    }
}