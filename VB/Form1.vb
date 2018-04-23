#Region "#Reference"
Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.Windows.Forms
Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.DataAccess.Sql
Imports DevExpress.XtraPivotGrid
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports.UI.PivotGrid
' ...
#End Region ' #Reference

Namespace docXRPivotGrid
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

#Region "#Code"
Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button1.Click
    ' Create a cross-tab report.
    Dim report As XtraReport = CreateReport()

    ' Show its Print Preview.
    report.ShowPreview()
End Sub

Private Function CreateReport() As XtraReport
    ' Create a blank report.
    Dim crossTabReport As New XtraReport()

    ' Create a detail band and add it to the report.
    Dim detail As New DetailBand()
    crossTabReport.Bands.Add(detail)

    ' Create a pivot grid and add it to the Detail band.
    Dim pivotGrid As New XRPivotGrid()
    detail.Controls.Add(pivotGrid)

    ' Create a data source
    Dim connectionParameters As New Access97ConnectionParameters("..\..\nwind.mdb", "", "")
    Dim ds As New SqlDataSource(connectionParameters)

    ' Create an SQL query to access the SalesPerson view.
    Dim query As New CustomSqlQuery()
    query.Name = "SalesPerson"
    query.Sql = "SELECT CategoryName, ProductName, Country, [Sales Person], Quantity, [Extended Price]  FROM SalesPerson"
    ds.Queries.Add(query)

    ' Bind the pivot grid to data.
    pivotGrid.DataSource = ds
    pivotGrid.DataMember = "SalesPerson"

    ' Generate pivot grid's fields.
    Dim fieldCategoryName As New XRPivotGridField("CategoryName", PivotArea.RowArea)
    Dim fieldProductName As New XRPivotGridField("ProductName", PivotArea.RowArea)
    Dim fieldCountry As New XRPivotGridField("Country", PivotArea.ColumnArea)
    Dim fieldSalesPerson As New XRPivotGridField("Sales Person", PivotArea.ColumnArea)
    Dim fieldQuantity As New XRPivotGridField("Quantity", PivotArea.DataArea)
    Dim fieldExtendedPrice As New XRPivotGridField("Extended Price", PivotArea.DataArea)

    ' Add these fields to the pivot grid.
    pivotGrid.Fields.AddRange(New XRPivotGridField() {fieldCategoryName, fieldProductName, fieldCountry, fieldSalesPerson, fieldQuantity, fieldExtendedPrice})

    Return crossTabReport
End Function
#End Region ' #Code
    End Class
End Namespace



