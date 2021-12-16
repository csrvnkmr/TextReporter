Public Class TextReportGroup
    Public Sub New(pField As TextReportField)
        ChangesOn = pField
    End Sub
    Public Property ChangesOn As DBField
    Public ReadOnly Property Headers As New TextReportSectionCollection
    Public ReadOnly Property Footers As New TextReportSectionCollection
    Public Property GroupValue As String
    Public Property SummaryFields As New SummaryFieldCollection
    Public Function IsGroupChanging(pRow As DataRow) As Boolean
        ChangesOn.DR = pRow
        If ChangesOn.GetDataValue() = GroupValue Then
            Return False
        End If
        Return True
    End Function
    Public Sub Initialize()
        SummaryFields.Initialize()
    End Sub
    Public Sub StoreValue(pRow As DataRow)
        ChangesOn.DR = pRow
        GroupValue = ChangesOn.GetDataValue
        SummaryFields.UpdateSummary(pRow)
    End Sub
    Public Function GetHeaderLines(pRow As DataRow) As List(Of String)
        Return Headers.GetLines(pRow)
    End Function
    Public Function GetFooterLines(pRow As DataRow) As List(Of String)
        Return Footers.GetLines(pRow)
    End Function
End Class
