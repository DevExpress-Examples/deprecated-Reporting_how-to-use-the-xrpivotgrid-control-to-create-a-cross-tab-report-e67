using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UI.CrossTab;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrossTabReportSample
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            // Creates a cross-tab report.
            using (XtraReport report = CreateReport())
            using (ReportPrintTool tool = new ReportPrintTool(report)) {
                // Shows the Print Preview.
                tool.ShowRibbonPreviewDialog();
            }
        }

        private XtraReport CreateReport() {
            // Creates a blank report.
            XtraReport crossTabReport = new XtraReport() {
                VerticalContentSplitting = VerticalContentSplitting.Smart,
                HorizontalContentSplitting = HorizontalContentSplitting.Smart
            };

            // Creates a detail band and adds it to the report.
            DetailBand detail = new DetailBand();
            crossTabReport.Bands.Add(detail);

            // Creates a cross tab and adds it to the Detail band.
            XRCrossTab crossTab = new XRCrossTab();
            detail.Controls.Add(crossTab);
            crossTab.PrintOptions.RepeatColumnHeaders = true;
            crossTab.PrintOptions.RepeatRowHeaders = true;

            // Creates a data source.
            SQLiteConnectionParameters connectionParameters = new SQLiteConnectionParameters(@"|DataDirectory|\nwind.db", "");
            SqlDataSource ds = new SqlDataSource(connectionParameters);

            // Creates an SQL query to access the SalesPerson view.
            SelectQuery query = SelectQueryFluentBuilder.AddTable("SalesPerson")
                        .SelectColumn("CategoryName")
                        .SelectColumn("ProductName")
                        .SelectColumn("Country")
                        .SelectColumn("FullName")
                        .SelectColumn("Quantity")
                        .SelectColumn("ExtendedPrice").Build("SalesPerson");
            ds.Queries.Add(query);

            // Binds the cross tab to data.
            crossTab.DataSource = ds;
            crossTab.DataMember = "SalesPerson";

            // Generates cross tab fields.
            crossTab.RowFields.Add(new CrossTabRowField() { FieldName = "CategoryName" });
            crossTab.RowFields.Add(new CrossTabRowField() { FieldName = "ProductName" });
            crossTab.ColumnFields.Add(new CrossTabColumnField() { FieldName = "Country" });
            crossTab.ColumnFields.Add(new CrossTabColumnField() { FieldName = "FullName" });
            crossTab.DataFields.Add(new CrossTabDataField() { FieldName = "Quantity" });
            crossTab.DataFields.Add(new CrossTabDataField() { FieldName = "ExtendedPrice" });
            crossTab.GenerateLayout();
            /*
            +----------------+---------------+-------------------------------+---------------------------+---------------------------+
            | Category Name  | Product Name  | [Country]                     | Total [Country]           | Grand total               |
            |                |               +-------------------------------+                           |                           |
            |                |               | [FullName]                    |                           |                           |
            |                |               +------------+------------------+----------+----------------+----------+----------------+
            |                |               | Quantity   | Extended Price   | Quantity | Extended Price | Quantity | Extended Price |
            +----------------+---------------+------------+------------------+----------+----------------+----------+----------------+
            | [CategoryName] | [ProductName] | [Quantity] | [ExtendedPrice]  |          |                |          |                |
            +----------------+---------------+------------+------------------+----------+----------------+----------+----------------+
            | Total [CategoryName]           |            |                  |          |                |          |                |
            +--------------------------------+------------+------------------+----------+----------------+----------+----------------+
            | Grand Total                    |            |                  |          |                |          |                |
            +--------------------------------+------------+------------------+----------+----------------+----------+----------------+
            */
            // Adjusts the generated cells.
            foreach(var c in crossTab.ColumnDefinitions) {
                // Enables auto-width for all columns.
                c.AutoWidthMode = DevExpress.XtraReports.UI.AutoSizeMode.GrowOnly;
            }

            foreach(XRCrossTabCell c in crossTab.Cells) {
                if(c.DataLevel == 1 && c.RowIndex != 2) {
                    // Adjusts format string for the "Extended Price" cells.
                    c.TextFormatString = "{0:c}";
                }
            }


            // Assigns styles to the cross tab.
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
    }
}



