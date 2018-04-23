#region #Reference
using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UI.PivotGrid;
// ...
#endregion #Reference

namespace docXRPivotGrid {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

#region #Code
private void button1_Click(object sender, EventArgs e) {
    // Create a cross-tab report.
    XtraReport report = CreateReport();

    // Show its Print Preview.
    report.ShowPreview();
}

private XtraReport CreateReport() {
    // Create a blank report.
    XtraReport crossTabReport = new XtraReport();

    // Create a detail band and add it to the report.
    DetailBand detail = new DetailBand();
    crossTabReport.Bands.Add(detail);

    // Create a pivot grid and add it to the Detail band.
    XRPivotGrid pivotGrid = new XRPivotGrid();
    detail.Controls.Add(pivotGrid);

    // Create a data source
    Access97ConnectionParameters connectionParameters = new Access97ConnectionParameters("..\\..\\nwind.mdb", "", "");
    SqlDataSource ds = new SqlDataSource(connectionParameters);

    // Create an SQL query to access the SalesPerson view.
    CustomSqlQuery query = new CustomSqlQuery();
    query.Name = "SalesPerson";
    query.Sql = "SELECT CategoryName, ProductName, Country, [Sales Person], Quantity, [Extended Price]  FROM SalesPerson";
    ds.Queries.Add(query);
    
    // Bind the pivot grid to data.
    pivotGrid.DataSource = ds;
    pivotGrid.DataMember = "SalesPerson";

    // Generate pivot grid's fields.
    XRPivotGridField fieldCategoryName = new XRPivotGridField("CategoryName", PivotArea.RowArea);
    XRPivotGridField fieldProductName = new XRPivotGridField("ProductName", PivotArea.RowArea);
    XRPivotGridField fieldCountry = new XRPivotGridField("Country", PivotArea.ColumnArea);
    XRPivotGridField fieldSalesPerson = new XRPivotGridField("Sales Person", PivotArea.ColumnArea);
    XRPivotGridField fieldQuantity = new XRPivotGridField("Quantity", PivotArea.DataArea);
    XRPivotGridField fieldExtendedPrice = new XRPivotGridField("Extended Price", PivotArea.DataArea);

    // Add these fields to the pivot grid.
    pivotGrid.Fields.AddRange(new XRPivotGridField[] {fieldCategoryName, fieldProductName, fieldCountry, 
        fieldSalesPerson, fieldQuantity, fieldExtendedPrice});

    return crossTabReport;
}
#endregion #Code
    }
}



