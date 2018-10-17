using System.Windows.Forms;

namespace ValidateAddIn
{
    partial class ValidateRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ValidateRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ValidateTab = this.Factory.CreateRibbonTab();
            this.ValidateGroup = this.Factory.CreateRibbonGroup();
            this.BtnValidate = this.Factory.CreateRibbonButton();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.ValidateTab.SuspendLayout();
            this.ValidateGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // ValidateTab
            // 
            this.ValidateTab.Groups.Add(this.ValidateGroup);
            this.ValidateTab.Label = "Validate";
            this.ValidateTab.Name = "ValidateTab";
            // 
            // ValidateGroup
            // 
            this.ValidateGroup.Items.Add(this.BtnValidate);
            this.ValidateGroup.Label = "Validate Data";
            this.ValidateGroup.Name = "ValidateGroup";
            // 
            // BtnValidate
            // 
            this.BtnValidate.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.BtnValidate.Description = "Validate previous information instead of sen to PS";
            this.BtnValidate.Image = global::ValidateAddIn.Properties.Resources.tool;
            this.BtnValidate.ImageName = "Validate";
            this.BtnValidate.Label = "Validate";
            this.BtnValidate.Name = "BtnValidate";
            this.BtnValidate.ShowImage = true;
            this.BtnValidate.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnValidate_Click);
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // ValidateRibbon
            // 
            this.Name = "ValidateRibbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.ValidateTab);
            this.ValidateTab.ResumeLayout(false);
            this.ValidateTab.PerformLayout();
            this.ValidateGroup.ResumeLayout(false);
            this.ValidateGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab ValidateTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup ValidateGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton BtnValidate;
        public static System.ComponentModel.BackgroundWorker backgroundWorker1;
    }

    partial class ThisRibbonCollection
    {
        internal ValidateRibbon ValidateRibbon
        {
            get { return this.GetRibbon<ValidateRibbon>(); }
        }
    }
}
