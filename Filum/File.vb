
Namespace Filum
    ''' <summary>
    ''' 'A very lightweight wrapper around a file
    ''' </summary>
    Public Class File
        Private ReadOnly m_FileName As String
        Private ReadOnly m_FullPath As String
        Private m_FileDescriptor As Integer
        Private m_FilePos As Integer
        ''' <summary>
        ''' The name of the file
        ''' </summary>
        ''' <returns>A String representing the name of the file</returns>
        Public ReadOnly Property FileName As String
            Get
                Return m_FileName
            End Get

        End Property
        ''' <summary>
        ''' A consuming iterator over all the lines of the file
        ''' </summary>
        ''' <returns>An iterator over all the lines in the file</returns>
        Public ReadOnly Iterator Property Lines As IEnumerable(Of String)
            Get
                While Not EOF(m_FileDescriptor)
                    Yield ReadLine()
                End While

            End Get
        End Property
        Public ReadOnly Property FullPath As String
            Get
                Return m_FullPath
            End Get
        End Property
        ''' <summary>
        ''' Creates a new file in the directory provided
        ''' </summary>
        ''' <param name="filename">The name of the file</param>
        ''' <param name="mode">The OpenMode the file should be in</param>
        ''' <param name="rootDir">root directory where file will be created</param>
        Public Sub New(filename As String, mode As OpenMode, rootDir As String)
            m_FilePos = 1
            m_FullPath = IO.Path.Combine(rootDir, filename)
            If Not IO.Directory.Exists(rootDir) Then
                IO.Directory.CreateDirectory(rootDir)
            End If
            OpenFile(mode)
            m_FileName = filename
        End Sub
        Protected Overrides Sub Finalize()
            If IO.File.Exists(m_FullPath) Then
                Close()
            End If
        End Sub
        ''' <summary>
        ''' Reads a line from the file and moves to the next line
        ''' </summary>
        ''' <returns>One line from the file</returns>
        Public Function ReadLine() As String
            Return LineInput(m_FileDescriptor)
        End Function
        ''' <summary>
        ''' Reads the entire file to a string
        ''' </summary>
        ''' <returns>A String containing the files contents</returns>
        Public Function Read_To_String() As String
            Dim res As String = ""
            While Not EOF(m_FileDescriptor)
                res += ReadLine()
            End While
            Return res
        End Function
        ''' <summary>
        ''' Writes the object to the file and appends a newline character
        ''' </summary>
        ''' <param name="data">The object to be written to the fike</param>
        ''' <exception cref="IO.IOException">Thrown when OpenMode is NOT set to OUTPUT or APPEND</exception>
        Public Sub WriteLine(data As Object)
            PrintLine(m_FileDescriptor, data.ToString)
        End Sub
        ''' <summary>
        ''' Writes the object to the file DOES NOT append newline character
        ''' </summary>
        ''' <param name="data">The object to be written to the fike</param>
        ''' <exception cref="IO.IOException">Thrown when OpenMode is NOT set to OUTPUT or APPEND</exception>
        Public Sub Write(data As Object)
            Print(m_FileDescriptor, data.ToString)
        End Sub
        ''' <summary>
        ''' Writes the objects to the file seperated by newline characters (Appends a newline after writing each object
        ''' </summary>
        ''' <param name="data">The objects to be written to the fike</param>
        ''' <exception cref="IO.IOException">Thrown when OpenMode is NOT set to OUTPUT or APPEND</exception>
        Public Sub WriteLines(Of T)(data As T())
            Print(m_FileDescriptor, String.Join(
                      Environment.NewLine, data.Select(Function(x) x.ToString())
                      ))
        End Sub
        ''' <summary>
        ''' Sets read/write position to the beginning of the file
        ''' </summary>
        Public Sub SeekBegin()

            FileSystem.Seek(m_FileDescriptor, 1)


        End Sub
        ''' <summary>
        ''' Deletes the file
        ''' </summary>
        Public Sub Delete()
            Close()
            IO.File.Delete(m_FullPath)
        End Sub

        Private Sub Close()
            FileClose(m_FileDescriptor)
        End Sub
        ''' <summary>
        ''' Changes the OpenMode of the file to the one provided
        ''' </summary>
        ''' <param name="newMode">OpenMode to change to</param>
        Public Sub ChangeMode(newMode As OpenMode)
            Close()
            OpenFile(newMode)

        End Sub
        Private Sub OpenFile(mode As OpenMode)
            m_FileDescriptor = FreeFile()
            FileOpen(m_FileDescriptor, m_FullPath, mode)
        End Sub
        ''' <summary>
        ''' Sets the position of the next read/write operation 
        ''' </summary>
        ''' <param name="pos">Position in file</param>
        Public Sub Seek(pos As ULong)
            FileSystem.Seek(m_FileDescriptor, pos)
        End Sub
        ''' <summary>
        ''' Reads charCount characters from the file from current read position
        ''' </summary>
        ''' <param name="charCount">Number of characters to read</param>
        ''' <returns></returns>
        Public Function Read(charCount As UInteger) As String
            Return InputString(m_FileDescriptor, charCount)
        End Function
    End Class
End Namespace