Public Class About
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub
    Protected Sub BOTON_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BOTON.Click
        Try
            Dim Dat As String = "Comparación de alguien vs alguien"
            Dim Etiqueta As String = "http://www.facebook.com/sharer.php?s=100&p[summary]=" & Dat.Replace(":", "%3A").Replace(".", "%2e").Replace(" ", "%20") & "&p[url]=http://ipn-1.apphb.com/About.aspx"

            Response.Redirect(Etiqueta)
        Catch ex As Exception

        End Try
    End Sub

End Class