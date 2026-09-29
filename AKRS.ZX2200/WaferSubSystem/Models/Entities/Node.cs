namespace AKRS.ZX2200.WaferSubSystem.Models.Entities
{
    /// <summary>
    /// 节点类
    /// </summary>
    public class Node
    {
        /// <summary>
        /// 节点编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 父节点编号
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 节点名
        /// </summary>
        public string Name { get; set; }
    }
}
