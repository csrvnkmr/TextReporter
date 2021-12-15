Public Delegate Function dlgFormula(pRpt As TextReportV3, pDataRow As DataRow) As Object
Public Class FormulaFieldV3
    Inherits TextReportFieldV3
    Dim mReport As TextReportV3
    Dim mFormula As dlgFormula
    Public Property DR As DataRow
    Public Sub New(pRpt As TextReportV3, pDlgFormula As dlgFormula, pLength As Integer, pColumn As Integer)
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
