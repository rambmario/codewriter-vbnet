Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Data.Common
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.IO
Imports System.Reflection
Imports System.IO.IsolatedStorage

Public Class frmPrincipal
    Dim NumeroFilas As Integer


    ' Defino el array donde se guardarán los datos de la tabla
    'Dim arrEstructura() As regSistemaTabla

    Const baseCon As String = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog={1};Data Source={0}"
    Dim BanderaSP As Integer = 0

    Private Sub frmPrincipal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtCadena.SelectAll()
        Me.btnServer.Focus()

        Dim strVersion As String = ""
        strVersion = My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & _
        My.Application.Info.Version.Build & "." & My.Application.Info.Version.Revision
        Me.Text = "Cooperator - Version: " & strVersion


        Me.cmbServer.Text = "(local)\SQLExpress"
        Try
            Dim con As String = String.Format(baseCon, cmbServer.Text, "master")
            Dim Connect As New SqlClient.SqlConnection(con)
            Connect.Open()
            Dim tbdb As DataTable = Connect.GetSchema("Databases")
            Connect.Close()
            lstBaseDato.DisplayMember = tbdb.Columns(0).ColumnName
            lstBaseDato.ValueMember = lstBaseDato.DisplayMember
            lstBaseDato.DataSource = tbdb.DefaultView
            btnTabla.Enabled = True
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub formGeneradorClasesSQL_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Marco el primer casillero de texto
        txtCadena.SelectAll()
    End Sub

    Private Sub chkNombresAutomaticos_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNombresAutomaticos.CheckedChanged
        ' Verirfico el estado del campo nombres automáticos
        If chkNombresAutomaticos.Checked Then
            ' Deshabilito los casilleros
            txtArchivo.Enabled = False
            txtClase.Enabled = False
            txtArchivoSql.Enabled = False

            ' Fuerzo a recargarse los casilleros de texto
            Call GenerarNombres()
        Else
            ' Habilito los casilleros
            txtArchivoSql.Enabled = True
            txtArchivo.Enabled = True
            txtClase.Enabled = True

            ' Limpio los nombres que ya hubiese
            txtArchivoSql.Text = ""
            txtArchivo.Text = ""
            txtClase.Text = ""
        End If
    End Sub

    Private Sub chkTodasLasTablas_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTodasLasTablas.CheckedChanged
        ' Verifico si está o no marcado el control para crear todas las clases de una db
        If chkTodasLasTablas.Checked Then
            chkNombresAutomaticos.Enabled = False
            chkNombresAutomaticos.Checked = False

            txtTabla.Enabled = False
            txtTabla.Text = ""

            txtArchivo.Enabled = False
            txtArchivo.Text = ""

            txtClase.Enabled = False
            txtClase.Text = ""

            txtArchivoSql.Enabled = False
            txtArchivoSql.Text = ""
        Else
            txtTabla.Enabled = True
            txtTabla.Text = ""

            chkNombresAutomaticos.Enabled = True
            chkNombresAutomaticos.Checked = True
        End If

        ' Verifico los datos
        Call Verificar(sender, e)
    End Sub

    Private Sub btnGenerarClase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerarClase.Click
        If Me.txtPath.Text = "" Then
            MessageBox.Show("HAY QUE PONER UN PATH")
            Exit Sub
        End If

        ' Verifico si se trata de una o todas las tablas
        If Not chkTodasLasTablas.Checked Then
            ' Solicito confirmación antes de crear la clase
            If MessageBox.Show("Desea Crear la Clase '" & txtClase.Text & "' Para la Tabla '" & txtTabla.Text & "' ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                ' Cargo la estructura de la tabla en un array "modular"
                Call Cargar(txtCadena.Text, txtTabla.Text)

                ' Genero la clase
                ' Call GenerarClase(txtArchivo.Text, txtClase.Text)
                ' oClase.ClaseOriginal(Me.txtPath.Text, Me.txtTabla.Text)

                oClase.Iniciar()
                oClase.EntidadOriginal(Me.txtPath.Text, Me.txtTabla.Text)
                oClase.ClaseOriginal(Me.txtPath.Text, Me.txtTabla.Text)

                If Me.chkPocket.Checked = True Then
                    oClase.EntidadPocket(Me.txtPath.Text, Me.txtTabla.Text, NumeroFilas, Me.chkTransactor.Checked)
                    oClase.ClasePocket(Me.txtPath.Text, Me.txtTabla.Text)
                End If

                oTest.Iniciar()
                oTest.ImprimirTest(Me.lstBaseDato.Text, Me.txtPath.Text, Me.txtTabla.Text, _
                Me.chkTestInsert.Checked, Me.chkTestDelete.Checked, Me.chkTestGetall.Checked, Me.chkTestGetone.Checked, _
                Me.chkTestGetcmb.Checked, Me.chkTestUpdate.Checked, Me.chkTestExist.Checked, Me.chkTestFind.Checked)

                oSql.Iniciar()
                oSql.ImprimirSps(Me.lstBaseDato.Text, Me.txtPath.Text, Me.txtTabla.Text, NumeroFilas, _
                Me.chkInsert.Checked, Me.chkDelete.Checked, Me.chkGetAll.Checked, Me.chkGetOne.Checked, Me.chkGetCmb.Checked, _
                Me.chkUpdate.Checked, Me.chkExist.Checked, Me.chkFind.Checked, Me.chkInsertOne.Checked, _
                Me.chkUpdateID.Checked, Me.chkDeleteEncabezado.Checked, Me.chkGetAllEncabezadoOne.Checked)


                If Me.chkMaestroDetalle.Checked = False Then
                    oAbm.Iniciar()
                    oAbm.ImprimirAbm(Me.txtPath.Text, Me.txtTabla.Text, Me.chkAbm.Checked, Me.chkDelete.Checked, Me.chkUsuario.Checked, NumeroFilas, Me.txtTituloFormulario.Text, Me.lstBaseDato.Text)
                    If Me.chkPocket.Checked = True Then
                        oAbm.ImprimirAbmPocket(Me.txtPath.Text, Me.txtTabla.Text, Me.chkAbm.Checked, Me.chkDelete.Checked, Me.chkUsuario.Checked, Me.chkTransactor.Checked)
                    End If
                Else
                    oAbmEncabezadoCuerpo.Iniciar()
                    oAbmEncabezadoCuerpo.ImprimirAbm(Me.txtPath.Text, Me.txtTabla.Text, Me.chkAbm.Checked, Me.chkDetalle.Checked, Me.chkUsuario.Checked)
                    oSql.ImprimirSps(Me.lstBaseDato.Text, Me.txtPath.Text, Me.txtTabla.Text, NumeroFilas, _
                    Me.chkInsert.Checked, Me.chkDelete.Checked, Me.chkGetAll.Checked, Me.chkGetOne.Checked, Me.chkGetCmb.Checked, _
                    Me.chkUpdate.Checked, Me.chkExist.Checked, Me.chkFind.Checked, Me.chkInsertOne.Checked, _
                    Me.chkUpdateID.Checked, Me.chkDeleteEncabezado.Checked, Me.chkGetAllEncabezadoOne.Checked)
                End If

                If Me.chkAgregarClaseEncabezado.Checked = True Then
                    oClase.GetEncabezadoCuerpo(Me.txtPath4.Text, Me.txtTabla.Text)
                End If

                ' Indico que se ha generado la clase
                MessageBox.Show("Se Generó la Clase '" & txtClase.Text & "' en el Archivo '" & txtArchivo.Text & "' de la Tabla '" & txtTabla.Text & "'." & Chr(13) & Chr(13) & "El Archivo se Guardó en C:\." & Chr(13) & Chr(13) & "Copiar la Clase en el Proyecto y Agregarla con --> Agregar elemento existente", "GENERACION DE CLASES", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.btnSP.Enabled = True
            End If
        Else
            Dim cn As New SqlConnection(Me.txtCadenaConexion.Text)
            Dim da As New SqlDataAdapter("SELECT TOP 100 PERCENT name FROM dbo.sysobjects WHERE (type = 'U') AND name <> 'dtproperties' ORDER BY name", cn)

            Dim cb As New SqlCommandBuilder(da)
            Dim cbSistema As New SqlCommandBuilder(da)
            Dim dt As New DataTable
            da.Fill(dt)
            ' Solicito confirmación antes de crear todas las tablas
            If MessageBox.Show("Desea las Clases Para Todas las Tablas de la Base de Datos Seleccionada ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Me.ProgressBar1.Value = Me.ProgressBar1.Minimum
                Me.ProgressBar1.Maximum = dt.Rows.Count - 1
                Me.ProgressBar2.Value = Me.ProgressBar2.Minimum
                Me.ProgressBar2.Maximum = dt.Rows.Count - 1

                Dim indice = 0
                For Each row As DataRow In dt.Rows
                    txtTabla.Text = row("name")
                    Me.GenerarNombres()
                    Call Cargar(txtCadena.Text, txtTabla.Text)

                    oClase.Iniciar()
                    oClase.EntidadOriginal(Me.txtPath.Text, Me.txtTabla.Text)
                    oClase.ClaseOriginal(Me.txtPath.Text, Me.txtTabla.Text)

                    If Me.chkPocket.Checked = True Then
                        oClase.EntidadPocket(Me.txtPath.Text, Me.txtTabla.Text, NumeroFilas, Me.chkTransactor.Checked)
                        oClase.ClasePocket(Me.txtPath.Text, Me.txtTabla.Text)
                    End If

                    oTest.Iniciar()
                    oTest.ImprimirTest(Me.lstBaseDato.Text, Me.txtPath.Text, Me.txtTabla.Text, _
                    Me.chkTestInsert.Checked, Me.chkTestDelete.Checked, Me.chkTestGetall.Checked, Me.chkTestGetone.Checked, _
                    Me.chkTestGetcmb.Checked, Me.chkTestUpdate.Checked, Me.chkTestExist.Checked, Me.chkTestFind.Checked)

                    oSql.Iniciar()
                    oSql.ImprimirSps(Me.lstBaseDato.Text, Me.txtPath.Text, Me.txtTabla.Text, NumeroFilas, _
                    Me.chkInsert.Checked, Me.chkDelete.Checked, Me.chkGetAll.Checked, Me.chkGetOne.Checked, Me.chkGetCmb.Checked, _
                    Me.chkUpdate.Checked, Me.chkExist.Checked, Me.chkFind.Checked, Me.chkInsertOne.Checked, _
                    Me.chkUpdateID.Checked, Me.chkDeleteEncabezado.Checked, Me.chkGetAllEncabezadoOne.Checked)

                    If Me.chkMaestroDetalle.Checked = False Then
                        oAbm.Iniciar()
                        oAbm.ImprimirAbm(Me.txtPath.Text, Me.txtTabla.Text, Me.chkAbm.Checked, Me.chkDelete.Checked, Me.chkUsuario.Checked, NumeroFilas, Me.txtTituloFormulario.Text, Me.lstBaseDato.Text)
                        If Me.chkPocket.Checked = True Then
                            oAbm.ImprimirAbmPocket(Me.txtPath.Text, Me.txtTabla.Text, Me.chkAbm.Checked, Me.chkDelete.Checked, Me.chkUsuario.Checked, Me.chkTransactor.Checked)
                        End If
                    Else
                        oAbmEncabezadoCuerpo.Iniciar()
                        oAbmEncabezadoCuerpo.ImprimirAbm(Me.txtPath.Text, Me.txtTabla.Text, Me.chkAbm.Checked, Me.chkDetalle.Checked, Me.chkUsuario.Checked)
                        oSql.ImprimirSps(Me.lstBaseDato.Text, Me.txtPath.Text, Me.txtTabla.Text, NumeroFilas, _
                        Me.chkInsert.Checked, Me.chkDelete.Checked, Me.chkGetAll.Checked, Me.chkGetOne.Checked, Me.chkGetCmb.Checked, _
                        Me.chkUpdate.Checked, Me.chkExist.Checked, Me.chkFind.Checked, Me.chkInsertOne.Checked, _
                        Me.chkUpdateID.Checked, Me.chkDeleteEncabezado.Checked, Me.chkGetAllEncabezadoOne.Checked)
                    End If

                    'Call GenerarClase(txtArchivo.Text, txtClase.Text)
                    Me.ProgressBar1.Value = indice
                    Me.ProgressBar2.Value = indice

                    indice = indice + 1
                Next
                MessageBox.Show("Se Generó la Clase Para Todas las Tablas de la Base de Datos." & Chr(13) & Chr(13) & "Los Archivos se Guardaron en C:\." & Chr(13) & Chr(13) & "Copiar la Clase en el Proyecto y Agregarla con --> Agregar elemento existente", "GENERACION DE CLASES", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.btnTodoSp.Enabled = True
            End If
        End If
    End Sub

    Private Sub CambioNombreTabla(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTabla.KeyUp
        ' Verifico si se está en nombres automáticos
        If chkNombresAutomaticos.Checked Then
            Call GenerarNombres()
        End If
    End Sub

    Private Sub GenerarNombres()
        ' Defino variables
        Dim tabla As String

        ' Doy formato al nombre de la tabla
        tabla = txtTabla.Text.Trim.ToLower
        tabla = tabla.Replace(" ", Nothing)
        tabla = Mid$(tabla, 1, 1).ToUpper & Mid$(tabla, 2).ToLower
        txtTabla.Text = tabla

        ' Genero los nombres en base al nombre de la tabla
        If txtTabla.Text <> "" Then
            txtArchivoSql.Text = "cop" & tabla
            txtArchivo.Text = "ent" & tabla
            txtClase.Text = tabla
        Else
            txtArchivoSql.Text = ""
            txtArchivo.Text = ""
            txtClase.Text = ""
        End If
    End Sub

    Private Sub LimpiarEspaciosFinales(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCadena.LostFocus, txtTabla.LostFocus, txtArchivo.LostFocus, txtClase.LostFocus, txtArchivoSql.LostFocus
        ' Limpio los espacios finales que pudiese llegar a tener la cadena
        sender.Text.Trim()
    End Sub

    Private Sub SeleccionarTexto(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCadena.GotFocus, txtTabla.GotFocus, txtArchivo.GotFocus, txtClase.GotFocus, txtArchivoSql.GotFocus
        ' Selecciono el texto del casillero
        sender.SelectAll()
    End Sub

    Private Sub Verificar(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCadena.TextChanged, txtTabla.TextChanged, txtArchivo.TextChanged, txtClase.TextChanged, txtArchivoSql.TextChanged
        ' Verifico que todos los datos hayan sido completados
        If chkTodasLasTablas.Checked Then
            If txtCadena.Text <> "" Then
                btnGenerarClase.Enabled = True
            Else
                btnGenerarClase.Enabled = False
            End If
        Else
            If txtCadena.Text <> "" And txtTabla.Text <> "" And txtArchivo.Text <> "" And txtClase.Text <> "" Then
                btnGenerarClase.Enabled = True
            Else
                btnGenerarClase.Enabled = False
            End If
        End If
    End Sub

    ' Este procedimiento recibe una cadena conexión y el nombre de una tabla y carga un array con el nombre y tiposql de cada uno de los campos de la tabla.
    ' El array es de tiposql regSistemaTabla y su alcance es "modular". El mismo es una estructura conteniendo nombre y tiposql del campo. Los tiposqls de los campos se
    ' guardan en el formato "oficial" de .NET (por ejemplo los Integer son Int32, etc.).

    Dim hj As Integer = 1
    Dim lk As Integer = 1
    Dim FileInsert2 As Integer = FreeFile()
    Dim regInsert2 As regSistemaTabla
    Dim regInsert3 As regSistemaTabla

    Private Sub generarClaseTruncate(ByVal PathCls As String, ByVal clase As String)
        If lk = 1 Then
            ' Doy formato al nombre del PathCls
            Dim PathInsert As String
            Dim PathFrm As String
            Dim PathFrmdetalle As String

            'seteo el path para los otros archivos
            PathInsert = Me.txtPath.Text & "\Truncates.sql"
            PathFrm = PathCls
            PathFrmdetalle = PathCls


            ' Defino variables
            Dim FileCls As Integer = FreeFile()
            Dim reg As regSistemaTabla
            Dim idTabla As String
            Dim idParametro As String

            FileOpen(FileInsert2, PathInsert, OpenMode.Output)
            Dim Prefijo As String = "cop_"
        Else


        End If

        Dim Contador As Integer
        lk = lk + 1

        'insertar el registro ninguno

        '  PrintLine(FileInsert2, TAB(0), "USE [" & Me.lstBaseDato.Text.Trim & "]")
        '  PrintLine(FileInsert2, TAB(0), "GO")
        PrintLine(FileInsert2, "")
        PrintLine(FileInsert2, TAB(0), "TRUNCATE TABLE [dbo].[" & Me.txtTabla.Text & "]")
        PrintLine(FileInsert2, TAB(0), "GO")
        PrintLine(FileInsert2, "")


        '   FileClose(FileInsert2)
    End Sub

    Private Sub generarClaseInsertOne(ByVal PathCls As String, ByVal clase As String)
        Dim Prefijo As String = "cop_"
        If lk = 1 Then
            ' Doy formato al nombre del PathCls
            Dim PathInsert As String
            Dim PathFrm As String
            Dim PathFrmdetalle As String

            'seteo el path para los otros archivos
            PathInsert = Me.txtPath.Text & "\InsertOne.sql"
            PathFrm = PathCls
            PathFrmdetalle = PathCls

            ' Defino variables
            Dim FileCls As Integer = FreeFile()

            FileOpen(FileInsert2, PathInsert, OpenMode.Output)

        Else
        End If

        lk = lk + 1
        Dim contador As Integer
        Dim regInsert As regSistemaTabla

        PrintLine(FileInsert2, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_InsertOne]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
        PrintLine(FileInsert2, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_InsertOne]")
        PrintLine(FileInsert2, TAB(0), "GO")
        PrintLine(FileInsert2, "")
        PrintLine(FileInsert2, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_InsertOne]")
        PrintLine(FileInsert2, TAB(0), "AS")
        PrintLine(FileInsert2, "")
        PrintLine(FileInsert2, TAB(0), "INSERT INTO [dbo].[" & Me.txtTabla.Text & "]")
        PrintLine(FileInsert2, TAB(0), "(")
        Contador = 1
        For Each regInsert In arrEstructura
            If regInsert.indice = 9 Or regInsert.indice = 1 Then
            ElseIf Contador = NumeroFilas Then
                PrintLine(FileInsert2, TAB(5), "[" & regInsert.nombre & "]")
            Else
                PrintLine(FileInsert2, TAB(5), "[" & regInsert.nombre & "],")
            End If
            Contador = Contador + 1
        Next
        PrintLine(FileInsert2, ")")
        PrintLine(FileInsert2, TAB(0), "VALUES")
        PrintLine(FileInsert2, TAB(0), "(")
        Contador = 1
        For Each regInsert In arrEstructura
            If regInsert.indice = 9 Or regInsert.indice = 1 Then
            ElseIf Contador = NumeroFilas Then
                PrintLine(FileInsert2, TAB(5), regInsert.valorinsert)
            Else
                PrintLine(FileInsert2, TAB(5), regInsert.valorinsert & ",")
            End If
            Contador = Contador + 1
        Next
        PrintLine(FileInsert2, ")")

        ' If Me.chkGo.Checked = True Then
        PrintLine(FileInsert2, TAB(0), "GO")
        '  End If

        PrintLine(FileInsert2, "")

    End Sub

    Private Sub GenerarClaseInsert(ByVal PathCls As String, ByVal clase As String)

        If hj = 1 Then
            ' Doy formato al nombre del PathCls
            Dim PathInsert As String
            Dim PathFrm As String
            Dim PathFrmdetalle As String

            'seteo el path para los otros archivos
            PathInsert = Me.txtPath.Text & "\Insertar_nulos.sql"
            PathFrm = PathCls
            PathFrmdetalle = PathCls


            ' Defino variables
            Dim FileCls As Integer = FreeFile()
            Dim reg As regSistemaTabla
            Dim idTabla As String
            Dim idParametro As String

            FileOpen(FileInsert2, PathInsert, OpenMode.Output)
            Dim Prefijo As String = "cop_"
        Else


        End If

        Dim Contador As Integer
        hj = hj + 1

        'insertar el registro ninguno

        ' PrintLine(FileInsert2, TAB(0), "USE [" & Me.lstBaseDato.Text.Trim & "]")
        ' PrintLine(FileInsert2, TAB(0), "GO")
        PrintLine(FileInsert2, "")
        PrintLine(FileInsert2, TAB(0), "INSERT INTO [dbo].[" & Me.txtTabla.Text & "]")
        PrintLine(FileInsert2, TAB(0), "(")
        Contador = 1
        For Each regInsert2 In arrEstructura
            If regInsert2.indice = 9 Or regInsert2.indice = 1 Then
            ElseIf Contador = NumeroFilas Then
                PrintLine(FileInsert2, TAB(12), "[" & regInsert2.nombre & "]")
            Else
                PrintLine(FileInsert2, TAB(12), "[" & regInsert2.nombre & "],")
            End If
            Contador = Contador + 1
        Next
        PrintLine(FileInsert2, ")")
        PrintLine(FileInsert2, TAB(0), "VALUES")
        PrintLine(FileInsert2, TAB(0), "(")
        Contador = 1
        For Each regInsert2 In arrEstructura
            If regInsert2.indice = 9 Or regInsert2.indice = 1 Then
            ElseIf Contador = NumeroFilas Then
                PrintLine(FileInsert2, TAB(12), regInsert2.valorinsert)
            Else
                PrintLine(FileInsert2, TAB(12), regInsert2.valorinsert & ",")
            End If
            Contador = Contador + 1
        Next
        PrintLine(FileInsert2, ")")

        If Me.chkGo.Checked = True Then
            PrintLine(FileInsert2, TAB(0), "GO")
        End If

        PrintLine(FileInsert2, "")


        '   FileClose(FileInsert2)

    End Sub


    Private Sub generarClaseKey(ByVal PathCls As String, ByVal clase As String)
        If lk = 1 Then
            ' Doy formato al nombre del PathCls
            Dim PathInsert As String
            Dim PathFrm As String
            Dim PathFrmdetalle As String

            'seteo el path para los otros archivos
            PathInsert = Me.txtPath.Text & "\Clave_suplente.sql"
            PathFrm = PathCls
            PathFrmdetalle = PathCls


            ' Defino variables
            Dim FileCls As Integer = FreeFile()
            Dim reg As regSistemaTabla
            Dim idTabla As String
            Dim idParametro As String

            FileOpen(FileInsert2, PathInsert, OpenMode.Output)
            Dim Prefijo As String = "cop_"
        Else


        End If

        Dim Contador As Integer
        lk = lk + 1

        'insertar el registro ninguno

        ' PrintLine(FileInsert2, TAB(0), "USE [" & Me.lstBaseDato.Text.Trim & "]")
        ' PrintLine(FileInsert2, TAB(0), "GO")
        PrintLine(FileInsert2, "")
        PrintLine(FileInsert2, TAB(0), "ALTER TABLE dbo." & Me.txtTabla.Text & " ADD")
        PrintLine(FileInsert2, TAB(5), "id_" & Me.txtTabla.Text & "_2 int NULL")
        PrintLine(FileInsert2, "GO")
        PrintLine(FileInsert2, "")

        '   FileClose(FileInsert2)
    End Sub


    Private Sub generarTablaPocket(ByVal PathCls As String, ByVal clase As String)
        If lk = 1 Then
            ' Doy formato al nombre del PathCls
            Dim PathInsert As String
            Dim PathFrm As String
            Dim PathFrmdetalle As String

            'seteo el path para los otros archivos
            PathInsert = Me.txtPath.Text & "\Tabla_Pocket.sql"
            PathFrm = PathCls
            PathFrmdetalle = PathCls


            ' Defino variables
            Dim FileCls As Integer = FreeFile()
            Dim reg As regSistemaTabla
            Dim idTabla As String
            Dim idParametro As String

            FileOpen(FileInsert2, PathInsert, OpenMode.Output)
            Dim Prefijo As String = "cop_"
        Else


        End If

        Dim Contador As Integer
        lk = lk + 1

        Dim Contador3 As Integer = 1
        ' PrintLine(FileInsert2, TAB(0), "USE [" & Me.lstBaseDato.Text.Trim & "]")
        ' PrintLine(FileInsert2, TAB(0), "GO")
        PrintLine(FileInsert2, "")

        PrintLine(FileInsert2, TAB(0), "CREATE TABLE [" & Me.txtTabla.Text.Trim & "](")

        For Each regInsert3 In arrEstructura
            If regInsert3.indice = 9 Or regInsert3.indice = 1 Then
                PrintLine(FileInsert2, TAB(5), "[" & regInsert3.nombre & "] [int] IDENTITY(1,1),")
            ElseIf Contador = NumeroFilas Then
                PrintLine(FileInsert2, TAB(5), "[" & regInsert3.nombre & "] " & regInsert3.tipopocket & "")
            Else
                PrintLine(FileInsert2, TAB(5), "[" & regInsert3.nombre & "] " & regInsert3.tipopocket & ",")
            End If
            Contador3 = Contador3 + 1
        Next

        PrintLine(FileInsert2, TAB(5), "CONSTRAINT pk_" & Me.txtTabla.Text.Trim & " PRIMARY KEY([id_" & Me.txtTabla.Text.Trim & "]))")

        PrintLine(FileInsert2, TAB(0), "GO")
        PrintLine(FileInsert2, "")


        '   FileClose(FileInsert2)
    End Sub

    Private Sub GenerarClaseExist(ByVal PathCls As String, ByVal clase As String)
        If lk = 1 Then
            ' Doy formato al nombre del PathCls
            Dim PathInsert As String
            Dim PathFrm As String
            Dim PathFrmdetalle As String

            'seteo el path para los otros archivos
            PathInsert = Me.txtPath.Text & "\Exist.sql"
            PathFrm = PathCls
            PathFrmdetalle = PathCls


            ' Defino variables
            Dim FileCls As Integer = FreeFile()
            Dim reg As regSistemaTabla
            Dim idTabla As String
            Dim idParametro As String

            FileOpen(FileInsert2, PathInsert, OpenMode.Output)
            Dim Prefijo As String = "cop_"
        Else


        End If

        Dim Contador As Integer
        lk = lk + 1

        'insertar el registro ninguno

        '  PrintLine(FileInsert2, TAB(0), "USE [" & Me.lstBaseDato.Text.Trim & "]")
        '  PrintLine(FileInsert2, TAB(0), "GO")
        PrintLine(FileInsert2, "")
        PrintLine(FileInsert2, TAB(0), "TRUNCATE TABLE [dbo].[" & Me.txtTabla.Text & "]")
        PrintLine(FileInsert2, TAB(0), "GO")
        PrintLine(FileInsert2, "")


        '   FileClose(FileInsert2)
    End Sub



    Private Sub generarSchema(ByVal PathCls As String, ByVal clase As String)
        If lk = 1 Then
            ' Doy formato al nombre del PathCls
            Dim PathInsert As String
            Dim PathFrm As String
            Dim PathFrmdetalle As String

            'seteo el path para los otros archivos
            PathInsert = Me.txtPath.Text & "\schema.txt"
            PathFrm = PathCls
            PathFrmdetalle = PathCls


            ' Defino variables
            Dim FileCls As Integer = FreeFile()
            Dim reg As regSistemaTabla
            Dim idTabla As String
            Dim idParametro As String

            FileOpen(FileInsert2, PathInsert, OpenMode.Output)
            Dim Prefijo As String = "cop_"
        Else


        End If

        Dim Contador As Integer
        lk = lk + 1

        'insertar el registro ninguno

        '  PrintLine(FileInsert2, TAB(0), "USE [" & Me.lstBaseDato.Text.Trim & "]")
        '  PrintLine(FileInsert2, TAB(0), "GO")
        PrintLine(FileInsert2, "")
        PrintLine(FileInsert2, TAB(0), "[" & Me.txtTabla.Text & ".txt]")
        PrintLine(FileInsert2, TAB(0), "ColNameHeader=True")
        PrintLine(FileInsert2, TAB(0), "CharacterSet=ANSI")
        PrintLine(FileInsert2, TAB(0), "Format=Delimited(|)")
        PrintLine(FileInsert2, TAB(0), "TextDelimiter=""")



        Dim Contador4 As Integer = 1


        For Each regInsert3 In arrEstructura

            Select Case regInsert3.tipo

                Case "int"
                    PrintLine(FileInsert2, TAB(0), "Col" & Contador4 & "=" & regInsert3.nombre & " Integer")
                Case "binary"

                Case "bit"
                    PrintLine(FileInsert2, TAB(0), "Col" & Contador4 & "=" & regInsert3.nombre & " Bit")
                Case "char"
                    PrintLine(FileInsert2, TAB(0), "Col" & Contador4 & "=" & regInsert3.nombre & " Char Width " & regInsert3.longitud)
                Case "datetime"
                    PrintLine(FileInsert2, TAB(0), "Col" & Contador4 & "=" & regInsert3.nombre & " Date")
                Case "decimal"
                    PrintLine(FileInsert2, TAB(0), "Col" & Contador4 & "=" & regInsert3.nombre & " single Width 9")
                Case "float"

                Case "image"

                Case "bigint"

                Case "money"
                    PrintLine(FileInsert2, TAB(0), "Col" & Contador4 & "=" & regInsert3.nombre & " single Width 9")
                Case "nchar"
                    PrintLine(FileInsert2, TAB(0), "Col" & Contador4 & "=" & regInsert3.nombre & " Char Width " & regInsert3.longitud)

                Case "ntext"

                Case "numeric"
                    PrintLine(FileInsert2, TAB(0), "Col" & Contador4 & "=" & regInsert3.nombre & " single Width 9")
                Case "nvarchar"
                    PrintLine(FileInsert2, TAB(0), "Col" & Contador4 & "=" & regInsert3.nombre & " Char Width " & regInsert3.longitud)
                Case "sysname"

                Case "real"

                Case "smalldatetime"

                Case "smallint"

                Case "smallmoney"

                Case "sql_variant"

                Case "text"

                Case "timestamp"

                Case "tinyint"

                Case "uniqueidentifier"

                Case "varbinary"

                Case "varchar"
                    PrintLine(FileInsert2, TAB(0), "Col" & Contador4 & "=" & regInsert3.nombre & " Char Width " & regInsert3.longitud)


            End Select





            Contador4 = Contador4 + 1
        Next

        PrintLine(FileInsert2, "")

        '   FileClose(FileInsert2)
    End Sub



    Private Sub Cargar(ByVal conexion As String, ByVal tabla As String)
        ' Defino variables de acceso y manipulación de datos
        Dim cn As New SqlConnection(conexion)
        Dim da As New SqlDataAdapter("SELECT * FROM [" & tabla & "]", cn) ' los [] ban por si es una tabla de nombre compuesto
        '  Dim datiposql As New SqlDataAdapter("SELECT TOP 100 PERCENT dbo.sysobjects.name AS tabla, dbo.syscolumns.scale, dbo.syscolumns.name AS campo, dbo.systypes.name AS tiposql, isnull(dbo.syscomments.text,'') AS defecto, dbo.syscolumns.isnullable AS isnull, dbo.syscolumns.length AS longitud, dbo.syscolumns.prec, ISNULL(dbo.sysproperties.[value],'') AS descripcion FROM dbo.syscolumns INNER JOIN dbo.sysobjects ON dbo.syscolumns.id = dbo.sysobjects.id INNER JOIN dbo.systypes ON dbo.syscolumns.xusertype = dbo.systypes.xtype LEFT OUTER JOIN dbo.sysproperties ON dbo.syscolumns.id = dbo.sysproperties.id AND dbo.syscolumns.colid = dbo.sysproperties.smallid LEFT OUTER JOIN dbo.syscomments ON dbo.syscolumns.cdefault = dbo.syscomments.id where dbo.sysobjects.name='" & tabla & "' ORDER BY dbo.syscolumns.colid", cn)
        Dim daSistema As New SqlDataAdapter("SELECT sys.tables.name AS tabla ,sys.columns.[scale] ,sys.columns.[name] AS campo ,sys.types.name AS tipo ,sys.columns.is_nullable  AS [isnull] ,[column_id] AS colorder ,sys.columns.[max_length] AS longitud ,sys.columns.[precisiON] AS prec " & _
                       "FROM sys.tables INNER JOIN sys.columns ON sys.columns.object_id=sys.tables.object_id INNER JOIN	sys.types ON sys.types.system_type_id=sys.columns.system_type_id  " & _
                       "WHERE sys.tables.name = '" & tabla & "' and sys.types.name not in('hierarchyid','geometry') ORDER BY sys.columns.column_id", cn)
        Dim cb As New SqlCommandBuilder(da)
        Dim cbSistema As New SqlCommandBuilder(daSistema)
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim dtSistema As New DataTable
        Dim odr As DataRow
        Dim dc As DataColumn

        ' Cargo la tabla con la estructura
        daSistema.Fill(dtSistema)
        ' Cargo la tabla
        da.Fill(dt)

        NumeroFilas = dtSistema.Rows.Count
        ' Defino variables
        Dim indice As Short

        ' Redimensiono el array con tantas posisiones como campos haya en la tabla
        ReDim arrEstructura(dt.Columns.Count() - 1)
        ' Inicializo variables
        indice = 0
        ' Recorro la lista de campos de estructura
        For Each odr In dtSistema.Rows
            arrEstructura(indice).nombre = dtSistema.Rows(indice).Item("campo").ToString
            arrEstructura(indice).longitud = dtSistema.Rows(indice).Item("longitud").ToString
            arrEstructura(indice).escala = dtSistema.Rows(indice).Item("scale").ToString
            '  arrEstructura(indice).indice = dtSistema.Rows(indice).Item("colstat").ToString
            arrEstructura(indice).nulo = dtSistema.Rows(indice).Item("isnull").ToString
            arrEstructura(indice).precicion = dtSistema.Rows(indice).Item("prec").ToString
            arrEstructura(indice).tipo = dtSistema.Rows(indice).Item("tipo").ToString
            arrEstructura(indice).Orden = dtSistema.Rows(indice).Item("colorder").ToString
            indice = indice + 1
        Next



        ' Recorro la lista de campos de la tabla
        indice = 0
        For Each dc In dt.Columns()
            ' Guardo el nombre y tiposql de cada uno de los campos
            ' La fórmula en el tiposql es para eliminar el "system." si llegase a aparecer
            arrEstructura(indice).tiposql = Mid$(dc.DataType.ToString, InStr(dc.DataType.ToString, ".", CompareMethod.Text) + 1)
            ' Incremento la posisión dentro del array
            Select Case arrEstructura(indice).tipo
                Case "int"
                    arrEstructura(indice).valorinicial = 0
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).valorinsert = 1
                    arrEstructura(indice).tipopocket = "int"
                Case "binary"
                    arrEstructura(indice).valorinicial = 0
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).tipopocket = "binary"
                Case "bit"
                    arrEstructura(indice).valorinicial = False
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).valorinsert = 0
                    arrEstructura(indice).tipopocket = "bit"
                Case "char"
                    arrEstructura(indice).valorinicial = """"""
                    arrEstructura(indice).sptamaño = "(" & arrEstructura(indice).longitud & ")"
                    arrEstructura(indice).valorinsert = "''"
                    arrEstructura(indice).tipopocket = "nchar(50)"
                Case "datetime"
                    arrEstructura(indice).valorinicial = "DateTime.Now"
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).valorinsert = "'01-01-2000'"
                    arrEstructura(indice).tipopocket = "datetime"
                Case "decimal"
                    arrEstructura(indice).valorinicial = 0
                    arrEstructura(indice).sptamaño = "(" & arrEstructura(indice).precicion & "," & arrEstructura(indice).escala & ")"
                    arrEstructura(indice).valorinsert = 0
                    arrEstructura(indice).tipopocket = "numeric(18,2)"
                Case "float"
                    arrEstructura(indice).valorinicial = 0
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).valorinsert = 0
                    arrEstructura(indice).tipopocket = "float"
                Case "image"
                    arrEstructura(indice).valorinicial = 0
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).tipopocket = "image"
                Case "bigint"
                    arrEstructura(indice).valorinicial = 0
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).tipopocket = "bigint"
                Case "money"
                    arrEstructura(indice).valorinicial = 0
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).valorinsert = 0
                    arrEstructura(indice).tipopocket = "money"
                Case "nchar"
                    arrEstructura(indice).valorinicial = """"""
                    arrEstructura(indice).sptamaño = "(" & arrEstructura(indice).longitud & ")"
                    arrEstructura(indice).valorinsert = "'Ninguno'"
                    arrEstructura(indice).tipopocket = "nchar(50)"
                Case "ntext"
                    arrEstructura(indice).valorinicial = """"""
                    arrEstructura(indice).sptamaño = "" '"(" & arrEstructura(indice).longitud & ")"
                    arrEstructura(indice).valorinsert = "'Ninguno'"
                    arrEstructura(indice).tipopocket = "ntext"
                Case "numeric"
                    arrEstructura(indice).valorinicial = 0
                    arrEstructura(indice).sptamaño = "(" & arrEstructura(indice).precicion & "," & arrEstructura(indice).escala & ")"
                    arrEstructura(indice).valorinsert = 0
                    arrEstructura(indice).tipopocket = "numeric(18,2)"
                Case "nvarchar"
                    arrEstructura(indice).valorinicial = """"""
                    arrEstructura(indice).sptamaño = "" '"(" & arrEstructura(indice).longitud & ")"
                    arrEstructura(indice).valorinsert = "'Ninguno'"
                    arrEstructura(indice).tipopocket = "nvarchar(50)"
                Case "sysname"
                    arrEstructura(indice).valorinicial = """"""
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).valorinsert = "'Ninguno'"
                Case "real"
                    arrEstructura(indice).valorinicial = 0
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).valorinsert = 0
                    arrEstructura(indice).tipopocket = "real"
                Case "smalldatetime"
                    arrEstructura(indice).valorinicial = DateTime.Now
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).valorinsert = "'01-01-2000'"
                Case "smallint"
                    arrEstructura(indice).valorinicial = 0
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).valorinsert = 0
                    arrEstructura(indice).tipopocket = "smallint"
                Case "smallmoney"
                    arrEstructura(indice).valorinicial = 0
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).valorinsert = 0
                Case "sql_variant"
                    arrEstructura(indice).valorinicial = 0
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).valorinsert = 0
                Case "text"
                    arrEstructura(indice).valorinicial = """"""
                    arrEstructura(indice).sptamaño = "" '"(" & arrEstructura(indice).longitud & ")"
                    arrEstructura(indice).valorinsert = "'Ninguno'"
                Case "timestamp"
                    arrEstructura(indice).valorinicial = 0
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).valorinsert = 0
                Case "tinyint"
                    arrEstructura(indice).valorinicial = 0
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).valorinsert = 0
                    arrEstructura(indice).tipopocket = "tinyint"
                Case "uniqueidentifier"
                    arrEstructura(indice).valorinicial = 0
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).valorinsert = 0
                    arrEstructura(indice).tipopocket = "uniqueidentifier"
                Case "varbinary"
                    arrEstructura(indice).valorinicial = 0
                    arrEstructura(indice).sptamaño = ""
                    arrEstructura(indice).valorinsert = 0
                    arrEstructura(indice).tipopocket = "varbinary"
                Case "varchar"
                    arrEstructura(indice).valorinicial = """"""
                    arrEstructura(indice).sptamaño = "(" & arrEstructura(indice).longitud & ")"
                    arrEstructura(indice).valorinsert = "'Ninguno'"
                    arrEstructura(indice).tipopocket = "nvarchar(50)"
            End Select

            indice = indice + 1
        Next

    End Sub

    ' Este procedimiento genera un Archivo de texto con extensión .VB con la definición de una clase correspondiente a una tabla de una base de datos.
    ' Al basarse enteramente en la estructura de una tabla de SQL Server, las propiedades de la clases son el mismo tiposql que en la tabla "física", por
    ' ejemplo en lugar de tener propiedades tiposql Integer va a tener propiedades de tiposql Int32, en lugar de Short van a ser Int16, etc.
    ' Esto no genera ningún problema, ya que VB.NET convierte en forma automática los datos (ya que las clase toma los datos "oficiales" de .NET).

    'Private Sub GenerarClase(ByVal PathCls As String, ByVal clase As String)
    '    ' Doy formato al nombre del PathCls
    '    Dim PathInsert As String
    '    Dim PathFrm As String
    '    Dim PathFrmdetalle As String
    '    Dim PathClsCe As String

    '    'seteo el path para los otros archivos
    '    PathInsert = PathCls
    '    PathFrm = PathCls
    '    PathFrmdetalle = PathCls
    '    PathClsCe = PathCls

    '    Dim a As String = Me.txtPath.Text & "\"
    '    If Mid$(PathCls, Len(PathCls) - 3, 3) <> ".vb" Then
    '        PathCls = Me.txtPath.Text & "\" & PathCls & ".vb"
    '    Else
    '        PathCls = Me.txtPath.Text & "\" & PathCls
    '    End If

    '    ' Defino variables
    '    Dim FileCls As Integer = FreeFile()
    '    Dim reg As regSistemaTabla
    '    Dim idTabla As String
    '    Dim idParametro As String

    '    ' Abro un PathCls de texto (si el mismo existía se reempleza)
    '    FileOpen(FileCls, PathCls, OpenMode.Output)


    '    '*********************************************
    '    '
    '    'DEFINICION DEL CODIGO PARA LAS CLASES
    '    '
    '    '*********************************************

    '    'importo las referencias
    '    PrintLine(FileCls, TAB(0), "Imports System.Data.SqlClient")
    '    PrintLine(FileCls, TAB(0), "Imports System.Data")
    '    PrintLine(FileCls, TAB(0), "Imports System.IO")
    '    PrintLine(FileCls, "")

    '    'defino la clase
    '    PrintLine(FileCls, "Public Class " & clase & "_ent")
    '    PrintLine(FileCls, "")

    '    'defino las variables
    '    PrintLine(FileCls, TAB(5), "'defino las variables")
    '    PrintLine(FileCls, TAB(5), "Private dt As DataTable")
    '    PrintLine(FileCls, TAB(5), "Private dr As DataRow")
    '    PrintLine(FileCls, TAB(5), "Private da As SqlClient.SqlDataAdapter")
    '    PrintLine(FileCls, TAB(5), "Friend cnn As New Conexion")
    '    PrintLine(FileCls, TAB(5), "Friend cnntxt As New Conexion_txt")
    '    PrintLine(FileCls, "")

    '    'defino las propiedades y su variable

    '    PrintLine(FileCls, TAB(5), "'defino las propiedades")
    '    For Each reg In arrEstructura
    '        If reg.Orden = 1 Then
    '            PrintLine(FileCls, TAB(5), "Private _" & reg.nombre & " As " & reg.tiposql)
    '            PrintLine(FileCls, TAB(5), "Public ReadOnly Property " & reg.nombre & "() As " & reg.tiposql)
    '            PrintLine(FileCls, TAB(9), "Get")
    '            PrintLine(FileCls, TAB(13), "_" & reg.nombre & " = dr(" & """" & reg.nombre & """" & ")")
    '            PrintLine(FileCls, TAB(13), "Return _" & reg.nombre)
    '            PrintLine(FileCls, TAB(9), "End Get")
    '            PrintLine(FileCls, TAB(5), "end property")
    '            PrintLine(FileCls, "")
    '        Else
    '            ' Creo la variable local para usarse con la propiedad
    '            PrintLine(FileCls, TAB(5), "Private _" & reg.nombre & " As " & reg.tiposql)

    '            ' Creo la cabecera de la propiedad
    '            PrintLine(FileCls, TAB(5), "Public Property " & reg.nombre & "() As " & reg.tiposql)

    '            ' Creo la cabecera del get de la propiedad
    '            PrintLine(FileCls, TAB(9), "Get")

    '            PrintLine(FileCls, TAB(13), "_" & reg.nombre & " = dr(" & """" & reg.nombre & """" & ")")
    '            ' Creo el cuerpo del get de la propiedad
    '            PrintLine(FileCls, TAB(13), "Return _" & reg.nombre)

    '            ' Creo el pie del get de la propiedad
    '            PrintLine(FileCls, TAB(9), "End Get")

    '            ' Creo una línea divisoria (espacio en blanco)
    '            '' PrintLine(FileCls, "")

    '            ' Creo la cabecera del set de la propiedad
    '            PrintLine(FileCls, TAB(9), "Set(ByVal Value As " & reg.tiposql & ")")

    '            ' Creo el cuerpo del set de la propiedad
    '            PrintLine(FileCls, TAB(13), "dr(" & """" & reg.nombre & """" & ") = Value")

    '            ' Creo el pie del set de la propiedad
    '            PrintLine(FileCls, TAB(9), "End Set")

    '            ' Creo el pie de la propiedad
    '            PrintLine(FileCls, TAB(5), "end property")

    '            ' Creo una línea divisoria (espacio en blanco)
    '            PrintLine(FileCls, "")
    '        End If
    '    Next

    '    'Public Sub Insertar()

    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(5), "'procedimiento insertar")
    '    PrintLine(FileCls, TAB(5), "Public Sub Insertar()")
    '    PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '    PrintLine(FileCls, TAB(9), "da.MissingSchemaAction = MissingSchemaAction.AddWithKey")
    '    PrintLine(FileCls, TAB(9), "da.Fill(dt)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Me.AsignarTipos()")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Dim cmdins As New SqlCommand(" & """" & "cop_" & txtTabla.Text.Trim & "_Insert" & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, TAB(9), "cmdins.CommandType = CommandType.StoredProcedure")
    '    PrintLine(FileCls, TAB(9), "da.InsertCommand = cmdins")
    '    PrintLine(FileCls, "")
    '    For Each reg In arrEstructura
    '        If reg.Orden = 1 Then
    '            idTabla = reg.nombre
    '            idParametro = "@" & reg.nombre
    '            PrintLine(FileCls, TAB(9), "Dim prm As SqlParameter")
    '            PrintLine(FileCls, TAB(9), "prm = da.InsertCommand.Parameters.Add(" & """" & "@" & reg.nombre & """" & ", SqlDbType." & reg.tipo & ", " & 0 & ", " & """" & reg.nombre & """" & ")")
    '            PrintLine(FileCls, TAB(9), "prm.Direction = ParameterDirection.Output")
    '        Else
    '            PrintLine(FileCls, TAB(9), "da.InsertCommand.Parameters.Add(" & """" & "@" & reg.nombre & """" & ", SqlDbType." & reg.tipo & ", " & reg.longitud & ", " & """" & reg.nombre & """" & ")")
    '        End If
    '    Next
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "dr = dt.NewRow()")
    '    For Each reg In arrEstructura
    '        If reg.Orden = 1 Then

    '        Else
    '            PrintLine(FileCls, TAB(9), "dr(" & """" & reg.nombre & """" & ") = " & reg.valorinicial)
    '        End If
    '    Next
    '    PrintLine(FileCls, TAB(9), "dt.Rows.Add(dr)")
    '    PrintLine(FileCls, TAB(9), "CrearComandoUpdate()")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")

    '    'Public Sub Modificar

    '    PrintLine(FileCls, TAB(5), "'procedimiento modificar")
    '    PrintLine(FileCls, TAB(5), "Public Sub Modificar(ByVal id" & txtTabla.Text.Trim & " As Integer)")
    '    PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '    PrintLine(FileCls, TAB(9), "da = New SqlClient.SqlDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & " WHERE " & idTabla & " = " & """" & " & _")
    '    PrintLine(FileCls, TAB(9), "id" & txtTabla.Text.Trim & ", cnn.Coneccion)")
    '    PrintLine(FileCls, TAB(9), "da.Fill(dt)")
    '    PrintLine(FileCls, TAB(9), "Me.AsignarTipos()")
    '    PrintLine(FileCls, TAB(9), "If dt.Rows.Count = 0 Then")
    '    PrintLine(FileCls, TAB(13), "Throw New Exception(" & """" & "No se a encontrado el Registro" & """" & ")")
    '    PrintLine(FileCls, TAB(9), "Else")
    '    PrintLine(FileCls, TAB(13), "dr = dt.Rows(0)")
    '    PrintLine(FileCls, TAB(9), "End If")
    '    PrintLine(FileCls, TAB(9), "CrearComandoUpdate()")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")

    '    'Public Sub Borrar


    '    PrintLine(FileCls, TAB(5), "'procedimiento borrar")
    '    PrintLine(FileCls, TAB(5), "Public Sub Borrar(ByVal id" & txtTabla.Text.Trim & " As Integer)")
    '    PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '    PrintLine(FileCls, TAB(9), "da = New SqlClient.SqlDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & " WHERE " & idTabla & " = " & """" & " & _")
    '    PrintLine(FileCls, TAB(9), "id" & txtTabla.Text.Trim & ", cnn.Coneccion)")
    '    PrintLine(FileCls, TAB(9), "da.Fill(dt)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Me.AsignarTipos()")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "If dt.Rows.Count = 0 Then")

    '    If Me.chkPocket.Checked = False Then
    '        PrintLine(FileCls, TAB(13), "MessageBox.Show(" & """" & "No se a encontrado el Registro" & """" & ")")
    '    End If

    '    PrintLine(FileCls, TAB(13), "Exit Sub")
    '    PrintLine(FileCls, TAB(9), "Else")
    '    PrintLine(FileCls, TAB(13), "dr = dt.Rows(0)")
    '    PrintLine(FileCls, TAB(9), "End If")
    '    PrintLine(FileCls, "")

    '    If Me.chkPocket.Checked = False Then
    '        '  PrintLine(FileCls, TAB(9), "If MessageBox.Show(" & """" & "Desea Realmente Eliminar el Registro " & """" & ", " & """" & "BORRAR" & """" & ", _")
    '        '  PrintLine(FileCls, TAB(13), "MessageBoxButtons.YesNo, MessageBoxIcon.Question) _")
    '        '  PrintLine(FileCls, TAB(13), "= DialogResult.No Then")
    '        '  PrintLine(FileCls, TAB(15), "Exit Sub")
    '        '  PrintLine(FileCls, TAB(9), "End If")
    '        '  PrintLine(FileCls, "")
    '    End If

    '    PrintLine(FileCls, TAB(9), "Try")
    '    PrintLine(FileCls, TAB(13), "Dim cmddel As New SqlCommand(" & """" & "cop_" & txtTabla.Text.Trim & "_Delete" & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, TAB(13), "cmddel.CommandType = CommandType.StoredProcedure")
    '    PrintLine(FileCls, TAB(13), "da.DeleteCommand = cmddel")
    '    PrintLine(FileCls, TAB(13), "Dim prm As SqlParameter")
    '    PrintLine(FileCls, TAB(13), "prm = da.DeleteCommand.Parameters.Add(" & """" & idParametro & """" & ", SqlDbType.Int, 4, " & """" & idTabla & """" & ")")
    '    PrintLine(FileCls, TAB(13), "dt.Rows(0).Delete()")
    '    PrintLine(FileCls, TAB(13), "CrearComandoUpdate()")
    '    PrintLine(FileCls, TAB(13), "Guardar()")
    '    PrintLine(FileCls, TAB(9), "Catch ex As Exception")
    '    PrintLine(FileCls, TAB(13), "If Err.Number = 5 Then")

    '    If Me.chkPocket.Checked = False Then
    '        PrintLine(FileCls, TAB(15), "MessageBox.Show(ex.Message, " & """" & "ERROR" & """" & ", MessageBoxButtons.OK, MessageBoxIcon.Error)")
    '    End If

    '    PrintLine(FileCls, TAB(13), "End If")
    '    PrintLine(FileCls, TAB(9), "End Try")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")

    '    'Private Sub AsignarTipos()

    '    PrintLine(FileCls, TAB(5), "'asigno el tipo de datos a los parametros")
    '    PrintLine(FileCls, TAB(5), "Private Sub AsignarTipos()")
    '    PrintLine(FileCls, TAB(9), "For Each dc As DataColumn In dt.Columns")
    '    PrintLine(FileCls, TAB(13), "Select Case dc.ColumnName")
    '    For Each reg In arrEstructura
    '        PrintLine(FileCls, TAB(16), "Case " & """" & reg.nombre & """")
    '        PrintLine(FileCls, TAB(19), "dc.DataType = Type.GetType(" & """" & "System." & reg.tiposql & """" & ")")
    '    Next
    '    PrintLine(FileCls, TAB(13), "End Select")
    '    PrintLine(FileCls, TAB(9), "Next")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")

    '    'Private Sub CrearComandoUpdate()

    '    PrintLine(FileCls, TAB(5), "'creo el commandbuilder")
    '    PrintLine(FileCls, TAB(5), "Private Sub CrearComandoUpdate()")
    '    PrintLine(FileCls, TAB(9), "Dim cmd As New SqlCommandBuilder(da)")
    '    PrintLine(FileCls, TAB(9), "da.UpdateCommand = cmd.GetUpdateCommand")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")

    '    'Public Sub Cancelar()

    '    PrintLine(FileCls, TAB(5), "'cancelo los cambios del datatable")
    '    PrintLine(FileCls, TAB(5), "Public Sub Cancelar()")
    '    PrintLine(FileCls, TAB(9), "dt.Clear()")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")

    '    'Public Function DataTable()

    '    PrintLine(FileCls, TAB(5), "'asigno el datatable")
    '    PrintLine(FileCls, TAB(5), "Public Function DataTable() As DataTable")
    '    PrintLine(FileCls, TAB(9), "Return dt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    'Public Sub Guardar()

    '    PrintLine(FileCls, TAB(5), "'actualizo la base de datos")
    '    PrintLine(FileCls, TAB(5), "Public Sub Guardar()")
    '    PrintLine(FileCls, TAB(9), "da.Update(dt)")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")

    '    'Public Function Cargar dt()

    '    PrintLine(FileCls, TAB(5), "'funcion que carga el datatable")
    '    PrintLine(FileCls, TAB(5), "Public Function Cargar() As DataTable")
    '    PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '    PrintLine(FileCls, TAB(9), "da = New SqlClient.SqlDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & " WHERE id_" & txtTabla.Text.Trim & " = 0" & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, TAB(9), "da.MissingSchemaAction = MissingSchemaAction.AddWithKey")
    '    PrintLine(FileCls, TAB(9), "da.Fill(dt)")
    '    PrintLine(FileCls, TAB(9), "Return dt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    'Public Function Buscar

    '    PrintLine(FileCls, TAB(5), "'funcion de busqueda")
    '    PrintLine(FileCls, TAB(5), "Public Function Buscar(ByVal Nombre As String) As DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & "_Find" & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & "@nombre" & """" & ", SqlDbType.NChar, 30, " & """" & "nombre" & """" & ")")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & "@nombre" & """" & ").Value = Nombre")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Return odt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    'Public Function ConsultarTodo()

    '    PrintLine(FileCls, TAB(5), "'funcion de consulta")
    '    PrintLine(FileCls, TAB(5), "Public Function ConsultarTodo() As DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & "_GetAll" & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Return odt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    'Public Function GetCmb()

    '    PrintLine(FileCls, TAB(5), "'funcion para llenar el combo")
    '    PrintLine(FileCls, TAB(5), "Public Function GetCmb() As DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & "_GetCmb" & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Return odt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    'public function GetOne()

    '    PrintLine(FileCls, TAB(5), "'funcion que trae un registro poniendo el id")
    '    PrintLine(FileCls, TAB(5), "Public Function GetOne(ByVal id_" & txtTabla.Text.Trim & " As Integer) As DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & "_GetOne" & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & idParametro & """" & ", SqlDbType.Int, 4, " & """" & idTabla & """" & ")")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & idParametro & """" & ").Value = id_" & txtTabla.Text.Trim)
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Return odt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    'Public Function Consultar con oda

    '    'PrintLine(FileCls, TAB(5), "'ejemplo de consulta con parametros")
    '    'PrintLine(FileCls, TAB(5), "Public Function Consultar(ByVal id_" & txtTabla.Text.Trim & " As Integer) As DataTable")
    '    'PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    'PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "Ejemplo_" & txtTabla.Text.Trim & """" & ", cnn.Coneccion)")
    '    'PrintLine(FileCls, "")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & idParametro & """" & ", SqlDbType.Int, 4, " & """" & idTabla & """" & ")")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & idParametro & """" & ").Value = id_" & txtTabla.Text.Trim)
    '    'PrintLine(FileCls, "")
    '    'PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    'PrintLine(FileCls, TAB(9), "Return odt")
    '    'PrintLine(FileCls, TAB(5), "End Function")
    '    'PrintLine(FileCls, "")

    '    'Public Function ConsultarDecimal

    '    'PrintLine(FileCls, TAB(5), "'ejemplo de consulta con parametros trayendo un decimal")
    '    'PrintLine(FileCls, TAB(5), "Public Function ConsultarDecimal(ByVal id_" & txtTabla.Text.Trim & " As Integer) As Decimal")
    '    'PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    'PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "Ejemplo_" & txtTabla.Text.Trim & "Decimal" & """" & ", cnn.Coneccion)")
    '    'PrintLine(FileCls, TAB(9), "Dim Total As Decimal")
    '    'PrintLine(FileCls, "")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & idParametro & """" & ", SqlDbType.Int, 4, " & """" & idTabla & """" & ")")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & idParametro & """" & ").Value = id_" & txtTabla.Text.Trim)
    '    'PrintLine(FileCls, "")
    '    'PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    'PrintLine(FileCls, TAB(9), "Total = odt.Rows(0).Item(" & """" & "Total" & """" & ").ToString")
    '    'PrintLine(FileCls, TAB(9), "Return Total")
    '    'PrintLine(FileCls, TAB(5), "End Function")
    '    'PrintLine(FileCls, "")

    '    'Public Function ControlarSiExiste

    '    'PrintLine(FileCls, TAB(5), "'ejemplo de consulta con parametros trayendo un boolean")
    '    'PrintLine(FileCls, TAB(5), "Public Function ControlarSiExiste(ByVal id_" & txtTabla.Text.Trim & " As Integer) As Boolean")
    '    'PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    'PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & "_Check" & """" & ", cnn.Coneccion)")
    '    'PrintLine(FileCls, TAB(9), "Dim Total As Integer")
    '    'PrintLine(FileCls, "")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & idParametro & """" & ", SqlDbType.Int, 4, " & """" & idTabla & """" & ")")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & idParametro & """" & ").Value = id_" & txtTabla.Text.Trim)
    '    'PrintLine(FileCls, "")
    '    'PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    'PrintLine(FileCls, TAB(9), "Total = odt.Rows(0).Item(" & """" & "Total" & """" & ").ToString")
    '    'PrintLine(FileCls, TAB(9), "If Total = 0 Then")
    '    'PrintLine(FileCls, TAB(13), "Return True  'NO EXISTE")
    '    'PrintLine(FileCls, TAB(9), "Else")
    '    'PrintLine(FileCls, TAB(13), "Return False 'SI EXISTE")
    '    'PrintLine(FileCls, TAB(9), "End If")
    '    'PrintLine(FileCls, TAB(5), "End Function")
    '    'PrintLine(FileCls, "")





    '    'Public Function Exist

    '    PrintLine(FileCls, TAB(5), "'controla si existe el registro en la base de datos")
    '    PrintLine(FileCls, TAB(5), "Public Function Exist() As Boolean")
    '    PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & "_Exist" & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, TAB(9), "Dim Total As Integer")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")

    '    For Each reg In arrEstructura
    '        If reg.Orden = 1 Then
    '        Else
    '            PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & "@" & reg.nombre & """" & ", SqlDbType." & reg.tipo & ", " & reg.longitud & ", " & """" & reg.nombre & """" & ")")
    '            PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & "@" & reg.nombre & """" & ").Value = Me." & reg.nombre)
    '        End If
    '    Next
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    PrintLine(FileCls, TAB(9), "Total = odt.Rows(0).Item(" & """" & "Total" & """" & ").ToString")
    '    PrintLine(FileCls, TAB(9), "If Total = 0 Then")
    '    PrintLine(FileCls, TAB(13), "Return False  'NO EXISTE")
    '    PrintLine(FileCls, TAB(9), "Else")
    '    PrintLine(FileCls, TAB(13), "Return True 'SI EXISTE")
    '    PrintLine(FileCls, TAB(9), "End If")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")



    '    'borra toda la tabla
    '    PrintLine(FileCls, TAB(5), "'borra todos los datos de la tabla")
    '    PrintLine(FileCls, TAB(5), "Public Sub BorrarTodo()")
    '    PrintLine(FileCls, TAB(9), "Dim oConexion As New Conexion")
    '    PrintLine(FileCls, TAB(9), "Dim Command As SqlCommand = New SqlCommand()")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Command.Connection = oConexion.Coneccion")
    '    PrintLine(FileCls, TAB(9), "Command.CommandText = ""DELETE FROM " & Me.txtTabla.Text & """")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oConexion.Abrir()")
    '    PrintLine(FileCls, TAB(9), "Command.ExecuteNonQuery()")
    '    PrintLine(FileCls, TAB(9), "oConexion.Cerrar()")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")

    '    'truncate toda la tabla
    '    PrintLine(FileCls, TAB(5), "'borra todos los datos de la tabla")
    '    PrintLine(FileCls, TAB(5), "Public Sub Truncate()")
    '    PrintLine(FileCls, TAB(9), "Dim oConexion As New Conexion")
    '    PrintLine(FileCls, TAB(9), "Dim Command As SqlCommand = New SqlCommand()")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Command.Connection = oConexion.Coneccion")
    '    PrintLine(FileCls, TAB(9), "Command.CommandText = ""TRUNCATE TABLE " & Me.txtTabla.Text & """")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oConexion.Abrir()")
    '    PrintLine(FileCls, TAB(9), "Command.ExecuteNonQuery()")
    '    PrintLine(FileCls, TAB(9), "oConexion.Cerrar()")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")

    '    'borra toda la tabla
    '    PrintLine(FileCls, TAB(5), "'inserta un registro en la tabla")
    '    PrintLine(FileCls, TAB(5), "Public Sub InsertOne()")
    '    PrintLine(FileCls, TAB(9), "Dim oConexion As New Conexion")
    '    PrintLine(FileCls, TAB(9), "Dim Command As SqlCommand = New SqlCommand()")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Command.Connection = oConexion.Coneccion")
    '    PrintLine(FileCls, TAB(9), "Command.CommandText = " & """" & "cop_" & txtTabla.Text.Trim & "_InsertOne" & """")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Command.CommandType = CommandType.StoredProcedure")
    '    PrintLine(FileCls, TAB(9), "oConexion.Abrir()")
    '    PrintLine(FileCls, TAB(9), "Command.ExecuteNonQuery()")
    '    PrintLine(FileCls, TAB(9), "oConexion.Cerrar()")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")


    '    'consulta el archivo txt para importarlo
    '    PrintLine(FileCls, TAB(5), "'importa los datos de una campaña desde el txt")
    '    PrintLine(FileCls, TAB(5), "Public Function Cargar_Archivo(ByVal path As String) As DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim odt As New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Dim oda As New System.Data.OleDb.OleDbDataAdapter( _")
    '    PrintLine(FileCls, TAB(9), """SELECT * FROM " & Me.txtTabla.Text & ".txt"", cnntxt.Coneccion(path))")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Return odt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    PrintLine(FileCls, "")
    '    '      PrintLine(FileCls, TAB(13), "'********************************************")
    '    '      PrintLine(FileCls, TAB(13), "'    CODIGO AGREGADO A LA CLASE ORIGINAL")
    '    '      PrintLine(FileCls, TAB(13), "'********************************************")
    '    '      PrintLine(FileCls, "")
    '    '      PrintLine(FileCls, "")
    '    '      PrintLine(FileCls, "")
    '    '      PrintLine(FileCls, "")
    '    ' Comienzo a generar la clase
    '    PrintLine(FileCls, "End Class")

    '    ' Cierro el PathCls de versión
    '    FileClose(FileCls)



    '    '*****************************************************
    '    ' DEFINICION DEL CODIGO PARA GENERAR LA CLASE HEREDADA
    '    '*****************************************************

    '    If Mid$(PathCls, Len(PathCls) - 3, 3) <> ".vb" Then
    '        PathCls = Me.txtPath.Text & "\cls" & Me.txtTabla.Text & ".vb"
    '    Else
    '        PathCls = Me.txtPath.Text & "\cls" & Me.txtTabla.Text & ".vb"
    '    End If

    '    'Abro un PathCls de texto (si el mismo existía se reempleza)
    '    FileOpen(FileCls, PathCls, OpenMode.Output)

    '    'importo las referencias
    '    PrintLine(FileCls, TAB(0), "Imports System.Data.SqlClient")
    '    PrintLine(FileCls, TAB(0), "Imports System.Data")
    '    PrintLine(FileCls, TAB(0), "Imports System.IO")
    '    PrintLine(FileCls, "")

    '    'defino la clase
    '    PrintLine(FileCls, "Public Class " & clase)
    '    PrintLine(FileCls, TAB(5), "Inherits " & clase & "_ent")
    '    PrintLine(FileCls, "")

    '    'Public Function Consultar con oda

    '    PrintLine(FileCls, TAB(5), "'ejemplo de consulta con parametros")
    '    PrintLine(FileCls, TAB(5), "Public Function Ejemplo(ByVal id_" & txtTabla.Text.Trim & " As Integer) As DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & idParametro & """" & ", SqlDbType.Int, 4, " & """" & idTabla & """" & ")")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & idParametro & """" & ").Value = id_" & txtTabla.Text.Trim)
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    PrintLine(FileCls, TAB(9), "Return odt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    PrintLine(FileCls, "End Class")

    '    ' Cierro el PathCls de versión
    '    FileClose(FileCls)



    '    '*******************************************************
    '    ' DEFINICION DEL CODIGO PARA GENERAR LA CLASE TESTEADORA
    '    '*******************************************************
    '    If Me.chkTest.Checked Then
    '        If Mid$(PathClsCe, Len(PathClsCe) - 3, 3) <> ".vb" Then
    '            PathClsCe = Me.txtPath.Text & "\" & Me.txtTabla.Text & "_entTest.vb"
    '        Else
    '            PathClsCe = Me.txtPath.Text & "\" & Me.txtTabla.Text
    '        End If


    '        PrintLine(FileCls, TAB(0), "Imports System.Data")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(0), "Imports Microsoft.VisualStudio.TestTools.UnitTesting")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(0), "Imports " & Me.lstBaseDato.Text.Trim)
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(0), "'''<summary>")
    '        PrintLine(FileCls, TAB(0), "'''Se trata de una clase de prueba para " & Me.txtTabla.Text & "_entTest y se pretende que")
    '        PrintLine(FileCls, TAB(0), "'''contenga todas las pruebas unitarias " & Me.txtTabla.Text & "_entTest. ")
    '        PrintLine(FileCls, TAB(0), "'''</summary>")
    '        PrintLine(FileCls, TAB(0), "<TestClass()> _")
    '        PrintLine(FileCls, TAB(0), "Public Class " & Me.txtTabla.Text & "_entTest")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "Private testContextInstance As TestContext")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), "'''Obtiene o establece el contexto de la prueba que proporciona")
    '        PrintLine(FileCls, TAB(4), "'''la información y funcionalidad para la ejecución de pruebas actual. ")
    '        PrintLine(FileCls, TAB(4), "'''</summary>")
    '        PrintLine(FileCls, TAB(4), "Public Property TestContext() As TestContext")
    '        PrintLine(FileCls, TAB(8), "Get")
    '        PrintLine(FileCls, TAB(12), "Return testContextInstance")
    '        PrintLine(FileCls, TAB(8), "End Get")
    '        PrintLine(FileCls, TAB(8), "Set(ByVal value As TestContext) ")
    '        PrintLine(FileCls, TAB(12), "testContextInstance = value")
    '        PrintLine(FileCls, TAB(8), "End Set")
    '        PrintLine(FileCls, TAB(4), "End Property")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(0), "#Region " & """" & "Atributos de prueba adicionales" & """")
    '        PrintLine(FileCls, TAB(4), "'")
    '        PrintLine(FileCls, TAB(4), "'Puede utilizar los siguientes atributos adicionales mientras escribe sus pruebas: ")
    '        PrintLine(FileCls, TAB(4), "'")
    '        PrintLine(FileCls, TAB(4), "'Use ClassInitialize para ejecutar código antes de ejecutar la primera prueba en la clase ")
    '        PrintLine(FileCls, TAB(4), "'<ClassInitialize()>  _")
    '        PrintLine(FileCls, TAB(4), "'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext) ")
    '        PrintLine(FileCls, TAB(4), "'End Sub")
    '        PrintLine(FileCls, TAB(4), "'")
    '        PrintLine(FileCls, TAB(4), "'Use ClassCleanup para ejecutar código después de haber ejecutado todas las pruebas en una clase")
    '        PrintLine(FileCls, TAB(4), "'<ClassCleanup()>  _")
    '        PrintLine(FileCls, TAB(4), "'Public Shared Sub MyClassCleanup()")
    '        PrintLine(FileCls, TAB(4), "'End Sub")
    '        PrintLine(FileCls, TAB(4), "'")
    '        PrintLine(FileCls, TAB(4), "'Use TestInitialize para ejecutar código antes de ejecutar cada prueba")
    '        PrintLine(FileCls, TAB(4), "'<TestInitialize()>  _")
    '        PrintLine(FileCls, TAB(4), "'Public Sub MyTestInitialize()")
    '        PrintLine(FileCls, TAB(4), "'End Sub")
    '        PrintLine(FileCls, TAB(4), "'")
    '        PrintLine(FileCls, TAB(4), "'Use TestCleanup para ejecutar código después de que se hayan ejecutado todas las pruebas")
    '        PrintLine(FileCls, TAB(4), "'<TestCleanup()>  _")
    '        PrintLine(FileCls, TAB(4), "'Public Sub MyTestCleanup()")
    '        PrintLine(FileCls, TAB(4), "'End Sub")
    '        PrintLine(FileCls, TAB(4), "'")
    '        PrintLine(FileCls, TAB(0), "#End Region")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), " '''Una prueba de Modificar")
    '        PrintLine(FileCls, TAB(4), " '''</summary>")
    '        PrintLine(FileCls, TAB(4), "<TestMethod()> _")
    '        PrintLine(FileCls, TAB(4), "Public Sub ModificarTest()")
    '        PrintLine(FileCls, TAB(8), "Dim target As " & Me.txtTabla.Text & "_ent = New " & Me.txtTabla.Text & "_ent ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim actual As DataTable")

    '        PrintLine(FileCls, TAB(8), "target.Truncate()")
    '        PrintLine(FileCls, TAB(8), "target.InsertOne()")
    '        PrintLine(FileCls, TAB(8), "target.Cargar()")
    '        PrintLine(FileCls, TAB(8), "target.ModificarOld(1) ")
    '        PrintLine(FileCls, TAB(8), "target.nombre_" & Me.txtTabla.Text & " = " & """" & "modificado" & """")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "actual = target.Modificar")
    '        PrintLine(FileCls, TAB(8), "Dim dt As DataTable = New DataTable")
    '        PrintLine(FileCls, TAB(8), "dt = target.ConsultarTodo")

    '        PrintLine(FileCls, TAB(8), "Assert.IsTrue(dt.Rows(0).Item(" & """" & "nombre_" & Me.txtTabla.Text & ") = " & """" & "modificado" & """" & "," & """" & "no:" & """" & "dt.Rows(0).Item(" & """" & "nombre_" & Me.txtTabla.Text & """" & ").ToString)")
    '        PrintLine(FileCls, TAB(4), "End Sub")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), "'''Una prueba de Insertar")
    '        PrintLine(FileCls, TAB(4), "'''</summary>")
    '        PrintLine(FileCls, TAB(4), "<TestMethod()> _")
    '        PrintLine(FileCls, TAB(4), "Public Sub InsertarTest()")
    '        PrintLine(FileCls, TAB(8), "Dim target As " & Me.txtTabla.Text & "_ent = New " & Me.txtTabla.Text & "_ent ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.Truncate()")
    '        PrintLine(FileCls, TAB(8), "target.InsertOne()")
    '        PrintLine(FileCls, TAB(8), "target.Cargar()")
    '        PrintLine(FileCls, TAB(8), "target.Insertar()")
    '        PrintLine(FileCls, TAB(8), "target.id_" & Me.txtTabla.Text & " = 2")
    '        PrintLine(FileCls, TAB(8), "target.nombre_" & Me.txtTabla.Text & " = " & """" & "prueba" & """")
    '        PrintLine(FileCls, TAB(8), "target.Guardar()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Dim dt As DataTable = New DataTable")
    '        PrintLine(FileCls, TAB(8), "dt = target.ConsultarTodo")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Assert.IsTrue(dt.Rows.Count = 2, " & """" & "son: " & """" & "dt.Rows.Count" & """" & " registros" & """" & ")")
    '        PrintLine(FileCls, TAB(4), "End Sub")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), "'''Una prueba de GetOne")
    '        PrintLine(FileCls, TAB(4), "'''</summary>")
    '        PrintLine(FileCls, TAB(4), "<TestMethod()> _")
    '        PrintLine(FileCls, TAB(4), "Public Sub GetOneTest()")
    '        PrintLine(FileCls, TAB(8), "Dim target As " & Me.txtTabla.Text & "_ent = New " & Me.txtTabla.Text & "_ent ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim id_" & Me.txtTabla.Text & " As Integer = 1 ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim actual As DataTable")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.Truncate()")
    '        PrintLine(FileCls, TAB(8), "target.InsertOne()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "actual = target.GetOne(id_ & Me.txtTabla.Text & ) ")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Assert.IsTrue(dt.Rows.Count = 1, " & """" & "son: " & """" & "dt.Rows.Count" & """" & " registros" & """" & ")")
    '        PrintLine(FileCls, TAB(4), "End Sub")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), "'''Una prueba de GetCmb")
    '        PrintLine(FileCls, TAB(4), "'''</summary>")
    '        PrintLine(FileCls, TAB(4), "<TestMethod()> _")
    '        PrintLine(FileCls, TAB(4), "Public Sub GetCmbTest()")
    '        PrintLine(FileCls, TAB(8), "Dim target As  & Me.txtTabla.Text & _ent = New  & Me.txtTabla.Text & _ent ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim actual As DataTable")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.Truncate(")
    '        PrintLine(FileCls, TAB(8), "target.InsertOne(")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "actual = target.GetCmb")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Assert.IsTrue(dt.Rows.Count = 1, " & """" & "son: " & """" & "dt.Rows.Count" & """" & " registros" & """" & ")")
    '        PrintLine(FileCls, TAB(4), "End Sub")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), "'''Una prueba de Exist")
    '        PrintLine(FileCls, TAB(4), "'''</summary>")
    '        PrintLine(FileCls, TAB(4), "<TestMethod()> _")
    '        PrintLine(FileCls, TAB(4), "Public Sub ExistTest()")
    '        PrintLine(FileCls, TAB(8), "Dim target As " & Me.txtTabla.Text & "_ent = New " & Me.txtTabla.Text & "_ent ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim expected As Boolean = False ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim actual As Boolean")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.Truncate()")
    '        PrintLine(FileCls, TAB(8), "target.InsertOne()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.ModificarOld(1) ")
    '        PrintLine(FileCls, TAB(8), "target.nombre_" & Me.txtTabla.Text & " = " & """" & "prueba" & """")
    '        PrintLine(FileCls, TAB(8), "target.Guardar()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "actual = target.Exist")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Assert.IsTrue(actual = True, " & """" & "no:" & """" & " actual) ")
    '        PrintLine(FileCls, TAB(4), "End Sub")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), "'''Una prueba de ConsultarTodo")
    '        PrintLine(FileCls, TAB(4), "'''</summary>")
    '        PrintLine(FileCls, TAB(4), "<TestMethod()> _")
    '        PrintLine(FileCls, TAB(4), "Public Sub ConsultarTodoTest()")
    '        PrintLine(FileCls, TAB(8), "Dim target As " & Me.txtTabla.Text & "_ent = New " & Me.txtTabla.Text & "_ent ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim actual As DataTable")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.Truncate()")
    '        PrintLine(FileCls, TAB(8), "target.InsertOne()")
    '        PrintLine(FileCls, TAB(8), "target.Cargar()")
    '        PrintLine(FileCls, TAB(8), "target.Insertar()")
    '        PrintLine(FileCls, TAB(8), "target.id_" & Me.txtTabla.Text & " = 2")
    '        PrintLine(FileCls, TAB(8), "target.nombre_" & Me.txtTabla.Text & " = " & """" & "prueba" & """")
    '        PrintLine(FileCls, TAB(8), "target.Guardar()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "actual = target.ConsultarTodo")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Assert.IsTrue(dt.Rows.Count = 2, " & """" & "son: " & """" & "dt.Rows.Count" & """" & " registros" & """" & ")")
    '        PrintLine(FileCls, TAB(4), "End Sub")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), "'''Una prueba de Buscar")
    '        PrintLine(FileCls, TAB(4), "'''</summary>")
    '        PrintLine(FileCls, TAB(4), "<TestMethod()> _")
    '        PrintLine(FileCls, TAB(4), "Public Sub BuscarTest()")
    '        PrintLine(FileCls, TAB(8), "Dim target As " & Me.txtTabla.Text & "_ent = New " & Me.txtTabla.Text & "_ent ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim Nombre As String = " & """" & "pru" & """" & " ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim actual As DataTable")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.Truncate()")
    '        PrintLine(FileCls, TAB(8), "target.InsertOne()")
    '        PrintLine(FileCls, TAB(8), "target.Cargar()")
    '        PrintLine(FileCls, TAB(8), "target.Insertar()")
    '        PrintLine(FileCls, TAB(8), "target.id_" & Me.txtTabla.Text & " = 3")
    '        PrintLine(FileCls, TAB(8), "target.nombre_" & Me.txtTabla.Text & " = " & """" & "prueba" & """")
    '        PrintLine(FileCls, TAB(8), "target.Guardar()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.Insertar()")
    '        PrintLine(FileCls, TAB(8), "target.id_" & Me.txtTabla.Text & " = 4")
    '        PrintLine(FileCls, TAB(8), "target.nombre_" & Me.txtTabla.Text & " = " & """" & "prudente" & """")
    '        PrintLine(FileCls, TAB(8), "target.Guardar()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "actual = target.Buscar(Nombre) ")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Assert.IsTrue(dt.Rows.Count = 2, " & """" & "son: " & """" & "dt.Rows.Count" & """" & " registros" & """" & ")")
    '        PrintLine(FileCls, TAB(4), "End Sub")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), "'''Una prueba de Borrar")
    '        PrintLine(FileCls, TAB(4), "'''</summary>")
    '        PrintLine(FileCls, TAB(4), "<TestMethod()> _")
    '        PrintLine(FileCls, TAB(4), "Public Sub BorrarTest()")
    '        PrintLine(FileCls, TAB(8), "Dim target As " & Me.txtTabla.Text & "_ent = New " & Me.txtTabla.Text & "_ent ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim id_" & Me.txtTabla.Text & " As Integer = 1 ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim actual As DataTable")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.Truncate()")
    '        PrintLine(FileCls, TAB(8), "target.InsertOne()")
    '        PrintLine(FileCls, TAB(8), "actual = target.Borrar(id_" & Me.txtTabla.Text & ") ")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Dim dt As DataTable = New DataTable")
    '        PrintLine(FileCls, TAB(8), "dt = target.GetOne(id_" & Me.txtTabla.Text & ") ")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Assert.IsTrue(dt.Rows.Count = 0, " & """" & "son: " & """" & "dt.Rows.Count" & """" & " registros" & """" & ")")
    '        PrintLine(FileCls, TAB(4), "End Sub")
    '        PrintLine(FileCls, TAB(0), "End Class")



    '        FileOpen(FileCls, PathClsCe, OpenMode.Output)
    '    End If


    '    '*****************************************************
    '    '*****************************************************
    '    'DEFINICION DEL CODIGO PARA LAS CLASES DE LA POCKET PC
    '    '*****************************************************
    '    '*****************************************************
    '    If Me.chkPocket.Checked Then

    '        If Mid$(PathClsCe, Len(PathClsCe) - 3, 3) <> ".vb" Then
    '            PathClsCe = Me.txtPath.Text & "\ce_" & PathClsCe & ".vb"
    '        Else
    '            PathClsCe = Me.txtPath.Text & "\ce_" & PathClsCe
    '        End If

    '        FileOpen(FileCls, PathClsCe, OpenMode.Output)

    '        'importo las referencias
    '        PrintLine(FileCls, TAB(0), "Imports System.Data")
    '        PrintLine(FileCls, TAB(0), "Imports System.Data.SqlServerCe")
    '        PrintLine(FileCls, "")

    '        'defino la clase
    '        PrintLine(FileCls, "Public Class " & clase)
    '        PrintLine(FileCls, "")

    '        'defino las variables
    '        PrintLine(FileCls, TAB(5), "'defino las variables")
    '        PrintLine(FileCls, TAB(5), "Private dt As DataTable")
    '        PrintLine(FileCls, TAB(5), "Private dr As DataRow")
    '        PrintLine(FileCls, TAB(5), "Private da As SqlCeDataAdapter")
    '        PrintLine(FileCls, TAB(5), "Private cnn As New Conexion")
    '        PrintLine(FileCls, TAB(5), "Dim ocnn As SqlCeConnection")
    '        PrintLine(FileCls, "")

    '        'defino las propiedades y su variable

    '        PrintLine(FileCls, TAB(5), "'defino las propiedades")
    '        For Each reg In arrEstructura
    '            If reg.Orden = 1 Then
    '                PrintLine(FileCls, TAB(5), "Private _" & reg.nombre & " As " & reg.tiposql)
    '                PrintLine(FileCls, TAB(5), "Public ReadOnly Property " & reg.nombre & "() As " & reg.tiposql)
    '                PrintLine(FileCls, TAB(9), "Get")
    '                PrintLine(FileCls, TAB(13), "_" & reg.nombre & " = dr(" & """" & reg.nombre & """" & ")")
    '                PrintLine(FileCls, TAB(13), "Return _" & reg.nombre)
    '                PrintLine(FileCls, TAB(9), "End Get")
    '                PrintLine(FileCls, TAB(5), "end property")
    '                PrintLine(FileCls, "")
    '            Else
    '                ' Creo la variable local para usarse con la propiedad
    '                PrintLine(FileCls, TAB(5), "Private _" & reg.nombre & " As " & reg.tiposql)

    '                ' Creo la cabecera de la propiedad
    '                PrintLine(FileCls, TAB(5), "Public Property " & reg.nombre & "() As " & reg.tiposql)

    '                ' Creo la cabecera del get de la propiedad
    '                PrintLine(FileCls, TAB(9), "Get")

    '                PrintLine(FileCls, TAB(13), "_" & reg.nombre & " = dr(" & """" & reg.nombre & """" & ")")
    '                ' Creo el cuerpo del get de la propiedad
    '                PrintLine(FileCls, TAB(13), "Return _" & reg.nombre)

    '                ' Creo el pie del get de la propiedad
    '                PrintLine(FileCls, TAB(9), "End Get")

    '                ' Creo una línea divisoria (espacio en blanco)
    '                '' PrintLine(FileCls, "")

    '                ' Creo la cabecera del set de la propiedad
    '                PrintLine(FileCls, TAB(9), "Set(ByVal Value As " & reg.tiposql & ")")

    '                ' Creo el cuerpo del set de la propiedad
    '                PrintLine(FileCls, TAB(13), "dr(" & """" & reg.nombre & """" & ") = Value")

    '                ' Creo el pie del set de la propiedad
    '                PrintLine(FileCls, TAB(9), "End Set")

    '                ' Creo el pie de la propiedad
    '                PrintLine(FileCls, TAB(5), "end property")

    '                ' Creo una línea divisoria (espacio en blanco)
    '                PrintLine(FileCls, "")
    '            End If
    '        Next

    '        'Public Sub Insertar()

    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(5), "'procedimiento insertar")
    '        Print(FileCls, TAB(5), "Public Sub Insertar(")
    '        Dim Contador3 As Integer = 1
    '        For Each reg In arrEstructura
    '            If reg.Orden = 1 Then
    '            ElseIf Contador3 = NumeroFilas Then
    '                Print(FileCls, "ByVal " & reg.nombre & " As " & reg.tiposql & ")")
    '            Else
    '                Print(FileCls, "ByVal " & reg.nombre & " As " & reg.tiposql & ", ")
    '            End If
    '            Contador3 = Contador3 + 1
    '        Next
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '        PrintLine(FileCls, TAB(9), "da.MissingSchemaAction = MissingSchemaAction.AddWithKey")
    '        PrintLine(FileCls, TAB(9), "da.Fill(dt)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Me.AsignarTipos()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "ocnn = cnn.Coneccion")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Dim ocmd As SqlCeCommand = ocnn.CreateCommand")
    '        PrintLine(FileCls, TAB(9), "ocmd.CommandText = " & """" & "INSERT INTO " & txtTabla.Text.Trim & "(" & """" & " & _")

    '        Print(FileCls, TAB(9), """")
    '        Dim Contador2 As Integer = 1
    '        For Each reg In arrEstructura
    '            If reg.Orden = 1 Then
    '            ElseIf Contador2 = NumeroFilas Then
    '                Print(FileCls, reg.nombre & ") VALUES (")
    '                For i As Integer = 1 To NumeroFilas - 1
    '                    If i = NumeroFilas - 1 Then
    '                        Print(FileCls, "?)" & """")
    '                    Else
    '                        Print(FileCls, "?,")
    '                    End If

    '                Next
    '            Else
    '                Print(FileCls, reg.nombre & ", ")
    '            End If
    '            Contador2 = Contador2 + 1
    '        Next

    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        For Each reg In arrEstructura
    '            If reg.Orden = 1 Then
    '                idTabla = reg.nombre
    '                idParametro = "" & reg.nombre
    '            Else
    '                PrintLine(FileCls, TAB(9), "ocmd.Parameters.Add(New SqlCeParameter(" & """" & "" & reg.nombre & """" & ", SqlDbType." & reg.tipo & ", " & reg.longitud & "))")
    '                PrintLine(FileCls, TAB(9), "ocmd.Parameters(" & """" & "" & reg.nombre & """" & ").Value = " & reg.nombre)
    '            End If
    '        Next
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "ocnn.Open()")
    '        PrintLine(FileCls, TAB(9), "ocmd.ExecuteNonQuery()")
    '        PrintLine(FileCls, TAB(9), "ocnn.Close()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "dr = dt.NewRow()")
    '        For Each reg In arrEstructura
    '            If reg.Orden = 1 Then

    '            Else
    '                PrintLine(FileCls, TAB(9), "dr(" & """" & reg.nombre & """" & ") = " & reg.nombre)
    '            End If
    '        Next
    '        PrintLine(FileCls, TAB(9), "dt.Rows.Add(dr)")
    '        PrintLine(FileCls, TAB(9), "CrearComandoUpdate()")
    '        PrintLine(FileCls, TAB(5), "End Sub")
    '        PrintLine(FileCls, "")

    '        'Public Sub Modificar

    '        PrintLine(FileCls, TAB(5), "'procedimiento modificar")
    '        PrintLine(FileCls, TAB(5), "Public Sub Modificar(ByVal id_" & txtTabla.Text.Trim & " As Integer)")
    '        PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '        PrintLine(FileCls, TAB(9), "da = New SqlCeDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & " WHERE " & idTabla & " = " & """" & " & _")
    '        PrintLine(FileCls, TAB(9), "id_" & txtTabla.Text.Trim & ", cnn.Coneccion)")
    '        PrintLine(FileCls, TAB(9), "da.Fill(dt)")
    '        PrintLine(FileCls, TAB(9), "Me.AsignarTipos()")
    '        PrintLine(FileCls, TAB(9), "If dt.Rows.Count = 0 Then")
    '        PrintLine(FileCls, TAB(13), "Throw New Exception(" & """" & "No se a encontrado el Registro" & """" & ")")
    '        PrintLine(FileCls, TAB(9), "Else")
    '        PrintLine(FileCls, TAB(13), "dr = dt.Rows(0)")
    '        PrintLine(FileCls, TAB(9), "End If")
    '        PrintLine(FileCls, TAB(9), "CrearComandoUpdate()")
    '        PrintLine(FileCls, TAB(5), "End Sub")
    '        PrintLine(FileCls, "")

    '        'Public Sub Borrar

    '        PrintLine(FileCls, TAB(5), "'procedimiento borrar")
    '        PrintLine(FileCls, TAB(5), "Public Sub Borrar(ByVal id_" & txtTabla.Text.Trim & " As Integer)")
    '        PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '        PrintLine(FileCls, TAB(9), "da = New SqlCeDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & " WHERE " & idTabla & " = " & """" & " & _")
    '        PrintLine(FileCls, TAB(9), "id_" & txtTabla.Text.Trim & ", cnn.Coneccion)")
    '        PrintLine(FileCls, TAB(9), "da.Fill(dt)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Me.AsignarTipos()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "If dt.Rows.Count = 0 Then")
    '        PrintLine(FileCls, TAB(13), "MessageBox.Show(" & """" & "No se a encontrado el Registro" & """" & ")")
    '        PrintLine(FileCls, TAB(13), "Exit Sub")
    '        PrintLine(FileCls, TAB(9), "Else")
    '        PrintLine(FileCls, TAB(13), "dr = dt.Rows(0)")
    '        PrintLine(FileCls, TAB(9), "End If")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "If MessageBox.Show(" & """" & "Desea Realmente Eliminar el Registro " & """" & ", " & """" & "BORRAR" & """" & ", _")
    '        PrintLine(FileCls, TAB(13), "MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) _")
    '        PrintLine(FileCls, TAB(13), "= DialogResult.No Then")
    '        PrintLine(FileCls, TAB(15), "Exit Sub")
    '        PrintLine(FileCls, TAB(9), "End If")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Try")
    '        PrintLine(FileCls, TAB(13), "dt.Rows(0).Delete()")
    '        PrintLine(FileCls, TAB(13), "CrearComandoUpdate()")
    '        PrintLine(FileCls, TAB(13), "Guardar()")
    '        PrintLine(FileCls, TAB(9), "Catch ex As Exception")
    '        PrintLine(FileCls, TAB(13), "If Err.Number = 5 Then")
    '        PrintLine(FileCls, TAB(15), "MessageBox.Show(ex.Message)")
    '        PrintLine(FileCls, TAB(13), "End If")
    '        PrintLine(FileCls, TAB(9), "End Try")
    '        PrintLine(FileCls, TAB(5), "End Sub")
    '        PrintLine(FileCls, "")

    '        'Private Sub AsignarTipos()

    '        PrintLine(FileCls, TAB(5), "'asigno el tipo de datos a los parametros")
    '        PrintLine(FileCls, TAB(5), "Private Sub AsignarTipos()")
    '        PrintLine(FileCls, TAB(9), "For Each dc As DataColumn In dt.Columns")
    '        PrintLine(FileCls, TAB(13), "Select Case dc.ColumnName")
    '        For Each reg In arrEstructura
    '            PrintLine(FileCls, TAB(16), "Case " & """" & reg.nombre & """")
    '            PrintLine(FileCls, TAB(19), "dc.DataType = Type.GetType(" & """" & "System." & reg.tiposql & """" & ")")
    '        Next
    '        PrintLine(FileCls, TAB(13), "End Select")
    '        PrintLine(FileCls, TAB(9), "Next")
    '        PrintLine(FileCls, TAB(5), "End Sub")
    '        PrintLine(FileCls, "")

    '        'Private Sub CrearComandoUpdate()

    '        PrintLine(FileCls, TAB(5), "'creo el commandbuilder")
    '        PrintLine(FileCls, TAB(5), "Private Sub CrearComandoUpdate()")
    '        PrintLine(FileCls, TAB(9), "Dim cmd As New SqlCeCommandBuilder(da)")
    '        PrintLine(FileCls, TAB(9), "da.UpdateCommand = cmd.GetUpdateCommand")
    '        PrintLine(FileCls, TAB(5), "End Sub")
    '        PrintLine(FileCls, "")

    '        'Public Sub Cancelar()

    '        PrintLine(FileCls, TAB(5), "'cancelo los cambios del datatable")
    '        PrintLine(FileCls, TAB(5), "Public Sub Cancelar()")
    '        PrintLine(FileCls, TAB(9), "dt.Clear()")
    '        PrintLine(FileCls, TAB(5), "End Sub")
    '        PrintLine(FileCls, "")

    '        'Public Function DataTable()

    '        PrintLine(FileCls, TAB(5), "'asigno el datatable")
    '        PrintLine(FileCls, TAB(5), "Public Function DataTable() As DataTable")
    '        PrintLine(FileCls, TAB(9), "Return dt")
    '        PrintLine(FileCls, TAB(5), "End Function")
    '        PrintLine(FileCls, "")

    '        'Public Sub Guardar()

    '        PrintLine(FileCls, TAB(5), "'actualizo la base de datos")
    '        PrintLine(FileCls, TAB(5), "Public Sub Guardar()")
    '        PrintLine(FileCls, TAB(9), "da.Update(dt)")
    '        PrintLine(FileCls, TAB(5), "End Sub")
    '        PrintLine(FileCls, "")

    '        'Public Function Cargar dt()

    '        PrintLine(FileCls, TAB(5), "'funcion que carga el datatable")
    '        PrintLine(FileCls, TAB(5), "Public Function Cargar() As DataTable")
    '        PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '        PrintLine(FileCls, TAB(9), "da = New SqlCeDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & " WHERE id_" & txtTabla.Text.Trim & " = 0" & """" & ", cnn.Coneccion)")
    '        PrintLine(FileCls, TAB(9), "da.MissingSchemaAction = MissingSchemaAction.AddWithKey")
    '        PrintLine(FileCls, TAB(9), "da.Fill(dt)")
    '        PrintLine(FileCls, TAB(9), "Return dt")
    '        PrintLine(FileCls, TAB(5), "End Function")
    '        PrintLine(FileCls, "")

    '        'Public Function Buscar

    '        PrintLine(FileCls, TAB(5), "'funcion de busqueda")
    '        PrintLine(FileCls, TAB(5), "Public Function Buscar(ByVal Nombre As String) As DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim oda As New SqlCeDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & "" & """" & ", cnn.Coneccion)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Return odt")
    '        PrintLine(FileCls, TAB(5), "End Function")
    '        PrintLine(FileCls, "")

    '        'Public Function ConsultarTodo()

    '        PrintLine(FileCls, TAB(5), "'funcion de consulta")
    '        PrintLine(FileCls, TAB(5), "Public Function ConsultarTodo() As DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim oda As New SqlCeDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & "" & """" & ", cnn.Coneccion)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Return odt")
    '        PrintLine(FileCls, TAB(5), "End Function")
    '        PrintLine(FileCls, "")

    '        'Public Function GetCmb()

    '        PrintLine(FileCls, TAB(5), "'funcion para llenar el combo")
    '        PrintLine(FileCls, TAB(5), "Public Function GetCmb() As DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim oda As New SqlCeDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & "" & """" & ", cnn.Coneccion)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Return odt")
    '        PrintLine(FileCls, TAB(5), "End Function")
    '        PrintLine(FileCls, "")

    '        'public function GetOne()

    '        PrintLine(FileCls, TAB(5), "'funcion que trae un registro poniendo el id")
    '        PrintLine(FileCls, TAB(5), "Public Function GetOne(ByVal id_" & txtTabla.Text.Trim & " As Integer) As DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '        PrintLine(FileCls, TAB(9), "oda = New SqlCeDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & " WHERE " & idTabla & " = " & """" & " & _")
    '        PrintLine(FileCls, TAB(9), "id_" & txtTabla.Text.Trim & ", cnn.Coneccion)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Return odt")
    '        PrintLine(FileCls, TAB(5), "End Function")
    '        PrintLine(FileCls, "")

    '        'Public Function Consultar con oda

    '        'PrintLine(FileCls, TAB(5), "'ejemplo de consulta con parametros")
    '        'PrintLine(FileCls, TAB(5), "Public Function Consultar(ByVal id_" & txtTabla.Text.Trim & " As Integer) As DataTable")
    '        'PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '        'PrintLine(FileCls, TAB(9), "Dim oda As New SqlCeDataAdapter(" & """" & "Ejemplo_" & txtTabla.Text.Trim & """" & ", cnn.Coneccion)")
    '        'PrintLine(FileCls, "")
    '        'PrintLine(FileCls, "")
    '        'PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '        'PrintLine(FileCls, TAB(9), "Return odt")
    '        'PrintLine(FileCls, TAB(5), "End Function")
    '        'PrintLine(FileCls, "")

    '        'Public Function ConsultarDecimal

    '        'PrintLine(FileCls, TAB(5), "'ejemplo de consulta con parametros trayendo un decimal")
    '        'PrintLine(FileCls, TAB(5), "Public Function ConsultarDecimal(ByVal id_" & txtTabla.Text.Trim & " As Integer) As Decimal")
    '        'PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '        'PrintLine(FileCls, TAB(9), "Dim oda As New SqlCeDataAdapter(" & """" & "Ejemplo_" & txtTabla.Text.Trim & "Decimal" & """" & ", cnn.Coneccion)")
    '        'PrintLine(FileCls, TAB(9), "Dim Total As Decimal")
    '        'PrintLine(FileCls, "")
    '        'PrintLine(FileCls, "")
    '        'PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '        'PrintLine(FileCls, TAB(9), "Total = odt.Rows(0).Item(" & """" & "Total" & """" & ").ToString")
    '        'PrintLine(FileCls, TAB(9), "Return Total")
    '        'PrintLine(FileCls, TAB(5), "End Function")
    '        'PrintLine(FileCls, "")

    '        'Public Function ControlarSiExiste

    '        'PrintLine(FileCls, TAB(5), "'ejemplo de consulta con parametros trayendo un boolean")
    '        'PrintLine(FileCls, TAB(5), "Public Function ControlarSiExiste(ByVal id_" & txtTabla.Text.Trim & " As Integer) As Boolean")
    '        'PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '        'PrintLine(FileCls, TAB(9), "Dim oda As New SqlCeDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & "_Check" & """" & ", cnn.Coneccion)")
    '        'PrintLine(FileCls, TAB(9), "Dim Total As Integer")
    '        'PrintLine(FileCls, "")
    '        'PrintLine(FileCls, "")
    '        'PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '        'PrintLine(FileCls, TAB(9), "Total = odt.Rows(0).Item(" & """" & "Total" & """" & ").ToString")
    '        'PrintLine(FileCls, TAB(9), "If Total = 0 Then")
    '        'PrintLine(FileCls, TAB(13), "Return True  'NO EXISTE")
    '        'PrintLine(FileCls, TAB(9), "Else")
    '        'PrintLine(FileCls, TAB(13), "Return False 'SI EXISTE")
    '        'PrintLine(FileCls, TAB(9), "End If")
    '        'PrintLine(FileCls, TAB(5), "End Function")
    '        'PrintLine(FileCls, "")

    '        'borra toda la tabla
    '        PrintLine(FileCls, TAB(5), "'borra todos los datos de la tabla")
    '        PrintLine(FileCls, TAB(5), "Public Sub BorrarTodo()")
    '        PrintLine(FileCls, TAB(9), "Dim oConexion As New Conexion")
    '        PrintLine(FileCls, TAB(9), "Dim Command As SqlCeCommand = New SqlCeCommand()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Command.Connection = oConexion.Coneccion")
    '        PrintLine(FileCls, TAB(9), "Command.CommandText = ""DELETE FROM " & Me.txtTabla.Text & """")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "oConexion.Abrir()")
    '        PrintLine(FileCls, TAB(9), "Command.ExecuteNonQuery()")
    '        PrintLine(FileCls, TAB(9), "oConexion.Cerrar()")
    '        PrintLine(FileCls, TAB(5), "End Sub")
    '        PrintLine(FileCls, "")

    '        'consulta el archivo txt para importarlo
    '        PrintLine(FileCls, TAB(5), "'importa los datos de una campaña desde el txt")
    '        PrintLine(FileCls, TAB(5), "Public Function Cargar_Archivo() As DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "'Dim oda As New System.Data.OleDb.OleDbDataAdapter( _")
    '        PrintLine(FileCls, TAB(9), "'""SELECT * FROM " & Me.txtTabla.Text & ".txt"", cnntxt.Coneccion)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "'oda.Fill(odt)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Return odt")
    '        PrintLine(FileCls, TAB(5), "End Function")
    '        PrintLine(FileCls, "")

    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(13), "'********************************************")
    '        PrintLine(FileCls, TAB(13), "'    CODIGO AGREGADO A LA CLASE ORIGINAL")
    '        PrintLine(FileCls, TAB(13), "'********************************************")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        ' Comienzo a generar la clase
    '        PrintLine(FileCls, "End Class")

    '        ' Cierro el PathCls de versión
    '        FileClose(FileCls)

    '    End If






















    '    If Mid$(PathInsert, Len(PathInsert) - 3, 3) <> ".sql" Then
    '        PathInsert = Me.txtPath.Text & "\sp" & Me.txtTabla.Text & ".sql"
    '    Else
    '        PathInsert = Me.txtPath.Text & "\" & PathInsert
    '    End If

    '    ' Defino variables
    '    Dim FileInsert As Integer = FreeFile()
    '    Dim regInsert As regSistemaTabla

    '    ' Abro un PathCls de texto (si el mismo existía se reempleza)
    '    FileOpen(FileInsert, PathInsert, OpenMode.Output)
    '    Dim Prefijo As String = "cop_"


    '    '*********************************************
    '    '
    '    'DEFINICION DEL CODIGO PARA LOS SP
    '    '
    '    '*********************************************

    '    'use base datos
    '    PrintLine(FileInsert, TAB(0), "USE [" & Me.lstBaseDato.Text.Trim & "]")
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")

    '    'sp insert
    '    PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_Insert]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '    PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Insert]")
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Insert]")

    '    Dim Contador As Integer = 1
    '    For Each regInsert In arrEstructura
    '        If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '            PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo & "  output,")
    '        ElseIf Contador = NumeroFilas Then
    '            PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo & "  " & regInsert.sptamaño)
    '        Else
    '            PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo & "  " & regInsert.sptamaño & ",")
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileInsert, TAB(0), "AS")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "INSERT INTO [dbo].[" & Me.txtTabla.Text & "]")
    '    PrintLine(FileInsert, TAB(0), "(")
    '    Contador = 1
    '    For Each regInsert In arrEstructura
    '        If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '        ElseIf Contador = NumeroFilas Then
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "]")
    '        Else
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "],")
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileInsert, ")")
    '    PrintLine(FileInsert, TAB(0), "VALUES")
    '    PrintLine(FileInsert, TAB(0), "(")
    '    Contador = 1
    '    For Each regInsert In arrEstructura
    '        If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '        ElseIf Contador = NumeroFilas Then
    '            PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre)
    '        Else
    '            PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & ",")
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileInsert, ")")
    '    Contador = 1
    '    For Each regInsert In arrEstructura
    '        If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '            PrintLine(FileInsert, TAB(0), "SET @" & regInsert.nombre & " = @@IDENTITY")
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")

    '    'sp delete

    '    PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_Delete]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '    PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Delete]")
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Delete]")

    '    For Each regInsert In arrEstructura
    '        If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '            PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo)
    '            Exit For
    '        End If
    '    Next
    '    PrintLine(FileInsert, TAB(0), "AS")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "DELETE FROM [dbo].[" & Me.txtTabla.Text & "]")
    '    PrintLine(FileInsert, TAB(0), "WHERE")
    '    For Each regInsert In arrEstructura
    '        If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "]  =  @" & regInsert.nombre)
    '            Exit For
    '        End If
    '    Next
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")

    '    'sp GetAll

    '    PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_GetAll]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '    PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_GetAll]")
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_GetAll]")
    '    PrintLine(FileInsert, TAB(0), "AS")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "SELECT")

    '    Contador = 1
    '    For Each regInsert In arrEstructura
    '        If Contador = NumeroFilas Then
    '            If regInsert.tipo = "varchar" Then
    '                PrintLine(FileInsert, TAB(5), "RTRIM(" & regInsert.nombre & ") AS " & regInsert.nombre.ToUpper)
    '            Else
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] AS " & regInsert.nombre.ToUpper)
    '            End If
    '        Else
    '            If regInsert.tipo = "varchar" Then
    '                PrintLine(FileInsert, TAB(5), "RTRIM(" & regInsert.nombre & ") AS " & regInsert.nombre.ToUpper & " ,")
    '            Else
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] AS " & regInsert.nombre.ToUpper & " ,")
    '            End If
    '        End If
    '        Contador = Contador + 1
    '    Next

    '    PrintLine(FileInsert, TAB(0), "FROM")
    '    PrintLine(FileInsert, TAB(5), "[dbo].[" & Me.txtTabla.Text & "]")
    '    PrintLine(FileInsert, TAB(0), "ORDER BY")

    '    Contador = 0
    '    For Each regInsert In arrEstructura
    '        If Contador = 1 Then
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "]")
    '            Exit For
    '        End If
    '        Contador = Contador + 1
    '    Next

    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")

    '    'sp GetCmb

    '    PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_GetCmb]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '    PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_GetCmb]")
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_GetCmb]")
    '    PrintLine(FileInsert, TAB(0), "AS")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "SELECT")

    '    Contador = 1
    '    For Each regInsert In arrEstructura
    '        If Contador = NumeroFilas Then
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "]")
    '        Else
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "],")
    '        End If
    '        Contador = Contador + 1
    '    Next

    '    PrintLine(FileInsert, TAB(0), "FROM")
    '    PrintLine(FileInsert, TAB(5), "[dbo].[" & Me.txtTabla.Text & "]")
    '    PrintLine(FileInsert, TAB(0), "ORDER BY")

    '    Contador = 0
    '    For Each regInsert In arrEstructura
    '        If Contador = 1 Then
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "]")
    '            Exit For
    '        End If
    '        Contador = Contador + 1
    '    Next

    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")

    '    'sp update

    '    PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_Update]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '    PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Update]")
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Update]")

    '    Contador = 1
    '    For Each regInsert In arrEstructura
    '        If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '            PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo & "  output,")
    '        ElseIf Contador = NumeroFilas Then
    '            PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo & "  " & regInsert.sptamaño)
    '        Else
    '            PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo & "  " & regInsert.sptamaño & ",")
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileInsert, TAB(0), "AS")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "UPDATE [dbo].[" & Me.txtTabla.Text & "] SET")
    '    Contador = 1
    '    For Each regInsert In arrEstructura
    '        If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '        ElseIf Contador = NumeroFilas Then
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] = @" & regInsert.nombre)
    '        Else
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] = @" & regInsert.nombre & ",")
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileInsert, TAB(0), "WHERE")
    '    For Each regInsert In arrEstructura
    '        If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "]  =  @" & regInsert.nombre)
    '            Exit For
    '        End If
    '    Next
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")

    '    'sp exist

    '    PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_Exist]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '    PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Exist]")
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Exist]")
    '    Contador = 1
    '    For Each regInsert In arrEstructura
    '        If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '            'PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo & "  output,")
    '        ElseIf Contador = NumeroFilas Then
    '            PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo & "  " & regInsert.sptamaño)
    '        Else
    '            PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo & "  " & regInsert.sptamaño & ",")
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileInsert, TAB(0), "AS")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "DECLARE @total int")
    '    PrintLine(FileInsert, TAB(0), "SET @total = 0")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "SELECT")
    '    For Each regInsert In arrEstructura
    '        If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '            PrintLine(FileInsert, TAB(5), "@total = " & regInsert.nombre)
    '            Exit For
    '        End If
    '    Next
    '    PrintLine(FileInsert, TAB(0), "FROM")
    '    PrintLine(FileInsert, TAB(5), "[dbo].[" & Me.txtTabla.Text & "]")
    '    PrintLine(FileInsert, TAB(0), "WHERE")
    '    Contador = 1
    '    For Each regInsert In arrEstructura
    '        If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '        ElseIf Contador = NumeroFilas Then
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] = @" & regInsert.nombre)
    '        Else
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] = @" & regInsert.nombre & " AND")
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "IF @total IS NULL")
    '    PrintLine(FileInsert, TAB(0), "BEGIN")
    '    PrintLine(FileInsert, TAB(5), "SET @total=0")
    '    PrintLine(FileInsert, TAB(0), "END")
    '    PrintLine(FileInsert, TAB(0), "SELECT @total AS Total")
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")


    '    'sp getone

    '    PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_GetOne]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '    PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_GetOne]")
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_GetOne]")
    '    For Each regInsert In arrEstructura
    '        If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '            PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo)
    '            Exit For
    '        End If
    '    Next
    '    PrintLine(FileInsert, TAB(0), "AS")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "SELECT")

    '    Contador = 1
    '    For Each regInsert In arrEstructura
    '        If Contador = NumeroFilas Then
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] AS " & regInsert.nombre.ToUpper)
    '        Else
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] AS " & regInsert.nombre.ToUpper & " ,")
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileInsert, TAB(0), "FROM")
    '    PrintLine(FileInsert, TAB(5), "[dbo].[" & Me.txtTabla.Text & "]")
    '    PrintLine(FileInsert, TAB(0), "WHERE")
    '    For Each regInsert In arrEstructura
    '        If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "]  =  @" & regInsert.nombre)
    '            Exit For
    '        End If
    '    Next
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")


    '    'sp find

    '    PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_Find]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '    PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Find]")
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Find]")
    '    PrintLine(FileInsert, TAB(5), "@nombre NVARCHAR (30)=NULL")
    '    PrintLine(FileInsert, TAB(0), "AS SET NOCOUNT ON")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "IF @nombre IS NOT NULL")
    '    PrintLine(FileInsert, TAB(0), "BEGIN")
    '    PrintLine(FileInsert, TAB(0), "SELECT @nombre=RTRIM(@nombre)+'%'")
    '    PrintLine(FileInsert, TAB(0), "SELECT")

    '    Contador = 1
    '    For Each regInsert In arrEstructura
    '        If Contador = NumeroFilas Then
    '            If regInsert.tipo = "varchar" Then
    '                PrintLine(FileInsert, TAB(5), "RTRIM(" & regInsert.nombre & ") AS " & regInsert.nombre.ToUpper)
    '            Else
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] AS " & regInsert.nombre.ToUpper)
    '            End If
    '        Else
    '            If regInsert.tipo = "varchar" Then
    '                PrintLine(FileInsert, TAB(5), "RTRIM(" & regInsert.nombre & ") AS " & regInsert.nombre.ToUpper & " ,")
    '            Else
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] AS " & regInsert.nombre.ToUpper & " ,")
    '            End If
    '        End If
    '        Contador = Contador + 1
    '    Next

    '    PrintLine(FileInsert, TAB(0), "FROM")
    '    PrintLine(FileInsert, TAB(5), "[dbo].[" & Me.txtTabla.Text & "]")
    '    PrintLine(FileInsert, TAB(0), "WHERE")
    '    Contador = 0
    '    For Each regInsert In arrEstructura
    '        If Contador = 1 Then
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] LIKE @nombre+'%'")
    '            Exit For
    '        End If
    '        Contador = Contador + 1
    '    Next

    '    PrintLine(FileInsert, TAB(0), "ORDER BY")
    '    Contador = 0
    '    For Each regInsert In arrEstructura
    '        If Contador = 1 Then
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "]")
    '            Exit For
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileInsert, TAB(0), "END")
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")




    '    'insertar el registro ninguno
    '    'sp InsertOne

    '    PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_InsertOne]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '    PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_InsertOne]")
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_InsertOne]")
    '    PrintLine(FileInsert, TAB(0), "AS")
    '    PrintLine(FileInsert, "")
    '    PrintLine(FileInsert, TAB(0), "INSERT INTO [dbo].[" & Me.txtTabla.Text & "]")
    '    PrintLine(FileInsert, TAB(0), "(")
    '    Contador = 1
    '    For Each regInsert In arrEstructura
    '        If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '        ElseIf Contador = NumeroFilas Then
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "]")
    '        Else
    '            PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "],")
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileInsert, ")")
    '    PrintLine(FileInsert, TAB(0), "VALUES")
    '    PrintLine(FileInsert, TAB(0), "(")
    '    Contador = 1
    '    For Each regInsert In arrEstructura
    '        If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '        ElseIf Contador = NumeroFilas Then
    '            PrintLine(FileInsert, TAB(5), regInsert.valorinsert)
    '        Else
    '            PrintLine(FileInsert, TAB(5), regInsert.valorinsert & ",")
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileInsert, ")")

    '    ' If Me.chkGo.Checked = True Then
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    '  End If

    '    PrintLine(FileInsert, "")



















    '    FileClose(FileInsert)


    '    '*********************************************
    '    '
    '    'DEFINICION DEL CODIGO PARA FORMULARIOS ABM
    '    '
    '    '*********************************************

    '    If Mid$(PathFrm, Len(PathFrm) - 3, 3) <> ".vb" Then
    '        PathFrm = Me.txtPath.Text & "\frmAbm" & Me.txtTabla.Text & ".vb"
    '    Else
    '        PathFrm = Me.txtPath.Text & "\" & PathFrm
    '    End If

    '    ' Defino variables
    '    Dim FileFrm As Integer = FreeFile()

    '    ' Abro un PathCls de texto (si el mismo existía se reempleza)
    '    FileOpen(FileInsert, PathFrm, OpenMode.Output)

    '    'definicion de variables

    '    PrintLine(FileFrm, TAB(0), "Public Class frmAbm" & Me.txtTabla.Text)
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(4), "Dim odt As DataTable")
    '    PrintLine(FileFrm, TAB(4), "Dim BanderaConsulta" & Me.txtTabla.Text & " As Integer")
    '    PrintLine(FileFrm, "")

    '    'Private Sub frmAbm_Load

    '    PrintLine(FileFrm, TAB(4), "Private Sub frmAbm" & Me.txtTabla.Text & "_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load")
    '    PrintLine(FileFrm, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnAgregar, " & """" & "Incorporar Nuevo " & Me.txtTabla.Text & """" & ")")
    '    PrintLine(FileFrm, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnBorrar, " & """" & "Borrar un  " & Me.txtTabla.Text & " Existente" & """" & ")")
    '    PrintLine(FileFrm, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnModificar, " & """" & "Modificar  un " & Me.txtTabla.Text & " Existente" & """" & ")")
    '    PrintLine(FileFrm, TAB(8), "'Me.ttGeneral.SetToolTip(Me.btnConsultar, " & """" & "Consultar Datos del  " & Me.txtTabla.Text & " Existente" & """" & ")")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(8), "Dim odt As DataTable")
    '    PrintLine(FileFrm, TAB(8), "odt = o" & Me.txtTabla.Text & ".Cargar()")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(8), "RefrescarGrilla()")
    '    PrintLine(FileFrm, TAB(8), "Me.txtBuscar.Focus()")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.Text = " & """" & Me.txtTabla.Text & """")
    '    PrintLine(FileFrm, TAB(8), "Me.Text = " & """" & Me.txtTabla.Text & """")
    '    PrintLine(FileFrm, TAB(8), "Me.CancelButton = Me.btnSalir")
    '    PrintLine(FileFrm, TAB(8), "Me.BackColor = Color.Teal")
    '    PrintLine(FileFrm, TAB(8), "Me.StartPosition = FormStartPosition.CenterScreen")
    '    PrintLine(FileFrm, TAB(8), "Me.MinimizeBox = False")
    '    PrintLine(FileFrm, TAB(8), "Me.MaximizeBox = False")
    '    PrintLine(FileFrm, TAB(4), "End Sub")
    '    PrintLine(FileFrm, "")

    '    'Public Sub RefrescarGrilla()

    '    PrintLine(FileFrm, TAB(4), "Public Sub RefrescarGrilla()")
    '    PrintLine(FileFrm, TAB(8), "Dim odt As DataTable")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(8), "odt = o" & Me.txtTabla.Text & ".ConsultarTodo()")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.DataSource = odt")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.Columns(0).Visible = False")
    '    PrintLine(FileFrm, TAB(4), "End Sub")
    '    PrintLine(FileFrm, "")

    '    'Private Sub txtBuscar_KeyPress

    '    PrintLine(FileFrm, TAB(4), "Private Sub txtBuscar_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBuscar.KeyPress")
    '    PrintLine(FileFrm, TAB(8), "If e.KeyChar.ToString = vbCr Then")
    '    PrintLine(FileFrm, TAB(12), "Me.btnModificar.Focus()")
    '    PrintLine(FileFrm, TAB(8), "End If")
    '    PrintLine(FileFrm, TAB(4), "End Sub")
    '    PrintLine(FileFrm, "")

    '    'Private Sub txtBuscar_TextChanged

    '    PrintLine(FileFrm, TAB(4), "Private Sub txtBuscar_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged")
    '    PrintLine(FileFrm, TAB(8), "If Me.txtBuscar.Text = " & """""" & " Then")
    '    PrintLine(FileFrm, TAB(12), "Me.txtBuscar.Text = " & """" & " " & """")
    '    PrintLine(FileFrm, TAB(8), "End If")
    '    PrintLine(FileFrm, TAB(8), "odt = o" & Me.txtTabla.Text & ".Buscar(Me.txtBuscar.Text)")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.DataSource = odt")
    '    PrintLine(FileFrm, TAB(4), "End Sub")
    '    PrintLine(FileFrm, "")

    '    'Private Sub dgv1_CurrentCellChanged

    '    PrintLine(FileFrm, TAB(4), "Private Sub dgv1_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv1.CurrentCellChanged")
    '    PrintLine(FileFrm, TAB(8), "Try")
    '    PrintLine(FileFrm, TAB(12), "Me.lblid_pk.Text = Me.dgv1.Item(0, Me.dgv1.CurrentRow.Index).Value")
    '    PrintLine(FileFrm, TAB(8), "Catch ex As Exception")
    '    PrintLine(FileFrm, TAB(8), "End Try")
    '    PrintLine(FileFrm, TAB(4), "End Sub")
    '    PrintLine(FileFrm, "")

    '    'Private Sub Botones_Click

    '    PrintLine(FileFrm, TAB(4), "Private Sub Botones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click, btnAyuda.Click, btnBorrar.Click, btnModificar.Click, btnSalir.Click")
    '    PrintLine(FileFrm, TAB(8), "Dim btnTemp As New Button")
    '    PrintLine(FileFrm, TAB(8), "Dim frmDetalle As New frmDetalle" & Me.txtTabla.Text)
    '    PrintLine(FileFrm, TAB(8), "btnTemp = sender")
    '    PrintLine(FileFrm, TAB(8), "Try")
    '    PrintLine(FileFrm, TAB(12), "Select Case btnTemp.Name")

    '    PrintLine(FileFrm, TAB(16), "Case " & """" & "btnAgregar" & """")
    '    PrintLine(FileFrm, TAB(20), "Bandera" & Me.txtTabla.Text & " = 1")
    '    PrintLine(FileFrm, TAB(20), "Me.AddOwnedForm(frmDetalle)")
    '    PrintLine(FileFrm, TAB(20), "o" & Me.txtTabla.Text & ".Insertar()")
    '    PrintLine(FileFrm, TAB(20), "frmDetalle.CargarCombos()")
    '    PrintLine(FileFrm, TAB(20), "frmDetalle.ShowDialog()")

    '    PrintLine(FileFrm, TAB(16), "Case " & """" & "btnModificar" & """")
    '    PrintLine(FileFrm, TAB(20), "If Not IsNumeric(Me.lblid_pk.Text) Then")
    '    PrintLine(FileFrm, TAB(24), "Exit Sub")
    '    PrintLine(FileFrm, TAB(20), "End If")

    '    '  PrintLine(FileFrm, TAB(16), "If Me.lblid_pk.Text <= 1 Then")
    '    '  PrintLine(FileFrm, TAB(20), "MessageBox.Show(" & """" & "No se Puede Modificar el Registro" & """" & ")")
    '    '  PrintLine(FileFrm, TAB(20), "Exit Sub")
    '    '  PrintLine(FileFrm, TAB(16), "End If")

    '    PrintLine(FileFrm, TAB(20), "Bandera" & Me.txtTabla.Text & " = 2")
    '    PrintLine(FileFrm, TAB(20), "Me.AddOwnedForm(frmDetalle)")
    '    PrintLine(FileFrm, TAB(20), "o" & Me.txtTabla.Text & ".Modificar(Me.lblid_pk.Text)")
    '    PrintLine(FileFrm, TAB(20), "frmDetalle.CargarCombos()")
    '    PrintLine(FileFrm, TAB(20), "frmDetalle.ShowDialog()")
    '    PrintLine(FileFrm, TAB(20), "RefrescarGrilla()")

    '    PrintLine(FileFrm, TAB(16), "Case " & """" & "btnBorrar" & """")
    '    PrintLine(FileFrm, TAB(20), "If Not IsNumeric(Me.lblid_pk.Text) Then")
    '    PrintLine(FileFrm, TAB(24), "Exit Sub")
    '    PrintLine(FileFrm, TAB(20), "End If")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(20), "If MessageBox.Show(" & """" & "Desea Realmente Eliminar el Registro " & """" & ", " & """" & "BORRAR" & """" & ", _")
    '    PrintLine(FileFrm, TAB(20), "MessageBoxButtons.YesNo, MessageBoxIcon.Question) _")
    '    PrintLine(FileFrm, TAB(20), "= Windows.Forms.DialogResult.No Then")
    '    PrintLine(FileFrm, TAB(24), "Exit Sub")
    '    PrintLine(FileFrm, TAB(20), "End If")
    '    PrintLine(FileFrm, "")

    '    '   PrintLine(FileFrm, TAB(16), "If Me.lblid_pk.Text <= 1 Then")
    '    '   PrintLine(FileFrm, TAB(20), "MessageBox.Show(" & """" & "No se Puede Borrar el Registro" & """" & ")")
    '    '   PrintLine(FileFrm, TAB(20), "Exit Sub")
    '    '   PrintLine(FileFrm, TAB(16), "End If")

    '    PrintLine(FileFrm, TAB(20), "o" & Me.txtTabla.Text & ".Borrar(Me.lblid_pk.Text)")
    '    PrintLine(FileFrm, TAB(20), "RefrescarGrilla()")

    '    PrintLine(FileFrm, TAB(16), "Case " & """" & "btnAyuda" & """")
    '    PrintLine(FileFrm, TAB(20), "'Process.Start(PathAyuda + " & """" & "frmAbm" & Me.txtTabla.Text & ".avi" & """" & ")")
    '    PrintLine(FileFrm, TAB(16), "Case " & """" & "btnSalir" & """")
    '    PrintLine(FileFrm, TAB(20), "Me.Close()")
    '    PrintLine(FileFrm, TAB(12), "End Select")
    '    PrintLine(FileFrm, TAB(12), "Me.txtBuscar.Text = """"")
    '    PrintLine(FileFrm, TAB(12), "Me.txtBuscar.Focus()")
    '    PrintLine(FileFrm, TAB(8), "Catch ex As Exception")
    '    PrintLine(FileFrm, TAB(8), "MessageBox.Show(ex.Message)")
    '    PrintLine(FileFrm, TAB(8), "End Try")
    '    PrintLine(FileFrm, TAB(4), "End Sub")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(4), "End Class")
    '    FileClose(FileFrm)





    '    '*********************************************
    '    '
    '    'DEFINICION DEL CODIGO PARA FORMULARIOS ABM 2 PARTE
    '    '
    '    '*********************************************

    '    If Mid$(PathFrm, Len(PathFrm) - 3, 3) <> ".vb" Then
    '        PathFrm = Me.txtPath.Text & "\frmAbm" & Me.txtTabla.Text & ".Designer.vb"
    '    Else
    '        PathFrm = Me.txtPath.Text & "\" & PathFrm
    '    End If

    '    ' Defino variables
    '    FileFrm = FreeFile()

    '    ' Abro un PathCls de texto (si el mismo existía se reempleza)
    '    FileOpen(FileInsert, PathFrm, OpenMode.Output)

    '    'definicion de variables
    '    PrintLine(FileFrm, TAB(0), "<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _")
    '    PrintLine(FileFrm, TAB(0), "Partial Class frmAbm" & Me.txtTabla.Text)
    '    PrintLine(FileFrm, TAB(4), "Inherits System.Windows.Forms.Form")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(4), "'Form reemplaza a Dispose para limpiar la lista de componentes.")
    '    PrintLine(FileFrm, TAB(4), "<System.Diagnostics.DebuggerNonUserCode()> _")
    '    PrintLine(FileFrm, TAB(4), "Protected Overrides Sub Dispose(ByVal disposing As Boolean)")
    '    PrintLine(FileFrm, TAB(8), "If disposing AndAlso components IsNot Nothing Then")
    '    PrintLine(FileFrm, TAB(12), "components.Dispose()")
    '    PrintLine(FileFrm, TAB(8), "End If")
    '    PrintLine(FileFrm, TAB(8), "MyBase.Dispose(disposing)")
    '    PrintLine(FileFrm, TAB(4), "End Sub")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(4), "'Requerido por el Diseñador de Windows Forms")
    '    PrintLine(FileFrm, TAB(4), "Private components As System.ComponentModel.IContainer")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(4), "'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento")
    '    PrintLine(FileFrm, TAB(4), "'Se puede modificar usando el Diseñador de Windows Forms.")
    '    PrintLine(FileFrm, TAB(4), "'No lo modifique con el editor de código.")
    '    PrintLine(FileFrm, TAB(4), "<System.Diagnostics.DebuggerStepThrough()> _")
    '    PrintLine(FileFrm, TAB(4), "Private Sub InitializeComponent()")
    '    PrintLine(FileFrm, TAB(8), "Me.components = New System.ComponentModel.Container")
    '    PrintLine(FileFrm, TAB(8), "Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAbm" & Me.txtTabla.Text & "))")
    '    PrintLine(FileFrm, TAB(8), "Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle")
    '    PrintLine(FileFrm, TAB(8), "Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle")
    '    PrintLine(FileFrm, TAB(8), "Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle")
    '    PrintLine(FileFrm, TAB(8), "Me.txtBuscar = New System.Windows.Forms.TextBox")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1 = New System.Windows.Forms.GroupBox")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda = New System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir = New System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar = New System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar = New System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar = New System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar = New System.Windows.Forms.Label")
    '    PrintLine(FileFrm, TAB(8), "Me.ttGeneral = New System.Windows.Forms.ToolTip(Me.components)")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo = New System.Windows.Forms.Label")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1 = New System.Windows.Forms.DataGridView")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk = New System.Windows.Forms.Label")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.SuspendLayout()")
    '    PrintLine(FileFrm, TAB(8), "CType(Me.dgv1, System.ComponentModel.ISupportInitialize).BeginInit()")
    '    PrintLine(FileFrm, TAB(8), "Me.SuspendLayout()")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'txtBuscar")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.txtBuscar.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper")
    '    PrintLine(FileFrm, TAB(8), "Me.txtBuscar.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "Me.txtBuscar.Location = New System.Drawing.Point(128, 561)")
    '    PrintLine(FileFrm, TAB(8), "Me.txtBuscar.Name = ""txtBuscar""")
    '    PrintLine(FileFrm, TAB(8), "Me.txtBuscar.Size = New System.Drawing.Size(873, 26)")
    '    PrintLine(FileFrm, TAB(8), "Me.txtBuscar.TabIndex = 563")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'GroupBox1")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.Controls.Add(Me.btnAyuda)")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.Controls.Add(Me.btnSalir)")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.Controls.Add(Me.btnBorrar)")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.Controls.Add(Me.btnModificar)")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.Controls.Add(Me.btnAgregar)")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.Location = New System.Drawing.Point(15, 606)")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.Name = ""GroupBox1""")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.Size = New System.Drawing.Size(986, 107)")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.TabIndex = 564")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.TabStop = False")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'btnAyuda")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.BackColor = System.Drawing.Color.Gainsboro")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.ForeColor = System.Drawing.Color.Black")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.Image = CType(resources.GetObject(""btnAyuda.Image""), System.Drawing.Image)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.Location = New System.Drawing.Point(715, 19)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.Name = ""btnAyuda""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.Size = New System.Drawing.Size(86, 71)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.TabIndex = 11")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.Text = ""A&yuda""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.UseVisualStyleBackColor = True")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'btnSalir")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.ForeColor = System.Drawing.Color.Black")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.Image = CType(resources.GetObject(""btnSalir.Image""), System.Drawing.Image)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.Location = New System.Drawing.Point(858, 19)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.Name = ""btnSalir""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.Size = New System.Drawing.Size(86, 71)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.TabIndex = 12")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.Text = ""&Salir""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.UseVisualStyleBackColor = True")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'btnBorrar")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.Cursor = System.Windows.Forms.Cursors.Hand")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.Image = CType(resources.GetObject(""btnBorrar.Image""), System.Drawing.Image)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.Location = New System.Drawing.Point(292, 19)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.Name = ""btnBorrar""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.Size = New System.Drawing.Size(86, 71)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.TabIndex = 10")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.Text = ""&Borrar""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.UseVisualStyleBackColor = True")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'btnModificar")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.Cursor = System.Windows.Forms.Cursors.Hand")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.Image = CType(resources.GetObject(""btnModificar.Image""), System.Drawing.Image)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.Location = New System.Drawing.Point(160, 19)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.Name = ""btnModificar""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.Size = New System.Drawing.Size(86, 71)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.TabIndex = 9")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.Text = ""&Modificar""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.UseVisualStyleBackColor = True")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'btnAgregar")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.Cursor = System.Windows.Forms.Cursors.Hand")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.Image = CType(resources.GetObject(""btnAgregar.Image""), System.Drawing.Image)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.Location = New System.Drawing.Point(28, 19)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.Name = ""btnAgregar""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.Size = New System.Drawing.Size(86, 71)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.TabIndex = 8")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.Text = ""&Agregar""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.UseVisualStyleBackColor = True")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'lblconsultar")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.AutoSize = True")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.ForeColor = System.Drawing.Color.Blue")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.Location = New System.Drawing.Point(15, 561)")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.Name = ""lblconsultar""")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.Size = New System.Drawing.Size(100, 26)")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.TabIndex = 567")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.Text = ""Consultar""")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'lblTitulo")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.Font = New System.Drawing.Font(""Times New Roman"", 18.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.ForeColor = System.Drawing.Color.Red")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.Location = New System.Drawing.Point(281, 22)")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.Name = ""lblTitulo""")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.Size = New System.Drawing.Size(456, 30)")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.TabIndex = 566")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'dgv1")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.AllowUserToAddRows = False")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.AllowUserToResizeColumns = False")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.AllowUserToResizeRows = False")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle1.BackColor = System.Drawing.Color.LightBlue")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightGoldenrodYellow")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Brown")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.BackgroundColor = System.Drawing.Color.PeachPuff")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.BorderStyle = System.Windows.Forms.BorderStyle.None")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle2.BackColor = System.Drawing.Color.Gold")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle2.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle3.BackColor = System.Drawing.Color.Lavender")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle3.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Beige")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Brown")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.DefaultCellStyle = DataGridViewCellStyle3")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.GridColor = System.Drawing.Color.MediumPurple")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.Location = New System.Drawing.Point(15, 83)")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.Name = ""dgv1""")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.ReadOnly = True")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.ShowCellErrors = False")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.ShowRowErrors = False")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.Size = New System.Drawing.Size(986, 457)")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.StandardTab = True")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.TabIndex = 562")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'lblid_pk")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk.AutoSize = True")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk.BackColor = System.Drawing.Color.Red")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk.Location = New System.Drawing.Point(21, 94)")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk.Name = ""lblid_pk""")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk.Size = New System.Drawing.Size(13, 13)")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk.TabIndex = 565")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk.Text = ""0""")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk.Visible = False")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'frmAbm" & Me.txtTabla.Text)
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)")
    '    PrintLine(FileFrm, TAB(8), "Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font")
    '    PrintLine(FileFrm, TAB(8), "Me.ClientSize = New System.Drawing.Size(1016, 734)")
    '    PrintLine(FileFrm, TAB(8), "Me.Controls.Add(Me.txtBuscar)")
    '    PrintLine(FileFrm, TAB(8), "Me.Controls.Add(Me.GroupBox1)")
    '    PrintLine(FileFrm, TAB(8), "Me.Controls.Add(Me.lblconsultar)")
    '    PrintLine(FileFrm, TAB(8), "Me.Controls.Add(Me.lblTitulo)")
    '    PrintLine(FileFrm, TAB(8), "Me.Controls.Add(Me.lblid_pk)")
    '    PrintLine(FileFrm, TAB(8), "Me.Controls.Add(Me.dgv1)")
    '    PrintLine(FileFrm, TAB(8), "Me.Name = ""frmAbm" & Me.txtTabla.Text & """")
    '    PrintLine(FileFrm, TAB(8), "Me.Text = ""frmAbm" & Me.txtTabla.Text & """")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.ResumeLayout(False)")
    '    PrintLine(FileFrm, TAB(8), "CType(Me.dgv1, System.ComponentModel.ISupportInitialize).EndInit()")
    '    PrintLine(FileFrm, TAB(8), "Me.ResumeLayout(False)")
    '    PrintLine(FileFrm, TAB(8), "Me.PerformLayout()")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(4), "End Sub")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents txtBuscar As System.Windows.Forms.TextBox")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents GroupBox1 As System.Windows.Forms.GroupBox")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents btnAyuda As System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents btnSalir As System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents btnBorrar As System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents btnModificar As System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents btnAgregar As System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents lblconsultar As System.Windows.Forms.Label")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents ttGeneral As System.Windows.Forms.ToolTip")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents lblTitulo As System.Windows.Forms.Label")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents dgv1 As System.Windows.Forms.DataGridView")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents lblid_pk As System.Windows.Forms.Label")
    '    PrintLine(FileFrm, TAB(0), "End Class")
    '    PrintLine(FileFrm, "")
    '    FileClose(FileFrm)

    '    Dim sErr As String = ""
    '    Dim sContents As String
    '    Dim bAns As String

    '    sContents = GetFileContents(Me.txtPath.Text & "\frmAbmGeneral.resx", sErr)
    '    If sErr = "" Then
    '        Debug.WriteLine("File Contents: " & sContents)
    '        'Save to different file
    '        bAns = SaveTextToFile(sContents, Me.txtPath.Text & "\frmAbm" & Me.txtTabla.Text & ".resx", sErr)
    '    End If











    '    '**************************************************
    '    '
    '    'DEFINICION DEL CODIGA PARA FORMULARIOS DE DETALLE
    '    '
    '    '**************************************************

    '    If Mid$(PathFrmdetalle, Len(PathFrm) - 3, 3) <> ".vb" Then
    '        PathFrmdetalle = Me.txtPath.Text & "\frmDetalle" & Me.txtTabla.Text & ".vb"
    '    Else
    '        PathFrmdetalle = Me.txtPath.Text & "\" & PathFrmdetalle
    '    End If

    '    ' Defino variables
    '    Dim FileFrmDetalle As Integer = FreeFile()
    '    Dim regDetalle As regSistemaTabla

    '    ' Abro un PathCls de texto (si el mismo existía se reempleza)
    '    FileOpen(FileFrmDetalle, PathFrmdetalle, OpenMode.Output)


    '    'saca el nombre del primer control para el focus
    '    Contador = 1
    '    Dim strFocus As String = ""
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.indice = 9 Or regDetalle.indice = 1 Then
    '            'SALTA LA PK
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)
    '                    strFocus = "Me.cmb" & Nombre
    '                    Exit For
    '                Case "String", "Decimal"
    '                    strFocus = "Me.txt" & regDetalle.nombre
    '                    Exit For
    '                Case "DateTime"
    '                    strFocus = "Me.dtp" & regDetalle.nombre
    '                    Exit For
    '                Case "Boolean"
    '                    strFocus = "Me.chk" & regDetalle.nombre
    '                    Exit For
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next

    '    'saca el nombre del primer txt para los vacios
    '    Contador = 1
    '    Dim strVacio As String = ""
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.indice = 9 Or regDetalle.indice = 1 Then
    '            'SALTA LA PK
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "String", "Decimal"
    '                    strVacio = "Me.txt" & regDetalle.nombre
    '                    Exit For
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next

    '    'Private Sub frmDetalle_Load
    '    PrintLine(FileFrmDetalle, TAB(0), "Public Class frmDetalle" & Me.txtTabla.Text)
    '    PrintLine(FileFrmDetalle, "")
    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub frmDetalle" & Me.txtTabla.Text & "_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load")
    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.indice = 9 Or regDetalle.indice = 1 Then
    '            'SALTA LA PK
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)
    '                    PrintLine(FileFrmDetalle, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnBusca" & Nombre & ", " & """" & "Buscar Nuevo " & Nombre & """" & ")")
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileFrmDetalle, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnGuardar, " & """" & "Guardar Datos del " & Me.txtTabla.Text & """" & ")")
    '    PrintLine(FileFrmDetalle, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnSalir, " & """" & "Volver a la Pantalla Anterior" & """" & ")")
    '    PrintLine(FileFrmDetalle, TAB(8), "Try")
    '    PrintLine(FileFrmDetalle, TAB(12), "ObtenerDatos()")
    '    PrintLine(FileFrmDetalle, "")
    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.indice = 9 Or regDetalle.indice = 1 Then
    '            'SALTA LA PK
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)
    '                    ' PrintLine(FileFrmDetalle, TAB(8), "o" & Nombre & ".Modificar(Me.lblid_" & Nombre & ".Text)")
    '                    ' PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Nombre & ".Text = o" & Nombre & ".nombre_" & Nombre)
    '                    PrintLine(FileFrmDetalle, TAB(12), "Me.cmb" & Nombre & ".Text = o" & Nombre & ".GetOne(Me.lblid_" & Nombre & ".Text).Rows(0).Item(" & """" & "nombre_" & Nombre & """" & ").ToString.Trim")
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileFrmDetalle, TAB(8), "Catch ex As Exception")
    '    PrintLine(FileFrmDetalle, TAB(12), "'    MessageBox.Show(ex.Message)")
    '    PrintLine(FileFrmDetalle, TAB(8), "End Try")
    '    PrintLine(FileFrmDetalle, "")
    '    PrintLine(FileFrmDetalle, TAB(8), "If Bandera" & Me.txtTabla.Text & "  = 1 Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "Me.LimpiarControles()")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrm, TAB(8), "Me.Text = " & """" & Me.txtTabla.Text & """")
    '    PrintLine(FileFrm, TAB(8), "Me.CancelButton = Me.btnSalir")
    '    PrintLine(FileFrm, TAB(8), "Me.BackColor = Color.Teal")
    '    PrintLine(FileFrm, TAB(8), "Me.StartPosition = FormStartPosition.CenterScreen")
    '    PrintLine(FileFrm, TAB(8), "Me.MinimizeBox = False")
    '    PrintLine(FileFrm, TAB(8), "Me.MaximizeBox = False")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Sub CargarCombos()

    '    PrintLine(FileFrmDetalle, TAB(4), "Sub CargarCombos()")
    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.indice = 9 Or regDetalle.indice = 1 Then
    '            'SALTA LA PK
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)

    '                    PrintLine(FileFrmDetalle, TAB(8), "Me.Cargar" & Nombre)
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")
    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.indice = 9 Or regDetalle.indice = 1 Then
    '            'SALTA LA PK
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)

    '                    PrintLine(FileFrmDetalle, TAB(4), "Sub Cargar" & Nombre & "()")
    '                    PrintLine(FileFrmDetalle, TAB(8), "Dim odt As New DataTable")
    '                    PrintLine(FileFrmDetalle, "")
    '                    PrintLine(FileFrmDetalle, TAB(8), "odt = o" & Nombre & ".GetCmb")
    '                    PrintLine(FileFrmDetalle, TAB(8), "With Me.cmb" & Nombre)
    '                    PrintLine(FileFrmDetalle, TAB(12), ".DataSource = odt")
    '                    PrintLine(FileFrmDetalle, TAB(12), ".DisplayMember = " & """" & "nombre_" & Nombre & """")
    '                    PrintLine(FileFrmDetalle, TAB(12), ".ValueMember = " & """" & "id_" & Nombre & """")
    '                    PrintLine(FileFrmDetalle, TAB(8), "End With")
    '                    PrintLine(FileFrmDetalle, TAB(8), "If Me.cmb" & Nombre & ".SelectedIndex.ToString >= 0 Then")
    '                    PrintLine(FileFrmDetalle, TAB(12), "cmb" & Nombre & ".SelectedIndex = 0")
    '                    PrintLine(FileFrmDetalle, TAB(12), "Me.lblid_" & Nombre & ".Text = cmb" & Nombre & ".SelectedValue.ToString")
    '                    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '                    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '                    PrintLine(FileFrmDetalle, "")
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next

    '    'Sub LimpiarControles()

    '    PrintLine(FileFrmDetalle, TAB(4), "Sub LimpiarControles()")
    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.indice = 9 Or regDetalle.indice = 1 Then
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)
    '                    PrintLine(FileInsert, TAB(8), "Me.lbl" & regDetalle.nombre & ".Text = " & """" & "0" & """")
    '                    PrintLine(FileInsert, TAB(8), "Me.cmb" & Nombre & ".Text =  """"")
    '                Case "String", "Decimal"
    '                    PrintLine(FileInsert, TAB(8), "Me.txt" & regDetalle.nombre & ".Text = """"")
    '                Case "DateTime"
    '                    PrintLine(FileInsert, TAB(8), "Me.dtp" & regDetalle.nombre & ".Value = DateTime.Now")
    '                Case "Boolean"
    '                    PrintLine(FileInsert, TAB(8), "Me.chk" & regDetalle.nombre & ".Checked = False")
    '                Case Else
    '                    PrintLine(FileInsert, TAB(8), "Me." & regDetalle.nombre & ".Text = """"")
    '            End Select

    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private Sub ObtenerDatos()

    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub ObtenerDatos()")
    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.indice = 9 Or regDetalle.indice = 1 Then
    '            'SALTA LA PK
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    PrintLine(FileInsert, TAB(8), "Me.lbl" & regDetalle.nombre & ".Text = o" & Me.txtTabla.Text & "." & regDetalle.nombre)
    '                Case "String"
    '                    PrintLine(FileInsert, TAB(8), "Me.txt" & regDetalle.nombre & ".Text = o" & Me.txtTabla.Text & "." & regDetalle.nombre & ".Trim")
    '                Case "Decimal"
    '                    PrintLine(FileInsert, TAB(8), "Me.txt" & regDetalle.nombre & ".Text = o" & Me.txtTabla.Text & "." & regDetalle.nombre & ".ToString.Trim")
    '                Case "DateTime"
    '                    PrintLine(FileInsert, TAB(8), "Me.dtp" & regDetalle.nombre & ".Value = o" & Me.txtTabla.Text & "." & regDetalle.nombre)
    '                Case "Boolean"
    '                    PrintLine(FileInsert, TAB(8), "Me.chk" & regDetalle.nombre & ".Checked = o" & Me.txtTabla.Text & "." & regDetalle.nombre)
    '                Case Else
    '                    PrintLine(FileInsert, TAB(8), "Me." & regDetalle.nombre & ".Text = Trim(o" & Me.txtTabla.Text & "." & regDetalle.nombre & ")")
    '            End Select

    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private Sub AsignarDatos()

    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub AsignarDatos()")
    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.indice = 9 Or regDetalle.indice = 1 Then
    '            'SALTA LA PK
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    PrintLine(FileInsert, TAB(8), "o" & Me.txtTabla.Text & "." & regDetalle.nombre & " = Me.lbl" & regDetalle.nombre & ".Text")
    '                Case "String", "Decimal"
    '                    PrintLine(FileInsert, TAB(8), "o" & Me.txtTabla.Text & "." & regDetalle.nombre & " = Me.txt" & regDetalle.nombre & ".Text")
    '                Case "DateTime"
    '                    PrintLine(FileInsert, TAB(8), "o" & Me.txtTabla.Text & "." & regDetalle.nombre & " = Me.dtp" & regDetalle.nombre & ".Value.Date")
    '                Case "Boolean"
    '                    PrintLine(FileInsert, TAB(8), "o" & Me.txtTabla.Text & "." & regDetalle.nombre & " = Me.chk" & regDetalle.nombre & ".Checked")
    '                Case Else
    '                    PrintLine(FileInsert, TAB(8), "o" & Me.txtTabla.Text & "." & regDetalle.nombre & " = Me." & regDetalle.nombre & ".Text")
    '            End Select

    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Public Sub SoloLectura()

    '    PrintLine(FileFrmDetalle, TAB(4), "Public Sub SoloLectura()")
    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.indice = 9 Or regDetalle.indice = 1 Then
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)
    '                    '  PrintLine(FileInsert, TAB(4), "Me.lbl" & regDetalle.nombre & ".Text = " & """" & "0" & """")
    '                    PrintLine(FileInsert, TAB(8), "Me.cmb" & Nombre & ".Enabled = False")
    '                    PrintLine(FileInsert, TAB(8), "Me.btnBusca" & Nombre & ".Enabled = False")
    '                Case "String", "Decimal"
    '                    PrintLine(FileInsert, TAB(8), "Me.txt" & regDetalle.nombre & ".Enabled = False")
    '                Case "DateTime"
    '                    PrintLine(FileInsert, TAB(8), "Me.dtp" & regDetalle.nombre & ".Enabled = False")
    '                Case "Boolean"
    '                    PrintLine(FileInsert, TAB(8), "Me.chk" & regDetalle.nombre & ".Enabled = False")
    '                Case Else
    '                    PrintLine(FileInsert, TAB(8), "Me." & regDetalle.nombre & ".Enabled = False")
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Visible = False")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private Sub Guardar()

    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub Guardar()")
    '    PrintLine(FileFrmDetalle, TAB(8), "Try")
    '    PrintLine(FileFrmDetalle, TAB(12), "Me.AsignarDatos()")

    '    PrintLine(FileFrmDetalle, TAB(12), "If o" & Me.txtTabla.Text & ".Exist() Then")
    '    PrintLine(FileFrmDetalle, TAB(16), "If Bandera" & Me.txtTabla.Text & " = 1 Then")
    '    PrintLine(FileFrmDetalle, TAB(20), "MessageBox.Show(" & """" & "Sr. Usuario: Los Datos que Pretende Ingresar ya Fueron Cargados en el Sistema" & """" & ")")
    '    PrintLine(FileFrmDetalle, TAB(16), "Exit Sub")
    '    PrintLine(FileFrmDetalle, TAB(16), "ElseIf Bandera" & Me.txtTabla.Text & " = 2 Then")
    '    PrintLine(FileCls, TAB(20), "If MessageBox.Show(" & """" & "Sr. Usuario: Los Datos que Desea Modificar ya Existen ¿Desea Reemplazarlos?" & """" & ", " & """" & "MODIFICAR" & """" & ", _")
    '    PrintLine(FileCls, TAB(24), "MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) _")
    '    PrintLine(FileCls, TAB(24), "= Windows.Forms.DialogResult.No Then")
    '    PrintLine(FileCls, TAB(24), "Exit Sub")
    '    PrintLine(FileCls, TAB(20), "End If")
    '    PrintLine(FileFrmDetalle, TAB(16), "End If")
    '    PrintLine(FileFrmDetalle, TAB(12), "End If")

    '    PrintLine(FileFrmDetalle, TAB(12), "Select Case Bandera" & Me.txtTabla.Text)
    '    PrintLine(FileFrmDetalle, TAB(16), "Case 1 'GUARDA,REFRESCA LA GRILLA E INSERTA UNO NUEVO (AGREGAR)")
    '    PrintLine(FileFrmDetalle, TAB(20), "o" & Me.txtTabla.Text & ".Guardar()")
    '    PrintLine(FileFrmDetalle, TAB(20), "CType(Me.Owner, frmAbm" & Me.txtTabla.Text & ").RefrescarGrilla()")
    '    PrintLine(FileFrmDetalle, TAB(20), "Me.CargarCombos()")
    '    PrintLine(FileFrmDetalle, TAB(20), "o" & Me.txtTabla.Text & ".Insertar()")
    '    PrintLine(FileFrmDetalle, TAB(20), "Me.ObtenerDatos()")
    '    PrintLine(FileFrmDetalle, TAB(20), "Me.LimpiarControles()")
    '    PrintLine(FileFrmDetalle, TAB(20), strFocus & ".Focus")
    '    PrintLine(FileFrmDetalle, TAB(16), "Case 2 'GUARDA Y SALE (MODIFICAR)")
    '    PrintLine(FileFrmDetalle, TAB(20), "o" & Me.txtTabla.Text & ".Guardar()")
    '    PrintLine(FileFrmDetalle, TAB(20), "Me.Close()")
    '    PrintLine(FileFrmDetalle, TAB(12), "End Select")
    '    PrintLine(FileFrmDetalle, TAB(8), "Catch ex As Exception")
    '    PrintLine(FileFrmDetalle, TAB(12), "MessageBox.Show(ex.Message)")
    '    PrintLine(FileFrmDetalle, TAB(8), "End Try")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private Function ChequearVacios()

    '    PrintLine(FileFrmDetalle, TAB(4), "Private Function ChequearVacios() As Boolean")
    '    PrintLine(FileFrmDetalle, TAB(8), "Dim bandera As Boolean")
    '    PrintLine(FileFrmDetalle, TAB(8), "If " & strVacio & ".Text = """"" & " Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "bandera = False")
    '    PrintLine(FileFrmDetalle, TAB(8), "Else")
    '    PrintLine(FileFrmDetalle, TAB(12), "bandera = True")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrmDetalle, TAB(8), "Return bandera")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Function")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private Sub btnAyuda_Click ANULADO

    '    '    PrintLine(FileFrmDetalle, TAB(0), "Private Sub btnAyuda_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAyuda.Click")
    '    '    PrintLine(FileFrmDetalle, TAB(4), "'      System.Diagnostics.Process.Start(PathAyuda + " & """" & "FrmDetalle" & Me.txtTabla.Text & """" & ".htm)")
    '    '    PrintLine(FileFrmDetalle, TAB(0), "End Sub")
    '    '    PrintLine(FileFrmDetalle, "")

    '    'Private Sub btnGuardar_Click

    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click")
    '    PrintLine(FileFrmDetalle, TAB(8), "Dim blnVacios As Boolean")
    '    PrintLine(FileFrmDetalle, "")


    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.indice = 9 Or regDetalle.indice = 1 Then
    '            'SALTA LA PK
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    PrintLine(FileFrmDetalle, TAB(8), "If Me.lbl" & regDetalle.nombre & ".Text = 0 Then")
    '                    'si no tiene 'id_' salta el error
    '                    Try
    '                        PrintLine(FileFrmDetalle, TAB(12), "MessageBox.Show(" & """" & "Debe Seleccionar un Dato del Combo de " & regDetalle.nombre.Substring(3) & """" & ")")
    '                    Catch ex As Exception
    '                    End Try
    '                    PrintLine(FileFrmDetalle, TAB(12), "Exit Sub")
    '                    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next

    '    PrintLine(FileFrmDetalle, "")
    '    PrintLine(FileFrmDetalle, TAB(8), "blnVacios = Me.ChequearVacios")
    '    PrintLine(FileFrmDetalle, TAB(8), "If blnVacios = False Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "MessageBox.Show(" & """" & "Debe Llenar los Campos Obligatorios" & """" & ")")
    '    PrintLine(FileFrmDetalle, TAB(12), "Exit Sub")
    '    PrintLine(FileFrmDetalle, TAB(8), "Else")
    '    PrintLine(FileFrmDetalle, TAB(12), "If Bandera" & Me.txtTabla.Text & "  = 1 Then")
    '    PrintLine(FileFrmDetalle, TAB(16), "Me.Guardar()")
    '    PrintLine(FileFrmDetalle, TAB(12), "Else")
    '    PrintLine(FileFrmDetalle, TAB(16), "Me.Guardar()")
    '    PrintLine(FileFrmDetalle, TAB(12), "End If")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private Sub btnSalir_Click

    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click")
    '    PrintLine(FileFrmDetalle, TAB(8), "Me.Close()")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private Sub cmb

    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.indice = 9 Or regDetalle.indice = 1 Then
    '            'SALTA LA PK
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)

    '                    PrintLine(FileFrmDetalle, TAB(4), "Private Sub cmb" & Nombre & "_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmb" & Nombre & ".SelectedIndexChanged")
    '                    PrintLine(FileFrmDetalle, TAB(8), "If Me.cmb" & Nombre & ".SelectedIndex >= 0 Then")
    '                    PrintLine(FileFrmDetalle, TAB(12), "Me.lbl" & regDetalle.nombre & ".Text = cmb" & Nombre & ".SelectedValue.ToString")
    '                    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '                    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '                    PrintLine(FileFrmDetalle, "")

    '                    PrintLine(FileFrmDetalle, TAB(4), "Private Sub btnBusca" & Nombre & "_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBusca" & Nombre & ".Click")
    '                    PrintLine(FileFrmDetalle, TAB(8), "Dim frmTemporal As New FrmAbm" & Nombre)
    '                    PrintLine(FileFrmDetalle, TAB(8), "frmTemporal.ShowDialog()")
    '                    PrintLine(FileFrmDetalle, TAB(8), "Me.Cargar" & Nombre & "()")
    '                    PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Nombre & ".Focus()")
    '                    PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Nombre & ".Text = """"")
    '                    PrintLine(FileFrmDetalle, TAB(8), "Me.lblid_" & Nombre & ".Text = " & """" & "0" & """")
    '                    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '                    PrintLine(FileFrmDetalle, "")
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next

    '    'arma las cadenas de tabulacion
    '    Contador = 1
    '    Dim TabPress As String = ""
    '    Dim TabDown As String = ""
    '    Dim TabDecimal As String = ""

    '    For Each regDetalle In arrEstructura
    '        If regDetalle.indice = 9 Or regDetalle.indice = 1 Then
    '            'SALTA LA PK
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)
    '                    '  PrintLine(FileInsert, TAB(4), "Me.lbl" & regDetalle.nombre & ".Text = " & """" & "0" & """")
    '                    TabDown = TabDown & "cmb" & Nombre & ".KeyDown, "
    '                Case "String"
    '                    TabPress = TabPress & "txt" & regDetalle.nombre & ".KeyPress, "
    '                Case "Decimal"
    '                    TabDecimal = TabDecimal & "txt" & regDetalle.nombre & ".KeyPress, "
    '                Case "DateTime"
    '                    TabDown = TabDown & "dtp" & regDetalle.nombre & ".KeyDown, "
    '                Case "Boolean"
    '                    TabPress = TabPress & "chk" & regDetalle.nombre & ".KeyPress, "
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    Dim LargoTab As Integer = TabPress.Length
    '    If LargoTab >= 2 Then
    '        TabPress = TabPress.Substring(0, LargoTab - 2)
    '    End If
    '    LargoTab = TabDown.Length
    '    If LargoTab >= 2 Then
    '        TabDown = TabDown.Substring(0, LargoTab - 2)
    '    End If
    '    LargoTab = TabDecimal.Length
    '    If LargoTab >= 2 Then
    '        TabDecimal = TabDecimal.Substring(0, LargoTab - 2)
    '    End If

    '    'Private Sub TabulacionTextBox

    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub TabulacionTextBox(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _") 'Handles " & TabPress)
    '    If TabPress = "" Then
    '        PrintLine(FileFrmDetalle, TAB(4), "'Handles " & TabPress)
    '    Else
    '        PrintLine(FileFrmDetalle, TAB(4), "Handles " & TabPress)
    '    End If
    '    PrintLine(FileFrmDetalle, TAB(8), "If e.KeyChar.ToString = vbCr Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "Me.SelectNextControl(Me.ActiveControl, True, True, True, True)")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private Sub TabulacionCombos

    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub TabulacionCombos(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) _") 'Handles " & TabDown)
    '    If TabDown = "" Then
    '        PrintLine(FileFrmDetalle, TAB(4), "'Handles " & TabDown)
    '    Else
    '        PrintLine(FileFrmDetalle, TAB(4), "Handles " & TabDown)
    '    End If
    '    PrintLine(FileFrmDetalle, TAB(8), "If e.KeyValue.ToString = 13 Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "Me.SelectNextControl(Me.ActiveControl, True, True, True, True)")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private sub tabulacion decimales
    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub Decimales(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _")
    '    If TabDecimal = "" Then
    '        PrintLine(FileFrmDetalle, TAB(4), "'Handles " & TabDecimal)
    '    Else
    '        PrintLine(FileFrmDetalle, TAB(4), "Handles " & TabDecimal)
    '    End If
    '    PrintLine(FileFrmDetalle, TAB(8), "Dim txtTemp As TextBox")
    '    PrintLine(FileFrmDetalle, TAB(8), "txtTemp = sender")
    '    PrintLine(FileFrmDetalle, "")
    '    PrintLine(FileFrmDetalle, TAB(8), "If e.KeyChar.ToString = vbCr Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "Me.SelectNextControl(Me.ActiveControl, True, True, True, True)")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrmDetalle, TAB(8), "If e.KeyChar.ToString = " & """" & "." & """" & " Or e.KeyChar.ToString = " & """" & "," & """" & " Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "If InStr(txtTemp.Text, " & """" & "," & """" & ") <> 0 Then")
    '    PrintLine(FileFrmDetalle, TAB(16), "e.Handled = True")
    '    PrintLine(FileFrmDetalle, TAB(12), "Else")
    '    PrintLine(FileFrmDetalle, TAB(16), "e.KeyChar = " & """" & "," & """")
    '    PrintLine(FileFrmDetalle, TAB(12), "End If")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrmDetalle, "")
    '    PrintLine(FileFrmDetalle, TAB(8), "Dim Largo As Integer = InStr(txtTemp.Text, " & """" & "," & """" & ")")
    '    PrintLine(FileFrmDetalle, "")
    '    PrintLine(FileFrmDetalle, TAB(8), "If txtTemp.Text.Length > Largo + 2 And Largo <> 0 And e.KeyChar.ToString <> vbBack Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "e.Handled = True")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrmDetalle, "")
    '    PrintLine(FileFrmDetalle, TAB(8), "If (e.KeyChar.ToString >= " & """" & "0" & """" & " And e.KeyChar.ToString <= " & """" & "9" & """" & ") Or e.KeyChar.ToString = " & """" & "," & """" & " Or e.KeyChar = vbBack Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "'  e.Handled = False")
    '    PrintLine(FileFrmDetalle, TAB(8), "Else")
    '    PrintLine(FileFrmDetalle, TAB(12), "e.Handled = True")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")
    '    PrintLine(FileFrmDetalle, TAB(0), "End Class")

    '    FileClose(FileFrmDetalle)

    '    '***********************************************************************
    '    '
    '    'DEFINICION DEL CODIGA PARA FORMULARIOS DE DETALLE PARTE 2 CON UN TEXBOX
    '    '
    '    '***********************************************************************
    '    If Me.chk_1.Checked = True Then

    '        sContents = GetFileContents(Me.txtPath.Text & "\frmDetalleModelo1.resx", sErr)
    '        If sErr = "" Then
    '            Debug.WriteLine("File Contents: " & sContents)
    '            'Save to different file
    '            bAns = SaveTextToFile(sContents, Me.txtPath.Text & "\frmDetalle" & Me.txtTabla.Text & ".resx", sErr)
    '        End If

    '        If Mid$(PathFrmdetalle, Len(PathFrm) - 3, 3) <> ".vb" Then
    '            PathFrmdetalle = Me.txtPath.Text & "\frmDetalle" & Me.txtTabla.Text & ".Designer.vb"
    '        Else
    '            PathFrmdetalle = Me.txtPath.Text & "\" & PathFrmdetalle
    '        End If

    '        FileFrmDetalle = FreeFile()

    '        ' Abro un PathCls de texto (si el mismo existía se reempleza)
    '        FileOpen(FileFrmDetalle, PathFrmdetalle, OpenMode.Output)
    '        PrintLine(FileFrmDetalle, TAB(0), "<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _")
    '        PrintLine(FileFrmDetalle, TAB(0), "Partial Class frmDetalle" & Me.txtTabla.Text)
    '        PrintLine(FileFrmDetalle, TAB(4), "Inherits System.Windows.Forms.Form")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "'Form reemplaza a Dispose para limpiar la lista de componentes.")
    '        PrintLine(FileFrmDetalle, TAB(4), "<System.Diagnostics.DebuggerNonUserCode()> _")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected Overrides Sub Dispose(ByVal disposing As Boolean)")
    '        PrintLine(FileFrmDetalle, TAB(8), "If disposing AndAlso components IsNot Nothing Then")
    '        PrintLine(FileFrmDetalle, TAB(12), "components.Dispose()")
    '        PrintLine(FileFrmDetalle, TAB(8), "End If")
    '        PrintLine(FileFrmDetalle, TAB(8), "MyBase.Dispose(disposing)")
    '        PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "'Requerido por el Diseñador de Windows Forms")
    '        PrintLine(FileFrmDetalle, TAB(4), "Private components As System.ComponentModel.IContainer")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento")
    '        PrintLine(FileFrmDetalle, TAB(4), "'Se puede modificar usando el Diseñador de Windows Forms.")
    '        PrintLine(FileFrmDetalle, TAB(4), "'No lo modifique con el editor de código.")
    '        PrintLine(FileFrmDetalle, TAB(4), "<System.Diagnostics.DebuggerStepThrough()> _")
    '        PrintLine(FileFrmDetalle, TAB(4), "Private Sub InitializeComponent()")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.components = New System.ComponentModel.Container")
    '        PrintLine(FileFrmDetalle, TAB(8), "Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetalle" & Me.txtTabla.Text & "))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir = New System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1 = New System.Windows.Forms.GroupBox")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar = New System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & " = New System.Windows.Forms.TextBox")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & " = New System.Windows.Forms.Label")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.ttGeneral = New System.Windows.Forms.ToolTip(Me.components)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.SuspendLayout()")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'btnSalir")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.ForeColor = System.Drawing.Color.Black")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Image = CType(resources.GetObject(""btnSalir.Image""), System.Drawing.Image)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Location = New System.Drawing.Point(484, 170)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Name = ""btnSalir""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Size = New System.Drawing.Size(81, 69)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.TabIndex = 21")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Text = ""&Salir""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.UseVisualStyleBackColor = True")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'GroupBox1")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.Location = New System.Drawing.Point(24, 24)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.Name = ""GroupBox1""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.Size = New System.Drawing.Size(541, 101)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.TabIndex = 0")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.TabStop = False")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'btnGuardar")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.ForeColor = System.Drawing.Color.Black")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Image = CType(resources.GetObject(""btnGuardar.Image""), System.Drawing.Image)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Location = New System.Drawing.Point(377, 170)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Name = ""btnGuardar""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Size = New System.Drawing.Size(78, 69)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.TabIndex = 20")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Text = ""&Guardar""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.UseVisualStyleBackColor = True")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'txt" & Me.txtTexto1.Text)
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".BackColor = System.Drawing.Color.White")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".Location = New System.Drawing.Point(159, 67)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".MaxLength = 50")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".Name = ""txt" & Me.txtTexto1.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".Size = New System.Drawing.Size(385, 20)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".TabIndex = 5")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'lbl" & Me.txtLabel1.Text)
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".ForeColor = System.Drawing.Color.Black")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".Location = New System.Drawing.Point(43, 66)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".Name = ""lbl" & Me.txtLabel1.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".Size = New System.Drawing.Size(110, 21)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".TabIndex = 577")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".Text = ""* " & Me.txtLabel1.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'frmDetalle" & Me.txtTabla.Text)
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.CancelButton = Me.btnSalir")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.ClientSize = New System.Drawing.Size(592, 266)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.btnSalir)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.btnGuardar)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.txt" & Me.txtTexto1.Text & ")")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.lbl" & Me.txtLabel1.Text & ")")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.GroupBox1)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Name = ""frmDetalle" & Me.txtTabla.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Text = ""frmDetalle" & Me.txtTabla.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.ResumeLayout(False)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.PerformLayout()")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents btnSalir As System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents GroupBox1 As System.Windows.Forms.GroupBox")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents btnGuardar As System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents txt" & Me.txtTexto1.Text & " As System.Windows.Forms.TextBox")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents lbl" & Me.txtLabel1.Text & " As System.Windows.Forms.Label")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents ttGeneral As System.Windows.Forms.ToolTip")
    '        PrintLine(FileFrmDetalle, TAB(0), "End Class")
    '        PrintLine(FileFrmDetalle, "")

    '        FileClose(FileFrmDetalle)
    '    End If



    '    '**********************************************************************************
    '    '
    '    'DEFINICION DEL CODIGA PARA FORMULARIOS DE DETALLE PARTE 2 CON UN TEXBOX Y UN COMBO
    '    '
    '    '**********************************************************************************
    '    If Me.chk_2.Checked = True Then
    '        sContents = GetFileContents(Me.txtPath.Text & "\frmDetalleModelo2.resx", sErr)
    '        If sErr = "" Then
    '            Debug.WriteLine("File Contents: " & sContents)
    '            'Save to different file
    '            bAns = SaveTextToFile(sContents, Me.txtPath.Text & "\frmDetalle" & Me.txtTabla.Text & ".resx", sErr)
    '        End If

    '        If Mid$(PathFrmdetalle, Len(PathFrm) - 3, 3) <> ".vb" Then
    '            PathFrmdetalle = Me.txtPath.Text & "\frmDetalle" & Me.txtTabla.Text & ".Designer.vb"
    '        Else
    '            PathFrmdetalle = Me.txtPath.Text & "\" & PathFrmdetalle
    '        End If

    '        FileFrmDetalle = FreeFile()

    '        ' Abro un PathCls de texto (si el mismo existía se reempleza)
    '        FileOpen(FileFrmDetalle, PathFrmdetalle, OpenMode.Output)

    '        PrintLine(FileFrmDetalle, TAB(0), "<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _")
    '        PrintLine(FileFrmDetalle, TAB(0), "Partial Class frmDetalle" & Me.txtTabla.Text)
    '        PrintLine(FileFrmDetalle, TAB(4), "Inherits System.Windows.Forms.Form")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "'Form reemplaza a Dispose para limpiar la lista de componentes.")
    '        PrintLine(FileFrmDetalle, TAB(4), "<System.Diagnostics.DebuggerNonUserCode()> _")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected Overrides Sub Dispose(ByVal disposing As Boolean)")
    '        PrintLine(FileFrmDetalle, TAB(8), "If disposing AndAlso components IsNot Nothing Then")
    '        PrintLine(FileFrmDetalle, TAB(12), "components.Dispose()")
    '        PrintLine(FileFrmDetalle, TAB(8), "End If")
    '        PrintLine(FileFrmDetalle, TAB(8), "MyBase.Dispose(disposing)")
    '        PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "'Requerido por el Diseñador de Windows Forms")
    '        PrintLine(FileFrmDetalle, TAB(4), "Private components As System.ComponentModel.IContainer")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento")
    '        PrintLine(FileFrmDetalle, TAB(4), "'Se puede modificar usando el Diseñador de Windows Forms.")
    '        PrintLine(FileFrmDetalle, TAB(4), "'No lo modifique con el editor de código.")
    '        PrintLine(FileFrmDetalle, TAB(4), "<System.Diagnostics.DebuggerStepThrough()> _")
    '        PrintLine(FileFrmDetalle, TAB(4), "Private Sub InitializeComponent()")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.components = New System.ComponentModel.Container")
    '        PrintLine(FileFrmDetalle, TAB(8), "Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetalle" & Me.txtTabla.Text & "))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir = New System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar = New System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & " = New System.Windows.Forms.Label")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & " = New System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Me.txtCombo1.Text.Substring(3) & " = New System.Windows.Forms.ComboBox")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2 = New System.Windows.Forms.Label")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.ttGeneral = New System.Windows.Forms.ToolTip(Me.components)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & " = New System.Windows.Forms.TextBox")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1 = New System.Windows.Forms.Label")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1 = New System.Windows.Forms.GroupBox")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.SuspendLayout()")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'btnSalir")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.ForeColor = System.Drawing.Color.Black")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Image = CType(Resources.GetObject(""btnSalir.Image""), System.Drawing.Image)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Location = New System.Drawing.Point(486, 175)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Name = ""btnSalir""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Size = New System.Drawing.Size(81, 69)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.TabIndex = 21")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Text = ""&Salir""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.UseVisualStyleBackColor = True")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'btnGuardar")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.ForeColor = System.Drawing.Color.Black")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Image = CType(Resources.GetObject(""btnGuardar.Image""), System.Drawing.Image)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Location = New System.Drawing.Point(379, 175)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Name = ""btnGuardar""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Size = New System.Drawing.Size(78, 69)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.TabIndex = 20")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Text = ""&Guardar""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.UseVisualStyleBackColor = True")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'lbl" & Me.txtCombo1.Text)
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & ".AutoSize = True")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & ".BackColor = System.Drawing.Color.Red")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & ".Location = New System.Drawing.Point(370, 99)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & ".Name = ""lbl" & Me.txtCombo1.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & ".Size = New System.Drawing.Size(13, 13)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & ".TabIndex = 586")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & ".Text = ""0""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & ".Visible = False")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'btnBusca" & Me.txtCombo1.Text.Substring(3))
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & ".Cursor = System.Windows.Forms.Cursors.Hand")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & ".Image = CType(Resources.GetObject(""btnBusca1.Image""), System.Drawing.Image)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & ".Location = New System.Drawing.Point(505, 94)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & ".Name = ""btnBusca" & Me.txtCombo1.Text.Substring(3) & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & ".Size = New System.Drawing.Size(41, 36)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & ".TabIndex = 30")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & ".UseVisualStyleBackColor = True")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'cmb" & Me.txtCombo1.Text.Substring(3))
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Me.txtCombo1.Text.Substring(3) & ".AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Me.txtCombo1.Text.Substring(3) & ".AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Me.txtCombo1.Text.Substring(3) & ".FormattingEnabled = True")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Me.txtCombo1.Text.Substring(3) & ".Location = New System.Drawing.Point(161, 96)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Me.txtCombo1.Text.Substring(3) & ".Name = ""cmb" & Me.txtCombo1.Text.Substring(3) & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Me.txtCombo1.Text.Substring(3) & ".Size = New System.Drawing.Size(329, 21)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Me.txtCombo1.Text.Substring(3) & ".TabIndex = 6")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'lblEtiqueta2")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.ForeColor = System.Drawing.Color.Black")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.Location = New System.Drawing.Point(28, 94)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.Name = ""lblEtiqueta2""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.Size = New System.Drawing.Size(127, 21)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.TabIndex = 584")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.Text = """ & Me.txtLabel2.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'txt" & Me.txtTexto1.Text)
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".BackColor = System.Drawing.Color.White")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".Location = New System.Drawing.Point(161, 50)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".MaxLength = 50")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".Name = ""txt" & Me.txtTexto1.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".Size = New System.Drawing.Size(385, 20)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".TabIndex = 5")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'lbllblEtiqueta1")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.ForeColor = System.Drawing.Color.Black")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.Location = New System.Drawing.Point(28, 49)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.Name = ""lblEtiqueta1""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.Size = New System.Drawing.Size(127, 21)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.TabIndex = 588")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.Text = "" * " & Me.txtLabel1.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'GroupBox1")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.Location = New System.Drawing.Point(12, 17)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.Name = ""GroupBox1""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.Size = New System.Drawing.Size(568, 135)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.TabIndex = 0")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.TabStop = False")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'frmDetalle" & Me.txtTabla.Text)
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.ClientSize = New System.Drawing.Size(592, 266)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.txt" & Me.txtTexto1.Text & ")")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.lblEtiqueta1)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.lbl" & Me.txtCombo1.Text & ")")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & ")")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.cmb" & Me.txtCombo1.Text.Substring(3) & ")")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.lblEtiqueta2)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.btnSalir)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.btnGuardar)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.GroupBox1)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Name = ""frmDetalle" & Me.txtTabla.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Text = ""frmDetalle" & Me.txtTabla.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.ResumeLayout(False)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.PerformLayout()")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents btnSalir As System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents btnGuardar As System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(4), "Friend WithEvents lbl" & Me.txtCombo1.Text & " As System.Windows.Forms.Label")
    '        PrintLine(FileFrmDetalle, TAB(4), "Friend WithEvents btnBusca" & Me.txtCombo1.Text.Substring(3) & " As System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(4), "Friend WithEvents cmb" & Me.txtCombo1.Text.Substring(3) & " As System.Windows.Forms.ComboBox")
    '        PrintLine(FileFrmDetalle, TAB(4), "Friend WithEvents lblEtiqueta2 As System.Windows.Forms.Label")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents ttGeneral As System.Windows.Forms.ToolTip")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents txt" & Me.txtTexto1.Text & " As System.Windows.Forms.TextBox")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents lblEtiqueta1 As System.Windows.Forms.Label")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents GroupBox1 As System.Windows.Forms.GroupBox")
    '        PrintLine(FileFrmDetalle, TAB(0), "End Class")
    '        PrintLine(FileFrmDetalle, "")

    '        FileClose(FileFrmDetalle)

    '    End If

    'End Sub

    Private Function CountLines(ByVal Stream As System.IO.StreamReader) As Integer
        Dim count As Integer = 0
        Dim line As String
        line = Stream.ReadLine()

        While line IsNot Nothing
            If line = "GO" Then
                count = count + 1
            Else
            End If
            line = Stream.ReadLine()
        End While

        Return count
    End Function

    Sub ParseStreamAndActionScript(ByVal stream As System.IO.StreamReader)
        '			// Parses the stream looking for GO markers which 
        '			// delimit blocks of SQL.
        Dim line As String
        Dim currentBlock As StringBuilder = New StringBuilder()
        Dim overallLine As Integer = 0
        Dim blockLine As Integer = 0
        line = stream.ReadLine()
        Dim LineaProgress As Integer = 1

        While line IsNot Nothing
            overallLine = overallLine + 1
            blockLine = blockLine + 1
            If line = "GO" Then
                '		GO marker found, Action the script block
                '		up to the GO marker
                ActionScript(currentBlock.ToString())

                '		Reset the script block
                currentBlock = New StringBuilder()
                blockLine = 0

                Me.ProgressBar1.Value = LineaProgress
                LineaProgress = LineaProgress + 1

            Else
                Console.WriteLine("{0}:{1}: {2}", overallLine, blockLine, line)
                currentBlock.Append(line)
                currentBlock.Append(Environment.NewLine)
            End If
            line = stream.ReadLine()
        End While

        '  ActionScript(currentBlock.ToString())
    End Sub

    Dim NumeroLinea As Integer = 0

    Sub ActionScript(ByVal script As String)
        Dim resultSet As DataSet = New DataSet()
        Try


            Dim connection As SqlConnection = New SqlConnection(txtCadena.Text)
            Dim Command As SqlCommand = New SqlCommand()
            Command.Connection = connection
            Command.CommandText = script
            Command.CommandType = CommandType.Text
            connection.Open()
            Command.ExecuteNonQuery()
            connection.Close()

        Catch ex As SqlException
            BanderaSP = 1
            MessageBox.Show("Error Nº: " & ex.Number & " En el SP: " & ex.Procedure & " Linea Nº: " & ex.LineNumber, "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Try
        End Try
    End Sub

    'Public Sub GenerarTodasLasClases(ByVal cadena As String)

    'End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
        Application.Exit()
    End Sub

    Private Sub btncop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSP.Click
        Dim PathArchivo As String = Me.txtPath.Text & "\cop_" & Me.txtTabla.Text & ".sql"
        Dim fi As FileInfo = New FileInfo(PathArchivo)
        Dim File As StreamReader = fi.OpenText()
        Dim File2 As StreamReader = fi.OpenText()
        BanderaSP = 0

        NumeroLinea = Me.CountLines(File2)
        Me.ProgressBar1.Value = Me.ProgressBar1.Minimum
        Me.ProgressBar1.Maximum = NumeroLinea


        ParseStreamAndActionScript(File)
        If BanderaSP = 0 Then
            MessageBox.Show("Los Comandos se Ejecutaron Correctamente", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            MessageBox.Show("Se ha Producido un Error en la Ejecución ", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub


    Private Sub btnServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnServer.Click
        Me.UseWaitCursor = True
        Me.Cursor = Cursors.WaitCursor

        cmbServer.Items.Clear()
        Me.Refresh()
        System.Windows.Forms.Application.DoEvents()
        System.Threading.Thread.Sleep(200)

        Dim DbServer As SqlDataSourceEnumerator = SqlDataSourceEnumerator.Instance
        Dim TServ As DataTable = DbServer.GetDataSources()

        If TServ.Rows.Count = 0 Then
            cmbServer.Text = Environment.MachineName
            Me.btnBaseDato.Enabled = True
        End If

        For Each r As DataRow In TServ.Rows
            If (r("InstanceName").ToString().Length > 0) Then
                cmbServer.Items.Add(r("ServerName").ToString() + "\" + r("InstanceName").ToString())
            Else
                cmbServer.Items.Add(r(0).ToString())
            End If
        Next
        Me.UseWaitCursor = False
        Me.Cursor = Cursors.Default
        Me.cmbServer.Focus()
    End Sub

    Private Sub btnBaseDato_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBaseDato.Click
        Try
            Dim con As String = String.Format(baseCon, cmbServer.Text, "master")
            Dim Connect As New SqlClient.SqlConnection(con)
            Connect.Open()
            Dim tbdb As DataTable = Connect.GetSchema("Databases")
            Connect.Close()
            lstBaseDato.DisplayMember = tbdb.Columns(0).ColumnName
            lstBaseDato.ValueMember = lstBaseDato.DisplayMember
            lstBaseDato.DataSource = tbdb.DefaultView
            btnTabla.Enabled = True
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub lstBaseDato_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lstBaseDato.KeyPress
        If e.KeyChar.ToString = vbCr Then
            Me.btnTabla.Focus()
        End If
    End Sub

    Private Sub lstBaseDato_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstBaseDato.SelectedIndexChanged
        txtCadenaConexion.Text = String.Format(baseCon, cmbServer.Text, lstBaseDato.SelectedValue.ToString())
        Me.txtCadena.Text = txtCadenaConexion.Text
        Me.txtPath.Text = "V:\VBNet\" & lstBaseDato.SelectedValue.ToString() & "\trunk\cop\"
        '  Me.txtPath_2.Text = "V:\VBNet\" & lstBaseDato.SelectedValue.ToString() & "\trunk\" & lstBaseDato.SelectedValue.ToString() & "\Entidad\"


        Try
            Dim cn As New SqlConnection(Me.txtCadenaConexion.Text)
            Dim da As New SqlDataAdapter("SELECT TOP 100 PERCENT name, id FROM dbo.sysobjects WHERE (type = 'U') AND name <> 'dtproperties' ORDER BY name", cn)

            Dim cb As New SqlCommandBuilder(da)
            Dim cbSistema As New SqlCommandBuilder(da)

            odtTabla.Clear()
            '   If odtTabla.Rows.Count <= 0 Then
            da.Fill(odtTabla)
            '  End If

            Me.lstTabla.Items.Clear()
            For i As Integer = 0 To odtTabla.Rows.Count - 1
                Me.lstTabla.Items.Add(odtTabla.Rows(i).Item("name"))
            Next
            Me.lstTabla.Focus()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub lstTabla_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lstTabla.KeyPress
        If e.KeyChar.ToString = vbCr Then
            Me.btnGenerarClase.Focus()
        End If
    End Sub

    Private Sub lstTabla_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstTabla.SelectedIndexChanged
        Me.txtTabla.Text = Me.lstTabla.SelectedItem
        Me.txtTituloFormulario.Text = Me.txtTabla.Text

        For Each row As DataRow In odtTabla.Rows
            If row.Item("name") = Me.txtTabla.Text Then
                id_tabla = row.Item("id")
                Exit For
            End If
        Next
        Me.GenerarNombres()
        Me.GetColumnas(id_tabla)
    End Sub

    Dim odtTabla As New DataTable
    Dim id_tabla As Integer = 0

    Private Sub btnTabla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTabla.Click
        Dim cn As New SqlConnection(Me.txtCadenaConexion.Text)
        Dim da As New SqlDataAdapter("SELECT TOP 100 PERCENT name, id FROM dbo.sysobjects WHERE (type = 'U') AND name <> 'dtproperties' ORDER BY name", cn)

        Dim cb As New SqlCommandBuilder(da)
        Dim cbSistema As New SqlCommandBuilder(da)

        odtTabla.Clear()
        '   If odtTabla.Rows.Count <= 0 Then
        da.Fill(odtTabla)
        '  End If

        Me.lstTabla.Items.Clear()
        For i As Integer = 0 To odtTabla.Rows.Count - 1
            Me.lstTabla.Items.Add(odtTabla.Rows(i).Item("name"))
        Next
        Me.lstTabla.Focus()
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Finalizo la ejecución
        Me.Close()
        Application.Exit()
    End Sub

    Private Sub btnTodocop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTodoSp.Click
        Dim cn As New SqlConnection(Me.txtCadenaConexion.Text)
        Dim da As New SqlDataAdapter("SELECT TOP 100 PERCENT name FROM dbo.sysobjects WHERE (type = 'U') AND name <> 'dtproperties' ORDER BY name", cn)

        Dim cb As New SqlCommandBuilder(da)
        Dim cbSistema As New SqlCommandBuilder(da)
        Dim dt As New DataTable
        da.Fill(dt)
        ' Solicito confirmación antes de crear todas las tablas
        If MessageBox.Show("Desea Crear los SP Para Todas las Tablas de la Base de Datos Seleccionada ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            Dim Indice As Integer = 0
            For Each row As DataRow In dt.Rows
                txtTabla.Text = row("name")
                Me.GenerarNombres()
                Dim PathArchivo As String = Me.txtPath.Text & "\cop_" & Me.txtTabla.Text & ".sql"
                Dim fi As FileInfo = New FileInfo(PathArchivo)
                Dim File As StreamReader = fi.OpenText()
                Dim File2 As StreamReader = fi.OpenText()
                BanderaSP = 0

                Me.ProgressBar2.Value = Indice

                NumeroLinea = Me.CountLines(File2)
                Me.ProgressBar1.Value = Me.ProgressBar1.Minimum
                Me.ProgressBar1.Maximum = NumeroLinea

                ParseStreamAndActionScript(File)
                Indice = Indice + 1
            Next
            '' Genero una clase para cada tabla
            'Call GenerarTodasLasClases(txtCadena.Text)
        End If
        If BanderaSP = 0 Then
            MessageBox.Show("Los Comandos se Ejecutaron Correctamente", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            MessageBox.Show("Se ha Producido un Error en la Ejecución ", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub cmbServer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbServer.KeyDown
        If e.KeyValue = 13 Then
            Me.btnBaseDato.Focus()
        End If
    End Sub

    Private Sub cmbServer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbServer.SelectedIndexChanged
        Me.btnBaseDato.Enabled = True
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim frmDetalle As New Form2
        Me.AddOwnedForm(frmDetalle)
        frmDetalle.ShowDialog()

    End Sub

    Private Sub btnNulo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNulo.Click
        ' Verifico si se trata de una o todas las tablas
        If Not chkTodasLasTablas.Checked Then
            ' Solicito confirmación antes de crear la clase
            If MessageBox.Show("Desea Crear el Registro nulo '" & txtClase.Text & "' Para la Tabla '" & txtTabla.Text & "' ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                ' Cargo la estructura de la tabla en un array "modular"
                Call Cargar(txtCadena.Text, txtTabla.Text)

                ' Genero la clase
                Call GenerarClaseInsert(txtArchivo.Text, txtClase.Text)

                ' Indico que se ha generado la clase
                MessageBox.Show("Se Generó la Clase '" & txtClase.Text & "' en el Archivo '" & txtArchivo.Text & "' de la Tabla '" & txtTabla.Text & "'." & Chr(13) & Chr(13) & "El Archivo se Guardó en C:\." & Chr(13) & Chr(13) & "Copiar la Clase en el Proyecto y Agregarla con --> Agregar elemento existente", "GENERACION DE CLASES", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.btnSP.Enabled = True
            End If
        Else
            Dim cn As New SqlConnection(Me.txtCadenaConexion.Text)
            Dim da As New SqlDataAdapter("SELECT TOP 100 PERCENT name FROM dbo.sysobjects WHERE (type = 'U') AND name <> 'dtproperties' ORDER BY name", cn)

            Dim cb As New SqlCommandBuilder(da)
            Dim cbSistema As New SqlCommandBuilder(da)
            Dim dt As New DataTable
            da.Fill(dt)
            ' Solicito confirmación antes de crear todas las tablas
            If MessageBox.Show("Desea Crear los Registros nulos Para Todas las Tablas de la Base de Datos Seleccionada ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Me.ProgressBar1.Value = Me.ProgressBar1.Minimum
                Me.ProgressBar1.Maximum = dt.Rows.Count - 1
                Me.ProgressBar2.Value = Me.ProgressBar2.Minimum
                Me.ProgressBar2.Maximum = dt.Rows.Count - 1

                Dim indice = 0
                For Each row As DataRow In dt.Rows
                    txtTabla.Text = row("name")
                    Me.GenerarNombres()
                    Call Cargar(txtCadena.Text, txtTabla.Text)
                    Call GenerarClaseInsert(txtArchivo.Text, txtClase.Text)
                    Me.ProgressBar1.Value = indice
                    Me.ProgressBar2.Value = indice

                    indice = indice + 1
                Next
                MessageBox.Show("Se Generó la Clase Para Todas las Tablas de la Base de Datos." & Chr(13) & Chr(13) & "Los Archivos se Guardaron en C:\." & Chr(13) & Chr(13) & "Copiar la Clase en el Proyecto y Agregarla con --> Agregar elemento existente", "GENERACION DE CLASES", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.btnTodoSp.Enabled = True
            End If
        End If
        FileClose(FileInsert2)
    End Sub

    Private Sub btnTruncate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTruncate.Click
        ' Verifico si se trata de una o todas las tablas
        If Not chkTodasLasTablas.Checked Then
            ' Solicito confirmación antes de crear la clase
            If MessageBox.Show("Desea Crear el Truncate '" & txtClase.Text & "' Para la Tabla '" & txtTabla.Text & "' ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                ' Cargo la estructura de la tabla en un array "modular"
                Call Cargar(txtCadena.Text, txtTabla.Text)

                ' Genero la clase
                Call generarClaseTruncate(txtArchivo.Text, txtClase.Text)

                ' Indico que se ha generado la clase
                MessageBox.Show("Se Generó el Truncate '" & txtClase.Text & "' en el Archivo '" & "Truncate.sql" & "' de la Tabla '" & txtTabla.Text & "'." & Chr(13) & Chr(13) & "El Archivo se Guardó en C:\." & Chr(13) & Chr(13) & "Ejecutar el Script", "GENERACION DE SCRIPT", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.btnSP.Enabled = True
            End If
        Else
            Dim cn As New SqlConnection(Me.txtCadenaConexion.Text)
            Dim da As New SqlDataAdapter("SELECT TOP 100 PERCENT name FROM dbo.sysobjects WHERE (type = 'U') AND name <> 'dtproperties' ORDER BY name", cn)

            Dim cb As New SqlCommandBuilder(da)
            Dim cbSistema As New SqlCommandBuilder(da)
            Dim dt As New DataTable
            da.Fill(dt)
            ' Solicito confirmación antes de crear todas las tablas
            If MessageBox.Show("Desea Crear los Truncates Para Todas las Tablas de la Base de Datos Seleccionada ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Me.ProgressBar1.Value = Me.ProgressBar1.Minimum
                Me.ProgressBar1.Maximum = dt.Rows.Count - 1
                Me.ProgressBar2.Value = Me.ProgressBar2.Minimum
                Me.ProgressBar2.Maximum = dt.Rows.Count - 1

                Dim indice = 0
                For Each row As DataRow In dt.Rows
                    txtTabla.Text = row("name")
                    Me.GenerarNombres()
                    Call Cargar(txtCadena.Text, txtTabla.Text)
                    Call generarClaseTruncate(txtArchivo.Text, txtClase.Text)
                    Me.ProgressBar1.Value = indice
                    Me.ProgressBar2.Value = indice

                    indice = indice + 1
                Next
                MessageBox.Show("Se Generó el Truncate Para Todas las Tablas de la Base de Datos." & Chr(13) & Chr(13) & "Los Archivos se Guardaron en C:\." & Chr(13) & Chr(13) & "Ejecutar el Script", "GENERACION DE SCRIPT", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.btnTodoSp.Enabled = True
            End If
        End If
        FileClose(FileInsert2)
    End Sub

    Private Sub btnIdSuplente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIdSuplente.Click
        ' Verifico si se trata de una o todas las tablas
        If Not chkTodasLasTablas.Checked Then
            ' Solicito confirmación antes de crear la clase
            If MessageBox.Show("Desea Crear la Clave Suplente '" & txtClase.Text & "' Para la Tabla '" & txtTabla.Text & "' ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                ' Cargo la estructura de la tabla en un array "modular"
                Call Cargar(txtCadena.Text, txtTabla.Text)

                ' Genero la clase
                Call generarClaseKey(txtArchivo.Text, txtClase.Text)

                ' Indico que se ha generado la clase
                MessageBox.Show("Se Generó la Clave Suplente '" & txtClase.Text & "' en el Archivo '" & "Clave_suplente.sql" & "' de la Tabla '" & txtTabla.Text & "'." & Chr(13) & Chr(13) & "El Archivo se Guardó en C:\." & Chr(13) & Chr(13) & "Ejecutar con SQL", "GENERACION DE SCRIPTS", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.btnSP.Enabled = True
            End If
        Else
            Dim cn As New SqlConnection(Me.txtCadenaConexion.Text)
            Dim da As New SqlDataAdapter("SELECT TOP 100 PERCENT name FROM dbo.sysobjects WHERE (type = 'U') AND name <> 'dtproperties' ORDER BY name", cn)

            Dim cb As New SqlCommandBuilder(da)
            Dim cbSistema As New SqlCommandBuilder(da)
            Dim dt As New DataTable
            da.Fill(dt)
            ' Solicito confirmación antes de crear todas las tablas
            If MessageBox.Show("Desea Crear las Claves Suplentes Para Todas las Tablas de la Base de Datos Seleccionada ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Me.ProgressBar1.Value = Me.ProgressBar1.Minimum
                Me.ProgressBar1.Maximum = dt.Rows.Count - 1
                Me.ProgressBar2.Value = Me.ProgressBar2.Minimum
                Me.ProgressBar2.Maximum = dt.Rows.Count - 1

                Dim indice = 0
                For Each row As DataRow In dt.Rows
                    txtTabla.Text = row("name")
                    Me.GenerarNombres()
                    Call Cargar(txtCadena.Text, txtTabla.Text)

                    Call generarClaseKey(txtArchivo.Text, txtClase.Text)
                    Me.ProgressBar1.Value = indice
                    Me.ProgressBar2.Value = indice

                    indice = indice + 1
                Next
                MessageBox.Show("Se Generó El Script Para Todas las Tablas de la Base de Datos." & Chr(13) & Chr(13) & "Los Archivos se Guardaron en C:\." & Chr(13) & Chr(13) & "Copiar la Clase en el Proyecto y Agregarla con --> Agregar elemento existente", "GENERACION DE CLASES", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.btnTodoSp.Enabled = True
            End If
        End If
        FileClose(FileInsert2)
    End Sub

    Private Sub btnTablaPocket_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTablaPocket.Click
        ' Verifico si se trata de una o todas las tablas
        If Not chkTodasLasTablas.Checked Then
            ' Solicito confirmación antes de crear la clase
            If MessageBox.Show("Desea Crear el Truncate '" & txtClase.Text & "' Para la Tabla '" & txtTabla.Text & "' ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                ' Cargo la estructura de la tabla en un array "modular"
                Call Cargar(txtCadena.Text, txtTabla.Text)

                ' Genero la clase
                Call generarTablaPocket(txtArchivo.Text, txtClase.Text)

                ' Indico que se ha generado la clase
                MessageBox.Show("Se Generó la Clase '" & txtClase.Text & "' en el Archivo '" & txtArchivo.Text & "' de la Tabla '" & txtTabla.Text & "'." & Chr(13) & Chr(13) & "El Archivo se Guardó en C:\." & Chr(13) & Chr(13) & "Copiar la Clase en el Proyecto y Agregarla con --> Agregar elemento existente", "GENERACION DE CLASES", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.btnSP.Enabled = True
            End If
        Else
            Dim cn As New SqlConnection(Me.txtCadenaConexion.Text)
            Dim da As New SqlDataAdapter("SELECT TOP 100 PERCENT name FROM dbo.sysobjects WHERE (type = 'U') AND name <> 'dtproperties' ORDER BY name", cn)

            Dim cb As New SqlCommandBuilder(da)
            Dim cbSistema As New SqlCommandBuilder(da)
            Dim dt As New DataTable
            da.Fill(dt)
            ' Solicito confirmación antes de crear todas las tablas
            If MessageBox.Show("Desea Crear los Truncates Para Todas las Tablas de la Base de Datos Seleccionada ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Me.ProgressBar1.Value = Me.ProgressBar1.Minimum
                Me.ProgressBar1.Maximum = dt.Rows.Count - 1
                Me.ProgressBar2.Value = Me.ProgressBar2.Minimum
                Me.ProgressBar2.Maximum = dt.Rows.Count - 1

                Dim indice = 0
                For Each row As DataRow In dt.Rows
                    txtTabla.Text = row("name")
                    Me.GenerarNombres()
                    Call Cargar(txtCadena.Text, txtTabla.Text)

                    Call generarTablaPocket(txtArchivo.Text, txtClase.Text)
                    Me.ProgressBar1.Value = indice
                    Me.ProgressBar2.Value = indice

                    indice = indice + 1
                Next
                MessageBox.Show("Se Generó la Clase Para Todas las Tablas de la Base de Datos." & Chr(13) & Chr(13) & "Los Archivos se Guardaron en C:\." & Chr(13) & Chr(13) & "Copiar la Clase en el Proyecto y Agregarla con --> Agregar elemento existente", "GENERACION DE CLASES", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.btnTodoSp.Enabled = True
            End If
        End If
        FileClose(FileInsert2)
    End Sub

    Private Sub btnSchema_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSchema.Click
        ' Verifico si se trata de una o todas las tablas
        If Not chkTodasLasTablas.Checked Then
            ' Solicito confirmación antes de crear la clase
            If MessageBox.Show("Desea Crear la Clave Suplente '" & txtClase.Text & "' Para la Tabla '" & txtTabla.Text & "' ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                ' Cargo la estructura de la tabla en un array "modular"
                Call Cargar(txtCadena.Text, txtTabla.Text)

                ' Genero la clase
                Call generarSchema(txtArchivo.Text, txtClase.Text)

                ' Indico que se ha generado la clase
                MessageBox.Show("Se Generó la Clave Suplente '" & txtClase.Text & "' en el Archivo '" & "Clave_suplente.sql" & "' de la Tabla '" & txtTabla.Text & "'." & Chr(13) & Chr(13) & "El Archivo se Guardó en C:\." & Chr(13) & Chr(13) & "Ejecutar con SQL", "GENERACION DE SCRIPTS", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.btnSP.Enabled = True
            End If
        Else
            Dim cn As New SqlConnection(Me.txtCadenaConexion.Text)
            Dim da As New SqlDataAdapter("SELECT TOP 100 PERCENT name FROM dbo.sysobjects WHERE (type = 'U') AND name <> 'dtproperties' ORDER BY name", cn)

            Dim cb As New SqlCommandBuilder(da)
            Dim cbSistema As New SqlCommandBuilder(da)
            Dim dt As New DataTable
            da.Fill(dt)
            ' Solicito confirmación antes de crear todas las tablas
            If MessageBox.Show("Desea Crear las Claves Suplentes Para Todas las Tablas de la Base de Datos Seleccionada ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Me.ProgressBar1.Value = Me.ProgressBar1.Minimum
                Me.ProgressBar1.Maximum = dt.Rows.Count - 1
                Me.ProgressBar2.Value = Me.ProgressBar2.Minimum
                Me.ProgressBar2.Maximum = dt.Rows.Count - 1

                Dim indice = 0
                For Each row As DataRow In dt.Rows
                    txtTabla.Text = row("name")
                    Me.GenerarNombres()
                    Call Cargar(txtCadena.Text, txtTabla.Text)

                    Call generarSchema(txtArchivo.Text, txtClase.Text)
                    Me.ProgressBar1.Value = indice
                    Me.ProgressBar2.Value = indice

                    indice = indice + 1
                Next
                MessageBox.Show("Se Generó El Script Para Todas las Tablas de la Base de Datos." & Chr(13) & Chr(13) & "Los Archivos se Guardaron en C:\." & Chr(13) & Chr(13) & "Copiar la Clase en el Proyecto y Agregarla con --> Agregar elemento existente", "GENERACION DE CLASES", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.btnTodoSp.Enabled = True
            End If
        End If
        FileClose(FileInsert2)
    End Sub

    Private Sub btnExist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExist.Click
        ' Verifico si se trata de una o todas las tablas
        If Not chkTodasLasTablas.Checked Then
            ' Solicito confirmación antes de crear la clase
            If MessageBox.Show("Desea Crear el Exist '" & txtClase.Text & "' Para la Tabla '" & txtTabla.Text & "' ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                ' Cargo la estructura de la tabla en un array "modular"
                Call Cargar(txtCadena.Text, txtTabla.Text)

                ' Genero la clase
                Call GenerarClaseExist(txtArchivo.Text, txtClase.Text)

                ' Indico que se ha generado la clase
                MessageBox.Show("Se Generó el Exist '" & txtClase.Text & "' en el Archivo '" & "Exist.sql" & "' de la Tabla '" & txtTabla.Text & "'." & Chr(13) & Chr(13) & "El Archivo se Guardó en C:\." & Chr(13) & Chr(13) & "Ejecutar el Script", "GENERACION DE SCRIPT", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.btnSP.Enabled = True
            End If
        Else
            Dim cn As New SqlConnection(Me.txtCadenaConexion.Text)
            Dim da As New SqlDataAdapter("SELECT TOP 100 PERCENT name FROM dbo.sysobjects WHERE (type = 'U') AND name <> 'dtproperties' ORDER BY name", cn)

            Dim cb As New SqlCommandBuilder(da)
            Dim cbSistema As New SqlCommandBuilder(da)
            Dim dt As New DataTable
            da.Fill(dt)
            ' Solicito confirmación antes de crear todas las tablas
            If MessageBox.Show("Desea Crear los Exist Para Todas las Tablas de la Base de Datos Seleccionada ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Me.ProgressBar1.Value = Me.ProgressBar1.Minimum
                Me.ProgressBar1.Maximum = dt.Rows.Count - 1
                Me.ProgressBar2.Value = Me.ProgressBar2.Minimum
                Me.ProgressBar2.Maximum = dt.Rows.Count - 1

                Dim indice = 0
                For Each row As DataRow In dt.Rows
                    txtTabla.Text = row("name")
                    Me.GenerarNombres()
                    Call Cargar(txtCadena.Text, txtTabla.Text)
                    Call GenerarClaseExist(txtArchivo.Text, txtClase.Text)
                    Me.ProgressBar1.Value = indice
                    Me.ProgressBar2.Value = indice

                    indice = indice + 1
                Next
                MessageBox.Show("Se Generó el Exist Para Todas las Tablas de la Base de Datos." & Chr(13) & Chr(13) & "Los Archivos se Guardaron en C:\." & Chr(13) & Chr(13) & "Ejecutar el Script", "GENERACION DE SCRIPT", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.btnTodoSp.Enabled = True
            End If
        End If
        FileClose(FileInsert2)
    End Sub

    Private Sub chkExpress_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkExpress.CheckedChanged
        If Me.chkExpress.Checked = True Then
            Me.cmbServer.Text = Me.cmbServer.Text & "\SQLExpress"
        End If
    End Sub

    Private Sub btnAyuda_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAyuda.Click
        Dim frmTemp As New frmAyuda

        frmTemp.ShowDialog()
    End Sub

    Sub GetColumnas(ByVal id_tabla As Integer)
        Dim ocn As New SqlConnection(Me.txtCadena.Text)
        Dim oda As New SqlDataAdapter("SELECT  * FROM  syscolumns WHERE  (id = " & id_tabla & ")", ocn)

        Dim ocb As New SqlCommandBuilder(oda)
        Dim ocbSistema As New SqlCommandBuilder(oda)
        Dim odt As New DataTable
        oda.Fill(odt)

        Me.lstCampo.Items.Clear()
        For i As Integer = 0 To odt.Rows.Count - 1
            Me.lstCampo.Items.Add(odt.Rows(i).Item("name"))
        Next
    End Sub








    Private MouseIsDown As Boolean = False

    Private Sub lstCampo_MouseDown(ByVal sender As Object, ByVal e As  _
    System.Windows.Forms.MouseEventArgs) Handles lstCampo.MouseDown
        ' Set a flag to show that the mouse is down.
        MouseIsDown = True
    End Sub

    Private Sub lstCampo_MouseMove(ByVal sender As Object, ByVal e As  _
    System.Windows.Forms.MouseEventArgs) Handles lstCampo.MouseMove
        If MouseIsDown Then
            ' Initiate dragging.
            lstCampo.DoDragDrop(lstCampo.Text, DragDropEffects.Copy)
        End If
        MouseIsDown = False
    End Sub


    Private Sub txtTexto1_DragEnter(ByVal sender As Object, ByVal e As  _
    System.Windows.Forms.DragEventArgs)
        ' Check the format of the data being dropped.
        If (e.Data.GetDataPresent(DataFormats.Text)) Then
            ' Display the copy cursor.
            e.Effect = DragDropEffects.Copy
        Else
            ' Display the no-drop cursor.
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub txtCombo1_DragEnter(ByVal sender As Object, ByVal e As  _
    System.Windows.Forms.DragEventArgs)
        ' Check the format of the data being dropped.
        If (e.Data.GetDataPresent(DataFormats.Text)) Then
            ' Display the copy cursor.
            e.Effect = DragDropEffects.Copy
        Else
            ' Display the no-drop cursor.
            e.Effect = DragDropEffects.None
        End If
    End Sub


    Private Sub chkLocal_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLocal.CheckedChanged
        If Me.chkLocal.Checked Then
            Me.cmbServer.Text = "(local)\SQLExpress"
            Me.btnBaseDato.Enabled = True
        Else
            Me.cmbServer.Text = ""
            Me.btnBaseDato.Enabled = False
        End If
    End Sub



    Private Sub btnBuscaPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscaPath.Click
        Me.FolderBrowserDialog1.ShowDialog()
        Me.txtPath.Text = Me.FolderBrowserDialog1.SelectedPath
    End Sub

    Private Sub btnSincro_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSincro.Click
        ' Verifico si se trata de una o todas las tablas
        If Not chkTodasLasTablas.Checked Then
            ' Solicito confirmación antes de crear la clase
            If MessageBox.Show("Desea Crear el Sincro '" & txtClase.Text & "' Para la Tabla '" & txtTabla.Text & "' ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                ' Cargo la estructura de la tabla en un array "modular"
                Call Cargar(txtCadena.Text, txtTabla.Text)

                ' Genero la clase
                Call generarClaseSincro(txtArchivo.Text, txtClase.Text)

                ' Indico que se ha generado la clase
                MessageBox.Show("Se Generó el Sincro '" & txtClase.Text & "' en el Archivo '" & "Truncate.sql" & "' de la Tabla '" & txtTabla.Text & "'." & Chr(13) & Chr(13) & "El Archivo se Guardó en C:\." & Chr(13) & Chr(13) & "Ejecutar el Script", "GENERACION DE SCRIPT", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.btnSP.Enabled = True
            End If
        Else
            Dim cn As New SqlConnection(Me.txtCadenaConexion.Text)
            Dim da As New SqlDataAdapter("SELECT TOP 100 PERCENT name FROM dbo.sysobjects WHERE (type = 'U') AND name <> 'dtproperties' ORDER BY name", cn)

            Dim cb As New SqlCommandBuilder(da)
            Dim cbSistema As New SqlCommandBuilder(da)
            Dim dt As New DataTable
            da.Fill(dt)
            ' Solicito confirmación antes de crear todas las tablas
            If MessageBox.Show("Desea Crear los Sincro Para Todas las Tablas de la Base de Datos Seleccionada ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Me.ProgressBar1.Value = Me.ProgressBar1.Minimum
                Me.ProgressBar1.Maximum = dt.Rows.Count - 1
                Me.ProgressBar2.Value = Me.ProgressBar2.Minimum
                Me.ProgressBar2.Maximum = dt.Rows.Count - 1

                Dim indice = 0
                For Each row As DataRow In dt.Rows
                    txtTabla.Text = row("name")
                    Me.GenerarNombres()
                    Call Cargar(txtCadena.Text, txtTabla.Text)
                    Call generarClaseSincro(txtArchivo.Text, txtClase.Text)
                    Me.ProgressBar1.Value = indice
                    Me.ProgressBar2.Value = indice

                    indice = indice + 1
                Next
                MessageBox.Show("Se Generó el Sincro Para Todas las Tablas de la Base de Datos." & Chr(13) & Chr(13) & "Los Archivos se Guardaron en C:\." & Chr(13) & Chr(13) & "Ejecutar el Script", "GENERACION DE SCRIPT", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.btnTodoSp.Enabled = True
            End If
        End If
        FileClose(FileInsert2)
    End Sub

    Private Sub generarClaseSincro(ByVal PathCls As String, ByVal clase As String)
        If lk = 1 Then
            ' Doy formato al nombre del PathCls
            Dim PathInsert As String
            Dim PathFrm As String
            Dim PathFrmdetalle As String

            'seteo el path para los otros archivos
            PathInsert = Me.txtPath.Text & "\Alter_" & Me.txtTabla.Text & ".sql"
            PathFrm = PathCls
            PathFrmdetalle = PathCls


            ' Defino variables
            Dim FileCls As Integer = FreeFile()
            Dim reg As regSistemaTabla
            Dim idTabla As String
            Dim idParametro As String

            FileOpen(FileInsert2, PathInsert, OpenMode.Output)
            Dim Prefijo As String = "cop_"
        Else


        End If

        Dim Contador As Integer
        'lk = lk + 1


        PrintLine(FileInsert2, "")
        PrintLine(FileInsert2, TAB(0), "ALTER TABLE [dbo].[" & Me.txtTabla.Text & "] ADD")
        PrintLine(FileInsert2, TAB(4), "operacion char(1) NULL,")
        PrintLine(FileInsert2, TAB(4), "sincronizado bit NULL,")
        PrintLine(FileInsert2, TAB(4), "id_cliente_maestro int NULL")
        PrintLine(FileInsert2, TAB(0), "GO")
        PrintLine(FileInsert2, TAB(0), "ALTER TABLE [dbo].[" & Me.txtTabla.Text & "] SET (LOCK_ESCALATION = TABLE)")
        PrintLine(FileInsert2, TAB(0), "GO")
        PrintLine(FileInsert2, "")


        'FileClose(FileInsert2)
    End Sub

    Private Sub btnClase2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Me.txtPath.Text = "" Then
            MessageBox.Show("HAY QUE PONER UN PATH")
            Exit Sub
        End If

        ' Verifico si se trata de una o todas las tablas
        If Not chkTodasLasTablas.Checked Then
            ' Solicito confirmación antes de crear la clase
            If MessageBox.Show("Desea Crear la Clase '" & txtClase.Text & "' Para la Tabla '" & txtTabla.Text & "' ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                ' Cargo la estructura de la tabla en un array "modular"
                Call Cargar(txtCadena.Text, txtTabla.Text)

                oClase.Iniciar()
                oClase.EntidadOriginal(Me.txtPath.Text, Me.txtTabla.Text)
                oClase.ClaseOriginal(Me.txtPath.Text, Me.txtTabla.Text)

                If Me.chkPocket.Checked = True Then
                    oClase.EntidadPocket(Me.txtPath.Text, Me.txtTabla.Text, NumeroFilas, Me.chkTransactor.Checked)
                    oClase.ClasePocket(Me.txtPath.Text, Me.txtTabla.Text)
                End If

                oTest.Iniciar()
                oTest.ImprimirTest(Me.lstBaseDato.Text, Me.txtPath.Text, Me.txtTabla.Text, _
                Me.chkTestInsert.Checked, Me.chkTestDelete.Checked, Me.chkTestGetall.Checked, Me.chkTestGetone.Checked, _
                Me.chkTestGetcmb.Checked, Me.chkTestUpdate.Checked, Me.chkTestExist.Checked, Me.chkTestFind.Checked)

                oSql.Iniciar()
                oSql.ImprimirSps(Me.lstBaseDato.Text, Me.txtPath.Text, Me.txtTabla.Text, NumeroFilas, _
                Me.chkInsert.Checked, Me.chkDelete.Checked, Me.chkGetAll.Checked, Me.chkGetOne.Checked, Me.chkGetCmb.Checked, _
                Me.chkUpdate.Checked, Me.chkExist.Checked, Me.chkFind.Checked, Me.chkInsertOne.Checked, _
                Me.chkUpdateID.Checked, Me.chkDeleteEncabezado.Checked, Me.chkGetAllEncabezadoOne.Checked)

                oAbm.Iniciar()
                oAbm.ImprimirAbm(Me.txtPath.Text, Me.txtTabla.Text, Me.chkAbm.Checked, Me.chkDelete.Checked, Me.chkUsuario.Checked, NumeroFilas, Me.txtTituloFormulario.Text, Me.lstBaseDato.Text)


                '  oClase.Iniciar()
                ' oClase.ClaseAgregada(Me.txtPath.Text, Me.txtTabla.Text)

                ' Indico que se ha generado la clase
                MessageBox.Show("Se Generó la Clase '" & txtClase.Text & "' en el Archivo '" & txtArchivo.Text & "' de la Tabla '" & txtTabla.Text & "'." & Chr(13) & Chr(13) & "El Archivo se Guardó en C:\." & Chr(13) & Chr(13) & "Copiar la Clase en el Proyecto y Agregarla con --> Agregar elemento existente", "GENERACION DE CLASES", MessageBoxButtons.OK, MessageBoxIcon.Information)
                '  Me.btnSP2.Enabled = True
            End If
        Else
            Dim cn As New SqlConnection(Me.txtCadenaConexion.Text)
            Dim da As New SqlDataAdapter("SELECT TOP 100 PERCENT name FROM dbo.sysobjects WHERE (type = 'U') AND name <> 'dtproperties' ORDER BY name", cn)

            Dim cb As New SqlCommandBuilder(da)
            Dim cbSistema As New SqlCommandBuilder(da)
            Dim dt As New DataTable
            da.Fill(dt)
            ' Solicito confirmación antes de crear todas las tablas
            If MessageBox.Show("Desea las Clases Para Todas las Tablas de la Base de Datos Seleccionada ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Me.ProgressBar1.Value = Me.ProgressBar1.Minimum
                Me.ProgressBar1.Maximum = dt.Rows.Count - 1
                Me.ProgressBar2.Value = Me.ProgressBar2.Minimum
                Me.ProgressBar2.Maximum = dt.Rows.Count - 1

                Dim indice = 0
                For Each row As DataRow In dt.Rows
                    txtTabla.Text = row("name")
                    Me.GenerarNombres()
                    Call Cargar(txtCadena.Text, txtTabla.Text)


                    oClase.Iniciar()
                    oClase.EntidadOriginal(Me.txtPath.Text, Me.txtTabla.Text)
                    oClase.ClaseOriginal(Me.txtPath.Text, Me.txtTabla.Text)

                    If Me.chkPocket.Checked = True Then
                        oClase.EntidadPocket(Me.txtPath.Text, Me.txtTabla.Text, NumeroFilas, Me.chkTransactor.Checked)
                        oClase.ClasePocket(Me.txtPath.Text, Me.txtTabla.Text)
                    End If

                    oTest.Iniciar()
                    oTest.ImprimirTest(Me.lstBaseDato.Text, Me.txtPath.Text, Me.txtTabla.Text, _
                    Me.chkTestInsert.Checked, Me.chkTestDelete.Checked, Me.chkTestGetall.Checked, Me.chkTestGetone.Checked, _
                    Me.chkTestGetcmb.Checked, Me.chkTestUpdate.Checked, Me.chkTestExist.Checked, Me.chkTestFind.Checked)

                    oSql.Iniciar()
                    oSql.ImprimirSps(Me.lstBaseDato.Text, Me.txtPath.Text, Me.txtTabla.Text, NumeroFilas, _
                    Me.chkInsert.Checked, Me.chkDelete.Checked, Me.chkGetAll.Checked, Me.chkGetOne.Checked, Me.chkGetCmb.Checked, _
                    Me.chkUpdate.Checked, Me.chkExist.Checked, Me.chkFind.Checked, Me.chkInsertOne.Checked, _
                    Me.chkUpdateID.Checked, Me.chkDeleteEncabezado.Checked, Me.chkGetAllEncabezadoOne.Checked)

                    oAbm.Iniciar()
                    oAbm.ImprimirAbm(Me.txtPath.Text, Me.txtTabla.Text, Me.chkAbm.Checked, Me.chkDelete.Checked, Me.chkUsuario.Checked, NumeroFilas, Me.txtTabla.Text, Me.lstBaseDato.Text)


                    Me.ProgressBar1.Value = indice
                    Me.ProgressBar2.Value = indice

                    indice = indice + 1
                Next
                MessageBox.Show("Se Generó la Clase Para Todas las Tablas de la Base de Datos." & Chr(13) & Chr(13) & "Los Archivos se Guardaron en C:\." & Chr(13) & Chr(13) & "Copiar la Clase en el Proyecto y Agregarla con --> Agregar elemento existente", "GENERACION DE CLASES", MessageBoxButtons.OK, MessageBoxIcon.Information)
                '     Me.btnTodoSP2.Enabled = True
            End If
        End If
    End Sub

    Private Sub btnSP2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim PathArchivo As String = Me.txtPath.Text & "\sp" & Me.txtTabla.Text & ".sql"
        Dim fi As FileInfo = New FileInfo(PathArchivo)
        Dim File As StreamReader = fi.OpenText()
        Dim File2 As StreamReader = fi.OpenText()
        BanderaSP = 0

        NumeroLinea = Me.CountLines(File2)
        Me.ProgressBar1.Value = Me.ProgressBar1.Minimum
        Me.ProgressBar1.Maximum = NumeroLinea


        ParseStreamAndActionScript(File)
        If BanderaSP = 0 Then
            MessageBox.Show("Los Comandos se Ejecutaron Correctamente", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            MessageBox.Show("Se ha Producido un Error en la Ejecución ", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub btnTodoSP2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim cn As New SqlConnection(Me.txtCadenaConexion.Text)
        Dim da As New SqlDataAdapter("SELECT TOP 100 PERCENT name FROM dbo.sysobjects WHERE (type = 'U') AND name <> 'dtproperties' ORDER BY name", cn)

        Dim cb As New SqlCommandBuilder(da)
        Dim cbSistema As New SqlCommandBuilder(da)
        Dim dt As New DataTable
        da.Fill(dt)
        ' Solicito confirmación antes de crear todas las tablas
        If MessageBox.Show("Desea Crear los SP Para Todas las Tablas de la Base de Datos Seleccionada ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            Dim Indice As Integer = 0
            For Each row As DataRow In dt.Rows
                txtTabla.Text = row("name")
                Me.GenerarNombres()
                Dim PathArchivo As String = Me.txtPath.Text & "\sp" & Me.txtTabla.Text & ".sql"
                Dim fi As FileInfo = New FileInfo(PathArchivo)
                Dim File As StreamReader = fi.OpenText()
                Dim File2 As StreamReader = fi.OpenText()
                BanderaSP = 0

                Me.ProgressBar2.Value = Indice

                NumeroLinea = Me.CountLines(File2)
                Me.ProgressBar1.Value = Me.ProgressBar1.Minimum
                Me.ProgressBar1.Maximum = NumeroLinea

                ParseStreamAndActionScript(File)
                Indice = Indice + 1
            Next
            '' Genero una clase para cada tabla
            'Call GenerarTodasLasClases(txtCadena.Text)
        End If
        If BanderaSP = 0 Then
            MessageBox.Show("Los Comandos se Ejecutaron Correctamente", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            MessageBox.Show("Se ha Producido un Error en la Ejecución ", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    'Private Sub GenerarClase2(ByVal PathCls As String, ByVal clase As String)
    '    ' Doy formato al nombre del PathCls
    '    Dim PathInsert As String
    '    Dim PathFrm As String
    '    Dim PathFrmdetalle As String
    '    Dim PathClsCe As String

    '    'seteo el path para los otros archivos
    '    PathInsert = PathCls
    '    PathFrm = PathCls
    '    PathFrmdetalle = PathCls
    '    PathClsCe = PathCls

    '    Dim a As String = Me.txtPath.Text & "\"
    '    If Mid$(PathCls, Len(PathCls) - 3, 3) <> ".vb" Then
    '        PathCls = Me.txtPath.Text & "\" & PathCls & ".vb"
    '    Else
    '        PathCls = Me.txtPath.Text & "\" & PathCls
    '    End If

    '    ' Defino variables
    '    Dim FileCls As Integer = FreeFile()
    '    Dim reg As regSistemaTabla
    '    Dim idTabla As String
    '    Dim idParametro As String

    '    ' Abro un PathCls de texto (si el mismo existía se reempleza)
    '    FileOpen(FileCls, PathCls, OpenMode.Output)

    '    'CODIGO GENERADO PARA TRABAJAR CON SERVIDORES
    '    '*********************************************
    '    '
    '    'DEFINICION DEL CODIGO PARA LAS CLASES
    '    '
    '    '*********************************************

    '    'importo las referencias
    '    PrintLine(FileCls, TAB(0), "Imports System.Data.SqlClient")
    '    PrintLine(FileCls, TAB(0), "Imports System.Data")
    '    PrintLine(FileCls, TAB(0), "Imports System.IO")
    '    PrintLine(FileCls, "")

    '    'defino la clase
    '    PrintLine(FileCls, "Public Class " & clase & "_ent")
    '    PrintLine(FileCls, "")

    '    'defino las variables
    '    PrintLine(FileCls, TAB(5), "'defino las variables")
    '    PrintLine(FileCls, TAB(5), "Private dt As DataTable")
    '    PrintLine(FileCls, TAB(5), "Private dr As DataRow")
    '    PrintLine(FileCls, TAB(5), "Private da As SqlClient.SqlDataAdapter")
    '    PrintLine(FileCls, TAB(5), "Friend cnn As New Conexion")
    '    PrintLine(FileCls, TAB(5), "Friend cnntxt As New Conexion_txt")
    '    PrintLine(FileCls, "")

    '    'defino las propiedades y su variable

    '    PrintLine(FileCls, TAB(5), "'defino las propiedades")
    '    For Each reg In arrEstructura
    '        If reg.Orden = 0 Then
    '            PrintLine(FileCls, TAB(5), "Private _" & reg.nombre & " As " & reg.tiposql)
    '            PrintLine(FileCls, TAB(5), "Public ReadOnly Property " & reg.nombre & "() As " & reg.tiposql)
    '            PrintLine(FileCls, TAB(9), "Get")
    '            PrintLine(FileCls, TAB(13), "_" & reg.nombre & " = dr(" & """" & reg.nombre & """" & ")")
    '            PrintLine(FileCls, TAB(13), "Return _" & reg.nombre)
    '            PrintLine(FileCls, TAB(9), "End Get")
    '            PrintLine(FileCls, TAB(5), "end property")
    '            PrintLine(FileCls, "")
    '        Else
    '            ' Creo la variable local para usarse con la propiedad
    '            PrintLine(FileCls, TAB(5), "Private _" & reg.nombre & " As " & reg.tiposql)

    '            PrintLine(FileCls, TAB(5), "Public Property " & reg.nombre & "() As " & reg.tiposql)
    '            PrintLine(FileCls, TAB(9), "Get")
    '            PrintLine(FileCls, TAB(13), "_" & reg.nombre & " = dr(" & """" & reg.nombre & """" & ")")
    '            PrintLine(FileCls, TAB(13), "Return _" & reg.nombre)
    '            PrintLine(FileCls, TAB(9), "End Get")
    '            PrintLine(FileCls, TAB(9), "Set(ByVal Value As " & reg.tiposql & ")")
    '            PrintLine(FileCls, TAB(13), "dr(" & """" & reg.nombre & """" & ") = Value")
    '            PrintLine(FileCls, TAB(9), "End Set")
    '            PrintLine(FileCls, TAB(5), "end property")
    '            PrintLine(FileCls, "")
    '        End If
    '    Next

    '    'Public Sub Insertar()

    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(5), "'procedimiento insertar")
    '    PrintLine(FileCls, TAB(5), "Public Sub Insertar()")
    '    PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '    PrintLine(FileCls, TAB(9), "da.MissingSchemaAction = MissingSchemaAction.AddWithKey")
    '    PrintLine(FileCls, TAB(9), "da.Fill(dt)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Me.AsignarTipos()")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Dim cmdins As New SqlCommand(" & """" & "cop_" & txtTabla.Text.Trim & "_Insert" & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, TAB(9), "cmdins.CommandType = CommandType.StoredProcedure")
    '    PrintLine(FileCls, TAB(9), "da.InsertCommand = cmdins")
    '    PrintLine(FileCls, "")
    '    For Each reg In arrEstructura
    '        If reg.Orden = 0 Then
    '            idTabla = reg.nombre
    '            idParametro = "@" & reg.nombre
    '            PrintLine(FileCls, TAB(9), "Dim prm As SqlParameter")
    '            PrintLine(FileCls, TAB(9), "prm = da.InsertCommand.Parameters.Add(" & """" & "@" & reg.nombre & """" & ", SqlDbType." & reg.tipo & ", " & 0 & ", " & """" & reg.nombre & """" & ")")
    '            PrintLine(FileCls, TAB(9), "prm.Direction = ParameterDirection.Output")
    '        Else
    '            PrintLine(FileCls, TAB(9), "da.InsertCommand.Parameters.Add(" & """" & "@" & reg.nombre & """" & ", SqlDbType." & reg.tipo & ", " & reg.longitud & ", " & """" & reg.nombre & """" & ")")
    '        End If
    '    Next
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "dr = dt.NewRow()")
    '    For Each reg In arrEstructura
    '        If reg.Orden = 1 Then
    '            PrintLine(FileCls, TAB(9), "dr(" & """" & reg.nombre & """" & ") = MaxId(" & """" & Me.txtTabla.Text & """" & ")")
    '        Else
    '            PrintLine(FileCls, TAB(9), "dr(" & """" & reg.nombre & """" & ") = " & reg.valorinicial)
    '        End If
    '    Next
    '    PrintLine(FileCls, TAB(9), "dt.Rows.Add(dr)")
    '    PrintLine(FileCls, TAB(9), "CrearComandoUpdate()")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")

    '    'Public Sub Modificar

    '    PrintLine(FileCls, TAB(5), "'funcion modificar")
    '    PrintLine(FileCls, TAB(5), "Public Function Modificar() As DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & "_Update" & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")

    '    For Each reg In arrEstructura
    '        If reg.Orden = 1 Then
    '            PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & "@" & reg.nombre & """" & ", SqlDbType." & reg.tipo & ", " & reg.longitud & ", " & """" & reg.nombre & """" & ")")
    '            PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & "@" & reg.nombre & """" & ").Value = " & reg.nombre)
    '        ElseIf reg.nombre = "operacion" Or reg.nombre = "sincronizado" Then
    '            'next row
    '        Else
    '            PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & "@" & reg.nombre & """" & ", SqlDbType." & reg.tipo & ", " & reg.longitud & ", " & """" & reg.nombre & """" & ")")
    '            PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & "@" & reg.nombre & """" & ").Value = " & reg.nombre)
    '        End If
    '    Next

    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Return odt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    'Public Sub Modificar Old

    '    PrintLine(FileCls, TAB(5), "'procedimiento modificar")
    '    PrintLine(FileCls, TAB(5), "Public Sub ModificarOld(ByVal id" & txtTabla.Text.Trim & " As Integer)")
    '    PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '    PrintLine(FileCls, TAB(9), "da = New SqlClient.SqlDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & " WHERE id_" & Me.txtTabla.Text & " = @" & Me.txtTabla.Text & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, TAB(9), "da.SelectCommand.Parameters.AddWithValue(" & """" & "@" & Me.txtTabla.Text & """" & ", id" & Me.txtTabla.Text & ")")
    '    PrintLine(FileCls, TAB(9), "da.Fill(dt)")
    '    PrintLine(FileCls, TAB(9), "Me.AsignarTipos()")
    '    PrintLine(FileCls, TAB(9), "If dt.Rows.Count = 0 Then")
    '    PrintLine(FileCls, TAB(13), "Throw New Exception(" & """" & "No se a encontrado el Registro" & """" & ")")
    '    PrintLine(FileCls, TAB(9), "Else")
    '    PrintLine(FileCls, TAB(13), "dr = dt.Rows(0)")
    '    PrintLine(FileCls, TAB(9), "End If")
    '    PrintLine(FileCls, TAB(9), "CrearComandoUpdate()")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")


    '    'Public Sub Borrar

    '    PrintLine(FileCls, TAB(5), "'funcion borrar")
    '    PrintLine(FileCls, TAB(5), "Public Function Borrar(ByVal id_" & txtTabla.Text.Trim & " As Integer) As DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & "_Delete" & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    For Each reg In arrEstructura
    '        If reg.Orden = 1 Then
    '            PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & "@" & reg.nombre & """" & ", SqlDbType." & reg.tipo & ", " & reg.longitud & ", " & """" & reg.nombre & """" & ")")
    '            PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & "@" & reg.nombre & """" & ").Value = " & reg.nombre)
    '            Exit For
    '        End If
    '    Next
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Return odt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    'Private Sub AsignarTipos()

    '    PrintLine(FileCls, TAB(5), "'asigno el tipo de datos a los parametros")
    '    PrintLine(FileCls, TAB(5), "Private Sub AsignarTipos()")
    '    PrintLine(FileCls, TAB(9), "For Each dc As DataColumn In dt.Columns")
    '    PrintLine(FileCls, TAB(13), "Select Case dc.ColumnName")
    '    For Each reg In arrEstructura
    '        PrintLine(FileCls, TAB(16), "Case " & """" & reg.nombre & """")
    '        PrintLine(FileCls, TAB(19), "dc.DataType = Type.GetType(" & """" & "System." & reg.tiposql & """" & ")")
    '    Next
    '    PrintLine(FileCls, TAB(13), "End Select")
    '    PrintLine(FileCls, TAB(9), "Next")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")

    '    'Private Sub CrearComandoUpdate()

    '    PrintLine(FileCls, TAB(5), "'creo el commandbuilder")
    '    PrintLine(FileCls, TAB(5), "Private Sub CrearComandoUpdate()")
    '    PrintLine(FileCls, TAB(9), "Dim cmd As New SqlCommandBuilder(da)")
    '    PrintLine(FileCls, TAB(9), "da.UpdateCommand = cmd.GetUpdateCommand")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")

    '    'Public Sub Cancelar()

    '    PrintLine(FileCls, TAB(5), "'cancelo los cambios del datatable")
    '    PrintLine(FileCls, TAB(5), "Public Sub Cancelar()")
    '    PrintLine(FileCls, TAB(9), "dt.Clear()")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")

    '    'Public Function DataTable()

    '    PrintLine(FileCls, TAB(5), "'asigno el datatable")
    '    PrintLine(FileCls, TAB(5), "Public Function DataTable() As DataTable")
    '    PrintLine(FileCls, TAB(9), "Return dt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    'Public Sub Guardar()

    '    PrintLine(FileCls, TAB(5), "'actualizo la base de datos")
    '    PrintLine(FileCls, TAB(5), "Public Sub Guardar()")
    '    PrintLine(FileCls, TAB(9), "da.Update(dt)")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")

    '    'Public Function Cargar dt()
    '    Dim id_temp As String = ""
    '    For Each reg In arrEstructura
    '        If reg.Orden = 1 Then
    '            id_temp = reg.nombre
    '            Exit For
    '        End If
    '    Next
    '    PrintLine(FileCls, TAB(5), "'funcion que carga el datatable")
    '    PrintLine(FileCls, TAB(5), "Public Function Cargar() As DataTable")
    '    PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '    PrintLine(FileCls, TAB(9), "da = New SqlClient.SqlDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & " WHERE " & id_temp & " = 0" & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, TAB(9), "da.MissingSchemaAction = MissingSchemaAction.AddWithKey")
    '    PrintLine(FileCls, TAB(9), "da.Fill(dt)")
    '    PrintLine(FileCls, TAB(9), "Return dt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    'Public Function Buscar

    '    PrintLine(FileCls, TAB(5), "'funcion de busqueda")
    '    PrintLine(FileCls, TAB(5), "Public Function Buscar(ByVal Nombre As String) As DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & "_Find" & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & "@nombre" & """" & ", SqlDbType.NChar, 30, " & """" & "nombre" & """" & ")")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & "@nombre" & """" & ").Value = Nombre")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Return odt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    'Public Function ConsultarTodo()

    '    PrintLine(FileCls, TAB(5), "'funcion de consulta")
    '    PrintLine(FileCls, TAB(5), "Public Function ConsultarTodo() As DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & "_GetAll" & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Return odt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    'Public Function GetCmb()

    '    PrintLine(FileCls, TAB(5), "'funcion para llenar el combo")
    '    PrintLine(FileCls, TAB(5), "Public Function GetCmb() As DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & "_GetCmb" & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Return odt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    'public function GetOne()

    '    PrintLine(FileCls, TAB(5), "'funcion que trae un registro poniendo el id")
    '    PrintLine(FileCls, TAB(5), "Public Function GetOne(ByVal id_" & txtTabla.Text.Trim & " As Integer) As DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & "_GetOne" & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    For Each reg In arrEstructura
    '        If reg.Orden = 1 Then
    '            PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & "@" & reg.nombre & """" & ", SqlDbType." & reg.tipo & ", " & reg.longitud & ", " & """" & reg.nombre & """" & ")")
    '            PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & "@" & reg.nombre & """" & ").Value = " & reg.nombre)
    '            Exit For
    '        End If
    '    Next
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Return odt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    ''Public Function Consultar con oda

    '    'PrintLine(FileCls, TAB(5), "'ejemplo de consulta con parametros")
    '    'PrintLine(FileCls, TAB(5), "Public Function Consultar(ByVal id_" & txtTabla.Text.Trim & " As Integer) As DataTable")
    '    'PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    'PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "Ejemplo_" & txtTabla.Text.Trim & """" & ", cnn.Coneccion)")
    '    'PrintLine(FileCls, "")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & idParametro & """" & ", SqlDbType.Int, 4, " & """" & idTabla & """" & ")")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & idParametro & """" & ").Value = id_" & txtTabla.Text.Trim)
    '    'PrintLine(FileCls, "")
    '    'PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    'PrintLine(FileCls, TAB(9), "Return odt")
    '    'PrintLine(FileCls, TAB(5), "End Function")
    '    'PrintLine(FileCls, "")

    '    ''Public Function ConsultarDecimal

    '    'PrintLine(FileCls, TAB(5), "'ejemplo de consulta con parametros trayendo un decimal")
    '    'PrintLine(FileCls, TAB(5), "Public Function ConsultarDecimal(ByVal id_" & txtTabla.Text.Trim & " As Integer) As Decimal")
    '    'PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    'PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "Ejemplo_" & txtTabla.Text.Trim & "Decimal" & """" & ", cnn.Coneccion)")
    '    'PrintLine(FileCls, TAB(9), "Dim Total As Decimal")
    '    'PrintLine(FileCls, "")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & idParametro & """" & ", SqlDbType.Int, 4, " & """" & idTabla & """" & ")")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & idParametro & """" & ").Value = id_" & txtTabla.Text.Trim)
    '    'PrintLine(FileCls, "")
    '    'PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    'PrintLine(FileCls, TAB(9), "Total = odt.Rows(0).Item(" & """" & "Total" & """" & ").ToString")
    '    'PrintLine(FileCls, TAB(9), "Return Total")
    '    'PrintLine(FileCls, TAB(5), "End Function")
    '    'PrintLine(FileCls, "")

    '    ''Public Function ControlarSiExiste

    '    'PrintLine(FileCls, TAB(5), "'ejemplo de consulta con parametros trayendo un boolean")
    '    'PrintLine(FileCls, TAB(5), "Public Function ControlarSiExiste(ByVal id_" & txtTabla.Text.Trim & " As Integer) As Boolean")
    '    'PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    'PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & "_Check" & """" & ", cnn.Coneccion)")
    '    'PrintLine(FileCls, TAB(9), "Dim Total As Integer")
    '    'PrintLine(FileCls, "")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & idParametro & """" & ", SqlDbType.Int, 4, " & """" & idTabla & """" & ")")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & idParametro & """" & ").Value = id_" & txtTabla.Text.Trim)
    '    'PrintLine(FileCls, "")
    '    'PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    'PrintLine(FileCls, TAB(9), "Total = odt.Rows(0).Item(" & """" & "Total" & """" & ").ToString")
    '    'PrintLine(FileCls, TAB(9), "If Total = 0 Then")
    '    'PrintLine(FileCls, TAB(13), "Return True  'NO EXISTE")
    '    'PrintLine(FileCls, TAB(9), "Else")
    '    'PrintLine(FileCls, TAB(13), "Return False 'SI EXISTE")
    '    'PrintLine(FileCls, TAB(9), "End If")
    '    'PrintLine(FileCls, TAB(5), "End Function")
    '    'PrintLine(FileCls, "")





    '    'Public Function Exist

    '    PrintLine(FileCls, TAB(5), "'controla si existe el registro en la base de datos")
    '    PrintLine(FileCls, TAB(5), "Public Function Exist() As Boolean")
    '    PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & "_Exist" & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, TAB(9), "Dim Total As Integer")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")

    '    For Each reg In arrEstructura
    '        If reg.Orden = 1 Then
    '            'next row
    '        ElseIf reg.nombre = "operacion" Or reg.nombre = "sincronizado" Or reg.nombre = "id_cliente_maestro" Then
    '            'next row
    '        Else
    '            PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & "@" & reg.nombre & """" & ", SqlDbType." & reg.tipo & ", " & reg.longitud & ", " & """" & reg.nombre & """" & ")")
    '            PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & "@" & reg.nombre & """" & ").Value = Me." & reg.nombre)
    '        End If
    '    Next
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    PrintLine(FileCls, TAB(9), "Total = odt.Rows(0).Item(" & """" & "Total" & """" & ").ToString")
    '    PrintLine(FileCls, TAB(9), "If Total = 0 Then")
    '    PrintLine(FileCls, TAB(13), "Return False  'NO EXISTE")
    '    PrintLine(FileCls, TAB(9), "Else")
    '    PrintLine(FileCls, TAB(13), "Return True 'SI EXISTE")
    '    PrintLine(FileCls, TAB(9), "End If")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")



    '    'borra toda la tabla
    '    PrintLine(FileCls, TAB(5), "'borra todos los datos de la tabla")
    '    PrintLine(FileCls, TAB(5), "Public Sub BorrarTodo()")
    '    PrintLine(FileCls, TAB(9), "Dim oConexion As New Conexion")
    '    PrintLine(FileCls, TAB(9), "Dim Command As SqlCommand = New SqlCommand()")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Command.Connection = oConexion.Coneccion")
    '    PrintLine(FileCls, TAB(9), "Command.CommandText = ""DELETE FROM " & Me.txtTabla.Text & """")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oConexion.Abrir()")
    '    PrintLine(FileCls, TAB(9), "Command.ExecuteNonQuery()")
    '    PrintLine(FileCls, TAB(9), "oConexion.Cerrar()")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")

    '    'truncate toda la tabla
    '    PrintLine(FileCls, TAB(5), "'borra todos los datos de la tabla")
    '    PrintLine(FileCls, TAB(5), "Public Sub Truncate()")
    '    PrintLine(FileCls, TAB(9), "Dim oConexion As New Conexion")
    '    PrintLine(FileCls, TAB(9), "Dim Command As SqlCommand = New SqlCommand()")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Command.Connection = oConexion.Coneccion")
    '    PrintLine(FileCls, TAB(9), "Command.CommandText = ""TRUNCATE TABLE " & Me.txtTabla.Text & """")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oConexion.Abrir()")
    '    PrintLine(FileCls, TAB(9), "Command.ExecuteNonQuery()")
    '    PrintLine(FileCls, TAB(9), "oConexion.Cerrar()")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")

    '    'borra toda la tabla
    '    PrintLine(FileCls, TAB(5), "'inserta un registro en la tabla")
    '    PrintLine(FileCls, TAB(5), "Public Sub InsertOne()")
    '    PrintLine(FileCls, TAB(9), "Dim oConexion As New Conexion")
    '    PrintLine(FileCls, TAB(9), "Dim Command As SqlCommand = New SqlCommand()")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Command.Connection = oConexion.Coneccion")
    '    PrintLine(FileCls, TAB(9), "Command.CommandText = " & """" & "cop_" & txtTabla.Text.Trim & "_InsertOne" & """")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Command.CommandType = CommandType.StoredProcedure")
    '    PrintLine(FileCls, TAB(9), "oConexion.Abrir()")
    '    PrintLine(FileCls, TAB(9), "Command.ExecuteNonQuery()")
    '    PrintLine(FileCls, TAB(9), "oConexion.Cerrar()")
    '    PrintLine(FileCls, TAB(5), "End Sub")
    '    PrintLine(FileCls, "")

    '    'consulta el archivo txt para importarlo
    '    PrintLine(FileCls, TAB(5), "'importa los datos de una campaña desde el txt")
    '    PrintLine(FileCls, TAB(5), "Public Function Cargar_Archivo(ByVal path As String) As DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim odt As New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Dim oda As New System.Data.OleDb.OleDbDataAdapter( _")
    '    PrintLine(FileCls, TAB(9), """SELECT * FROM " & Me.txtTabla.Text & ".txt"", cnntxt.Coneccion(path))")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "Return odt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    '***realizado con un sp general
    '    'PrintLine(FileCls, TAB(5), "'select max id")
    '    'PrintLine(FileCls, TAB(5), "Public Function MaxId(ByVal Tabla As String) As Integer")
    '    'PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    'PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop__NewId" & """" & ", cnn.Coneccion)")
    '    'PrintLine(FileCls, "")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & "@Tabla" & """" & ", SqlDbType.Varchar, 100, " & """" & "Tabla" & """" & ")")
    '    'PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & "@Tabla" & """" & ").Value = Tabla")
    '    'PrintLine(FileCls, "")
    '    'PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    'PrintLine(FileCls, TAB(9), "Return odt.Rows(0).Item(" & """" & "id" & """" & ")")
    '    'PrintLine(FileCls, TAB(5), "End Function")
    '    'PrintLine(FileCls, "")

    '    '      PrintLine(FileCls, TAB(13), "'********************************************")
    '    '      PrintLine(FileCls, TAB(13), "'    CODIGO AGREGADO A LA CLASE ORIGINAL")
    '    '      PrintLine(FileCls, TAB(13), "'********************************************")
    '    '      PrintLine(FileCls, "")
    '    '      PrintLine(FileCls, "")
    '    '      PrintLine(FileCls, "")
    '    '      PrintLine(FileCls, "")
    '    ' Comienzo a generar la clase
    '    PrintLine(FileCls, "End Class")

    '    ' Cierro el PathCls de versión
    '    FileClose(FileCls)



    '    '*****************************************************
    '    ' DEFINICION DEL CODIGO PARA GENERAR LA CLASE HEREDADA
    '    '*****************************************************

    '    If Mid$(PathCls, Len(PathCls) - 3, 3) <> ".vb" Then
    '        PathCls = Me.txtPath.Text & "\cls" & Me.txtTabla.Text & ".vb"
    '    Else
    '        PathCls = Me.txtPath.Text & "\cls" & Me.txtTabla.Text & ".vb"
    '    End If

    '    'Abro un PathCls de texto (si el mismo existía se reempleza)
    '    FileOpen(FileCls, PathCls, OpenMode.Output)

    '    'importo las referencias
    '    PrintLine(FileCls, TAB(0), "Imports System.Data.SqlClient")
    '    PrintLine(FileCls, TAB(0), "Imports System.Data")
    '    PrintLine(FileCls, TAB(0), "Imports System.IO")
    '    PrintLine(FileCls, "")

    '    'defino la clase
    '    PrintLine(FileCls, "Public Class " & clase)
    '    PrintLine(FileCls, TAB(5), "Inherits " & clase & "_ent")
    '    PrintLine(FileCls, "")

    '    'Public Function Consultar con oda

    '    PrintLine(FileCls, TAB(5), "'ejemplo de consulta con parametros")
    '    PrintLine(FileCls, TAB(5), "Public Function Consultar_Ejemplo(ByVal id_" & txtTabla.Text.Trim & " As Integer) As DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '    PrintLine(FileCls, TAB(9), "Dim oda As New SqlDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & """" & ", cnn.Coneccion)")
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.CommandType = CommandType.StoredProcedure")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters.Add(" & """" & "@id_" & txtTabla.Text.Trim & """" & ", SqlDbType.Int, 4, " & """" & "id_" & txtTabla.Text.Trim & """" & ")")
    '    PrintLine(FileCls, TAB(9), "oda.SelectCommand.Parameters(" & """" & "@id_" & txtTabla.Text.Trim & """" & ").Value = id_" & txtTabla.Text.Trim)
    '    PrintLine(FileCls, "")
    '    PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '    PrintLine(FileCls, TAB(9), "Return odt")
    '    PrintLine(FileCls, TAB(5), "End Function")
    '    PrintLine(FileCls, "")

    '    PrintLine(FileCls, "End Class")

    '    ' Cierro el PathCls de versión
    '    FileClose(FileCls)









    '    '*******************************************************
    '    ' DEFINICION DEL CODIGO PARA GENERAR LA CLASE TESTEADORA
    '    '*******************************************************
    '    If Me.chkTest.Checked Then
    '        If Mid$(PathCls, Len(PathCls) - 3, 3) <> ".vb" Then
    '            PathCls = Me.txtPath.Text & "\" & Me.txtTabla.Text & "Test.vb"
    '        Else
    '            PathCls = Me.txtPath.Text & "\" & Me.txtTabla.Text
    '        End If

    '        'Abro un PathCls de texto (si el mismo existía se reempleza)
    '        FileOpen(FileCls, PathCls, OpenMode.Output)

    '        PrintLine(FileCls, TAB(0), "Imports System.Data")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(0), "Imports Microsoft.VisualStudio.TestTools.UnitTesting")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(0), "Imports " & Me.lstBaseDato.Text.Trim)
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(0), "'''<summary>")
    '        PrintLine(FileCls, TAB(0), "'''Se trata de una clase de prueba para " & Me.txtTabla.Text & "_entTest y se pretende que")
    '        PrintLine(FileCls, TAB(0), "'''contenga todas las pruebas unitarias " & Me.txtTabla.Text & "_entTest. ")
    '        PrintLine(FileCls, TAB(0), "'''</summary>")
    '        PrintLine(FileCls, TAB(0), "<TestClass()> _")
    '        PrintLine(FileCls, TAB(0), "Public Class " & Me.txtTabla.Text & "Test")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "Private testContextInstance As TestContext")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), "'''Obtiene o establece el contexto de la prueba que proporciona")
    '        PrintLine(FileCls, TAB(4), "'''la información y funcionalidad para la ejecución de pruebas actual. ")
    '        PrintLine(FileCls, TAB(4), "'''</summary>")
    '        PrintLine(FileCls, TAB(4), "Public Property TestContext() As TestContext")
    '        PrintLine(FileCls, TAB(8), "Get")
    '        PrintLine(FileCls, TAB(12), "Return testContextInstance")
    '        PrintLine(FileCls, TAB(8), "End Get")
    '        PrintLine(FileCls, TAB(8), "Set(ByVal value As TestContext) ")
    '        PrintLine(FileCls, TAB(12), "testContextInstance = value")
    '        PrintLine(FileCls, TAB(8), "End Set")
    '        PrintLine(FileCls, TAB(4), "End Property")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(0), "#Region " & """" & "Atributos de prueba adicionales" & """")
    '        PrintLine(FileCls, TAB(4), "'")
    '        PrintLine(FileCls, TAB(4), "'Puede utilizar los siguientes atributos adicionales mientras escribe sus pruebas: ")
    '        PrintLine(FileCls, TAB(4), "'")
    '        PrintLine(FileCls, TAB(4), "'Use ClassInitialize para ejecutar código antes de ejecutar la primera prueba en la clase ")
    '        PrintLine(FileCls, TAB(4), "'<ClassInitialize()>  _")
    '        PrintLine(FileCls, TAB(4), "'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext) ")
    '        PrintLine(FileCls, TAB(4), "'End Sub")
    '        PrintLine(FileCls, TAB(4), "'")
    '        PrintLine(FileCls, TAB(4), "'Use ClassCleanup para ejecutar código después de haber ejecutado todas las pruebas en una clase")
    '        PrintLine(FileCls, TAB(4), "'<ClassCleanup()>  _")
    '        PrintLine(FileCls, TAB(4), "'Public Shared Sub MyClassCleanup()")
    '        PrintLine(FileCls, TAB(4), "'End Sub")
    '        PrintLine(FileCls, TAB(4), "'")
    '        PrintLine(FileCls, TAB(4), "'Use TestInitialize para ejecutar código antes de ejecutar cada prueba")
    '        PrintLine(FileCls, TAB(4), "'<TestInitialize()>  _")
    '        PrintLine(FileCls, TAB(4), "'Public Sub MyTestInitialize()")
    '        PrintLine(FileCls, TAB(4), "'End Sub")
    '        PrintLine(FileCls, TAB(4), "'")
    '        PrintLine(FileCls, TAB(4), "'Use TestCleanup para ejecutar código después de que se hayan ejecutado todas las pruebas")
    '        PrintLine(FileCls, TAB(4), "'<TestCleanup()>  _")
    '        PrintLine(FileCls, TAB(4), "'Public Sub MyTestCleanup()")
    '        PrintLine(FileCls, TAB(4), "'End Sub")
    '        PrintLine(FileCls, TAB(4), "'")
    '        PrintLine(FileCls, TAB(0), "#End Region")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), " '''Una prueba de Modificar")
    '        PrintLine(FileCls, TAB(4), " '''</summary>")
    '        PrintLine(FileCls, TAB(4), "<TestMethod()> _")
    '        PrintLine(FileCls, TAB(4), "Public Sub Modificar" & Me.txtTabla.Text & "Test()")
    '        PrintLine(FileCls, TAB(8), "Dim target As " & Me.txtTabla.Text & "_ent = New " & Me.txtTabla.Text & "_ent ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim actual As DataTable")

    '        PrintLine(FileCls, TAB(8), "target.Truncate()")
    '        PrintLine(FileCls, TAB(8), "target.InsertOne()")
    '        PrintLine(FileCls, TAB(8), "target.Cargar()")
    '        PrintLine(FileCls, TAB(8), "target.ModificarOld(1) ")
    '        PrintLine(FileCls, TAB(8), "target.nombre_" & Me.txtTabla.Text & " = " & """" & "modificado" & """")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "actual = target.Modificar")
    '        PrintLine(FileCls, TAB(8), "Dim dt As DataTable = New DataTable")
    '        PrintLine(FileCls, TAB(8), "dt = target.ConsultarTodo")

    '        PrintLine(FileCls, TAB(8), "Assert.IsTrue(dt.Rows(0).Item(" & """" & "nombre_" & Me.txtTabla.Text & """" & ") = " & """" & "modificado" & """" & "," & """" & "no:" & """" & " & dt.Rows(0).Item(" & """" & "nombre_" & Me.txtTabla.Text & """" & ").ToString)")
    '        PrintLine(FileCls, TAB(4), "End Sub")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), "'''Una prueba de Insertar")
    '        PrintLine(FileCls, TAB(4), "'''</summary>")
    '        PrintLine(FileCls, TAB(4), "<TestMethod()> _")
    '        PrintLine(FileCls, TAB(4), "Public Sub Insertar" & Me.txtTabla.Text & "Test()")
    '        PrintLine(FileCls, TAB(8), "Dim target As " & Me.txtTabla.Text & "_ent = New " & Me.txtTabla.Text & "_ent ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.Truncate()")
    '        PrintLine(FileCls, TAB(8), "target.InsertOne()")
    '        PrintLine(FileCls, TAB(8), "target.Cargar()")
    '        PrintLine(FileCls, TAB(8), "target.Insertar()")
    '        PrintLine(FileCls, TAB(8), "target.id_" & Me.txtTabla.Text & " = 2")
    '        PrintLine(FileCls, TAB(8), "target.nombre_" & Me.txtTabla.Text & " = " & """" & "prueba" & """")
    '        PrintLine(FileCls, TAB(8), "target.Guardar()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Dim dt As DataTable = New DataTable")
    '        PrintLine(FileCls, TAB(8), "dt = target.ConsultarTodo")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Assert.IsTrue(dt.Rows.Count = 2, " & """" & "son: " & """" & " & dt.Rows.Count & " & """" & " registros" & """" & ")")
    '        PrintLine(FileCls, TAB(4), "End Sub")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), "'''Una prueba de GetOne")
    '        PrintLine(FileCls, TAB(4), "'''</summary>")
    '        PrintLine(FileCls, TAB(4), "<TestMethod()> _")
    '        PrintLine(FileCls, TAB(4), "Public Sub GetOne" & Me.txtTabla.Text & "Test()")
    '        PrintLine(FileCls, TAB(8), "Dim target As " & Me.txtTabla.Text & "_ent = New " & Me.txtTabla.Text & "_ent ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim id_" & Me.txtTabla.Text & " As Integer = 1 ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim actual As DataTable")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.Truncate()")
    '        PrintLine(FileCls, TAB(8), "target.InsertOne()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "actual = target.GetOne(id_" & Me.txtTabla.Text & ") ")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Assert.IsTrue(actual.Rows.Count = 1, " & """" & "son: " & """" & " & actual.Rows.Count & " & """" & " registros" & """" & ")")
    '        PrintLine(FileCls, TAB(4), "End Sub")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), "'''Una prueba de GetCmb")
    '        PrintLine(FileCls, TAB(4), "'''</summary>")
    '        PrintLine(FileCls, TAB(4), "<TestMethod()> _")
    '        PrintLine(FileCls, TAB(4), "Public Sub GetCmb" & Me.txtTabla.Text & "Test()")
    '        PrintLine(FileCls, TAB(8), "Dim target As " & Me.txtTabla.Text & "_ent = New " & Me.txtTabla.Text & "_ent ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim actual As DataTable")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.Truncate()")
    '        PrintLine(FileCls, TAB(8), "target.InsertOne()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "actual = target.GetCmb()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Assert.IsTrue(actual.Rows.Count = 1, " & """" & "son: " & """" & " & actual.Rows.Count & " & """" & " registros" & """" & ")")
    '        PrintLine(FileCls, TAB(4), "End Sub")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), "'''Una prueba de Exist")
    '        PrintLine(FileCls, TAB(4), "'''</summary>")
    '        PrintLine(FileCls, TAB(4), "<TestMethod()> _")
    '        PrintLine(FileCls, TAB(4), "Public Sub Exist" & Me.txtTabla.Text & "Test()")
    '        PrintLine(FileCls, TAB(8), "Dim target As " & Me.txtTabla.Text & "_ent = New " & Me.txtTabla.Text & "_ent ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim expected As Boolean = False ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim actual As Boolean")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.Truncate()")
    '        PrintLine(FileCls, TAB(8), "target.InsertOne()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.ModificarOld(1) ")
    '        PrintLine(FileCls, TAB(8), "target.nombre_" & Me.txtTabla.Text & " = " & """" & "prueba" & """")
    '        PrintLine(FileCls, TAB(8), "target.Guardar()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "actual = target.Exist")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Assert.IsTrue(actual = True, " & """" & "no:" & """" & " & actual) ")
    '        PrintLine(FileCls, TAB(4), "End Sub")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), "'''Una prueba de ConsultarTodo")
    '        PrintLine(FileCls, TAB(4), "'''</summary>")
    '        PrintLine(FileCls, TAB(4), "<TestMethod()> _")
    '        PrintLine(FileCls, TAB(4), "Public Sub ConsultarTodo" & Me.txtTabla.Text & "Test()")
    '        PrintLine(FileCls, TAB(8), "Dim target As " & Me.txtTabla.Text & "_ent = New " & Me.txtTabla.Text & "_ent ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim actual As DataTable")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.Truncate()")
    '        PrintLine(FileCls, TAB(8), "target.InsertOne()")
    '        PrintLine(FileCls, TAB(8), "target.Cargar()")
    '        PrintLine(FileCls, TAB(8), "target.Insertar()")
    '        PrintLine(FileCls, TAB(8), "target.id_" & Me.txtTabla.Text & " = 2")
    '        PrintLine(FileCls, TAB(8), "target.nombre_" & Me.txtTabla.Text & " = " & """" & "prueba" & """")
    '        PrintLine(FileCls, TAB(8), "target.Guardar()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "actual = target.ConsultarTodo")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Assert.IsTrue(actual.Rows.Count = 2, " & """" & "son: " & """" & " & actual.Rows.Count & " & """" & " registros" & """" & ")")
    '        PrintLine(FileCls, TAB(4), "End Sub")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), "'''Una prueba de Buscar")
    '        PrintLine(FileCls, TAB(4), "'''</summary>")
    '        PrintLine(FileCls, TAB(4), "<TestMethod()> _")
    '        PrintLine(FileCls, TAB(4), "Public Sub Buscar" & Me.txtTabla.Text & "Test()")
    '        PrintLine(FileCls, TAB(8), "Dim target As " & Me.txtTabla.Text & "_ent = New " & Me.txtTabla.Text & "_ent ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim Nombre As String = " & """" & "pru" & """" & " ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim actual As DataTable")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.Truncate()")
    '        PrintLine(FileCls, TAB(8), "target.InsertOne()")
    '        PrintLine(FileCls, TAB(8), "target.Cargar()")
    '        PrintLine(FileCls, TAB(8), "target.Insertar()")
    '        PrintLine(FileCls, TAB(8), "target.id_" & Me.txtTabla.Text & " = 3")
    '        PrintLine(FileCls, TAB(8), "target.nombre_" & Me.txtTabla.Text & " = " & """" & "prueba" & """")
    '        PrintLine(FileCls, TAB(8), "target.Guardar()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.Insertar()")
    '        PrintLine(FileCls, TAB(8), "target.id_" & Me.txtTabla.Text & " = 4")
    '        PrintLine(FileCls, TAB(8), "target.nombre_" & Me.txtTabla.Text & " = " & """" & "prudente" & """")
    '        PrintLine(FileCls, TAB(8), "target.Guardar()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "actual = target.Buscar(Nombre) ")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Assert.IsTrue(actual.Rows.Count = 2, " & """" & "son: " & """" & " & actual.Rows.Count & " & """" & " registros" & """" & ")")
    '        PrintLine(FileCls, TAB(4), "End Sub")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(4), "'''<summary>")
    '        PrintLine(FileCls, TAB(4), "'''Una prueba de Borrar")
    '        PrintLine(FileCls, TAB(4), "'''</summary>")
    '        PrintLine(FileCls, TAB(4), "<TestMethod()> _")
    '        PrintLine(FileCls, TAB(4), "Public Sub Borrar" & Me.txtTabla.Text & "Test()")
    '        PrintLine(FileCls, TAB(8), "Dim target As " & Me.txtTabla.Text & "_ent = New " & Me.txtTabla.Text & "_ent ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim id_" & Me.txtTabla.Text & " As Integer = 1 ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
    '        PrintLine(FileCls, TAB(8), "Dim actual As DataTable")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "target.Truncate()")
    '        PrintLine(FileCls, TAB(8), "target.InsertOne()")
    '        PrintLine(FileCls, TAB(8), "actual = target.Borrar(id_" & Me.txtTabla.Text & ") ")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Dim dt As DataTable = New DataTable")
    '        PrintLine(FileCls, TAB(8), "dt = target.GetOne(id_" & Me.txtTabla.Text & ") ")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(8), "Assert.IsTrue(dt.Rows.Count = 0, " & """" & "son: " & """" & " & dt.Rows.Count & " & """" & " registros" & """" & ")")
    '        PrintLine(FileCls, TAB(4), "End Sub")
    '        PrintLine(FileCls, TAB(0), "End Class")

    '        ' Cierro el PathCls de versión
    '        FileClose(FileCls)
    '    End If


    '    '*****************************************************
    '    '*****************************************************
    '    'DEFINICION DEL CODIGO PARA LAS CLASES DE LA POCKET PC
    '    '*****************************************************
    '    '*****************************************************
    '    If Me.chkPocket.Checked Then

    '        If Mid$(PathClsCe, Len(PathClsCe) - 3, 3) <> ".vb" Then
    '            PathClsCe = Me.txtPath.Text & "\ce_" & PathClsCe & ".vb"
    '        Else
    '            PathClsCe = Me.txtPath.Text & "\ce_" & PathClsCe
    '        End If

    '        FileOpen(FileCls, PathClsCe, OpenMode.Output)

    '        'importo las referencias
    '        PrintLine(FileCls, TAB(0), "Imports System.Data")
    '        PrintLine(FileCls, TAB(0), "Imports System.Data.SqlServerCe")
    '        PrintLine(FileCls, "")

    '        'defino la clase
    '        PrintLine(FileCls, "Public Class " & clase)
    '        PrintLine(FileCls, "")

    '        'defino las variables
    '        PrintLine(FileCls, TAB(5), "'defino las variables")
    '        PrintLine(FileCls, TAB(5), "Private dt As DataTable")
    '        PrintLine(FileCls, TAB(5), "Private dr As DataRow")
    '        PrintLine(FileCls, TAB(5), "Private da As SqlCeDataAdapter")
    '        PrintLine(FileCls, TAB(5), "Private cnn As New Conexion")
    '        PrintLine(FileCls, TAB(5), "Dim ocnn As SqlCeConnection")
    '        PrintLine(FileCls, "")

    '        'defino las propiedades y su variable

    '        PrintLine(FileCls, TAB(5), "'defino las propiedades")
    '        For Each reg In arrEstructura
    '            If reg.Orden = 1 Then
    '                PrintLine(FileCls, TAB(5), "Private _" & reg.nombre & " As " & reg.tiposql)
    '                PrintLine(FileCls, TAB(5), "Public ReadOnly Property " & reg.nombre & "() As " & reg.tiposql)
    '                PrintLine(FileCls, TAB(9), "Get")
    '                PrintLine(FileCls, TAB(13), "_" & reg.nombre & " = dr(" & """" & reg.nombre & """" & ")")
    '                PrintLine(FileCls, TAB(13), "Return _" & reg.nombre)
    '                PrintLine(FileCls, TAB(9), "End Get")
    '                PrintLine(FileCls, TAB(5), "end property")
    '                PrintLine(FileCls, "")
    '            Else
    '                ' Creo la variable local para usarse con la propiedad
    '                PrintLine(FileCls, TAB(5), "Private _" & reg.nombre & " As " & reg.tiposql)

    '                ' Creo la cabecera de la propiedad
    '                PrintLine(FileCls, TAB(5), "Public Property " & reg.nombre & "() As " & reg.tiposql)

    '                ' Creo la cabecera del get de la propiedad
    '                PrintLine(FileCls, TAB(9), "Get")

    '                PrintLine(FileCls, TAB(13), "_" & reg.nombre & " = dr(" & """" & reg.nombre & """" & ")")
    '                ' Creo el cuerpo del get de la propiedad
    '                PrintLine(FileCls, TAB(13), "Return _" & reg.nombre)

    '                ' Creo el pie del get de la propiedad
    '                PrintLine(FileCls, TAB(9), "End Get")

    '                ' Creo una línea divisoria (espacio en blanco)
    '                '' PrintLine(FileCls, "")

    '                ' Creo la cabecera del set de la propiedad
    '                PrintLine(FileCls, TAB(9), "Set(ByVal Value As " & reg.tiposql & ")")

    '                ' Creo el cuerpo del set de la propiedad
    '                PrintLine(FileCls, TAB(13), "dr(" & """" & reg.nombre & """" & ") = Value")

    '                ' Creo el pie del set de la propiedad
    '                PrintLine(FileCls, TAB(9), "End Set")

    '                ' Creo el pie de la propiedad
    '                PrintLine(FileCls, TAB(5), "end property")

    '                ' Creo una línea divisoria (espacio en blanco)
    '                PrintLine(FileCls, "")
    '            End If
    '        Next

    '        'Public Sub Insertar()

    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(5), "'procedimiento insertar")
    '        Print(FileCls, TAB(5), "Public Sub Insertar(")
    '        Dim Contador3 As Integer = 1
    '        For Each reg In arrEstructura
    '            If reg.Orden = 1 Then
    '            ElseIf Contador3 = NumeroFilas Then
    '                Print(FileCls, "ByVal " & reg.nombre & " As " & reg.tiposql & ")")
    '            Else
    '                Print(FileCls, "ByVal " & reg.nombre & " As " & reg.tiposql & ", ")
    '            End If
    '            Contador3 = Contador3 + 1
    '        Next
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '        PrintLine(FileCls, TAB(9), "da.MissingSchemaAction = MissingSchemaAction.AddWithKey")
    '        PrintLine(FileCls, TAB(9), "da.Fill(dt)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Me.AsignarTipos()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "ocnn = cnn.Coneccion")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Dim ocmd As SqlCeCommand = ocnn.CreateCommand")
    '        PrintLine(FileCls, TAB(9), "ocmd.CommandText = " & """" & "INSERT INTO " & txtTabla.Text.Trim & "(" & """" & " & _")

    '        Print(FileCls, TAB(9), """")
    '        Dim Contador2 As Integer = 1
    '        For Each reg In arrEstructura
    '            If reg.Orden = 1 Then
    '            ElseIf Contador2 = NumeroFilas Then
    '                Print(FileCls, reg.nombre & ") VALUES (")
    '                For i As Integer = 1 To NumeroFilas - 1
    '                    If i = NumeroFilas - 1 Then
    '                        Print(FileCls, "?)" & """")
    '                    Else
    '                        Print(FileCls, "?,")
    '                    End If

    '                Next
    '            Else
    '                Print(FileCls, reg.nombre & ", ")
    '            End If
    '            Contador2 = Contador2 + 1
    '        Next

    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        For Each reg In arrEstructura
    '            If reg.Orden = 1 Then
    '                idTabla = reg.nombre
    '                idParametro = "" & reg.nombre
    '            Else
    '                PrintLine(FileCls, TAB(9), "ocmd.Parameters.Add(New SqlCeParameter(" & """" & "" & reg.nombre & """" & ", SqlDbType." & reg.tipo & ", " & reg.longitud & "))")
    '                PrintLine(FileCls, TAB(9), "ocmd.Parameters(" & """" & "" & reg.nombre & """" & ").Value = " & reg.nombre)
    '            End If
    '        Next
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "ocnn.Open()")
    '        PrintLine(FileCls, TAB(9), "ocmd.ExecuteNonQuery()")
    '        PrintLine(FileCls, TAB(9), "ocnn.Close()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "dr = dt.NewRow()")
    '        For Each reg In arrEstructura
    '            If reg.Orden = 1 Then

    '            Else
    '                PrintLine(FileCls, TAB(9), "dr(" & """" & reg.nombre & """" & ") = " & reg.nombre)
    '            End If
    '        Next
    '        PrintLine(FileCls, TAB(9), "dt.Rows.Add(dr)")
    '        PrintLine(FileCls, TAB(9), "CrearComandoUpdate()")
    '        PrintLine(FileCls, TAB(5), "End Sub")
    '        PrintLine(FileCls, "")

    '        'Public Sub Modificar

    '        PrintLine(FileCls, TAB(5), "'procedimiento modificar")
    '        PrintLine(FileCls, TAB(5), "Public Sub Modificar(ByVal id_" & txtTabla.Text.Trim & " As Integer)")
    '        PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '        PrintLine(FileCls, TAB(9), "da = New SqlCeDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & " WHERE " & idTabla & " = " & """" & " & _")
    '        PrintLine(FileCls, TAB(9), "id_" & txtTabla.Text.Trim & ", cnn.Coneccion)")
    '        PrintLine(FileCls, TAB(9), "da.Fill(dt)")
    '        PrintLine(FileCls, TAB(9), "Me.AsignarTipos()")
    '        PrintLine(FileCls, TAB(9), "If dt.Rows.Count = 0 Then")
    '        PrintLine(FileCls, TAB(13), "Throw New Exception(" & """" & "No se a encontrado el Registro" & """" & ")")
    '        PrintLine(FileCls, TAB(9), "Else")
    '        PrintLine(FileCls, TAB(13), "dr = dt.Rows(0)")
    '        PrintLine(FileCls, TAB(9), "End If")
    '        PrintLine(FileCls, TAB(9), "CrearComandoUpdate()")
    '        PrintLine(FileCls, TAB(5), "End Sub")
    '        PrintLine(FileCls, "")

    '        'Public Sub Borrar

    '        PrintLine(FileCls, TAB(5), "'procedimiento borrar")
    '        PrintLine(FileCls, TAB(5), "Public Sub Borrar(ByVal id_" & txtTabla.Text.Trim & " As Integer)")
    '        PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '        PrintLine(FileCls, TAB(9), "da = New SqlCeDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & " WHERE " & idTabla & " = " & """" & " & _")
    '        PrintLine(FileCls, TAB(9), "id_" & txtTabla.Text.Trim & ", cnn.Coneccion)")
    '        PrintLine(FileCls, TAB(9), "da.Fill(dt)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Me.AsignarTipos()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "If dt.Rows.Count = 0 Then")
    '        PrintLine(FileCls, TAB(13), "MessageBox.Show(" & """" & "No se a encontrado el Registro" & """" & ")")
    '        PrintLine(FileCls, TAB(13), "Exit Sub")
    '        PrintLine(FileCls, TAB(9), "Else")
    '        PrintLine(FileCls, TAB(13), "dr = dt.Rows(0)")
    '        PrintLine(FileCls, TAB(9), "End If")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "If MessageBox.Show(" & """" & "Desea Realmente Eliminar el Registro " & """" & ", " & """" & "BORRAR" & """" & ", _")
    '        PrintLine(FileCls, TAB(13), "MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) _")
    '        PrintLine(FileCls, TAB(13), "= DialogResult.No Then")
    '        PrintLine(FileCls, TAB(15), "Exit Sub")
    '        PrintLine(FileCls, TAB(9), "End If")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Try")
    '        PrintLine(FileCls, TAB(13), "dt.Rows(0).Delete()")
    '        PrintLine(FileCls, TAB(13), "CrearComandoUpdate()")
    '        PrintLine(FileCls, TAB(13), "Guardar()")
    '        PrintLine(FileCls, TAB(9), "Catch ex As Exception")
    '        PrintLine(FileCls, TAB(13), "If Err.Number = 5 Then")
    '        PrintLine(FileCls, TAB(15), "MessageBox.Show(ex.Message)")
    '        PrintLine(FileCls, TAB(13), "End If")
    '        PrintLine(FileCls, TAB(9), "End Try")
    '        PrintLine(FileCls, TAB(5), "End Sub")
    '        PrintLine(FileCls, "")

    '        'Private Sub AsignarTipos()

    '        PrintLine(FileCls, TAB(5), "'asigno el tipo de datos a los parametros")
    '        PrintLine(FileCls, TAB(5), "Private Sub AsignarTipos()")
    '        PrintLine(FileCls, TAB(9), "For Each dc As DataColumn In dt.Columns")
    '        PrintLine(FileCls, TAB(13), "Select Case dc.ColumnName")
    '        For Each reg In arrEstructura
    '            PrintLine(FileCls, TAB(16), "Case " & """" & reg.nombre & """")
    '            PrintLine(FileCls, TAB(19), "dc.DataType = Type.GetType(" & """" & "System." & reg.tiposql & """" & ")")
    '        Next
    '        PrintLine(FileCls, TAB(13), "End Select")
    '        PrintLine(FileCls, TAB(9), "Next")
    '        PrintLine(FileCls, TAB(5), "End Sub")
    '        PrintLine(FileCls, "")

    '        'Private Sub CrearComandoUpdate()

    '        PrintLine(FileCls, TAB(5), "'creo el commandbuilder")
    '        PrintLine(FileCls, TAB(5), "Private Sub CrearComandoUpdate()")
    '        PrintLine(FileCls, TAB(9), "Dim cmd As New SqlCeCommandBuilder(da)")
    '        PrintLine(FileCls, TAB(9), "da.UpdateCommand = cmd.GetUpdateCommand")
    '        PrintLine(FileCls, TAB(5), "End Sub")
    '        PrintLine(FileCls, "")

    '        'Public Sub Cancelar()

    '        PrintLine(FileCls, TAB(5), "'cancelo los cambios del datatable")
    '        PrintLine(FileCls, TAB(5), "Public Sub Cancelar()")
    '        PrintLine(FileCls, TAB(9), "dt.Clear()")
    '        PrintLine(FileCls, TAB(5), "End Sub")
    '        PrintLine(FileCls, "")

    '        'Public Function DataTable()

    '        PrintLine(FileCls, TAB(5), "'asigno el datatable")
    '        PrintLine(FileCls, TAB(5), "Public Function DataTable() As DataTable")
    '        PrintLine(FileCls, TAB(9), "Return dt")
    '        PrintLine(FileCls, TAB(5), "End Function")
    '        PrintLine(FileCls, "")

    '        'Public Sub Guardar()

    '        PrintLine(FileCls, TAB(5), "'actualizo la base de datos")
    '        PrintLine(FileCls, TAB(5), "Public Sub Guardar()")
    '        PrintLine(FileCls, TAB(9), "da.Update(dt)")
    '        PrintLine(FileCls, TAB(5), "End Sub")
    '        PrintLine(FileCls, "")

    '        'Public Function Cargar dt()

    '        PrintLine(FileCls, TAB(5), "'funcion que carga el datatable")
    '        PrintLine(FileCls, TAB(5), "Public Function Cargar() As DataTable")
    '        PrintLine(FileCls, TAB(9), "dt = New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '        PrintLine(FileCls, TAB(9), "da = New SqlCeDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & " WHERE id_" & txtTabla.Text.Trim & " = 0" & """" & ", cnn.Coneccion)")
    '        PrintLine(FileCls, TAB(9), "da.MissingSchemaAction = MissingSchemaAction.AddWithKey")
    '        PrintLine(FileCls, TAB(9), "da.Fill(dt)")
    '        PrintLine(FileCls, TAB(9), "Return dt")
    '        PrintLine(FileCls, TAB(5), "End Function")
    '        PrintLine(FileCls, "")

    '        'Public Function Buscar

    '        PrintLine(FileCls, TAB(5), "'funcion de busqueda")
    '        PrintLine(FileCls, TAB(5), "Public Function Buscar(ByVal Nombre As String) As DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim oda As New SqlCeDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & "" & """" & ", cnn.Coneccion)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Return odt")
    '        PrintLine(FileCls, TAB(5), "End Function")
    '        PrintLine(FileCls, "")

    '        'Public Function ConsultarTodo()

    '        PrintLine(FileCls, TAB(5), "'funcion de consulta")
    '        PrintLine(FileCls, TAB(5), "Public Function ConsultarTodo() As DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim oda As New SqlCeDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & "" & """" & ", cnn.Coneccion)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Return odt")
    '        PrintLine(FileCls, TAB(5), "End Function")
    '        PrintLine(FileCls, "")

    '        'Public Function GetCmb()

    '        PrintLine(FileCls, TAB(5), "'funcion para llenar el combo")
    '        PrintLine(FileCls, TAB(5), "Public Function GetCmb() As DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim oda As New SqlCeDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & "" & """" & ", cnn.Coneccion)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Return odt")
    '        PrintLine(FileCls, TAB(5), "End Function")
    '        PrintLine(FileCls, "")

    '        'public function GetOne()

    '        PrintLine(FileCls, TAB(5), "'funcion que trae un registro poniendo el id")
    '        PrintLine(FileCls, TAB(5), "Public Function GetOne(ByVal id_" & txtTabla.Text.Trim & " As Integer) As DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '        PrintLine(FileCls, TAB(9), "oda = New SqlCeDataAdapter(" & """" & "SELECT * FROM " & txtTabla.Text.Trim & " WHERE " & idTabla & " = " & """" & " & _")
    '        PrintLine(FileCls, TAB(9), "id_" & txtTabla.Text.Trim & ", cnn.Coneccion)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Return odt")
    '        PrintLine(FileCls, TAB(5), "End Function")
    '        PrintLine(FileCls, "")

    '        'Public Function Consultar con oda

    '        PrintLine(FileCls, TAB(5), "'ejemplo de consulta con parametros")
    '        PrintLine(FileCls, TAB(5), "Public Function Consultar(ByVal id_" & txtTabla.Text.Trim & " As Integer) As DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim oda As New SqlCeDataAdapter(" & """" & "Ejemplo_" & txtTabla.Text.Trim & """" & ", cnn.Coneccion)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '        PrintLine(FileCls, TAB(9), "Return odt")
    '        PrintLine(FileCls, TAB(5), "End Function")
    '        PrintLine(FileCls, "")

    '        'Public Function ConsultarDecimal

    '        PrintLine(FileCls, TAB(5), "'ejemplo de consulta con parametros trayendo un decimal")
    '        PrintLine(FileCls, TAB(5), "Public Function ConsultarDecimal(ByVal id_" & txtTabla.Text.Trim & " As Integer) As Decimal")
    '        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim oda As New SqlCeDataAdapter(" & """" & "Ejemplo_" & txtTabla.Text.Trim & "Decimal" & """" & ", cnn.Coneccion)")
    '        PrintLine(FileCls, TAB(9), "Dim Total As Decimal")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '        PrintLine(FileCls, TAB(9), "Total = odt.Rows(0).Item(" & """" & "Total" & """" & ").ToString")
    '        PrintLine(FileCls, TAB(9), "Return Total")
    '        PrintLine(FileCls, TAB(5), "End Function")
    '        PrintLine(FileCls, "")

    '        'Public Function ControlarSiExiste

    '        PrintLine(FileCls, TAB(5), "'ejemplo de consulta con parametros trayendo un boolean")
    '        PrintLine(FileCls, TAB(5), "Public Function ControlarSiExiste(ByVal id_" & txtTabla.Text.Trim & " As Integer) As Boolean")
    '        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim oda As New SqlCeDataAdapter(" & """" & "cop_" & txtTabla.Text.Trim & "_Check" & """" & ", cnn.Coneccion)")
    '        PrintLine(FileCls, TAB(9), "Dim Total As Integer")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "oda.Fill(odt)")
    '        PrintLine(FileCls, TAB(9), "Total = odt.Rows(0).Item(" & """" & "Total" & """" & ").ToString")
    '        PrintLine(FileCls, TAB(9), "If Total = 0 Then")
    '        PrintLine(FileCls, TAB(13), "Return True  'NO EXISTE")
    '        PrintLine(FileCls, TAB(9), "Else")
    '        PrintLine(FileCls, TAB(13), "Return False 'SI EXISTE")
    '        PrintLine(FileCls, TAB(9), "End If")
    '        PrintLine(FileCls, TAB(5), "End Function")
    '        PrintLine(FileCls, "")

    '        'borra toda la tabla
    '        PrintLine(FileCls, TAB(5), "'borra todos los datos de la tabla")
    '        PrintLine(FileCls, TAB(5), "Public Sub BorrarTodo()")
    '        PrintLine(FileCls, TAB(9), "Dim oConexion As New Conexion")
    '        PrintLine(FileCls, TAB(9), "Dim Command As SqlCeCommand = New SqlCeCommand()")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Command.Connection = oConexion.Coneccion")
    '        PrintLine(FileCls, TAB(9), "Command.CommandText = ""DELETE FROM " & Me.txtTabla.Text & """")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "oConexion.Abrir()")
    '        PrintLine(FileCls, TAB(9), "Command.ExecuteNonQuery()")
    '        PrintLine(FileCls, TAB(9), "oConexion.Cerrar()")
    '        PrintLine(FileCls, TAB(5), "End Sub")
    '        PrintLine(FileCls, "")

    '        'consulta el archivo txt para importarlo
    '        PrintLine(FileCls, TAB(5), "'importa los datos de una campaña desde el txt")
    '        PrintLine(FileCls, TAB(5), "Public Function Cargar_Archivo() As DataTable")
    '        PrintLine(FileCls, TAB(9), "Dim odt As New DataTable(" & """" & txtTabla.Text.Trim & """" & ")")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "'Dim oda As New System.Data.OleDb.OleDbDataAdapter( _")
    '        PrintLine(FileCls, TAB(9), "'""SELECT * FROM " & Me.txtTabla.Text & ".txt"", cnntxt.Coneccion)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "'oda.Fill(odt)")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(9), "Return odt")
    '        PrintLine(FileCls, TAB(5), "End Function")
    '        PrintLine(FileCls, "")

    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, TAB(13), "'********************************************")
    '        PrintLine(FileCls, TAB(13), "'    CODIGO AGREGADO A LA CLASE ORIGINAL")
    '        PrintLine(FileCls, TAB(13), "'********************************************")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        PrintLine(FileCls, "")
    '        ' Comienzo a generar la clase
    '        PrintLine(FileCls, "End Class")

    '        ' Cierro el PathCls de versión
    '        FileClose(FileCls)

    '    End If






















    '    If Mid$(PathInsert, Len(PathInsert) - 3, 3) <> ".sql" Then
    '        PathInsert = Me.txtPath.Text & "\sp" & Me.txtTabla.Text & ".sql"
    '    Else
    '        PathInsert = Me.txtPath.Text & "\" & PathInsert
    '    End If

    '    ' Defino variables
    '    Dim FileInsert As Integer = FreeFile()
    '    Dim regInsert As regSistemaTabla

    '    ' Abro un PathCls de texto (si el mismo existía se reempleza)
    '    FileOpen(FileInsert, PathInsert, OpenMode.Output)
    '    Dim Prefijo As String = "cop_"


    '    '*********************************************
    '    '
    '    'DEFINICION DEL CODIGO PARA LOS SP
    '    '
    '    '*********************************************

    '    PrintLine(FileInsert, TAB(0), "USE [" & Me.lstBaseDato.Text.Trim & "]")
    '    PrintLine(FileInsert, TAB(0), "GO")
    '    PrintLine(FileInsert, "")

    '    Dim Contador As Integer = 1

    '    'sp insert
    '    If Me.chkInsert.Checked Then

    '        PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_Insert]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '        PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Insert]")
    '        PrintLine(FileInsert, TAB(0), "GO")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Insert]")

    '        For Each regInsert In arrEstructura
    '            'If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '            'PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo & "  output,")
    '            If Contador = NumeroFilas Then
    '                PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo & "  " & regInsert.sptamaño)
    '            Else
    '                'If regInsert.nombre = "operacion" Or regInsert.nombre = "sincronizado" Then
    '                '    'no imprimo
    '                'Else
    '                PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo & "  " & regInsert.sptamaño & ",")
    '                'End If
    '            End If
    '            Contador = Contador + 1
    '        Next
    '        PrintLine(FileInsert, TAB(0), "AS")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "SET @id_" & Me.txtTabla.Text & " = (SELECT MAX(id_" & Me.txtTabla.Text & ") + 1 FROM " & Me.txtTabla.Text & ")")
    '        PrintLine(FileInsert, TAB(0), "IF @id_" & Me.txtTabla.Text & " is null BEGIN SET @id_" & Me.txtTabla.Text & " = 1 END")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "INSERT INTO [dbo].[" & Me.txtTabla.Text & "]")
    '        PrintLine(FileInsert, TAB(0), "(")
    '        Contador = 1
    '        For Each regInsert In arrEstructura
    '            If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '            ElseIf Contador = NumeroFilas Then
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "]")
    '            Else
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "],")
    '            End If
    '            Contador = Contador + 1
    '        Next
    '        PrintLine(FileInsert, ")")
    '        PrintLine(FileInsert, TAB(0), "VALUES")
    '        PrintLine(FileInsert, TAB(0), "(")
    '        Contador = 1
    '        For Each regInsert In arrEstructura
    '            If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '            ElseIf Contador = NumeroFilas Then
    '                PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre)
    '            Else
    '                If regInsert.nombre = "operacion" Then
    '                    PrintLine(FileInsert, TAB(5), "dbo.DefineTipoOperacion('', 'I', 0),")
    '                ElseIf regInsert.nombre = "sincronizado" Then
    '                    PrintLine(FileInsert, TAB(5), "0,")
    '                Else
    '                    PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & ",")
    '                End If

    '            End If
    '            Contador = Contador + 1
    '        Next
    '        PrintLine(FileInsert, ")")
    '        PrintLine(FileInsert, TAB(0), "GO")
    '        PrintLine(FileInsert, "")
    '    End If

    '    'sp delete
    '    If Me.chkDelete.Checked Then

    '        PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_Delete]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '        PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Delete]")
    '        PrintLine(FileInsert, TAB(0), "GO")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Delete]")

    '        For Each regInsert In arrEstructura
    '            If regInsert.Orden = 1 Then
    '                PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo)
    '                Exit For
    '            End If
    '        Next
    '        PrintLine(FileInsert, TAB(0), "AS")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "DECLARE @sincronizado bit, @operacion char(1)")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "SELECT @sincronizado = sincronizado, @operacion = operacion FROM " & Me.txtTabla.Text & " WHERE id_" & Me.txtTabla.Text & " = @id_" & Me.txtTabla.Text)
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "if @sincronizado = 0 and @operacion = 'I'")

    '        PrintLine(FileInsert, TAB(5), "DELETE FROM [dbo].[" & Me.txtTabla.Text & "]")
    '        PrintLine(FileInsert, TAB(5), "WHERE")
    '        For Each regInsert In arrEstructura
    '            If regInsert.Orden = 1 Then
    '                PrintLine(FileInsert, TAB(9), "[" & regInsert.nombre & "]  =  @" & regInsert.nombre)
    '                Exit For
    '            End If
    '        Next
    '        PrintLine(FileInsert, TAB(0), "Else")
    '        PrintLine(FileInsert, TAB(5), "UPDATE [dbo].[" & Me.txtTabla.Text & "]")
    '        PrintLine(FileInsert, TAB(5), "SET")
    '        PrintLine(FileInsert, TAB(9), "operacion = dbo.DefineTipoOperacion(operacion, 'D', sincronizado),")
    '        PrintLine(FileInsert, TAB(9), "sincronizado = 0")
    '        PrintLine(FileInsert, TAB(5), "WHERE")
    '        For Each regInsert In arrEstructura
    '            If regInsert.Orden = 1 Then
    '                PrintLine(FileInsert, TAB(9), "[" & regInsert.nombre & "]  =  @" & regInsert.nombre)
    '                Exit For
    '            End If
    '        Next
    '        PrintLine(FileInsert, TAB(0), "GO")
    '        PrintLine(FileInsert, "")
    '    End If

    '    'sp GetAll
    '    If Me.chkGetAll.Checked Then

    '        PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_GetAll]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '        PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_GetAll]")
    '        PrintLine(FileInsert, TAB(0), "GO")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_GetAll]")
    '        PrintLine(FileInsert, TAB(0), "AS")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "SELECT")

    '        Contador = 1
    '        For Each regInsert In arrEstructura
    '            If Contador = NumeroFilas - 3 Then
    '                If regInsert.nombre = "sincronizado" Or regInsert.nombre = "operacion" Or regInsert.nombre = "id_cliente_maestro" Then
    '                    'next row
    '                ElseIf regInsert.tipo = "varchar" Then
    '                    PrintLine(FileInsert, TAB(5), "RTRIM(" & regInsert.nombre & ") AS " & regInsert.nombre.ToUpper)
    '                Else
    '                    PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] AS " & regInsert.nombre.ToUpper)
    '                End If
    '            Else
    '                If regInsert.nombre = "sincronizado" Or regInsert.nombre = "operacion" Or regInsert.nombre = "id_cliente_maestro" Then
    '                    'next row
    '                ElseIf regInsert.tipo = "varchar" Then
    '                    PrintLine(FileInsert, TAB(5), "RTRIM(" & regInsert.nombre & ") AS " & regInsert.nombre.ToUpper & " ,")
    '                Else
    '                    PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] AS " & regInsert.nombre.ToUpper & " ,")
    '                End If
    '            End If
    '            Contador = Contador + 1
    '        Next

    '        PrintLine(FileInsert, TAB(0), "FROM")
    '        PrintLine(FileInsert, TAB(5), "[dbo].[" & Me.txtTabla.Text & "]")
    '        PrintLine(FileInsert, TAB(0), "WHERE")
    '        PrintLine(FileInsert, TAB(5), "[operacion] <> 'D'")
    '        PrintLine(FileInsert, TAB(0), "ORDER BY")

    '        Contador = 0
    '        For Each regInsert In arrEstructura
    '            If Contador = 1 Then
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "]")
    '                Exit For
    '            End If
    '            Contador = Contador + 1
    '        Next

    '        PrintLine(FileInsert, TAB(0), "GO")
    '        PrintLine(FileInsert, "")
    '    End If

    '    'sp GetCmb
    '    If Me.chkGetCmb.Checked Then

    '        PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_GetCmb]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '        PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_GetCmb]")
    '        PrintLine(FileInsert, TAB(0), "GO")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_GetCmb]")
    '        PrintLine(FileInsert, TAB(0), "AS")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "SELECT")

    '        Contador = 1
    '        For Each regInsert In arrEstructura
    '            If Contador = NumeroFilas Then
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "]")
    '            Else
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "],")
    '            End If
    '            Contador = Contador + 1
    '        Next

    '        PrintLine(FileInsert, TAB(0), "FROM")
    '        PrintLine(FileInsert, TAB(5), "[dbo].[" & Me.txtTabla.Text & "]")
    '        PrintLine(FileInsert, TAB(0), "WHERE")
    '        PrintLine(FileInsert, TAB(5), "[operacion] <> 'D'")
    '        PrintLine(FileInsert, TAB(0), "ORDER BY")

    '        Contador = 0
    '        For Each regInsert In arrEstructura
    '            If Contador = 1 Then
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "]")
    '                Exit For
    '            End If
    '            Contador = Contador + 1
    '        Next

    '        PrintLine(FileInsert, TAB(0), "GO")
    '        PrintLine(FileInsert, "")
    '    End If

    '    'sp update
    '    If Me.chkUpdate.Checked Then

    '        PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_Update]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '        PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Update]")
    '        PrintLine(FileInsert, TAB(0), "GO")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Update]")

    '        Contador = 1
    '        For Each regInsert In arrEstructura
    '            If regInsert.nombre = "operacion" Or regInsert.nombre = "sincronizado" Then
    '                'no imprimir
    '            Else
    '                If Contador = NumeroFilas Then
    '                    PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo & "  " & regInsert.sptamaño)
    '                Else
    '                    PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo & "  " & regInsert.sptamaño & ",")
    '                End If
    '            End If
    '            Contador = Contador + 1
    '        Next
    '        PrintLine(FileInsert, TAB(0), "AS")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "UPDATE [dbo].[" & Me.txtTabla.Text & "] SET")
    '        Contador = 1
    '        For Each regInsert In arrEstructura
    '            If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '                'paso al siguiente
    '            ElseIf regInsert.nombre = "operacion" Then
    '                PrintLine(FileInsert, TAB(5), "[operacion] = dbo.DefineTipoOperacion(operacion, 'U', sincronizado),")
    '            ElseIf regInsert.nombre = "sincronizado" Then
    '                PrintLine(FileInsert, TAB(5), "[sincronizado] = 0,")
    '            ElseIf Contador = NumeroFilas Then
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] = @" & regInsert.nombre)
    '            Else
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] = @" & regInsert.nombre & ",")
    '            End If
    '            Contador = Contador + 1
    '        Next
    '        PrintLine(FileInsert, TAB(0), "WHERE")
    '        For Each regInsert In arrEstructura
    '            If regInsert.Orden = 1 Then
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "]  =  @" & regInsert.nombre)
    '                Exit For
    '            End If
    '        Next
    '        PrintLine(FileInsert, TAB(0), "GO")
    '        PrintLine(FileInsert, "")
    '    End If

    '    'sp exist
    '    If Me.chkExist.Checked Then

    '        PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_Exist]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '        PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Exist]")
    '        PrintLine(FileInsert, TAB(0), "GO")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Exist]")
    '        Contador = 1
    '        For Each regInsert In arrEstructura
    '            If regInsert.Orden = 1 Then
    '                'next row
    '            ElseIf regInsert.nombre = "operacion" Or regInsert.nombre = "sincronizado" Or regInsert.nombre = "id_cliente_maestro" Then
    '                'next row
    '            ElseIf Contador = NumeroFilas - 3 Then
    '                PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo & "  " & regInsert.sptamaño)
    '            Else
    '                PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo & "  " & regInsert.sptamaño & ",")
    '            End If
    '            Contador = Contador + 1
    '        Next
    '        PrintLine(FileInsert, TAB(0), "AS")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "DECLARE @total int")
    '        PrintLine(FileInsert, TAB(0), "SET @total = 0")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "SELECT")
    '        For Each regInsert In arrEstructura
    '            If regInsert.Orden = 1 Then
    '                PrintLine(FileInsert, TAB(5), "@total = " & regInsert.nombre)
    '                Exit For
    '            End If
    '        Next
    '        PrintLine(FileInsert, TAB(0), "FROM")
    '        PrintLine(FileInsert, TAB(5), "[dbo].[" & Me.txtTabla.Text & "]")
    '        PrintLine(FileInsert, TAB(0), "WHERE")
    '        Contador = 1
    '        For Each regInsert In arrEstructura
    '            If regInsert.Orden = 1 Then
    '                'next row
    '            ElseIf regInsert.nombre = "operacion" Or regInsert.nombre = "sincronizado" Or regInsert.nombre = "id_cliente_maestro" Then
    '                'next row
    '            ElseIf Contador = NumeroFilas Then
    '                'PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] = @" & regInsert.nombre)
    '                'PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] = @" & regInsert.nombre)
    '            Else
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] = @" & regInsert.nombre & " AND")
    '            End If
    '            Contador = Contador + 1
    '        Next
    '        PrintLine(FileInsert, TAB(5), "[operacion] <> 'D'")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "IF @total IS NULL")
    '        PrintLine(FileInsert, TAB(0), "BEGIN")
    '        PrintLine(FileInsert, TAB(5), "SET @total=0")
    '        PrintLine(FileInsert, TAB(0), "END")
    '        PrintLine(FileInsert, TAB(0), "SELECT @total AS Total")
    '        PrintLine(FileInsert, TAB(0), "GO")
    '        PrintLine(FileInsert, "")
    '    End If

    '    'sp getone
    '    If Me.chkGetOne.Checked Then

    '        PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_GetOne]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '        PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_GetOne]")
    '        PrintLine(FileInsert, TAB(0), "GO")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_GetOne]")
    '        For Each regInsert In arrEstructura
    '            If regInsert.Orden = 1 Then
    '                PrintLine(FileInsert, TAB(5), "@" & regInsert.nombre & "    " & regInsert.tipo)
    '                Exit For
    '            End If
    '        Next
    '        PrintLine(FileInsert, TAB(0), "AS")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "SELECT")

    '        Contador = 1
    '        For Each regInsert In arrEstructura
    '            If Contador = NumeroFilas Then
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] AS " & regInsert.nombre.ToUpper)
    '            Else
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] AS " & regInsert.nombre.ToUpper & " ,")
    '            End If
    '            Contador = Contador + 1
    '        Next
    '        PrintLine(FileInsert, TAB(0), "FROM")
    '        PrintLine(FileInsert, TAB(5), "[dbo].[" & Me.txtTabla.Text & "]")
    '        PrintLine(FileInsert, TAB(0), "WHERE")
    '        For Each regInsert In arrEstructura
    '            If regInsert.Orden = 1 Then
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "]  =  @" & regInsert.nombre & " AND")
    '                Exit For
    '            End If
    '        Next
    '        PrintLine(FileInsert, TAB(5), "[operacion] <> 'D'")
    '        PrintLine(FileInsert, TAB(0), "GO")
    '        PrintLine(FileInsert, "")
    '    End If

    '    'sp find
    '    If Me.chkFind.Checked Then

    '        PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_Find]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '        PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Find]")
    '        PrintLine(FileInsert, TAB(0), "GO")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_Find]")
    '        PrintLine(FileInsert, TAB(5), "@nombre NVARCHAR (30)=NULL")
    '        PrintLine(FileInsert, TAB(0), "AS SET NOCOUNT ON")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "IF @nombre IS NOT NULL")
    '        PrintLine(FileInsert, TAB(0), "BEGIN")
    '        PrintLine(FileInsert, TAB(0), "SELECT @nombre=RTRIM(@nombre)+'%'")
    '        PrintLine(FileInsert, TAB(0), "SELECT")

    '        Contador = 1
    '        For Each regInsert In arrEstructura
    '            If Contador = NumeroFilas - 3 Then
    '                If regInsert.nombre = "sincronizado" Or regInsert.nombre = "operacion" Or regInsert.nombre = "id_cliente_maestro" Then
    '                    'next row
    '                ElseIf regInsert.tipo = "varchar" Then
    '                    PrintLine(FileInsert, TAB(5), "RTRIM(" & regInsert.nombre & ") AS " & regInsert.nombre.ToUpper)
    '                Else
    '                    PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] AS " & regInsert.nombre.ToUpper)
    '                End If
    '            Else
    '                If regInsert.nombre = "sincronizado" Or regInsert.nombre = "operacion" Or regInsert.nombre = "id_cliente_maestro" Then
    '                    'next row
    '                ElseIf regInsert.tipo = "varchar" Then
    '                    PrintLine(FileInsert, TAB(5), "RTRIM(" & regInsert.nombre & ") AS " & regInsert.nombre.ToUpper & " ,")
    '                Else
    '                    PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] AS " & regInsert.nombre.ToUpper & " ,")
    '                End If
    '            End If
    '            Contador = Contador + 1
    '        Next

    '        PrintLine(FileInsert, TAB(0), "FROM")
    '        PrintLine(FileInsert, TAB(5), "[dbo].[" & Me.txtTabla.Text & "]")
    '        PrintLine(FileInsert, TAB(0), "WHERE")
    '        Contador = 0
    '        For Each regInsert In arrEstructura
    '            If Contador = 1 Then
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "] LIKE @nombre+'%' AND")
    '                Exit For
    '            End If
    '            Contador = Contador + 1
    '        Next
    '        PrintLine(FileInsert, TAB(5), "[operacion] <> 'D'")
    '        PrintLine(FileInsert, TAB(0), "ORDER BY")
    '        Contador = 0
    '        For Each regInsert In arrEstructura
    '            If Contador = 1 Then
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "]")
    '                Exit For
    '            End If
    '            Contador = Contador + 1
    '        Next
    '        PrintLine(FileInsert, TAB(0), "END")
    '        PrintLine(FileInsert, TAB(0), "GO")
    '        PrintLine(FileInsert, "")
    '    End If




    '    'insertar el registro ninguno
    '    'sp InsertOne
    '    If Me.chkInsertOne.Checked Then

    '        PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.txtTabla.Text & "_InsertOne]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
    '        PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_InsertOne]")
    '        PrintLine(FileInsert, TAB(0), "GO")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.txtTabla.Text & "_InsertOne]")
    '        PrintLine(FileInsert, TAB(0), "AS")
    '        PrintLine(FileInsert, "")
    '        PrintLine(FileInsert, TAB(0), "INSERT INTO [dbo].[" & Me.txtTabla.Text & "]")
    '        PrintLine(FileInsert, TAB(0), "(")
    '        Contador = 1
    '        For Each regInsert In arrEstructura
    '            If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '            ElseIf Contador = NumeroFilas Then
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "]")
    '            Else
    '                PrintLine(FileInsert, TAB(5), "[" & regInsert.nombre & "],")
    '            End If
    '            Contador = Contador + 1
    '        Next
    '        PrintLine(FileInsert, ")")
    '        PrintLine(FileInsert, TAB(0), "VALUES")
    '        PrintLine(FileInsert, TAB(0), "(")
    '        Contador = 1
    '        For Each regInsert In arrEstructura
    '            If regInsert.indice = 9 Or regInsert.indice = 1 Then
    '            ElseIf Contador = NumeroFilas Then
    '                PrintLine(FileInsert, TAB(5), regInsert.valorinsert)
    '            Else
    '                PrintLine(FileInsert, TAB(5), regInsert.valorinsert & ",")
    '            End If
    '            Contador = Contador + 1
    '        Next
    '        PrintLine(FileInsert, ")")

    '        ' If Me.chkGo.Checked = True Then
    '        PrintLine(FileInsert, TAB(0), "GO")
    '        '  End If

    '        PrintLine(FileInsert, "")
    '    End If







    '    FileClose(FileInsert)


    '    '*********************************************
    '    '
    '    'DEFINICION DEL CODIGO PARA FORMULARIOS ABM
    '    '
    '    '*********************************************

    '    If Mid$(PathFrm, Len(PathFrm) - 3, 3) <> ".vb" Then
    '        PathFrm = Me.txtPath.Text & "\frmAbm" & Me.txtTabla.Text & ".vb"
    '    Else
    '        PathFrm = Me.txtPath.Text & "\" & PathFrm
    '    End If

    '    ' Defino variables
    '    Dim FileFrm As Integer = FreeFile()

    '    ' Abro un PathCls de texto (si el mismo existía se reempleza)
    '    FileOpen(FileInsert, PathFrm, OpenMode.Output)

    '    'definicion de variables

    '    PrintLine(FileFrm, TAB(0), "Public Class frmAbm" & Me.txtTabla.Text)
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(4), "Dim odt As DataTable")
    '    PrintLine(FileFrm, TAB(4), "Dim BanderaConsulta" & Me.txtTabla.Text & " As Integer")
    '    PrintLine(FileFrm, "")

    '    'Private Sub frmAbm_Load

    '    PrintLine(FileFrm, TAB(4), "Private Sub frmAbm" & Me.txtTabla.Text & "_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load")
    '    PrintLine(FileFrm, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnAgregar, " & """" & "Incorporar Nuevo " & Me.txtTabla.Text & """" & ")")
    '    PrintLine(FileFrm, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnBorrar, " & """" & "Borrar un  " & Me.txtTabla.Text & " Existente" & """" & ")")
    '    PrintLine(FileFrm, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnModificar, " & """" & "Modificar  un " & Me.txtTabla.Text & " Existente" & """" & ")")
    '    PrintLine(FileFrm, TAB(8), "'Me.ttGeneral.SetToolTip(Me.btnConsultar, " & """" & "Consultar Datos del  " & Me.txtTabla.Text & " Existente" & """" & ")")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(8), "Dim odt As DataTable")
    '    PrintLine(FileFrm, TAB(8), "odt = o" & Me.txtTabla.Text & ".Cargar()")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(8), "RefrescarGrilla()")
    '    PrintLine(FileFrm, TAB(8), "Me.txtBuscar.Focus()")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.Text = " & """" & Me.txtTabla.Text & """")
    '    PrintLine(FileFrm, TAB(8), "Me.Text = " & """" & Me.txtTabla.Text & """")
    '    PrintLine(FileFrm, TAB(8), "Me.CancelButton = Me.btnSalir")
    '    PrintLine(FileFrm, TAB(8), "Me.BackColor = Color.Teal")
    '    PrintLine(FileFrm, TAB(8), "Me.StartPosition = FormStartPosition.CenterScreen")
    '    PrintLine(FileFrm, TAB(8), "Me.MinimizeBox = False")
    '    PrintLine(FileFrm, TAB(8), "Me.MaximizeBox = False")
    '    PrintLine(FileFrm, TAB(4), "End Sub")
    '    PrintLine(FileFrm, "")

    '    'Public Sub RefrescarGrilla()

    '    PrintLine(FileFrm, TAB(4), "Public Sub RefrescarGrilla()")
    '    PrintLine(FileFrm, TAB(8), "Dim odt As DataTable")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(8), "odt = o" & Me.txtTabla.Text & ".ConsultarTodo()")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.DataSource = odt")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.Columns(0).Visible = False")
    '    PrintLine(FileFrm, TAB(4), "End Sub")
    '    PrintLine(FileFrm, "")

    '    'Private Sub txtBuscar_KeyPress

    '    PrintLine(FileFrm, TAB(4), "Private Sub txtBuscar_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBuscar.KeyPress")
    '    PrintLine(FileFrm, TAB(8), "If e.KeyChar.ToString = vbCr Then")
    '    PrintLine(FileFrm, TAB(12), "Me.btnModificar.Focus()")
    '    PrintLine(FileFrm, TAB(8), "End If")
    '    PrintLine(FileFrm, TAB(4), "End Sub")
    '    PrintLine(FileFrm, "")

    '    'Private Sub txtBuscar_TextChanged

    '    PrintLine(FileFrm, TAB(4), "Private Sub txtBuscar_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged")
    '    PrintLine(FileFrm, TAB(8), "If Me.txtBuscar.Text = " & """""" & " Then")
    '    PrintLine(FileFrm, TAB(12), "Me.txtBuscar.Text = " & """" & " " & """")
    '    PrintLine(FileFrm, TAB(8), "End If")
    '    PrintLine(FileFrm, TAB(8), "odt = o" & Me.txtTabla.Text & ".Buscar(Me.txtBuscar.Text)")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.DataSource = odt")
    '    PrintLine(FileFrm, TAB(4), "End Sub")
    '    PrintLine(FileFrm, "")

    '    'Private Sub dgv1_CurrentCellChanged

    '    PrintLine(FileFrm, TAB(4), "Private Sub dgv1_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv1.CurrentCellChanged")
    '    PrintLine(FileFrm, TAB(8), "Try")
    '    PrintLine(FileFrm, TAB(12), "Me.lblid_pk.Text = Me.dgv1.Item(0, Me.dgv1.CurrentRow.Index).Value")
    '    PrintLine(FileFrm, TAB(8), "Catch ex As Exception")
    '    PrintLine(FileFrm, TAB(8), "End Try")
    '    PrintLine(FileFrm, TAB(4), "End Sub")
    '    PrintLine(FileFrm, "")

    '    'Private Sub Botones_Click

    '    PrintLine(FileFrm, TAB(4), "Private Sub Botones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click, btnAyuda.Click, btnBorrar.Click, btnModificar.Click, btnSalir.Click")
    '    PrintLine(FileFrm, TAB(8), "Dim btnTemp As New Button")
    '    PrintLine(FileFrm, TAB(8), "Dim frmDetalle As New frmDetalle" & Me.txtTabla.Text)
    '    PrintLine(FileFrm, TAB(8), "btnTemp = sender")
    '    PrintLine(FileFrm, TAB(8), "Try")
    '    PrintLine(FileFrm, TAB(12), "Select Case btnTemp.Name")

    '    PrintLine(FileFrm, TAB(16), "Case " & """" & "btnAgregar" & """")
    '    PrintLine(FileFrm, TAB(20), "Bandera" & Me.txtTabla.Text & " = 1")
    '    PrintLine(FileFrm, TAB(20), "Me.AddOwnedForm(frmDetalle)")
    '    PrintLine(FileFrm, TAB(20), "o" & Me.txtTabla.Text & ".Insertar()")
    '    PrintLine(FileFrm, TAB(20), "frmDetalle.CargarCombos()")
    '    PrintLine(FileFrm, TAB(20), "frmDetalle.ShowDialog()")

    '    PrintLine(FileFrm, TAB(16), "Case " & """" & "btnModificar" & """")
    '    PrintLine(FileFrm, TAB(20), "If Not IsNumeric(Me.lblid_pk.Text) Then")
    '    PrintLine(FileFrm, TAB(24), "Exit Sub")
    '    PrintLine(FileFrm, TAB(20), "End If")

    '    '  PrintLine(FileFrm, TAB(16), "If Me.lblid_pk.Text <= 1 Then")
    '    '  PrintLine(FileFrm, TAB(20), "MessageBox.Show(" & """" & "No se Puede Modificar el Registro" & """" & ")")
    '    '  PrintLine(FileFrm, TAB(20), "Exit Sub")
    '    '  PrintLine(FileFrm, TAB(16), "End If")

    '    PrintLine(FileFrm, TAB(20), "Bandera" & Me.txtTabla.Text & " = 2")
    '    PrintLine(FileFrm, TAB(20), "Me.AddOwnedForm(frmDetalle)")
    '    PrintLine(FileFrm, TAB(20), "o" & Me.txtTabla.Text & ".ModificarOld(Me.lblid_pk.Text)")
    '    PrintLine(FileFrm, TAB(20), "frmDetalle.CargarCombos()")
    '    PrintLine(FileFrm, TAB(20), "frmDetalle.ShowDialog()")
    '    PrintLine(FileFrm, TAB(20), "RefrescarGrilla()")

    '    PrintLine(FileFrm, TAB(16), "Case " & """" & "btnBorrar" & """")
    '    PrintLine(FileFrm, TAB(20), "If Not IsNumeric(Me.lblid_pk.Text) Then")
    '    PrintLine(FileFrm, TAB(24), "Exit Sub")
    '    PrintLine(FileFrm, TAB(20), "End If")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(20), "If MessageBox.Show(" & """" & "Desea Realmente Eliminar el Registro " & """" & ", " & """" & "BORRAR" & """" & ", _")
    '    PrintLine(FileFrm, TAB(20), "MessageBoxButtons.YesNo, MessageBoxIcon.Question) _")
    '    PrintLine(FileFrm, TAB(20), "= Windows.Forms.DialogResult.No Then")
    '    PrintLine(FileFrm, TAB(24), "Exit Sub")
    '    PrintLine(FileFrm, TAB(20), "End If")
    '    PrintLine(FileFrm, "")

    '    '   PrintLine(FileFrm, TAB(16), "If Me.lblid_pk.Text <= 1 Then")
    '    '   PrintLine(FileFrm, TAB(20), "MessageBox.Show(" & """" & "No se Puede Borrar el Registro" & """" & ")")
    '    '   PrintLine(FileFrm, TAB(20), "Exit Sub")
    '    '   PrintLine(FileFrm, TAB(16), "End If")

    '    PrintLine(FileFrm, TAB(20), "o" & Me.txtTabla.Text & ".Borrar(Me.lblid_pk.Text)")
    '    PrintLine(FileFrm, TAB(20), "RefrescarGrilla()")

    '    PrintLine(FileFrm, TAB(16), "Case " & """" & "btnAyuda" & """")
    '    PrintLine(FileFrm, TAB(20), "'Process.Start(PathAyuda + " & """" & "frmAbm" & Me.txtTabla.Text & ".avi" & """" & ")")
    '    PrintLine(FileFrm, TAB(16), "Case " & """" & "btnSalir" & """")
    '    PrintLine(FileFrm, TAB(20), "Me.Close()")
    '    PrintLine(FileFrm, TAB(12), "End Select")
    '    PrintLine(FileFrm, TAB(12), "Me.txtBuscar.Text = """"")
    '    PrintLine(FileFrm, TAB(12), "Me.txtBuscar.Focus()")
    '    PrintLine(FileFrm, TAB(8), "Catch ex As Exception")
    '    PrintLine(FileFrm, TAB(8), "MessageBox.Show(ex.Message)")
    '    PrintLine(FileFrm, TAB(8), "End Try")
    '    PrintLine(FileFrm, TAB(4), "End Sub")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(4), "End Class")
    '    FileClose(FileFrm)





    '    '*********************************************
    '    '
    '    'DEFINICION DEL CODIGO PARA FORMULARIOS ABM 2 PARTE
    '    '
    '    '*********************************************

    '    If Mid$(PathFrm, Len(PathFrm) - 3, 3) <> ".vb" Then
    '        PathFrm = Me.txtPath.Text & "\frmAbm" & Me.txtTabla.Text & ".Designer.vb"
    '    Else
    '        PathFrm = Me.txtPath.Text & "\" & PathFrm
    '    End If

    '    ' Defino variables
    '    FileFrm = FreeFile()

    '    ' Abro un PathCls de texto (si el mismo existía se reempleza)
    '    FileOpen(FileInsert, PathFrm, OpenMode.Output)

    '    'definicion de variables
    '    PrintLine(FileFrm, TAB(0), "<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _")
    '    PrintLine(FileFrm, TAB(0), "Partial Class frmAbm" & Me.txtTabla.Text)
    '    PrintLine(FileFrm, TAB(4), "Inherits System.Windows.Forms.Form")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(4), "'Form reemplaza a Dispose para limpiar la lista de componentes.")
    '    PrintLine(FileFrm, TAB(4), "<System.Diagnostics.DebuggerNonUserCode()> _")
    '    PrintLine(FileFrm, TAB(4), "Protected Overrides Sub Dispose(ByVal disposing As Boolean)")
    '    PrintLine(FileFrm, TAB(8), "If disposing AndAlso components IsNot Nothing Then")
    '    PrintLine(FileFrm, TAB(12), "components.Dispose()")
    '    PrintLine(FileFrm, TAB(8), "End If")
    '    PrintLine(FileFrm, TAB(8), "MyBase.Dispose(disposing)")
    '    PrintLine(FileFrm, TAB(4), "End Sub")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(4), "'Requerido por el Diseñador de Windows Forms")
    '    PrintLine(FileFrm, TAB(4), "Private components As System.ComponentModel.IContainer")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(4), "'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento")
    '    PrintLine(FileFrm, TAB(4), "'Se puede modificar usando el Diseñador de Windows Forms.")
    '    PrintLine(FileFrm, TAB(4), "'No lo modifique con el editor de código.")
    '    PrintLine(FileFrm, TAB(4), "<System.Diagnostics.DebuggerStepThrough()> _")
    '    PrintLine(FileFrm, TAB(4), "Private Sub InitializeComponent()")
    '    PrintLine(FileFrm, TAB(8), "Me.components = New System.ComponentModel.Container")
    '    PrintLine(FileFrm, TAB(8), "Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAbm" & Me.txtTabla.Text & "))")
    '    PrintLine(FileFrm, TAB(8), "Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle")
    '    PrintLine(FileFrm, TAB(8), "Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle")
    '    PrintLine(FileFrm, TAB(8), "Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle")
    '    PrintLine(FileFrm, TAB(8), "Me.txtBuscar = New System.Windows.Forms.TextBox")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1 = New System.Windows.Forms.GroupBox")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda = New System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir = New System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar = New System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar = New System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar = New System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar = New System.Windows.Forms.Label")
    '    PrintLine(FileFrm, TAB(8), "Me.ttGeneral = New System.Windows.Forms.ToolTip(Me.components)")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo = New System.Windows.Forms.Label")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1 = New System.Windows.Forms.DataGridView")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk = New System.Windows.Forms.Label")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.SuspendLayout()")
    '    PrintLine(FileFrm, TAB(8), "CType(Me.dgv1, System.ComponentModel.ISupportInitialize).BeginInit()")
    '    PrintLine(FileFrm, TAB(8), "Me.SuspendLayout()")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'txtBuscar")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.txtBuscar.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper")
    '    PrintLine(FileFrm, TAB(8), "Me.txtBuscar.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "Me.txtBuscar.Location = New System.Drawing.Point(128, 561)")
    '    PrintLine(FileFrm, TAB(8), "Me.txtBuscar.Name = ""txtBuscar""")
    '    PrintLine(FileFrm, TAB(8), "Me.txtBuscar.Size = New System.Drawing.Size(873, 26)")
    '    PrintLine(FileFrm, TAB(8), "Me.txtBuscar.TabIndex = 563")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'GroupBox1")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.Controls.Add(Me.btnAyuda)")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.Controls.Add(Me.btnSalir)")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.Controls.Add(Me.btnBorrar)")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.Controls.Add(Me.btnModificar)")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.Controls.Add(Me.btnAgregar)")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.Location = New System.Drawing.Point(15, 606)")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.Name = ""GroupBox1""")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.Size = New System.Drawing.Size(986, 107)")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.TabIndex = 564")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.TabStop = False")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'btnAyuda")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.BackColor = System.Drawing.Color.Gainsboro")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.ForeColor = System.Drawing.Color.Black")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.Image = CType(resources.GetObject(""btnAyuda.Image""), System.Drawing.Image)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.Location = New System.Drawing.Point(715, 19)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.Name = ""btnAyuda""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.Size = New System.Drawing.Size(86, 71)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.TabIndex = 11")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.Text = ""A&yuda""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAyuda.UseVisualStyleBackColor = True")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'btnSalir")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.ForeColor = System.Drawing.Color.Black")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.Image = CType(resources.GetObject(""btnSalir.Image""), System.Drawing.Image)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.Location = New System.Drawing.Point(858, 19)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.Name = ""btnSalir""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.Size = New System.Drawing.Size(86, 71)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.TabIndex = 12")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.Text = ""&Salir""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnSalir.UseVisualStyleBackColor = True")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'btnBorrar")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.Cursor = System.Windows.Forms.Cursors.Hand")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.Image = CType(resources.GetObject(""btnBorrar.Image""), System.Drawing.Image)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.Location = New System.Drawing.Point(292, 19)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.Name = ""btnBorrar""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.Size = New System.Drawing.Size(86, 71)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.TabIndex = 10")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.Text = ""&Borrar""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnBorrar.UseVisualStyleBackColor = True")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'btnModificar")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.Cursor = System.Windows.Forms.Cursors.Hand")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.Image = CType(resources.GetObject(""btnModificar.Image""), System.Drawing.Image)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.Location = New System.Drawing.Point(160, 19)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.Name = ""btnModificar""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.Size = New System.Drawing.Size(86, 71)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.TabIndex = 9")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.Text = ""&Modificar""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnModificar.UseVisualStyleBackColor = True")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'btnAgregar")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.Cursor = System.Windows.Forms.Cursors.Hand")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.Image = CType(resources.GetObject(""btnAgregar.Image""), System.Drawing.Image)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.Location = New System.Drawing.Point(28, 19)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.Name = ""btnAgregar""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.Size = New System.Drawing.Size(86, 71)")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.TabIndex = 8")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.Text = ""&Agregar""")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '    PrintLine(FileFrm, TAB(8), "Me.btnAgregar.UseVisualStyleBackColor = True")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'lblconsultar")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.AutoSize = True")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.ForeColor = System.Drawing.Color.Blue")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.Location = New System.Drawing.Point(15, 561)")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.Name = ""lblconsultar""")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.Size = New System.Drawing.Size(100, 26)")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.TabIndex = 567")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.Text = ""Consultar""")
    '    PrintLine(FileFrm, TAB(8), "Me.lblconsultar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'lblTitulo")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.Font = New System.Drawing.Font(""Times New Roman"", 18.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.ForeColor = System.Drawing.Color.Red")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.Location = New System.Drawing.Point(281, 22)")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.Name = ""lblTitulo""")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.Size = New System.Drawing.Size(456, 30)")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.TabIndex = 566")
    '    PrintLine(FileFrm, TAB(8), "Me.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'dgv1")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.AllowUserToAddRows = False")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.AllowUserToResizeColumns = False")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.AllowUserToResizeRows = False")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle1.BackColor = System.Drawing.Color.LightBlue")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightGoldenrodYellow")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Brown")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.BackgroundColor = System.Drawing.Color.PeachPuff")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.BorderStyle = System.Windows.Forms.BorderStyle.None")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle2.BackColor = System.Drawing.Color.Gold")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle2.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle3.BackColor = System.Drawing.Color.Lavender")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle3.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Beige")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Brown")
    '    PrintLine(FileFrm, TAB(8), "DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.DefaultCellStyle = DataGridViewCellStyle3")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.GridColor = System.Drawing.Color.MediumPurple")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.Location = New System.Drawing.Point(15, 83)")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.Name = ""dgv1""")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.ReadOnly = True")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.ShowCellErrors = False")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.ShowRowErrors = False")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.Size = New System.Drawing.Size(986, 457)")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.StandardTab = True")
    '    PrintLine(FileFrm, TAB(8), "Me.dgv1.TabIndex = 562")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'lblid_pk")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk.AutoSize = True")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk.BackColor = System.Drawing.Color.Red")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk.Location = New System.Drawing.Point(21, 94)")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk.Name = ""lblid_pk""")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk.Size = New System.Drawing.Size(13, 13)")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk.TabIndex = 565")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk.Text = ""0""")
    '    PrintLine(FileFrm, TAB(8), "Me.lblid_pk.Visible = False")
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "'frmAbm" & Me.txtTabla.Text)
    '    PrintLine(FileFrm, TAB(8), "'")
    '    PrintLine(FileFrm, TAB(8), "Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)")
    '    PrintLine(FileFrm, TAB(8), "Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font")
    '    PrintLine(FileFrm, TAB(8), "Me.ClientSize = New System.Drawing.Size(1016, 734)")
    '    PrintLine(FileFrm, TAB(8), "Me.Controls.Add(Me.txtBuscar)")
    '    PrintLine(FileFrm, TAB(8), "Me.Controls.Add(Me.GroupBox1)")
    '    PrintLine(FileFrm, TAB(8), "Me.Controls.Add(Me.lblconsultar)")
    '    PrintLine(FileFrm, TAB(8), "Me.Controls.Add(Me.lblTitulo)")
    '    PrintLine(FileFrm, TAB(8), "Me.Controls.Add(Me.lblid_pk)")
    '    PrintLine(FileFrm, TAB(8), "Me.Controls.Add(Me.dgv1)")
    '    PrintLine(FileFrm, TAB(8), "Me.Name = ""frmAbm" & Me.txtTabla.Text & """")
    '    PrintLine(FileFrm, TAB(8), "Me.Text = ""frmAbm" & Me.txtTabla.Text & """")
    '    PrintLine(FileFrm, TAB(8), "Me.GroupBox1.ResumeLayout(False)")
    '    PrintLine(FileFrm, TAB(8), "CType(Me.dgv1, System.ComponentModel.ISupportInitialize).EndInit()")
    '    PrintLine(FileFrm, TAB(8), "Me.ResumeLayout(False)")
    '    PrintLine(FileFrm, TAB(8), "Me.PerformLayout()")
    '    PrintLine(FileFrm, "")
    '    PrintLine(FileFrm, TAB(4), "End Sub")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents txtBuscar As System.Windows.Forms.TextBox")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents GroupBox1 As System.Windows.Forms.GroupBox")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents btnAyuda As System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents btnSalir As System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents btnBorrar As System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents btnModificar As System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents btnAgregar As System.Windows.Forms.Button")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents lblconsultar As System.Windows.Forms.Label")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents ttGeneral As System.Windows.Forms.ToolTip")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents lblTitulo As System.Windows.Forms.Label")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents dgv1 As System.Windows.Forms.DataGridView")
    '    PrintLine(FileFrm, TAB(4), "Protected WithEvents lblid_pk As System.Windows.Forms.Label")
    '    PrintLine(FileFrm, TAB(0), "End Class")
    '    PrintLine(FileFrm, "")
    '    FileClose(FileFrm)

    '    Dim sErr As String = ""
    '    Dim sContents As String
    '    Dim bAns As String

    '    sContents = GetFileContents(Me.txtPath.Text & "\frmAbmGeneral.resx", sErr)
    '    If sErr = "" Then
    '        Debug.WriteLine("File Contents: " & sContents)
    '        'Save to different file
    '        bAns = SaveTextToFile(sContents, Me.txtPath.Text & "\frmAbm" & Me.txtTabla.Text & ".resx", sErr)
    '    End If











    '    '**************************************************
    '    '
    '    'DEFINICION DEL CODIGA PARA FORMULARIOS DE DETALLE
    '    '
    '    '**************************************************

    '    If Mid$(PathFrmdetalle, Len(PathFrm) - 3, 3) <> ".vb" Then
    '        PathFrmdetalle = Me.txtPath.Text & "\frmDetalle" & Me.txtTabla.Text & ".vb"
    '    Else
    '        PathFrmdetalle = Me.txtPath.Text & "\" & PathFrmdetalle
    '    End If

    '    ' Defino variables
    '    Dim FileFrmDetalle As Integer = FreeFile()
    '    Dim regDetalle As regSistemaTabla

    '    ' Abro un PathCls de texto (si el mismo existía se reempleza)
    '    FileOpen(FileFrmDetalle, PathFrmdetalle, OpenMode.Output)


    '    'saca el nombre del primer control para el focus
    '    Contador = 1
    '    Dim strFocus As String = ""
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.Orden = 1 Then
    '            'SALTA LA PK
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)
    '                    strFocus = "Me.cmb" & Nombre
    '                    Exit For
    '                Case "String", "Decimal"
    '                    strFocus = "Me.txt" & regDetalle.nombre
    '                    Exit For
    '                Case "DateTime"
    '                    strFocus = "Me.dtp" & regDetalle.nombre
    '                    Exit For
    '                Case "Boolean"
    '                    strFocus = "Me.chk" & regDetalle.nombre
    '                    Exit For
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next

    '    'saca el nombre del primer txt para los vacios
    '    Contador = 1
    '    Dim strVacio As String = ""
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.Orden = 1 Then
    '            'SALTA LA PK
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "String", "Decimal"
    '                    strVacio = "Me.txt" & regDetalle.nombre
    '                    Exit For
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next

    '    'Private Sub frmDetalle_Load
    '    PrintLine(FileFrmDetalle, TAB(0), "Public Class frmDetalle" & Me.txtTabla.Text)
    '    PrintLine(FileFrmDetalle, "")
    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub frmDetalle" & Me.txtTabla.Text & "_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load")
    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.Orden = 1 Then
    '            'SALTA LA PK
    '        ElseIf regDetalle.nombre = "id_cliente_maestro" Then
    '            'next row
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)
    '                    PrintLine(FileFrmDetalle, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnBusca" & Nombre & ", " & """" & "Buscar Nuevo " & Nombre & """" & ")")
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileFrmDetalle, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnGuardar, " & """" & "Guardar Datos del " & Me.txtTabla.Text & """" & ")")
    '    PrintLine(FileFrmDetalle, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnSalir, " & """" & "Volver a la Pantalla Anterior" & """" & ")")
    '    PrintLine(FileFrmDetalle, TAB(8), "Try")
    '    PrintLine(FileFrmDetalle, TAB(12), "ObtenerDatos()")
    '    PrintLine(FileFrmDetalle, "")
    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.Orden = 1 Then
    '            'SALTA LA PK
    '        ElseIf regDetalle.nombre = "id_cliente_maestro" Then
    '            'next row
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)
    '                    ' PrintLine(FileFrmDetalle, TAB(8), "o" & Nombre & ".Modificar(Me.lblid_" & Nombre & ".Text)")
    '                    ' PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Nombre & ".Text = o" & Nombre & ".nombre_" & Nombre)
    '                    PrintLine(FileFrmDetalle, TAB(12), "Me.cmb" & Nombre & ".Text = o" & Nombre & ".GetOne(Me.lblid_" & Nombre & ".Text).Rows(0).Item(" & """" & "nombre_" & Nombre & """" & ").ToString.Trim")
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileFrmDetalle, TAB(8), "Catch ex As Exception")
    '    PrintLine(FileFrmDetalle, TAB(12), "'    MessageBox.Show(ex.Message)")
    '    PrintLine(FileFrmDetalle, TAB(8), "End Try")
    '    PrintLine(FileFrmDetalle, "")
    '    PrintLine(FileFrmDetalle, TAB(8), "If Bandera" & Me.txtTabla.Text & "  = 1 Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "Me.LimpiarControles()")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrm, TAB(8), "Me.Text = " & """" & Me.txtTabla.Text & """")
    '    PrintLine(FileFrm, TAB(8), "Me.CancelButton = Me.btnSalir")
    '    PrintLine(FileFrm, TAB(8), "Me.BackColor = Color.Teal")
    '    PrintLine(FileFrm, TAB(8), "Me.StartPosition = FormStartPosition.CenterScreen")
    '    PrintLine(FileFrm, TAB(8), "Me.MinimizeBox = False")
    '    PrintLine(FileFrm, TAB(8), "Me.MaximizeBox = False")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Sub CargarCombos()

    '    PrintLine(FileFrmDetalle, TAB(4), "Sub CargarCombos()")
    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.Orden = 1 Then
    '            'SALTA LA PK
    '        ElseIf regDetalle.nombre = "id_cliente_maestro" Then
    '            'next row
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)

    '                    PrintLine(FileFrmDetalle, TAB(8), "Me.Cargar" & Nombre)
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")
    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.Orden = 1 Then
    '            'SALTA LA PK
    '        ElseIf regDetalle.nombre = "id_cliente_maestro" Then
    '            'next row
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)

    '                    PrintLine(FileFrmDetalle, TAB(4), "Sub Cargar" & Nombre & "()")
    '                    PrintLine(FileFrmDetalle, TAB(8), "Dim odt As New DataTable")
    '                    PrintLine(FileFrmDetalle, "")
    '                    PrintLine(FileFrmDetalle, TAB(8), "odt = o" & Nombre & ".GetCmb")
    '                    PrintLine(FileFrmDetalle, TAB(8), "With Me.cmb" & Nombre)
    '                    PrintLine(FileFrmDetalle, TAB(12), ".DataSource = odt")
    '                    PrintLine(FileFrmDetalle, TAB(12), ".DisplayMember = " & """" & "nombre_" & Nombre & """")
    '                    PrintLine(FileFrmDetalle, TAB(12), ".ValueMember = " & """" & "id_" & Nombre & """")
    '                    PrintLine(FileFrmDetalle, TAB(8), "End With")
    '                    PrintLine(FileFrmDetalle, TAB(8), "If Me.cmb" & Nombre & ".SelectedIndex.ToString >= 0 Then")
    '                    PrintLine(FileFrmDetalle, TAB(12), "cmb" & Nombre & ".SelectedIndex = 0")
    '                    PrintLine(FileFrmDetalle, TAB(12), "Me.lblid_" & Nombre & ".Text = cmb" & Nombre & ".SelectedValue.ToString")
    '                    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '                    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '                    PrintLine(FileFrmDetalle, "")
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next

    '    'Sub LimpiarControles()

    '    PrintLine(FileFrmDetalle, TAB(4), "Sub LimpiarControles()")
    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.Orden = 1 Then
    '            'SALTA LA PK
    '        ElseIf regDetalle.nombre = "id_cliente_maestro" Or regDetalle.nombre = "sincronizado" Or regDetalle.nombre = "operacion" Then
    '            'next row
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)
    '                    PrintLine(FileInsert, TAB(8), "Me.lbl" & regDetalle.nombre & ".Text = " & """" & "0" & """")
    '                    PrintLine(FileInsert, TAB(8), "Me.cmb" & Nombre & ".Text =  """"")
    '                Case "String", "Decimal"
    '                    PrintLine(FileInsert, TAB(8), "Me.txt" & regDetalle.nombre & ".Text = """"")
    '                Case "DateTime"
    '                    PrintLine(FileInsert, TAB(8), "Me.dtp" & regDetalle.nombre & ".Value = DateTime.Now")
    '                Case "Boolean"
    '                    PrintLine(FileInsert, TAB(8), "Me.chk" & regDetalle.nombre & ".Checked = False")
    '                Case Else
    '                    PrintLine(FileInsert, TAB(8), "Me." & regDetalle.nombre & ".Text = """"")
    '            End Select

    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private Sub ObtenerDatos()

    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub ObtenerDatos()")
    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.Orden = 1 Then
    '            'SALTA LA PK
    '        ElseIf regDetalle.nombre = "id_cliente_maestro" Or regDetalle.nombre = "sincronizado" Or regDetalle.nombre = "operacion" Then
    '            'next row
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    PrintLine(FileInsert, TAB(8), "Me.lbl" & regDetalle.nombre & ".Text = o" & Me.txtTabla.Text & "." & regDetalle.nombre)
    '                Case "String"
    '                    PrintLine(FileInsert, TAB(8), "Me.txt" & regDetalle.nombre & ".Text = o" & Me.txtTabla.Text & "." & regDetalle.nombre & ".Trim")
    '                Case "Decimal"
    '                    PrintLine(FileInsert, TAB(8), "Me.txt" & regDetalle.nombre & ".Text = o" & Me.txtTabla.Text & "." & regDetalle.nombre & ".ToString.Trim")
    '                Case "DateTime"
    '                    PrintLine(FileInsert, TAB(8), "Me.dtp" & regDetalle.nombre & ".Value = o" & Me.txtTabla.Text & "." & regDetalle.nombre)
    '                Case "Boolean"
    '                    PrintLine(FileInsert, TAB(8), "Me.chk" & regDetalle.nombre & ".Checked = o" & Me.txtTabla.Text & "." & regDetalle.nombre)
    '                Case Else
    '                    PrintLine(FileInsert, TAB(8), "Me." & regDetalle.nombre & ".Text = Trim(o" & Me.txtTabla.Text & "." & regDetalle.nombre & ")")
    '            End Select

    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private Sub AsignarDatos()

    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub AsignarDatos()")
    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.Orden = 1 Then
    '            'SALTA LA PK
    '        ElseIf regDetalle.nombre = "operacion" Or regDetalle.nombre = "sincronizado" Then
    '            'next row
    '        ElseIf regDetalle.nombre = "id_cliente_maestro" Then
    '            PrintLine(FileInsert, TAB(8), "o" & Me.txtTabla.Text & "." & regDetalle.nombre & " = id_cliente_maestro")
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    PrintLine(FileInsert, TAB(8), "o" & Me.txtTabla.Text & "." & regDetalle.nombre & " = Me.lbl" & regDetalle.nombre & ".Text")
    '                Case "String", "Decimal"
    '                    PrintLine(FileInsert, TAB(8), "o" & Me.txtTabla.Text & "." & regDetalle.nombre & " = Me.txt" & regDetalle.nombre & ".Text")
    '                Case "DateTime"
    '                    PrintLine(FileInsert, TAB(8), "o" & Me.txtTabla.Text & "." & regDetalle.nombre & " = Me.dtp" & regDetalle.nombre & ".Value.Date")
    '                Case "Boolean"
    '                    PrintLine(FileInsert, TAB(8), "o" & Me.txtTabla.Text & "." & regDetalle.nombre & " = Me.chk" & regDetalle.nombre & ".Checked")
    '                Case Else
    '                    PrintLine(FileInsert, TAB(8), "o" & Me.txtTabla.Text & "." & regDetalle.nombre & " = Me." & regDetalle.nombre & ".Text")
    '            End Select

    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Public Sub SoloLectura()

    '    PrintLine(FileFrmDetalle, TAB(4), "Public Sub SoloLectura()")
    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.Orden = 1 Then
    '            'SALTA LA PK
    '        ElseIf regDetalle.nombre = "id_cliente_maestro" Or regDetalle.nombre = "sincronizado" Or regDetalle.nombre = "operacion" Then
    '            'next row
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)
    '                    '  PrintLine(FileInsert, TAB(4), "Me.lbl" & regDetalle.nombre & ".Text = " & """" & "0" & """")
    '                    PrintLine(FileInsert, TAB(8), "Me.cmb" & Nombre & ".Enabled = False")
    '                    PrintLine(FileInsert, TAB(8), "Me.btnBusca" & Nombre & ".Enabled = False")
    '                Case "String", "Decimal"
    '                    PrintLine(FileInsert, TAB(8), "Me.txt" & regDetalle.nombre & ".Enabled = False")
    '                Case "DateTime"
    '                    PrintLine(FileInsert, TAB(8), "Me.dtp" & regDetalle.nombre & ".Enabled = False")
    '                Case "Boolean"
    '                    PrintLine(FileInsert, TAB(8), "Me.chk" & regDetalle.nombre & ".Enabled = False")
    '                Case Else
    '                    PrintLine(FileInsert, TAB(8), "Me." & regDetalle.nombre & ".Enabled = False")
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Visible = False")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private Sub Guardar()

    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub Guardar()")
    '    PrintLine(FileFrmDetalle, TAB(8), "Try")
    '    PrintLine(FileFrmDetalle, TAB(12), "Me.AsignarDatos()")

    '    PrintLine(FileFrmDetalle, TAB(12), "If o" & Me.txtTabla.Text & ".Exist() Then")
    '    PrintLine(FileFrmDetalle, TAB(16), "If Bandera" & Me.txtTabla.Text & " = 1 Then")
    '    PrintLine(FileFrmDetalle, TAB(20), "MessageBox.Show(" & """" & "Sr. Usuario: Los Datos que Pretende Ingresar ya Fueron Cargados en el Sistema" & """" & ")")
    '    PrintLine(FileFrmDetalle, TAB(16), "Exit Sub")
    '    PrintLine(FileFrmDetalle, TAB(16), "ElseIf Bandera" & Me.txtTabla.Text & " = 2 Then")
    '    PrintLine(FileCls, TAB(20), "If MessageBox.Show(" & """" & "Sr. Usuario: Los Datos que Desea Modificar ya Existen ¿Desea Reemplazarlos?" & """" & ", " & """" & "MODIFICAR" & """" & ", _")
    '    PrintLine(FileCls, TAB(24), "MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) _")
    '    PrintLine(FileCls, TAB(24), "= Windows.Forms.DialogResult.No Then")
    '    PrintLine(FileCls, TAB(24), "Exit Sub")
    '    PrintLine(FileCls, TAB(20), "End If")
    '    PrintLine(FileFrmDetalle, TAB(16), "End If")
    '    PrintLine(FileFrmDetalle, TAB(12), "End If")

    '    PrintLine(FileFrmDetalle, TAB(12), "Select Case Bandera" & Me.txtTabla.Text)
    '    PrintLine(FileFrmDetalle, TAB(16), "Case 1 'GUARDA,REFRESCA LA GRILLA E INSERTA UNO NUEVO (AGREGAR)")
    '    PrintLine(FileFrmDetalle, TAB(20), "o" & Me.txtTabla.Text & ".Guardar()")
    '    PrintLine(FileFrmDetalle, TAB(20), "CType(Me.Owner, frmAbm" & Me.txtTabla.Text & ").RefrescarGrilla()")
    '    PrintLine(FileFrmDetalle, TAB(20), "Me.CargarCombos()")
    '    PrintLine(FileFrmDetalle, TAB(20), "o" & Me.txtTabla.Text & ".Insertar()")
    '    PrintLine(FileFrmDetalle, TAB(20), "Me.ObtenerDatos()")
    '    PrintLine(FileFrmDetalle, TAB(20), "Me.LimpiarControles()")
    '    PrintLine(FileFrmDetalle, TAB(20), strFocus & ".Focus")
    '    PrintLine(FileFrmDetalle, TAB(16), "Case 2 'GUARDA Y SALE (MODIFICAR)")
    '    PrintLine(FileFrmDetalle, TAB(20), "o" & Me.txtTabla.Text & ".Modificar()")
    '    PrintLine(FileFrmDetalle, TAB(20), "Me.Close()")
    '    PrintLine(FileFrmDetalle, TAB(12), "End Select")
    '    PrintLine(FileFrmDetalle, TAB(8), "Catch ex As Exception")
    '    PrintLine(FileFrmDetalle, TAB(12), "MessageBox.Show(ex.Message)")
    '    PrintLine(FileFrmDetalle, TAB(8), "End Try")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private Function ChequearVacios()

    '    PrintLine(FileFrmDetalle, TAB(4), "Private Function ChequearVacios() As Boolean")
    '    PrintLine(FileFrmDetalle, TAB(8), "Dim bandera As Boolean")
    '    PrintLine(FileFrmDetalle, TAB(8), "If " & strVacio & ".Text = """"" & " Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "bandera = False")
    '    PrintLine(FileFrmDetalle, TAB(8), "Else")
    '    PrintLine(FileFrmDetalle, TAB(12), "bandera = True")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrmDetalle, TAB(8), "Return bandera")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Function")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private Sub btnAyuda_Click ANULADO

    '    '    PrintLine(FileFrmDetalle, TAB(0), "Private Sub btnAyuda_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAyuda.Click")
    '    '    PrintLine(FileFrmDetalle, TAB(4), "'      System.Diagnostics.Process.Start(PathAyuda + " & """" & "FrmDetalle" & Me.txtTabla.Text & """" & ".htm)")
    '    '    PrintLine(FileFrmDetalle, TAB(0), "End Sub")
    '    '    PrintLine(FileFrmDetalle, "")

    '    'Private Sub btnGuardar_Click

    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click")
    '    PrintLine(FileFrmDetalle, TAB(8), "Dim blnVacios As Boolean")
    '    PrintLine(FileFrmDetalle, "")


    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.Orden = 1 Then
    '            'SALTA LA PK
    '        ElseIf regDetalle.nombre = "id_cliente_maestro" Or regDetalle.nombre = "sincronizado" Or regDetalle.nombre = "operacion" Then
    '            'next row
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    PrintLine(FileFrmDetalle, TAB(8), "If Me.lbl" & regDetalle.nombre & ".Text = 0 Then")
    '                    'si no tiene 'id_' salta el error
    '                    Try
    '                        PrintLine(FileFrmDetalle, TAB(12), "MessageBox.Show(" & """" & "Debe Seleccionar un Dato del Combo de " & regDetalle.nombre.Substring(3) & """" & ")")
    '                    Catch ex As Exception
    '                    End Try
    '                    PrintLine(FileFrmDetalle, TAB(12), "Exit Sub")
    '                    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next

    '    PrintLine(FileFrmDetalle, "")
    '    PrintLine(FileFrmDetalle, TAB(8), "blnVacios = Me.ChequearVacios")
    '    PrintLine(FileFrmDetalle, TAB(8), "If blnVacios = False Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "MessageBox.Show(" & """" & "Debe Llenar los Campos Obligatorios" & """" & ")")
    '    PrintLine(FileFrmDetalle, TAB(12), "Exit Sub")
    '    PrintLine(FileFrmDetalle, TAB(8), "Else")
    '    PrintLine(FileFrmDetalle, TAB(12), "If Bandera" & Me.txtTabla.Text & "  = 1 Then")
    '    PrintLine(FileFrmDetalle, TAB(16), "Me.Guardar()")
    '    PrintLine(FileFrmDetalle, TAB(12), "Else")
    '    PrintLine(FileFrmDetalle, TAB(16), "Me.Guardar()")
    '    PrintLine(FileFrmDetalle, TAB(12), "End If")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private Sub btnSalir_Click

    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click")
    '    PrintLine(FileFrmDetalle, TAB(8), "Me.Close()")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private Sub cmb

    '    Contador = 1
    '    For Each regDetalle In arrEstructura
    '        If regDetalle.Orden = 1 Then
    '            'SALTA LA PK
    '        ElseIf regDetalle.nombre = "id_cliente_maestro" Or regDetalle.nombre = "sincronizado" Or regDetalle.nombre = "operacion" Then
    '            'next row
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)

    '                    PrintLine(FileFrmDetalle, TAB(4), "Private Sub cmb" & Nombre & "_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmb" & Nombre & ".SelectedIndexChanged")
    '                    PrintLine(FileFrmDetalle, TAB(8), "If Me.cmb" & Nombre & ".SelectedIndex >= 0 Then")
    '                    PrintLine(FileFrmDetalle, TAB(12), "Me.lbl" & regDetalle.nombre & ".Text = cmb" & Nombre & ".SelectedValue.ToString")
    '                    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '                    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '                    PrintLine(FileFrmDetalle, "")

    '                    PrintLine(FileFrmDetalle, TAB(4), "Private Sub btnBusca" & Nombre & "_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBusca" & Nombre & ".Click")
    '                    PrintLine(FileFrmDetalle, TAB(8), "Dim frmTemporal As New FrmAbm" & Nombre)
    '                    PrintLine(FileFrmDetalle, TAB(8), "frmTemporal.ShowDialog()")
    '                    PrintLine(FileFrmDetalle, TAB(8), "Me.Cargar" & Nombre & "()")
    '                    PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Nombre & ".Focus()")
    '                    PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Nombre & ".Text = """"")
    '                    PrintLine(FileFrmDetalle, TAB(8), "Me.lblid_" & Nombre & ".Text = " & """" & "0" & """")
    '                    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '                    PrintLine(FileFrmDetalle, "")
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next

    '    'arma las cadenas de tabulacion
    '    Contador = 1
    '    Dim TabPress As String = ""
    '    Dim TabDown As String = ""
    '    Dim TabDecimal As String = ""

    '    For Each regDetalle In arrEstructura
    '        If regDetalle.Orden = 1 Then
    '            'SALTA LA PK
    '        ElseIf regDetalle.nombre = "id_cliente_maestro" Or regDetalle.nombre = "sincronizado" Or regDetalle.nombre = "operacion" Then
    '            'next row
    '        Else
    '            Select Case regDetalle.tiposql
    '                Case "Int32", "Int64", "Int16", "Integer"
    '                    Dim Nombre As String = Mid$(regDetalle.nombre, 1, 4).ToUpper & Mid$(regDetalle.nombre, 5).ToLower
    '                    Nombre = Mid$(Nombre, 4)
    '                    '  PrintLine(FileInsert, TAB(4), "Me.lbl" & regDetalle.nombre & ".Text = " & """" & "0" & """")
    '                    TabDown = TabDown & "cmb" & Nombre & ".KeyDown, "
    '                Case "String"
    '                    TabPress = TabPress & "txt" & regDetalle.nombre & ".KeyPress, "
    '                Case "Decimal"
    '                    TabDecimal = TabDecimal & "txt" & regDetalle.nombre & ".KeyPress, "
    '                Case "DateTime"
    '                    TabDown = TabDown & "dtp" & regDetalle.nombre & ".KeyDown, "
    '                Case "Boolean"
    '                    TabPress = TabPress & "chk" & regDetalle.nombre & ".KeyPress, "
    '            End Select
    '        End If
    '        Contador = Contador + 1
    '    Next
    '    Dim LargoTab As Integer = TabPress.Length
    '    If LargoTab >= 2 Then
    '        TabPress = TabPress.Substring(0, LargoTab - 2)
    '    End If
    '    LargoTab = TabDown.Length
    '    If LargoTab >= 2 Then
    '        TabDown = TabDown.Substring(0, LargoTab - 2)
    '    End If
    '    LargoTab = TabDecimal.Length
    '    If LargoTab >= 2 Then
    '        TabDecimal = TabDecimal.Substring(0, LargoTab - 2)
    '    End If

    '    'Private Sub TabulacionTextBox

    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub TabulacionTextBox(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _") 'Handles " & TabPress)
    '    If TabPress = "" Then
    '        PrintLine(FileFrmDetalle, TAB(4), "'Handles " & TabPress)
    '    Else
    '        PrintLine(FileFrmDetalle, TAB(4), "Handles " & TabPress)
    '    End If
    '    PrintLine(FileFrmDetalle, TAB(8), "If e.KeyChar.ToString = vbCr Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "Me.SelectNextControl(Me.ActiveControl, True, True, True, True)")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private Sub TabulacionCombos

    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub TabulacionCombos(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) _") 'Handles " & TabDown)
    '    If TabDown = "" Then
    '        PrintLine(FileFrmDetalle, TAB(4), "'Handles " & TabDown)
    '    Else
    '        PrintLine(FileFrmDetalle, TAB(4), "Handles " & TabDown)
    '    End If
    '    PrintLine(FileFrmDetalle, TAB(8), "If e.KeyValue.ToString = 13 Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "Me.SelectNextControl(Me.ActiveControl, True, True, True, True)")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")

    '    'Private sub tabulacion decimales
    '    PrintLine(FileFrmDetalle, TAB(4), "Private Sub Decimales(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _")
    '    If TabDecimal = "" Then
    '        PrintLine(FileFrmDetalle, TAB(4), "'Handles " & TabDecimal)
    '    Else
    '        PrintLine(FileFrmDetalle, TAB(4), "Handles " & TabDecimal)
    '    End If
    '    PrintLine(FileFrmDetalle, TAB(8), "Dim txtTemp As TextBox")
    '    PrintLine(FileFrmDetalle, TAB(8), "txtTemp = sender")
    '    PrintLine(FileFrmDetalle, "")
    '    PrintLine(FileFrmDetalle, TAB(8), "If e.KeyChar.ToString = vbCr Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "Me.SelectNextControl(Me.ActiveControl, True, True, True, True)")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrmDetalle, TAB(8), "If e.KeyChar.ToString = " & """" & "." & """" & " Or e.KeyChar.ToString = " & """" & "," & """" & " Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "If InStr(txtTemp.Text, " & """" & "," & """" & ") <> 0 Then")
    '    PrintLine(FileFrmDetalle, TAB(16), "e.Handled = True")
    '    PrintLine(FileFrmDetalle, TAB(12), "Else")
    '    PrintLine(FileFrmDetalle, TAB(16), "e.KeyChar = " & """" & "," & """")
    '    PrintLine(FileFrmDetalle, TAB(12), "End If")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrmDetalle, "")
    '    PrintLine(FileFrmDetalle, TAB(8), "Dim Largo As Integer = InStr(txtTemp.Text, " & """" & "," & """" & ")")
    '    PrintLine(FileFrmDetalle, "")
    '    PrintLine(FileFrmDetalle, TAB(8), "If txtTemp.Text.Length > Largo + 2 And Largo <> 0 And e.KeyChar.ToString <> vbBack Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "e.Handled = True")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrmDetalle, "")
    '    PrintLine(FileFrmDetalle, TAB(8), "If (e.KeyChar.ToString >= " & """" & "0" & """" & " And e.KeyChar.ToString <= " & """" & "9" & """" & ") Or e.KeyChar.ToString = " & """" & "," & """" & " Or e.KeyChar = vbBack Then")
    '    PrintLine(FileFrmDetalle, TAB(12), "'  e.Handled = False")
    '    PrintLine(FileFrmDetalle, TAB(8), "Else")
    '    PrintLine(FileFrmDetalle, TAB(12), "e.Handled = True")
    '    PrintLine(FileFrmDetalle, TAB(8), "End If")
    '    PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '    PrintLine(FileFrmDetalle, "")
    '    PrintLine(FileFrmDetalle, TAB(0), "End Class")

    '    FileClose(FileFrmDetalle)

    '    '***********************************************************************
    '    '
    '    'DEFINICION DEL CODIGA PARA FORMULARIOS DE DETALLE PARTE 2 CON UN TEXBOX
    '    '
    '    '***********************************************************************
    '    If Me.chk_1.Checked = True Then

    '        sContents = GetFileContents(Me.txtPath.Text & "\frmDetalleModelo1.resx", sErr)
    '        If sErr = "" Then
    '            Debug.WriteLine("File Contents: " & sContents)
    '            'Save to different file
    '            bAns = SaveTextToFile(sContents, Me.txtPath.Text & "\frmDetalle" & Me.txtTabla.Text & ".resx", sErr)
    '        End If

    '        If Mid$(PathFrmdetalle, Len(PathFrm) - 3, 3) <> ".vb" Then
    '            PathFrmdetalle = Me.txtPath.Text & "\frmDetalle" & Me.txtTabla.Text & ".Designer.vb"
    '        Else
    '            PathFrmdetalle = Me.txtPath.Text & "\" & PathFrmdetalle
    '        End If

    '        FileFrmDetalle = FreeFile()

    '        ' Abro un PathCls de texto (si el mismo existía se reempleza)
    '        FileOpen(FileFrmDetalle, PathFrmdetalle, OpenMode.Output)
    '        PrintLine(FileFrmDetalle, TAB(0), "<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _")
    '        PrintLine(FileFrmDetalle, TAB(0), "Partial Class frmDetalle" & Me.txtTabla.Text)
    '        PrintLine(FileFrmDetalle, TAB(4), "Inherits System.Windows.Forms.Form")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "'Form reemplaza a Dispose para limpiar la lista de componentes.")
    '        PrintLine(FileFrmDetalle, TAB(4), "<System.Diagnostics.DebuggerNonUserCode()> _")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected Overrides Sub Dispose(ByVal disposing As Boolean)")
    '        PrintLine(FileFrmDetalle, TAB(8), "If disposing AndAlso components IsNot Nothing Then")
    '        PrintLine(FileFrmDetalle, TAB(12), "components.Dispose()")
    '        PrintLine(FileFrmDetalle, TAB(8), "End If")
    '        PrintLine(FileFrmDetalle, TAB(8), "MyBase.Dispose(disposing)")
    '        PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "'Requerido por el Diseñador de Windows Forms")
    '        PrintLine(FileFrmDetalle, TAB(4), "Private components As System.ComponentModel.IContainer")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento")
    '        PrintLine(FileFrmDetalle, TAB(4), "'Se puede modificar usando el Diseñador de Windows Forms.")
    '        PrintLine(FileFrmDetalle, TAB(4), "'No lo modifique con el editor de código.")
    '        PrintLine(FileFrmDetalle, TAB(4), "<System.Diagnostics.DebuggerStepThrough()> _")
    '        PrintLine(FileFrmDetalle, TAB(4), "Private Sub InitializeComponent()")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.components = New System.ComponentModel.Container")
    '        PrintLine(FileFrmDetalle, TAB(8), "Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetalle" & Me.txtTabla.Text & "))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir = New System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1 = New System.Windows.Forms.GroupBox")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar = New System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & " = New System.Windows.Forms.TextBox")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & " = New System.Windows.Forms.Label")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.ttGeneral = New System.Windows.Forms.ToolTip(Me.components)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.SuspendLayout()")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'btnSalir")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.ForeColor = System.Drawing.Color.Black")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Image = CType(resources.GetObject(""btnSalir.Image""), System.Drawing.Image)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Location = New System.Drawing.Point(484, 170)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Name = ""btnSalir""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Size = New System.Drawing.Size(81, 69)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.TabIndex = 21")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Text = ""&Salir""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.UseVisualStyleBackColor = True")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'GroupBox1")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.Location = New System.Drawing.Point(24, 24)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.Name = ""GroupBox1""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.Size = New System.Drawing.Size(541, 101)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.TabIndex = 0")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.TabStop = False")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'btnGuardar")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.ForeColor = System.Drawing.Color.Black")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Image = CType(resources.GetObject(""btnGuardar.Image""), System.Drawing.Image)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Location = New System.Drawing.Point(377, 170)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Name = ""btnGuardar""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Size = New System.Drawing.Size(78, 69)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.TabIndex = 20")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Text = ""&Guardar""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.UseVisualStyleBackColor = True")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'txt" & Me.txtTexto1.Text)
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".BackColor = System.Drawing.Color.White")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".Location = New System.Drawing.Point(159, 67)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".MaxLength = 50")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".Name = ""txt" & Me.txtTexto1.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".Size = New System.Drawing.Size(385, 20)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".TabIndex = 5")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'lbl" & Me.txtLabel1.Text)
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".ForeColor = System.Drawing.Color.Black")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".Location = New System.Drawing.Point(43, 66)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".Name = ""lbl" & Me.txtLabel1.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".Size = New System.Drawing.Size(110, 21)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".TabIndex = 577")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".Text = ""* " & Me.txtLabel1.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtLabel1.Text & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'frmDetalle" & Me.txtTabla.Text)
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.CancelButton = Me.btnSalir")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.ClientSize = New System.Drawing.Size(592, 266)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.btnSalir)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.btnGuardar)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.txt" & Me.txtTexto1.Text & ")")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.lbl" & Me.txtLabel1.Text & ")")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.GroupBox1)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Name = ""frmDetalle" & Me.txtTabla.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Text = ""frmDetalle" & Me.txtTabla.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.ResumeLayout(False)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.PerformLayout()")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents btnSalir As System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents GroupBox1 As System.Windows.Forms.GroupBox")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents btnGuardar As System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents txt" & Me.txtTexto1.Text & " As System.Windows.Forms.TextBox")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents lbl" & Me.txtLabel1.Text & " As System.Windows.Forms.Label")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents ttGeneral As System.Windows.Forms.ToolTip")
    '        PrintLine(FileFrmDetalle, TAB(0), "End Class")
    '        PrintLine(FileFrmDetalle, "")

    '        FileClose(FileFrmDetalle)
    '    End If



    '    '**********************************************************************************
    '    '
    '    'DEFINICION DEL CODIGA PARA FORMULARIOS DE DETALLE PARTE 2 CON UN TEXBOX Y UN COMBO
    '    '
    '    '**********************************************************************************
    '    If Me.chk_2.Checked = True Then
    '        sContents = GetFileContents(Me.txtPath.Text & "\frmDetalleModelo2.resx", sErr)
    '        If sErr = "" Then
    '            Debug.WriteLine("File Contents: " & sContents)
    '            'Save to different file
    '            bAns = SaveTextToFile(sContents, Me.txtPath.Text & "\frmDetalle" & Me.txtTabla.Text & ".resx", sErr)
    '        End If

    '        If Mid$(PathFrmdetalle, Len(PathFrm) - 3, 3) <> ".vb" Then
    '            PathFrmdetalle = Me.txtPath.Text & "\frmDetalle" & Me.txtTabla.Text & ".Designer.vb"
    '        Else
    '            PathFrmdetalle = Me.txtPath.Text & "\" & PathFrmdetalle
    '        End If

    '        FileFrmDetalle = FreeFile()

    '        ' Abro un PathCls de texto (si el mismo existía se reempleza)
    '        FileOpen(FileFrmDetalle, PathFrmdetalle, OpenMode.Output)

    '        PrintLine(FileFrmDetalle, TAB(0), "<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _")
    '        PrintLine(FileFrmDetalle, TAB(0), "Partial Class frmDetalle" & Me.txtTabla.Text)
    '        PrintLine(FileFrmDetalle, TAB(4), "Inherits System.Windows.Forms.Form")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "'Form reemplaza a Dispose para limpiar la lista de componentes.")
    '        PrintLine(FileFrmDetalle, TAB(4), "<System.Diagnostics.DebuggerNonUserCode()> _")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected Overrides Sub Dispose(ByVal disposing As Boolean)")
    '        PrintLine(FileFrmDetalle, TAB(8), "If disposing AndAlso components IsNot Nothing Then")
    '        PrintLine(FileFrmDetalle, TAB(12), "components.Dispose()")
    '        PrintLine(FileFrmDetalle, TAB(8), "End If")
    '        PrintLine(FileFrmDetalle, TAB(8), "MyBase.Dispose(disposing)")
    '        PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "'Requerido por el Diseñador de Windows Forms")
    '        PrintLine(FileFrmDetalle, TAB(4), "Private components As System.ComponentModel.IContainer")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento")
    '        PrintLine(FileFrmDetalle, TAB(4), "'Se puede modificar usando el Diseñador de Windows Forms.")
    '        PrintLine(FileFrmDetalle, TAB(4), "'No lo modifique con el editor de código.")
    '        PrintLine(FileFrmDetalle, TAB(4), "<System.Diagnostics.DebuggerStepThrough()> _")
    '        PrintLine(FileFrmDetalle, TAB(4), "Private Sub InitializeComponent()")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.components = New System.ComponentModel.Container")
    '        PrintLine(FileFrmDetalle, TAB(8), "Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetalle" & Me.txtTabla.Text & "))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir = New System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar = New System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & " = New System.Windows.Forms.Label")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & " = New System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Me.txtCombo1.Text.Substring(3) & " = New System.Windows.Forms.ComboBox")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2 = New System.Windows.Forms.Label")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.ttGeneral = New System.Windows.Forms.ToolTip(Me.components)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & " = New System.Windows.Forms.TextBox")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1 = New System.Windows.Forms.Label")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1 = New System.Windows.Forms.GroupBox")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.SuspendLayout()")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'btnSalir")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.ForeColor = System.Drawing.Color.Black")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Image = CType(Resources.GetObject(""btnSalir.Image""), System.Drawing.Image)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Location = New System.Drawing.Point(486, 175)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Name = ""btnSalir""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Size = New System.Drawing.Size(81, 69)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.TabIndex = 21")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Text = ""&Salir""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.UseVisualStyleBackColor = True")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'btnGuardar")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.ForeColor = System.Drawing.Color.Black")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Image = CType(Resources.GetObject(""btnGuardar.Image""), System.Drawing.Image)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Location = New System.Drawing.Point(379, 175)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Name = ""btnGuardar""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Size = New System.Drawing.Size(78, 69)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.TabIndex = 20")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Text = ""&Guardar""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.UseVisualStyleBackColor = True")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'lbl" & Me.txtCombo1.Text)
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & ".AutoSize = True")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & ".BackColor = System.Drawing.Color.Red")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & ".Location = New System.Drawing.Point(370, 99)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & ".Name = ""lbl" & Me.txtCombo1.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & ".Size = New System.Drawing.Size(13, 13)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & ".TabIndex = 586")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & ".Text = ""0""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & Me.txtCombo1.Text & ".Visible = False")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'btnBusca" & Me.txtCombo1.Text.Substring(3))
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & ".Cursor = System.Windows.Forms.Cursors.Hand")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & ".Image = CType(Resources.GetObject(""btnBusca1.Image""), System.Drawing.Image)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & ".Location = New System.Drawing.Point(505, 94)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & ".Name = ""btnBusca" & Me.txtCombo1.Text.Substring(3) & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & ".Size = New System.Drawing.Size(41, 36)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & ".TabIndex = 30")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & ".UseVisualStyleBackColor = True")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'cmb" & Me.txtCombo1.Text.Substring(3))
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Me.txtCombo1.Text.Substring(3) & ".AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Me.txtCombo1.Text.Substring(3) & ".AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Me.txtCombo1.Text.Substring(3) & ".FormattingEnabled = True")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Me.txtCombo1.Text.Substring(3) & ".Location = New System.Drawing.Point(161, 96)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Me.txtCombo1.Text.Substring(3) & ".Name = ""cmb" & Me.txtCombo1.Text.Substring(3) & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Me.txtCombo1.Text.Substring(3) & ".Size = New System.Drawing.Size(329, 21)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & Me.txtCombo1.Text.Substring(3) & ".TabIndex = 6")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'lblEtiqueta2")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.ForeColor = System.Drawing.Color.Black")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.Location = New System.Drawing.Point(28, 94)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.Name = ""lblEtiqueta2""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.Size = New System.Drawing.Size(127, 21)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.TabIndex = 584")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.Text = """ & Me.txtLabel2.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'txt" & Me.txtTexto1.Text)
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".BackColor = System.Drawing.Color.White")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".Location = New System.Drawing.Point(161, 50)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".MaxLength = 50")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".Name = ""txt" & Me.txtTexto1.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".Size = New System.Drawing.Size(385, 20)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & Me.txtTexto1.Text & ".TabIndex = 5")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'lbllblEtiqueta1")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.ForeColor = System.Drawing.Color.Black")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.Location = New System.Drawing.Point(28, 49)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.Name = ""lblEtiqueta1""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.Size = New System.Drawing.Size(127, 21)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.TabIndex = 588")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.Text = "" * " & Me.txtLabel1.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.lblEtiqueta1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'GroupBox1")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.Location = New System.Drawing.Point(12, 17)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.Name = ""GroupBox1""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.Size = New System.Drawing.Size(568, 135)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.TabIndex = 0")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.TabStop = False")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'frmDetalle" & Me.txtTabla.Text)
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.ClientSize = New System.Drawing.Size(592, 266)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.txt" & Me.txtTexto1.Text & ")")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.lblEtiqueta1)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.lbl" & Me.txtCombo1.Text & ")")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.btnBusca" & Me.txtCombo1.Text.Substring(3) & ")")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.cmb" & Me.txtCombo1.Text.Substring(3) & ")")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.lblEtiqueta2)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.btnSalir)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.btnGuardar)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.GroupBox1)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Name = ""frmDetalle" & Me.txtTabla.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Text = ""frmDetalle" & Me.txtTabla.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.ResumeLayout(False)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.PerformLayout()")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents btnSalir As System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents btnGuardar As System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(4), "Friend WithEvents lbl" & Me.txtCombo1.Text & " As System.Windows.Forms.Label")
    '        PrintLine(FileFrmDetalle, TAB(4), "Friend WithEvents btnBusca" & Me.txtCombo1.Text.Substring(3) & " As System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(4), "Friend WithEvents cmb" & Me.txtCombo1.Text.Substring(3) & " As System.Windows.Forms.ComboBox")
    '        PrintLine(FileFrmDetalle, TAB(4), "Friend WithEvents lblEtiqueta2 As System.Windows.Forms.Label")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents ttGeneral As System.Windows.Forms.ToolTip")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents txt" & Me.txtTexto1.Text & " As System.Windows.Forms.TextBox")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents lblEtiqueta1 As System.Windows.Forms.Label")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents GroupBox1 As System.Windows.Forms.GroupBox")
    '        PrintLine(FileFrmDetalle, TAB(0), "End Class")
    '        PrintLine(FileFrmDetalle, "")

    '        FileClose(FileFrmDetalle)

    '    End If

    '    'formulario completo
    '    If Me.chk_3.Checked Then



    '        '--------- formulario de detalle parte del diseño

    '        sContents = GetFileContents(Me.txtPath.Text & "\frmDetalleModelo2.resx", sErr)
    '        If sErr = "" Then
    '            Debug.WriteLine("File Contents: " & sContents)
    '            'Save to different file
    '            bAns = SaveTextToFile(sContents, Me.txtPath.Text & "\frmDetalle" & Me.txtTabla.Text & ".resx", sErr)
    '        End If

    '        If Mid$(PathFrmdetalle, Len(PathFrm) - 3, 3) <> ".vb" Then
    '            PathFrmdetalle = Me.txtPath.Text & "\frmDetalle" & Me.txtTabla.Text & ".Designer.vb"
    '        Else
    '            PathFrmdetalle = Me.txtPath.Text & "\" & PathFrmdetalle
    '        End If

    '        FileFrmDetalle = FreeFile()

    '        ' Abro un PathCls de texto (si el mismo existía se reempleza)
    '        FileOpen(FileFrmDetalle, PathFrmdetalle, OpenMode.Output)

    '        PrintLine(FileFrmDetalle, TAB(0), "<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _")
    '        PrintLine(FileFrmDetalle, TAB(0), "Partial Class frmDetalle" & Me.txtTabla.Text)
    '        PrintLine(FileFrmDetalle, TAB(4), "Inherits System.Windows.Forms.Form")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "'Form reemplaza a Dispose para limpiar la lista de componentes.")
    '        PrintLine(FileFrmDetalle, TAB(4), "<System.Diagnostics.DebuggerNonUserCode()> _")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected Overrides Sub Dispose(ByVal disposing As Boolean)")
    '        PrintLine(FileFrmDetalle, TAB(8), "If disposing AndAlso components IsNot Nothing Then")
    '        PrintLine(FileFrmDetalle, TAB(12), "components.Dispose()")
    '        PrintLine(FileFrmDetalle, TAB(8), "End If")
    '        PrintLine(FileFrmDetalle, TAB(8), "MyBase.Dispose(disposing)")
    '        PrintLine(FileFrmDetalle, TAB(4), "End Sub")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "'Requerido por el Diseñador de Windows Forms")
    '        PrintLine(FileFrmDetalle, TAB(4), "Private components As System.ComponentModel.IContainer")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento")
    '        PrintLine(FileFrmDetalle, TAB(4), "'Se puede modificar usando el Diseñador de Windows Forms.")
    '        PrintLine(FileFrmDetalle, TAB(4), "'No lo modifique con el editor de código.")
    '        PrintLine(FileFrmDetalle, TAB(4), "<System.Diagnostics.DebuggerStepThrough()> _")
    '        PrintLine(FileFrmDetalle, TAB(4), "Private Sub InitializeComponent()")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.components = New System.ComponentModel.Container")
    '        PrintLine(FileFrmDetalle, TAB(8), "Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetalle" & Me.txtTabla.Text & "))")

    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir = New System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar = New System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1 = New System.Windows.Forms.GroupBox")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.ttGeneral = New System.Windows.Forms.ToolTip(Me.components)")

    '        For Each reg In arrEstructura
    '            If reg.Orden = 1 Or reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
    '                'next row
    '            Else
    '                Select Case reg.tiposql
    '                    Case "Int32", "Int64", "Int16", "Integer"
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & reg.nombre & " = New System.Windows.Forms.Label")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & " = New System.Windows.Forms.Button")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & " = New System.Windows.Forms.ComboBox")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & " = New System.Windows.Forms.Label")
    '                    Case "String", "Decimal"
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & " = New System.Windows.Forms.TextBox")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & " = New System.Windows.Forms.Label")
    '                    Case "DateTime"
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.dtp" & reg.nombre & " = New System.Windows.Forms.DateTimePicker")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & " = New System.Windows.Forms.Label")
    '                    Case "Boolean"
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.chk" & reg.nombre & " = New System.Windows.Forms.CheckBox")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & " = New System.Windows.Forms.Label")
    '                    Case Else
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & " = New System.Windows.Forms.TextBox")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & " = New System.Windows.Forms.Label")
    '                End Select
    '            End If
    '        Next
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.SuspendLayout()")

    '        Dim tabID As Integer = 500
    '        Dim tabBtn As Integer = 30
    '        Dim tabIndex As Integer = 4
    '        Dim posY As Integer = 0

    '        Dim col As Integer = 1

    '        For Each reg In arrEstructura
    '            If reg.Orden = 1 Or reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
    '                'next row
    '            Else

    '                If posY = 0 Then
    '                    posY = 48
    '                End If

    '                Select Case reg.tiposql
    '                    Case "Int32", "Int64", "Int16", "Integer"
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'lbl" & reg.nombre)
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & reg.nombre & ".AutoSize = True")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & reg.nombre & ".BackColor = System.Drawing.Color.Red")
    '                        If col = 1 Then
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & reg.nombre & ".Location = New System.Drawing.Point(370, " & posY & ")")
    '                        Else
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & reg.nombre & ".Location = New System.Drawing.Point(925, " & posY & ")")
    '                        End If
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & reg.nombre & ".Name = ""lbl" & reg.nombre & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & reg.nombre & ".Size = New System.Drawing.Size(13, 13)")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & reg.nombre & ".TabIndex = " & tabID)
    '                        tabID = tabID + 1
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & reg.nombre & ".Text = ""0""")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.lbl" & reg.nombre & ".Visible = False")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'btnBusca" & reg.nombre.Substring(3))
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Cursor = System.Windows.Forms.Cursors.Hand")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Image = CType(Resources.GetObject(""btnBusca1.Image""), System.Drawing.Image)")
    '                        If col = 1 Then
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Location = New System.Drawing.Point(505, " & posY & ")")
    '                        Else
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Location = New System.Drawing.Point(1060, " & posY & ")")
    '                        End If
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Name = ""btnBusca" & reg.nombre.Substring(3) & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Size = New System.Drawing.Size(41, 36)")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".TabIndex = " & tabBtn)
    '                        tabBtn = tabBtn + 1
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".UseVisualStyleBackColor = True")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'cmb" & reg.nombre.Substring(3))
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".FormattingEnabled = True")
    '                        If col = 1 Then
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".Location = New System.Drawing.Point(161, " & posY & ")")
    '                        Else
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".Location = New System.Drawing.Point(716, " & posY & ")")
    '                        End If
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".Name = ""cmb" & reg.nombre.Substring(3) & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".Size = New System.Drawing.Size(329, 21)")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".TabIndex = " & tabIndex)
    '                        tabIndex = tabIndex + 1
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'etiquieta" & reg.nombre)
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".ForeColor = System.Drawing.Color.Black")
    '                        If col = 1 Then
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(28, " & posY & ")")
    '                        Else
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(583, " & posY & ")")
    '                        End If
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Size = New System.Drawing.Size(127, 21)")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".TabIndex = " & tabID)
    '                        tabID = tabID + 1
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Text = """ & reg.nombre & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                    Case "String"
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'txt" & reg.nombre)
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & ".BackColor = System.Drawing.Color.White")
    '                        If col = 1 Then
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & ".Location = New System.Drawing.Point(161, " & posY & ")")
    '                        Else
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & ".Location = New System.Drawing.Point(716, " & posY & ")")
    '                        End If
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & ".MaxLength = 50")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & ".Name = ""txt" & reg.nombre & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & ".Size = New System.Drawing.Size(385, 20)")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & ".TabIndex = " & tabIndex)
    '                        tabIndex = tabIndex + 1
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'etiquieta" & reg.nombre)
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".ForeColor = System.Drawing.Color.Black")
    '                        If col = 1 Then
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(28, " & posY & ")")
    '                        Else
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(583, " & posY & ")")
    '                        End If
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Size = New System.Drawing.Size(127, 21)")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".TabIndex = " & tabID)
    '                        tabID = tabID + 1
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Text = """ & reg.nombre & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                    Case "Decimal"
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'txt" & reg.nombre)
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & ".BackColor = System.Drawing.Color.White")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & ".ForeColor = System.Drawing.Color.Blue")
    '                        If col = 1 Then
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & ".Location = New System.Drawing.Point(161, " & posY & ")")
    '                        Else
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & ".Location = New System.Drawing.Point(716, " & posY & ")")
    '                        End If
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & ".MaxLength = 50")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & ".Name = ""txt" & reg.nombre & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & ".Size = New System.Drawing.Size(222, 31)")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & ".TabIndex = " & tabIndex)
    '                        tabIndex = tabIndex + 1
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.txt" & reg.nombre & ".TextAlign = System.Windows.Forms.HorizontalAlignment.Right")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'etiquieta" & reg.nombre)
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".ForeColor = System.Drawing.Color.Black")
    '                        If col = 1 Then
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(28, " & posY & ")")
    '                        Else
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(583, " & posY & ")")
    '                        End If
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Size = New System.Drawing.Size(127, 21)")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".TabIndex = " & tabID)
    '                        tabID = tabID + 1
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Text = """ & reg.nombre & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                    Case "DateTime"
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'dtp" & reg.nombre)
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.dtp" & reg.nombre & ".Format = System.Windows.Forms.DateTimePickerFormat.[Short]")
    '                        If col = 1 Then
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.dtp" & reg.nombre & ".Location = New System.Drawing.Point(161, " & posY & ")")
    '                        Else
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.dtp" & reg.nombre & ".Location = New System.Drawing.Point(716, " & posY & ")")
    '                        End If
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.dtp" & reg.nombre & ".Name = ""cmb" & reg.nombre & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.dtp" & reg.nombre & ".Size = New System.Drawing.Size(93, 20)")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.dtp" & reg.nombre & ".TabIndex = " & tabIndex)
    '                        tabIndex = tabIndex + 1
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'etiquieta" & reg.nombre)
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".ForeColor = System.Drawing.Color.Black")
    '                        If col = 1 Then
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(28, " & posY & ")")
    '                        Else
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(583, " & posY & ")")
    '                        End If
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Size = New System.Drawing.Size(127, 21)")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".TabIndex = " & tabID)
    '                        tabID = tabID + 1
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Text = """ & reg.nombre & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                    Case "Boolean"
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'chk" & reg.nombre)
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.chk" & reg.nombre & ".AutoSize = True")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.chk" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '                        If col = 1 Then
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.chk" & reg.nombre & ".Location = New System.Drawing.Point(161, " & posY & ")")
    '                        Else
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.chk" & reg.nombre & ".Location = New System.Drawing.Point(716, " & posY & ")")
    '                        End If
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.chk" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.chk" & reg.nombre & ".Size = New System.Drawing.Size(15, 14)")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.chk" & reg.nombre & ".TabIndex = " & tabIndex)
    '                        tabIndex = tabIndex + 1
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.chk" & reg.nombre & ".Text = """ & reg.nombre & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.chk" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'etiquieta" & reg.nombre)
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".ForeColor = System.Drawing.Color.Black")
    '                        If col = 1 Then
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(28, " & posY & ")")
    '                        Else
    '                            PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(583, " & posY & ")")
    '                        End If
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Size = New System.Drawing.Size(127, 21)")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".TabIndex = " & tabID)
    '                        tabID = tabID + 1
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".Text = """ & reg.nombre & """")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.etiqueta" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
    '                        PrintLine(FileFrmDetalle, TAB(8), "'")
    '                    Case Else
    '                End Select
    '                'incremento la posicion
    '                If reg.tiposql = "Decimal" Then
    '                    posY = posY + 53
    '                Else
    '                    posY = posY + 43
    '                End If

    '                If posY > 568 Then
    '                    col = 2
    '                    posY = 0
    '                End If

    '            End If
    '        Next
    '        'posY = posY + 43
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'btnSalir")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.ForeColor = System.Drawing.Color.Black")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Image = CType(Resources.GetObject(""btnSalir.Image""), System.Drawing.Image)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '        If col = 1 Then
    '            PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Location = New System.Drawing.Point(499, " & posY + 40 & ")")
    '        Else
    '            PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Location = New System.Drawing.Point(1049, 653)")
    '        End If
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Name = ""btnSalir""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Size = New System.Drawing.Size(81, 69)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.TabIndex = 21")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.Text = ""&Salir""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnSalir.UseVisualStyleBackColor = True")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'btnGuardar")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.ForeColor = System.Drawing.Color.Black")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Image = CType(Resources.GetObject(""btnGuardar.Image""), System.Drawing.Image)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
    '        If col = 1 Then
    '            PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Location = New System.Drawing.Point(392," & posY + 40 & ")")
    '        Else
    '            PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Location = New System.Drawing.Point(947, 653)")
    '        End If
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Name = ""btnGuardar""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Size = New System.Drawing.Size(78, 69)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.TabIndex = 20")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.Text = ""&Guardar""")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.btnGuardar.UseVisualStyleBackColor = True")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'GroupBox1")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.Location = New System.Drawing.Point(12, 17)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.Name = ""GroupBox1""")
    '        If col = 1 Then
    '            PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.Size = New System.Drawing.Size(568, " & posY & ")")
    '        Else
    '            PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.Size = New System.Drawing.Size(1118, 606)")
    '        End If
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.TabIndex = 0")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.GroupBox1.TabStop = False")
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "'frmDetalle" & Me.txtTabla.Text)
    '        PrintLine(FileFrmDetalle, TAB(8), "'")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font")
    '        If col = 1 Then
    '            PrintLine(FileFrmDetalle, TAB(8), "Me.ClientSize = New System.Drawing.Size(600, " & posY + 120 & ")")
    '        Else
    '            PrintLine(FileFrmDetalle, TAB(8), "Me.ClientSize = New System.Drawing.Size(1150, 768)")
    '        End If
    '        'control.add
    '        For Each reg In arrEstructura
    '            If reg.Orden = 1 Or reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
    '                'next row
    '            Else
    '                Select Case reg.tiposql
    '                    Case "Int32", "Int64", "Int16", "Integer"
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.lbl" & reg.nombre & ")")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.btnBusca" & reg.nombre.Substring(3) & ")")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.cmb" & reg.nombre.Substring(3) & ")")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.etiqueta" & reg.nombre & ")")
    '                    Case "String", "Decimal"
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.txt" & reg.nombre & ")")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.etiqueta" & reg.nombre & ")")
    '                    Case "DateTime"
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.dtp" & reg.nombre & ")")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.etiqueta" & reg.nombre & ")")
    '                    Case "Boolean"
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.chk" & reg.nombre & ")")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.etiqueta" & reg.nombre & ")")
    '                    Case Else
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me." & reg.nombre & ")")
    '                        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.etiqueta" & reg.nombre & ")")
    '                End Select
    '            End If
    '        Next
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.btnSalir)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.btnGuardar)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Controls.Add(Me.GroupBox1)")

    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Name = ""frmDetalle" & Me.txtTabla.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.Text = ""frmDetalle" & Me.txtTabla.Text & """")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.ResumeLayout(False)")
    '        PrintLine(FileFrmDetalle, TAB(8), "Me.PerformLayout()")
    '        PrintLine(FileFrmDetalle, "")
    '        PrintLine(FileFrmDetalle, TAB(4), "End Sub")

    '        'withevents
    '        For Each reg In arrEstructura
    '            If reg.Orden = 1 Or reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
    '                'next row
    '            Else
    '                Select Case reg.tiposql
    '                    Case "Int32", "Int64", "Int16", "Integer"
    '                        PrintLine(FileFrmDetalle, TAB(4), "Friend WithEvents lbl" & reg.nombre & " As System.Windows.Forms.Label")
    '                        PrintLine(FileFrmDetalle, TAB(4), "Friend WithEvents btnBusca" & reg.nombre.Substring(3) & " As System.Windows.Forms.Button")
    '                        PrintLine(FileFrmDetalle, TAB(4), "Friend WithEvents cmb" & reg.nombre.Substring(3) & " As System.Windows.Forms.ComboBox")
    '                        PrintLine(FileFrmDetalle, TAB(4), "Friend WithEvents etiqueta" & reg.nombre & " As System.Windows.Forms.Label")
    '                    Case "String", "Decimal"
    '                        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents txt" & reg.nombre & " As System.Windows.Forms.TextBox")
    '                        PrintLine(FileFrmDetalle, TAB(4), "Friend WithEvents etiqueta" & reg.nombre & " As System.Windows.Forms.Label")
    '                    Case "DateTime"
    '                        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents dtp" & reg.nombre & " As System.Windows.Forms.DateTimePicker")
    '                        PrintLine(FileFrmDetalle, TAB(4), "Friend WithEvents etiqueta" & reg.nombre & " As System.Windows.Forms.Label")
    '                    Case "Boolean"
    '                        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents chk" & reg.nombre & " As System.Windows.Forms.CheckBox")
    '                        PrintLine(FileFrmDetalle, TAB(4), "Friend WithEvents etiqueta" & reg.nombre & " As System.Windows.Forms.Label")
    '                    Case Else
    '                        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents txt" & reg.nombre & " As System.Windows.Forms.TextBox")
    '                        PrintLine(FileFrmDetalle, TAB(4), "Friend WithEvents etiqueta" & reg.nombre & " As System.Windows.Forms.Label")
    '                End Select
    '            End If
    '        Next

    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents btnSalir As System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents btnGuardar As System.Windows.Forms.Button")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents GroupBox1 As System.Windows.Forms.GroupBox")
    '        PrintLine(FileFrmDetalle, TAB(4), "Protected WithEvents ttGeneral As System.Windows.Forms.ToolTip")
    '        PrintLine(FileFrmDetalle, TAB(0), "End Class")
    '        PrintLine(FileFrmDetalle, "")


    '        FileClose(FileFrmDetalle)

    '    End If

    'End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Me.txtPath.Text = "" Then
            MessageBox.Show("HAY QUE PONER UN PATH")
            Exit Sub
        End If

        ' Verifico si se trata de una o todas las tablas
        If Not chkTodasLasTablas.Checked Then
            ' Solicito confirmación antes de crear la clase
            If MessageBox.Show("Desea Crear la Clase '" & txtClase.Text & "' Para la Tabla '" & txtTabla.Text & "' ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                ' Cargo la estructura de la tabla en un array "modular"
                Call Cargar(txtCadena.Text, txtTabla.Text)

                ' Genero la clase
                '  Call GenerarClase2(txtArchivo.Text, txtClase.Text)

                oClase.Iniciar()
                oClase.ClaseAgregada(Me.txtPath.Text, Me.txtTabla.Text)

                ' Indico que se ha generado la clase
                MessageBox.Show("Se Generó la Clase '" & txtClase.Text & "' en el Archivo '" & txtArchivo.Text & "' de la Tabla '" & txtTabla.Text & "'." & Chr(13) & Chr(13) & "El Archivo se Guardó en C:\." & Chr(13) & Chr(13) & "Copiar la Clase en el Proyecto y Agregarla con --> Agregar elemento existente", "GENERACION DE CLASES", MessageBoxButtons.OK, MessageBoxIcon.Information)
                '  Me.btnSP2.Enabled = True
            End If
        Else
            Dim cn As New SqlConnection(Me.txtCadenaConexion.Text)
            Dim da As New SqlDataAdapter("SELECT TOP 100 PERCENT name FROM dbo.sysobjects WHERE (type = 'U') AND name <> 'dtproperties' ORDER BY name", cn)

            Dim cb As New SqlCommandBuilder(da)
            Dim cbSistema As New SqlCommandBuilder(da)
            Dim dt As New DataTable
            da.Fill(dt)
            ' Solicito confirmación antes de crear todas las tablas
            If MessageBox.Show("Desea las Clases Para Todas las Tablas de la Base de Datos Seleccionada ?", "CONFIRMACION", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Me.ProgressBar1.Value = Me.ProgressBar1.Minimum
                Me.ProgressBar1.Maximum = dt.Rows.Count - 1
                Me.ProgressBar2.Value = Me.ProgressBar2.Minimum
                Me.ProgressBar2.Maximum = dt.Rows.Count - 1

                Dim indice = 0
                For Each row As DataRow In dt.Rows
                    txtTabla.Text = row("name")
                    Me.GenerarNombres()
                    Call Cargar(txtCadena.Text, txtTabla.Text)

                    oClase.Iniciar()
                    oClase.ClaseAgregada(Me.txtPath.Text, Me.txtTabla.Text)

                    Me.ProgressBar1.Value = indice
                    Me.ProgressBar2.Value = indice

                    indice = indice + 1
                Next
                MessageBox.Show("Se Generó la Clase Para Todas las Tablas de la Base de Datos." & Chr(13) & Chr(13) & "Los Archivos se Guardaron en C:\." & Chr(13) & Chr(13) & "Copiar la Clase en el Proyecto y Agregarla con --> Agregar elemento existente", "GENERACION DE CLASES", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' Me.btnTodoSP2.Enabled = True
            End If
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.FolderBrowserDialog1.ShowDialog()
        ' Me.txtPath_2.Text = Me.FolderBrowserDialog1.SelectedPath
    End Sub

    Private Sub chk08_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk08.CheckedChanged
        If Me.chk08.Checked Then
            Me.cmbServer.Text = "(local)\SQLExpress08"
            Me.btnBaseDato.Enabled = True
        Else
            Me.cmbServer.Text = ""
            Me.btnBaseDato.Enabled = False
        End If
    End Sub

    Private Sub chk2008_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chk2008.CheckedChanged
        If Me.chk2008.Checked Then
            Me.cmbServer.Text = "(local)\SQLExpress2008"
            Me.btnBaseDato.Enabled = True
        Else
            Me.cmbServer.Text = ""
            Me.btnBaseDato.Enabled = False
        End If
    End Sub

    Private Sub btnAbrir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbrir.Click
        Try
            With ofdScript
                ' Vea btnRetriveFileNames_Click para obtener una explicación de los valores predeterminados
                ' de las propiedades.

                ' Comprobar que el archivo seleccionado existe. El cuadro de diálogo muestra
                ' una advertencia en caso contrario.
                .CheckFileExists = True

                ' Comprobar que la ruta de acceso seleccionada existe. El cuadro de diálogo muestra 
                ' una advertencia en caso contrario.
                .CheckPathExists = True

                ' Obtener o establecer una extensión predeterminada. No incluye el "." inicial.
                .DefaultExt = "sql"

                ' ¿Devolver el archivo al que hace referencia un vínculo? Si False, devuelve el archivo de vínculo
                ' seleccionado. Si True, devuelve el archivo vinculado al archivo LNK.
                .DereferenceLinks = True

                ' Al igual que en VB6, utilice un conjunto de pares de filtros, separados por "|". Cada
                ' par consta de una especificación descripción|archivo. Utilice "|" entre los pares. No es necesario
                ' poner "|" al final. Puede establecer la propiedad FilterIndex también, para seleccionar el
                ' filtro predeterminado. El primer filtro tiene el número 1 (no 0). El valor predeterminado es 1. 
                .Filter = _
                "Archivos SQL (*.sql)|*.sql" '|All files|*.*"

                ' .Multiselect = False

                ' ¿Restaurar el directorio original después de seleccionar
                ' un archivo? Si False, el directorio actual cambia
                ' al directorio en el que seleccionó el archivo.
                ' Establézcalo como True para poner la carpeta actual de nuevo
                ' donde estaba cuando comenzó.
                ' El valor predeterminado es False.
                .RestoreDirectory = True

                ' ¿Mostrar el botón Ayuda y la casilla de verificación Sólo lectura?
                .ShowHelp = True
                .ShowReadOnly = False

                ' ¿Comenzar con la casilla de verificación Sólo lectura activada?
                ' Esto sólo tiene sentido si ShowReadOnly es True.
                .ReadOnlyChecked = False

                .Title = "Select a file to open"

                ' ¿Aceptar sólo nombres de archivo Win32 válidos?
                .ValidateNames = True

                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                    Try
                        txtPath3.Text = .FileName
                        '  txtFileContents.Text = My.Computer.FileSystem.ReadAllText(.FileName)
                    Catch fileException As Exception
                        Throw fileException
                    End Try
                End If

            End With

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text)
        End Try
    End Sub


    Private Sub btnRunScript_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunScript.Click
        Dim Indice As Integer = 0
        Dim fileA, files() As String
        files = Me.ofdScript.FileNames

        Me.ProgressBar2.Maximum = files.Length - 1

        For Each fileA In files
            Dim PathArchivo As String = fileA.ToString
            Dim fi As FileInfo = New FileInfo(PathArchivo)
            Dim File As StreamReader = fi.OpenText()
            Dim File2 As StreamReader = fi.OpenText()
            BanderaSP = 0

            Me.ProgressBar2.Value = Indice

            NumeroLinea = Me.CountLines(File2)
            Me.ProgressBar1.Value = Me.ProgressBar1.Minimum
            Me.ProgressBar1.Maximum = NumeroLinea

            ParseStreamAndActionScript(File)
            Indice = Indice + 1
        Next

        If BanderaSP = 0 Then
            MessageBox.Show("Los Comandos se Ejecutaron Correctamente", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            MessageBox.Show("Se ha Producido un Error en la Ejecución ", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub


    Private Sub btnCodigo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigo.Click
        Dim frmTemp As New frmCodigo

        Me.AddOwnedForm(frmTemp)
        frmTemp.ShowDialog()
    End Sub

    Private Sub btnBuscar2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar2.Click
        Me.FolderBrowserDialog2.ShowDialog()
        Me.txtPath4.Text = Me.FolderBrowserDialog2.SelectedPath
    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim path As String = Me.txtPath3.Text

        Dim readText As String = File.ReadAllText(path)
        File.AppendAllText(Me.txtDestino.Text, readText)
    End Sub

    Private Sub Button3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub


End Class
