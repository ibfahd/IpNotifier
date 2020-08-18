Imports System.Net
'Imports System.IO.File
Imports System.Net.Mail
Public Class Service1

    Protected Overrides Sub OnStart(ByVal args() As String)
        Timer1.Interval = CDec(ReadIni(IniFile, "Intervalle", "s", "5")) * 1000
        Timer1.Enabled = True
    End Sub

    Protected Overrides Sub OnStop()
        Timer1.Enabled = False
    End Sub

    Private Sub Timer1_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles Timer1.Elapsed
        VerifierIP(ReadIni(IniFile, "Adresse", "Ip", ""))
    End Sub

    Private Function TrouverIP() As String
        Dim client As WebClient = New WebClient()
        Return client.DownloadString("http://monip.org/")
    End Function

    Private Sub SendIP(ByVal aip As String)
        Dim Femail As String = ReadIni(IniFile, "FromEmail", "Login", "")
        Dim Temail As String = ReadIni(IniFile, "ToEmail", "Mail", "")
        Dim Fsmtp As String = ReadIni(IniFile, "FromEmail", "Smtp", "")
        Dim Fport As String = ReadIni(IniFile, "FromEmail", "Port", "")
        Dim Fcrypt As Boolean = CBool(ReadIni(IniFile, "FromEmail", "Cryptage", "True"))
        Dim Ftls As Boolean = CBool(ReadIni(IniFile, "FromEmail", "TLS", "True"))
        Dim Fpass As String = AES_Decrypt(ReadIni(IniFile, "FromEmail", "Password", ""), "zYmVtypeQ98932#%")
        Try
            Dim mail As New MailMessage(Femail, Temail, "Votre Ip", "IP: " & aip)
            Dim smtp As New SmtpClient(Fsmtp, Fport)
            smtp.EnableSsl = CBool(ReadIni(IniFile, "FromEmail", "Cryptage", "True"))
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network
            If Fcrypt And Ftls Then ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
            If Fcrypt And Not Ftls Then ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
            smtp.UseDefaultCredentials = False
            smtp.Credentials = New Net.NetworkCredential(Femail, Fpass)
            smtp.Send(mail)
            WinLog("Email envoyer avec l'adresse IP : " & aip)
        Catch ex As Exception
            WinLog(ex.Message)
        End Try
    End Sub

    Private Sub VerifierIP(ByVal lip As String)
        Dim ip, ipt As String
        ip = ""
        ipt = TrouverIP()
        ipt = ipt.Substring(ipt.IndexOf(":") + 2, 15)
        Dim i As Integer
        For i = 0 To ipt.Length - 1
            If IsNumeric(ipt.Substring(i, 1)) Or ipt.Substring(i, 1) = "." Then
                ip = ip + ipt.Substring(i, 1)
            End If
        Next
        If ip <> lip Then
            writeIni(IniFile, "Adresse", "Ip", ip)
            SendIP(ip)
        End If

        'If Not Exists("ip.txt") Then
        '    WriteAllText("ip.txt", ip)
        '    mylog.WriteEntry("Création du fichier de sauvegarde", EventLogEntryType.Information)
        'Else
        '    ipr = ReadAllText("ip.txt")
        '    Console.WriteLine("Encienne IP: " & ipr & Chr(13))
        '    mylog.WriteEntry("Encienne IP: " & ipr, EventLogEntryType.Information)
        'End If
        'If ipr <> ip Then
        '    WriteAllText("ip.txt", ip)
        '    Console.WriteLine("Nouvelle IP:" & ip & Chr(13))
        '    mylog.WriteEntry("Nouvelle IP:" & ip, EventLogEntryType.Information)
        '    SendIP(ip)
        'Else
        '    mylog.WriteEntry("IP identique...", EventLogEntryType.Information)
        'End If
    End Sub

    Private Function IniFile() As String
        Dim file As String
        file = Environment.GetEnvironmentVariable("windir") & "\ipnotifier.ini"
        Return file
    End Function

    Private Sub WinLog(ByVal log As String)
        Dim mylog As New EventLog
        If Not Diagnostics.EventLog.SourceExists("IpNotifierService") Then
            Diagnostics.EventLog.CreateEventSource("IpNotifierService", "IpNotifierService Log")
        End If
        mylog.Source = "IpNotifierService"
        mylog.WriteEntry(log, EventLogEntryType.Information)
    End Sub
End Class
