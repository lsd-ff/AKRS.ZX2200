namespace AKRS.ZX2200.Infrastructure.Models.BaseModels
{
    using System;

    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Models;

    /// <summary>
    /// 程式
    /// </summary>
    [Serializable]
    public abstract class BaseProgramEntity
    {
        /// <summary>
        /// 是否启用程式
        /// </summary>
        public bool IsUse { get; set; } = true;

        /// <summary>
        /// 程式名称
        /// </summary>
        public string Name  {get;set;}

        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 获取程式内流程
        /// </summary>
        /// <returns></returns>
        public virtual ActionNode[] GetActionNodes()
        {
            return null;
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        public abstract void InitConfig();

        /// <summary>
        /// 删除程式相关PR
        /// </summary>
        //public void DeletePR()
        //{
        //    //此处判断除此程式外其它程式中的PR模板是否和此模板共用一个文件夹，如果公用则不删除模板文件夹
        //    //但是当前设计不允许共用，所以此处是可以直接删除文件夹的
   
        //    if (this is BondProgramEntity bondProgramEntity)
        //    {

        //        if (!ProgramRepository.GetInstance().Programs.Any(item => { return item.GetPREntity().Any(pr => pr.GetName() == bondProgramEntity.PickActionNode.PREntity?.GetName()); }))
        //        {
        //           if(Directory.Exists(bondProgramEntity.PickActionNode.PREntity?.FrmSetPRMode?.interfaceTool.ModelPath))
        //                Directory.Delete(bondProgramEntity.PickActionNode.PREntity?.FrmSetPRMode.interfaceTool.ModelPath, true);
                  
        //        }

        //        if (!ProgramRepository.GetInstance().Programs.Any(item => { return item.GetPREntity().Any(pr => pr.GetName() == bondProgramEntity.UpLookCorrectionActionNode.P1PREntity?.GetName()); }))
        //        {
        //            if (Directory.Exists(bondProgramEntity.UpLookCorrectionActionNode.P1PREntity?.FrmSetPRMode?.interfaceTool.ModelPath))
        //                Directory.Delete(bondProgramEntity.UpLookCorrectionActionNode.P1PREntity?.FrmSetPRMode.interfaceTool.ModelPath, true);
                    
                    
        //        }

        //        if (!ProgramRepository.GetInstance().Programs.Any(item => { return item.GetPREntity().Any(pr => pr.GetName() == bondProgramEntity.UpLookCorrectionActionNode.P2PREntity?.GetName()); }))
        //        {
        //            if (Directory.Exists(bondProgramEntity.UpLookCorrectionActionNode.P2PREntity?.FrmSetPRMode?.interfaceTool.ModelPath))
        //                Directory.Delete(bondProgramEntity.UpLookCorrectionActionNode.P2PREntity?.FrmSetPRMode.interfaceTool.ModelPath, true);
        //        }

        //        if (!ProgramRepository.GetInstance().Programs.Any(item => { return item.GetPREntity().Any(pr => pr.GetName() == bondProgramEntity.BondCorrectionActionNode.P1PREntity?.GetName()); }))
        //        {
        //            if (Directory.Exists(bondProgramEntity.BondCorrectionActionNode.P1PREntity?.FrmSetPRMode?.interfaceTool.ModelPath))
        //                Directory.Delete(bondProgramEntity.BondCorrectionActionNode.P1PREntity?.FrmSetPRMode.interfaceTool.ModelPath, true);
        //        }

        //        if (!ProgramRepository.GetInstance().Programs.Any(item => { return item.GetPREntity().Any(pr => pr.GetName() == bondProgramEntity.BondCorrectionActionNode.P2PREntity?.GetName()); }))
        //        {
        //            if (Directory.Exists(bondProgramEntity.BondCorrectionActionNode.P2PREntity?.FrmSetPRMode?.interfaceTool.ModelPath))
        //                Directory.Delete(bondProgramEntity.BondCorrectionActionNode.P2PREntity?.FrmSetPRMode.interfaceTool.ModelPath, true);
        //        }       
        //    }
        //    else if (this is EpoxyProgramEntity epoxyProgramEntity)
        //    {
        //        if (!ProgramRepository.GetInstance().Programs.Any(item => { return item.GetPREntity().Any(pr => pr.GetName() == epoxyProgramEntity.EpoxyActionNode.P1PREntity?.GetName()); }))
        //        {
        //            if (Directory.Exists(epoxyProgramEntity.EpoxyActionNode.P1PREntity?.FrmSetPRMode?.interfaceTool.ModelPath))
        //                Directory.Delete(epoxyProgramEntity.EpoxyActionNode.P1PREntity?.FrmSetPRMode.interfaceTool.ModelPath, true);
        //        }

        //        if (!ProgramRepository.GetInstance().Programs.Any(item => { return item.GetPREntity().Any(pr => pr.GetName() == epoxyProgramEntity.EpoxyActionNode.P2PREntity?.GetName()); }))
        //        {
        //            if (Directory.Exists(epoxyProgramEntity.EpoxyActionNode.P2PREntity?.FrmSetPRMode?.interfaceTool.ModelPath))
        //                Directory.Delete(epoxyProgramEntity.EpoxyActionNode.P2PREntity?.FrmSetPRMode.interfaceTool.ModelPath, true);
        //        }
        //    }
        //    return;
        //}

        ///// <summary>
        ///// 获取程式相关联的PREntity集合
        ///// </summary>
        ///// <returns></returns>
        //public List<PREntity> GetPREntity()
        //{
        //    List<PREntity> res = new List<PREntity>();
        //    if(this is BondProgramEntity bondProgramEntity)
        //    {
        //        res.Add(bondProgramEntity.PickActionNode.PREntity);
        //        res.Add(bondProgramEntity.UpLookCorrectionActionNode.P1PREntity);
        //        res.Add(bondProgramEntity.UpLookCorrectionActionNode.P2PREntity);
        //        res.Add(bondProgramEntity.BondCorrectionActionNode.P1PREntity);
        //        res.Add(bondProgramEntity.BondCorrectionActionNode.P2PREntity);              
        //    }
        //    else if(this is EpoxyProgramEntity epoxyProgramEntity)
        //    {
        //        res.Add(epoxyProgramEntity.EpoxyActionNode.P1PREntity);
        //        res.Add(epoxyProgramEntity.EpoxyActionNode.P2PREntity);
        //    }

        //    return res;

        //}

    }
}
