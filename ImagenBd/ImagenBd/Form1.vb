Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Public Class Form1
    Dim Str As String = "Server=(local);Database=DatosClientes;User Id=sa;Password=Ads720510.;"
    Dim Da As New SqlDataAdapter
    Dim Dt As DataTable
    Dim Cn As New SqlConnection(Str)
    Dim Cmd As New SqlCommand

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        OpenFileDialog1.Filter = "Todos(*.Jpg, *.Png, *.Gif, *.Tiff, *.Jpeg, *.Bmp)|*.Jpg; *.Png; *.Gif; *.Tiff; *.Jpeg; *.Bmp"

        'ejecutar evento click de boton
        Button3_Click(sender, e)
    End Sub

    'boton mostrar 
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Size = New System.Drawing.Size(396, 591)
        Try
            Cn.Open()
            With Cmd
                .CommandType = CommandType.Text
                .CommandText = "SELECT * from imagen"
                .Connection = Cn
            End With
            With Da
                .SelectCommand = Cmd
                Dt = New DataTable
                .Fill(Dt)
                DataGridView1.DataSource = Dt
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Cn.Close()
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage

    End Sub

    'boton guardar imagen 
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            Cn.Open()
            Dim arrFilename() As String = Split(Text, "\")
            Array.Reverse(arrFilename)
            Dim ms As New MemoryStream
            PictureBox1.Image.Save(ms, PictureBox1.Image.RawFormat)
            Dim arrImage() As Byte = ms.GetBuffer
            With Cmd
                .CommandType = CommandType.Text
                .CommandText = "Insert Into imagen(Imagen)Values(@Imagen)"
                .Connection = Cn
                .Parameters.Add(New SqlParameter("@Imagen", SqlDbType.Image)).Value = arrImage
            End With
            MessageBox.Show("Registrado Correctamente")
            Cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Cmd.Parameters.Clear()
            Cn.Close()
        End Try
    End Sub

    'boton seleccionar imagen 
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        OpenFileDialog1.ShowDialog()
        Label1.Text = OpenFileDialog1.FileName.ToString
        PictureBox1.Image = System.Drawing.Image.FromFile(Label1.Text)
    End Sub

    'extraer imagen
    Function ExtraerImagen(ByVal Foto As Integer) As Byte()
        With Cmd
            .CommandType = CommandType.Text
            .CommandText = "Select Imagen From imagen Where id_imagen = " & Foto
            .Connection = Cn
        End With
        With Cn
            .Open()
            Dim MyPhoto() As Byte = CType(Cmd.ExecuteScalar(), Byte())
            .Close()
            Return MyPhoto
        End With
    End Function

    'evento click de gridview
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Try
            Dim ms As New MemoryStream(ExtraerImagen(CInt(DataGridView1.SelectedCells(0).Value)))
            PictureBox1.Image = Image.FromStream(ms)
            Me.PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub



End Class
