﻿


Imports System.IO
Imports System.Net

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports GateInspection
Imports System.Runtime.InteropServices

Imports System.IO.Ports
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Timers

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

    Private WithEvents objCamera1 As New Camera 'Top View
    Private WithEvents objCamera2 As New Camera 'Side View
    Dim vCamera2Index As Boolean

    Dim vCenter As Boolean
    Dim vShowImage As Boolean

    Dim sensorPort As SerialPort
    Dim vPortName As String
    Dim iSensorDelay As Integer = 0

    Dim vCurrentImagePath As String = ""
    'Dim vReConnectTimer As New Timer
    Dim vReConnectTimer As New Timer

    Private Sub Button1_Click(sender As Object, e As EventArgs)

        AxVLCPlugin21.playlist.add("rtsp://gate:Gateview2018@" & vCameraIp_1 & "/Streaming/Channels/2")
        AxVLCPlugin21.playlist.playItem(0)

    End Sub

    Private Sub btnDownload_Click(sender As Object, e As EventArgs)

    End Sub

    'Function save_image_to_storage() As String
    '    '1)List images
    '    Dim files() As String
    '    Dim FileName As String
    '    Dim fileCreatedDate As DateTime

    '    If vPath = "" Then
    '        Exit Function
    '    End If

    '    files = Directory.GetFiles(Application.StartupPath, "main*.png", SearchOption.TopDirectoryOnly)

    '    If files.Count = 0 Then
    '        Exit Function
    '    End If
    '    FileName = files(0)
    '    fileCreatedDate = File.GetLastWriteTime(FileName) '  .GetCreationTime(FileName)

    '    Dim vYear, vMonth, vDay, vHour As String
    '    vYear = fileCreatedDate.Year.ToString()
    '    vMonth = fileCreatedDate.Month.ToString("00")
    '    vDay = fileCreatedDate.Day.ToString("00")
    '    vHour = fileCreatedDate.Hour.ToString("00") & fileCreatedDate.Minute.ToString("00")
    '    '2)Created Folder on Storage
    '    Dim vNewFolder As String = vPath & vYear & "\" & vMonth & "\" & vDay & "\" &
    '                            vStationName & "\" & vHour
    '    If Directory.Exists(vNewFolder) = False Then
    '        Directory.CreateDirectory(vNewFolder)
    '    End If
    '    '3)Move file to Storage
    '    Dim vOnlyFilename As String = ""
    '    For Each FileName In files
    '        vOnlyFilename = Path.GetFileName(FileName)
    '        If File.Exists(vNewFolder & "\" & vOnlyFilename) Then
    '            File.Delete(vNewFolder & "\" & vOnlyFilename)
    '        End If
    '        My.Computer.FileSystem.MoveFile(FileName, vNewFolder & "\" & vOnlyFilename)
    '    Next
    '    Return vNewFolder
    '    '4)Delete images prefix with original*
    '    'Found error on some process open some file.
    '    'files = Directory.GetFiles(Application.StartupPath, "original_*.png", SearchOption.TopDirectoryOnly)

    '    'If files.Count > 0 Then
    '    '    For Each FileName In files
    '    '        File.Delete(FileName)
    '    '    Next
    '    'End If

    'End Function


    Sub save_image_to_mostupdate()
        Try
            '1)List images
            Dim files() As String
            Dim FileName As String

            If vPath = "" Then
                Exit Sub
            End If

            files = Directory.GetFiles(Application.StartupPath, "original*.png", SearchOption.TopDirectoryOnly)

            If files.Count = 0 Then
                Exit Sub
            End If


            files = Directory.GetFiles(Application.StartupPath, "original*.png", SearchOption.TopDirectoryOnly)
            FileName = files(0)

            Dim vMostUpdatePath As String
            vMostUpdatePath = vPath & "mostupdate\" & vStationName

            'Delete all file
            For Each deleteFile In Directory.GetFiles(vMostUpdatePath, "*.*", SearchOption.TopDirectoryOnly)
                File.Delete(deleteFile)
            Next

            '3)Move file to Storage
            Dim vOnlyFilename As String = ""
            For Each FileName In files
                vOnlyFilename = Path.GetFileName(FileName)
                My.Computer.FileSystem.CopyFile(FileName, vMostUpdatePath & "\" & vOnlyFilename)
            Next
        Catch ex As Exception
            MsgBox("Error on saving file to most update folder :" & vbCrLf &
                    ex.Message)
        End Try


    End Sub

    Sub save_image_to_storage()
        Try
            '1)List images
            Dim files() As String
            Dim FileName As String
            Dim fileCreatedDate As DateTime

            If vPath = "" Then
                Exit Sub
            End If

            files = Directory.GetFiles(Application.StartupPath, "original*.png", SearchOption.TopDirectoryOnly)

            If files.Count = 0 Then
                Exit Sub
            End If

            'resise first
            'Add on Nov 22,2018 , version 1.0.0.18
            'To resize before save to server 
            For Each FileName In files
                resize_file(FileName)
            Next
            '------------
            files = Directory.GetFiles(Application.StartupPath, "main*.png", SearchOption.TopDirectoryOnly)
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

            vCurrentImagePath = vNewFolder

            '3)Move file to Storage
            Dim vOnlyFilename As String = ""
            For Each FileName In files
                vOnlyFilename = Path.GetFileName(FileName)
                If File.Exists(vNewFolder & "\" & vOnlyFilename) Then
                    File.Delete(vNewFolder & "\" & vOnlyFilename)
                End If
                My.Computer.FileSystem.MoveFile(FileName, vNewFolder & "\" & vOnlyFilename)
            Next

            '4)Delete images prefix with original*
            'Found error on some process open some file.
            files = Directory.GetFiles(Application.StartupPath, "original_*.png", SearchOption.TopDirectoryOnly)

            If files.Count > 0 Then
                For Each FileName In files
                    File.Delete(FileName)
                Next
            End If
        Catch ex As Exception
            MsgBox("Error on saving file to storage :" & vbCrLf &
                    ex.Message)
        End Try


    End Sub


    Private Sub start_capture(url As String, delay As Integer, capture As Integer)
        Try



            Dim tClient As New System.Net.WebClient
            tClient.Credentials = New System.Net.NetworkCredential("gate", "Gateview2018")
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

    Sub reConnectSensorPort(vPort As String)
        Try
            If sensorPort.IsOpen Then
                Exit Try
            Else
                sensorPort.Open()
            End If
            'sensorPort = New SerialPort(vPort)
            'With sensorPort
            '    .BaudRate = 9600
            '    .Parity = Parity.None
            '    .StopBits = StopBits.One
            '    .DataBits = 8
            '    .Handshake = Handshake.None
            '    AddHandler .DataReceived, AddressOf DataReceviedHandler
            '    .Open()
            'End With
        Catch ex As Exception
            lblSensor.BackColor = Color.Red
            lblSensor.Text = vPort & " error." : Application.DoEvents()
        End Try

    End Sub

    Private Sub DataReceviedHandler(sender As Object, e As SerialDataReceivedEventArgs)
        Dim sp As SerialPort = CType(sender, SerialPort)
        Dim indata As String = sp.ReadLine() '  .ReadExisting() 
        indata = indata.Replace(vbCr, "")
        displaySensorData(indata)

        If indata = "1" Or indata = "M" Then
            'Added by Chutchai on Oct 9,2018
            'To dalay after capture "1" from COM port.
            If indata = "1" Then
                Threading.Thread.Sleep(iSensorDelay)
            End If
            btnAuto1_Click(sender, e)
        End If

        If indata = "S" Then
            btnCapture2_Click(sender, e)
        End If

    End Sub

    Private Sub displaySensorData(ByVal vData As String)
        If lblSensor.InvokeRequired Then
            lblSensor.Invoke(New Action(Of String)(AddressOf displaySensorData), vData)
        Else
            lblSensor.Text = vData
            If vData = "1" Then
                lblSensor.BackColor = Color.Red
                lblSensor.Text = "Arrived"

            ElseIf vData = "0" Then
                lblSensor.BackColor = Color.Red
                lblSensor.Text = "Leaved"

            ElseIf vData = "S" Then
                lblSensor.BackColor = Color.Green
                lblSensor.Text = "Single Capture"
            ElseIf vData = "M" Then
                lblSensor.BackColor = Color.Blue
                lblSensor.Text = "Multiple Capture"
            Else
                lblSensor.BackColor = Color.Yellow
                lblSensor.Text = "Waiting (" & Now.Second.ToString & ")"
            End If
        End If
    End Sub


    Function savePicture(index As Integer, tImage As Bitmap,
                            Optional Height As Integer = 0,
                            Optional Width As Integer = 0,
                            Optional show As Boolean = False,
                            Optional center As Boolean = False,
                            Optional file_prefix As String = "") As MyPictureBox
        'Method#2
        Dim imageViewer As MyPictureBox = New MyPictureBox

        If show Then
            ShowImageToPictureBox(tImage)
            'PictureBox1.Image = tImage
            'PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        End If

        If center Then
            PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
        End If



        Dim vOriginalFile As String = "original_" & file_prefix & "-" & "image" & index.ToString & ".png"

        tImage.Save(vOriginalFile,
                    System.Drawing.Imaging.ImageFormat.Png)

        '---
        ' Dim a = New Bitmap(My.Application.Info.DirectoryPath & "\" & file_prefix & "-" & "image" & index.ToString & ".png")


        imageViewer.fileName = vOriginalFile
        imageViewer.Image = tImage
        imageViewer.SizeMode = PictureBoxSizeMode.Zoom
        imageViewer.center = center
        AddHandler imageViewer.Click, AddressOf PictureBoxClick


        imageViewer.Height = Height 'FlowLayoutPanel1.Height - 20
        imageViewer.Width = Width 'FlowLayoutPanel1.Width / 7
        'lblCapture.Text = Width'"Capture picture #" & index.ToString

        ''Add on Nov 17,2018
        ''By Chutchai s
        ''Resize image file by using new Threading
        'comment on Nov 22,2018 -- found some error on threading(1.0.0.18)
        'Dim t1 As New Threading.Thread(AddressOf resize_file)
        't1.Start(vOriginalFile)
        ''----------------------------------------

        Return imageViewer
    End Function

    Sub resize_file(vOriginalFile As String)
        Try
            'Resize picture and Delete Original file

            Dim FileStream1 As New System.IO.FileStream(vOriginalFile, IO.FileMode.Open, IO.FileAccess.Read)
            Dim original As Image = Image.FromStream(FileStream1)
            'PictureBox1.Image = MyImage



            'Dim original As Image = Image.FromFile(vOriginalFile)
            Dim resized As Image = ResizeImage(original, New Size(1024, 768))
            Dim memStream As MemoryStream = New MemoryStream()
            resized.Save(vOriginalFile.Replace("original_", ""), ImageFormat.Png)
            'File.Delete(file_prefix & "-" & "image" & index.ToString & ".png")
            memStream.Dispose()
            memStream.Close()
            '----------------------------------------
            FileStream1.Dispose()
            FileStream1.Close()
        Catch ex As Exception
            MsgBox("Error on resize picture function" & vbCrLf &
                    ex.Message)
        End Try

    End Sub

    Public Shared Function ResizeImage(ByVal image As Image,
  ByVal size As Size, Optional ByVal preserveAspectRatio As Boolean = True) As Image
        Dim newWidth As Integer
        Dim newHeight As Integer
        If preserveAspectRatio Then
            Dim originalWidth As Integer = image.Width
            Dim originalHeight As Integer = image.Height
            Dim percentWidth As Single = CSng(size.Width) / CSng(originalWidth)
            Dim percentHeight As Single = CSng(size.Height) / CSng(originalHeight)
            Dim percent As Single = If(percentHeight < percentWidth,
                percentHeight, percentWidth)
            newWidth = CInt(originalWidth * percent)
            newHeight = CInt(originalHeight * percent)
        Else
            newWidth = size.Width
            newHeight = size.Height
        End If
        Dim newImage As Image = New Bitmap(newWidth, newHeight)
        Using graphicsHandle As Graphics = Graphics.FromImage(newImage)
            graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic
            graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight)
        End Using
        Return newImage
    End Function


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
            ShowImageToPictureBox(tImage)
            'PictureBox1.Image = tImage
            'PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        End If

        If center Then
            PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
        End If

        tImage.Save(file_prefix & "-" & "image" & index.ToString & ".png",
                    System.Drawing.Imaging.ImageFormat.Png)

        imageViewer.fileName = file_prefix & "-" & "image" & index.ToString & ".png"
        imageViewer.Image = tImage
        imageViewer.SizeMode = PictureBoxSizeMode.Zoom
        imageViewer.center = center
        AddHandler imageViewer.Click, AddressOf PictureBoxClick


        imageViewer.Height = Height 'FlowLayoutPanel1.Height - 20
        imageViewer.Width = Width 'FlowLayoutPanel1.Width / 7
        'lblCapture.Text = Width'"Capture picture #" & index.ToString

        Return imageViewer
    End Function


    Private Sub PictureBoxClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As MyPictureBox = DirectCast(sender, MyPictureBox)
        'MsgBox("Click working " & btn.Image.ToString)
        ' lblCapture.Text = btn.fileName
        PictureBox1.Image = btn.Image
        If btn.center Then
            PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
        Else
            PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
            PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        End If


    End Sub

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndInsertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As Integer) As Boolean
    End Function

    Private Const SWP_NOSIZE As Integer = &H1
    Private Const SWP_NOMOVE As Integer = &H2

    Private Shared ReadOnly HWND_TOPMOST As New IntPtr(-1)
    Private Shared ReadOnly HWND_NOTOPMOST As New IntPtr(-2)

    Public Function MakeTopMost(hw As IntPtr)
        SetWindowPos(hw, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE Or SWP_NOSIZE)
        Return 0
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load



        Dim fName As String = ("system.ini")              'path to text file
        Dim My_Ini As New Ini(fName)



        vCameraIp_1 = My_Ini.GetValue("Camera1", "IP")

        'vCameraCaptureUrl_1 = "http://gate:Gateview2018@" & vCameraIp_1 & "/Streaming/Channels/2/picture"
        vCameraCaptureUrl_1 = "http://" & vCameraIp_1 & "/Streaming/Channels/1/picture"


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

        'Added by Chutchai on Sep 25,2018
        'To delete older folder (15 days)
        Dim iDay As Integer = 0
        Dim sDay As String = ""
        sDay = My_Ini.GetValue("storage", "day")
        If sDay = "" Then
            iDay = 15
        Else
            iDay = Val(sDay)
        End If
        'Comment by CHutchai on Nov 21,2018
        'To delete older files will do by manually.
        'DeleteFolderOlder15Day(vPath, vStationName, iDay)
        '-----------------------------------------
        DeletePictureFiles()

        'My_Ini.DeleteValue("NewSection", "TestValue", True)
        'My_Ini.DeleteSection("NewSection", True)
        'My_Ini.SetValue("Camera1", "IP", "230", True)
        'Me.Text = Me.Text

        Dim versionNumber As Version

        versionNumber = Assembly.GetExecutingAssembly().GetName().Version
        Me.Text = Me.Text & " version :" & versionNumber.ToString


        objCamera1.ip = vCameraIp_1
        objCamera1.delay = vCameraDelay_1

        objCamera2.ip = vCameraIp_2
        objCamera2.delay = vCameraDelay_2


        'Enable Sensor Port
        vPortName = My_Ini.GetValue("sensor", "port")
        If vPortName = "" Then
            vPortName = "COM5"
        End If



        'enableSensorPort(vPortName)
        Dim vSensorDelay As String
        vSensorDelay = My_Ini.GetValue("sensor", "delay")
        If vSensorDelay = "" Then
            iSensorDelay = 0
        Else
            iSensorDelay = Int(vSensorDelay)
        End If
        'end Port

        'Added by CHutchai S on Jan 2,2019
        'To enable auto reconnect to comport function
        sensorPort = New SerialPort(vPortName)
        With sensorPort
            .BaudRate = 9600
            .Parity = Parity.None
            .StopBits = StopBits.One
            .DataBits = 8
            .Handshake = Handshake.None
            AddHandler .DataReceived, AddressOf DataReceviedHandler
            .Open()
        End With
        vReConnectTimer.Interval = 2000
        AddHandler vReConnectTimer.Elapsed, New ElapsedEventHandler(AddressOf reConnectSensor)
        vReConnectTimer.Start()
    End Sub

    Sub reConnectSensor()
        Try
            vReConnectTimer.Stop()
            reConnectSensorPort(vPortName)
        Catch ex As Exception
            ShowPortReconnectStatus("Disconnect(" & Now.Second.ToString & ")")
        End Try
        vReConnectTimer.Start()
    End Sub

    Private Sub ShowPortReconnectStatus(ByVal vStatus As String)

        If lblSensor.InvokeRequired Then
            lblSensor.Invoke(New Action(Of String)(AddressOf ShowPortReconnectStatus), vStatus)
            lblSensor.BackColor = Color.Red
        Else
            lblSensor.Text = vStatus
            lblSensor.BackColor = Color.Red
        End If

    End Sub

    Sub setCTCStoAlwaysTop()
        '---Test for Set On Top for CTCS window.
        Dim processes() As Process
        Dim instance As Process
        Dim process As New Process()
        processes = Process.GetProcesses
        For Each instance In processes
            If instance.ProcessName = "pcsws" Then
                ''something here
                SetWindowPos(instance.MainWindowHandle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE Or SWP_NOSIZE)
            End If
        Next
    End Sub


    Private Sub DeletePictureFiles()
        Try
            Dim files() As String
            files = Directory.GetFiles(".", "*.png", SearchOption.TopDirectoryOnly)
            For Each FileName As String In files
                Dim fileExists As Boolean = File.Exists(FileName)
                If fileExists Then
                    File.Delete(FileName)
                End If
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error Deleting Folder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DeleteFolderOlder15Day(sDirectory As String,
                                       sBooth As String,
                                       Optional iDay As Integer = 15)
        Try

            Dim strFile As String = "delete_log.txt"
            Dim fileExists As Boolean = File.Exists(strFile)
            If fileExists Then
                File.Delete(strFile)
            End If
            Using sw As New StreamWriter(File.Open(strFile, FileMode.OpenOrCreate))
                '    sw.WriteLine(
                '        IIf(fileExists,
                '"Error Message in  Occured at-- " & DateTime.Now,
                '"Start Error Log for today"))


                Dim dtToday As DateTime = Today.Date
                Dim dt30day As DateTime = Today.Date.AddDays(-60)
                Dim dtCurrent As DateTime
                Dim strCurrentPath As String = ""
                Dim diObj As DirectoryInfo

                For i = 0 To (60 - iDay - 1)
                    dtCurrent = dt30day.AddDays(i)
                    strCurrentPath = sDirectory + dtCurrent.Year.ToString + "\" +
                        dtCurrent.Month.ToString("00") + "\" + dtCurrent.Day.ToString("00") + "\" + sBooth
                    diObj = New DirectoryInfo(strCurrentPath)

                    If diObj.Exists Then
                        diObj.Delete(True) 'True for recursive deleting
                        sw.WriteLine(strCurrentPath & "--> Deleted")
                    Else
                        sw.WriteLine(strCurrentPath & "--> Not Existing")
                    End If
                Next

            End Using

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error Deleting Folder", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    'Private Sub DeleteFolderOlder15Day(sDirectory As String)
    '    Try
    '        Dim dtCreated As DateTime
    '        Dim dtToday As DateTime = Today.Date
    '        Dim diObj As DirectoryInfo
    '        Dim ts As TimeSpan
    '        Dim lstDirsToDelete As New List(Of String)

    '        For Each sSubDir As String In Directory.GetDirectories(sDirectory)
    '            diObj = New DirectoryInfo(sSubDir)
    '            dtCreated = diObj.CreationTime

    '            ts = dtToday - dtCreated

    '            'Add whatever storing you want here for all folders...

    '            If ts.Days > 10 Then
    '                lstDirsToDelete.Add(sSubDir)
    '                'Store whatever values you want here... like how old the folder is
    '                diObj.Delete(True) 'True for recursive deleting
    '            End If
    '        Next
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "Error Deleting Folder", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

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
        'tClient.Credentials = New System.Net.NetworkCredential("gate", "Gateview2018")
        Dim imageViewer As MyPictureBox = New MyPictureBox
        imageViewer = capturePicture(FlowLayoutPanel1.Controls.Count + 1, tClient, vCameraCaptureUrl_1)
        FlowLayoutPanel1.Controls.Add(imageViewer)
        FlowLayoutPanel1.ScrollControlIntoView(imageViewer)
    End Sub

    Private Sub btnLive1_Click(sender As Object, e As EventArgs) Handles btnLive1.Click
        AxVLCPlugin21.playlist.add("rtsp://gate:Gateview2018@" & vCameraIp_1 & "/Streaming/Channels/2")
        AxVLCPlugin21.playlist.playItem(0)
        Me.ActiveControl = Nothing
    End Sub

    Private Sub btnLive2_Click(sender As Object, e As EventArgs) Handles btnLive2.Click
        AxVLCPlugin22.playlist.add("rtsp://gate:Gateview2018@" & vCameraIp_2 & "/Streaming/Channels/2")
        AxVLCPlugin22.playlist.playItem(0)
        Me.ActiveControl = Nothing
    End Sub

    Private Sub btnCapture1_Click(sender As Object, e As EventArgs) Handles btnCapture1.Click
        'Using new Camera class
        vShowImage = True
        objCamera1.CapturesAsync(1)
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom

        Me.ActiveControl = Nothing
        'Older Style
        'captureOneShort(vCameraCaptureUrl_1, chkShowCaptured.Checked)
        'lblCapture.Text = "Capture picture #" & FlowLayoutPanel1.Controls.Count.ToString
    End Sub

    Sub captureOneShort(url As String, Optional show As Boolean = False,
                        Optional file_prefix As String = "top")
        Dim tClient As New System.Net.WebClient
        ' tClient.Credentials = New System.Net.NetworkCredential("admin", "Autogate2018")
        tClient.Credentials = New System.Net.NetworkCredential("gate", "Gateview2018")
        Dim imageViewer As MyPictureBox = New MyPictureBox
        imageViewer = capturePicture(FlowLayoutPanel1.Controls.Count + 1, tClient, url,
                                     FlowLayoutPanel1.Height - 20, FlowLayoutPanel1.Width / (vCameraCapture_1 + 4),
                                     show, False, file_prefix)
        FlowLayoutPanel1.Controls.Add(imageViewer)
        FlowLayoutPanel1.ScrollControlIntoView(imageViewer)

        tClient.Dispose()
    End Sub

    Private Sub AddImageToPanel1(ByVal image As MyPictureBox)

        If FlowLayoutPanel1.InvokeRequired Then
            FlowLayoutPanel1.Invoke(New Action(Of MyPictureBox)(AddressOf AddImageToPanel1), image)
        Else
            FlowLayoutPanel1.Controls.Add(image)
        End If
        FlowLayoutPanel1.ScrollControlIntoView(image)
    End Sub

    Private Sub ShowImageToPictureBox(ByVal image As Bitmap)

        If PictureBox1.InvokeRequired Then
            PictureBox1.Invoke(New Action(Of Bitmap)(AddressOf ShowImageToPictureBox), image)
        Else
            PictureBox1.Image = image
            PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        End If

    End Sub

    'Sub captureOneShort2(url As String, Optional show As Boolean = False,
    '                     Optional center As Boolean = False,
    '                     Optional file_prefix As String = "sub")
    '    Dim tClient As New System.Net.WebClient
    '    tClient.Credentials = New System.Net.NetworkCredential("admin", "Autogate2018")
    '    Dim imageViewer As MyPictureBox = New MyPictureBox
    '    imageViewer = capturePicture(Now.Second, tClient, url,
    '                                 FlowLayoutPanel2.Height - 5, FlowLayoutPanel2.Width / 3, show, center, file_prefix)
    '    FlowLayoutPanel2.Controls.Add(imageViewer)
    '    FlowLayoutPanel2.ScrollControlIntoView(imageViewer)
    '    tClient.Dispose()
    'End Sub

    Private Sub btnCapture2_Click(sender As Object, e As EventArgs) Handles btnCapture2.Click


        'Dim remoteUri As String = "http://192.168.103.11/Streaming/Channels/1/picture"
        'Dim fileName As String = "materi.zip"
        'Dim password As String = "Gateview2018"
        'Dim username As String = "gate"

        'Dim client As New WebClient()

        'client.Credentials = New NetworkCredential(username, password)
        'client.DownloadFile(remoteUri, "test.png")

        'Threading
        vCenter = False
        objCamera2.CapturesAsync(1)

        Me.ActiveControl = Nothing

    End Sub




    Private Sub btnAuto1_Click(sender As Object, e As EventArgs) Handles btnAuto1.Click

        '-----Start to save all images to Storage---
        'save_image_to_storage()
        '-------------------------------------------
        clearFlowLayout(Nothing)
        'FlowLayoutPanel1.Controls.Clear()

        SetCamera1AutoBtnText("Auto(0)")
        objCamera1.CapturesAsync(vCameraCapture_1)

        Me.ActiveControl = Nothing

        ' start_capture(vCameraCaptureUrl_1, vCameraDelay_1, vCameraCapture_1)
    End Sub

    Private Sub clearFlowLayout(ByVal image As Bitmap)

        If FlowLayoutPanel1.InvokeRequired Then
            'PictureBox1.Invoke(New Action(Of Bitmap)(AddressOf ShowImageToPictureBox), image)
            FlowLayoutPanel1.Invoke(New Action(Of Bitmap)(AddressOf clearFlowLayout), image)
        Else
            FlowLayoutPanel1.Controls.Clear()
        End If

    End Sub

    Private Sub objCamera1_DownloadCompleted(ByVal sender As Object, ByVal e As DownloadCompletedEventArgs) Handles objCamera1.downloadCompleted


        'save_image_to_mostupdate()
        '-----Start to save all images to Storage---
        'save_image_to_storage()

        'FOund error after finished on version 1.0.0.8
        'Rollback to original function/
        'Dim t1 As New Threading.Thread(AddressOf save_image_to_storage)
        't1.Start()
        '-------------------------------------------
        Dim startTime As DateTime = DateTime.Now
        'Save to most update
        If chkSaveMostUpdate.Checked Then
            Dim t1 As New Threading.Thread(AddressOf save_image_to_mostupdate)
            t1.Start()
        End If

        Dim t2 As New Threading.Thread(AddressOf save_image_to_storage)
        t2.Start()
        Dim duration As TimeSpan = DateTime.Now - startTime

        SetCamera1LabelText(e.Message & " Saved :" & (duration.TotalSeconds).ToString + " Sec(s)")

    End Sub



    Private Sub btnCenter_Click(sender As Object, e As EventArgs) Handles btnCenter.Click
        'If FlowLayoutPanel2.Controls.Count = 2 Then
        '    FlowLayoutPanel2.Controls.RemoveAt(0)
        'End If
        'captureOneShort2(vCameraCaptureUrl_2, True, True)
        'lblCount2.Text = "Capture picture #" & FlowLayoutPanel2.Controls.Count.ToString
        'objCamera2.CaptureAsync()
        vCenter = True
        objCamera2.CapturesAsync(1)
        PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage

        Me.ActiveControl = Nothing
    End Sub

    Private Sub SetCamera1AutoBtnText(ByVal text As String)
        If btnAuto1.InvokeRequired Then
            btnAuto1.Invoke(New Action(Of String)(AddressOf SetCamera1AutoBtnText), text)
        Else
            btnAuto1.Text = text
        End If
    End Sub

    Private Sub SetCamera1LabelText(ByVal text As String)
        If lblCapture.InvokeRequired Then
            lblCapture.Invoke(New Action(Of String)(AddressOf SetCamera1LabelText), text)
        Else
            lblCapture.Text = text
        End If
    End Sub

    Private Sub SetCamera2LabelText(ByVal text As String)
        If lblCount2.InvokeRequired Then
            lblCount2.Invoke(New Action(Of String)(AddressOf SetCamera2LabelText), text)
        Else
            lblCount2.Text = text
        End If
    End Sub


    Private Sub objCamera2_downloadCompleted(sender As Object, e As DownloadCompletedEventArgs) Handles objCamera2.downloadCompleted

        SetCamera2LabelText(e.Message)

        ' PictureBox1.Image = e.image
        'AddImageToPictureBox(e.image)


        ''FlowLayoutPanel2.Controls.Add(imageViewer)
        ''FlowLayoutPanel2.ScrollControlIntoView(imageViewer)

        ''lblCount2.Text = "Capture picture #" & FlowLayoutPanel2.Controls.Count.ToString
        'vCamera2Index = Not vCamera2Index
    End Sub



    Private Sub AddImageToPictureBox(ByVal image As Bitmap)

        If PictureBox1.InvokeRequired Then
            PictureBox1.Invoke(New Action(Of Bitmap)(AddressOf AddImageToPictureBox), image)
        Else
            PictureBox1.Image = image
        End If

    End Sub

    Private Sub AddImageToPanel2(ByVal image As MyPictureBox)

        If FlowLayoutPanel2.InvokeRequired Then
            FlowLayoutPanel2.Invoke(New Action(Of MyPictureBox)(AddressOf AddImageToPanel2), image)
        Else
            FlowLayoutPanel2.Controls.Add(image)
        End If
        FlowLayoutPanel2.ScrollControlIntoView(image)
    End Sub


    'Private Sub PictureBox1_MouseWheel(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseWheel
    '    If e.Delta <> 0 Then
    '        If e.Delta <= 0 Then
    '            If PictureBox1.Width < 500 Then Exit Sub 'minimum 500?
    '        Else
    '            If PictureBox1.Width > 2000 Then Exit Sub 'maximum 2000?
    '        End If

    '        PictureBox1.Width += CInt(PictureBox1.Width * e.Delta / 1000)
    '        PictureBox1.Height += CInt(PictureBox1.Height * e.Delta / 1000)
    '    End If
    'End Sub

    Private Sub objCamera2_DownloadChanged(sender As Object, e As DownloadChangedEventArgs) Handles objCamera2.DownloadChanged

        AddImageToPictureBox(e.image)

        If FlowLayoutPanel2.Controls.Count = 2 Then
            FlowLayoutPanel2.Controls.RemoveAt(0)
        End If

        Dim vImage As Bitmap = e.image
        Dim imageViewer As MyPictureBox = New MyPictureBox
        'FlowLayoutPanel1.Controls.Count + 1
        'IIf(vCamera2Index, 1, 0) -- Original
        imageViewer = savePicture(IIf(vCamera2Index, 100, 101), e.image,
                                     FlowLayoutPanel2.Height - 5, FlowLayoutPanel2.Width / 3,
                                       True, vCenter, "main")

        SetCamera2LabelText(imageViewer.fileName)
        AddImageToPanel2(imageViewer)
        vCamera2Index = Not vCamera2Index
    End Sub

    Private Sub objCamera1_DownloadChanged(sender As Object, e As DownloadChangedEventArgs) Handles objCamera1.DownloadChanged

        SetCamera1AutoBtnText("Auto(" & e.CurrentCount & ")")

        'Edit by Chutchai on Oct 6 ,2018
        'To show first captured picture on view , if "Show capture" is checked.
        'On version 1.0.0.13

        If chkShowCaptured.Checked Or vShowImage Then
            If e.CurrentCount = 1 Then
                AddImageToPictureBox(e.image)
            End If

            vShowImage = False
        End If

        'If chkAllPic.Checked Then
        '    AddImageToPictureBox(e.image)
        'End If

        If chkShowLast.Checked And e.CurrentCount = vCameraCapture_1 Then
            AddImageToPictureBox(e.image)
        End If


        Dim imageViewer As MyPictureBox = New MyPictureBox
        imageViewer = savePicture(FlowLayoutPanel1.Controls.Count + 1, e.image,
                                     FlowLayoutPanel1.Height - 20,
                                  FlowLayoutPanel1.Width / (vCameraCapture_1 + 4),
                                     (chkShowCaptured.Checked And e.CurrentCount = 1) Or
                                  (chkShowLast.Checked And e.CurrentCount = vCameraCapture_1) Or
                                  chkAllPic.Checked, False, "main")

        SetCamera1LabelText(imageViewer.fileName)
        AddImageToPanel1(imageViewer)



    End Sub

    Private Sub llSetToTop_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles llSetToTop.LinkClicked
        setCTCStoAlwaysTop()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        frmViewPicture.Show()
    End Sub

    'Private Sub lkPlayall_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lkPlayall.LinkClicked
    '    Dim img As Object
    '    For Each img In FlowLayoutPanel1.Controls

    '        ShowImageToPictureBox(img.image)
    '        Application.DoEvents()
    '        Threading.Thread.Sleep(1000)
    '    Next
    'End Sub

    Private Sub btnPlayAll_Click(sender As Object, e As EventArgs) Handles btnPlayAll.Click
        Dim img As Object
        For Each img In FlowLayoutPanel1.Controls

            ShowImageToPictureBox(img.image)
            Application.DoEvents()
            Threading.Thread.Sleep(1000)
        Next
    End Sub
End Class


Public Class Camera

    Private context As Threading.SynchronizationContext = Threading.SynchronizationContext.Current

    Dim _ip As String
    Dim _delay As Integer = 100
    Dim _short_number As Integer = 7
    Dim _image As Bitmap
    Private vUrl As String = ""


    Public Property ip() As String
        Get
            Return _ip
        End Get
        Set(ByVal value As String)
            _ip = value
            'Comment on Apr 21,2020
            'IE doesn't support url with User password
            'vUrl = "http://gate:Gateview2018@" & _ip & "/Streaming/Channels/2/picture"

            vUrl = "http://" & _ip & "/Streaming/Channels/1/picture"
        End Set
    End Property
    Public Property delay() As Integer
        Get
            Return _delay
        End Get
        Set(ByVal value As Integer)
            _delay = value
        End Set
    End Property
    Public Property short_number() As Integer
        Get
            Return _short_number
        End Get
        Set(ByVal value As Integer)
            _short_number = value
        End Set
    End Property

    Public ReadOnly Property image() As Bitmap
        Get
            Return _image
        End Get
    End Property

    Public Sub New(Optional ip As String = "")
        _ip = ip
        'IE doesn't support url with User password
        'vUrl = "http://gate:Gateview2018@" & _ip & "/Streaming/Channels/2/picture"
        'vUrl = "http://gate:Gateview2018@" & _ip & "/Streaming/Channels/2/picture"
        vUrl = "http://" & _ip & "/Streaming/Channels/1/picture"
    End Sub

    Public Event DownloadChanged As EventHandler(Of DownloadChangedEventArgs)
    Protected Overridable Sub OnDownloadChanged(ByVal e As DownloadChangedEventArgs)
        RaiseEvent DownloadChanged(Me, e)
    End Sub

    Public Event downloadCompleted As EventHandler(Of DownloadCompletedEventArgs)
    Protected Overridable Sub OnDownloadCompleted(ByVal e As DownloadCompletedEventArgs)
        RaiseEvent downloadCompleted(Me, e)
    End Sub


    Function getSnapShort() As Bitmap
        Dim vImage As Bitmap
        Try
            'Dim e As DownloadCompletedEventArgs
            'e = New DownloadCompletedEventArgs(1, max)

            'Dim client As New WebClient()

            'client.Credentials = New NetworkCredential("gate", "Gateview2018")
            'Dim ImageInBytes2() As Byte = client.DownloadData("http://192.168.103.11/Streaming/Channels/1/picture")
            'client.DownloadFile("http://192.168.103.11/Streaming/Channels/1/picture", "test2.png")

            'Dim ImageInBytes2() As Byte = client.DownloadData(vUrl)
            'client.DownloadFile(vUrl, "test2.png")


            Dim tClient As New System.Net.WebClient

            tClient.Credentials = New System.Net.NetworkCredential("gate", "Gateview2018")

            Dim ImageInBytes() As Byte = tClient.DownloadData(vUrl)
            Dim ImageStream As New IO.MemoryStream(ImageInBytes)
            vImage = Bitmap.FromStream(ImageStream)
            tClient.Dispose()
        Catch ex As Exception
            vImage = Nothing
        End Try
        Return vImage
    End Function

    'Public Function capture(ByVal max As Integer) As String
    '    'Try

    '    '    Dim tClient As New System.Net.WebClient
    '    '    tClient.Credentials = New System.Net.NetworkCredential("admin", "Autogate2018")

    '    '    Dim ImageInBytes() As Byte = tClient.DownloadData(vUrl)
    '    '    Dim ImageStream As New IO.MemoryStream(ImageInBytes)
    '    '    _image = Bitmap.FromStream(ImageStream)
    '    '    tClient.Dispose()
    '    'Catch ex As Exception
    '    '    _image = Nothing
    '    'End Try
    '    _image = getSnapShort()
    '    OnDownloadCompleted(New DownloadCompletedEventArgs(_image))
    '    Return "ok"
    'End Function

    Public Function captures(ByVal max As Integer) As String
        Dim startTime As DateTime = DateTime.Now
        Dim e As DownloadChangedEventArgs
        Dim msg As String

        For i = 1 To max
            _image = getSnapShort()
            e = New DownloadChangedEventArgs(i, max, _image)

            If context Is Nothing Then
                OnDownloadChanged(e)
            Else
                ThreadExtensions.ScSend(context, New Action(Of DownloadChangedEventArgs)(AddressOf OnDownloadChanged), e)
            End If

            Threading.Thread.Sleep(_delay)
        Next
        Dim duration As TimeSpan = DateTime.Now - startTime
        msg = "Download : " + (duration.TotalSeconds).ToString + " Sec(s)"

        If context Is Nothing Then
            OnDownloadCompleted(New DownloadCompletedEventArgs(msg))
        Else
            ThreadExtensions.ScSend(context, New Action(Of DownloadCompletedEventArgs)(AddressOf OnDownloadCompleted), New DownloadCompletedEventArgs(msg))
        End If

        Return msg
    End Function


    'Public Sub CaptureAsync() 'Asynchronous 
    '    ThreadExtensions.QueueUserWorkItem(New Func(Of Integer, String)(AddressOf capture), 1)
    '    'System.Threading.ThreadPool.QueueUserWorkItem(AddressOf capture)
    'End Sub

    Public Sub CapturesAsync(max As Integer) 'Asynchronous 
        ThreadExtensions.QueueUserWorkItem(New Func(Of Integer, String)(AddressOf captures), max)
        'System.Threading.ThreadPool.QueueUserWorkItem(AddressOf capture)
    End Sub
End Class



Public Class ThreadExtensions
    Private args() As Object
    Private DelegateToInvoke As [Delegate]

    Public Shared Function QueueUserWorkItem(ByVal method As [Delegate], ByVal ParamArray args() As Object) As Boolean
        Return Threading.ThreadPool.QueueUserWorkItem(AddressOf ProperDelegate, New ThreadExtensions With {.args = args, .DelegateToInvoke = method})
    End Function

    Private Shared Sub ProperDelegate(ByVal state As Object)
        Dim sd As ThreadExtensions = DirectCast(state, ThreadExtensions)

        sd.DelegateToInvoke.DynamicInvoke(sd.args)
    End Sub

    Public Shared Sub ScSend(ByVal sc As Threading.SynchronizationContext, ByVal del As [Delegate], ByVal ParamArray args() As Object)
        sc.Send(New Threading.SendOrPostCallback(AddressOf ProperDelegate), New ThreadExtensions With {.args = args, .DelegateToInvoke = del})
    End Sub
End Class

Public Class DownloadChangedEventArgs
    Inherits EventArgs

    Private _CurrentCount As Integer
    Private _Max As Integer
    Private _Image As Bitmap

    Public Sub New(ByVal cc As Integer, ByVal max As Integer, ByVal image As Bitmap)
        _CurrentCount = cc
        _Max = max
        _Image = image
    End Sub
    Public ReadOnly Property CurrentCount() As Integer
        Get
            Return _CurrentCount
        End Get
    End Property
    Public ReadOnly Property Max() As Integer
        Get
            Return _Max
        End Get
    End Property

    Public ReadOnly Property image() As Bitmap
        Get
            Return _Image
        End Get
    End Property
End Class

Public Class DownloadCompletedEventArgs
    Inherits EventArgs
    Private _message As String
    Public Sub New(ByVal msg As String)
        _message = msg
    End Sub
    Public ReadOnly Property Message() As String
        Get
            Return _message
        End Get
    End Property

End Class

Public Class MyPictureBox
    Inherits PictureBox

    Private _fileName As String
    Private _center As Boolean

    Public Property fileName() As String
        Get
            Return _fileName
        End Get
        Set(ByVal value As String)
            _fileName = value
        End Set
    End Property

    Public Property center() As Boolean
        Get
            Return _center
        End Get
        Set(ByVal value As Boolean)
            _center = value
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