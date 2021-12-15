Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'TestTextReport1()
        TestReportV3()
        '    TestFieldsV3()

    End Sub
    Public Sub TestReportV3()
        Dim mTRV3 = New TestReportV3
        mTRV3.TestReportV3()
    End Sub
    Public Sub TestReportV2()
        Dim mTRV2 = New TestReportV2
        mTRV2.Test()
    End Sub
    Public Sub TestFieldsV3()
        Dim tf = New TestFields
        tf.TestStaticField()

    End Sub
    Public Sub TestSectionV3()
        Dim tf = New TestReportSection
        tf.TestReportSectionV3()

    End Sub
    Public Sub TestTextReport()
        Dim mDashline = "-".PadLeft(70, "-")
        Dim mReport As New TextReport
        mReport.LineCount = 15
        mReport.SetText(1, 10, 30, "This is a test report")
        mReport.SetText(2, 1, 10, "2019-01-24")
        mReport.SetText(2, 30, 10, "Page 1")
        mReport.SetText(4, 1, 70, mDashline)
        mReport.SetText(5, 1, 10, "Sr.No")
        mReport.SetText(5, 11, 10, "Itemcode")
        mReport.SetText(5, 21, 40, "Description")
        mReport.SetText(5, 61, 10, "Quantity")
        mReport.SetText(6, 1, 70, mDashline)
        mReport.SetText(7, 1, 10, "1")
        mReport.SetText(7, 11, 10, "Item1")
        mReport.SetText(7, 21, 40, "Item Description 1")
        mReport.SetText(7, 61, 10, "10")
        mReport.SetText(8, 1, 10, "2")
        mReport.SetText(8, 11, 10, "Item2")
        mReport.SetText(8, 21, 40, "Item Description 2")
        mReport.SetText(8, 61, 10, "20")
        mReport.SetText(13, 61, 70, "-".PadRight(10, "-"))
        mReport.SetText(14, 61, 10, "30")
        mReport.SetText(15, 1, 70, mDashline)
        mReport.SaveToFile("D:\Development\Saravana\TestReport1.txt")
    End Sub
    Public Sub TestTextReport1()
        Dim rf = Function(p1 As Integer, p2 As Integer, p3 As String)
                     Return New ReportField(p1, p2, p3)
                 End Function

        Dim mDashline = "-".PadLeft(70, "-")
        Dim mReport As New TextReport
        mReport.LineCount = 15

        mReport.SetText(1, 10, 30, "This is a test report")

        mReport.AddFields(2, rf(1, 10, "2019-01-24"), rf(30, 10, "Page 1"))

        mReport.SetText(4, 1, 70, mDashline)

        mReport.AddFields(5, rf(1, 10, "Sr.No"),
                             rf(11, 10, "Itemcode"),
                             rf(21, 40, "Description"),
                             rf(61, 10, "Quantity"))

        mReport.SetText(6, 1, 70, mDashline)

        mReport.AddFields(7, rf(1, 10, "1"),
                             rf(11, 10, "Item1"),
                             rf(21, 40, "Item Description 1"),
                             rf(61, 10, "10"))
        mReport.AddFields(8, rf(1, 10, "2"),
                             rf(11, 10, "Item2"),
                             rf(21, 40, "Item Description 2"),
                             rf(61, 10, "20"))

        mReport.SetText(13, 61, 70, "-".PadRight(10, "-"))
        mReport.SetText(14, 61, 10, "30")
        mReport.SetText(15, 1, 70, mDashline)

        mReport.SaveToFile("D:\Development\Saravana\TestReport1.txt")

    End Sub
    Public Sub TestTextReport2()
        Dim rf = Function(p1 As Integer, p2 As Integer, p3 As String)
                     Return New ReportField(p1, p2, p3)
                 End Function
        Dim mReport As New TextReport
        Dim mComp As New SAPbobsCOM.Company
        Dim rs As SAPbobsCOM.Recordset = mComp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        rs.DoQuery("")
        Dim mn As New RFHelper(rs)

        mReport.AddFields(4, mn.rf(5, 12, "CardCode"),
                             mn.rf(5, 12, "Phone1"),
                             mn.rf(5, 12, "DocNum"))
        mReport.AddFields(5, mn.rf(5, 12, "Address1"),
                             mn.rf(5, 12, "Phone1"),
                             mn.rf(5, 12, "DocNum"))

        Dim mDetailRS As SAPbobsCOM.Recordset = mComp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        mDetailRS.DoQuery("")
        Dim dtl As New RFHelper(mDetailRS)

        Dim mRowCount As Integer = mDetailRS.RecordCount
        Dim mDetailRowStart As Integer = 10
        For i As Integer = 0 To mRowCount - 1
            mReport.AddFields(mDetailRowStart + i, dtl.rf(1, 10, ""))
        Next

        mReport.AddFields(4, mn.rf(1, 1, ""))

    End Sub
    Public Class RFHelper
        Public Property RS As SAPbobsCOM.Recordset
        Public Sub New(pRS)
            RS = pRS
        End Sub
        Function rf(p1 As Integer, p2 As Integer, p3 As String)
            Return New ReportField(p1, p2, RS.Fields.Item(p3).Value.ToString)
        End Function

    End Class
End Class
