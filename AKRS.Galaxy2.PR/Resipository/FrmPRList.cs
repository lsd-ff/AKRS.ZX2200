using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.Galaxy2.PR.Controls;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using DevExpress.XtraEditors;
using LanguageExt;
using VM.PlatformSDKCS;

namespace AKRS.Galaxy2.PR.Resipository
{
    /// <summary>
    /// 视觉模板列表
    /// </summary>
    public partial class FrmPRList : XtraForm
    {
        /// <summary>
        /// 模板名称
        /// </summary>
        public string VisionEntityName { get; set; }

        /// <summary>
        /// FrmPREditor状态标志
        /// </summary>
        public bool IsFrmOpen { get; set; } = false;

        /// <summary>
        /// 当前RecipePR列表
        /// </summary>
        public List<string> PREntityList;

        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmPRList(List<string> RecipeAllPREntitys)
        {
            InitializeComponent();
            PREntityList = RecipeAllPREntitys;
        }


        public FrmPRList()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshBinding() 
        {
            List<PREntity> PRentitylists = new List<PREntity>();
            if (VisionEntityRepository.GetInstance().PRVisionList != null && VisionEntityRepository.GetInstance().PRVisionList.Count > 0)
            {
                List<BaseVisionEntity> pRentitylists1 = VisionEntityRepository.GetInstance().PRVisionList;
                for (int i = 0; i < VisionEntityRepository.GetInstance().PRVisionList.Count; i++)
                {
                    PREntity prentity = (PREntity)VisionEntityRepository.GetInstance().PRVisionList[i];
                    if (prentity.Alg.AlgBeLong == AlgBeLongEnum.Calibration || PREntityList.Contains(prentity.Alg.Name))
                    {
                        PRentitylists.Add(prentity);
                    }
                }
            }

            BsPRList.DataSource = PRentitylists;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtRemove_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BaseVisionEntity visionEntity = GvPRTemps.GetFocusedRow() as BaseVisionEntity;
            if (visionEntity != null)
            {
                VisionEntityRepository.GetInstance().RemoveVisionEntity(visionEntity);
                PREntity prEntity = (PREntity)visionEntity;
                string path = prEntity.Alg.GetPRSavePath();
                string tempPath = Path.Combine(path, visionEntity.GetName());
                if (Directory.Exists(tempPath))
                {
                    Directory.Delete(tempPath, true);
                }
            }
            RefreshBinding();
            GcPRTemps.Refresh();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            VisionEntityRepository.GetInstance().Save();
            RefreshBinding();
        }

        /// <summary>
        /// 双击编辑
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void GvPRTemps_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            
            if (e.Clicks == 2)
            {
                BaseVisionEntity visionEntity = GvPRTemps.GetFocusedRow() as BaseVisionEntity;

                if (visionEntity != null&&IsFrmOpen==false) 
                {
                    IsFrmOpen = true;

                    // 由界面控件控制相机采图，故参数为false
                    FrmPREditor editorForm = new FrmPREditor((PREntity)visionEntity, false);
                    DialogResult result = editorForm.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        editorForm.Dispose();
                    }
                    IsFrmOpen = false;
                }          
            }
        }

        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtOk_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 添加PR
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtAddPR_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //// 新建一个新的模版 
            string name = $"新PR模板{DateTime.Now.ToString("ssffff")}";
            PREntity preEntity = new PREntity(name);
            BaseVisionEntity visionEntity = preEntity;
            VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            RefreshBinding();
            GvPRTemps.MoveLast();
        }

        /// <summary>
        /// 从选中的模版创建
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtAddPRFromSelectedPR_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        { 
            //// 从其他模版复制
            //string name = $"新PR模板{DateTime.Now.ToString("ssffff")}";
            //PREntity prEntity = (PREntity)(GvPRTemps.GetFocusedRow() as BaseVisionEntity);

            //// 根据名称拷贝文件后另存为name
            //DirAndFileHelper.CopyDirectory(prEntity.Alg.GetPRSavePath() + prEntity.GetName(), prEntity.Alg.GetPRSavePath() + name);
            //if (prEntity == null)
            //{
            //    XtraMessageBox.Show("请先选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}

            ////深拷贝，[serializable]
            ////PREntity Entity = JsonFormatHelper<PREntity>.DeepGenericCopy<PREntity>(prEntity);    
            ////prEntity.Alg.Name= name;
            ////BaseVisionEntity visionEntity = prEntity;

            //PREntity Entity = prEntity.Clone(name);
            //Entity.Alg.Name = name;
            //BaseVisionEntity visionEntity = Entity;

            //visionEntity.SetName(name);
            //VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            
            //RefreshBinding();
            //GvPRTemps.MoveLast();
        }

        private void FrmPRList_Load(object sender, EventArgs e)
        {
            RefreshBinding();
        }

    }
}