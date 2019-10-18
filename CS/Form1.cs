#region #Reference
using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UI.CrossTab;
using DevExpress.XtraReports.UI.PivotGrid;
using ExpressionBinding = DevExpress.XtraReports.UI.ExpressionBinding;
// ...
#endregion #Reference

namespace docXRPivotGrid {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            // Create a cross-tab report.
            using (XtraReport report = CreateReport())
            using (ReportPrintTool tool = new ReportPrintTool(report)) {
                // Show its Print Preview.
                tool.ShowRibbonPreviewDialog();
            }
        }

        #region #Report Generation Code

        private XtraReport CreateReport() {
            // Create a blank report.
            XtraReport crossTabReport = new XtraReport() {
                VerticalContentSplitting = VerticalContentSplitting.Smart,
                HorizontalContentSplitting = HorizontalContentSplitting.Smart
            };

            // Create a detail band and add it to the report.
            DetailBand detail = new DetailBand();
            crossTabReport.Bands.Add(detail);

            // Create a cross tab and add it to the Detail band.
            XRCrossTab crossTab = new XRCrossTab();
            detail.Controls.Add(crossTab);
            crossTab.PrintOptions.RepeatColumnHeaders = true;
            crossTab.PrintOptions.RepeatRowHeaders = true;

            // Create a data source
            Access97ConnectionParameters connectionParameters = new Access97ConnectionParameters(@"|DataDirectory|\nwind.mdb", "", "");
            SqlDataSource ds = new SqlDataSource(connectionParameters);

            // Create an SQL query to access the SalesPerson view.
            SelectQuery query = SelectQueryFluentBuilder.AddTable("SalesPerson")
                        .SelectColumn("CategoryName")
                        .SelectColumn("ProductName")
                        .SelectColumn("Country")
                        .SelectColumn("Sales Person")
                        .SelectColumn("Quantity")
                        .SelectColumn("Extended Price").Build("SalesPerson");
            ds.Queries.Add(query);

            // Bind the cross tab to data.
            crossTab.DataSource = ds;
            crossTab.DataMember = "SalesPerson";

            // Generate cross tab's fields.
            XRCrossTabCell cellCategoryName = new XRCrossTabCell();
            crossTab.RowFields.Add(new CrossTabRowField() { FieldName = "CategoryName" });
            crossTab.RowFields.Add(new CrossTabRowField() { FieldName = "ProductName" });
            crossTab.ColumnFields.Add(new CrossTabColumnField() { FieldName = "Country" });
            crossTab.ColumnFields.Add(new CrossTabColumnField() { FieldName = "Sales Person" });
            crossTab.DataFields.Add(new CrossTabDataField() { FieldName = "Quantity" });
            crossTab.DataFields.Add(new CrossTabDataField() { FieldName = "Extended Price" });
            crossTab.GenerateLayout();
            
            foreach(var c in crossTab.ColumnDefinitions) {
                c.AutoWidthMode = DevExpress.XtraReports.UI.AutoSizeMode.GrowOnly;
            }


            // Assign styles to cross tab
            crossTab.CrossTabStyles.GeneralStyle = new XRControlStyle() { 
                Name = "Default",
                Borders = BorderSide.All,
                Padding = new PaddingInfo() { All = 2 }                
            };
            crossTab.CrossTabStyles.DataAreaStyle = crossTab.CrossTabStyles.TotalAreaStyle = new XRControlStyle() {
                Name = "Data",
                TextAlignment = TextAlignment.TopRight
            };
            crossTab.CrossTabStyles.HeaderAreaStyle = new XRControlStyle() {
                Name = "HeaderAndTotals",
                BackColor = Color.WhiteSmoke
            };
            return crossTabReport;
        }
        #endregion #Code
    }
}



