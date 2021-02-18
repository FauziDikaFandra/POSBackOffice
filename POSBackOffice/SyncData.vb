Public Class SyncData
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim ds As New DataSet
        Try
            ds = SelProcSer("spp_SOD_Bali", 0)
            ds.Clear()
            ds = SelProcSer("spp_EOD_Bali", 0)
            MsgBox("Successfull")
        Catch ex As Exception
            MsgBox("Error !!!!")
        End Try

    End Sub
End Class