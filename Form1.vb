


Imports System.IO
Imports System.Net

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Reflection

Public Class Form1

    Dim vCameraIp_1 As String
    Dim vCameraCaptureUrl_1 As String
    Dim vCameraLiveUrl_1 As String

    Dim vCameraDelay_1 As Integer = 1000
    Dim vCameraCapture_1 As Integer = 6



    Dim vCameraIp_2 As String
    Dim vCameraCaptureUrl_2 As String
    Dim vCameraLiveUrl_2 As String

    Dim vCameraDelay_2 As Integer = 1000
    Dim vCameraCapture_2 As Integer = 6


    Dim vFirstCapture As Boolean = True

    Dim vStationName As String = ""
    Dim vPath As String = ""

    Private Sub Button1_Click(sender As Object, e As EventArgs)

        AxVLCPlugin21.playlist.add("rtsp://gate:Gateview2018@" & vCameraIp_1 & "/Streaming/Channels/2")
        AxVLCPlugin21.playlist.playItem(0)

    End Sub

    Private Sub btnDownload_Click(sender As Object, e As EventArgs)

    End Sub

    Sub save_image_to_storage()
        '1)List images
        Dim files() As String
        Dim FileName As String
        Dim fileCreatedDate As DateTime

        files = Directory.GetFiles(vPath, "*.png", SearchOption.TopDirectoryOnly)

        If files.Count = 0 Then

            Exit Sub
        End If
        FileName = files(0)
        fileCreatedDate = File.GetLastWriteTime(FileName) '  .GetCreationTime(FileName)

        Dim vYear, vMonth, vDay, vHour As String
        vYear = fileCreatedDate.Year.ToString()
        vMonth = fileCreatedDate.Month.ToString("00")
        vDay = fileCreatedDate.Day.ToString("00")
        vHour = fileCreatedDate.Hour.ToString("00") & fileCreatedDate.Minute.ToString("00")
        '2)Created Folder on Storage
        Dim vNewFolder As String = vPath & vYear & "\" & vMonth & "\" & vDay & "\" &
                                vStationName & "\" & vHour
        If Directory.Exists(vNewFolder) = False Then
            Directory.CreateDirectory(vNewFolder)
        End If
        '3)Move file to Storage
        Dim vOnlyFilename As String = ""
        For Each FileName In files
            vOnlyFilename = Path.GetFileName(FileName)
            If File.Exists(vNewFolder & "\" & vOnlyFilename) Then
                File.Delete(vNewFolder & "\" & vOnlyFilename)
            End If
            My.Computer.FileSystem.MoveFile(FileName, vNewFolder & "\" & vOnlyFilename)
        Next

        '4)Delete images
    End Sub

    Sub start_capture(url As String, delay As Integer, capture As Integer)
        Try

            '-----Start to save all images to Storage---
            save_image_to_storage()
            '-------------------------------------------

            FlowLayoutPanel1.Controls.Clear()

            Dim tClient As New System.Net.WebClient
            tClient.Credentials = New System.Net.NetworkCredential("admin", "Autogate2018")
            If vFirstCapture Then
                Threading.Thread.Sleep(delay)
                vFirstCapture = False
            End If

            For index As Integer = 1 To capture
                captureOneShort(url)

                'Dim imageViewer As MyPictureBox = New MyPictureBox
                'imageViewer = capturePicture(index, tClient, url)
                'FlowLayoutPanel1.Controls.Add(imageViewer)
                'FlowLayoutPanel1.ScrollControlIntoView(imageViewer)
                Application.DoEvents()
                Threading.Thread.Sleep(delay)
            Next
            'tClient.Dispose()

        Catch ex As Exception
            MessageBox.Show("There was an error opening the image file. Check the URL " & ex.Message)
        End Try
    End Sub

    Function capturePicture(index As Integer, cameraObj As WebClient,
                            cameraUrl As String,
                            Optional Height As Integer = 0,
                            Optional Width As Integer = 0,
                            Optional show As Boolean = False,
                            Optional center As Boolean = False,
                            Optional file_prefix As String = "") As MyPictureBox
        'Method#2
        Dim tImage As Bitmap
        Dim imageViewer As MyPictureBox = New MyPictureBox

        Dim ImageInBytes() As Byte = cameraObj.DownloadData(cameraUrl)
        Dim ImageStream As New IO.MemoryStream(ImageInBytes)
        tImage = Bitmap.FromStream(ImageStream)

        If show Then
            PictureBox1.Image = tImage
            PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        End If

        If center Then
            PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
        End If

        tImage.Save(file_prefix & "-" & "image" & index.ToString & ".png", System.Drawing.Imaging.ImageFormat.Png)

        imageViewer.fileName = file_prefix & "-" & "image" & index.ToString & ".png"
        imageViewer.Image = tImage
        imageViewer.SizeMode = PictureBoxSizeMode.Zoom
        AddHandler imageViewer.Click, AddressOf PictureBoxClick


        imageViewer.Height = Height 'FlowLayoutPanel1.Height - 20
        imageViewer.Width = Width 'FlowLayoutPanel1.Width / 7
        'lblCapture.Text = Width'"Capture picture #" & index.ToString

        Return imageViewer
    End Function


    Private Sub PictureBoxClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As MyPictureBox = DirectCast(sender, MyPictureBox)
        'MsgBox("Click working " & btn.Image.ToString)
        lblCapture.Text = btn.fileName
        PictureBox1.Image = btn.Image
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage

    End Sub



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load



        Dim fName As String = ("system.ini")              'path to text file
        Dim My_Ini As New Ini(fName)



        vCameraIp_1 = My_Ini.GetValue("Camera1", "IP")
        vCameraCaptureUrl_1 = "http://gate:Gateview2018@" & vCameraIp_1 & "/Streaming/Channels/2/picture"
        vCameraLiveUrl_1 = "rtsp://gate:Gateview2018@" & vCameraIp_1 & "/Streaming/Channels/2"

        vCameraDelay_1 = Int(My_Ini.GetValue("Camera1", "delay"))
        vCameraCapture_1 = Int(My_Ini.GetValue("Camera1", "capture"))

        GroupBox1.Text = vCameraIp_1 & " - " & My_Ini.GetValue("Camera1", "name")
        btnAuto1.Text = "Auto(" & vCameraCapture_1.ToString & ")"


        vCameraIp_2 = My_Ini.GetValue("Camera2", "IP")
        vCameraCaptureUrl_2 = "http://gate:Gateview2018@" & vCameraIp_2 & "/Streaming/Channels/2/picture"
        vCameraLiveUrl_2 = "rtsp://gate:Gateview2018@" & vCameraIp_2 & "/Streaming/Channels/2"

        vCameraDelay_2 = Int(My_Ini.GetValue("Camera2", "delay"))
        vCameraCapture_2 = Int(My_Ini.GetValue("Camera2", "capture"))

        GroupBox2.Text = vCameraIp_2 & " - " & My_Ini.GetValue("Camera2", "name")



        'Storage setup
        vStationName = My_Ini.GetValue("storage", "name")

        If vStationName = "" Then
            vStationName = Environment.MachineName
        End If

        vPath = My_Ini.GetValue("storage", "path")
        If vPath = "" Then
            vPath = Directory.GetCurrentDirectory() & "\"
        End If



        'My_Ini.DeleteValue("NewSection", "TestValue", True)
        'My_Ini.DeleteSection("NewSection", True)
        'My_Ini.SetValue("Camera1", "IP", "230", True)
        'Me.Text = Me.Text

        Dim versionNumber As Version

        versionNumber = Assembly.GetExecutingAssembly().GetName().Version
        Me.Text = Me.Text & " version :" & versionNumber.ToString

    End Sub


    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If (e.KeyCode = Keys.C AndAlso e.Modifiers = Keys.Alt) Then
            e.Handled = True
            btnDownload_Click(sender, e) 'or cmdExit.PerformClick()
        End If

        If e.KeyCode = Keys.V AndAlso e.Modifiers = Keys.Alt Then
            e.Handled = True
            Button1_Click(sender, e) 'or cmdExit.PerformClick()
        End If

    End Sub

    Private Sub btnSingleCapture_Click(sender As Object, e As EventArgs)
        Dim tClient As New System.Net.WebClient
        tClient.Credentials = New System.Net.NetworkCredential("admin", "Autogate2018")
        Dim imageViewer As MyPictureBox = New MyPictureBox
        imageViewer = capturePicture(FlowLayoutPanel1.Controls.Count + 1, tClient, vCameraCaptureUrl_1)
        FlowLayoutPanel1.Controls.Add(imageViewer)
        FlowLayoutPanel1.ScrollControlIntoView(imageViewer)
    End Sub

    Private Sub btnLive1_Click(sender As Object, e As EventArgs) Handles btnLive1.Click
        AxVLCPlugin21.playlist.add("rtsp://gate:Gateview2018@" & vCameraIp_1 & "/Streaming/Channels/2")
        AxVLCPlugin21.playlist.playItem(0)
    End Sub

    Private Sub btnLive2_Click(sender As Object, e As EventArgs) Handles btnLive2.Click
        AxVLCPlugin22.playlist.add("rtsp://gate:Gateview2018@" & vCameraIp_2 & "/Streaming/Channels/2")
        AxVLCPlugin22.playlist.playItem(0)
    End Sub

    Private Sub btnCapture1_Click(sender As Object, e As EventArgs) Handles btnCapture1.Click
        captureOneShort(vCameraCaptureUrl_1, chkShowCaptured.Checked)
        lblCapture.Text = "Capture picture #" & FlowLayoutPanel1.Controls.Count.ToString
    End Sub

    Sub captureOneShort(url As String, Optional show As Boolean = False,
                        Optional file_prefix As String = "main")
        Dim tClient As New System.Net.WebClient
        tClient.Credentials = New System.Net.NetworkCredential("admin", "Autogate2018")
        Dim imageViewer As MyPictureBox = New MyPictureBox
        imageViewer = capturePicture(FlowLayoutPanel1.Controls.Count + 1, tClient, url,
                                     FlowLayoutPanel1.Height - 20, FlowLayoutPanel1.Width / (vCameraCapture_1 + 4),
                                     show, False, file_prefix)
        FlowLayoutPanel1.Controls.Add(imageViewer)
        FlowLayoutPanel1.ScrollControlIntoView(imageViewer)
        tClient.Dispose()
    End Sub

    Sub captureOneShort2(url As String, Optional show As Boolean = False,
                         Optional center As Boolean = False,
                         Optional file_prefix As String = "sub")
        Dim tClient As New System.Net.WebClient
        tClient.Credentials = New System.Net.NetworkCredential("admin", "Autogate2018")
        Dim imageViewer As MyPictureBox = New MyPictureBox
        imageViewer = capturePicture(Now.Second, tClient, url,
                                     FlowLayoutPanel2.Height - 5, FlowLayoutPanel2.Width / 3, show, center, file_prefix)
        FlowLayoutPanel2.Controls.Add(imageViewer)
        FlowLayoutPanel2.ScrollControlIntoView(imageViewer)
        tClient.Dispose()
    End Sub

    Private Sub btnCapture2_Click(sender As Object, e As EventArgs) Handles btnCapture2.Click
        If FlowLayoutPanel2.Controls.Count = 2 Then
            FlowLayoutPanel2.Controls.RemoveAt(0)
        End If
        captureOneShort2(vCameraCaptureUrl_2, True)
        lblCount2.Text = "Capture picture #" & FlowLayoutPanel2.Controls.Count.ToString
    End Sub

    Private Sub btnAuto1_Click(sender As Object, e As EventArgs) Handles btnAuto1.Click
        start_capture(vCameraCaptureUrl_1, vCameraDelay_1, vCameraCapture_1)
    End Sub

    Private Sub FlowLayoutPanel2_Paint(sender As Object, e As PaintEventArgs) Handles FlowLayoutPanel2.Paint

    End Sub

    Private Sub btnCenter_Click(sender As Object, e As EventArgs) Handles btnCenter.Click
        If FlowLayoutPanel2.Controls.Count = 2 Then
            FlowLayoutPanel2.Controls.RemoveAt(0)
        End If
        captureOneShort2(vCameraCaptureUrl_2, True, True)
        lblCount2.Text = "Capture picture #" & FlowLayoutPanel2.Controls.Count.ToString
    End Sub
End Class

Public Class MyPictureBox
    Inherits PictureBox

    Private _fileName As String
    Public Property fileName() As String
        Get
            Return _fileName
        End Get
        Set(ByVal value As String)
            _fileName = value
        End Set
    End Property




End Class


Public Class Ini

    Private _Sections As New Dictionary(Of String, Dictionary(Of String, String))
    Private _FileName As String
    ''' <summary>
    ''' </summary>
    ''' <param name="IniFileName">Drive,Path and Filname for the inifile</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal IniFileName As String)

        Dim Rd As StreamReader
        Dim Content As String
        Dim Lines() As String
        Dim Line As String
        Dim Key As String
        Dim Value As String
        Dim SectionValues As Dictionary(Of String, String) = Nothing
        Dim Name As String

        _FileName = IniFileName

        'check if the file exists
        If Not File.Exists(IniFileName) Then
            Throw New FileLoadException(String.Format("The file {0} is not found", IniFileName))
        Else
            'Read the file if present.
            Rd = New StreamReader(IniFileName)
            Content = Rd.ReadToEnd
            'Split It into lines
            Lines = Content.Split(vbCrLf)

            'Place the content in an object sructure
            For Each Line In Lines

                'Trim the line
                Line = Line.Trim
                If Line.Length <= 2 OrElse Line.Substring(0, 1) = "'" OrElse Line.Substring(0, 3).ToUpper = "REM" Then
                    'There's no valid data or it's commented out... Do nothing 
                ElseIf Line.IndexOf("[") = 0 AndAlso Line.IndexOf("]") = Line.Length - 1 Then
                    'We hit a section
                    Name = Line.Replace("]", String.Empty).Replace("[", String.Empty).Trim.ToUpper
                    SectionValues = New Dictionary(Of String, String)
                    _Sections.Add(Name.ToUpper, SectionValues)

                    'An = character as the firstcharacter is an invalid line... let's be relaxed an just ignore it.
                ElseIf Line.IndexOf("=") > 0 AndAlso SectionValues IsNot Nothing Then
                    'We hit a value line , empty line or out commented line
                    'we don't use split as that character could be part of the value as well
                    Key = Line.Substring(0, Line.IndexOf("=")).Trim
                    If Line.IndexOf("=") = Line.Length - 1 Then
                        Value = String.Empty
                    Else
                        Value = Line.Substring(Line.IndexOf("=") + 1, Line.Length - (Line.IndexOf("=") + 1)).Trim
                    End If
                    'Add the valu to 
                    SectionValues.Add(Key.ToUpper, Value)
                End If
            Next

            Rd.Close()
            Rd.Dispose()
            Rd = Nothing

        End If
    End Sub

    Public Function GetValue(ByVal Section As String, ByVal Name As String) As String

        If _Sections.ContainsKey(Section.ToUpper) Then
            Dim SectionValues As Dictionary(Of String, String) = Nothing
            SectionValues = _Sections(Section.ToUpper)
            If SectionValues.ContainsKey(Name.ToUpper) Then
                Return SectionValues(Name.ToUpper)
            End If
        End If

        Return Nothing 'if preferred return String.empty here

    End Function

    'Public Function SetValue(ByVal Section As String, ByVal Name As String, ByVal Value As String, Optional ByVal Save As Boolean = False) As Boolean
    '    Dim SectionValues As Dictionary(Of String, String) = Nothing
    '    Name = Name.ToUpper.Trim
    '    Section = Section.ToUpper.Trim
    '    If _Sections.ContainsKey(Section) Then
    '        SectionValues = _Sections(Section)
    '        If SectionValues.ContainsKey(Name) Then
    '            SectionValues.Remove(Name)
    '        End If
    '        SectionValues.Add(Name, Value)
    '    Else
    '        SectionValues = New Dictionary(Of String, String)
    '        _Sections.Add(Section, SectionValues)
    '        SectionValues.Add(Name, Value)
    '    End If

    '    If Save Then
    '        Return SaveIniFile()
    '    Else
    '        Return True
    '    End If

    'End Function


    'Public Function SaveIniFile() As Boolean

    '    Dim Rw As StreamWriter
    '    Dim SectionPair As KeyValuePair(Of String, Dictionary(Of String, String))
    '    Dim ValuePair As KeyValuePair(Of String, String)

    '    Dim Pth As String = Path.GetDirectoryName(_FileName)

    '    If Directory.Exists(Pth) Then
    '        Rw = New StreamWriter(_FileName, False)
    '        For Each SectionPair In _Sections
    '            Rw.WriteLine("[" & SectionPair.Key & "]")
    '            If SectionPair.Value IsNot Nothing Then
    '                For Each ValuePair In SectionPair.Value
    '                    Rw.WriteLine(ValuePair.Key & "=" & ValuePair.Value)
    '                Next
    '            End If
    '        Next
    '        Rw.WriteLine("")
    '        Rw.Flush()
    '        Rw.Close()
    '        Rw.Dispose()
    '        Rw = Nothing
    '        SaveIniFile = True
    '    End If

    'End Function

    'Function DeleteValue(ByVal Section As String, ByVal Name As String, Optional ByVal Save As Boolean = False) As Boolean

    '    Dim SectionValues As Dictionary(Of String, String) = Nothing

    '    Name = Name.ToUpper.Trim
    '    Section = Section.ToUpper.Trim
    '    If _Sections.ContainsKey(Section) Then
    '        SectionValues = _Sections(Section)
    '        If SectionValues.ContainsKey(Name) Then
    '            SectionValues.Remove(Name)
    '        End If
    '    End If

    '    If Save Then
    '        Return SaveIniFile()
    '    Else
    '        Return True
    '    End If

    'End Function

    'Function DeleteSection(ByVal Section As String, Optional ByVal Save As Boolean = False) As Boolean

    '    Dim SectionValues As Dictionary(Of String, String) = Nothing

    '    Section = Section.ToUpper.Trim
    '    If _Sections.ContainsKey(Section) Then
    '        _Sections.Remove(Section)
    '    End If

    '    If Save Then
    '        Return SaveIniFile()
    '    Else
    '        Return True
    '    End If

    'End Function


End Class