Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.DataAccess.Sql
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports.UI.CrossTab
Imports System
Imports System.Drawing

Namespace CrossTabReportSample

    Public Partial Class Form1
        Inherits DevExpress.XtraEditors.XtraForm

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs)
            ' Creates a cross-tab report.
            Using report As XtraReport = CreateReport()
                Using tool As ReportPrintTool = New ReportPrintTool(report)
                    ' Shows the Print Preview.
                    tool.ShowRibbonPreviewDialog()
                End Using
            End Using
        End Sub

        Private Function CreateReport() As XtraReport
            ' Creates a blank report.
            Dim crossTabReport As XtraReport = New XtraReport() With {.VerticalContentSplitting = VerticalContentSplitting.Smart, .HorizontalContentSplitting = HorizontalContentSplitting.Smart}
            ' Creates a detail band and adds it to the report.
            Dim detail As DetailBand = New DetailBand()
            crossTabReport.Bands.Add(detail)
            ' Creates a cross tab and adds it to the Detail band.
            Dim crossTab As XRCrossTab = New XRCrossTab()
            detail.Controls.Add(crossTab)
            crossTab.PrintOptions.RepeatColumnHeaders = True
            crossTab.PrintOptions.RepeatRowHeaders = True
            ' Creates a data source.
            Dim connectionParameters As SQLiteConnectionParameters = New SQLiteConnectionParameters("|DataDirectory|\nwind.db", "")
            Dim ds As SqlDataSource = New SqlDataSource(connectionParameters)
            ' Creates an SQL query to access the SalesPerson view.
            Dim query As SelectQuery = SelectQueryFluentBuilder.AddTable("SalesPerson").SelectColumn("CategoryName").SelectColumn("ProductName").SelectColumn("Country").SelectColumn("FullName").SelectColumn("Quantity").SelectColumn("ExtendedPrice").Build("SalesPerson")
            ds.Queries.Add(query)
            ' Binds the cross tab to data.
            crossTab.DataSource = ds
            crossTab.DataMember = "SalesPerson"
            ' Generates cross tab fields.
            crossTab.RowFields.Add(New CrossTabRowField() With {.FieldName = "CategoryName"})
            crossTab.RowFields.Add(New CrossTabRowField() With {.FieldName = "ProductName"})
            crossTab.ColumnFields.Add(New CrossTabColumnField() With {.FieldName = "Country"})
            crossTab.ColumnFields.Add(New CrossTabColumnField() With {.FieldName = "FullName"})
            crossTab.DataFields.Add(New CrossTabDataField() With {.FieldName = "Quantity"})
            crossTab.DataFields.Add(New CrossTabDataField() With {.FieldName = "ExtendedPrice"})
            crossTab.GenerateLayout()
            ' 
            ' +----------------+---------------+-------------------------------+---------------------------+---------------------------+
            ' | Category Name  | Product Name  | [Country]                     | Total [Country]           | Grand total               |
            ' |                |               +-------------------------------+                           |                           |
            ' |                |               | [FullName]                    |                           |                           |
            ' |                |               +------------+------------------+----------+----------------+----------+----------------+
            ' |                |               | Quantity   | Extended Price   | Quantity | Extended Price | Quantity | Extended Price |
            ' +----------------+---------------+------------+------------------+----------+----------------+----------+----------------+
            ' | [CategoryName] | [ProductName] | [Quantity] | [ExtendedPrice]  |          |                |          |                |
            ' +----------------+---------------+------------+------------------+----------+----------------+----------+----------------+
            ' | Total [CategoryName]           |            |                  |          |                |          |                |
            ' +--------------------------------+------------+------------------+----------+----------------+----------+----------------+
            ' | Grand Total                    |            |                  |          |                |          |                |
            ' +--------------------------------+------------+------------------+----------+----------------+----------+----------------+
            ' 
            ' Adjusts the generated cells.
            For Each c In crossTab.ColumnDefinitions
                ' Enables auto-width for all columns.
                c.AutoWidthMode = DevExpress.XtraReports.UI.AutoSizeMode.GrowOnly
            Next

            For Each c As XRCrossTabCell In crossTab.Cells
                If c.DataLevel = 1 AndAlso c.RowIndex <> 2 Then
                    ' Adjusts format string for the "Extended Price" cells.
                    c.TextFormatString = "{0:c}"
                End If
            Next

            ' Assigns styles to the cross tab.
            crossTab.CrossTabStyles.GeneralStyle = New XRControlStyle() With {.Name = "Default", .Borders = BorderSide.All, .Padding = New PaddingInfo() With {.All = 2}}
            crossTab.CrossTabStyles.TotalAreaStyle = New XRControlStyle() With {.Name = "Data", .TextAlignment = TextAlignment.TopRight}
            crossTab.CrossTabStyles.DataAreaStyle = crossTab.CrossTabStyles.TotalAreaStyle
            crossTab.CrossTabStyles.HeaderAreaStyle = New XRControlStyle() With {.Name = "HeaderAndTotals", .BackColor = Color.WhiteSmoke}
            Return crossTabReport
        End Function
    End Class
End Namespace
