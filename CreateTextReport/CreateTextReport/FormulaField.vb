Public Delegate Function dlgFormula(pRpt As TextReport, pDataRow As DataRow) As Object
Public Class FormulaField
    Inherits TextReportField
    Dim mReport As TextReport
    Dim mFormula As dlgFormula
    Public Property DR As DataRow
    Public Sub New(pRpt As TextReport, pDlgFormula As dlgFormula, pLength As Integer, pColumn As Integer)
        MyBase.New(pLength, pColumn)
        mReport = pRpt
        mFormula = pDlgFormula
    End Sub
    Public Overrides Function GetDataValue() As String
        If mFormula IsNot Nothing Then
            Dim mObj As Object = mFormula(mReport, DR)
            Return mObj.ToString
        End If
        Return ""
    End Function
End Class
