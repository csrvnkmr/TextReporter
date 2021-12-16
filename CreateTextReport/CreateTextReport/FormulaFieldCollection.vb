Public Class FormulaFieldCollection
    Inherits List(Of FormulaField)
    Public Sub SetDataRow(pDR As DataRow)
        For i = 0 To Count - 1
            Item(i).DR = pDR
        Next
    End Sub
End Class
