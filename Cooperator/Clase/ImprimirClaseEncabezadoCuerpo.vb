Public Class ImprimirClaseEncabezadoCuerpo
    Dim idTabla As String
    Dim idParametro As String

    Public Sub Iniciar()
        For Each reg As regSistemaTabla In arrEstructura
            If reg.Orden = 1 Then
                idTabla = reg.nombre
                idParametro = "@" & idTabla
                Exit For
            End If
        Next
    End Sub

    'definicion de la entidad
    Public Sub EntidadOriginal(ByVal path As String, ByVal tabla As String)
        Dim nombre As String = "ent" & tabla

        If Mid$(nombre, Len(nombre) - 3, 3) <> ".vb" Then
            nombre = path & "\" & nombre & ".vb"
        Else
            nombre = path & "\" & nombre
        End If

        ' Defino variables
        Dim FileCls As Integer = FreeFile()
        Dim reg As regSistemaTabla


        ' Abro un PathCls de texto (si el mismo existía se reempleza)
        FileOpen(FileCls, nombre, OpenMode.Output)

        'importo las referencias
        PrintLine(FileCls, TAB(0), "Imports System.Data.SqlClient")
        PrintLine(FileCls, TAB(0), "Imports System.Data")
        PrintLine(FileCls, TAB(0), "Imports System.IO")
        PrintLine(FileCls, "")

        'defino la clase
        PrintLine(FileCls, "Public Class " & tabla & "_ent")
        PrintLine(FileCls, "")

        'defino las variables
        PrintLine(FileCls, TAB(5), "'defino las variables")
        PrintLine(FileCls, TAB(5), "Private dt As DataTable")
        PrintLine(FileCls, TAB(5), "Private dr As DataRow")
        PrintLine(FileCls, TAB(5), "Private da As SqlClient.SqlDataAdapter")
        PrintLine(FileCls, TAB(5), "Friend cnn As New Conexion")
        PrintLine(FileCls, TAB(5), "Friend cnntxt As New Conexion_txt")
        PrintLine(FileCls, "")

        'defino las propiedades y su variable

        PrintLine(FileCls, TAB(5), "'defino las propiedades")
        For Each reg In arrEstructura
            If reg.Orden = 1 Then
                PrintLine(FileCls, TAB(5), "Private _" & reg.nombre & " As " & reg.tiposql)
                PrintLine(FileCls, TAB(5), "Public ReadOnly Property " & reg.nombre & "() As " & reg.tiposql)
                PrintLine(FileCls, TAB(9), "Get")
                PrintLine(FileCls, TAB(13), "_" & reg.nombre & " = CInt(dr(" & """" & reg.nombre & """" & "))")
                PrintLine(FileCls, TAB(13), "Return _" & reg.nombre)
                PrintLine(FileCls, TAB(9), "End Get")
                PrintLine(FileCls, TAB(5), "end property")
                PrintLine(FileCls, "")
            Else
                ' Creo la variable local para usarse con la propiedad
                PrintLine(FileCls, TAB(5), "Private _" & reg.nombre & " As " & reg.tiposql)

                ' Creo la cabecera de la propiedad
                PrintLine(FileCls, TAB(5), "Public Property " & reg.nombre & "() As " & reg.tiposql)
                PrintLine(FileCls, TAB(9), "Get")

                'creo el nombre segun el tipo de dato
                Select Case reg.tiposql
                    Case "Int32"
                        PrintLine(FileCls, TAB(13), "_" & reg.nombre & " = CInt(dr(" & """" & reg.nombre & """" & "))")
                    Case "String"
                        PrintLine(FileCls, TAB(13), "_" & reg.nombre & " = CStr(dr(" & """" & reg.nombre & """" & "))")
                    Case "Boolean"
                        PrintLine(FileCls, TAB(13), "_" & reg.nombre & " = CBool(dr(" & """" & reg.nombre & """" & "))")
                    Case "Decimal"
                        PrintLine(FileCls, TAB(13), "_" & reg.nombre & " = CDec(dr(" & """" & reg.nombre & """" & "))")
                    Case "DateTime"
                        PrintLine(FileCls, TAB(13), "_" & reg.nombre & " = CDate(dr(" & """" & reg.nombre & """" & "))")
                    Case Else
                        PrintLine(FileCls, TAB(13), "_" & reg.nombre & " = dr(" & """" & reg.nombre & """" & ")")
                End Select

                PrintLine(FileCls, TAB(13), "Return _" & reg.nombre)
                PrintLine(FileCls, TAB(9), "End Get")
                PrintLine(FileCls, TAB(9), "Set(ByVal Value As " & reg.tiposql & ")")
                PrintLine(FileCls, TAB(13), "dr(" & """" & reg.nombre & """" & ") = Value")
                PrintLine(FileCls, TAB(9), "End Set")
                PrintLine(FileCls, TAB(5), "end property")

                ' Creo una línea divisoria (espacio en blanco)
                PrintLine(FileCls, "")
            End If
        Next

        'Public Sub Insertar()

        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(5), "'procedimiento insertar")
        PrintLine(FileCls, TAB(5), "Public Sub Insertar()")
        PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & tabla & """" & ")")
        PrintLine(FileCls, TAB(9), "da.MissingSchemaAction = MissingSchemaAction.AddWithKey")
        PrintLine(FileCls, TAB(9), "da.Fill(dt)")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "Me.AsignarTipos()")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "Dim cmdins As New SqlCommand(" & """" & "cop_" & tabla & "_Insert" & """" & ", cnn.Coneccion)")
        PrintLine(FileCls, TAB(9), "cmdins.CommandType = CommandType.StoredProcedure")
        PrintLine(FileCls, TAB(9), "da.InsertCommand = cmdins")
        PrintLine(FileCls, "")
        For Each reg In arrEstructura
            If reg.Orden = 1 Then
                idTabla = reg.nombre
                idParametro = "@" & reg.nombre
                PrintLine(FileCls, TAB(9), "Dim prm As SqlParameter")
                PrintLine(FileCls, TAB(9), "prm = da.InsertCommand.Parameters.Add(" & """" & "@" & reg.nombre & """" & ", SqlDbType." & reg.tipo & ", " & 0 & ", " & """" & reg.nombre & """" & ")")
                PrintLine(FileCls, TAB(9), "prm.Direction = ParameterDirection.Output")
            Else
                PrintLine(FileCls, TAB(9), "da.InsertCommand.Parameters.Add(" & """" & "@" & reg.nombre & """" & ", SqlDbType." & reg.tipo & ", " & reg.longitud & ", " & """" & reg.nombre & """" & ")")
            End If
        Next
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "dr = dt.NewRow()")
        For Each reg In arrEstructura
            If reg.Orden = 1 Then

            Else
                PrintLine(FileCls, TAB(9), "dr(" & """" & reg.nombre & """" & ") = " & reg.valorinicial)
            End If
        Next
        PrintLine(FileCls, TAB(9), "dt.Rows.Add(dr)")
        PrintLine(FileCls, TAB(9), "CrearComandoUpdate()")
        PrintLine(FileCls, TAB(5), "End Sub")
        PrintLine(FileCls, "")

        'Public Sub Modificar
        Dim id_temp As String = ""
        For Each reg In arrEstructura
            If reg.Orden = 1 Then
                id_temp = reg.nombre
                Exit For
            End If
        Next
        PrintLine(FileCls, TAB(5), "'procedimiento modificar")
        PrintLine(FileCls, TAB(5), "Public Sub Modificar(ByVal id" & tabla & " As Integer)")
        PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & tabla & """" & ")")
        PrintLine(FileCls, TAB(9), "da = New SqlClient.SqlDataAdapter(" & """" & "SELECT * FROM " & tabla & " WHERE " & id_temp & " = @id_" & tabla & """" & ", cnn.Coneccion)")
        PrintLine(FileCls, TAB(9), "da.SelectCommand.Parameters.AddWithValue(" & """" & "@id_" & tabla & """" & ", id" & tabla & ")")
        ' PrintLine(FileCls, TAB(9), "da = New SqlClient.SqlDataAdapter(" & """" & "SELECT * FROM " & tabla & " WHERE " & idTabla & " = " & """" & " & _")
        ' PrintLine(FileCls, TAB(9), "id" & tabla & ", cnn.Coneccion)")
        PrintLine(FileCls, TAB(9), "da.Fill(dt)")
        PrintLine(FileCls, TAB(9), "Me.AsignarTipos()")
        PrintLine(FileCls, TAB(9), "If dt.Rows.Count = 0 Then")
        PrintLine(FileCls, TAB(13), "Throw New Exception(" & """" & "No se a encontrado el Registro" & """" & ")")
        PrintLine(FileCls, TAB(9), "Else")
        PrintLine(FileCls, TAB(13), "dr = dt.Rows(0)")
        PrintLine(FileCls, TAB(9), "End If")
        PrintLine(FileCls, TAB(9), "CrearComandoUpdate()")
        PrintLine(FileCls, TAB(5), "End Sub")
        PrintLine(FileCls, "")

        'Public Sub Borrar


        PrintLine(FileCls, TAB(5), "'procedimiento borrar")
        PrintLine(FileCls, TAB(5), "Public Sub Borrar(ByVal id" & tabla & " As Integer)")
        PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & tabla & """" & ")")
        PrintLine(FileCls, TAB(9), "da = New SqlClient.SqlDataAdapter(" & """" & "SELECT * FROM " & tabla & " WHERE " & idTabla & " = " & """" & " & _")
        PrintLine(FileCls, TAB(9), "id" & tabla & ", cnn.Coneccion)")
        PrintLine(FileCls, TAB(9), "da.Fill(dt)")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "Me.AsignarTipos()")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "If dt.Rows.Count = 0 Then")
        PrintLine(FileCls, TAB(13), "Exit Sub")
        PrintLine(FileCls, TAB(9), "Else")
        PrintLine(FileCls, TAB(13), "dr = dt.Rows(0)")
        PrintLine(FileCls, TAB(9), "End If")
        PrintLine(FileCls, "")


        PrintLine(FileCls, TAB(9), "Try")
        PrintLine(FileCls, TAB(13), "Dim cmddel As New SqlCommand(" & """" & "cop_" & tabla & "_Delete" & """" & ", cnn.Coneccion)")
        PrintLine(FileCls, TAB(13), "cmddel.CommandType = CommandType.StoredProcedure")
        PrintLine(FileCls, TAB(13), "da.DeleteCommand = cmddel")
        PrintLine(FileCls, TAB(13), "Dim prm As SqlParameter")
        PrintLine(FileCls, TAB(13), "prm = da.DeleteCommand.Parameters.Add(" & """" & idParametro & """" & ", SqlDbType.Int, 4, " & """" & idTabla & """" & ")")
        PrintLine(FileCls, TAB(13), "dt.Rows(0).Delete()")
        PrintLine(FileCls, TAB(13), "CrearComandoUpdate()")
        PrintLine(FileCls, TAB(13), "Guardar()")
        PrintLine(FileCls, TAB(9), "Catch ex As Exception")
        PrintLine(FileCls, TAB(13), "If Err.Number = 5 Then")
        PrintLine(FileCls, TAB(13), "End If")
        PrintLine(FileCls, TAB(9), "End Try")
        PrintLine(FileCls, TAB(5), "End Sub")
        PrintLine(FileCls, "")

        'Private Sub AsignarTipos()

        PrintLine(FileCls, TAB(5), "'asigno el tipo de datos a los parametros")
        PrintLine(FileCls, TAB(5), "Private Sub AsignarTipos()")
        PrintLine(FileCls, TAB(9), "For Each dc As DataColumn In dt.Columns")
        PrintLine(FileCls, TAB(13), "Select Case dc.ColumnName")
        For Each reg In arrEstructura
            PrintLine(FileCls, TAB(16), "Case " & """" & reg.nombre & """")
            PrintLine(FileCls, TAB(19), "dc.DataType = Type.GetType(" & """" & "System." & reg.tiposql & """" & ")")
        Next
        PrintLine(FileCls, TAB(13), "End Select")
        PrintLine(FileCls, TAB(9), "Next")
        PrintLine(FileCls, TAB(5), "End Sub")
        PrintLine(FileCls, "")

        'Private Sub CrearComandoUpdate()

        PrintLine(FileCls, TAB(5), "'creo el commandbuilder")
        PrintLine(FileCls, TAB(5), "Private Sub CrearComandoUpdate()")
        PrintLine(FileCls, TAB(9), "Dim cmd As New SqlCommandBuilder(da)")
        PrintLine(FileCls, TAB(9), "da.UpdateCommand = cmd.GetUpdateCommand")
        PrintLine(FileCls, TAB(5), "End Sub")
        PrintLine(FileCls, "")

        'Public Sub Cancelar()

        PrintLine(FileCls, TAB(5), "'cancelo los cambios del datatable")
        PrintLine(FileCls, TAB(5), "Public Sub Cancelar()")
        PrintLine(FileCls, TAB(9), "dt.Clear()")
        PrintLine(FileCls, TAB(5), "End Sub")
        PrintLine(FileCls, "")

        'Public Function DataTable()

        PrintLine(FileCls, TAB(5), "'asigno el datatable")
        PrintLine(FileCls, TAB(5), "Public Function DataTable() As DataTable")
        PrintLine(FileCls, TAB(9), "Return dt")
        PrintLine(FileCls, TAB(5), "End Function")
        PrintLine(FileCls, "")

        'Public Sub Guardar()

        PrintLine(FileCls, TAB(5), "'actualizo la base de datos")
        PrintLine(FileCls, TAB(5), "Public Sub Guardar()")
        PrintLine(FileCls, TAB(9), "da.Update(dt)")
        PrintLine(FileCls, TAB(5), "End Sub")
        PrintLine(FileCls, "")

        'Public Function Cargar dt()
        id_temp = ""
        For Each reg In arrEstructura
            If reg.Orden = 1 Then
                id_temp = reg.nombre
                Exit For
            End If
        Next
        PrintLine(FileCls, TAB(5), "'funcion que carga el datatable")
        PrintLine(FileCls, TAB(5), "Public Function Cargar() As DataTable")
        PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & tabla & """" & ")")
        PrintLine(FileCls, TAB(9), "da = New SqlClient.SqlDataAdapter(" & """" & "SELECT * FROM " & tabla & " WHERE " & id_temp & " = 0" & """" & ", cnn.Coneccion)")
        PrintLine(FileCls, TAB(9), "da.MissingSchemaAction = MissingSchemaAction.AddWithKey")
        PrintLine(FileCls, TAB(9), "da.Fill(dt)")
        PrintLine(FileCls, TAB(9), "Return dt")
        PrintLine(FileCls, TAB(5), "End Function")
        PrintLine(FileCls, "")

        'Public Function Buscar

        PrintLine(FileCls, TAB(5), "'funcion de busqueda")
        PrintLine(FileCls, TAB(5), "Public Function Buscar(ByVal Nombre As String) As DataTable")
        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
        PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & tabla & "_Find" & """" & ", cnn.Coneccion)")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
        PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & "@nombre" & """" & ", SqlDbType.NChar, 30, " & """" & "nombre" & """" & ")")
        PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & "@nombre" & """" & ").Value = Nombre")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "Return odt")
        PrintLine(FileCls, TAB(5), "End Function")
        PrintLine(FileCls, "")

        'Public Function ConsultarTodo()

        PrintLine(FileCls, TAB(5), "'funcion de consulta")
        PrintLine(FileCls, TAB(5), "Public Function ConsultarTodo() As DataTable")
        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
        PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & tabla & "_GetAll" & """" & ", cnn.Coneccion)")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "Return odt")
        PrintLine(FileCls, TAB(5), "End Function")
        PrintLine(FileCls, "")

        'Public Function GetCmb()

        PrintLine(FileCls, TAB(5), "'funcion para llenar el combo")
        PrintLine(FileCls, TAB(5), "Public Function GetCmb() As DataTable")
        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
        PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & tabla & "_GetCmb" & """" & ", cnn.Coneccion)")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "Return odt")
        PrintLine(FileCls, TAB(5), "End Function")
        PrintLine(FileCls, "")

        'public function GetOne()

        PrintLine(FileCls, TAB(5), "'funcion que trae un registro poniendo el id")
        PrintLine(FileCls, TAB(5), "Public Function GetOne(ByVal id_" & tabla & " As Integer) As DataTable")
        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
        PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & tabla & "_GetOne" & """" & ", cnn.Coneccion)")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
        PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & idParametro & """" & ", SqlDbType.Int, 4, " & """" & idTabla & """" & ")")
        PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & idParametro & """" & ").Value = id_" & tabla)
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "Return odt")
        PrintLine(FileCls, TAB(5), "End Function")
        PrintLine(FileCls, "")

        'Public Function Exist

        PrintLine(FileCls, TAB(5), "'controla si existe el registro en la base de datos")
        PrintLine(FileCls, TAB(5), "Public Function Exist() As Boolean")
        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
        PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & tabla & "_Exist" & """" & ", cnn.Coneccion)")
        PrintLine(FileCls, TAB(9), "Dim Total As Integer")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")

        For Each reg In arrEstructura
            If reg.Orden = 1 Then
            Else
                PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & "@" & reg.nombre & """" & ", SqlDbType." & reg.tipo & ", " & reg.longitud & ", " & """" & reg.nombre & """" & ")")
                PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & "@" & reg.nombre & """" & ").Value = Me." & reg.nombre)
            End If
        Next
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
        PrintLine(FileCls, TAB(9), "Total = CInt(odt.Rows(0).Item(" & """" & "Total" & """" & ").ToString)")
        PrintLine(FileCls, TAB(9), "If Total = 0 Then")
        PrintLine(FileCls, TAB(13), "Return False  'NO EXISTE")
        PrintLine(FileCls, TAB(9), "Else")
        PrintLine(FileCls, TAB(13), "Return True 'SI EXISTE")
        PrintLine(FileCls, TAB(9), "End If")
        PrintLine(FileCls, TAB(5), "End Function")
        PrintLine(FileCls, "")

        'borra toda la tabla
        PrintLine(FileCls, TAB(5), "'borra todos los datos de la tabla")
        PrintLine(FileCls, TAB(5), "Public Sub BorrarTodo()")
        PrintLine(FileCls, TAB(9), "Dim oConexion As New Conexion")
        PrintLine(FileCls, TAB(9), "Dim Command As SqlCommand = New SqlCommand()")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "Command.Connection = oConexion.Coneccion")
        PrintLine(FileCls, TAB(9), "Command.CommandText = ""DELETE FROM " & tabla & """")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "oConexion.Abrir()")
        PrintLine(FileCls, TAB(9), "Command.ExecuteNonQuery()")
        PrintLine(FileCls, TAB(9), "oConexion.Cerrar()")
        PrintLine(FileCls, TAB(5), "End Sub")
        PrintLine(FileCls, "")

        'truncate toda la tabla
        PrintLine(FileCls, TAB(5), "'borra todos los datos de la tabla")
        PrintLine(FileCls, TAB(5), "Public Sub Truncate()")
        PrintLine(FileCls, TAB(9), "Dim oConexion As New Conexion")
        PrintLine(FileCls, TAB(9), "Dim Command As SqlCommand = New SqlCommand()")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "Command.Connection = oConexion.Coneccion")
        PrintLine(FileCls, TAB(9), "Command.CommandText = ""TRUNCATE TABLE " & tabla & """")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "oConexion.Abrir()")
        PrintLine(FileCls, TAB(9), "Command.ExecuteNonQuery()")
        PrintLine(FileCls, TAB(9), "oConexion.Cerrar()")
        PrintLine(FileCls, TAB(5), "End Sub")
        PrintLine(FileCls, "")

        'borra toda la tabla
        PrintLine(FileCls, TAB(5), "'inserta un registro en la tabla")
        PrintLine(FileCls, TAB(5), "Public Sub InsertOne()")
        PrintLine(FileCls, TAB(9), "Dim oConexion As New Conexion")
        PrintLine(FileCls, TAB(9), "Dim Command As SqlCommand = New SqlCommand()")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "Command.Connection = oConexion.Coneccion")
        PrintLine(FileCls, TAB(9), "Command.CommandText = " & """" & "cop_" & tabla & "_InsertOne" & """")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "Command.CommandType = CommandType.StoredProcedure")
        PrintLine(FileCls, TAB(9), "oConexion.Abrir()")
        PrintLine(FileCls, TAB(9), "Command.ExecuteNonQuery()")
        PrintLine(FileCls, TAB(9), "oConexion.Cerrar()")
        PrintLine(FileCls, TAB(5), "End Sub")
        PrintLine(FileCls, "")


        'consulta el archivo txt para importarlo
        PrintLine(FileCls, TAB(5), "'importa los datos de una campaña desde el txt")
        PrintLine(FileCls, TAB(5), "Public Function Cargar_Archivo(ByVal path As String) As DataTable")
        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable(" & """" & tabla & """" & ")")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "Dim oda As New System.Data.OleDb.OleDbDataAdapter( _")
        PrintLine(FileCls, TAB(9), """SELECT * FROM " & tabla & ".txt"", cnntxt.Coneccion(path))")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "Return odt")
        PrintLine(FileCls, TAB(5), "End Function")
        PrintLine(FileCls, "")
        PrintLine(FileCls, "")

        PrintLine(FileCls, "End Class")

        ' Cierro el PathCls de versión
        FileClose(FileCls)


    End Sub

    'clase heredada
    Public Sub ClaseOriginal(ByVal path As String, ByVal tabla As String)
        ' Defino variables
        Dim FileCls As Integer = FreeFile()
        Dim nombre As String = "cls" & tabla

        If Mid$(nombre, Len(nombre) - 3, 3) <> ".vb" Then
            nombre = path & "\cls" & tabla & ".vb"
        Else
            nombre = path & "\cls" & tabla & ".vb"
        End If

        'Abro un PathCls de texto (si el mismo existía se reempleza)
        FileOpen(FileCls, nombre, OpenMode.Output)

        'importo las referencias
        PrintLine(FileCls, TAB(0), "Imports System.Data.SqlClient")
        PrintLine(FileCls, TAB(0), "Imports System.Data")
        PrintLine(FileCls, TAB(0), "Imports System.IO")
        PrintLine(FileCls, "")

        'defino la clase
        PrintLine(FileCls, "Public Class " & tabla)
        PrintLine(FileCls, TAB(5), "Inherits " & tabla & "_ent")
        PrintLine(FileCls, "")

        'Public Function Consultar con oda

        PrintLine(FileCls, TAB(5), "'ejemplo de consulta con parametros")
        PrintLine(FileCls, TAB(5), "Public Function Ejemplo(ByVal id_" & tabla & " As Integer) As DataTable")
        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
        PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & tabla & """" & ", cnn.Coneccion)")
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
        PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & idParametro & """" & ", SqlDbType.Int, 4, " & """" & idTabla & """" & ")")
        PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & idParametro & """" & ").Value = id_" & tabla)
        PrintLine(FileCls, "")
        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
        PrintLine(FileCls, TAB(9), "Return odt")
        PrintLine(FileCls, TAB(5), "End Function")
        PrintLine(FileCls, "")




        PrintLine(FileCls, "End Class")

        ' Cierro el PathCls de versión
        FileClose(FileCls)
    End Sub

End Class
