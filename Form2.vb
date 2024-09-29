Public Class Form2

    Public Property Form1 As Form_Main

    Public Sub New(_Form1 As Form_Main)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Form1 = _Form1

    End Sub
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class