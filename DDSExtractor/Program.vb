Imports System
Imports System.IO
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Threading

Public Module DdsExtractor
    ' DDS �ļ�ͷ��ʶ
    Private ReadOnly DDS_HEADER As Byte() = {&H44, &H44, &H53, &H20} ' "DDS "
    Private ReadOnly POF_MARKER As String = "POF"
    Public Const Version As String = "v1.2.6"
    Dim currentPath As String = AppDomain.CurrentDomain.BaseDirectory
    Dim targetExePath As String = Path.Combine(currentPath, "DDSPatcher.exe")
    Dim FolderMode As Boolean = False
    Dim NoFolder As Boolean = False
    Dim Recursion As Boolean = False
    Dim OutputPathSetting As String = ""

    Sub Main()
        Console.ForegroundColor = ConsoleColor.DarkCyan
        Console.WriteLine($"DDS �ļ���ȡ���� {Version} by ChilorXN.")
        Console.ForegroundColor = ConsoleColor.DarkYellow
        Console.WriteLine("���Ϸ�Ҫ������ .afb �� .svo �ļ������ڣ��������ļ�·��(֧�ֶ���ļ�)")
        Console.WriteLine("��ʹ��SwitchMode�����л����ļ���ģʽ���Ϸ�/�����ļ���·��")
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine("���� 'Patcher' ����ͬĿ¼�µ�DDS�޲�����")
        Console.WriteLine("���� 'help' �鿴�������")
        Console.WriteLine("���� 'exit' �˳�����")

        ' ��������ѭ��
        While True
            Console.WriteLine()
            Console.ForegroundColor = ConsoleColor.Blue
            Console.Write("[Extractor]")
            Console.ForegroundColor = ConsoleColor.White
            If FolderMode = True Then
                Console.Write("(FolderMode)")
                If Recursion = True Then
                    Console.ForegroundColor = ConsoleColor.Green
                    Console.Write("(R)")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.Write("> ")
                Else
                    Console.Write("> ")
                End If
            Else
                Console.Write("> ")
            End If
            Dim input As String = Console.ReadLine()

            ' �����������
            Select Case input.Trim().ToLower()
                Case "patcher"
                    Console.WriteLine($"��ǰ·����{currentPath}")
                    If File.Exists(targetExePath) Then
                        Console.ForegroundColor = ConsoleColor.Green
                        Console.WriteLine("��������...")
                        Console.ForegroundColor = ConsoleColor.White
                        Try
                            ' ʹ��Process�������򣨲��ȴ��˳���
                            Dim processInfo As New ProcessStartInfo() With {
                                .FileName = targetExePath,
                                .UseShellExecute = True  ' ʹ��Shellִ�п��Ա�������
                            }

                            Process.Start(processInfo)
                            Console.WriteLine("�ѳ�������DDSPatcher")
                        Catch ex As Exception
                            Console.ForegroundColor = ConsoleColor.Red
                            Console.WriteLine($"����DDS�޲�����ʱ������{ex.Message}")
                            Console.ForegroundColor = ConsoleColor.White
                        End Try
                    Else
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.WriteLine("�����ڵ�ǰĿ¼��δ�ҵ�DDSPatcher.exe����ȷ�����Ƿ��Ѿ��������DDSExtractor���ڵ��ļ�����")
                        Console.ForegroundColor = ConsoleColor.White
                    End If
                    Continue While
                Case "switchmode", "switch"
                    If FolderMode = False Then
                        FolderMode = True
                        Console.ForegroundColor = ConsoleColor.Green
                        Console.WriteLine("���л����ļ���ģʽ�����Զ������ļ����ڵ�����afb/svo�ļ�")
                        Console.ForegroundColor = ConsoleColor.White
                    Else
                        FolderMode = False
                        Console.ForegroundColor = ConsoleColor.Green
                        Console.WriteLine("���л�������ģʽ")
                        Console.ForegroundColor = ConsoleColor.White
                    End If
                    Continue While
                Case "scanmode"
                    If FolderMode = True Then
                        If Recursion = False Then
                            If String.IsNullOrWhiteSpace(OutputPathSetting) Or Not Directory.Exists(OutputPathSetting) Then
                                Console.ForegroundColor = ConsoleColor.DarkYellow
                                Console.WriteLine("���棺�ù�����δ������ֲ��ԣ�Ϊ�����ƻ�ԭ�е�Ŀ¼�ṹ��������ʹ�� 'SetPath' ����������Ч�����·��")
                            End If
                            Recursion = True
                            Console.ForegroundColor = ConsoleColor.Green
                            Console.WriteLine("�����õݹ�ɨ�裬���Զ�������Ŀ¼�ڵ��ļ�")
                            Console.ForegroundColor = ConsoleColor.White
                        Else
                            Recursion = False
                            Console.ForegroundColor = ConsoleColor.Green
                            Console.WriteLine("��ͣ�õݹ�ɨ�裬����������ǰĿ¼�ڵ��ļ�")
                            Console.ForegroundColor = ConsoleColor.White
                        End If
                    Else
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.WriteLine("�����ļ���ģʽ�����ã�")
                        Console.ForegroundColor = ConsoleColor.White
                    End If
                    Continue While
                Case "setpath"
                    Console.ForegroundColor = ConsoleColor.DarkCyan
                    Console.WriteLine("���������·��")
                    Console.ForegroundColor = ConsoleColor.Blue
                    Console.Write("[Extractor]")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.Write("(SetPath)> ")
                    Dim PathSetting As String = Console.ReadLine
                    If String.IsNullOrWhiteSpace(PathSetting) Then
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.WriteLine("������·����")
                        Console.ForegroundColor = ConsoleColor.White
                    Else
                        If Not Directory.Exists(PathSetting) Then
                            Console.ForegroundColor = ConsoleColor.Red
                            Console.WriteLine("·�������ڣ�")
                            Console.ForegroundColor = ConsoleColor.White
                        Else
                            OutputPathSetting = PathSetting
                            PathSetting = ""
                            Console.ForegroundColor = ConsoleColor.Green
                            Console.WriteLine("���óɹ���")
                            Console.ForegroundColor = ConsoleColor.White
                        End If
                    End If
                    Continue While
                Case "outputmode"
                    If NoFolder = False Then
                        If Not String.IsNullOrWhiteSpace(OutputPathSetting) And Directory.Exists(OutputPathSetting) Then
                            NoFolder = True
                            Console.ForegroundColor = ConsoleColor.Green
                            Console.WriteLine("���ģʽ���óɹ�����ֱ������ļ�������Ϊÿ���ļ������������ļ���")
                            Console.ForegroundColor = ConsoleColor.White
                        Else
                            Console.ForegroundColor = ConsoleColor.Red
                            Console.WriteLine("����������Ч�����Ŀ¼��")
                            Console.ForegroundColor = ConsoleColor.White
                        End If
                    Else
                        NoFolder = False
                        Console.ForegroundColor = ConsoleColor.Green
                        Console.WriteLine("���ģʽ���óɹ�������Ϊÿ��afb/svo�ļ������������ļ���")
                        Console.ForegroundColor = ConsoleColor.White
                    End If
                    Continue While
                Case "clear"
                    Console.Clear()
                    Continue While
                Case "reset"
                    FolderMode = False
                    Recursion = False
                    NoFolder = False
                    OutputPathSetting = ""
                    Console.ForegroundColor = ConsoleColor.DarkCyan
                    Console.WriteLine("���������ã�")
                    Console.ForegroundColor = ConsoleColor.White
                    Thread.Sleep(3000)
                    Console.Clear()
                    Continue While
                Case "help", "about", "version"
                    Console.ForegroundColor = ConsoleColor.DarkCyan
                    Console.WriteLine($"DDS �ļ���ȡ���� {Version} by ChilorXN.")
                    Console.ForegroundColor = ConsoleColor.DarkYellow
                    Console.WriteLine("���Ϸ�Ҫ������ .afb �� .svo �ļ������ڣ��������ļ�·��(֧�ֶ���ļ�)")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.WriteLine("���� 'SwitchMode' �л�����ģʽ")
                    Console.WriteLine("���� 'OutputMode' �л����ģʽ")
                    Console.WriteLine("���� 'SetPath' �������·��")
                    Console.WriteLine("���� 'ScanMode' �����ļ���ģʽ���Ƿ�ݹ�ɨ��")
                    Console.WriteLine("���� 'Patcher' ����ͬĿ¼�µ�DDS�޲�����")
                    Console.WriteLine("���� 'clear' �����Ļ")
                    Console.WriteLine("���� 'reset' �����������ò������Ļ")
                    Console.WriteLine("���� 'help' �ٴβ鿴����")
                    Console.WriteLine("���� 'exit' �˳�����")
                    Continue While
                Case "exit", "quit"
                    Exit While
            End Select

            ' ����������ļ�
            ProcessInputFiles(input)
        End While

        Console.WriteLine("�������˳�")
    End Sub

    Private Sub ProcessInputFiles(input As String)
        ' �����Ϸŵ��ļ�·��(Windows�ն˻������Ű������ո���ļ�·��)
        Dim filePaths As New List(Of String)()
        Dim inQuotes As Boolean = False
        Dim currentPath As New System.Text.StringBuilder()

        For Each c As Char In input
            If c = """"c Then
                If inQuotes Then
                    ' �������Ű�����·��
                    filePaths.Add(currentPath.ToString())
                    currentPath.Clear()
                    inQuotes = False
                Else
                    ' ��ʼ���Ű�����·��
                    inQuotes = True
                End If
            ElseIf Not inQuotes AndAlso Char.IsWhiteSpace(c) Then
                ' �����Ű����Ŀո�ָ���
                If currentPath.Length > 0 Then
                    filePaths.Add(currentPath.ToString())
                    currentPath.Clear()
                End If
            Else
                ' ���ӵ���ǰ·��
                currentPath.Append(c)
            End If
        Next

        ' �������һ��·��
        If currentPath.Length > 0 Then
            filePaths.Add(currentPath.ToString())
        End If

        ' ����ÿ���ļ����ļ���
        For Each filePath In filePaths
            If Not String.IsNullOrWhiteSpace(filePath) Then
                Try
                    If FolderMode Then
                        ' �ļ���ģʽ�������ļ����ڵ�����afb/svo�ļ�
                        ProcessFolder(filePath)
                    Else
                        ' ����ģʽ�����������ļ�
                        ProcessFile(filePath)
                    End If
                Catch ex As Exception
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine($"�����ļ� {filePath} ʱ����: {ex.Message}")
                    Console.ForegroundColor = ConsoleColor.White
                End Try
            End If
        Next
    End Sub

    Private Sub ProcessFolder(folderPath As String)
        If Not Directory.Exists(folderPath) Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine($"�ļ��л��������: {folderPath}")
            Console.ForegroundColor = ConsoleColor.White
            Return
        End If

        Console.ForegroundColor = ConsoleColor.DarkCyan
        Console.WriteLine($"����ɨ���ļ���: {folderPath}")
        Console.ForegroundColor = ConsoleColor.White

        ' ��ȡ����afb��svo�ļ�
        ' �������Ҫͬʱ��ȡ��Ŀ¼�е��ļ����뽫'SearchOption.TopDirectoryOnly'��Ϊ'SearchOption.AllDirectories'
        Dim files As String()
        If Recursion = True Then
            files = Directory.GetFiles(folderPath, "*.afb", SearchOption.AllDirectories)
            files = files.Concat(Directory.GetFiles(folderPath, "*.svo", SearchOption.AllDirectories)).ToArray()
        Else
            files = Directory.GetFiles(folderPath, "*.afb", SearchOption.TopDirectoryOnly)
            files = files.Concat(Directory.GetFiles(folderPath, "*.svo", SearchOption.TopDirectoryOnly)).ToArray()
        End If

        If files.Length = 0 Then
            Console.ForegroundColor = ConsoleColor.DarkYellow
            Console.WriteLine($"�ļ�����û���ҵ�afb��svo�ļ�")
            Console.ForegroundColor = ConsoleColor.White
            Return
        End If

        Console.WriteLine($"�ҵ� {files.Length} ���ļ���Ҫ����")

        Dim successCount As Integer = 0
        Dim failCount As Integer = 0

        For Each filePath In files
            Try
                ProcessFile(filePath)
                successCount += 1
            Catch ex As Exception
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine($"�����ļ� {Path.GetFileName(filePath)} ʱ����: {ex.Message}")
                Console.ForegroundColor = ConsoleColor.White
                failCount += 1
            End Try
        Next

        Console.ForegroundColor = ConsoleColor.Green
        Console.WriteLine($"�ļ��д������: {successCount} ���ɹ�, {failCount} ��ʧ��")
        Console.ForegroundColor = ConsoleColor.White
    End Sub


    Private Sub ProcessFile(filePath As String)
        If Not File.Exists(filePath) Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine($"�ļ����������: {filePath}")
            Console.ForegroundColor = ConsoleColor.White
            Return
        End If

        Dim extension As String = Path.GetExtension(filePath).ToLower()
        If extension <> ".afb" AndAlso extension <> ".svo" Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine($"��֧�ֵ��ļ�����: {filePath} (��֧�� .afb �� .svo)")
            Console.ForegroundColor = ConsoleColor.White
            Return
        End If

        Console.WriteLine($"���ڴ����ļ�: {filePath}")

        Dim fileData As Byte() = File.ReadAllBytes(filePath)
        Dim ddsList As List(Of Byte()) = ExtractDdsFiles(fileData, extension = ".afb")

        Console.WriteLine($"�ҵ� {ddsList.Count} �� DDS �ļ�")

        ' ������ȡ�� DDS �ļ�
        Dim baseName As String = Path.GetFileNameWithoutExtension(filePath)
        Dim outputDir As String = ""

        '������·����д����
        If String.IsNullOrWhiteSpace(OutputPathSetting) Then
            outputDir = Path.Combine(Path.GetDirectoryName(filePath), $"{baseName}_extracted")
        Else
            If Not Directory.Exists(OutputPathSetting) Then '��ֹ�û��㳴��
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("���·����Ч����ʹ��Ĭ��·������")
                Console.ForegroundColor = ConsoleColor.White
                outputDir = Path.Combine(Path.GetDirectoryName(filePath), $"{baseName}_extracted")
            Else
                If NoFolder = True Then
                    outputDir = OutputPathSetting
                Else
                    outputDir = Path.Combine(OutputPathSetting, $"{baseName}_extracted")
                End If
            End If
        End If

        If NoFolder = False Then
            Directory.CreateDirectory(outputDir)
        End If

        For i As Integer = 0 To ddsList.Count - 1
            Dim outputPath As String = Path.Combine(outputDir, $"{baseName}_{i + 1}.dds")
            File.WriteAllBytes(outputPath, ddsList(i))
            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine($"�ѱ���: {outputPath}")
            Console.ForegroundColor = ConsoleColor.White
        Next
    End Sub

    Public Function ExtractDdsFiles(fileData As Byte(), isAfbFile As Boolean) As List(Of Byte())
        Dim ddsFiles As New List(Of Byte())()
        Dim position As Integer = 0

        While position < fileData.Length - 4
            ' ����Ƿ��� DDS �ļ�ͷ
            If fileData(position) = DDS_HEADER(0) AndAlso
               fileData(position + 1) = DDS_HEADER(1) AndAlso
               fileData(position + 2) = DDS_HEADER(2) AndAlso
               fileData(position + 3) = DDS_HEADER(3) Then

                ' ������һ�� DDS �ļ�ͷ��������
                Dim nextDdsPos As Integer = FindNextDdsHeader(fileData, position + 4)
                Dim endPos As Integer = If(nextDdsPos <> -1, nextDdsPos, fileData.Length)

                ' ���� AFB �ļ�������Ƿ��� POF ���
                If isAfbFile AndAlso nextDdsPos = -1 Then
                    Dim pofPos As Integer = FindPofMarker(fileData, position + 4)
                    If pofPos <> -1 Then
                        endPos = pofPos
                    End If
                End If

                ' ��ȡ DDS ����
                Dim ddsLength As Integer = endPos - position
                Dim ddsData(ddsLength - 1) As Byte
                Array.Copy(fileData, position, ddsData, 0, ddsLength)
                ddsFiles.Add(ddsData)

                position = endPos
            Else
                position += 1
            End If
        End While

        Return ddsFiles
    End Function

    Public Function FindNextDdsHeader(data As Byte(), startPos As Integer) As Integer
        For i As Integer = startPos To data.Length - 4
            If data(i) = DDS_HEADER(0) AndAlso
               data(i + 1) = DDS_HEADER(1) AndAlso
               data(i + 2) = DDS_HEADER(2) AndAlso
               data(i + 3) = DDS_HEADER(3) Then
                Return i
            End If
        Next
        Return -1
    End Function

    Public Function FindPofMarker(data As Byte(), startPos As Integer) As Integer
        ' POF ����� ASCII �ַ��� "POF"
        For i As Integer = startPos To data.Length - 3
            If data(i) = AscW("P"c) AndAlso
               data(i + 1) = AscW("O"c) AndAlso
               data(i + 2) = AscW("F"c) Then
                Return i
            End If
        Next
        Return -1
    End Function
End Module