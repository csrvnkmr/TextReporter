Public Class TestReport
    Public Function TestReport() As Boolean

        Dim mGroupSummaryInvoiceTotal As New SummaryField("InvoiceValue", 10, 59, enumFieldAlignment.Right, enumTRSummaryType.Sum)
        Dim mGroupSummaryBalanceTotal As New SummaryField("BalanceValue", 10, 69, enumFieldAlignment.Right, enumTRSummaryType.Sum)
        Dim mReportSummaryInvoiceTotal As New SummaryField("InvoiceValue", 10, 59, enumFieldAlignment.Right, enumTRSummaryType.Sum)
        Dim mReportSummaryBalanceTotal As New SummaryField("BalanceValue", 10, 69, enumFieldAlignment.Right, enumTRSummaryType.Sum)
        Dim mTopGroupSummaryInvoiceTotal As New SummaryField("InvoiceValue", 10, 59, enumFieldAlignment.Right, enumTRSummaryType.Sum)
        Dim mTopGroupSummaryBalanceTotal As New SummaryField("BalanceValue", 10, 69, enumFieldAlignment.Right, enumTRSummaryType.Sum)
        Dim mPageSummaryInvoiceTotal As New SummaryField("InvoiceValue", 10, 59, enumFieldAlignment.Right, enumTRSummaryType.Sum)
        Dim mPageSummaryBalanceTotal As New SummaryField("BalanceValue", 10, 69, enumFieldAlignment.Right, enumTRSummaryType.Sum)

        Dim mRPT As New TextReport

        mRPT.PageSummaryFields.Add(mPageSummaryBalanceTotal)
        mRPT.PageSummaryFields.Add(mPageSummaryInvoiceTotal)

        Dim mPageNumberField As New PageNumberField(mRPT, 5, 70)
        mRPT.PageHeader.AddRange(GetPageHeader(mPageNumberField))
        mRPT.PageFooter.AddRange(GetPageFooter(mPageSummaryInvoiceTotal, mPageSummaryBalanceTotal))


        'mRPT.ReportHeader.AddRange(getr)
        mRPT.ReportSummaryFields.Add(mReportSummaryBalanceTotal)
        mRPT.ReportSummaryFields.Add(mReportSummaryInvoiceTotal)
        mRPT.ReportFooter.AddRange(GetReportFooter(mReportSummaryInvoiceTotal, mReportSummaryBalanceTotal))

        Dim mGroupRunningTotalInvoiceTotal As New SummaryField("InvoiceValue", 10, 81, enumFieldAlignment.Right, enumTRSummaryType.Sum)
        Dim mFormulaPaidAmount As New FormulaField(mRPT, AddressOf CalculatePaidAmount, 10, 81)
        mRPT.FormulaFields.Add(mFormulaPaidAmount)
        mRPT.DetailSection.AddRange(GetDetailSection(mFormulaPaidAmount))

        Dim mtopGrp As New TextReportGroup(New DBField("TopGroup", 100, 1))
        mtopGrp.Headers.AddRange(GetTopGroupSectionHeader)
        mtopGrp.Footers.AddRange(GetTopGroupSectionFooter(mTopGroupSummaryInvoiceTotal, mTopGroupSummaryBalanceTotal))
        mtopGrp.SummaryFields.Add(mTopGroupSummaryInvoiceTotal)
        mtopGrp.SummaryFields.Add(mTopGroupSummaryBalanceTotal)

        Dim mGrp As New TextReportGroup(New DBField("Group", 100, 1))
        mGrp.Headers.AddRange(GetGroupSectionHeader)
        mGrp.Footers.AddRange(GetGroupSectionFooter(mGroupSummaryInvoiceTotal, mGroupSummaryBalanceTotal, mRPT))
        mGrp.SummaryFields.Add(mGroupSummaryInvoiceTotal)
        mGrp.SummaryFields.Add(mGroupSummaryBalanceTotal)
        mGrp.SummaryFields.Add(mGroupRunningTotalInvoiceTotal)

        mRPT.Groups.Add(mtopGrp)
        mRPT.Groups.Add(mGrp)

        Dim mTestData As DataTable = GetTestDT()
        Dim mFileName = GetFileName()
        mRPT.WriteToFile(mFileName, mTestData)
        Diagnostics.Process.Start(mFileName)
        Return True
    End Function

    Public Function CalculatePaidAmount(pRpt As TextReport, pDataRow As DataRow) As Object
        If pDataRow IsNot Nothing Then
            Return pDataRow("InvoiceValue") - pDataRow("BalanceValue")
        End If
        Return ""
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

    Private Function GetTopGroupSectionHeader() As TextReportSectionCollection
        Dim mTRSColl As New TextReportSectionCollection
        Dim mTRS As New TextReportSection
        mTRS.AddFields(1, New TextReportField() {
                    New StaticTextField("TopGroup Number: ", 25, 1),
                    New DBField("TopGroup", 10, 26)
                       })
        mTRSColl.Add(mTRS)
        Return mTRSColl
    End Function

    Private Function GetTopGroupSectionFooter(f1 As TextReportField, f2 As TextReportField) As TextReportSectionCollection
        Dim mTRSColl As New TextReportSectionCollection
        Dim mTRS As New TextReportSection
        mTRS.AddFields(1, New TextReportField() {
            New StaticTextField("".PadLeft(80, "-"), 80, 1)
                       })
        mTRS.AddFields(2, New TextReportField() {
                    New StaticTextField("TopGroup Number: ", 25, 1),
                    New DBField("TopGroup", 10, 26),
                    New StaticTextField("Completed", 10, 37)
                       })
        mTRS.AddFields(2, New TextReportField() {
            f1, f2
                       })
        mTRS.AddFields(3, New TextReportField() {
            New StaticTextField("".PadLeft(80, "-"), 80, 1)
                       })
        mTRSColl.Add(mTRS)
        Return mTRSColl

    End Function

    Private Function GetPageFooter(f1 As TextReportField, f2 As TextReportField) As TextReportSectionCollection
        Dim mTRSColl As New TextReportSectionCollection
        Dim mTRS As New TextReportSection
        mTRS.AddFields(1, New TextReportField() {
            New StaticTextField("".PadLeft(80, "-"), 80, 1)
                       })
        mTRS.AddFields(2, New TextReportField() {
            f1, f2
                       })
        mTRS.AddFields(3, New TextReportField() {
                    New StaticTextField("Page ..", 80, 1, enumFieldAlignment.Right)
                       })
        mTRSColl.Add(mTRS)
        Return mTRSColl

    End Function

    Private Function GetGroupSectionHeader() As TextReportSectionCollection
        Dim mTRSColl As New TextReportSectionCollection
        Dim mTRS1 As New TextReportSection
        mTRS1.AddFields(1, New TextReportField() {
                    New StaticTextField("Group Number: ", 25, 1),
                    New DBField("Group", 10, 26)
                       })
        mTRSColl.Add(mTRS1)
        Return mTRSColl
    End Function

    Private Function GetGroupSectionFooter(f1 As SummaryField, f2 As SummaryField,
                                           pRpt As TextReport) As TextReportSectionCollection
        Dim mTRSColl As New TextReportSectionCollection
        Dim mTRS1 As New TextReportSection
        mTRS1.AddFields(2, New TextReportField() {
                    New StaticTextField("Group Number: ", 25, 1),
                    New DBField("Group", 10, 26),
                    New StaticTextField("Completed", 10, 37)
                       })
        mTRS1.AddFields(1, New TextReportField() {
            New StaticTextField("".PadLeft(80, "-"), 80, 1)
                       })
        mTRS1.AddFields(2, New TextReportField() {
            f1, f2})

        Dim msubrpt = GetSubReport(pRpt)
        Dim msubrptlist As New List(Of SubReport)
        msubrptlist.Add(msubrpt)
        mTRS1.Subreports.Add(3, msubrptlist)

        Dim mTRS2 As New TextReportSection
        mTRS2.AddFields(1, New TextReportField() {
            New StaticTextField("".PadLeft(80, "-"), 80, 1)
        })
        mTRSColl.Add(mTRS1)
        mTRSColl.Add(mTRS2)
        Return mTRSColl

    End Function

    Private Function GetReportFooter(f1 As SummaryField, f2 As SummaryField) As TextReportSectionCollection
        Dim mTRS As New TextReportSection
        mTRS.AddFields(2, New TextReportField() {
                    New StaticTextField("Report Footer ", 25, 1),
                    New StaticTextField("Total", 10, 37)
                       })
        mTRS.AddFields(1, New TextReportField() {
            New StaticTextField("".PadLeft(80, "-"), 80, 1)
                       })
        mTRS.AddFields(2, New TextReportField() {
            f1, f2
                       })
        mTRS.AddFields(3, New TextReportField() {
            New StaticTextField("".PadLeft(80, "-"), 80, 1)
                       })
        Dim mtrscoll As New TextReportSectionCollection
        mtrscoll.Add(mTRS)
        Return mtrscoll
    End Function

    Private Function GetDetailSection(f1 As TextReportField) As TextReportSectionCollection
        Dim mTRS As New TextReportSection
        mTRS.AddFields(1, New TextReportField() {
                        New DBField("InvoiceNumber", 12, 1),
                        New DBField("InvoiceDate", 14, 13),
                        New DBField("CustomerName", 30, 28),
                        New DBField("InvoiceValue", 10, 59, enumFieldAlignment.Right),
                        New DBField("BalanceValue", 10, 70, enumFieldAlignment.Right),
                        f1
                       })
        Dim mtrscoll As New TextReportSectionCollection
        mtrscoll.Add(mTRS)
        Return mtrscoll
    End Function

    Private Function GetPageHeader(f1 As TextReportField) As TextReportSectionCollection
        Dim mTRS As New TextReportSection
        Dim mDashLine As String = "".PadLeft(80, "-")

        mTRS.AddFields(1, New TextReportField() {
                New StaticTextField("Accenture Services Private Limited", 80, 1, enumFieldAlignment.Center)
                       })
        mTRS.AddFields(2, New TextReportField() {
                New StaticTextField("Bangalore", 80, 1, enumFieldAlignment.Center)
                       })
        f1.Column = 77
        mTRS.AddFields(3, New TextReportField() {
                New StaticTextField("Date " + Now.ToString("dd/MM/yyyy"), 30, 1),
                New StaticTextField("Page ", 5, 71, enumFieldAlignment.Right),
                f1
                       })
        mTRS.AddFields(4, New TextReportField() {
                        New StaticTextField(mDashLine, 80, 1)
                       })
        mTRS.AddFields(5, New TextReportField() {
                        New StaticTextField("Invoice No", 12, 1),
                        New StaticTextField("Invoice Date", 14, 13),
                        New StaticTextField("Customer", 30, 28),
                        New StaticTextField("Value", 10, 59, enumFieldAlignment.Right),
                        New StaticTextField("Balance", 10, 70, enumFieldAlignment.Right)
                       })
        mTRS.AddFields(6, New TextReportField() {
                        New StaticTextField(mDashLine, 80, 1)
                       })
        Dim mTRSColl As New TextReportSectionCollection
        mTRSColl.Add(mTRS)
        Return mTRSColl
    End Function

    Private Function GetTestDT() As DataTable
        Dim mDT As New System.Data.DataTable
        mDT.Columns.Add("TopGroup", "".GetType)
        mDT.Columns.Add("Group", "".GetType)
        mDT.Columns.Add("CustomerName", "".GetType)
        mDT.Columns.Add("InvoiceNumber", "".GetType)
        mDT.Columns.Add("InvoiceValue", "".GetType)
        mDT.Columns.Add("InvoiceDate", "".GetType)
        mDT.Columns.Add("BalanceValue", "".GetType)
        For i = 1 To 40
            Dim t1 As Integer = Math.Ceiling(i / 10)
            'mDT.Rows.Add(t1.ToString, i.ToString, "Candy Wines Limited with more than 30 characters", 1000 + ((i - 1) * 4), "20000", "2018-04-01", "18000")
            mDT.Rows.Add(t1.ToString, i.ToString, "Candy Wines Limited with more than 30 characters", 1000 + ((i - 1) * 4), "20000", "2018-04-01", "18000")
            mDT.Rows.Add(t1.ToString, i.ToString, "Candy", 1001 + ((i - 1) * 4), "14000", "2018-04-01", "8000")
            mDT.Rows.Add(t1.ToString, i.ToString, "Satyam", 1002 + ((i - 1) * 4), "18000", "2018-04-01", "10000")
            mDT.Rows.Add(t1.ToString, i.ToString, "Satyam Pharmaceuticals Private Limited", 1003 + ((i - 1) * 4), "25000", "2018-04-01", "20000")
        Next
        Return mDT
    End Function

    Private Function GetSubReport(pReport As TextReport) As SubReport
        Dim mRpt As New TextReport
        Dim mReportHeader As New TextReportSection
        mReportHeader.AddFields(1, New TextReportField() {
                              New StaticTextField("This is subreport line 1", 30, 10)})

        mRpt.ReportHeader.Add(mReportHeader)

        Dim mDtlSection As New TextReportSection()
        mDtlSection.AddFields(1, New TextReportField() {
                              New DBField("CustomerName", 30, 1)})
        mDtlSection.AddFields(1, New TextReportField() {
                              New DBField("Month", 10, 31)})
        mDtlSection.AddFields(1, New TextReportField() {
                              New DBField("PreviousBalance", 20, 42)})
        mRpt.DetailSection.Add(mDtlSection)
        Return New SubReport(AddressOf GetSubReportTable, 1, 80, mRpt, pReport)
    End Function

    Private Function GetSubReportTable(pParentReport As TextReport, pSubReport As TextReport,
                                       pDataRow As DataRow) As DataTable
        Dim mDT As New System.Data.DataTable
        mDT.Columns.Add("CustomerName", "".GetType)
        mDT.Columns.Add("Month", "".GetType)
        mDT.Columns.Add("PreviousBalance", "".GetType)
        mDT.Rows.Add(pDataRow("CustomerName"), "Jan", pDataRow("TopGroup") & "/" & pDataRow("Group"))
        mDT.Rows.Add(pDataRow("CustomerName"), "Feb", pDataRow("TopGroup") & "///" & pDataRow("Group"))
        Return mDT
    End Function
End Class
