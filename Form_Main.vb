Option Strict On
Imports System.Windows.Forms.VisualStyles

Public Class Form_Main


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim PropertySet As String
        Dim PropertyName As String
        Dim Formula = "System.meyou|R11"

        Dim i = Formula.IndexOf(".")  ' First occurrence
        Dim L = Len(Formula)

        PropertySet = Formula.Substring(0, i)
        PropertyName = Formula.Substring(i + 1, L - (i + 1))

        Dim tmpFormula = Formula.Split(CType(".", Char))
        If tmpFormula.Length > 0 Then PropertySet = tmpFormula(0) 'Formula.Substring(0, i)    ' "Custom"
        If tmpFormula.Length > 1 Then PropertyName = tmpFormula(1) 'Formula.Substring(i + 1)  ' "hmk_Engineer|R1"


    End Sub



    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub SelectPropertyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectPropertyToolStripMenuItem.Click
        Dim TextBox = CType(ContextMenuStrip1.SourceControl, TextBox)
        Dim CaretPosition = TextBox.SelectionStart
        TextBox.Text = TextBox.Text.Insert(CaretPosition, "%{Material}")
        Dim BeforeString = TextBox.Text.Substring(0, CaretPosition - 1)
        Dim i = 0
    End Sub
End Class