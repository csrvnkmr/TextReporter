Public Enum enumFieldType
    StaticText = 1
    DataField = 2
    FormulaField = 3
    SummaryField = 4
    NotDefined = -99
End Enum
Public Enum enumFieldAlignment
    Left = 1
    Right = 2
    Center = 3
End Enum
Public Enum enumFieldDataType
    Number = 1
    [Date] = 2
    [String] = 3
End Enum
Public MustInherit Class TextReportField
    Public Property Name As String
    Public Property Length As Integer
    Public Property Column As Integer
    Public Property FieldType As enumFieldType = enumFieldType.NotDefined
    Public Property Alignment As enumFieldAlignment = enumFieldAlignment.Left
    Public Property NoOfLines As Integer = 1
    Public Property DataType As enumFieldDataType = enumFieldDataType.String
    Public Sub New(pLength As Integer, pColumn As Integer)
        Length = pLength
        Column = pColumn
    End Sub
    Public Function GetAlignedValue(pValue As String, pLength As Integer, pAlignment As enumFieldAlignment) As String
        If pValue.Length >= pLength Then
            Return pValue
        End If
        If pAlignment = enumFieldAlignment.Left Then
            Return pValue.PadRight(pLength)
        End If
        If pAlignment = enumFieldAlignment.Right Then
            Return pValue.PadLeft(pLength)
        End If
        If pAlignment = enumFieldAlignment.Center Then
            Dim mCurrentLength As Integer = pValue.Length
            Dim mDiff As Integer = pLength - mCurrentLength
            Dim mSpacesLeft = Math.Round(mDiff / 2, MidpointRounding.AwayFromZero)
            Dim mSpacesRight = mDiff - mSpacesLeft
            Return Space(mSpacesLeft) + pValue + Space(mSpacesRight)
        End If
        Return pValue
    End Function
    Public Function GetNthLineValue(pValue As String, Optional pLinenumber As Integer = 1)
        Dim mValue As String = pValue
        If pLinenumber > 1 Then
            Dim mCharsToExclude As Integer = (pLinenumber - 1) * Length
            If mValue.Length <= mCharsToExclude Then
                Return ""
            End If
            mValue = mValue.Substring(mCharsToExclude)
        End If
        Return Left(mValue, Length)
    End Function
    Public Function GetValue(Optional pLinenumber As Integer = 1) As String
        Dim mValue As String = GetDataValue().Trim
        mValue = GetNthLineValue(mValue, pLinenumber)
        If mValue = "" Then Return ""
        Return GetAlignedValue(mValue, Length, Alignment)
    End Function
    Public MustOverride Function GetDataValue() As String
End Class
