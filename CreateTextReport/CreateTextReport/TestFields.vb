Public Class TestFields
    Public Sub TestStaticField()
        'Dim m1 As New StaticTextFieldV3("TESTINGFORMULTILINE", 10)
        'm1.Alignment = enumFieldAlignment.Left
        'Debug.Assert(m1.GetValue(1) = "TESTINGFOR")
        'Debug.Assert(m1.GetValue(2) = "MULTILINE ")
        'm1.Alignment = enumFieldAlignment.Right
        'Debug.Assert(m1.GetValue(1) = "TESTINGFOR")
        'Debug.Assert(m1.GetValue(2) = " MULTILINE")
        'm1.Alignment = enumFieldAlignment.Center
        'Debug.Assert(m1.GetValue(1) = "TESTINGFOR")
        'Debug.Assert(m1.GetValue(2) = " MULTILINE")
    End Sub
End Class
