Public Class TextReportGroupV3
    Public Sub New(pField As TextReportFieldV3)
        ChangesOn = pField
    End Sub
    Public Property ChangesOn As DBFieldV3
    Public ReadOnly Property Headers As New TextReportSectionCollectionV3
    Public ReadOnly Property Footers As New TextReportSectionCollectionV3
    Public Property GroupValue As String
    Public Property SummaryFields As New SummaryFieldCollectionV3
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
