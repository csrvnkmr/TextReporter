Public Class SummaryFieldCollection
    Inherits List(Of SummaryField)
    Public Sub UpdateSummary(pRow As DataRow)
        For i = 0 To Count - 1
            Item(i).CalculateSummary(pRow)
        Next
    End Sub
    Public Sub Initialize()
        For i = 0 To Count - 1
            Item(i).Initialize()
        Next
    End Sub
End Class
