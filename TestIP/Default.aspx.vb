Imports System.Net
Imports System.IO

Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            RecibirDatos()
        End If
    End Sub

    Private Sub RecibirDatos()
        Response.AppendHeader("Access-Control-Allow-Origin", "*") 'Allow Request Cross Domain
        'En esta página de notificación se reciben los datos a través del método POST
        Dim IPNModeURL As String
        'Post back to either sandbox or live
        If ConfigurationManager.AppSettings.Item("Paypal_IPNMode") = "live" Then
            IPNModeURL = "https://www.paypal.com/cgi-bin/webscr"
        Else
            IPNModeURL = "https://www.sandbox.paypal.com/cgi-bin/webscr"
        End If

        Dim req As HttpWebRequest = CType(WebRequest.Create(IPNModeURL), HttpWebRequest)
        'Dim CodePage As Integer = 20127  'ASCII
        ''1252 'ANSI windows-1252 por Default
        ''65001:  'UTF-8 
        ''20127  'ASCII
        'Dim charset = Request.Form("charset")
        'For Each ei As EncodingInfo In Encoding.GetEncodings()
        '    If ei.Name = charset Then
        '        CodePage = ei.CodePage 'Se asigna el Encoding del campo charset
        '        Exit For
        '    End If
        'Next

       'Set values for the request back
        req.Method = "POST"
        req.ContentType = "application/x-www-form-urlencoded"
        Dim Param() As Byte = Request.BinaryRead(HttpContext.Current.Request.ContentLength)
        Dim strPayPal As String = System.Text.Encoding.Default.GetString(Param)
        Dim strRequest As String = "cmd=_notify-validate" & strPayPal
        req.ContentLength = strRequest.Length

        Try
            My.Computer.FileSystem.WriteAllText(Server.MapPath("~/App_Data/Files") & "archivo.txt", Date.Now.ToString & " - " & strPayPal & vbCrLf, True)
        Catch ex As Exception

        End Try
        'for proxy
        'Dim proxy As New WebProxy(New System.Uri("http://url:port#"))
        'req.Proxy = proxy

        'Send the request to PayPal and get the response


        Dim Item_name As String = Request.Form("item_name")
        Dim Item_number As String = Request.Form("item_number")
        Dim Payment_status As String = Request.Form("payment_status")
        Dim Payment_amount As String = Request.Form("mc_gross")
        Dim Payment_currency As String = Request.Form("mc_currency")
        Dim Txn_id As String = Request.Form("txn_id")
        Dim Receiver_email As String = Request.Form("receiver_email")
        Dim Payer_email As String = Request.Form("payer_email")
        Dim Txn_type As String = Request.Form("Txn_type")


        ' Write the request back IPN strings
        Dim stOut As New StreamWriter(req.GetRequestStream(), System.Text.Encoding.Default)
        stOut.Write(strRequest)
        stOut.Close()

        ' Do the request to PayPal and get the response
        Dim stIn As New StreamReader(req.GetResponse().GetResponseStream())
        Dim strResponse = stIn.ReadToEnd()
        stIn.Close()



        Dim _reintentar As Boolean = False

        If strResponse = "VERIFIED" Then
            If Payment_status = "Completed" Then



            End If
            'check the payment_status is Completed
            'check that txn_id has not been previously processed
            'check that receiver_email is your Primary PayPal email
            'check that payment_amount/payment_currency are correct
            'process payment
        ElseIf strResponse = "INVALID" Then
            'log for manual investigation
        Else
            'Response wasn't VERIFIED or INVALID, log for manual investigation
        End If

        Try
            My.Computer.FileSystem.WriteAllText(Server.MapPath("~/App_Data/Files") & "archivo.txt", Date.Now.ToString & " - Txn_id: " & Txn_id & ", Txn_type: " & Txn_type & ", RESPONSE: " & strResponse & ", PAYMENT STATUS: " & Payment_status & vbCrLf, True)
        Catch ex As Exception

        End Try

        If _reintentar Then
            Response.StatusCode = HttpStatusCode.InternalServerError
        Else
            Response.StatusCode = HttpStatusCode.OK
        End If

        Response.End()
    End Sub
End Class