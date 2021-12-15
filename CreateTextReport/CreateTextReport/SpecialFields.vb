Public MustInherit Class baseSpecialFields
    Inherits TextReportFieldV3
    Protected mReport As TextReportV3
    Public Sub New(pRpt As TextReportV3, pLength As Integer, pColumn As Integer)
        MyBase.New(pLength, pColumn)
        mReport = pRpt
    End Sub
End Class
Public Class PageNumberField
    Inherits baseSpecialFields
    Public Sub New(pRpt As TextReportV3, pLength As Integer, pColumn As Integer)
        MyBase.New(pRpt, pLength, pColumn)
    End Sub
    Public Overrides Function GetDataValue() As String
        Return mReport.PageNumber.ToString
    End Function
End Class
Public Class LineNumberField
    Inherits baseSpecialFields
    Public Sub New(pRpt As TextReportV3, pLength As Integer, pColumn As Integer)
        MyBase.New(pRpt, pLength, pColumn)
    End Sub
    Public Overrides Function GetDataValue() As String
        Return mReport.LineNumber.ToString
    End Function
End Class
Public Class DateField
    Inherits baseSpecialFields
    Public Property Format As String = "dd/MM/yyyy"
    Public Sub New(pRpt As TextReportV3, pLength As Integer, pColumn As Integer)
        MyBase.New(pRpt, pLength, pColumn)
    End Sub
    Public Sub New(pRpt As TextReportV3, pLength As Integer, pColumn As Integer, pFormat As String)
        MyBase.New(pRpt, pLength, pColumn)
        Format = pFormat
    End Sub
    Public Overrides Function GetDataValue() As String
        Return Now.ToString(Format)
    End Function
End Class
