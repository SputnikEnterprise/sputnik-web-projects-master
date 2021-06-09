
Option Compare Text

Imports System.IO
Imports System.Text
Imports System.Security.Cryptography


' '' Beschreibung:
' '' Mit dieser Klasse kann man komfortabel und effizient nach Dateien auf einem lokalen Computer suchen.
' ''Dabei hat man die Möglichkeit eine Vielzahl an Parametern für die Suche anzugeben um die Suche präzise
' ''an seine Bedürfnisse anzupassen. Diese Parameter werden als Objekt gekapselt an die Suchroutine übergeben.
' ''So hat man die Möglichkeit den Suchfilter zu serialisieren falls erwünscht.
' ''Hier eine Übersicht der wichtigsten Features dieser Klasse:
' ''-Man kann sich entscheiden ob man nur nach Dateien, nur nach Ordnern oder nach beidem suchen will.
' ''Die Ergebnisse werden dann in einem List(Of String) Array gespeichert.
' ''-Wenn man ein Suchwort angegeben hat kann man sich entscheiden wie dieses Wort mit den Dateinamen
' ''verglichen werden soll. Entweder ein normaler Vergleich oder ein exakter Vergleich (Case Sensitive).
' ''Man kann aber auch den vergleich auslassen, wenn man allerdings ein Suchwort angegeben hat wird
' ''zumindest geprüft ob sich das Suchwort in dem Dateinamen befindet. Wenn nicht dann wird diese Datei
' ''nicht zu den Suchergebnissen gepackt.
' ''-Man kann auch bestimmte Ordner oder Dateien von der Suche ausschließen dabei kann man entweder
' ''den Datei-/Ordnernamen mit kompletten Pfad angeben (für eine spezifische Ausnahme) oder nur den
' ''Datei-/Ordnernamen angeben (für eine allgemeine Ausnahme). Das ist dann praktisch wenn man z.B. die
' ''Datei "thumbs.db" nicht zu den Suchergebnissen packen möchte etc.
' ''-Die Suche kann auch auf bestimmte Dateitypen begrenzt werden so das nur nach
' ''BMP und JPG Dateien gesucht wird. Oder umgekehrt das z.B. alle BMP und JPG Dateien
' ''von der Suche ausgeschlossen werden und nicht zu den Suchergebnissen kommen.
' ''-Man kann die Suche nach Dateien nach einem vorgegebenen Größenspektrum eingrenzen,
' ''das heißt das man z.B. nur nach Dateien suche kann die mindestens 10 MB groß sind und/oder
' ''maximal 20 MB größ sind. Dazu kann man bei der Filtereigenschaft "SizeType" festlegen im welchen
' ''Format die Größenangabe ist, entweder Byte, KB, MB oder GB.
' ''-Wie bei der Größeneingrenzung kann man das auch mit dem Datum einer Datei machen so das nur
' ''Dateien zu den Suchergebnissen kommen die nicht neuer oder älter als ein vorgegebenes Datum sind.
' ''Auch hier kann man sich ein Datum aussuchen da ja eine Datei immer drei Datumsangaben hat:
' ''Änderungsdatum, Erstellungsdatum und Zugriffdatum.
' ''-Außerdem kann man auch nach Dateien suchen die einen bestimmten Hash-Wert haben, dabei kann man entweder
' ''einen CRC32 Hash, einen MD5 Hash oder einen SHA1 Hash angeben. Es kommen dann nur die Dateien zu den
' ''Suchergebnissen die auch dieses Hash haben.
' ''
' ''Das Filterungsprinzip:
' ''Bei der Filterung von alle Dateien wird wie in einem Sieb vorgegangen bzw. mehrere übereinanderliegende Siebe.
' ''Natürlich werden die schnellsten Filter zuerst angewendet (Dateiname) und die langsamsten Filter zuletzt (FileHash).
' ''Das funktktioniert dann so das die Datei alle Filter solange durchläuft bis die Datei bei einem Filter hängen
' ''bleibt weil diese Datei nicht mehr zu den vorgaben des Filters passt.
' ''Hier ein Beispiel: Angenommen man sucht nach einer Datei mit einem bestimmten Hash und nur die
' ''Dateien die kleiner als 5 MB sind. Als erstes wird abgefragt ob Datei X kleiner als 5 MB ist,
' ''wenn das der Fall ist dann wird geprüft ob die Datei auch diesen Hash hat. Wenn die Datei aber
' ''größer als 5 MB ist wird diese Datei als ungültig markiert und nicht mehr von den anderen Filtern
' ''abgefragt das heißt das auch nicht mehr der Hash der Datei berechnet wird.
' ''Hinweise:
' ''Man muss beachten das man nicht alle Filtereigenschaften angeben muss sondern nur die die
' ''man auch wirklich brauch. Und mann muss aufpassen das sich die Filter nicht gegeneinandern
' ''aufheben so das es zu keinen Ergebnisse kommt. Das kann z.B. dann passieren wenn man die
' ''Mindestdateigröße größer macht als die Maximaldateigröße oder wenn man nur nach TXT Dateien
' ''sucht und gleichzeitig TXT Dateien ausschließt.
' ''Diese Klasse wurde ausgiebig getestet und Funktioniert einwandfrei.
' ''Für weitere Informationen kann man sich die entsprechenden XML Kommentare ansehen.
' ''Für Fragen stehe ich gerne zur Verfügung.
' ''Ich freue mich auf euer Feedback in den Kommentaren.


