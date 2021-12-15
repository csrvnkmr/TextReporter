Public Class FormulaFieldCollectionV3
    Inherits List(Of FormulaFieldV3)
    Public Sub SetDataRow(pDR As DataRow)
        For i = 0 To Count - 1
            Item(i).DR = pDR
        Next
    End Sub
End Class
