using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ValidateAddIn.Class;
using ValidateAddIn.ServiceReference;
using Error = ValidateAddIn.Class.Error;
using Label = System.Windows.Forms.Label;

namespace ValidateAddIn
{
    public partial class ValidateRibbon
    {
        #region Constants

        private Form _validatingForm;
        private Label _titleLabel;
        private ProgressBar _updateBar;
        private Label _percentageLabel;
        //private Label _cancelButton;

        public static string ActionName { get; set; }

        #endregion

        #region #Events

        /// <summary>
        /// This event work when RunWorkerAsync method is called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!(sender is BackgroundWorker)) return;
            ActionName = "Validating data...";
            backgroundWorker1.ReportProgress(0);
            ValidateData();

        }

        /// <summary>
        /// This event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnValidate_Click(object sender, RibbonControlEventArgs e)
        {
            InitializeComponents();

            _validatingForm.Show();

            backgroundWorker1.RunWorkerAsync();
        }


        /// <summary>
        /// This event wor when ReportProgress(int) method is called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage <= 100)
            {
                //_updateBar.Value = e.ProgressPercentage;
                _percentageLabel.Text = ActionName + @" " + e.ProgressPercentage + @"%";
            }
            else if (e.ProgressPercentage == 999)
            {
                ActionName = ("Validation complete please wait...");
                _percentageLabel.Text = ActionName;
            }

        }

        /// <summary>
        /// This event is called when RunWorkerAsync is completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _validatingForm.Hide();

            var excel = Globals.ThisAddIn.Application;
            excel.ScreenUpdating = true;
        }


        #endregion

        #region Methods

        /// <summary>
        /// Process and write in excel file all errors under validations
        /// </summary>
        /// <param name="result"></param>
        private static void ProcessErrorInformation(ErrorLog result)
        {
            try
            {
                var activeSheet = (Worksheet)Globals.ThisAddIn.Application.ActiveSheet;
                var range = activeSheet.UsedRange;
                var progressRate = 50M;

                var progressRatePerRow = 60M / range.Rows.Count;

                if (result.ErrorList.Any())
                {
                    HiddenColumn(ref range, 1, 4, false);
                    HiddenColumn(ref range, 1, 5, false);

                    for (var row = 6; row <= range.Rows.Count; row++)
                    {
                        var cellValue = string.Empty;

                        int lineNumber = Convert.ToInt32(range[row, 3].Value);

                        var errorsPerRecordList = result.ErrorList.Where(x => x.RecordId == lineNumber)
                            .Select(x => x.ErrorDescription).ToList();

                        var generalErrorsList = result.ErrorList.Where(x => x.RecordId == 0).Select(x => x.ErrorDescription)
                            .ToList();

                        foreach (var errorPerRecord in errorsPerRecordList)
                        {
                            cellValue = cellValue + errorPerRecord + "\r\n";
                        }

                        range[row, 5] = cellValue;

                        cellValue = string.Empty;

                        range.EntireColumn.Hidden = false;

                        foreach (var generalError in generalErrorsList)
                        {
                            cellValue = cellValue + generalError + "\r\n";
                        }

                        range[row, 4] = cellValue;

                        range.Rows.AutoFit();

                        progressRate = progressRate + progressRatePerRow;

                        ActionName = "Process error information...";
                        backgroundWorker1.ReportProgress(Convert.ToInt32(Math.Round(progressRate, MidpointRounding.ToEven)));
                    }
                }
                else
                {
                    HiddenColumn(ref range, 1, 4, true);
                    HiddenColumn(ref range, 1, 5, true);

                    range = activeSheet.UsedRange;
                    range.Rows.RowHeight = 15.43;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }

        }

        /// <summary>
        /// Returns ErrorLog Object from web service result
        /// </summary>
        /// <param name="journalObjectXml">Journal Object created</param>
        /// <returns>ErrorLog object</returns>
        // ReSharper disable once UnusedParameter.Local
        private static ErrorLog CreateErrorLog(GetJournalValidationDC journalObjectXml)
        {
            try
            {
                var result = new ErrorLog();

                var listErrors = new List<Error>();

                var client = new ValidacionesSiacServiceContractClient();

                var obGetJournalValidationCollectionDc = client.GetJournalValidation(journalObjectXml);

                foreach (var item in obGetJournalValidationCollectionDc)
                {
                    var addError = new Error
                    {
                        ErrorDescription = item.ErrorDescription,
                        ProcessId = item.ProcessId,
                        RecordId = item.RecordId,
                        ValidationId = item.ValidationId
                    };

                    listErrors.Add(addError);
                }

                result.ErrorList = listErrors;

                ActionName = ("Creating error log...");
                backgroundWorker1.ReportProgress(43);

                return result;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }

        }

        /// <summary>
        /// Contains all logic to validate excel data.
        /// </summary>
        private static void ValidateData()
        {
            try
            {
                var ws = (Worksheet)Globals.ThisAddIn.Application.ActiveSheet;

                var excel = Globals.ThisAddIn.Application;
                excel.ScreenUpdating = false;

                var rn = ws.UsedRange;

                var journalObjectXml = CreateJournalXml(ref rn);

                var result = CreateErrorLog(journalObjectXml);

                ProcessErrorInformation(result);

                backgroundWorker1.ReportProgress(999);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }

        }

        /// <summary>
        /// Initialize all components of the validating screen
        /// </summary>
        private void InitializeComponents()
        {


            _validatingForm = new Form
            {
                BackColor = Color.Gainsboro,
                Width = 530,
                Height = 100,
                AutoScroll = false,
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterScreen,
                //TopMost = true
            };

            _titleLabel = new Label
            {
                Font = new System.Drawing.Font("Microsoft Tai Le", 10, FontStyle.Bold),
                Height = 20,
                Width = 530,
                // ReSharper disable once LocalizableElement
                Text = "Validating your data...",
                Location = new System.Drawing.Point(0, 0),
                ForeColor = Color.AntiqueWhite,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.DarkSlateGray
            };

            _updateBar = new ProgressBar
            {
                Location = new System.Drawing.Point(0, 40),
                Height = 30,
                Width = 530,
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 26,
                BackColor = Color.DarkSlateGray,
                ForeColor = Color.DarkSlateGray,

            };

            _percentageLabel = new Label
            {
                Font = new System.Drawing.Font("Microsoft Tai Le", 10),
                Height = 20,
                Width = 530,
                Location = new System.Drawing.Point(0, 70),
                ForeColor = Color.DarkSlateGray,
            };

            //_cancelButton = new Label
            //{
            //    // ReSharper disable once LocalizableElement
            //    Text = "Cancel",
            //    Location = new System.Drawing.Point(635, 130),
            //    Height = 30,
            //    Width = 100,
            //    ForeColor = Color.AntiqueWhite,
            //    Font = new System.Drawing.Font("Microsoft Tai Le", 17),
            //    BorderStyle = BorderStyle.None,
            //    BackColor = Color.DarkGray,
            //    TextAlign = ContentAlignment.BottomCenter,
            //    Cursor = Cursors.Hand

            //};

            _validatingForm.Controls.Add(_titleLabel);
            _validatingForm.Controls.Add(_updateBar);
            _validatingForm.Controls.Add(_percentageLabel);

            //_validatingForm.Controls.Add(_cancelButton);
            //_cancelButton.Click += (s, e) =>
            //{
            //    Thread.CurrentThread.Abort();
            //};
        }

        /// <summary>
        /// Hide or show specific column in the range
        /// </summary>
        /// <param name="range"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="isHidden"></param>
        private static void HiddenColumn(ref Range range, int row, int column, bool isHidden)
        {
            range[row, column].EntireColumn.Hidden = isHidden;
        }

        /// <summary>
        /// Create the journal object and serialize to XML 
        /// </summary>
        /// <param name="rn">Used range</param>
        /// <returns>Journal serialize to XML</returns>
        private static GetJournalValidationDC CreateJournalXml(ref Range rn)
        {
            try
            {
                var journal = new GetJournalValidationDC();
                var detailList = new List<Detail>();
                var header = new Header
                {
                    SourceSystemId = Convert.ToInt32(rn[3, 1].Value),
                    AccountingDate = rn[3, 6].Value,
                    Branch = 0,
                    BusinessUnit = Convert.ToInt32(rn[3, 2].Value),
                    RecordType = "PS"
                };

                journal.Header = header;

                for (var row = 6; row <= rn.Rows.Count; row++)
                {
                    var journalDetail = new Detail()

                    {
                        RegisterId = Convert.ToInt32(rn[row, 3].Value),
                        Ledger = Convert.ToString(rn[row, 7].Value),
                        PsAccount = Convert.ToString(rn[row, 8].Value),
                        AltAccount = Convert.ToString(rn[row, 9].Value),
                        OperatingUnit = Convert.ToString(rn[row, 10].Value),
                        DepId = Convert.ToString(rn[row, 11].Value),
                        Product = Convert.ToString(rn[row, 12].Value),
                        PoliceYear = Convert.ToString(rn[row, 13].Value),
                        Mcc = Convert.ToString(rn[row, 14].Value),
                        DistributionChannel = Convert.ToString(rn[row, 15].Value),
                        Location = Convert.ToString(rn[row, 16].Value),
                        Function = Convert.ToString(rn[row, 17].Value),
                        ProjectId = Convert.ToString(rn[row, 18].Value),
                        Year = Convert.ToInt32(rn[row, 19].Value),
                        Affiliate = Convert.ToString(rn[row, 20].Value),
                        OriginalCurrency = Convert.ToString(rn[row, 21].Value),
                        OriginalAmount = Convert.ToDecimal(rn[row, 22].Value),
                        JrnlLnRef = Convert.ToString(rn[row, 28].Value),
                        LineDescription = Convert.ToString(rn[row, 28].Value)
                    };

                    if (journalDetail.PsAccount != null)
                    {
                        detailList.Add(journalDetail);
                    }

                }

                journal.AccountingDetail = detailList.ToArray();
                journal.Header.RegisterCount = detailList.Count;

                ActionName = "Creating journal XML...";
                backgroundWorker1.ReportProgress(20);
                return journal;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        #endregion

    }
}
