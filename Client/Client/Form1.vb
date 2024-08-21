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

            ' Prompt the user to enter a username
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

                Dim toRecive(100000) As Byte
                Dim bytesRead As Integer = ns.Read(toRecive, 0, toRecive.Length)
                Dim txt As String = Encoding.ASCII.GetString(toRecive, 0, bytesRead)

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

            ' Send the message with the username
            Dim message As String = username & ": " & TextBox1.Text
            ns.Write(Encoding.ASCII.GetBytes(message), 0, message.Length)

            ' Display the sent message in the chat box
            If RichTextBox1.Text.Length > 0 Then
                RichTextBox1.Text &= vbNewLine & message
            Else
                RichTextBox1.Text = message
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        TextBox1.Clear()
        TextBox1.Focus()
    End Sub
End Class
