Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        With My.Settings
            txtEmail.Text = .FromEmail
            txtPass.Text = .Pass
            txtToEmail.Text = .ToEmail
            txtSmtp.Text = .SmtpServer
            txtPort.Text = .Port
            rbNoCrypt.Checked = Not .Cryptage
            rbSSL.Checked = .Cryptage
            cbTLS.CheckState = .TLS
        End With
        cbTLS.Enabled = rbSSL.Checked
    End Sub

    Private Sub cmdok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdok.Click
        With My.Settings
            .FromEmail = txtEmail.Text
            .Pass = txtPass.Text
            .ToEmail = txtToEmail.Text
            .SmtpServer = txtSmtp.Text
            .Port = txtPort.Text
            .Cryptage = rbSSL.Checked
            .TLS = cbTLS.CheckState
        End With
    End Sub

    Private Sub rbSSL_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbSSL.CheckedChanged
        cbTLS.Enabled = rbSSL.Checked
    End Sub
End Class