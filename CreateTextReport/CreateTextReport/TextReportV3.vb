
Public Class TextReportV3
    Public Property ReportHeader As New TextReportSectionCollectionV3
    Public Property ReportFooter As New TextReportSectionCollectionV3
    Public Property PageHeader As New TextReportSectionCollectionV3
    Public Property PageFooter As New TextReportSectionCollectionV3
    Public Property Groups As New TextReportGroupCollectionV3
    Public Property DetailSection As New TextReportSectionCollectionV3
    Public Property PageSize As Integer = 80
    Public Property ReportSummaryFields As New SummaryFieldCollectionV3
    Public Property PageSummaryFields As New SummaryFieldCollectionV3
    Public Property FormulaFields As New FormulaFieldCollectionV3

    Dim mPageLineCount As Integer
    Dim mPageCount As Integer = 1
    Public ReadOnly Property PageNumber As Integer
        Get
            Return mPageCount
        End Get
    End Property
    Public ReadOnly Property LineNumber As Integer
        Get
            Return mPageLineCount
        End Get
    End Property
    Public Function WriteToFile(pFilename As String, pDT As DataTable) As String
        Dim msw As New IO.StreamWriter(pFilename, True)
        Dim mText As String = GetReportText(pDT)
        msw.Write(mText)
        msw.Close()
        Return ""
    End Function
    Public Function GetReportText(pDT As DataTable) As String
        Dim mOutput As String = ""
        Dim mLines = GetReportLines(pDT)
        mLines.ForEach(Sub(line) mOutput += (line & vbCrLf))
        Return mOutput
    End Function
    Public Function GetReportLines(pDT As DataTable) As List(Of String)

        mReportLines = New List(Of String)
            Dim mLines As List(Of String)
            Dim mDataRow As DataRow = pDT.Rows(0)
            mLines = ReportHeader.GetLines(pDT.Rows(0))
            AddReportLines(mLines, mDataRow)
            mLines = PageHeader.GetLines(pDT.Rows(0))
            AddReportLines(mLines, mDataRow)

            Dim mRowCount As Integer = pDT.Rows.Count
            Dim mGroupCount As Integer = Groups.Count
            Dim mChangingGroupIndex As Integer = 0 ' this is important
            Dim mGroupChanged As Boolean = True
            ReportSummaryFields.Initialize()
            PageSummaryFields.Initialize()

            For i As Integer = 0 To mRowCount - 1
                mDataRow = pDT.Rows(i)
                If i > 0 Then
                    mChangingGroupIndex = Groups.BreakingGroupIndex(mDataRow)
                    If mChangingGroupIndex <> -1 Then
                        mGroupChanged = True
                        Dim mPreviousDataRow = pDT.Rows(i - 1)
                        For grpcounter As Integer = mGroupCount - 1 To mChangingGroupIndex Step -1
                            Dim mGrp As TextReportGroupV3 = Groups(grpcounter)
                            Dim mGroupLines = mGrp.Footers.GetLines(mPreviousDataRow)
                            AddReportLines(mGroupLines, mPreviousDataRow)
                        Next
                    End If
                End If
                If i = 0 Or mGroupChanged Then
                    For grpCounter As Integer = mChangingGroupIndex To mGroupCount - 1
                        Dim mGrp As TextReportGroupV3 = Groups(grpCounter)
                        mGrp.Initialize()
                        Dim mGroupLines = mGrp.Headers.GetLines(mDataRow)
                        AddReportLines(mGroupLines, mDataRow)
                    Next
                End If
                mGroupChanged = False
                Groups.SetGroupValue(mDataRow)
                FormulaFields.SetDataRow(mDataRow)
                ReportSummaryFields.UpdateSummary(mDataRow)
                PageSummaryFields.UpdateSummary(mDataRow)
                mLines = DetailSection.GetLines(mDataRow)
                AddReportLines(mLines, mDataRow)
            Next
            mDataRow = pDT.Rows(mRowCount - 1)
            For grpcounter As Integer = mGroupCount - 1 To 0 Step -1
                Dim mGrp As TextReportGroupV3 = Groups(grpcounter)
                Dim mGroupLines = mGrp.Footers.GetLines(mDataRow)
                AddReportLines(mGroupLines, mDataRow)
            Next
            mLines = ReportFooter.GetLines(mDataRow)
            AddReportLines(mLines, mDataRow)
            WritePageFooter(mDataRow)
            Return mReportLines


    End Function
    Dim mReportLines As New List(Of String)
    Private Sub AddReportLines(pLines As List(Of String), pRow As DataRow)
        If mPageLineCount = PageSize Then
            mPageCount = mPageCount + 1
            mPageLineCount = 0
            Dim mPHLines = PageHeader.GetLines(pRow)
            For Each mLine In mPHLines
                'pSW.WriteLine(mLine)
                mReportLines.Add(mLine)
                mPageLineCount += 1
            Next
        End If
        For Each mLine In pLines
            Dim mPFLines1 = PageFooter.GetLines(pRow)
            If mPageLineCount + mPFLines1.Count >= PageSize Then
                For Each mPFLine In mPFLines1
                    mReportLines.Add(mPFLine)
                    'pSW.WriteLine(mPFLine)
                Next
                mPageLineCount = 0
                mPageCount = mPageCount + 1
                Dim mPHLines = PageHeader.GetLines(pRow)
                PageSummaryFields.Initialize()
                For Each mPHLine In mPHLines
                    mReportLines.Add(mPHLine)
                    'pSW.WriteLine(mPHLine)
                    mPageLineCount += 1
                Next
            End If
            mReportLines.Add(mLine)
            'pSW.WriteLine(mLine)
            mPageLineCount += 1
        Next
    End Sub
    Public Sub WritePageFooter(pRow As DataRow)
        If mPageLineCount = 0 Then Exit Sub
        Dim mPFLines = PageFooter.GetLines(pRow)
        If mPFLines.Count = 0 Then Return
        Dim mPFStartLine As Integer = PageSize - mPFLines.Count + 1
        For i = mPageLineCount To mPFStartLine - 1
            mReportLines.Add("")
        Next
        For Each mPFLine In mPFLines
            mReportLines.Add(mPFLine)
        Next
        mPageLineCount = 0
    End Sub
End Class
