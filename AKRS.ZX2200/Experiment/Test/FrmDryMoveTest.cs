namespace AKRS.ZX2200.Experiment.Test
{
    using System;
    using System.Threading.Tasks;

    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;

    public partial class FrmDryMoveTest : DevExpress.XtraEditors.XtraForm
    {
        public FrmDryMoveTest()
        {
            this.InitializeComponent();
        }


        bool testFlag = true;
        private void BtBondMoveTest_Click(object sender, EventArgs e)
        {
            this.testFlag = true;
            // BondX轴
            Axis bondAxisX = HardwareRepositoryService.GetHardware<Axis>("BondX");

            // BondY轴
            Axis bondAxisY = HardwareRepositoryService.GetHardware<Axis>("BondY");

            // Z轴
            Axis bondAxisZ = HardwareRepositoryService.GetHardware<Axis>("BondZ");

            Task.Run(
                () => {

                    while (this.testFlag)
                    {
                        bondAxisX.AbsoluteMove(-170);
                        bondAxisX.AbsoluteMove(180);
                    }

                });

            Task.Run(
               () => {

                   while (this.testFlag)
                   {
                       bondAxisY.AbsoluteMove(-154);
                       bondAxisY.AbsoluteMove(108);
                   }

               });

            Task.Run(
               () => {

                   while (this.testFlag)
                   {
                       bondAxisZ.AbsoluteMove(-35);
                       bondAxisZ.AbsoluteMove(0);
                   }
               });
        }

        private void BtDispenseMoveTest_Click(object sender, EventArgs e)
        {
            this.testFlag = true;
            // BondX轴
            Axis dispenseX = HardwareRepositoryService.GetHardware<Axis>("点胶X");

            // BondY轴
            Axis dispenseY = HardwareRepositoryService.GetHardware<Axis>("点胶Y");

            // Z轴
            Axis dispenseZ = HardwareRepositoryService.GetHardware<Axis>("点胶Z");

            Task.Run(
                () => {

                    while (this.testFlag)
                    {
                        dispenseX.AbsoluteMove(0);
                        dispenseX.AbsoluteMove(0);
                    }

                });

            Task.Run(
               () => {

                   while (this.testFlag)
                   {
                       dispenseY.AbsoluteMove(0);
                       dispenseY.AbsoluteMove(0);
                   }

               });

            Task.Run(
               () => {

                   while (this.testFlag)
                   {
                       dispenseZ.AbsoluteMove(0);
                       dispenseZ.AbsoluteMove(0);
                   }
               });
        }

        private void BtWaferTableTest_Click(object sender, EventArgs e)
        {
            this.testFlag = true;

            // BondX轴
            Axis waferTableX = HardwareRepositoryService.GetHardware<Axis>("晶圆台X");

            // BondY轴
            Axis waferTableY = HardwareRepositoryService.GetHardware<Axis>("晶圆台Y");


            Task.Run(
                () => {

                    while (this.testFlag)
                    {
                        waferTableX.AbsoluteMove(0);
                        waferTableX.AbsoluteMove(190);
                    }

                });

            Task.Run(
               () => {

                   while (this.testFlag)
                   {
                       waferTableY.AbsoluteMove(-300);
                       waferTableY.AbsoluteMove(0);
                   }

               });
        }

        private void BtStop_Click(object sender, EventArgs e)
        {
            this.testFlag = false;
        }
    }
}