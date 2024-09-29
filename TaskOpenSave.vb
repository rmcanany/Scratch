Public Class TaskOpenSave
    Inherits Task

    Public Sub New()
        Me.Name = "TaskOpenSave"
        Me.LabelText = GenerateLabelText()
        Me.HelpText = GenerateHelpText()
        Me.RequiresSave = True
        Me.AppliesToAssembly = True
        Me.AppliesToPart = True
        Me.AppliesToSheetmetal = True
        Me.AppliesToDraft = True
        Me.HasOptions = False
        Me.HelpURL = "https://github.com"
    End Sub

    Public Sub New(Task As TaskOpenSave)
        Me.Name = Task.Name
        Me.LabelText = Task.LabelText
        Me.HelpText = Task.HelpText
        Me.RequiresSave = Task.RequiresSave
        Me.AppliesToAssembly = Task.AppliesToAssembly
        Me.AppliesToPart = Task.AppliesToPart
        Me.AppliesToSheetmetal = Task.AppliesToSheetmetal
        Me.AppliesToDraft = Task.AppliesToDraft
        Me.HasOptions = Task.HasOptions
        Me.HelpURL = Task.HelpURL
    End Sub

    Public Overrides Function Process(
        ByVal SEDoc As SolidEdgeFramework.SolidEdgeDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        ErrorMessage = InvokeSTAThread(
                               Of SolidEdgeFramework.SolidEdgeDocument,
                               Dictionary(Of String, String),
                               SolidEdgeFramework.Application,
                               Dictionary(Of Integer, List(Of String)))(
                                   AddressOf ProcessInternal,
                                   SEDoc,
                                   Configuration,
                                   SEApp)

        Return ErrorMessage

    End Function

    Private Function ProcessInternal(
        ByVal SEDoc As SolidEdgeFramework.SolidEdgeDocument,
        ByVal Configuration As Dictionary(Of String, String),
        ByVal SEApp As SolidEdgeFramework.Application
        ) As Dictionary(Of Integer, List(Of String))

        Dim ErrorMessageList As New List(Of String)
        Dim ExitStatus As Integer = 0
        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        If SEDoc.ReadOnly Then
            ExitStatus = 1
            ErrorMessageList.Add("Cannot save document marked 'Read Only'")
        Else
            SEDoc.Save()
            SEApp.DoIdle()
        End If

        ErrorMessage(ExitStatus) = ErrorMessageList
        Return ErrorMessage
    End Function

    Private Function GenerateHelpText()
        Dim HelpString As String
        HelpString = "Open a document and save in the current version."

        Return HelpString
    End Function

End Class
