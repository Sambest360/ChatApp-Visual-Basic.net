Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class Form1

    Dim server As TcpListener
    Dim listOfClients As New List(Of TcpClient)

    Private Sub btnStartServer_Click(sender As Object, e As EventArgs) Handles btnStartServer.Click
        Try
            Dim ip As String = "10.16.170.120"
            Dim port As Integer = 6655

            server = New TcpListener(IPAddress.Parse(ip), port)
            server.Start()

            Threading.ThreadPool.QueueUserWorkItem(AddressOf newClient)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        MsgBox("SERVER HAS STARTED", MessageBoxIcon.Information)
    End Sub

    Private Sub newClient(state As Object)
        Dim client As TcpClient = server.AcceptTcpClient
        Try
            listOfClients.Add(client)
            Threading.ThreadPool.QueueUserWorkItem(AddressOf newClient)

            While True
                Dim ns As NetworkStream = client.GetStream()
                Dim buffer(1024) As Byte
                Dim bytesRead As Integer = ns.Read(buffer, 0, buffer.Length)

                If bytesRead > 0 Then

                    Dim txt As String = Encoding.UTF8.GetString(buffer, 0, bytesRead)

                    For Each c As TcpClient In listOfClients
                        If c IsNot client Then
                            Dim nns As NetworkStream = c.GetStream()
                            Dim data As Byte() = Encoding.UTF8.GetBytes(txt)
                            nns.Write(data, 0, data.Length)
                        End If
                    Next
                End If
            End While

        Catch ex As Exception
            If listOfClients.Contains(client) Then
                listOfClients.Remove(client)
            End If
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