''' <summary>
''' Mit dieser Klasse können Sie auf einem lokalen Computer nach Dateien suchen.
''' Dazu stehen Ihnen eine Vielzahl an Filtern mit der man die Suche präzise an seine Bedürfnisse anpassen kann.
''' </summary>
''' <remarks>
''' Sie können diese Klasse frei verwenden auch in kommerziellen Programmen.
''' Ich bitte Sie lediglich um einen Hinweis über diese Klasse und dem Autor in Ihrem Programm (z.B. Info-Dialog)
''' Kontakt: khartak@freenet.de
''' Copyright © 2006 Tim Hartwig
''' </remarks>
Public Class clsFileSearch
  Private FolderSize As Long = 0
  Private ErrorLog As New System.Text.StringBuilder
  Private CRC32Table() As Integer
  Private AbortProgress As Boolean

  Private mRaiseErrors As Boolean

#Region "Properties"
  ''' <summary>
  ''' Gibt an ob das Event für Fehlermeldungen bei einem Fehler aufgerufen werden soll.
  ''' In den meisten fällen tritt ein Fehler auf wenn versucht wird z.B.
  ''' den Ordner "System Volume Information" zu öffnen etc.
  ''' </summary>
  Public Property RaiseErrors() As Boolean
    Get
      RaiseErrors = mRaiseErrors
    End Get
    Set(ByVal Value As Boolean)
      mRaiseErrors = Value
    End Set
  End Property
#End Region

#Region "Events"
  ''' <summary>
  ''' Wird aufgerufen wenn eine Datei gültige gefunden wurde.
  ''' </summary>
  ''' <param name="FileName">Der Dateiname mit kompletten Pfad</param>
  Public Event MatchFound(ByVal sender As Object, ByVal FileName As String)

  ''' <summary>
  ''' Wird aufgerufen wenn ein Fehler auftritt z.B. ein verweigerter Zugriff auf einen Ordner etc.
  ''' </summary>
  ''' <param name="ErrorMessage">Die Fehlernachricht</param>
  Public Event RaiseError(ByVal sender As Object, ByVal ErrorMessage As String)

  ''' <summary>
  ''' Wird aufgerufen wenn der Suchvorgang vollständig ist
  ''' </summary>
  ''' <remarks></remarks>
  Public Event SearchComplete(ByVal sender As Object)
#End Region

#Region "Public Methods"
#Region "[Search] = SUCHFUNKTION -> Dateisuche mit Filter"
  ''' <summary>
  ''' Diese Funktion sucht nach bestimmten Dateien
  ''' </summary>
  ''' <param name="Root">Der Ordner in dem gesucht werden soll</param>
  ''' <param name="SearchWord">Ein Optionales Suchwort</param>
  ''' <param name="Filter">Der Filter für die Suche</param>
  ''' <param name="FileArray">Das Array in der die gefundenen Dateien und/oder Ordner kommen</param>
  Public Sub Search(ByVal Root As String, ByVal SearchWord As String, ByVal Filter As SearchFilter, ByRef FileArray As List(Of String))
    ErrorLog.Remove(0, ErrorLog.Length)
    AbortProgress = False
    DoSearch(Root, SearchWord, Filter, FileArray)
    AbortProgress = False
    RaiseEvent SearchComplete(Me)
  End Sub

  Private Sub DoSearch(ByVal Root As String, ByVal SearchWord As String, ByVal Filter As SearchFilter, ByRef FileArray As List(Of String))
    Try
      If AbortProgress = True Then Exit Sub
      '      My.Application.DoEvents()

      Dim Files() As String = System.IO.Directory.GetFiles(Root)
      Dim Folders() As String = System.IO.Directory.GetDirectories(Root)
      Dim Recurse As Boolean = True

      If Filter.Listing = SearchFilter.LO.BOTH Or Filter.Listing = SearchFilter.LO.FILES_ONLY Then
        For i As Integer = 0 To UBound(Files)
          If FileFilter(Files(i).ToString, SearchWord, Filter) = True Then
            FileArray.Add(Files(i).ToString)
            RaiseEvent MatchFound(Me, Files(i))
          End If
        Next
      End If

      For i As Integer = 0 To UBound(Folders)
        'Es wird schon hier geprüft ob der Ordner erlaubt ist, denn wenn der Filter
        'auf(FILES_ONLY) gestellt ist, wird der ausgeschlossene Ordner trotzdem geöffnet
        If AllowedFolder(Folders(i), Filter.ExcludeFolders) = False Then Recurse = False
        If Filter.Listing = SearchFilter.LO.BOTH Or Filter.Listing = SearchFilter.LO.FOLDERS_ONLY Then
          If Recurse = True Then
            FileArray.Add(Folders(i).ToString)
            RaiseEvent MatchFound(Me, Folders(i))
          End If
        End If
        If Recurse = True And Filter.NoSubFolders = False Then DoSearch(Folders(i), SearchWord, Filter, FileArray)
        Recurse = True
      Next
    Catch Ex As Exception
      'Hier werden die Fehler aufgefangen und in einem String geschrieben
      If mRaiseErrors = True Then
        ErrorLog.Append(Ex.Message & vbNewLine)
        RaiseEvent RaiseError(Me, Ex.Message)
      End If
    End Try
  End Sub
