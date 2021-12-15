Public Class TextReport
    Public Property LineCount As String = 80
    Dim mLines As New Dictionary(Of Integer, String)
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
    Public Function SaveToFile(pFileName As String) As String
        Dim mFile As New IO.StreamWriter(pFileName)
        For i As Integer = 1 To LineCount
            If mLines.ContainsKey(i) Then
                mFile.WriteLine(mLines(i))
            Else
                mFile.WriteLine("")
            End If
        Next
        mFile.Close()
        Return ""
    End Function
    Public Function GetReportData() As String()
        Dim mOutputLines(LineCount) As String
        For i As Integer = 1 To LineCount
            If mLines.ContainsKey(i) Then
                mOutputLines(i - 1) = mLines(i)
            Else
                mOutputLines(i - 1) = ""
            End If
        Next
        Return mOutputLines

    End Function
    Public Sub AddFields(pRow As Integer, ParamArray pFields() As ReportField)
        For Each pField In pFields
            SetText(pRow, pField.Column, pField.Length, pField.Text)
        Next
    End Sub

End Class


Public Class ReportField
    Private Sub New()

    End Sub
    Public Sub New(pColumn As Integer, pLength As Integer, pText As String)
        Column = pColumn
        Length = pLength
        Text = pText
    End Sub
    Public Property Column As Integer
    Public Property Length As Integer
    Dim _text As String
    Public Property Text() As String
        Get
            Return GetValue()
        End Get
        Set(value As String)
            _text = value
        End Set
    End Property
    Private Function GetValue() As String
        Dim mReturnValue As String = _text
        If mReturnValue Is Nothing Then
            mReturnValue = ""
        End If
        If Len(mReturnValue) < Length Then
            mReturnValue = mReturnValue.PadRight(Length)
        ElseIf Len(mReturnValue) > Length Then
            mReturnValue = Left(mReturnValue, Length)
        End If
        Return mReturnValue
    End Function
End Class
