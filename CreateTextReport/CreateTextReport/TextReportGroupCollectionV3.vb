Public Class TextReportGroupCollectionV3
    Inherits List(Of TextReportGroupV3)
    Public Function BreakingGroup(pRow As DataRow) As TextReportGroupV3
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
