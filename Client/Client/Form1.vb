Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class Form1

    Dim client As TcpClient

    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        Try
            Dim ip As String = "192.168.1.105"
            Dim port As Integer = 6655

            client = New TcpClient(ip, port)

            CheckForIllegalCrossThreadCalls = False

            Threading.ThreadPool.QueueUserWorkItem(AddressOf recieveMessages)

            Me.AcceptButton = btnSned

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        MsgBox("CONNECTED", MessageBoxIcon.Information)
    End Sub

    Private Sub recieveMessages()
        Try
            While True

                Dim ns As NetworkStream = client.GetStream()

                Dim toRecive(100000) As Byte
                ns.Read(toRecive, 0, toRecive.Length)
                Dim txt As String = Encoding.ASCII.GetString(toRecive)


                If RichTextBox1.Text.Length > 0 Then

                    RichTextBox1.Text &= vbNewLine & txt
                Else
                    RichTextBox1.Text = txt
                End If


            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnSned_Click(sender As Object, e As EventArgs) Handles btnSned.Click
        Try

            Dim ns As NetworkStream = client.GetStream()

            ns.Write(Encoding.ASCII.GetBytes(TextBox1.Text), 0, TextBox1.Text.Length)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        TextBox1.Clear()
        TextBox1.Focus()
    End Sub
End Class
