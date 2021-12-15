Public Class TestReportV2
    Public Function Test()
        Dim mDT As New System.Data.DataTable
        mDT.Columns.Add("CompName", "".GetType)
        mDT.Columns.Add("CustomerName", "".GetType)
        mDT.Columns.Add("InvoiceNumber", "".GetType)
        mDT.Columns.Add("InvoiceValue", "".GetType)
        mDT.Columns.Add("InvoiceDate", "".GetType)
        mDT.Columns.Add("BalanceValue", "".GetType)
        For i = 1 To 40
            mDT.Rows.Add("OSPL", "Candy", 1000 + ((i - 1) * 4), "20000", "2018-04-01", "18000")
            mDT.Rows.Add("OSPL", "Candy", 1001 + ((i - 1) * 4), "14000", "2018-04-01", "8000")
            mDT.Rows.Add("OSPL", "Satyam", 1002 + ((i - 1) * 4), "18000", "2018-04-01", "10000")
            mDT.Rows.Add("OSPL", "Satyam", 1003 + ((i - 1) * 4), "25000", "2018-04-01", "20000")
        Next
        Dim mTestRpt As New TextReportV2
        mTestRpt.PageHeader.AddFields(1, New ReportFieldV2(40, 40, "OCTOPUS SYSTEMS PVT LTD"))
        mTestRpt.PageHeader.AddFields(2, New ReportFieldV2(40, 30, "CUSTOMER BALANCE LEDGER"))
        mTestRpt.PageHeader.AddFields(2, New ReportFieldV2(80, 10, Now.ToString("dd-MM-yyyy")))
        mTestRpt.PageHeader.AddFields(3, New ReportFieldV2(1, 100, "-".PadLeft(100, "-")))
        mTestRpt.PageHeader.AddFields(4, New ReportFieldV2(1, 40, "Customer Name"))
        mTestRpt.PageHeader.AddFields(4, New ReportFieldV2(41, 15, "Invoice No."))
        mTestRpt.PageHeader.AddFields(4, New ReportFieldV2(56, 15, "Invoice Date"))
        mTestRpt.PageHeader.AddFields(4, New ReportFieldV2(71, 15, "Invoice Value"))
        mTestRpt.PageHeader.AddFields(4, New ReportFieldV2(86, 15, "Balance Value"))
        mTestRpt.PageHeader.AddFields(5, New ReportFieldV2(1, 100, "-".PadLeft(100, "-")))

        mTestRpt.DetailSection.AddFields(1, New ReportFieldV2(1, 40, "CustomerName", True))
        mTestRpt.DetailSection.AddFields(1, New ReportFieldV2(41, 15, "InvoiceNumber", True))
        mTestRpt.DetailSection.AddFields(1, New ReportFieldV2(56, 15, "InvoiceDate", True))
        mTestRpt.DetailSection.AddFields(1, New ReportFieldV2(71, 15, "InvoiceValue", True))
        mTestRpt.DetailSection.AddFields(1, New ReportFieldV2(86, 15, "BalanceValue", True))
        Dim mLinesList = mTestRpt.GenerateReport(mDT)
        Dim mFileName As String = IO.Path.Combine(IO.Path.GetTempPath, IO.Path.GetTempFileName + ".txt")
        Dim mFile As New IO.StreamWriter(mFileName)
        For Each mLine In mLinesList
            mFile.WriteLine(mLine)
        Next
        mFile.Close()
        Process.Start(mFileName)
        Return ""
    End Function
End Class
Public Class TextReportV2
    Dim _pageHeader As TextReportSection
    Public ReadOnly Property PageHeader As TextReportSection
        Get
            If _pageHeader Is Nothing Then
                _pageHeader = New TextReportSection
            End If
            Return _pageHeader
        End Get
    End Property
    Dim _detailSection As TextReportSection
    Public ReadOnly Property DetailSection As TextReportSection
        Get
            If _detailSection Is Nothing Then
                _detailSection = New TextReportSection
            End If
            Return _detailSection
        End Get
    End Property


    Public Property PageSize As Integer = 80
    Public Function GenerateReport(pDT As DataTable) As List(Of String)
        Dim mDT As System.Data.DataTable = pDT
        Dim mPageLineCount As Integer = 0
        Dim mLines As New List(Of String)
        mLines.AddRange(PageHeader.GetReportData(pDT.Rows(0)))
        mPageLineCount = mLines.Count
        Dim mPageCount As Integer = 1
        For Each mRow As DataRow In mDT.Rows
            mPageLineCount = mLines.Count Mod PageSize
            Dim mSectionLines As String() = DetailSection.GetReportData(mRow)
            If mSectionLines.Length + mPageLineCount >= PageSize Then
                For i = mPageLineCount + 1 To PageSize
                    mLines.Add("")
                Next
                mLines.AddRange(PageHeader.GetReportData(mRow))
            End If
            mLines.AddRange(mSectionLines)
        Next
        Return mLines
    End Function
End Class

Public Class TextReportSection
    Public Property LineCount As String = 80
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
    Public Function GetReportData(pRow As DataRow) As String()
        mLines = New Dictionary(Of Integer, String)
        Dim mLastRow As Integer = 0
        For Each mKey As Int16 In SectionFields.Keys
            If mKey > mLastRow Then
                mLastRow = mKey
            End If
        Next
        For i As Integer = 1 To mLastRow
            If SectionFields.ContainsKey(i) Then
                For Each fld In SectionFields(i)
                    If fld.FieldType = ReportFieldType.Text Then
                        SetText(i, fld.Column, fld.Length, fld.Text)
                    Else
                        SetText(i, fld.Column, fld.Length, fld.GetColumnValue(pRow))
                    End If

                Next
            End If
        Next
        Dim mOutputLines(mLastRow - 1) As String
        For i = 0 To mLastRow - 1
            If mLines.ContainsKey(i + 1) Then
                mOutputLines(i) = mLines(i + 1)
            Else
                mOutputLines(i) = ""
            End If
        Next
        Return mOutputLines
    End Function
    Public Property SectionFields As New Dictionary(Of Integer, List(Of ReportFieldV2))
    Public Sub AddFields(pRow As Integer, ParamArray pFields() As ReportFieldV2)
        If SectionFields.ContainsKey(pRow) Then
        Else
            SectionFields.Add(pRow, New List(Of ReportFieldV2))
        End If
        For Each pField In pFields
            SectionFields(pRow).Add(pField)
        Next
    End Sub
End Class

Public Enum ReportFieldType
    Text = 0
    DB = 1
End Enum

Public Class ReportFieldV2
    Dim _reportFieldType As ReportFieldType = ReportFieldType.Text
    Private Sub New()

    End Sub
    Public Sub New(pColumn As Integer, pLength As Integer, pText As String)
        Column = pColumn
        Length = pLength
        Text = pText
    End Sub
    Public Sub New(pColumn As Integer, pLength As Integer, pText As String, pIsDBColumn As Boolean)
        Column = pColumn
        Length = pLength
        Text = pText
        _reportFieldType = ReportFieldType.DB
    End Sub
    Public Property Column As Integer
    Public Property Length As Integer
    Public ReadOnly Property FieldType As ReportFieldType
        Get
            Return _reportFieldType
        End Get
    End Property

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
    Public Function GetColumnValue(pRow As System.Data.DataRow) As String
        Dim mReturnValue As String = pRow(_text).ToString
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

