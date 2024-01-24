Option Strict On

Public Class TaskTabUtilities

    Public Sub BuildTLPTasks(TLPTasks As TableLayoutPanel)

        Dim TTUBUI As New TaskTabUtilitiesBuildUI(TLPTasks)
        TTUBUI.BuildTLPTasks()

    End Sub


End Class
