Public Class TestReportSection
    Public Function TestReportSectionV3() As Boolean
        ' TO DO: Multiple Groups
        ' Multiple sub Sections for a section
        ' Running Total
        ' Page Break
        ' Page number, date, Total pages variables
        ' Formatting data
        ' Formulae
        ' Delegates for formula
        ' Hide empty sections
        ' Sub reports
        ' Excel formatting
        Dim mPHSection As TextReportSectionV3 = GetPageHeader()
        Dim mDtlSection As TextReportSectionV3 = GetDetailSection()
        Dim mGroupSummaryInvoiceTotal As New SummaryFieldV3("InvoiceValue", 10, 59, enumFieldAlignment.Right, enumTRSummaryType.Sum)
        Dim mGroupSummaryBalanceTotal As New SummaryFieldV3("BalanceValue", 10, 69, enumFieldAlignment.Right, enumTRSummaryType.Sum)
        Dim mReportSummaryInvoiceTotal As New SummaryFieldV3("InvoiceValue", 10, 59, enumFieldAlignment.Right, enumTRSummaryType.Sum)
        Dim mReportSummaryBalanceTotal As New SummaryFieldV3("BalanceValue", 10, 69, enumFieldAlignment.Right, enumTRSummaryType.Sum)
        Dim mGrpSummaryList As New List(Of SummaryFieldV3)
        mGrpSummaryList.AddRange(New SummaryFieldV3() {
                        mGroupSummaryInvoiceTotal, mGroupSummaryBalanceTotal
                        })
        Dim mReportSummaryList As New List(Of SummaryFieldV3)
        mReportSummaryList.AddRange(New SummaryFieldV3() {
                                     mReportSummaryInvoiceTotal, mReportSummaryBalanceTotal}
                                     )
        Dim mGrp As New TextReportGroupV3(New DBFieldV3("Group", 100, 1))
        mGrp.Headers.Add(GetGroupSectionHeader)
        mGrp.Footers.Add(GetGroupSectionFooter(mGroupSummaryInvoiceTotal, mGroupSummaryBalanceTotal))
        Dim mTestData As DataTable = GetTestDT()
        Dim mRow As DataRow = mTestData.Rows(0)
        Dim mFileName = GetFileName()
        Dim mLines = mPHSection.GetLines(mRow)
        WriteToFile(mLines, mFileName)
        Dim mRowCount As Integer = mTestData.Rows.Count
        Dim mGroupChanged As Boolean = True
        For Each mSummart In mGrpSummaryList
            mSummart.Initialize()
        Next
        For Each mSummart In mReportSummaryList
            mSummart.Initialize()
        Next
        For i As Integer = 0 To mRowCount - 1
            If i > 0 Then
                If mGrp.IsGroupChanging(mTestData.Rows(i)) Then
                    mLines = mGrp.GetFooterLines(mRow)
                    WriteToFile(mLines, mFileName)
                    mGroupChanged = True
                    For Each mSummart In mGrpSummaryList
                        mSummart.Initialize()
                    Next
                End If
            End If
            mRow = mTestData.Rows(i)
            For Each mSummart In mGrpSummaryList
                mSummart.CalculateSummary(mRow)
            Next
            For Each mSummart In mReportSummaryList
                mSummart.CalculateSummary(mRow)
            Next

            If mGroupChanged Then
                mLines = mGrp.GetHeaderLines(mRow)
                WriteToFile(mLines, mFileName)
                mGrp.StoreValue(mRow)
                mGroupChanged = False
            End If
            mLines = mDtlSection.GetLines(mRow)
            WriteToFile(mLines, mFileName)
        Next
        mRow = mTestData.Rows(mRowCount - 1)
        mLines = mGrp.GetFooterLines(mRow)
        WriteToFile(mLines, mFileName)
        Dim mReportFooter = GetReportFooter(mReportSummaryInvoiceTotal, mReportSummaryBalanceTotal)
        mLines = mReportFooter.GetLines(mRow)
        WriteToFile(mLines, mFileName)
        Diagnostics.Process.Start(mFileName)
        Return True
    End Function
    Private Sub WriteToFile(pLines As List(Of String), pFileName As String)
        Dim mSW As IO.StreamWriter = New IO.StreamWriter(pFileName, True)
        For Each mLine In pLines
            mSW.WriteLine(mLine)
        Next
        mSW.Close()
    End Sub
    Private Function GetFileName() As String
        Return IO.Path.Combine(IO.Path.GetTempPath, IO.Path.GetTempFileName + ".txt")
    End Function
    Private Function GetGroupSectionHeader() As TextReportSectionV3
        Dim mTRS As New TextReportSectionV3
        mTRS.AddFields(1, New TextReportFieldV3() {
                    New StaticTextFieldV3("Group Number: ", 25, 1),
                    New DBFieldV3("Group", 10, 26)
                       })
        Return mTRS
    End Function
    Private Function GetGroupSectionFooter(f1 As SummaryFieldV3, f2 As SummaryFieldV3) As TextReportSectionV3
        Dim mTRS As New TextReportSectionV3
        mTRS.AddFields(2, New TextReportFieldV3() {
                    New StaticTextFieldV3("Group Number: ", 25, 1),
                    New DBFieldV3("Group", 10, 26),
                    New StaticTextFieldV3("Completed", 10, 37)
                       })
        mTRS.AddFields(1, New TextReportFieldV3() {
            New StaticTextFieldV3("".PadLeft(80, "-"), 80, 1)
                       })
        mTRS.AddFields(2, New TextReportFieldV3() {
            f1, f2
                       })
        mTRS.AddFields(3, New TextReportFieldV3() {
            New StaticTextFieldV3("".PadLeft(80, "-"), 80, 1)
                       })
        Return mTRS

    End Function
    Private Function GetReportFooter(f1 As SummaryFieldV3, f2 As SummaryFieldV3) As TextReportSectionV3
        Dim mTRS As New TextReportSectionV3
        mTRS.AddFields(2, New TextReportFieldV3() {
                    New StaticTextFieldV3("Report Footer ", 25, 1),
                    New StaticTextFieldV3("Total", 10, 37)
                       })
        mTRS.AddFields(1, New TextReportFieldV3() {
            New StaticTextFieldV3("".PadLeft(80, "-"), 80, 1)
                       })
        mTRS.AddFields(2, New TextReportFieldV3() {
            f1, f2
                       })
        mTRS.AddFields(3, New TextReportFieldV3() {
            New StaticTextFieldV3("".PadLeft(80, "-"), 80, 1)
                       })
        Return mTRS
    End Function
    Private Function GetDetailSection() As TextReportSectionV3
        Dim mTRS As New TextReportSectionV3
        mTRS.AddFields(1, New TextReportFieldV3() {
                        New DBFieldV3("InvoiceNumber", 12, 1),
                        New DBFieldV3("InvoiceDate", 14, 13),
                        New DBFieldV3("CustomerName", 30, 28),
                        New DBFieldV3("InvoiceValue", 10, 59, enumFieldAlignment.Right),
                        New DBFieldV3("BalanceValue", 10, 70, enumFieldAlignment.Right)
                       })
        Return mTRS
    End Function

    Private Function GetPageHeader() As TextReportSectionV3
        Dim mTRS As New TextReportSectionV3
        Dim mDashLine As String = "".PadLeft(80, "-")

        mTRS.AddFields(1, New TextReportFieldV3() {
                New StaticTextFieldV3("Accenture Services Private Limited", 80, 1, enumFieldAlignment.Center)
                       })
        mTRS.AddFields(2, New TextReportFieldV3() {
                New StaticTextFieldV3("Bangalore", 80, 1, enumFieldAlignment.Center)
                       })
        mTRS.AddFields(3, New TextReportFieldV3() {
                New StaticTextFieldV3("Date " + Now.ToString("dd/MM/yyyy"), 30, 1),
                New StaticTextFieldV3("Page 1", 10, 71, enumFieldAlignment.Right)
                       })
        mTRS.AddFields(4, New TextReportFieldV3() {
                        New StaticTextFieldV3(mDashLine, 80, 1)
                       })
        mTRS.AddFields(5, New TextReportFieldV3() {
                        New StaticTextFieldV3("Invoice No", 12, 1),
                        New StaticTextFieldV3("Invoice Date", 14, 13),
                        New StaticTextFieldV3("Customer", 30, 28),
                        New StaticTextFieldV3("Value", 10, 59, enumFieldAlignment.Right),
                        New StaticTextFieldV3("Balance", 10, 70, enumFieldAlignment.Right)
                       })
        mTRS.AddFields(6, New TextReportFieldV3() {
                        New StaticTextFieldV3(mDashLine, 80, 1)
                       })
        Return mTRS
    End Function
    Private Function GetTestDT() As DataTable
        Dim mDT As New System.Data.DataTable
        mDT.Columns.Add("Group", "".GetType)
        mDT.Columns.Add("CustomerName", "".GetType)
        mDT.Columns.Add("InvoiceNumber", "".GetType)
        mDT.Columns.Add("InvoiceValue", "".GetType)
        mDT.Columns.Add("InvoiceDate", "".GetType)
        mDT.Columns.Add("BalanceValue", "".GetType)
        For i = 1 To 40
            mDT.Rows.Add(i.ToString, "Candy Wines Limited with more than 30 characters", 1000 + ((i - 1) * 4), "20000", "2018-04-01", "18000")
            mDT.Rows.Add(i.ToString, "Candy", 1001 + ((i - 1) * 4), "14000", "2018-04-01", "8000")
            mDT.Rows.Add(i.ToString, "Satyam", 1002 + ((i - 1) * 4), "18000", "2018-04-01", "10000")
            mDT.Rows.Add(i.ToString, "Satyam Pharmaceuticals Private Limited", 1003 + ((i - 1) * 4), "25000", "2018-04-01", "20000")
        Next
        Return mDT
    End Function
End Class
