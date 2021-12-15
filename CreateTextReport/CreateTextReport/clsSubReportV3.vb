Public Class SubReportV3
    Property dlgDataTable As dlgGetSubreportDT
    Property Column As Integer
    Property Length As Integer
    Property SubReport As TextReportV3
    Property ParentReport As TextReportV3
    Public Sub New(pDlgDataTable As dlgGetSubreportDT, pColumn As Integer, pLength As Integer,
                   pRpt As TextReportV3, pParentReport As TextReportV3)
        dlgDataTable = pDlgDataTable
        Column = pColumn
        Length = pLength
        SubReport = pRpt
        ParentReport = pParentReport
    End Sub
    Public Function GetReportLines(pDataRow As DataRow) As List(Of String)
        Dim mDt As DataTable = dlgDataTable(ParentReport, SubReport, pDataRow)
        Return SubReport.GetReportLines(mDt)
    End Function

End Class
