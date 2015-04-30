Imports System.IO

Public Class Contact
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim value As String = File.ReadAllText(Server.MapPath("~/App_Data/Files") & "archivo.txt")
            ' Write to screen.
            LBInfoFile.Text = value
        End If
    End Sub
End Class