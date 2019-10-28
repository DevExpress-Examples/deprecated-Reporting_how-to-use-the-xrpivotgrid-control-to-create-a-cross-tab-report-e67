#Region "#Reference"
Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.DataAccess.Sql
Imports DevExpress.XtraPivotGrid
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports.UI.CrossTab
Imports DevExpress.XtraReports.UI.PivotGrid
Imports ExpressionBinding = DevExpress.XtraReports.UI.ExpressionBinding
' ...
#End Region ' #Reference

Namespace docXRPivotGrid
	Partial Public Class Form1
		Inherits Form

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button1.Click
			' Create a cross-tab report.
			Using report As XtraReport = CreateReport()
			Using tool As New ReportPrintTool(report)
				' Show its Print Preview.
				tool.ShowRibbonPreviewDialog()
			End Using
			End Using
		End Sub

		#Region "#Report Generation Code"

		Private Function CreateReport() As XtraReport
			' Create a blank report.
			Dim crossTabReport As New XtraReport() With {
				.VerticalContentSplitting = VerticalContentSplitting.Smart,
				.HorizontalContentSplitting = HorizontalContentSplitting.Smart
			}

			' Create a detail band and add it to the report.
			Dim detail As New DetailBand()
			crossTabReport.Bands.Add(detail)

			' Create a cross tab and add it to the Detail band.
			Dim crossTab As New XRCrossTab()
			detail.Controls.Add(crossTab)
			crossTab.PrintOptions.RepeatColumnHeaders = True
			crossTab.PrintOptions.RepeatRowHeaders = True

			' Create a data source
			Dim connectionParameters As New Access97ConnectionParameters("|DataDirectory|\nwind.mdb", "", "")
			Dim ds As New SqlDataSource(connectionParameters)

			' Create an SQL query to access the SalesPerson view.
			Dim query As SelectQuery = SelectQueryFluentBuilder.AddTable("SalesPerson").SelectColumn("CategoryName").SelectColumn("ProductName").SelectColumn("Country").SelectColumn("Sales Person").SelectColumn("Quantity").SelectColumn("Extended Price").Build("SalesPerson")
			ds.Queries.Add(query)

			' Bind the cross tab to data.
			crossTab.DataSource = ds
			crossTab.DataMember = "SalesPerson"

			' Generate cross tab's fields.
			Dim cellCategoryName As New XRCrossTabCell()
			crossTab.RowFields.Add(New CrossTabRowField() With {.FieldName = "CategoryName"})
			crossTab.RowFields.Add(New CrossTabRowField() With {.FieldName = "ProductName"})
			crossTab.ColumnFields.Add(New CrossTabColumnField() With {.FieldName = "Country"})
			crossTab.ColumnFields.Add(New CrossTabColumnField() With {.FieldName = "Sales Person"})
			crossTab.DataFields.Add(New CrossTabDataField() With {.FieldName = "Quantity"})
			crossTab.DataFields.Add(New CrossTabDataField() With {.FieldName = "Extended Price"})
			crossTab.GenerateLayout()

			For Each c In crossTab.ColumnDefinitions
				c.AutoWidthMode = DevExpress.XtraReports.UI.AutoSizeMode.GrowOnly
			Next c


			' Assign styles to cross tab
			crossTab.CrossTabStyles.GeneralStyle = New XRControlStyle() With {
				.Name = "Default",
				.Borders = BorderSide.All,
				.Padding = New PaddingInfo() With {.All = 2}
			}
'INSTANT VB WARNING: An assignment within expression was extracted from the following statement:
'ORIGINAL LINE: crossTab.CrossTabStyles.DataAreaStyle = crossTab.CrossTabStyles.TotalAreaStyle = new XRControlStyle() { Name = "Data", TextAlignment = TextAlignment.TopRight };
			crossTab.CrossTabStyles.TotalAreaStyle = New XRControlStyle() With {
				.Name = "Data",
				.TextAlignment = TextAlignment.TopRight
			}
			crossTab.CrossTabStyles.DataAreaStyle = crossTab.CrossTabStyles.TotalAreaStyle
			crossTab.CrossTabStyles.HeaderAreaStyle = New XRControlStyle() With {
				.Name = "HeaderAndTotals",
				.BackColor = Color.WhiteSmoke
			}
			Return crossTabReport
		End Function
		#End Region ' #Code
	End Class
End Namespace



