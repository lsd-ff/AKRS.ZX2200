namespace AKRS.ZX2200.SupportFeature.Parameters.Model
{
    /// <summary>
    /// 初始化对象的封装，包括上传的对象和节点位置信息
    /// </summary>
    public class ObjAndNodesInfo
    {
        /// <summary>
        /// 当前上传的对象
        /// </summary>
        public object CurrentObj { get; set; }

        /// <summary>
        ///  TreeGroupList的子节点
        /// </summary>
        public TreeGroupChildNodesEnum GroupTreeCurrentText { get; set; }

        /// <summary>
        ///  TreeDataSetList节点
        /// </summary>
        public string TreeDataSetListNode { get; set; }


        /// <summary>
        ///  节点信息
        /// </summary>
        /// <param name="currentObj">要设置参数的对象</param>
        /// <param name="groupTreeCurrentNodesTextEnum">当前节点的名称</param>
        /// <param name="treeDataSetListNode">TreeDataSetListNode</param>
        public ObjAndNodesInfo(object currentObj, TreeGroupChildNodesEnum groupTreeCurrentNodesTextEnum, string treeDataSetListNode)
        {
            this.CurrentObj = currentObj;
            this.GroupTreeCurrentText = groupTreeCurrentNodesTextEnum;
            this.TreeDataSetListNode = treeDataSetListNode;
        }
    }
}
