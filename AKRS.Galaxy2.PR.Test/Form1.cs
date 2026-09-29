using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.Galaxy2.PR.Controls;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Resipository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AKRS.Galaxy2.PR.Test
{
    // 1. 创建PR  （2种创建方式，名称不能重复） 删除
    //  2. 保存 读取
    //  3. PR 功能
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static string Root { get; } = Application.StartupPath;
        public static string RecipeRootDirPath => Path.Combine(SettingsPath, "RecipeParams");

        /// <summary>
        /// Recipe 路径
        /// settings/RecipeParams/RecipeID/
        /// </summary>
        public static string RecipeDirPath => Path.Combine(RecipeRootDirPath, MachineConfigContext.GetInstance().RecipeID.ToString());



        /// <summary>
        /// 配置文件存放路径
        /// </summary>
        public static string SettingsPath => Path.Combine(Root, "Settings");

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //FrmPRList frmPRList=new FrmPRList();

            //frmPRList.ShowDialog();
            EditPr("test2");

        }
        public static void EditPr(string name)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);
            BaseVisionEntity visionEntity = null;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity(name);

                visionEntity = prEntity;

                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);

                prEntity.SetAlgFlowType(Models.CommonModels.AlgFlowTypeEnum.XldModelAlg);
            }

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);

            editor.ShowDialog();
        }
    }
}
