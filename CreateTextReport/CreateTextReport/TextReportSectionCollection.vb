Public Class TextReportSectionCollection
    Inherits List(Of TextReportSection)
    Public Function GetLines(pRow As DataRow) As List(Of String)
        Dim mLines As New List(Of String)
        Dim mCount As Integer = Me.Count
        For i As Integer = 0 To mCount - 1
            Dim mSectionLines = Item(i).GetLines(pRow)
            mLines.AddRange(mSectionLines)
        Next
        Return mLines
    End Function
End Class