#End Region

#Region "[ListAll] = AUFLISTUNGSFUNKTION -> Auflistung von Dateien und Ordnern"
  ''' <summary>
  ''' Diese Funktion listet alle Dateien und/oder Ordner inklusive Unterordner auf und speichert diese in einem Array
  ''' </summary>
  ''' <param name="Root">Der Ordner in dem gesucht werden soll</param>
  ''' <param name="FileArray">Ein 0-basiertes Array in welchem die Suchergebnisse gespeichert werden sollen</param>
  ''' <param name="FFBFilter">Angabe ob nur alle Dateien oder nur alle Ordner oder beides aufgelistet werden soll</param>
  Public Sub ListAll(ByVal Root As String, ByRef FileArray As List(Of String), Optional ByVal FFBFilter As SearchFilter.LO = SearchFilter.LO.BOTH)
    ErrorLog.Remove(0, ErrorLog.Length)
    AbortProgress = False
    DoListAll(Root, FileArray, FFBFilter)
    AbortProgress = False
    RaiseEvent SearchComplete(Me)
  End Sub
  Private Sub DoListAll(ByVal Root As String, ByRef FileArray As List(Of String), Optional ByVal ListingOption As SearchFilter.LO = SearchFilter.LO.BOTH)
    Try
      If AbortProgress = True Then Exit Sub
      '      My.Application.DoEvents()
      Dim Files() As String = System.IO.Directory.GetFiles(Root)
      Dim Folders() As String = System.IO.Directory.GetDirectories(Root)

      If ListingOption = SearchFilter.LO.BOTH Or ListingOption = SearchFilter.LO.FILES_ONLY Then
        For i As Integer = 0 To UBound(Files)
          FileArray.Add(Files(i).ToString)
          RaiseEvent MatchFound(Me, Files(i))
        Next
      End If

      For i As Integer = 0 To UBound(Folders)
        If ListingOption = SearchFilter.LO.BOTH Or ListingOption = SearchFilter.LO.FOLDERS_ONLY Then
          FileArray.Add(Folders(i).ToString)
          RaiseEvent MatchFound(Me, Folders(i))
        End If
        DoListAll(Folders(i), FileArray, ListingOption)
      Next
    Catch Ex As Exception
      If mRaiseErrors = True Then
        ErrorLog.Append(Ex.Message & vbNewLine)
        RaiseEvent RaiseError(Me, Ex.Message)
      End If
    End Try
  End Sub
#End Region

