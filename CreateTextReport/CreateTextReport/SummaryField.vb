Public Enum enumTRSummaryType
    Sum = 1
    Count = 2
    Average = 3
End Enum
Public Class SummaryField
    Inherits TextReportField
    Public Property ColumnName As String
    Public Property DR As System.Data.DataRow
    Dim CalculatedValue As Double
    Dim Counter As Integer = 0
    Dim SummaryType As enumTRSummaryType = enumTRSummaryType.Count
    Public Sub New(pColumnName As String, pLength As Integer, pColumn As Integer)
        MyBase.New(pLength, pColumn)
        FieldType = enumFieldType.SummaryField
        ColumnName = pColumnName
    End Sub
    Public Sub New(pColumnName As String, pLength As Integer, pColumn As Integer, pAlignment As enumFieldAlignment)
        MyClass.New(pColumnName, pLength, pColumn)
        Alignment = pAlignment
    End Sub
    Public Sub New(pColumnName As String, pLength As Integer, pColumn As Integer, pAlignment As enumFieldAlignment,
                   pSummaryType As enumTRSummaryType)
        MyClass.New(pColumnName, pLength, pColumn, pAlignment)
        SummaryType = pSummaryType
    End Sub
    Public Sub Initialize()
        CalculatedValue = 0
        Counter = 0
    End Sub
    Public Sub CalculateSummary(pRow As DataRow)
        Counter += 1
        If SummaryType = enumTRSummaryType.Average Or SummaryType = enumTRSummaryType.Sum Then
            CalculatedValue += Val(pRow.Item(ColumnName).ToString)
        End If
    End Sub
    Public Overrides Function GetDataValue() As String
        If SummaryType = enumTRSummaryType.Sum Then
            Return CalculatedValue
        End If
        If SummaryType = enumTRSummaryType.Average Then
            If Counter > 0 Then
                Return CalculatedValue / Counter
            End If
            Return 0
        End If
        Return Counter
    End Function
End Class

