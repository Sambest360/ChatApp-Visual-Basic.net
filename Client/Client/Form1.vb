Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class Form1

    Dim client As TcpClient
    Dim username As String

    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        Try
            Dim ip As String = "10.16.170.120"
            Dim port As Integer = 6655

            client = New TcpClient(ip, port)

            username = InputBox("Enter your username:", "Username")

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

                Dim buffer(1024) As Byte
                Dim bytesRead As Integer = ns.Read(buffer, 0, buffer.Length)

                If bytesRead > 0 Then
                    Dim txt As String = Encoding.ASCII.GetString(buffer, 0, bytesRead)

                    If Not txt.StartsWith(username & ":") Then
                        AppendText(txt)
                    End If
                End If

            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub AppendText(text As String)

        If RichTextBox1.InvokeRequired Then
            RichTextBox1.Invoke(Sub() RichTextBox1.AppendText(text & vbNewLine))
        Else
            RichTextBox1.AppendText(text & vbNewLine)
        End If
    End Sub

    Private Sub btnSned_Click(sender As Object, e As EventArgs) Handles btnSned.Click
        Try
            Dim ns As NetworkStream = client.GetStream()

            Dim message As String = username & ": " & TextBox1.Text
            ns.Write(Encoding.ASCII.GetBytes(message), 0, message.Length)

            AppendText(message)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        TextBox1.Clear()
        TextBox1.Focus()
    End Sub
End Class
