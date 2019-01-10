Imports System.IO

Public Class frmViewPicture
    Dim vStationName As String
    Dim vPath As String

    Private Sub frmViewPicture_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim fName As String = ("system.ini")              'path to text file
        Dim My_Ini As New Ini(fName)
        vStationName = My_Ini.GetValue("storage", "name")

        If vStationName = "" Then
            vStationName = Environment.MachineName
        End If

        vPath = My_Ini.GetValue("storage", "path")
        If vPath = "" Then
            vPath = Directory.GetCurrentDirectory() & "\"
        End If

        For Each Dir As String In Directory.GetDirectories(vPath & "mostupdate")
            cbStation.Items.Add(Dir.Split("\").Last)
        Next



    End Sub

    Private Sub cmdShow_Click(sender As Object, e As EventArgs) Handles cmdShow.Click

        PictureBox1.Image = Nothing
        FlowLayoutPanel1.Controls.Clear()


        Dim files() As String = Directory.GetFiles(vPath & "mostupdate\" & cbStation.SelectedItem)

        For Each file As String In Directory.GetFiles(vPath & "mostupdate\" & cbStation.SelectedItem)
            'PictureBox1.Image = SafeImageFromFile(file)
            Dim imageViewer As MyPictureBox = New MyPictureBox
            imageViewer.fileName = file
            imageViewer.Image = SafeImageFromFile(file)
            imageViewer.SizeMode = PictureBoxSizeMode.Zoom
            imageViewer.center = True
            AddHandler imageViewer.Click, AddressOf PictureBoxClick

            imageViewer.Height = FlowLayoutPanel1.Height - 20
            imageViewer.Width = FlowLayoutPanel1.Width / 12

            AddImageToPanel1(imageViewer)
        Next
    End Sub

    Function SafeImageFromFile(path As String) As Image
        Dim bytes = File.ReadAllBytes(path)
        Using ms As New MemoryStream(bytes)
            Dim img = Image.FromStream(ms)
            Return img
        End Using
    End Function

    Private Sub PictureBoxClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As MyPictureBox = DirectCast(sender, MyPictureBox)
        'MsgBox("Click working " & btn.Image.ToString)
        ' lblCapture.Text = btn.fileName
        PictureBox1.Image = btn.Image
        'If btn.center Then
        '    PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
        'Else
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        'End If


    End Sub

    Private Sub AddImageToPictureBox(ByVal image As Bitmap)

        If PictureBox1.InvokeRequired Then
            PictureBox1.Invoke(New Action(Of Bitmap)(AddressOf AddImageToPictureBox), image)
        Else
            PictureBox1.Image = image
        End If

    End Sub

    Private Sub AddImageToPanel1(ByVal image As MyPictureBox)

        If FlowLayoutPanel1.InvokeRequired Then
            FlowLayoutPanel1.Invoke(New Action(Of MyPictureBox)(AddressOf AddImageToPanel1), image)
        Else
            FlowLayoutPanel1.Controls.Add(image)
        End If
        FlowLayoutPanel1.ScrollControlIntoView(image)
    End Sub


End Class

