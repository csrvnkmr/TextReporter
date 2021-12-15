Public Class DBFieldV3
    Inherits TextReportFieldV3
    Public Property ColumnName As String
    Public Property DR As System.Data.DataRow
    Public Sub New(pColumnName As String, pLength As Integer, pColumn As Integer)
        MyBase.New(pLength, pColumn)
        FieldType = enumFieldType.DataField
        ColumnName = pColumnName
    End Sub
    Public Sub New(pColumnName As String, pLength As Integer, pColumn As Integer, pAlignment As enumFieldAlignment)
        MyClass.New(pColumnName, pLength, pColumn)
        Alignment = pAlignment
    End Sub
    Public Overrides Function GetDataValue() As String
        Return DR.Item(ColumnName)
    End Function
End Class
