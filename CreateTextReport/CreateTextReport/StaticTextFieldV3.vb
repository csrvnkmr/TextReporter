Public Class StaticTextFieldV3
    Inherits TextReportFieldV3
    Public Property Text As String

    Public Sub New(pText As String, pLength As Integer, pColumn As Integer)
        MyBase.New(pLength, pColumn)
        FieldType = enumFieldType.StaticText
        Text = pText
    End Sub
    Public Sub New(pText As String, pLength As Integer, pColumn As Integer, pAlignment As enumFieldAlignment)
        MyClass.New(pText, pLength, pColumn)
        Alignment = pAlignment
    End Sub
    Public Overrides Function GetDataValue() As String
        Return Text
    End Function
End Class
