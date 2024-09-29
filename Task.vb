Public MustInherit Class Task
    Inherits IsolatedTaskProxy

    Public Property Name As String
    Public Property LabelText As String
    Public Property HelpText As String
    'Public Property RequiresTemplate As Boolean
    'Public Property RequiresMaterialTable As Boolean
    'Public Property RequiresLaserOutputDirectory As Boolean
    'Public Property RequiresPartNumberFields As Boolean
    Public Property RequiresSave As Boolean
    'Public Property RequiresSaveAsOutputDirectory As Boolean
    'Public Property IncompatibleWithOtherTasks As Boolean
    'Public Property RequiresExternalProgram As Boolean
    'Public Property RequiresSaveAsFlatDXFOutputDirectory As Boolean
    'Public Property RequiresFindReplaceFields As Boolean
    'Public Property RequiresPrinter As Boolean
    'Public Property RequiresPictorialView As Boolean
    'Public Property RequiresForegroundProcessing As Boolean
    'Public Property RequiresVariablesToEdit As Boolean
    'Public Property RequiresOverallSizeVariables As Boolean
    Public Property AppliesToAssembly As Boolean
    Public Property AppliesToPart As Boolean
    Public Property AppliesToSheetmetal As Boolean
    Public Property AppliesToDraft As Boolean
    Public Property HasOptions As Boolean
    Public Property HelpURL As String

    Public MustOverride Function Process(
        SEDoc As SolidEdgeFramework.SolidEdgeDocument,
        Configuration As Dictionary(Of String, String),
        SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

    Public Sub New()

    End Sub

    Public Sub UpdateErrorMessage(
        ExitStatus As Integer,
        ErrorMessageList As List(Of String),
        SupplementalErrorMessage As Dictionary(Of Integer, List(Of String))
        )

        Dim SupplementalExitStatus As Integer = SupplementalErrorMessage.Keys(0)

        If Not SupplementalExitStatus = 0 Then
            If SupplementalExitStatus > ExitStatus Then
                ExitStatus = SupplementalExitStatus
            End If
            For Each s As String In SupplementalErrorMessage(SupplementalExitStatus)
                ErrorMessageList.Add(s)
            Next
        End If
    End Sub

    Public Function GenerateLabelText()
        ' Scratch.TaskOpenSave -> Open save

        Dim InString As String
        Dim OutString As String = ""

        InString = Me.ToString
        InString = InString.Replace("Scratch.Task", "")  ' 'Scratch.TaskOpenSave' -> 'OpenSave'

        OutString = InString(0)  ' '' -> 'O'
        InString = Right(InString, Len(InString) - 1)  ' 'OpenSave' -> 'penSave'

        For Each c As Char In InString
            If (Asc(c) >= 65) And (Asc(c) <= 90) Then  ' It's a capital letter
                ' Upper case.  Add a space and change the character to lower case.
                OutString = String.Format("{0} {1}", OutString, CStr(c).ToLower)
            Else
                ' Lower case.  Add the character as is.
                OutString = String.Format("{0}{1}", OutString, CStr(c))
            End If
        Next

        Return OutString
    End Function

End Class
