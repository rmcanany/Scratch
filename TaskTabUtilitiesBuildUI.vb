Option Strict On
Public Class TaskTabUtilitiesBuildUI

    Private Tasks As New LabelToAction
    'Private ControlNames As New List(Of String)
    Private ControlsDict As New Dictionary(Of String, Control)
    Private TLPTasks As TableLayoutPanel

    Public Sub New(TLPTasks As TableLayoutPanel)
        Me.TLPTasks = TLPTasks
    End Sub


    Public Sub BuildTLPTasks()
        Dim ColumnNames As New List(Of String)

        ColumnNames.Add("Expand")
        ColumnNames.Add("Select")
        ColumnNames.Add("Assembly")
        ColumnNames.Add("Part")
        ColumnNames.Add("Sheetmetal")
        ColumnNames.Add("Draft")
        ColumnNames.Add("Task")
        ColumnNames.Add("Help")

        TLPTasks.SuspendLayout()

        BuildPanel(ColumnNames)
        BuildHeaderRow(ColumnNames)
        PopulateControls(ColumnNames)

        PopulateControlsDict()

        TLPTasks.ResumeLayout()


    End Sub

    Private Sub BuildPanel(ColumnNames As List(Of String))

        Dim RowIndex As Integer
        Dim ColumnIndex As Integer
        Dim ColumnWidth As Integer = 30
        Dim RowHeight As Integer = 30

        TLPTasks.RowCount = Tasks.Keys.Count
        TLPTasks.ColumnCount = ColumnNames.Count

        For RowIndex = 0 To TLPTasks.RowCount - 1
            ' Rows 0 and 1 already present in TLPTasks
            If RowIndex = 0 Then
                TLPTasks.RowStyles(RowIndex).SizeType = SizeType.Absolute
                TLPTasks.RowStyles(RowIndex).Height = RowHeight
            ElseIf RowIndex = 1 Then
                TLPTasks.RowStyles(RowIndex).SizeType = SizeType.AutoSize
            Else
                TLPTasks.RowStyles.Add(New RowStyle(SizeType.AutoSize))
            End If
        Next

        For ColumnIndex = 0 To TLPTasks.ColumnCount - 1
            ' Columns 0 and 1 already present in TLPTasks
            If ColumnIndex = 0 Then
                TLPTasks.ColumnStyles(ColumnIndex).SizeType = SizeType.Absolute
                TLPTasks.ColumnStyles(ColumnIndex).Width = ColumnWidth
            ElseIf ColumnIndex = 1 Then
                TLPTasks.ColumnStyles(ColumnIndex).SizeType = SizeType.Absolute
                TLPTasks.ColumnStyles(ColumnIndex).Width = CInt(ColumnWidth)
            ElseIf ColumnIndex = TLPTasks.ColumnCount - 1 Then
                TLPTasks.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, ColumnWidth))
            Else
                TLPTasks.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
            End If
        Next

    End Sub

    Private Sub BuildHeaderRow(ColumnNames As List(Of String))
        Dim RowIndex As Integer
        Dim ColumnIndex As Integer
        Dim ColumnName As String
        'Dim TaskName As String
        Dim ColumnWidth As Integer = 30
        Dim RowHeight As Integer = 30

        Dim Button As Button
        Dim Label As Label
        'Dim CheckBox As CheckBox
        'Dim TLP As TableLayoutPanel

        TLPTasks.RowCount = Tasks.Keys.Count
        TLPTasks.ColumnCount = ColumnNames.Count

        RowIndex = 0
        ColumnIndex = 0
        For Each ColumnName In ColumnNames
            Select Case ColumnName

                Case "Expand"
                    Button = New Button
                    Button.Name = String.Format("{0}_Button_{1}", TLPTasks.Name, ColumnName)
                    FormatButton(Button)
                    Button.ImageList = Form_Main.ImageList1
                    Button.ImageKey = "expand.png"
                    Button.Image = Form_Main.ImageList1.Images(Button.ImageKey)
                    TLPTasks.Controls.Add(Button, ColumnIndex, RowIndex)

                Case "Select"
                    Button = New Button
                    Button.Name = String.Format("{0}_Button_{1}", TLPTasks.Name, ColumnName)
                    FormatButton(Button)
                    Button.Image = Form_Main.ImageList1.Images("icons8_Checked_Checkbox_16.png")
                    TLPTasks.Controls.Add(Button, ColumnIndex, RowIndex)

                Case "Assembly"
                    Button = New Button
                    Button.Name = String.Format("{0}_Button_{1}", TLPTasks.Name, ColumnName)
                    FormatButton(Button)
                    Button.Image = Form_Main.ImageList1.Images("ST9 - asm.png")
                    TLPTasks.Controls.Add(Button, ColumnIndex, RowIndex)

                Case "Part"
                    Button = New Button
                    Button.Name = String.Format("{0}_Button_{1}", TLPTasks.Name, ColumnName)
                    FormatButton(Button)
                    Button.Image = Form_Main.ImageList1.Images("ST9 - par.png")
                    TLPTasks.Controls.Add(Button, ColumnIndex, RowIndex)

                Case "Sheetmetal"
                    Button = New Button
                    Button.Name = String.Format("{0}_Button_{1}", TLPTasks.Name, ColumnName)
                    FormatButton(Button)
                    Button.Image = Form_Main.ImageList1.Images("ST9 - psm.png")
                    TLPTasks.Controls.Add(Button, ColumnIndex, RowIndex)

                Case "Draft"
                    Button = New Button
                    Button.Name = String.Format("{0}_Button_{1}", TLPTasks.Name, ColumnName)
                    FormatButton(Button)
                    Button.Image = Form_Main.ImageList1.Images("ST9 - dft.png")
                    TLPTasks.Controls.Add(Button, ColumnIndex, RowIndex)

                Case "Task"
                    TLPTasks.ColumnStyles(ColumnIndex).SizeType = SizeType.Percent
                    TLPTasks.ColumnStyles(ColumnIndex).Width = 100.0F
                    Label = New Label
                    Label.Name = String.Format("{0}_Label_{1}", TLPTasks.Name, ColumnName)
                    Label.Text = ColumnName.ToUpper
                    Label.Anchor = AnchorStyles.None
                    TLPTasks.Controls.Add(Label, ColumnIndex, RowIndex)

                Case "Help"
                    Button = New Button
                    Button.Name = String.Format("{0}_Button_{1}", TLPTasks.Name, ColumnName)
                    FormatButton(Button)
                    Button.Image = Form_Main.ImageList1.Images("icons8_help_16.png")
                    'Button.Image = Form_Main.ImageList1.Images("ST9 - dft.png")
                    TLPTasks.Controls.Add(Button, ColumnIndex, RowIndex)

            End Select

            ColumnIndex += 1
        Next

    End Sub

    Private Sub PopulateControls(ColumnNames As List(Of String))
        Dim RowIndex As Integer
        Dim ColumnIndex As Integer
        Dim ColumnName As String
        Dim TaskName As String

        Dim Button As Button
        Dim Label As Label
        Dim CheckBox As CheckBox
        Dim TLP As TableLayoutPanel

        TLPTasks.RowCount = Tasks.Keys.Count
        TLPTasks.ColumnCount = ColumnNames.Count

        ' Populate controls
        RowIndex = 1
        For Each TaskName In Tasks.Keys
            ColumnIndex = 0
            For Each ColumnName In ColumnNames
                Select Case ColumnName

                    Case "Expand"
                        If Tasks(TaskName).HasOptions Then
                            Button = New Button
                            Button.Name = String.Format("{0}_Button_{1}", TaskName, ColumnName)
                            FormatButton(Button)
                            Button.FlatStyle = FlatStyle.Standard
                            Button.FlatAppearance.BorderSize = 1
                            Button.ImageList = Form_Main.ImageList1
                            Button.ImageKey = "expand.png"
                            Button.Image = Form_Main.ImageList1.Images(Button.ImageKey)
                            TLPTasks.Controls.Add(Button, ColumnIndex, RowIndex)

                        End If

                    Case "Select"
                        CheckBox = New CheckBox
                        CheckBox.Name = String.Format("{0}_CheckBox_{1}", TaskName, ColumnName)
                        FormatCheckBox(CheckBox)
                        TLPTasks.Controls.Add(CheckBox, ColumnIndex, RowIndex)

                    Case "Assembly"
                        If Tasks(TaskName).AppliesToAssembly Then
                            CheckBox = New CheckBox
                            CheckBox.Name = String.Format("{0}_CheckBox_{1}", TaskName, ColumnName)
                            FormatCheckBox(CheckBox)
                            TLPTasks.Controls.Add(CheckBox, ColumnIndex, RowIndex)
                        End If

                    Case "Part"
                        If Tasks(TaskName).AppliesToPart Then
                            CheckBox = New CheckBox
                            CheckBox.Name = String.Format("{0}_CheckBox_{1}", TaskName, ColumnName)
                            FormatCheckBox(CheckBox)
                            TLPTasks.Controls.Add(CheckBox, ColumnIndex, RowIndex)
                        End If

                    Case "Sheetmetal"
                        If Tasks(TaskName).AppliesToSheetmetal Then
                            CheckBox = New CheckBox
                            CheckBox.Name = String.Format("{0}_CheckBox_{1}", TaskName, ColumnName)
                            FormatCheckBox(CheckBox)
                            TLPTasks.Controls.Add(CheckBox, ColumnIndex, RowIndex)
                        End If

                    Case "Draft"
                        If Tasks(TaskName).AppliesToDraft Then
                            CheckBox = New CheckBox
                            CheckBox.Name = String.Format("{0}_CheckBox_{1}", TaskName, ColumnName)
                            FormatCheckBox(CheckBox)
                            TLPTasks.Controls.Add(CheckBox, ColumnIndex, RowIndex)
                        End If

                    Case "Task"
                        ' This TLP is the container for the task info.
                        ' The first row holds the task description.
                        ' The second row holds the options, if any, in their own TLP.

                        TLP = New TableLayoutPanel()
                        TLP.Name = String.Format("{0}_TLP_{1}", TaskName, ColumnName)
                        TLP.RowCount = 2
                        TLP.ColumnCount = 1
                        TLP.Anchor = CType(AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Top + AnchorStyles.Bottom, AnchorStyles)
                        TLP.AutoSize = True

                        TLP.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
                        TLP.RowStyles.Add(New RowStyle(SizeType.AutoSize))
                        TLP.RowStyles.Add(New RowStyle(SizeType.AutoSize))

                        Label = New Label
                        Label.Name = String.Format("{0}_Label_{1}", TaskName, ColumnName)
                        Label.Text = String.Format("       {0}", Tasks(TaskName).LabelText)
                        Label.ImageList = Form_Main.ImageList1
                        Label.ImageKey = String.Format("{0}.png", TaskName)
                        Label.ImageAlign = ContentAlignment.MiddleLeft
                        Label.Image = Label.ImageList.Images(Label.ImageKey)
                        Label.Anchor = AnchorStyles.Left
                        Label.AutoSize = True

                        TLP.Controls.Add(Label, 0, 0)
                        TLP.SetColumnSpan(Label, 2)

                        TLPTasks.Controls.Add(TLP, ColumnIndex, RowIndex)

                        If Tasks(TaskName).HasOptions Then
                            Dim TLPOptions As New TableLayoutPanel
                            TLPOptions = BuildOptionsTLP(TaskName)

                            TLPOptions.Visible = False

                            TLP.Controls.Add(TLPOptions, 0, 1)
                        End If

                    Case "Help"
                        Button = New Button
                        Button.Name = String.Format("{0}_Button_{1}", TaskName, ColumnName)
                        FormatButton(Button)
                        Button.Image = Form_Main.ImageList1.Images("icons8_help_16.png")
                        TLPTasks.Controls.Add(Button, ColumnIndex, RowIndex)

                End Select

                ColumnIndex += 1
            Next

            RowIndex += 1
        Next

    End Sub

    Private Function BuildOptionsTLP(TaskName As String) As TableLayoutPanel
        Dim TLP As New TableLayoutPanel
        Dim Button As Button
        Dim TextBox As TextBox
        Dim CheckBox As CheckBox
        Dim Label As Label
        Dim RowIndex As Integer
        Dim ComboBox As ComboBox
        Dim ManualOptionsOnlyString As String = "Only show options manually. Use [+] to show."

        Select Case TaskName

            Case "PropertyFindReplace"
                FormatOptionsTLP(TLP, String.Format("{0}_TLP_Options", TaskName), 4)

                RowIndex = 0

                Button = FormatOptionsButton(String.Format("{0}_Button_Edit", TaskName), "Edit")
                TLP.Controls.Add(Button, 0, RowIndex)

                TextBox = FormatOptionsTextBox(String.Format("{0}_TextBox_JSON", TaskName), "")
                TextBox.BackColor = Color.FromArgb(255, 240, 240, 240)
                TLP.Controls.Add(TextBox, 1, RowIndex)

                RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_AutoAddMissingProperty", TaskName),
                                                 "Add the property to the file if not found")
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

                RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_AutoUpdateMaterialProperties", TaskName),
                                                 "Update System.Material density, face style, etc")
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

                RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_ShowHideOptions", TaskName),
                                                 ManualOptionsOnlyString)
                CheckBox.Checked = True
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

                'TLP.AutoSize = True

            Case "VariablesEdit"
                FormatOptionsTLP(TLP, String.Format("{0}_TLP_Options", TaskName), 3)

                RowIndex = 0

                Button = FormatOptionsButton(String.Format("{0}_Button_Edit", TaskName), "Edit")
                TLP.Controls.Add(Button, 0, RowIndex)

                TextBox = FormatOptionsTextBox(String.Format("{0}_TextBox_JSON", TaskName), "")
                TextBox.BackColor = Color.FromArgb(255, 240, 240, 240)
                TLP.Controls.Add(TextBox, 1, RowIndex)

                RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_AutoAddMissingVariable", TaskName),
                                                 "Add the variable to the file if not found")
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

                RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_ShowHideOptions", TaskName),
                                                 ManualOptionsOnlyString)
                CheckBox.Checked = True
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

            Case "UpdatePhysicalProperties"
                FormatOptionsTLP(TLP, String.Format("{0}_TLP_Options", TaskName), 3)

                RowIndex = 0

                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_ShowCenterOfMass", TaskName),
                                                 "Show center of mass")
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

                RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_HideCenterOfMass", TaskName),
                                                 "Hide center of mass")
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

                RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_ShowHideOptions", TaskName),
                                                 ManualOptionsOnlyString)
                CheckBox.Checked = True
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

            Case "CheckInterference"
                FormatOptionsTLP(TLP, String.Format("{0}_TLP_Options", TaskName), 2)

                RowIndex = 0

                TextBox = FormatOptionsTextBox(String.Format("{0}_TextBox_MaxPartsToProcess", TaskName), "")
                TextBox.Dock = DockStyle.None
                TextBox.Width = 50
                TLP.Controls.Add(TextBox, 0, RowIndex)


                Label = FormatOptionsLabel(String.Format("{0}_Label_MaxPartsToProcess", TaskName),
                                           "Maximum number of parts to process")
                TLP.Controls.Add(Label, 1, RowIndex)
                'TLP.SetColumnSpan(Label, 2)


                RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_ShowHideOptions", TaskName),
                                                 ManualOptionsOnlyString)
                CheckBox.Checked = True
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

            Case "SaveAs"
                FormatOptionsTLP(TLP, String.Format("{0}_TLP_Options", TaskName), 3)

                RowIndex = 0

                Button = FormatOptionsButton(String.Format("{0}_Button_Edit", TaskName), "Edit")
                TLP.Controls.Add(Button, 0, RowIndex)

                TextBox = FormatOptionsTextBox(String.Format("{0}_TextBox_JSON", TaskName), "")
                TextBox.BackColor = Color.FromArgb(255, 240, 240, 240)
                TLP.Controls.Add(TextBox, 1, RowIndex)

                RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_CropImageToModelSize", TaskName),
                                                 "Save as image -- Crop to model size")
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

                RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_ShowHideOptions", TaskName),
                                                 ManualOptionsOnlyString)
                CheckBox.Checked = True
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

            Case "Print"
                FormatOptionsTLP(TLP, String.Format("{0}_TLP_Options", TaskName), 2)

                RowIndex = 0

                Button = FormatOptionsButton(String.Format("{0}_Button_Edit", TaskName), "Edit")
                TLP.Controls.Add(Button, 0, RowIndex)

                TextBox = FormatOptionsTextBox(String.Format("{0}_TextBox_JSON", TaskName), "")
                TextBox.BackColor = Color.FromArgb(255, 240, 240, 240)
                TLP.Controls.Add(TextBox, 1, RowIndex)

                RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_ShowHideOptions", TaskName),
                                                 ManualOptionsOnlyString)
                CheckBox.Checked = True
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

            Case "CopyOverallSizeToVariableTable"
                FormatOptionsTLP(TLP, String.Format("{0}_TLP_Options", TaskName), 9)

                Dim CheckBoxNames As New List(Of String)
                CheckBoxNames = "XYZ,MinMidMax".Split(","c).ToList

                Dim VariableNames As New List(Of String)
                VariableNames = "X,Y,Z,Min,Mid,Max".Split(","c).ToList

                RowIndex = 0

                For i As Integer = 0 To CheckBoxNames.Count - 1
                    CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_{1}", TaskName, CheckBoxNames(i)),
                                                 String.Format("Use {0}", CheckBoxNames(i)))
                    TLP.Controls.Add(CheckBox, 0, RowIndex)
                    TLP.SetColumnSpan(CheckBox, 2)

                    RowIndex += 1

                    For j As Integer = 0 To 2
                        Label = FormatOptionsLabel(String.Format("{0}_Label_Variable{1}", TaskName, VariableNames(3 * i + j)),
                                           String.Format("Variable {0}", VariableNames(3 * i + j)))
                        TLP.Controls.Add(Label, 0, RowIndex)

                        TextBox = FormatOptionsTextBox(String.Format("{0}_TextBox_Variable{1}", TaskName, VariableNames(3 * i + j)), "")
                        TLP.Controls.Add(TextBox, 1, RowIndex)

                        RowIndex += 1
                    Next

                Next

                RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_ShowHideOptions", TaskName),
                                                 ManualOptionsOnlyString)
                CheckBox.Checked = True
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

            Case "FitPictorialView"
                FormatOptionsTLP(TLP, String.Format("{0}_TLP_Options", TaskName), 4)

                RowIndex = 0

                Dim ViewNames As New List(Of String)
                ViewNames = "Isometric,Dimetric,Trimetric".Split(","c).ToList

                For i = 0 To ViewNames.Count - 1
                    CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_{1}", TaskName, ViewNames(i)),
                                                 String.Format("{0} view", ViewNames(i)))
                    TLP.Controls.Add(CheckBox, 0, RowIndex)
                    TLP.SetColumnSpan(CheckBox, 2)

                    RowIndex += 1
                Next

                'RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_ShowHideOptions", TaskName),
                                                 ManualOptionsOnlyString)
                CheckBox.Checked = True
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

            Case "PartNumberDoesNotMatchFilename"
                FormatOptionsTLP(TLP, String.Format("{0}_TLP_Options", TaskName), 3)

                RowIndex = 0

                Label = FormatOptionsLabel(String.Format("{0}_Label_Instructions", TaskName),
                                           "Set the property that holds the part number")
                TLP.Controls.Add(Label, 0, RowIndex)
                TLP.SetColumnSpan(Label, 2)

                RowIndex += 1
                Dim ItemTextList As New List(Of String)
                ItemTextList = {"System", "Custom"}.ToList
                ComboBox = FormatOptionsComboBox(String.Format("{0}_ComboBox_PropertySet", TaskName),
                                                 ItemTextList)
                TLP.Controls.Add(ComboBox, 0, RowIndex)

                TextBox = FormatOptionsTextBox(String.Format("{0}_TextBox_PropertyName", TaskName), "")
                TLP.Controls.Add(TextBox, 1, RowIndex)

                RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_ShowHideOptions", TaskName),
                                                 ManualOptionsOnlyString)
                CheckBox.Checked = True
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

                TLP.AutoSize = True

            Case "RunExternalProgram"
                FormatOptionsTLP(TLP, String.Format("{0}_TLP_Options", TaskName), 3)

                RowIndex = 0

                Button = FormatOptionsButton(String.Format("{0}_Button_Browse", TaskName), "Browse")
                TLP.Controls.Add(Button, 0, RowIndex)

                TextBox = FormatOptionsTextBox(String.Format("{0}_TextBox_ExternalProgram", TaskName), "")
                TextBox.BackColor = Color.FromArgb(255, 240, 240, 240)
                TLP.Controls.Add(TextBox, 1, RowIndex)

                RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_SaveAfterProcessing", TaskName),
                                                 "Save file after processing")
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

                RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_ShowHideOptions", TaskName),
                                                 ManualOptionsOnlyString)
                CheckBox.Checked = True
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

            Case "UpdateMaterialFromMaterialTable"
                FormatOptionsTLP(TLP, String.Format("{0}_TLP_Options", TaskName), 2)

                RowIndex = 0

                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_RemoveFaceStyleOverrides", TaskName),
                                                 "Remove face style overrides")
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

                RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_ShowHideOptions", TaskName),
                                                 ManualOptionsOnlyString)
                CheckBox.Checked = True
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

                TLP.AutoSize = True

            Case "UpdateInsertPartCopies"
                FormatOptionsTLP(TLP, String.Format("{0}_TLP_Options", TaskName), 2)

                RowIndex = 0

                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_RecursiveSearch", TaskName),
                                                 "Open all parent files and update")
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                TLP.SetColumnSpan(CheckBox, 2)

                RowIndex += 1
                CheckBox = FormatOptionsCheckBox(String.Format("{0}_CheckBox_ShowHideOptions", TaskName),
                                                 ManualOptionsOnlyString)
                TLP.Controls.Add(CheckBox, 0, RowIndex)
                CheckBox.Checked = True
                TLP.SetColumnSpan(CheckBox, 2)

                TLP.AutoSize = True

            Case Else
                MsgBox(String.Format("Options not implemented for '{0}'", TaskName))

        End Select

        Return TLP
    End Function

    Private Function FormatOptionsComboBox(ControlName As String,
                                           ItemTextList As List(Of String)) As ComboBox
        Dim ItemText As String

        Dim ComboBox = New ComboBox
        ComboBox.Name = ControlName
        For Each ItemText In ItemTextList
            ComboBox.Items.Add(ItemText)
        Next
        ComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox.SelectedItem = ComboBox.Items(0)
        ComboBox.Anchor = AnchorStyles.Left
        ComboBox.Width = 75

        Return ComboBox
    End Function

    Private Function FormatOptionsLabel(ControlName As String,
                                        LabelText As String) As Label
        Dim Label = New Label
        Label.Name = ControlName
        Label.Text = LabelText
        Label.Anchor = AnchorStyles.Left
        Label.AutoSize = True

        Return Label
    End Function

    Private Function FormatOptionsCheckBox(ControlName As String,
                                           CheckBoxText As String) As CheckBox
        Dim CheckBox = New CheckBox
        CheckBox.Name = ControlName
        CheckBox.Text = CheckBoxText
        CheckBox.Anchor = AnchorStyles.Left
        CheckBox.AutoSize = True

        Return CheckBox
    End Function

    Private Function FormatOptionsTextBox(ControlName As String,
                                          TextBoxText As String) As TextBox
        Dim TextBox = New TextBox
        TextBox.Name = ControlName
        TextBox.Text = TextBoxText
        TextBox.Dock = DockStyle.Fill

        Return TextBox
    End Function

    Private Function FormatOptionsButton(ControlName As String,
                                         ButtonText As String) As Button
        Dim Button = New Button
        Button.Name = ControlName
        Button.Name = ControlName
        Button.Text = ButtonText

        Return Button
    End Function

    Private Sub FormatOptionsTLP(TLP As TableLayoutPanel,
                                 Name As String,
                                 NumRows As Integer)

        TLP.Name = Name
        TLP.RowCount = NumRows
        For i As Integer = 0 To TLP.RowCount - 1
            TLP.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        Next

        TLP.ColumnCount = 2
        TLP.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
        TLP.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        TLP.Dock = DockStyle.Fill

        TLP.AutoSize = True

    End Sub

    Private Function GetTaskImage(TaskName As String) As Image
        Dim Image As Image

        Select Case TaskName
            Case "OpenSave"
                Image = Form_Main.ImageList1.Images("OpenSave.png")
            Case "ActivateAndUpdateAll"
                Image = Form_Main.ImageList1.Images("ActivateAndUpdateAll.png")
            Case "PropertyFindReplace"
                Image = Form_Main.ImageList1.Images("PropertyFindReplace.png")
            Case "UpdatePhysicalProperties"
                Image = Form_Main.ImageList1.Images("UpdatePhysicalProperties.png")
            Case "VariablesEdit"
                Image = Form_Main.ImageList1.Images("VariablesEdit.png")
            Case "CopyOverallSizeToVariableTable"
                Image = Form_Main.ImageList1.Images("CopyOverallSizeToVariableTable.png")
            Case "RemoveFaceStyleOverrides"
                Image = Form_Main.ImageList1.Images("RemoveFaceStyleOverrides.png")
            Case "UpdateFaceAndViewStylesFromTemplate"
                Image = Form_Main.ImageList1.Images("UpdateFaceAndViewStylesFromTemplate.png")
            Case "HideConstructions"
                Image = Form_Main.ImageList1.Images("HideConstructions.png")
            Case "FitPictorialView"
                Image = Form_Main.ImageList1.Images("FitPictorialView.png")
            Case "PartNumberDoesNotMatchFilename"
                Image = Form_Main.ImageList1.Images("PartNumberDoesNotMatchFilename.png")
            Case "MissingDrawing"
                Image = Form_Main.ImageList1.Images("MissingDrawing.png")
            Case "BrokenLinks"
                Image = Form_Main.ImageList1.Images("BrokenLinks.png")
            Case "LinksOutsideInputDirectory"
                Image = Form_Main.ImageList1.Images("LinksOutsideInputDirectory.png")
            Case "FailedRelationships"
                Image = Form_Main.ImageList1.Images("FailedRelationships.png")
            Case "UnderconstrainedRelationships"
                Image = Form_Main.ImageList1.Images("UnderconstrainedRelationships.png")
            Case "CheckInterference"
                Image = Form_Main.ImageList1.Images("CheckInterference.png")
            Case "RunExternalProgram"
                Image = Form_Main.ImageList1.Images("RunExternalProgram.png")
            Case "InteractiveEdit"
                Image = Form_Main.ImageList1.Images("InteractiveEdit.png")
            Case "SaveAs"
                Image = Form_Main.ImageList1.Images("SaveAs.png")
            Case "UpdateMaterialFromMaterialTable"
                Image = Form_Main.ImageList1.Images("UpdateMaterialFromMaterialTable.png")
            Case "UpdateInsertPartCopies"
                Image = Form_Main.ImageList1.Images("UpdateInsertPartCopies.png")
            Case "FailedOrWarnedFeatures"
                Image = Form_Main.ImageList1.Images("FailedOrWarnedFeatures.png")
            Case "SuppressedOrRolledBackFeatures"
                Image = Form_Main.ImageList1.Images("SuppressedOrRolledBackFeatures.png")
            Case "UnderconstrainedProfiles"
                Image = Form_Main.ImageList1.Images("UnderconstrainedProfiles.png")
            Case "InsertPartCopiesOutOfDate"
                Image = Form_Main.ImageList1.Images("InsertPartCopiesOutOfDate.png")
            Case "MaterialNotInMaterialTable"
                Image = Form_Main.ImageList1.Images("MaterialNotInMaterialTable.png")
            Case "UpdateDesignForCost"
                Image = Form_Main.ImageList1.Images("UpdateDesignForCost.png")
            Case "FlatPatternMissingOrOutOfDate"
                Image = Form_Main.ImageList1.Images("FlatPatternMissingOrOutOfDate.png")
            Case "UpdateDrawingViews"
                Image = Form_Main.ImageList1.Images("UpdateDrawingViews.png")
            Case "UpdateStylesFromTemplate"
                Image = Form_Main.ImageList1.Images("UpdateStylesFromTemplate.png")
            Case "UpdateDrawingBorderFromTemplate"
                Image = Form_Main.ImageList1.Images("UpdateDrawingBorderFromTemplate.png")
            Case "DrawingViewOnBackgroundSheet"
                Image = Form_Main.ImageList1.Images("DrawingViewOnBackgroundSheet.png")
            Case "DrawingViewsOutOfDate"
                Image = Form_Main.ImageList1.Images("DrawingViewsOutOfDate.png")
            Case "DetachedDimensionsOrAnnotations"
                Image = Form_Main.ImageList1.Images("DetachedDimensionsOrAnnotations.png")
            Case "Print"
                Image = Form_Main.ImageList1.Images("Print.png")

            Case Else
                Image = Form_Main.ImageList1.Images("Synch_16.png")
        End Select

        Return Image
    End Function

    Private Sub FormatButton(Button As Button)
        Button.Text = ""
        Button.FlatStyle = FlatStyle.Flat
        Button.FlatAppearance.BorderSize = 0
        Button.Anchor = AnchorStyles.Top
        Button.Size = New Size(20, 20)
        AddHandler Button.Click, AddressOf Button_Click
    End Sub

    Private Sub FormatCheckBox(CheckBox As CheckBox)
        CheckBox.Text = ""
        CheckBox.AutoSize = True
        CheckBox.Anchor = AnchorStyles.Top
        'CheckBox.Padding = New Padding(2, 5, 0, 0)

        CheckBox.Appearance = Appearance.Button
        CheckBox.FlatStyle = FlatStyle.Flat
        CheckBox.FlatAppearance.BorderSize = 0
        CheckBox.FlatAppearance.CheckedBackColor = SystemColors.Control

        CheckBox.ImageList = Form_Main.ImageList1
        CheckBox.ImageKey = "icons8_unchecked_checkbox_16.png"
        CheckBox.Image = Form_Main.ImageList1.Images(CheckBox.ImageKey)
        AddHandler CheckBox.CheckedChanged, AddressOf CheckBox_CheckedChanged
    End Sub

    Public Sub CheckBox_CheckedChanged(sender As System.Object, e As System.EventArgs)
        'AddHandler CheckBoxDict(CheckBoxName).CheckedChanged, AddressOf CheckBox_CheckedChanged
        Dim CheckBox As CheckBox = DirectCast(sender, CheckBox)
        Dim L As New List(Of String)
        Dim TaskName As String
        Dim ControlType As String
        Dim ControlFunction As String

        L = CheckBox.Name.Split("_"c).ToList ' eg, PartNumberDoesNotMatchFilename_Button_Select
        TaskName = L(0) ' can also be TLPTasks
        ControlType = L(1) ' hopefully CheckBox
        ControlFunction = L(2)  ' eg, Expand, Select, Edit, etc

        If CheckBox.Checked Then
            CheckBox.ImageKey = "icons8_Checked_Checkbox_16.png"
            CheckBox.Image = Form_Main.ImageList1.Images("icons8_Checked_Checkbox_16.png")
        Else
            CheckBox.ImageKey = "icons8_unchecked_checkbox_16.png"
            CheckBox.Image = Form_Main.ImageList1.Images("icons8_unchecked_checkbox_16.png")
        End If

        If (Not TaskName = "TLPTasks") And (ControlFunction.ToLower = "select") Then
            Dim TLPOptionsName As String = String.Format("{0}_TLP_Options", TaskName)

            ' Some tasks do not have options. Those shouldn't have an Expand button or ShowHideOption checkbox either.
            If ControlsDict.Keys.Contains(TLPOptionsName) Then

                Dim ExpandButtonName As String = String.Format("{0}_Button_Expand", TaskName)
                Dim ShowHideOptionsCheckBoxName As String = String.Format("{0}_CheckBox_ShowHideOptions", TaskName)

                Dim ExpandButton As Button = CType(ControlsDict(ExpandButtonName), Button)
                Dim ShowHideOptionsCheckBox As CheckBox = CType(ControlsDict(ShowHideOptionsCheckBoxName), CheckBox)

                If CheckBox.Checked Then
                    If ShowHideOptionsCheckBox.Checked Then
                        ControlsDict(TLPOptionsName).Visible = False
                        ExpandButton.ImageKey = "expand.png"
                        ExpandButton.Image = Form_Main.ImageList1.Images("expand.png")
                    Else
                        ControlsDict(TLPOptionsName).Visible = True
                        ExpandButton.ImageKey = "collapse.png"
                        ExpandButton.Image = Form_Main.ImageList1.Images("collapse.png")
                    End If
                Else
                    ControlsDict(TLPOptionsName).Visible = False
                    ExpandButton.ImageKey = "expand.png"
                    ExpandButton.Image = Form_Main.ImageList1.Images("expand.png")
                End If
            End If
        End If

    End Sub

    Public Sub Button_Click(sender As System.Object, e As System.EventArgs)

        Dim Button As Button = DirectCast(sender, Button)
        Dim L As New List(Of String)
        Dim TaskName As String
        Dim ControlType As String
        Dim ControlFunction As String
        Dim tf As Boolean

        L = Button.Name.Split("_"c).ToList ' eg, PartNumberDoesNotMatchFilename_Button_Select
        TaskName = L(0) ' can also be TLPTasks
        ControlType = L(1) ' hopefully Button
        ControlFunction = L(2)  ' eg, Select, Edit, etc

        If ControlFunction.ToLower = "expand" Then
            ToggleShowHideOptions(TaskName, Button.ImageKey = "expand.png")
        End If

        If ControlFunction.ToLower = "select" Then
            If TaskName = "TLPTasks" Then
                ClearSelections()
            End If
        End If

        tf = ControlFunction = "Assembly"
        tf = tf Or (ControlFunction = "Part")
        tf = tf Or (ControlFunction = "Sheetmetal")
        tf = tf Or (ControlFunction = "Draft")

        If tf Then
            If TaskName = "TLPTasks" Then
                SelectedTasksAddFileType(ControlFunction)
            End If
        End If

        If ControlFunction.ToLower = "help" Then
            ' https://stackoverflow.com/questions/4580263/how-to-open-in-default-browser-in-c-sharp

            If Not TaskName = "TLPTasks" Then
                Dim HelpURL As String = Tasks(TaskName).HelpURL
                System.Diagnostics.Process.Start(HelpURL)
            Else
                System.Diagnostics.Process.Start("https://github.com/rmcanany/SolidEdgeHousekeeper#readme")
            End If
        End If

    End Sub

    Private Sub SelectedTasksAddFileType(FileType As String)
        Dim TaskNameList As New List(Of String)
        Dim TaskName As String
        Dim SelectCheckBoxName As String
        Dim SelectCheckBox As CheckBox
        Dim FileTypeCheckBoxName As String  ' Assembly, Part, Sheetmetal, Draft
        Dim FileTypeCheckBox As CheckBox
        Dim AllAlreadyChecked As Boolean
        TaskNameList = GetTaskNames()

        ' See if the file type for every selected task is already checked
        AllAlreadyChecked = True

        For Each TaskName In TaskNameList
            If Not TaskName = "TLPTasks" Then
                SelectCheckBoxName = String.Format("{0}_CheckBox_Select", TaskName)
                SelectCheckBox = CType(ControlsDict(SelectCheckBoxName), CheckBox)
                FileTypeCheckBoxName = String.Format("{0}_CheckBox_{1}", TaskName, FileType)
                If ControlsDict.Keys.Contains(FileTypeCheckBoxName) Then
                    FileTypeCheckBox = CType(ControlsDict(FileTypeCheckBoxName), CheckBox)
                    If SelectCheckBox.Checked Then
                        If Not FileTypeCheckBox.Checked Then
                            AllAlreadyChecked = False
                        End If
                    Else
                        FileTypeCheckBox.Checked = False  ' Uncheck FileType if task is not selected
                    End If
                End If
            End If
        Next

        For Each TaskName In TaskNameList
            If Not TaskName = "TLPTasks" Then
                SelectCheckBoxName = String.Format("{0}_CheckBox_Select", TaskName)
                SelectCheckBox = CType(ControlsDict(SelectCheckBoxName), CheckBox)
                FileTypeCheckBoxName = String.Format("{0}_CheckBox_{1}", TaskName, FileType)
                If ControlsDict.Keys.Contains(FileTypeCheckBoxName) Then
                    FileTypeCheckBox = CType(ControlsDict(FileTypeCheckBoxName), CheckBox)
                    If SelectCheckBox.Checked Then
                        If AllAlreadyChecked Then
                            FileTypeCheckBox.Checked = False
                        Else
                            FileTypeCheckBox.Checked = True
                        End If
                    Else
                        FileTypeCheckBox.Checked = False
                    End If
                End If
            End If
        Next

    End Sub

    Private Sub ClearSelections()
        Dim TaskNameList As New List(Of String)
        Dim TaskName As String
        TaskNameList = GetTaskNames()
        Dim SelectCheckBoxName As String
        Dim CheckBox As CheckBox

        For Each TaskName In TaskNameList
            If Not TaskName = "TLPTasks" Then
                SelectCheckBoxName = String.Format("{0}_CheckBox_Select", TaskName)
                CheckBox = CType(ControlsDict(SelectCheckBoxName), CheckBox)
                CheckBox.Checked = False
            End If
        Next

    End Sub

    Private Sub ToggleShowHideOptions(TaskName As String, Show As Boolean)

        Dim Button As Button

        If TaskName = "TLPTasks" Then
            Dim TaskNameList As New List(Of String)
            Dim tmpTaskName As String
            TaskNameList = GetTaskNames()

            For Each tmpTaskName In TaskNameList
                If Not tmpTaskName = "TLPTasks" Then
                    ToggleShowHideOptions(tmpTaskName, Show)
                End If
            Next

            Dim ExpandButtonName As String = String.Format("{0}_Button_Expand", TaskName)
            Button = CType(ControlsDict(ExpandButtonName), Button)
            If Button.ImageKey = "expand.png" Then
                Button.ImageKey = "collapse.png"
                Button.Image = Form_Main.ImageList1.Images("collapse.png")
            Else
                Button.ImageKey = "expand.png"
                Button.Image = Form_Main.ImageList1.Images("expand.png")
            End If
        Else
            Dim TLPOptionsName As String = String.Format("{0}_TLP_Options", TaskName)
            Dim ExpandButtonName As String = String.Format("{0}_Button_Expand", TaskName)
            If ControlsDict.Keys.Contains(TLPOptionsName) Then
                Button = CType(ControlsDict(ExpandButtonName), Button)
                If Show Then
                    ControlsDict(TLPOptionsName).Visible = True
                    Button.ImageKey = "collapse.png"
                    Button.Image = Form_Main.ImageList1.Images("collapse.png")
                Else
                    ControlsDict(TLPOptionsName).Visible = False
                    Button.ImageKey = "expand.png"
                    Button.Image = Form_Main.ImageList1.Images("expand.png")
                End If
            End If
        End If

    End Sub

    Private Function GetTaskNames() As List(Of String)
        Dim TaskNameList As New List(Of String)
        Dim ControlName As String
        Dim L As New List(Of String)

        Dim TaskName As String
        Dim ControlType As String
        Dim ControlFunction As String

        For Each ControlName In ControlsDict.Keys
            If Not ControlName = "TLPTasks" Then  ' This is the name of the parent TableLayoutPanel
                L = ControlName.Split("_"c).ToList
                TaskName = L(0) ' can also be TLPTasks
                ControlType = L(1) ' hopefully Button
                ControlFunction = L(2)  ' eg, Select, Edit, etc

                If Not TaskNameList.Contains(TaskName) Then
                    TaskNameList.Add(TaskName)
                End If

            End If

        Next

        Return TaskNameList
    End Function

    Private Sub PopulateControlsDict()
        ControlsDict.Clear()
        AddControlsToControlsDict(TLPTasks)
    End Sub

    Private Sub AddControlsToControlsDict(Control As Control)
        If Not ControlsDict.Keys.Contains(Control.Name) Then
            ControlsDict(Control.Name) = Control
            If Control.HasChildren Then
                For Each C As Control In Control.Controls
                    AddControlsToControlsDict(C)
                Next
            End If
        End If
    End Sub


End Class
