Public Class SubReport
    Property dlgDataTable As dlgGetSubreportDT
    Property Column As Integer
    Property Length As Integer
    Property SubReport As TextReport
    Property ParentReport As TextReport
    Public Sub New(pDlgDataTable As dlgGetSubreportDT, pColumn As Integer, pLength As Integer,
                   pRpt As TextReport, pParentReport As TextReport)
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
