Public Class TextReportGroupCollection
    Inherits List(Of TextReportGroup)
    Public Function BreakingGroup(pRow As DataRow) As TextReportGroup
        For i As Integer = 0 To Count - 1
            If Item(i).IsGroupChanging(pRow) Then
                Return Item(i)
            End If
        Next
        Return Nothing
    End Function
    Public Function BreakingGroupIndex(pRow As DataRow) As Integer
        For i As Integer = 0 To Count - 1
            If Item(i).IsGroupChanging(pRow) Then
                Return i
            End If
        Next
        Return -1
    End Function
    Public Function SetGroupValue(pRow As DataRow) As String
        For i As Integer = 0 To Count - 1
            Item(i).StoreValue(pRow)

        Next
        Return ""
    End Function
End Class
