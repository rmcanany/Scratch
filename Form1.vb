Option Strict On
Imports System.Windows.Forms.VisualStyles

Public Class Form1

    Public Shared SelectedSheets As Dictionary(Of String, SolidEdgeDraft.PaperSizeConstants)
    'Private ColumnNames As New List(Of String)
    Private Tasks As New LabelToAction
    Private ControlNames As New List(Of String)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' https://stackoverflow.com/questions/2197452/how-to-disable-horizontal-scrollbar-for-table-panel-in-winforms
        Dim VerticalScrollWidth As Integer = SystemInformation.VerticalScrollBarWidth
        'TLPTasks.Padding = New Padding(0, 0, VerticalScrollWidth, 0)
        TLPTasks.Padding = New Padding(0, 0, 1, 0)

        Dim TTU As New TaskTabUtilities
        TTU.BuildTLPTasks(TLPTasks)

    End Sub


    Private Sub TLPTasks_CellPaint(sender As Object, e As TableLayoutCellPaintEventArgs) Handles TLPTasks.CellPaint
        ' https://stackoverflow.com/questions/34064499/how-to-set-cell-color-in-tablelayoutpanel-dynamically

        Dim BrushColorFileType As Brush = New SolidBrush(Color.FromArgb(32 + 16, 190, 190, 255))
        Dim BrushColorHeader As Brush = New SolidBrush(Color.FromArgb(127, 190, 190, 255))
        Dim tf As Boolean

        tf = e.Column = 2
        tf = tf Or e.Column = 3
        tf = tf Or e.Column = 4
        tf = tf Or e.Column = 5

        tf = tf And Not e.Row = 0

        ' File type columns
        If tf Then
            e.Graphics.FillRectangle(BrushColorFileType, e.CellBounds)
        End If

        ' Header row
        If e.Row = 0 Then
            e.Graphics.FillRectangle(BrushColorHeader, e.CellBounds)
        End If

        ' Cell borders
        e.Graphics.DrawRectangle(New Pen(Color.FromArgb(255, 220, 220, 255)), e.CellBounds)

    End Sub

    'Private Sub Form1_ResizeBegin(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeBegin
    '    Me.SuspendLayout()
    'End Sub

    'Private Sub Form1_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
    '    Me.ResumeLayout()
    'End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    End Sub
End Class