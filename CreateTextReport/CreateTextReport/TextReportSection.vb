Public Delegate Function dlgGetSubreportDT(pParentReport As TextReport, pSubReport As TextReport, pDataRow As DataRow) As DataTable
Public Class TextReportSection
    Public ReadOnly Property Fields As New Dictionary(Of Integer, List(Of TextReportField))
    Public ReadOnly Property Subreports As New Dictionary(Of Integer, List(Of SubReport))
    Public Sub AddFields(pLineNumber As Integer, pFields As IEnumerable(Of TextReportField))
        If Not Fields.ContainsKey(pLineNumber) Then
            Fields.Add(pLineNumber, New List(Of TextReportField))
        End If

        Fields(pLineNumber).AddRange(pFields)
    End Sub
    Dim mLines As Dictionary(Of Integer, String)
    Public Function SetText(pRow As Integer, pColumn As Integer, pLength As Integer, pText As String) As String
        If Not mLines.ContainsKey(pRow) Then
            mLines.Add(pRow, "")
        End If
        Dim mLineText As String = mLines(pRow)
        If Len(mLineText) < pColumn + pLength - 1 Then
            mLineText = mLineText.PadRight(pColumn + pLength - 1)
        End If
        Dim mLeftText = "", mRightText = ""
        If pColumn > 1 Then
            mLeftText = Left(mLineText, pColumn - 1)
        End If
        If Len(mLineText) > pColumn + pLength - 1 Then
            mRightText = mLineText.Substring(pColumn + pLength)
        End If
        Dim mText = pText
        If pText Is Nothing Then
            mText = ""
        End If
        If Len(mText) < pLength Then
            mText = mText.PadRight(pLength)
        ElseIf Len(mText) > pLength Then
            mText = Left(pText, pLength)
        End If
        mLineText = mLeftText + mText + mRightText
        mLines(pRow) = mLineText
        Return ""
    End Function

    Public Function GetLines(pDR As DataRow) As List(Of String)
        mLines = New Dictionary(Of Integer, String)
        Dim mLastLine As Integer = 1
        For Each mKey As Integer In Fields.Keys
            If mLastLine < mKey Then
                mLastLine = mKey
            End If
        Next
        For i As Integer = 1 To mLastLine
            mLines.Add(i, "")
        Next
        For Each mKey As Integer In Fields.Keys
            For Each mField In Fields(mKey)
                Dim mCurrentline As Integer = 1
                If mField.FieldType = enumFieldType.DataField Then
                    Dim mDBField As DBField = mField
                    mDBField.DR = pDR
                End If
                Dim mValue As String = mField.GetValue(mCurrentline)
                Do
                    SetText(mKey + mCurrentline - 1, mField.Column, mField.Length, mValue)
                    mCurrentline += 1
                    mValue = mField.GetValue(mCurrentline)
                Loop While mValue & "" <> ""
            Next
        Next
        For Each mKey As Integer In Subreports.Keys
            For Each mSubreport In Subreports(mKey)
                mSubreport.SubReport.PageSize = 99999999
                Dim mSubreportLines As List(Of String) = mSubreport.GetReportLines(pDR)
                If mSubreportLines Is Nothing Then
                    Continue For
                End If
                Dim mCurrentline As Integer = 1
                For Each mSubreportLine In mSubreportLines
                    SetText(mKey + mCurrentline - 1, mSubreport.Column, mSubreport.Length, mSubreportLine)
                    mCurrentline += 1
                Next
            Next

        Next
        Dim mListLines As New List(Of String)
        mLastLine = 1
        For Each mKey As Integer In mLines.Keys
            If mLastLine < mKey Then
                mLastLine = mKey
            End If
        Next
        For i As Integer = 1 To mLastLine
            mListLines.Add(mLines(i))
        Next
        Return mListLines
    End Function
End Class