#Region "[GetFolderSize] = Ordnergröße ermitteln"
  ''' <summary>
  ''' Diese Funktion berechnet die Größe eines Ordners
  ''' </summary>
  ''' <param name="Root">Der Ordner wessen größe berechnet werden soll</param>
  ''' <param name="SizeFormat">Angabe in welchem Format die Gesamtgröße zurückgegeben werden soll: GB,MB,KB,B</param>
  Public Function GetFolderSize(ByVal Root As String, ByVal SizeFormat As SearchFilter.ST) As Long
    Dim TmpSize As Long = 0
    FolderSize = 0
    ErrorLog.Remove(0, ErrorLog.Length)
    DoGetFolderSize(Root, SizeFormat)

    Select Case SizeFormat
      Case SearchFilter.ST.BYTES : FolderSize = CLng(FolderSize / 1)
      Case SearchFilter.ST.KILO_BYTES : FolderSize = CLng(FolderSize / 1024)
      Case SearchFilter.ST.MEGA_BYTES : FolderSize = CLng(FolderSize / 1048576)
      Case SearchFilter.ST.GIGA_BYTES : FolderSize = CLng(FolderSize / 1073741824)
    End Select

    Return FolderSize
  End Function
  Private Function DoGetFolderSize(ByVal Root As String, ByVal SizeFormat As SearchFilter.ST) As Long
    Try
      Dim Files() As String = System.IO.Directory.GetFiles(Root)
      Dim Folders() As String = System.IO.Directory.GetDirectories(Root)

      For i As Integer = 0 To UBound(Files)
        FolderSize += FileLen(Files(i))
      Next

      For i As Integer = 0 To UBound(Folders)
        DoGetFolderSize(Folders(i), SizeFormat)
      Next
    Catch Ex As Exception
      ErrorLog.Append(Ex.Message & vbNewLine)
      RaiseEvent RaiseError(Me, Ex.Message)
    End Try
  End Function
#End Region

  ''' <summary>
  ''' Bricht den Suchvorgang ab
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub Abort()
    AbortProgress = True
  End Sub

  ''' <summary>
  ''' Diese Funktion gibt alle bei der Suche aufgetretenen Fehlermeldungen zurück
  ''' </summary>
  Public Function GetErrorLog() As String
    Return ErrorLog.ToString
  End Function

  ''' <summary>
  ''' Diese Funktion berechnet den CRC32 Hash einer Datei
  ''' </summary>
  Public Function GetCRC32(ByVal FileName As String) As String
    Dim FS As FileStream = New FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read, 8192)
    Dim CRC32Result As Integer = &HFFFFFFFF
    Dim Buffer(4096) As Byte
    Dim ReadSize As Integer = 4096
    Dim Count As Integer = FS.Read(Buffer, 0, ReadSize)
    Dim i As Integer, iLookup As Integer

    If CRC32Table.Length = 0 Then
      'CRC32 Tabelle erstellen
      CreateCRC32Table()
    End If

    Do While (Count > 0)
      For i = 0 To Count - 1
        iLookup = (CRC32Result And &HFF) Xor Buffer(i)
        CRC32Result = ((CRC32Result And &HFFFFFF00) \ &H100) And &HFFFFFF
        CRC32Result = CRC32Result Xor CRC32Table(iLookup)
      Next i
      Count = FS.Read(Buffer, 0, ReadSize)
    Loop
    Return Hex(Not (CRC32Result))
  End Function

  ''' <summary>
  ''' Diese Funktion berechnet den MD5 Hash einer Datei
  ''' </summary>
  Public Function GetMD5(ByVal File As String) As String
    Dim FN As New FileStream(File, FileMode.Open, FileAccess.Read, FileShare.Read, 8192)
    Dim HashValue(0) As Byte
    Dim Result As String = ""
    Dim Tmp As String = ""

    Dim MD5 As New MD5CryptoServiceProvider
    MD5.ComputeHash(FN)
    HashValue = MD5.Hash

    FN.Close()

    For i As Integer = 0 To HashValue.Length - 1
      Tmp = Hex(HashValue(i))
      If Len(Tmp) = 1 Then Tmp = "0" & Tmp
      Result += Tmp
    Next
    Return Result
  End Function

  ''' <summary>
  ''' Diese Funktion berechnet den SHA1 Hash einer Datei
  ''' </summary>
  Public Function GetSHA(ByVal File As String) As String
    Dim FN As New FileStream(File, FileMode.Open, FileAccess.Read, FileShare.Read, 8192)
    Dim HashValue(0) As Byte
    Dim Result As String = ""
    Dim Tmp As String = ""

    Dim SHA1 As New SHA1CryptoServiceProvider
    SHA1.ComputeHash(FN)
    HashValue = SHA1.Hash

    FN.Close()

    For i As Integer = 0 To HashValue.Length - 1
      Tmp = Hex(HashValue(i))
      If Len(Tmp) = 1 Then Tmp = "0" & Tmp
      Result += Tmp
    Next
    Return Result
  End Function
#End Region

#Region "Private Methods"
  Private Function FileFilter(ByVal FileName As String, ByVal SearchWord As String, ByVal Filter As SearchFilter) As Boolean
    'Fastest First Slowest Last!
    Dim IsFileValid As Boolean = True
    Dim Extensions() As String
    Dim FileArray() As String

    If AbortProgress = True Then Exit Function

    'Wenn eine Suchwort angegeben wurde dann wird geguckt ob es sich überhaupt in der Datei
    'befindet, falls nicht wird direkt abgebrochen um nicht unnötig die Filter zu durchlaufen
    If SearchWord <> "" Then
      If InStr(FileName, SearchWord) = 0 Then Exit Function
    End If

    'ExcludeFiles
    If IsFileValid Then
      If Filter.ExcludeFiles <> "" Then
        FileArray = Split(Filter.ExcludeFiles, ",")
        For i As Integer = 0 To UBound(FileArray)
          If InStr(FileArray(i), "\") > 0 Then
            If FileArray(i) = FileName Then
              IsFileValid = False
              Exit For
            End If
          Else
            If Path.GetFileName(FileArray(i)) = Path.GetFileName(FileName) Then
              IsFileValid = False
              Exit For
            End If
          End If
        Next
      End If
    End If

    'ExcludeFileTypes
    If IsFileValid Then
      If Filter.ExcludeFileTypes <> "" Then
        Extensions = Split(Filter.ExcludeFileTypes, ",")
        For i As Integer = 0 To UBound(Extensions)
          If LCase(Extensions(i)) = LCase(GetFileExt(FileName)) Then
            IsFileValid = False
            Exit For
          End If
        Next
      End If
    End If

    'FileTypes
    If IsFileValid Then
      If Filter.FileTypes <> "" Then
        IsFileValid = False
        Extensions = Split(Filter.FileTypes, ",")
        For i As Integer = 0 To UBound(Extensions)
          If LCase(Extensions(i)) = LCase(GetFileExt(FileName)) Then
            IsFileValid = True
            Exit For
          End If
        Next
      End If
    End If

    'WordComparing
    If IsFileValid Then
      Select Case Filter.WordComparing
        Case SearchFilter.CO.NORMAL
          If Path.GetFileName(FileName) <> SearchWord Then IsFileValid = False
        Case SearchFilter.CO.CASESENSITIVE
          If Path.GetFileName(FileName).CompareTo(SearchWord) <> 0 Then IsFileValid = False
      End Select
    End If

    'MaxSize
    If IsFileValid Then
      If Filter.MaxSize > 0 Then
        Select Case Filter.SizeType
          Case SearchFilter.ST.BYTES
            If FileLen(FileName) > Filter.MaxSize Then IsFileValid = False
          Case SearchFilter.ST.KILO_BYTES
            If FileLen(FileName) \ 1024 > Filter.MaxSize Then IsFileValid = False
          Case SearchFilter.ST.MEGA_BYTES
            If FileLen(FileName) \ 1048576 > Filter.MaxSize Then IsFileValid = False
          Case SearchFilter.ST.GIGA_BYTES
            If FileLen(FileName) \ 1073741824 > Filter.MaxSize Then IsFileValid = False
        End Select
      End If
    End If

    'MinSize
    If IsFileValid Then
      If Filter.MinSize > 0 Then
        Select Case Filter.SizeType
          Case SearchFilter.ST.BYTES
            If FileLen(FileName) < Filter.MinSize Then IsFileValid = False
          Case SearchFilter.ST.KILO_BYTES
            If FileLen(FileName) \ 1024 < Filter.MinSize Then IsFileValid = False
          Case SearchFilter.ST.MEGA_BYTES
            If FileLen(FileName) \ 1048576 < Filter.MinSize Then IsFileValid = False
          Case SearchFilter.ST.GIGA_BYTES
            If FileLen(FileName) \ 1073741824 < Filter.MinSize Then IsFileValid = False
        End Select
      End If
    End If

    'MaxDate
    If IsFileValid Then
      If Filter.MaxDate <> Nothing Then
        Select Case Filter.DateType
          Case SearchFilter.DT.CREATE_TIME
            If System.IO.File.GetCreationTime(FileName) > Filter.MaxDate Then IsFileValid = False
          Case SearchFilter.DT.LAST_ACCESS_TIME
            If System.IO.File.GetLastAccessTime(FileName) > Filter.MaxDate Then IsFileValid = False
          Case SearchFilter.DT.LAST_WRITE_TIME
            If System.IO.File.GetLastWriteTime(FileName) > Filter.MaxDate Then IsFileValid = False
        End Select
      End If
    End If

    'MinDate
    If IsFileValid Then
      If Filter.MinDate <> Nothing Then
        Select Case Filter.DateType
          Case SearchFilter.DT.CREATE_TIME
            If System.IO.File.GetCreationTime(FileName) < Filter.MinDate Then IsFileValid = False
          Case SearchFilter.DT.LAST_ACCESS_TIME
            If System.IO.File.GetLastAccessTime(FileName) < Filter.MinDate Then IsFileValid = False
          Case SearchFilter.DT.LAST_WRITE_TIME
            If System.IO.File.GetLastWriteTime(FileName) < Filter.MinDate Then IsFileValid = False
        End Select
      End If
    End If

    'MD5
    If IsFileValid Then
      If Filter.MD5Hash <> "" Then
        If UCase(Filter.MD5Hash) <> UCase(GetMD5(FileName)) Then IsFileValid = False
      End If
    End If

    'SHA1
    If IsFileValid Then
      If Filter.SHAHash <> "" Then
        If UCase(Filter.SHAHash) <> UCase(GetSHA(FileName)) Then IsFileValid = False
      End If
    End If

    'CRC32
    If IsFileValid Then
      If Filter.CRCHash <> "" Then
        If UCase(Filter.CRCHash) <> UCase(GetCRC32(FileName)) Then IsFileValid = False
      End If
    End If

    Return IsFileValid
  End Function

  Private Function AllowedFolder(ByVal CurrentFolder As String, ByVal ExcludeFolders As String) As Boolean
    Dim ExcludeArr() As String
    AllowedFolder = True
    If ExcludeFolders = "" Then Exit Function
    ExcludeArr = Split(ExcludeFolders, ",")

    For i As Integer = 0 To UBound(ExcludeArr)
      If InStr(ExcludeArr(i), "\") > 0 Then
        'Wenn ein Ordner mit Pfad angegeben wurde
        If InStr(CurrentFolder, ExcludeArr(i)) > 0 Then
          AllowedFolder = False
          Exit For
        End If
      Else
        'Wenn ein Ordner ohne Pfad angegeben wurde (nur Name)
        If Path.GetFileName(CurrentFolder) = ExcludeArr(i) Then
          AllowedFolder = False
          Exit For
        End If
      End If
    Next
  End Function

  Private Sub CreateCRC32Table()
    Dim DWPolynomial As Integer = &HEDB88320
    Dim DWCRC As Integer
    Dim i As Integer, j As Integer
    ReDim CRC32Table(256)

    For i = 0 To 255
      DWCRC = i
      For j = 8 To 1 Step -1
        If CBool((DWCRC And 1)) Then
          DWCRC = CInt(((DWCRC And &HFFFFFFFE) \ 2&) And &H7FFFFFFF)
          DWCRC = DWCRC Xor DWPolynomial
        Else
          DWCRC = CInt(((DWCRC And &HFFFFFFFE) \ 2&) And &H7FFFFFFF)
        End If
      Next j
      CRC32Table(i) = DWCRC
    Next i
  End Sub

  Private Function GetFileExt(ByVal File As String) As String
    If File.EndsWith("\") Then File = File.Substring(0, File.Length - 1)
    Return LCase(Mid(File, InStrRev(File, ".") + 1))
  End Function
#End Region


  <Serializable()> _
  Public Class SearchFilter
    Private mListing As LO
    Private mFileTypes As String
    Private mWordComparing As CO
    Private mExcludeFolders As String
    Private mExcludeFiles As String
    Private mExcludeFileTypes As String
    Private mMaxSize As Long
    Private mMinSize As Long
    Private mSizeType As ST
    Private mMaxDate As Date
    Private mMinDate As Date
    Private mDateType As DT
    Private mCRCHash As String
    Private mMD5Hash As String
    Private mSHAHash As String
    Private mNoSubFolders As Boolean

    Public Enum LO As Integer 'ListingOptions
      ''' <summary>Ordner und Dateien auflisten</summary>
      BOTH = 0
      ''' <summary>Nur Dateien auflisten</summary>
      FILES_ONLY = 1
      ''' <summary>Nur Ordner auflisten</summary>
      FOLDERS_ONLY = 2
    End Enum

    Public Enum DT As Integer 'DateType
      ''' <summary>Das Erstellungsdatum der Datei zum vergleichen benutzen</summary>
      CREATE_TIME = 0
      ''' <summary>Das Zugriffsdatum der Datei zum vergleichen benutzen</summary>
      LAST_ACCESS_TIME = 1
      ''' <summary>Das Änderungsdatum der Datei zum vergleichen benutzen</summary>
      LAST_WRITE_TIME = 2
    End Enum

    Public Enum ST As Integer 'SizeType
      ''' <summary>Auf Byte-Ebene vergleichen (FileSize/1)</summary>
      BYTES = 0
      ''' <summary>Auf KiloByte-Ebene vergleichen (FileSize/1024)</summary>
      KILO_BYTES = 1
      ''' <summary>Auf MegaByte-Ebene vergleichen (FileSize/1048576)</summary>
      MEGA_BYTES = 2
      ''' <summary>Auf GigaByte-Ebene vergleichen (FileSize/1073741824)</summary>
      GIGA_BYTES = 3
    End Enum

    Public Enum CO As Integer 'CompareOption
      ''' <summary>Das Suchwort wird nicht mit dem Dateinamen verglichen</summary>
      NONE = 0
      ''' <summary>Das Suchwort wird normal mit dem Dateinamen verglichen</summary>
      NORMAL = 1
      ''' <summary>Das Suchwort wird exakt mit dem Dateinamen verglichen</summary>
      CASESENSITIVE = 2
    End Enum

    ''' <summary>
    ''' Legt fest ob nur Dateien, nur Ordner oder beides aufgelistet werden soll.
    ''' </summary>
    Public Property Listing() As LO
      Get
        Return mListing
      End Get
      Set(ByVal value As LO)
        mListing = value
      End Set
    End Property

    ''' <summary>
    ''' Legt die Dateitypen fest die gesucht werden sollen z.B. exe,txt,bmp etc.
    ''' Mehrere Extensionen müssen mit einem Komma getrennt und ohne Punkt und Sternchen angegeben werden.
    ''' </summary>
    Public Property FileTypes() As String
      Get
        Return mFileTypes
      End Get
      Set(ByVal value As String)
        mFileTypes = value
      End Set
    End Property

    ''' <summary>
    ''' Legt fest wie die Dateien mit dem Suchwort verglichen werden sollen.
    ''' Entweder garnicht, einfacher Textvergleich oder exakter Textvergleich (Case Sensitive).
    ''' </summary>
    Public Property WordComparing() As CO
      Get
        Return mWordComparing
      End Get
      Set(ByVal value As CO)
        mWordComparing = value
      End Set
    End Property

    ''' <summary>
    ''' Gibt an welche Ordner von der Suche ausgeschlossen werden sollen.
    ''' Man kann entweder einen bestimmten Ordner mit Pfad angeben oder einen Ordnernamen.
    ''' Mehrere Ordner müssen mit einem Komma getrennt werden.
    ''' </summary>
    Public Property ExcludeFolders() As String
      Get
        Return mExcludeFolders
      End Get
      Set(ByVal value As String)
        mExcludeFolders = value
      End Set
    End Property

    ''' <summary>
    ''' Gibt an welche Dateien von der Suche ausgeschlossen werden sollen.
    ''' Man kann entweder eine bestimmte Datei mit Pfad angeben oder einen Dateinamen.
    ''' Mehrere Dateien müssen mit einem Komma getrennt werden
    ''' </summary>
    Public Property ExcludeFiles() As String
      Get
        Return mExcludeFiles
      End Get
      Set(ByVal value As String)
        mExcludeFiles = value
      End Set
    End Property

    ''' <summary>
    ''' Legt fest nach welchen Dateitypen nicht gesucht werden soll z.B. exe,txt,bmp...
    ''' Mehrere Extensionen müssen mit einem Komma getrennt und ohne Punkt und Sternchen angegeben werden.
    ''' </summary>
    Public Property ExcludeFileTypes() As String
      Get
        Return mExcludeFileTypes
      End Get
      Set(ByVal value As String)
        mExcludeFileTypes = value
      End Set
    End Property

    ''' <summary>
    ''' Legt die Maximale größe der zu suchenden Dateien fest.
    ''' Es werden keine Dateien aufgelistet die größer sind als hier angegeben.
    ''' Man kann die größe in B,KB,MB und GB angeben.
    ''' Das kann man in der Eigenschaft SizeType festlegen.
    ''' </summary>
    Public Property MaxSize() As Long
      Get
        Return mMaxSize
      End Get
      Set(ByVal value As Long)
        mMaxSize = value
      End Set
    End Property

    ''' <summary>
    ''' Legt die Minimale größe der zu suchenden Dateien fest.
    ''' Es werden keine Dateien aufgelistet die kleiner sind als hier angegeben.
    ''' Man kann die größe in B,KB,MB und GB angeben.
    ''' Das kann man in der Eigenschaft SizeType festlegen.
    ''' </summary>
    Public Property MinSize() As Long
      Get
        Return mMinSize
      End Get
      Set(ByVal value As Long)
        mMinSize = value
      End Set
    End Property

    ''' <summary>
    ''' Legt den Größentyp für die Eigenschaften MaxSize und MinSize fest.
    ''' </summary>
    Public Property SizeType() As ST
      Get
        Return mSizeType
      End Get
      Set(ByVal value As ST)
        mSizeType = value
      End Set
    End Property

    ''' <summary>
    ''' Legt das maximale Datum der zu suchenden Dateien fest.
    ''' Es werden keine Dateien aufgelistet die neuer sind als hier angegeben.
    ''' Welches Datum man vergleichen will (Zugriffdatum, Erstellungsdatum oder Änderungsdatum)
    ''' kann man in der Eigenschaft DateType festlegen.
    ''' </summary>
    Public Property MaxDate() As Date
      Get
        Return mMaxDate
      End Get
      Set(ByVal value As Date)
        mMaxDate = value
      End Set
    End Property

    ''' <summary>
    ''' Legt das minimale Datum der zu suchenden Dateien fest.
    ''' Es werden keine Dateien aufgelistet die älter sind als hier angegeben.
    ''' Welches Datum man vergleichen will (Zugriffdatum, Erstellungsdatum oder Änderungsdatum)
    ''' kann man in der Eigenschaft DateType festlegen.
    ''' </summary>
    Public Property MinDate() As Date
      Get
        Return mMinDate
      End Get
      Set(ByVal value As Date)
        mMinDate = value
      End Set
    End Property

    ''' <summary>
    ''' Legt für die Eigenschaften MinDate und MaxDate fest welches Datum der Dateien verglichen werden soll.
    ''' </summary>
    Public Property DateType() As DT
      Get
        Return mDateType
      End Get
      Set(ByVal value As DT)
        mDateType = value
      End Set
    End Property

    ''' <summary>
    ''' Legt einen CRC32 Hash fest.
    ''' Wenn man einen CRC32 Hash festlegt werden nur die Dateien aufgelistet die diesen Hash haben.
    ''' </summary>
    Public Property CRCHash() As String
      Get
        Return mCRCHash
      End Get
      Set(ByVal value As String)
        mCRCHash = value
      End Set
    End Property

    ''' <summary>
    ''' Legt einen MD5 Hash fest.
    ''' Wenn man einen MD5 Hash festlegt werden nur die Dateien aufgelistet die diesen Hash haben.
    ''' </summary>
    Public Property MD5Hash() As String
      Get
        Return mMD5Hash
      End Get
      Set(ByVal value As String)
        mMD5Hash = value
      End Set
    End Property

    ''' <summary>
    ''' Legt einen SHA1 Hash fest.
    ''' Wenn man einen SHA1 Hash festlegt werden nur die Dateien aufgelistet die diesen Hash haben.
    ''' </summary>
    Public Property SHAHash() As String
      Get
        Return mSHAHash
      End Get
      Set(ByVal value As String)
        mSHAHash = value
      End Set
    End Property

    ''' <summary>
    ''' Legt fest ob Unterordner von der Suche ausgeschlossen werden sollen.
    ''' </summary>
    Public Property NoSubFolders() As Boolean
      Get
        Return mNoSubFolders
      End Get
      Set(ByVal value As Boolean)
        mNoSubFolders = value
      End Set
    End Property
  End Class


End Class
