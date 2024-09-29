Imports Newtonsoft.Json
Imports System.Text.RegularExpressions

Public Class TaskPropertyFindReplace
    Inherits Task

    Public Property JSONDict As String

    Public Sub New()
        Me.LabelText = GenerateLabelText()
        Me.HelpText = GenerateHelpText()
        Me.RequiresSave = True
        Me.AppliesToAssembly = True
        Me.AppliesToPart = True
        Me.AppliesToSheetmetal = True
        Me.AppliesToDraft = True
        Me.HasOptions = True
        Me.HelpURL = "https://github.com"

        Me.JSONDict = ""
    End Sub

    Public Sub New(Task As TaskPropertyFindReplace)
        Me.LabelText = Task.LabelText
        Me.HelpText = Task.HelpText
        Me.RequiresSave = Task.RequiresSave
        Me.AppliesToAssembly = Task.AppliesToAssembly
        Me.AppliesToPart = Task.AppliesToPart
        Me.AppliesToSheetmetal = Task.AppliesToSheetmetal
        Me.AppliesToDraft = Task.AppliesToDraft
        Me.HasOptions = Task.HasOptions
        Me.HelpURL = Task.HelpURL

        Me.JSONDict = Task.JSONDict

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


        ' Convert glob to regex 
        ' https://stackoverflow.com/questions/74683013/regex-to-glob-and-vice-versa-conversion
        ' https://stackoverflow.com/questions/11276909/how-to-convert-between-a-glob-pattern-and-a-regexp-pattern-in-ruby
        ' https://learn.microsoft.com/en-us/dotnet/visual-basic/language-reference/operators/like-operator

        Dim ErrorMessageList As New List(Of String)
        Dim ExitStatus As Integer = 0
        Dim ErrorMessage As New Dictionary(Of Integer, List(Of String))

        Dim SupplementalExitStatus As Integer
        Dim SupplementalErrorMessage As New Dictionary(Of Integer, List(Of String))

        Dim PropertySets As SolidEdgeFramework.PropertySets = Nothing
        Dim Properties As SolidEdgeFramework.Properties = Nothing
        Dim Prop As SolidEdgeFramework.Property = Nothing

        Dim PropertySetName As String = ""
        Dim PropertyName As String = ""
        Dim FindString As String = ""
        Dim ReplaceString As String = ""
        Dim FindSearchType As String = ""
        Dim ReplaceSearchType As String = ""

        Dim PropertyFound As Boolean = False
        Dim AutoAddMissingProperty As Boolean = Configuration("CheckBoxAutoAddMissingProperty").ToLower = "true"

        Dim Proceed As Boolean = True
        Dim s As String

        Dim PropertiesToEditDict As New Dictionary(Of String, Dictionary(Of String, String))
        Dim PropertiesToEdit As String = ""
        Dim RowIndexString As String

        Dim DocType As String = GetDocType(SEDoc)
        If DocType = "asm" Then PropertiesToEdit = Configuration("TextBoxPropertiesEditAssembly")
        If DocType = "par" Then PropertiesToEdit = Configuration("TextBoxPropertiesEditPart")
        If DocType = "psm" Then PropertiesToEdit = Configuration("TextBoxPropertiesEditSheetmetal")
        If DocType = "dft" Then PropertiesToEdit = Configuration("TextBoxPropertiesEditDraft")

        If Not PropertiesToEdit = "" Then
            PropertiesToEditDict = JsonConvert.DeserializeObject(Of Dictionary(Of String, Dictionary(Of String, String)))(PropertiesToEdit)

            'PropertiesToEditDict format
            '{"1":{
            '    "PropertySet":"System",
            '    "PropertyName":"Material",
            '    "Find_PT":"True",
            '    "Find_WC":"False",
            '    "Find_RX":"False",
            '    "FindString":"Aluminum",
            '    "Replace_PT":"True",
            '    "Replace_RX":"False",
            '    "ReplaceString":"Aluminum 6061-T6"},
            ' ...
            '}

        Else
            ExitStatus = 1
            ErrorMessageList.Add("No properties provided")
        End If

        For Each RowIndexString In PropertiesToEditDict.Keys

            Proceed = True

            PropertyName = PropertiesToEditDict(RowIndexString)("PropertyName")
            PropertySetName = PropertiesToEditDict(RowIndexString)("PropertySet")
            FindString = PropertiesToEditDict(RowIndexString)("FindString")
            ReplaceString = PropertiesToEditDict(RowIndexString)("ReplaceString")

            If PropertiesToEditDict(RowIndexString)("Find_PT").ToLower = "true" Then
                FindSearchType = "PT"
            ElseIf PropertiesToEditDict(RowIndexString)("Find_WC").ToLower = "true" Then
                FindSearchType = "WC"
            Else
                FindSearchType = "RX"
            End If

            If PropertiesToEditDict(RowIndexString)("Replace_PT").ToLower = "true" Then
                ReplaceSearchType = "PT"
            Else
                ReplaceSearchType = "RX"
            End If

            If Proceed Then
                Try
                    FindString = SubstitutePropertyFormula(SEDoc, FindString)
                Catch ex As Exception
                    Proceed = False
                    ExitStatus = 1
                    s = String.Format("Unable to process formula in Find text '{0}' for property '{1}'", FindString, PropertyName)
                    ErrorMessageList.Add(s)
                End Try

                Try
                    ReplaceString = SubstitutePropertyFormula(SEDoc, ReplaceString)
                Catch ex As Exception
                    Proceed = False
                    ExitStatus = 1
                    s = String.Format("Unable to process formula in Replace text '{0}' for property '{1}'", ReplaceString, PropertyName)
                    ErrorMessageList.Add(s)
                End Try
            End If

            If Proceed Then
                Try
                    Prop = GetProp(SEDoc, PropertySetName, PropertyName, 0, AutoAddMissingProperty)
                    If Prop Is Nothing Then
                        Proceed = False
                        ExitStatus = 1
                        s = String.Format("Property '{0}.{1}' not found or not recognized.", PropertySetName, PropertyName)
                        ErrorMessageList.Add(s)
                    End If
                Catch ex As Exception
                    Proceed = False
                    ExitStatus = 1
                    s = String.Format("Property '{0}.{1}' not found or not recognized.", PropertySetName, PropertyName)
                    ErrorMessageList.Add(s)
                End Try

            End If

            If Proceed Then
                Try
                    If FindSearchType = "PT" Then
                        Prop.Value = Replace(CType(Prop.Value, String), FindString, ReplaceString, 1, -1, vbTextCompare)
                    Else
                        If FindSearchType = "WC" Then
                            FindString = CommonTasks.GlobToRegex(FindString)
                        End If
                        If ReplaceSearchType = "PT" Then
                            ' ReplaceString = Regex.Escape(ReplaceString)
                        End If

                        Prop.Value = Regex.Replace(CType(Prop.Value, String), FindString, ReplaceString, RegexOptions.IgnoreCase)

                    End If
                    ' Properties.Save()
                Catch ex As Exception
                    Proceed = False
                    ExitStatus = 1
                    s = String.Format("Unable to replace property value '{0}'.  This command only works on text type properties.", PropertyName)
                    ErrorMessageList.Add(s)
                End Try

            End If

            If Proceed Then
                Try
                    PropertySets = CType(SEDoc.Properties, SolidEdgeFramework.PropertySets)
                    For Each Properties In PropertySets
                        Properties.Save()
                        SEApp.DoIdle()
                    Next
                    If SEDoc.ReadOnly Then
                        ExitStatus = 1
                        s = "Cannot save document marked 'Read Only'"
                        If Not ErrorMessageList.Contains(s) Then
                            ErrorMessageList.Add(s)
                        End If
                    Else
                        SEDoc.Save()
                        SEApp.DoIdle()
                    End If

                Catch ex As Exception
                    Proceed = False
                    ExitStatus = 1
                    s = "Problem accessing or saving Property."
                    If Not ErrorMessageList.Contains(s) Then
                        ErrorMessageList.Add(s)
                    End If
                End Try
            End If

            ' If the changed property was System.Material, need to update properties from the material table.
            If Proceed Then
                PropertyName = PropertiesToEditDict(RowIndexString)("PropertyName")
                PropertySetName = PropertiesToEditDict(RowIndexString)("PropertySet")

                If (PropertyName.ToLower = "material") And (PropertySetName.ToLower = "system") Then

                    If CBool(Configuration("CheckBoxAutoUpdateMaterialProperties")) Then

                        If DocType = "par" Then
                            Dim MaterialDoctorPart As New MaterialDoctorPart()

                            SupplementalErrorMessage = MaterialDoctorPart.UpdateMaterialFromMaterialTable(SEDoc, Configuration, SEApp)
                            SupplementalExitStatus = SupplementalErrorMessage.Keys(0)
                            If ExitStatus = 0 Then
                                ExitStatus = SupplementalExitStatus
                                If Not ExitStatus = 0 Then
                                    ErrorMessageList.Add("Problem updating material properties")
                                    For Each s In SupplementalErrorMessage(SupplementalErrorMessage.Keys(0))
                                        ErrorMessageList.Add(s)
                                    Next
                                End If
                            End If
                        End If

                        If DocType = "psm" Then
                            Dim MaterialDoctorSheetmetal As New MaterialDoctorSheetmetal()

                            SupplementalErrorMessage = MaterialDoctorSheetmetal.UpdateMaterialFromMaterialTable(SEDoc, Configuration, SEApp)
                            SupplementalExitStatus = SupplementalErrorMessage.Keys(0)
                            If ExitStatus = 0 Then
                                ExitStatus = SupplementalExitStatus
                                If Not ExitStatus = 0 Then
                                    ErrorMessageList.Add("Problem updating material properties")
                                    For Each s In SupplementalErrorMessage(SupplementalErrorMessage.Keys(0))
                                        ErrorMessageList.Add(s)
                                    Next
                                End If
                            End If
                        End If

                    End If
                End If
            End If

        Next

        ErrorMessage(ExitStatus) = ErrorMessageList
        Return ErrorMessage
    End Function


    Private Function GenerateHelpText()
        Dim HelpString As String

        HelpString = "Searches for text in a specified property and replaces it if found. "
        HelpString += "The property, search text, and replacement text are entered on the Input Editor. "
        HelpString += "Activate the editor using the `Property find/replace` `Edit` button on the **Task Tab** below the task list. "
        HelpString += ""
        HelpString += vbCrLf + vbCrLf + "![Find_Replace](My%20Project/media/property_input_editor.png)"
        HelpString += ""
        HelpString += vbCrLf + vbCrLf + "A `Property set`, either `System` or `Custom`, is required. "
        HelpString += "For more information, see the **Property Filter** section above. "
        HelpString += vbCrLf + vbCrLf + "There are three search modes, `PT`, `WC`, and `RX`. "
        HelpString += vbCrLf + vbCrLf + "- `PT` stands for 'Plain Text'.  It is simple to use, but finds literal matches only. "
        HelpString += vbCrLf + "- `WC` stands for 'Wild Card'.  You use `*`, `?`  `[charlist]`, and `[!charlist]` according to the VB Like syntax. "
        HelpString += vbCrLf + "- `RX` stands for 'Regex'.  It is a more comprehensive (and notoriously cryptic) method of matching text. "
        HelpString += "Check the [<ins>**.NET Regex Guide**</ins>](https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference) "
        HelpString += "for more information."
        HelpString += vbCrLf + vbCrLf + "The search *is not* case sensitive, the replacement *is*. "
        HelpString += "For example, say the search is `aluminum`, "
        HelpString += "the replacement is `ALUMINUM`, "
        HelpString += "and the property value is `Aluminum 6061-T6`. "
        HelpString += "Then the new value would be `ALUMINUM 6061-T6`. "
        ' HelpString += vbCrLf + vbCrLf + "![Property Formula](My%20Project/media/property_formula.png)"
        HelpString += vbCrLf + vbCrLf + "In addition to plain text and pattern matching, you can also use "
        HelpString += "a property formula.  The formula has the same syntax as the Callout command, "
        HelpString += "except preceeded with `System.` or `Custom.` as shown in the Input Editor above. "
        HelpString += vbCrLf + vbCrLf + "If the specified property does not exist in the file, "
        HelpString += "you can optionally have it added automatically. "
        HelpString += "This option is set on the **Configuration Tab -- General Page**. "
        HelpString += "Note, this only works for `Custom` properties.  Adding `System` properties is not allowed. "
        HelpString += vbCrLf + vbCrLf + "If you are changing `System.Material` specifically, there is an option "
        HelpString += "to automatically update the part's material properties (density, face styles, etc.). "
        HelpString += "Set the option on the **Configuration Tab -- General Page**. "
        HelpString += vbCrLf + vbCrLf + "The properties are processed in the order in the table. "
        HelpString += "You can change the order by selecting a row and using the Up/Down buttons "
        HelpString += "at the top of the form.  Only one row can be moved at a time. "
        HelpString += "The delete button, also at the top of the form, removes selected rows. "
        HelpString += vbCrLf + vbCrLf + "You can copy the settings on the form to other tabs. "
        HelpString += "Set the `Copy To` CheckBoxes as desired."
        HelpString += vbCrLf + vbCrLf + "Note the textbox adjacent to the `Edit` button "
        HelpString += "is a `Dictionary` representation of the table settings in `JSON` format. "
        HelpString += "You can edit it if you want, but the form is probably easier to use. "

        Return HelpString
    End Function

End Class
