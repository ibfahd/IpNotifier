Public Class Form1

    Private Function IniFile() As String
        Dim file As String
        file = Environment.GetEnvironmentVariable("windir") & "\ipnotifier.ini"
        Return file
    End Function

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        txtEmail.Text = ReadIni(IniFile, "FromEmail", "Login", "")
        txtPass.Text = AES_Decrypt(ReadIni(IniFile, "FromEmail", "Password", ""), "zYmVtypeQ98932#%")
        txtSmtp.Text = ReadIni(IniFile, "FromEmail", "Smtp", "")
        txtPort.Text = ReadIni(IniFile, "FromEmail", "Port", "")
        rbSSL.Checked = CBool(ReadIni(IniFile, "FromEmail", "Cryptage", "True"))
        cbTLS.Checked = CBool(ReadIni(IniFile, "FromEmail", "TLS", "True"))
        txtToEmail.Text = ReadIni(IniFile, "ToEmail", "Mail", "")
        nudInterval.Value = CDec(ReadIni(IniFile, "Intervalle", "s", ""))
    End Sub

    Private Sub cmdok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdok.Click
        writeIni(IniFile, "FromEmail", "Login", txtEmail.Text)
        writeIni(IniFile, "FromEmail", "Password", AES_Encrypt(txtPass.Text, "zYmVtypeQ98932#%"))
        writeIni(IniFile, "FromEmail", "Smtp", txtSmtp.Text)
        writeIni(IniFile, "FromEmail", "Port", txtPort.Text)
        writeIni(IniFile, "FromEmail", "Cryptage", rbSSL.Checked.ToString)
        writeIni(IniFile, "FromEmail", "TLS", cbTLS.Checked.ToString)
        writeIni(IniFile, "ToEmail", "Mail", txtToEmail.Text)
        writeIni(IniFile, "Intervalle", "s", nudInterval.Value)
        MsgBox("Configuration Enregistrer", MsgBoxStyle.Information)
        Application.Exit()
    End Sub

    Private Sub rbSSL_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbSSL.CheckedChanged
        cbTLS.Enabled = rbSSL.Checked
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Application.Exit()
    End Sub

    Private Sub cmdAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbout.Click
        MsgBox("IpNotifier v1.0" & Chr(13) & "i-Xpert.com", MsgBoxStyle.Information)
    End Sub
End Class