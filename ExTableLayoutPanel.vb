Public Class ExTableLayoutPanel
    ' https://www.vbforums.com/showthread.php?600989-Slow-resizing-of-a-TableLayoutPanel

    Inherits TableLayoutPanel

    Public Sub New()
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

End Class