Imports System.IO

Module Inicio

    Public Function GetFileContents(ByVal FullPath As String, _
   Optional ByRef ErrInfo As String = "") As String

        Dim strContents As String
        Dim objReader As StreamReader
        Try

            objReader = New StreamReader(FullPath)
            strContents = objReader.ReadToEnd()
            objReader.Close()
            Return strContents
        Catch Ex As Exception
            ErrInfo = Ex.Message
            Return ""
        End Try
    End Function

    Public Function SaveTextToFile(ByVal strData As String, _
     ByVal FullPath As String, _
       Optional ByVal ErrInfo As String = "") As Boolean

        'Dim Contents As String
        Dim bAns As Boolean = False
        Dim objReader As StreamWriter
        Try


            objReader = New StreamWriter(FullPath)
            objReader.Write(strData)
            objReader.Close()
            bAns = True
        Catch Ex As Exception
            ErrInfo = Ex.Message

        End Try
        Return bAns
    End Function

    ' Defino la estructura del array de campos
    Public Structure regSistemaTabla
        Dim nombre As String
        Dim tipo As String
        Dim longitud As String
        Dim escala As String
        Dim nulo As String
        Dim precicion As String
        Dim indice As String
        Dim tiposql As String
        Dim valorinicial As Object
        Dim valorinsert As Object
        Dim sptamaño As String
        Dim tipopocket As String
        Dim LongitudPocket As String
        Dim Orden As String
        Dim es_referenciado As Boolean
        Dim referencia As String
    End Structure

    Public oClase As New ImprimirClase
    Public oTest As New ImprimirTest
    Public oSql As New ImprimirSql
    Public oAbm As New ImprimirAbm
    Public oAbmEncabezadoCuerpo As New ImprimirAbmEncabezadoCuerpo

    Public arrEstructura() As regSistemaTabla

    Public Function FormatoNombre(ByVal nombre As String) As String
        Dim strTemp As String = ""

        ' Doy formato al nombre de la tabla
        strTemp = nombre.Trim.ToLower
        strTemp = strTemp.Replace(" ", Nothing)
        strTemp = Mid$(strTemp, 1, 1).ToUpper & Mid$(strTemp, 2).ToLower

        Return strTemp
    End Function

    Public Function FormatoNombreLabels(ByVal nombre As String) As String
        Dim strTemp As String = ""

        ' Doy formato al nombre de la tabla
        strTemp = nombre.Trim.ToLower
        strTemp = strTemp.Replace(" ", Nothing)
        strTemp = Mid$(strTemp, 1, 1).ToUpper & Mid$(strTemp, 2).ToLower

        strTemp = strTemp.Replace("_", " ")
        Return strTemp
    End Function

End Module
