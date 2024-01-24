Imports Microsoft.Search.Interop

Public Class Form2
    ''Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
    '' https://stackoverflow.com/questions/16032193/how-do-i-open-windows-search-in-a-visual-basic-application
    ''~~> API declaration for the windows "Search Results" dialog
    'Private Declare Function ShellSearch Lib "shell32.dll" _
    '    Alias "ShellExecuteA" (ByVal hwnd As Integer,
    '                           ByVal lpOperation As String,
    '                           ByVal lpFile As String,
    '                           ByVal lpParameters As String,
    '                           ByVal lpDirectory As String,
    '                           ByVal nShowCmd As Integer) As Integer

    'Private Const SW_SHOWNORMAL = 1

    'Const drv As String = "C:\"

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'ShellSearch(0, "Find", drv, "", "", SW_SHOWNORMAL)

        Dim CSM As New CSearchManager()

        Dim Manager As CSearchCrawlScopeManager = CSM.GetCatalog("SystemIndex").GetCrawlScopeManager()

        Dim Path As String = "c:\users"
        'CSearchCrawlScopeManager manager = CSM.GetCatalog("SystemIndex").GetCrawlScopeManager();

        If (Manager.IncludedInCrawlScope(Path) = 0) Then
            MsgBox("Got 0")
        End If
    End Sub

End Class