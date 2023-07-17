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

Public Class Form2

    Dim valor As String = ""
    Dim BanderaSP As Integer = 0
    Dim BanderaList As Integer = 0
    Dim InnerJoin As String = ""

    Dim pathAnterior As String = ""

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.txtCadena.Text = CType(Me.Owner, frmPrincipal).txtCadenaConexion.Text.Trim()
        Me.txtBaseDato.Text = CType(Me.Owner, frmPrincipal).lstBaseDato.SelectedValue.ToString
        Me.GetTabla()

        pathAnterior = CType(Me.Owner, frmPrincipal).txtPath.Text
    End Sub

    Sub GetTabla()
        Dim ocn As New SqlConnection(Me.txtCadena.Text)
        Dim oda As New SqlDataAdapter("SELECT TOP 100 PERCENT name, id FROM dbo.sysobjects WHERE (type = 'U') AND name <> 'dtproperties' ORDER BY name", ocn)

        Dim ocb As New SqlCommandBuilder(oda)
        Dim ocbSistema As New SqlCommandBuilder(oda)
        Dim odt As New DataTable
        oda.Fill(odt)

        With Me.cmbTabla
            .DataSource = odt
            .DisplayMember = "name"
            .ValueMember = "id"
        End With
        If Me.cmbTabla.SelectedIndex.ToString >= 0 Then
            cmbTabla.SelectedIndex = 0
            Me.lblid_tabla.Text = cmbTabla.SelectedValue.ToString
        End If
    End Sub

    Sub GetColumnas(ByVal id_tabla As Integer)
        Dim ocn As New SqlConnection(Me.txtCadena.Text)
        Dim oda As New SqlDataAdapter("SELECT  * FROM  syscolumns WHERE  (id = " & id_tabla & ")", ocn)

        Dim ocb As New SqlCommandBuilder(oda)
        Dim ocbSistema As New SqlCommandBuilder(oda)
        Dim odt As New DataTable
        oda.Fill(odt)

        Me.CheckedListBox1.Items.Clear()
        For i As Integer = 0 To odt.Rows.Count - 1
            Me.CheckedListBox1.Items.Add(odt.Rows(i).Item("name"))
        Next
    End Sub

    Sub GetColumnas2(ByVal id_tabla As Integer, ByVal nombre_tabla As String)
        Dim ocn As New SqlConnection(Me.txtCadena.Text)
        Dim oda As New SqlDataAdapter("SELECT  * FROM  syscolumns WHERE  (id = " & id_tabla & ")", ocn)

        Dim ocb As New SqlCommandBuilder(oda)
        Dim ocbSistema As New SqlCommandBuilder(oda)
        Dim odt As New DataTable
        oda.Fill(odt)

        For i As Integer = 0 To odt.Rows.Count - 1
            Me.CheckedListBox2.Items.Add(nombre_tabla & odt.Rows(i).Item("name"))
        Next
    End Sub

    Function GetIdTabla(ByVal nombre As String) As Integer
        Dim ocn As New SqlConnection(Me.txtCadena.Text)
        Dim oda As New SqlDataAdapter("SELECT id FROM sysobjects WHERE name = '" & nombre & "'", ocn)
        Dim i As Integer

        Dim ocb As New SqlCommandBuilder(oda)
        Dim ocbSistema As New SqlCommandBuilder(oda)
        Dim odt As New DataTable
        oda.Fill(odt)
        i = odt.Rows(0).Item("id")
        Return i
    End Function

    Private Sub txtCampo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCampo.KeyPress
        Select Case e.KeyChar.ToString
            Case 5 'boolean
                valor = "] [bit] NULL,"
                Me.txtCampo.BackColor = Color.Red
                Me.lblTipoDato.Text = "BOOLEAN"
                Me.lblTipoDato.Visible = True
                e.Handled = True
                BanderaList = 1
            Case 6 'entero
                valor = "] [int] NULL,"
                Me.txtCampo.BackColor = Color.Red
                Me.lblTipoDato.Text = "ENTERO"
                Me.lblTipoDato.Visible = True
                e.Handled = True
                BanderaList = 1
            Case 7 'varchar
                valor = "] [varchar](" & Me.txtDimension.Text.Trim & ") COLLATE Modern_Spanish_CI_AS NULL,"
                Me.txtCampo.BackColor = Color.Red
                Me.lblTipoDato.Text = "CARACTER"
                Me.lblTipoDato.Visible = True
                e.Handled = True
                BanderaList = 1
            Case 8 'decimal
                valor = "] [decimal](18, 2) NULL,"
                Me.txtCampo.BackColor = Color.Red
                Me.lblTipoDato.Text = "DECIMAL"
                Me.lblTipoDato.Visible = True
                e.Handled = True
                BanderaList = 1
            Case 9 'datetime
                valor = "] [datetime] NULL,"
                Me.txtCampo.BackColor = Color.Red
                Me.lblTipoDato.Text = "FECHA"
                Me.lblTipoDato.Visible = True
                e.Handled = True
                BanderaList = 1
            Case vbCr
                If Me.txtCampo.Text = "" Or BanderaList = 0 Then
                    Exit Sub
                End If
                Me.ListBox1.Items.Add("[" & Me.txtCampo.Text.Trim & valor)
                Me.ListBox2.Items.Add(Me.txtCampo.Text)
                valor = ""
                Me.txtCampo.Text = ""
                Me.txtCampo.Focus()
                Me.txtCampo.BackColor = Color.White
                Me.lblTipoDato.Text = ""
                Me.lblTipoDato.Visible = False
                BanderaList = 0
        End Select
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim PathInsert As String

        PathInsert = pathAnterior & "CREATE_" & Me.txtTabla.Text & ".sql"

        ' Defino variables
        Dim FileInsert As Integer = FreeFile()
        ' Dim regInsert As regSistemaTabla

        ' Abro un PathCls de texto (si el mismo existía se reempleza)
        FileOpen(FileInsert, PathInsert, OpenMode.Output)
        Dim Prefijo As String = "sp_"


        '*********************************************
        '
        'DEFINICION DEL CODIGO PARA LOS SP
        '
        '*********************************************

        'sp insert

        PrintLine(FileInsert, TAB(0), "USE [" & Me.txtBaseDato.Text.Trim & "]")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, "")
        PrintLine(FileInsert, TAB(0), "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" & Me.txtTabla.Text.Trim & "]') AND type in (N'U'))")
        PrintLine(FileInsert, TAB(0), "DROP TABLE [dbo].[" & Me.txtTabla.Text.Trim & "]")
        PrintLine(FileInsert, TAB(0), "GO")
        'PrintLine(FileInsert, TAB(0), "SET ANSI_NULLS ON")
        'PrintLine(FileInsert, TAB(0), "GO")
        'PrintLine(FileInsert, TAB(0), "SET QUOTED_IDENTIFIER ON")
        'PrintLine(FileInsert, TAB(0), "GO")
        'PrintLine(FileInsert, TAB(0), "SET ANSI_PADDING ON")
        'PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "")
        PrintLine(FileInsert, TAB(0), "CREATE TABLE [dbo].[" & Me.txtTabla.Text.Trim & "](")
        PrintLine(FileInsert, TAB(4), "[" & Me.txtPK.Text.Trim & "] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,")

        For i As Integer = 0 To Me.ListBox1.Items.Count - 1
            PrintLine(FileInsert, TAB(4), Me.ListBox1.Items.Item(i).ToString.Trim())
        Next


        PrintLine(FileInsert, TAB(1), " CONSTRAINT [PK_" & Me.txtTabla.Text.Trim & "] PRIMARY KEY CLUSTERED ")
        PrintLine(FileInsert, TAB(0), "(")
        PrintLine(FileInsert, TAB(4), "[" & Me.txtPK.Text.Trim & "] ASC")
        PrintLine(FileInsert, TAB(0), ")WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]")
        PrintLine(FileInsert, TAB(0), ") ON [PRIMARY]")
        'PrintLine(FileInsert, "")
        PrintLine(FileInsert, TAB(0), "GO")
        'PrintLine(FileInsert, TAB(0), "SET ANSI_PADDING OFF")
        PrintLine(FileInsert, "")

        FileClose(FileInsert)

        Dim PathArchivo As String = pathAnterior & "CREATE_" & Me.txtTabla.Text & ".sql"
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
        File.Close()
        File2.Close()
    End Sub

    Dim NumeroLinea As Integer = 0

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


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub TabulacionTextBox(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBaseDato.KeyPress, txtTabla.KeyPress, txtPK.KeyPress
        If e.KeyChar.ToString = vbCr Then
            Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.ListBox1.Items.RemoveAt(Me.ListBox2.SelectedIndex)
        Me.ListBox2.Items.RemoveAt(Me.ListBox2.SelectedIndex)
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.ListBox1.Items.Clear()
        Me.ListBox2.Items.Clear()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Me.ListBox1.Items.Clear()
        Me.ListBox2.Items.Clear()
        Me.txtTabla.Text = ""
        Me.txtPK.Text = ""
        Me.txtCampo.Text = ""
        Me.txtTabla.Focus()
    End Sub

    Private Sub btnPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPath.Click
        Try
            With fbdDB
                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                    Try
                        txtPath.Text = .SelectedPath
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

    Private Sub btnMakeDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMakeDB.Click
        Dim PathInsert As String

        PathInsert = pathAnterior & "sp_CREAR_DB.sql"

        ' Defino variables
        Dim FileInsert As Integer = FreeFile()
        ' Dim regInsert As regSistemaTabla
        ' Abro un PathCls de texto (si el mismo existía se reempleza)
        FileOpen(FileInsert, PathInsert, OpenMode.Output)



        PrintLine(FileInsert, TAB(0), "USE [master]")
        PrintLine(FileInsert, TAB(0), "GO")

        PrintLine(FileInsert, TAB(0), "CREATE DATABASE [" & Me.txtDBNew.Text.Trim() & "] ON  PRIMARY")
        PrintLine(FileInsert, TAB(0), "( NAME = N'" & Me.txtDBNew.Text.Trim() & "', FILENAME = N'" & Me.txtPath.Text.Trim() & "\" & Me.txtDBNew.Text.Trim() & ".mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )")
        PrintLine(FileInsert, TAB(0), " LOG ON ")
        PrintLine(FileInsert, TAB(0), "( NAME = N'" & Me.txtDBNew.Text.Trim() & "_log', FILENAME = N'" & Me.txtPath.Text.Trim() & "\" & Me.txtDBNew.Text.Trim() & "_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)")
        PrintLine(FileInsert, TAB(0), " COLLATE Modern_Spanish_CI_AS")
        PrintLine(FileInsert, TAB(0), "GO")

        PrintLine(FileInsert, TAB(0), "EXEC dbo.sp_dbcmptlevel @dbname=N'" & Me.txtDBNew.Text.Trim() & "', @new_cmptlevel=90")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))")
        PrintLine(FileInsert, TAB(0), "begin")
        PrintLine(FileInsert, TAB(0), "EXEC [" & Me.txtDBNew.Text.Trim() & "].[dbo].[sp_fulltext_database] @action = 'enable'")
        PrintLine(FileInsert, TAB(0), "end")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET ANSI_NULL_DEFAULT OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET ANSI_NULLS OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET ANSI_PADDING OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET ANSI_WARNINGS OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET ARITHABORT OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET AUTO_CLOSE OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET AUTO_CREATE_STATISTICS ON")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET AUTO_SHRINK OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET AUTO_UPDATE_STATISTICS ON")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET CURSOR_CLOSE_ON_COMMIT OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET CURSOR_DEFAULT  GLOBAL")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET CONCAT_NULL_YIELDS_NULL OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET NUMERIC_ROUNDABORT OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET QUOTED_IDENTIFIER OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET RECURSIVE_TRIGGERS OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET  ENABLE_BROKER")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET AUTO_UPDATE_STATISTICS_ASYNC OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET DATE_CORRELATION_OPTIMIZATION OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET TRUSTWORTHY OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET ALLOW_SNAPSHOT_ISOLATION OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET PARAMETERIZATION SIMPLE")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET  READ_WRITE")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET RECOVERY SIMPLE")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET  MULTI_USER")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET PAGE_VERIFY CHECKSUM")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET DB_CHAINING OFF")
        PrintLine(FileInsert, TAB(0), "GO")

        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET QUOTED_IDENTIFIER OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET RECURSIVE_TRIGGERS OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET AUTO_UPDATE_STATISTICS_ASYNC OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET DATE_CORRELATION_OPTIMIZATION OFF")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET PARAMETERIZATION SIMPLE")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET  READ_WRITE")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET RECOVERY SIMPLE")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET  MULTI_USER")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] SET PAGE_VERIFY CHECKSUM")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "USE [" & Me.txtDBNew.Text.Trim() & "]")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "IF NOT EXISTS (SELECT name FROM sys.filegroups WHERE is_default=1 AND name = N'PRIMARY') ALTER DATABASE [" & Me.txtDBNew.Text.Trim() & "] MODIFY FILEGROUP [PRIMARY] DEFAULT")
        PrintLine(FileInsert, TAB(0), "GO")


        PrintLine(FileInsert, "")

        FileClose(FileInsert)

        Dim PathArchivo As String = pathAnterior & "sp_CREAR_DB.sql"
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
        File.Close()
        File2.Close()

    End Sub

    Private Sub cmbTabla_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTabla.SelectedIndexChanged
        If Me.cmbTabla.SelectedIndex >= 0 Then
            Me.lblid_tabla.Text = cmbTabla.SelectedValue.ToString
        End If
        If Not IsNumeric(Me.lblid_tabla.Text) Then
            Exit Sub
        End If
        Me.GetColumnas(Me.lblid_tabla.Text)
        InnerJoin = "dbo." & Me.cmbTabla.Text
    End Sub

    Private Sub btnGetColumns_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetColumns.Click
        Dim i As Integer
        For i = 0 To Me.CheckedListBox1.CheckedItems.Count - 1
            Dim PathInsert As String

            PathInsert = pathAnterior & "sp_DROP_COLUMN.sql"

            ' Defino variables
            Dim FileInsert As Integer = FreeFile()
            ' Dim regInsert As regSistemaTabla

            ' Abro un PathCls de texto (si el mismo existía se reempleza)
            FileOpen(FileInsert, PathInsert, OpenMode.Output)
            Dim Prefijo As String = "sp_"


            '*********************************************
            '
            'DEFINICION DEL CODIGO PARA LOS SP
            '
            '*********************************************

            'codigo para borrar columnas

            PrintLine(FileInsert, TAB(0), "USE [" & Me.txtBaseDato.Text.Trim & "]")
            PrintLine(FileInsert, TAB(0), "GO")
            PrintLine(FileInsert, "")
            PrintLine(FileInsert, TAB(0), "BEGIN TRANSACTION")
            PrintLine(FileInsert, TAB(0), "GO")
            PrintLine(FileInsert, TAB(0), "ALTER TABLE " & Me.cmbTabla.Text.Trim)
            PrintLine(FileInsert, TAB(0), "DROP COLUMN " & Me.CheckedListBox1.CheckedItems(i))
            PrintLine(FileInsert, TAB(0), "GO")
            PrintLine(FileInsert, TAB(0), "COMMIT")

            FileClose(FileInsert)

            Dim PathArchivo As String = pathAnterior & "sp_DROP_COLUMN.sql"
            Dim fi As FileInfo = New FileInfo(PathArchivo)
            Dim File As StreamReader = fi.OpenText()
            Dim File2 As StreamReader = fi.OpenText()
            BanderaSP = 0

            NumeroLinea = Me.CountLines(File2)
            Me.ProgressBar1.Value = Me.ProgressBar1.Minimum
            Me.ProgressBar1.Maximum = NumeroLinea


            ParseStreamAndActionScript(File)

            File.Close()
            File2.Close()
            '  System.IO.File.Delete(PathArchivo)
        Next
        If BanderaSP = 0 Then
            MessageBox.Show("Los Comandos se Ejecutaron Correctamente", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            MessageBox.Show("Se ha Producido un Error en la Ejecución ", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub btnAdd_column_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd_column.Click
        Dim PathInsert As String

        PathInsert = pathAnterior & "sp_ADD_COLUMN.sql"

        ' Defino variables
        Dim FileInsert As Integer = FreeFile()
        ' Dim regInsert As regSistemaTabla

        ' Abro un PathCls de texto (si el mismo existía se reempleza)
        FileOpen(FileInsert, PathInsert, OpenMode.Output)
        Dim Prefijo As String = "sp_"


        '*********************************************
        '
        'DEFINICION DEL CODIGO PARA LOS SP
        '
        '*********************************************

        'codigo para modificar las tablas de la base de datos

        PrintLine(FileInsert, TAB(0), "USE [" & Me.txtBaseDato.Text.Trim & "]")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, "")

        PrintLine(FileInsert, TAB(0), "BEGIN TRANSACTION")
        PrintLine(FileInsert, TAB(0), "GO")

        PrintLine(FileInsert, TAB(0), "ALTER TABLE " & Me.cmbTabla.Text.Trim & " ADD")

        For i As Integer = 0 To Me.ListBox1.Items.Count - 1
            If i = Me.ListBox1.Items.Count - 1 Then
                Dim last As Integer = Me.ListBox1.Items.Item(i).ToString.Length - 1
                PrintLine(FileInsert, TAB(4), Me.ListBox1.Items.Item(i).ToString.Remove(last, 1))
            Else
                PrintLine(FileInsert, TAB(4), Me.ListBox1.Items.Item(i).ToString.Trim())
            End If
        Next

        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, TAB(0), "COMMIT")

        PrintLine(FileInsert, "")

        FileClose(FileInsert)

        Dim PathArchivo As String = pathAnterior & "sp_ADD_COLUMN.sql"
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
        File.Close()
        File2.Close()
    End Sub

    Private Sub btnSeleccionCampo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionCampo.Click
        Dim i As Integer
        Dim j As Integer
        Dim tabla2 As String

        Me.CheckedListBox2.Items.Clear()
        Me.GetColumnas2(Me.lblid_tabla.Text, "dbo." & Me.cmbTabla.Text.Trim & ".")

        For i = 0 To Me.CheckedListBox1.CheckedItems.Count - 1
            tabla2 = Me.CheckedListBox1.CheckedItems(i).ToString.Substring(3)
            j = Me.GetIdTabla(tabla2)
            Me.GetColumnas2(j, "dbo." & tabla2.ToUpper & ".")
            InnerJoin = InnerJoin & " INNER JOIN " & vbCrLf & "   dbo." & tabla2.ToUpper & " ON dbo." & Me.cmbTabla.Text.Trim & ".id_" & tabla2 & " = dbo." & tabla2.ToUpper & ".id_" & tabla2
        Next
    End Sub

    'devuelve true si row es varchar
    Function EsString(ByVal odtTemp As DataTable, ByVal strTemp As String) As Boolean
        Dim blnTemp As Boolean = False
        For Each row As DataRow In odtTemp.Rows
            If row.Item(0).ToString = strTemp Then
                blnTemp = True
            End If
        Next
        Return blnTemp
    End Function

    Private Sub btnGetAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetAll.Click
        If Me.txtPath.Text = "" Then
            MessageBox.Show("DEBE SELECCIONAR UN PATH")
            Exit Sub
        End If

        'variables para traer los varchar
        Dim ocn As New SqlConnection(Me.txtCadena.Text)
        Dim oda As New SqlDataAdapter("SELECT  DISTINCT name FROM  syscolumns WHERE typestat=2 and xtype=167", ocn)

        Dim ocb As New SqlCommandBuilder(oda)
        Dim ocbSistema As New SqlCommandBuilder(oda)
        Dim odt As New DataTable
        oda.Fill(odt)



        Dim PathInsert As String
        Dim i As Integer = 0

        PathInsert = pathAnterior & "cop_" & Me.cmbTabla.Text & "_GetAll_2.sql"

        ' Defino variables
        Dim FileInsert As Integer = FreeFile()
        ' Dim regInsert As regSistemaTabla

        ' Abro un PathCls de texto (si el mismo existía se reempleza)
        FileOpen(FileInsert, PathInsert, OpenMode.Output)
        Dim Prefijo As String = "cop_"


        '*********************************************
        '
        'DEFINICION DEL CODIGO PARA LOS SP
        '
        '*********************************************

        Dim strTemp As String = ""
        Dim strTemp2 As String = ""
        Dim blnExiste As Boolean = False

        'codigo para modificar el getall

        PrintLine(FileInsert, TAB(0), "USE [" & Me.txtBaseDato.Text.Trim & "]")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, "")

        'modificacion del getall
        PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.cmbTabla.Text & "_GetAll_2]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
        PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.cmbTabla.Text & "_GetAll_2]")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, "")
        PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.cmbTabla.Text & "_GetAll_2]")
        PrintLine(FileInsert, TAB(0), "AS")
        PrintLine(FileInsert, TAB(0), "SELECT TOP 100")

        For i = 0 To Me.CheckedListBox2.CheckedItems.Count - 1
            strTemp = Me.CheckedListBox2.CheckedItems.Item(i).ToString
            strTemp2 = strTemp.Substring(strTemp.LastIndexOf(".") + 1)
            blnExiste = Me.EsString(odt, strTemp2)

            If i = Me.CheckedListBox2.CheckedItems.Count - 1 Then
                If blnExiste Then
                    PrintLine(FileInsert, TAB(4), "RTRIM(" & strTemp.Trim & ") AS " & "[" & FormatoNombreLabels(strTemp2) & "]")
                Else
                    PrintLine(FileInsert, TAB(4), strTemp.Trim & " AS " & "[" & FormatoNombreLabels(strTemp2) & "]")
                End If
            Else
                If blnExiste Then
                    PrintLine(FileInsert, TAB(4), "RTRIM(" & strTemp.Trim & ") AS " & "[" & FormatoNombreLabels(strTemp2) & "]" & ",")
                Else
                    PrintLine(FileInsert, TAB(4), strTemp.Trim & " AS " & "[" & FormatoNombreLabels(strTemp2) & "]" & ",")
                End If
            End If
        Next

        PrintLine(FileInsert, TAB(0), "FROM")
        PrintLine(FileInsert, TAB(4), InnerJoin)

        If Me.chkServidor.Checked Then
            PrintLine(FileInsert, TAB(0), "WHERE")
            PrintLine(FileInsert, TAB(4), "dbo." & Me.cmbTabla.Text.Trim & ".operacion <> 'D'")
        End If

        PrintLine(FileInsert, TAB(0), "ORDER BY")
        PrintLine(FileInsert, TAB(4), Me.CheckedListBox2.SelectedItem)
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, "")


        ''modificacion del getprint
        'PrintLine(FileInsert, TAB(0), "ALTER PROCEDURE [dbo].[sp_" & Me.cmbTabla.Text.Trim & "_GetPrint]")
        'PrintLine(FileInsert, TAB(0), "AS")
        'PrintLine(FileInsert, "")
        'PrintLine(FileInsert, TAB(0), "SELECT")

        'For i = 0 To Me.CheckedListBox2.CheckedItems.Count - 1
        '    strTemp = Me.CheckedListBox2.CheckedItems.Item(i).ToString
        '    strTemp2 = strTemp.Substring(strTemp.LastIndexOf(".") + 1)
        '    blnExiste = Me.EsString(odt, strTemp2)

        '    If i = Me.CheckedListBox2.CheckedItems.Count - 1 Then
        '        If blnExiste Then
        '            PrintLine(FileInsert, TAB(4), "RTRIM(" & strTemp.Trim & ") AS " & strTemp2.ToUpper)
        '        Else
        '            PrintLine(FileInsert, TAB(4), strTemp.Trim & " AS " & strTemp2.ToUpper)
        '        End If
        '    Else
        '        If blnExiste Then
        '            PrintLine(FileInsert, TAB(4), "RTRIM(" & strTemp.Trim & ") AS " & strTemp2.ToUpper & ",")
        '        Else
        '            PrintLine(FileInsert, TAB(4), strTemp.Trim & " AS " & strTemp2.ToUpper & ",")
        '        End If
        '    End If
        'Next

        'PrintLine(FileInsert, TAB(0), "FROM")
        'PrintLine(FileInsert, TAB(4), InnerJoin)

        'PrintLine(FileInsert, TAB(0), "ORDER BY")
        'PrintLine(FileInsert, TAB(4), Me.CheckedListBox2.SelectedItem)

        'PrintLine(FileInsert, TAB(0), "GO")

        'PrintLine(FileInsert, "")


        'modificacion del find
        PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.cmbTabla.Text & "_Find_2]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
        PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.cmbTabla.Text & "_Find_2]")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, "")
        PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.cmbTabla.Text & "_Find_2]")
        PrintLine(FileInsert, TAB(5), "@nombre NVARCHAR (30)=NULL")
        PrintLine(FileInsert, TAB(0), "AS SET NOCOUNT ON")
        PrintLine(FileInsert, "")
        PrintLine(FileInsert, TAB(0), "IF @nombre IS NOT NULL")
        PrintLine(FileInsert, TAB(0), "BEGIN")
        PrintLine(FileInsert, TAB(5), "SELECT @nombre=RTRIM(@nombre)+'%'")
        PrintLine(FileInsert, TAB(5), "SELECT TOP 100")

        For i = 0 To Me.CheckedListBox2.CheckedItems.Count - 1
            strTemp = Me.CheckedListBox2.CheckedItems.Item(i).ToString
            strTemp2 = strTemp.Substring(strTemp.LastIndexOf(".") + 1)
            blnExiste = Me.EsString(odt, strTemp2)

            If i = Me.CheckedListBox2.CheckedItems.Count - 1 Then
                If blnExiste Then
                    PrintLine(FileInsert, TAB(8), "RTRIM(" & strTemp.Trim & ") AS " & "[" & FormatoNombreLabels(strTemp2) & "]")
                Else
                    PrintLine(FileInsert, TAB(8), strTemp.Trim & " AS " & "[" & FormatoNombreLabels(strTemp2) & "]")
                End If
            Else
                If blnExiste Then
                    PrintLine(FileInsert, TAB(8), "RTRIM(" & strTemp.Trim & ") AS " & "[" & FormatoNombreLabels(strTemp2) & "]" & ",")
                Else
                    PrintLine(FileInsert, TAB(8), strTemp.Trim & " AS " & "[" & FormatoNombreLabels(strTemp2) & "]" & ",")
                End If
            End If
        Next

        PrintLine(FileInsert, TAB(5), "FROM")
        PrintLine(FileInsert, TAB(8), InnerJoin)
        PrintLine(FileInsert, TAB(5), "WHERE")

        If Me.chkServidor.Checked Then
            PrintLine(FileInsert, TAB(9), "dbo." & Me.cmbTabla.Text.Trim & ".operacion <> 'D' AND")
        End If

        PrintLine(FileInsert, TAB(9), Me.CheckedListBox2.SelectedItem & " LIKE @nombre+'%'")

        PrintLine(FileInsert, TAB(0), "ORDER BY")
        PrintLine(FileInsert, TAB(4), Me.CheckedListBox2.SelectedItem)

        PrintLine(FileInsert, TAB(0), "END")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, "")


        FileClose(FileInsert)

        Dim PathArchivo As String = pathAnterior & "cop_" & Me.cmbTabla.Text & "_GetAll_2.sql"
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
        File.Close()
        File2.Close()


        Dim oImprimirClase As New ImprimirClase
        oImprimirClase.GetAllFind2(Me.txtPath.Text, Me.cmbTabla.Text)
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MessageBox.Show(Me.CheckedListBox2.SelectedItem)
    End Sub


    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        ' up
        Dim item As Object = Me.CheckedListBox2.SelectedItem
        If Not item Is Nothing Then
            Dim index As Integer = Me.CheckedListBox2.Items.IndexOf(item)
            If index <> 0 Then
                Me.CheckedListBox2.Items.RemoveAt(index)
                index -= 1
                Me.CheckedListBox2.Items.Insert(index, item)
                Me.CheckedListBox2.SelectedIndex = index
            End If
        End If
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        'down
        Dim item As Object = Me.CheckedListBox2.SelectedItem
        If Not item Is Nothing Then
            Dim index As Integer = Me.CheckedListBox2.Items.IndexOf(item)
            If index < Me.CheckedListBox2.Items.Count - 1 Then
                Me.CheckedListBox2.Items.RemoveAt(index)
                index += 1
                Me.CheckedListBox2.Items.Insert(index, item)
                Me.CheckedListBox2.SelectedIndex = index
            End If
        End If
    End Sub



    Private Sub CambioNombrePK(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTabla.KeyUp
        Dim tabla As String

        tabla = txtTabla.Text.Trim.ToLower

        If txtTabla.Text <> "" Then
            Me.txtPK.Text = "id_" & tabla
            Me.txtCampo.Text = "nombre_" & tabla
        Else
            Me.txtPK.Text = ""
            Me.txtCampo.Text = ""
        End If
    End Sub



    Private Sub txtTabla_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTabla.TextChanged

    End Sub

    Private Sub txtCampo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCampo.TextChanged

    End Sub


    Private Sub btnGetOne_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetOne.Click
        'variables para traer los varchar
        Dim ocn As New SqlConnection(Me.txtCadena.Text)
        Dim oda As New SqlDataAdapter("SELECT  DISTINCT name FROM  syscolumns WHERE typestat=2 and xtype=167", ocn)

        Dim ocb As New SqlCommandBuilder(oda)
        Dim ocbSistema As New SqlCommandBuilder(oda)
        Dim odt As New DataTable
        oda.Fill(odt)



        Dim PathInsert As String
        Dim i As Integer = 0

        PathInsert = pathAnterior & "cop_" & Me.cmbTabla.Text & "_GetCmb_2.sql"

        ' Defino variables
        Dim FileInsert As Integer = FreeFile()
        ' Dim regInsert As regSistemaTabla

        ' Abro un PathCls de texto (si el mismo existía se reempleza)
        FileOpen(FileInsert, PathInsert, OpenMode.Output)
        Dim Prefijo As String = "cop_"


        '*********************************************
        '
        'DEFINICION DEL CODIGO PARA LOS SP
        '
        '*********************************************

        Dim strTemp As String = ""
        Dim strTemp2 As String = ""
        Dim blnExiste As Boolean = False

        'codigo para modificar el GetCmb

        PrintLine(FileInsert, TAB(0), "USE [" & Me.txtBaseDato.Text.Trim & "]")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, "")

        'modificacion del GetCmb
        PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.cmbTabla.Text & "_GetCmb_2]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
        PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.cmbTabla.Text & "_GetCmb_2]")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, "")
        PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.cmbTabla.Text & "_GetCmb_2]")
        PrintLine(FileInsert, TAB(0), "AS")
        PrintLine(FileInsert, TAB(0), "SELECT")

        For i = 0 To Me.CheckedListBox2.CheckedItems.Count - 1
            strTemp = Me.CheckedListBox2.CheckedItems.Item(i).ToString
            strTemp2 = strTemp.Substring(strTemp.LastIndexOf(".") + 1)
            blnExiste = Me.EsString(odt, strTemp2)

            If i = Me.CheckedListBox2.CheckedItems.Count - 1 Then
                If blnExiste Then
                    PrintLine(FileInsert, TAB(4), "RTRIM(" & strTemp.Trim & ") AS " & strTemp2.ToUpper)
                Else
                    PrintLine(FileInsert, TAB(4), strTemp.Trim & " AS " & strTemp2.ToUpper)
                End If
            Else
                If blnExiste Then
                    PrintLine(FileInsert, TAB(4), "RTRIM(" & strTemp.Trim & ") AS " & strTemp2.ToUpper & ",")
                Else
                    PrintLine(FileInsert, TAB(4), strTemp.Trim & " AS " & strTemp2.ToUpper & ",")
                End If
            End If
        Next

        PrintLine(FileInsert, TAB(0), "FROM")
        PrintLine(FileInsert, TAB(4), InnerJoin)

        If Me.chkServidor.Checked Then
            PrintLine(FileInsert, TAB(0), "WHERE")
            PrintLine(FileInsert, TAB(4), "dbo." & Me.cmbTabla.Text.Trim & ".operacion <> 'D'")
        End If

        PrintLine(FileInsert, TAB(0), "ORDER BY")
        PrintLine(FileInsert, TAB(4), Me.CheckedListBox2.SelectedItem)
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, "")


        ''modificacion del getprint
        'PrintLine(FileInsert, TAB(0), "ALTER PROCEDURE [dbo].[sp_" & Me.cmbTabla.Text.Trim & "_GetPrint]")
        'PrintLine(FileInsert, TAB(0), "AS")
        'PrintLine(FileInsert, "")
        'PrintLine(FileInsert, TAB(0), "SELECT")

        'For i = 0 To Me.CheckedListBox2.CheckedItems.Count - 1
        '    strTemp = Me.CheckedListBox2.CheckedItems.Item(i).ToString
        '    strTemp2 = strTemp.Substring(strTemp.LastIndexOf(".") + 1)
        '    blnExiste = Me.EsString(odt, strTemp2)

        '    If i = Me.CheckedListBox2.CheckedItems.Count - 1 Then
        '        If blnExiste Then
        '            PrintLine(FileInsert, TAB(4), "RTRIM(" & strTemp.Trim & ") AS " & strTemp2.ToUpper)
        '        Else
        '            PrintLine(FileInsert, TAB(4), strTemp.Trim & " AS " & strTemp2.ToUpper)
        '        End If
        '    Else
        '        If blnExiste Then
        '            PrintLine(FileInsert, TAB(4), "RTRIM(" & strTemp.Trim & ") AS " & strTemp2.ToUpper & ",")
        '        Else
        '            PrintLine(FileInsert, TAB(4), strTemp.Trim & " AS " & strTemp2.ToUpper & ",")
        '        End If
        '    End If
        'Next

        'PrintLine(FileInsert, TAB(0), "FROM")
        'PrintLine(FileInsert, TAB(4), InnerJoin)

        'PrintLine(FileInsert, TAB(0), "ORDER BY")
        'PrintLine(FileInsert, TAB(4), Me.CheckedListBox2.SelectedItem)

        'PrintLine(FileInsert, TAB(0), "GO")

        'PrintLine(FileInsert, "")


        'modificacion del find
        PrintLine(FileInsert, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & Me.cmbTabla.Text & "_GetOne_2]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
        PrintLine(FileInsert, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & Me.cmbTabla.Text & "_GetOne_2]")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, "")
        PrintLine(FileInsert, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & Me.cmbTabla.Text & "_GetOne_2]")
        PrintLine(FileInsert, TAB(5), "@nombre NVARCHAR (30)=NULL")
        PrintLine(FileInsert, TAB(0), "AS SET NOCOUNT ON")
        PrintLine(FileInsert, "")
        PrintLine(FileInsert, TAB(0), "IF @nombre IS NOT NULL")
        PrintLine(FileInsert, TAB(0), "BEGIN")
        PrintLine(FileInsert, TAB(5), "SELECT @nombre=RTRIM(@nombre)+'%'")
        PrintLine(FileInsert, TAB(5), "SELECT")

        For i = 0 To Me.CheckedListBox2.CheckedItems.Count - 1
            strTemp = Me.CheckedListBox2.CheckedItems.Item(i).ToString
            strTemp2 = strTemp.Substring(strTemp.LastIndexOf(".") + 1)
            blnExiste = Me.EsString(odt, strTemp2)

            If i = Me.CheckedListBox2.CheckedItems.Count - 1 Then
                If blnExiste Then
                    PrintLine(FileInsert, TAB(8), "RTRIM(" & strTemp.Trim & ") AS " & strTemp2.ToUpper)
                Else
                    PrintLine(FileInsert, TAB(8), strTemp.Trim & " AS " & strTemp2.ToUpper)
                End If
            Else
                If blnExiste Then
                    PrintLine(FileInsert, TAB(8), "RTRIM(" & strTemp.Trim & ") AS " & strTemp2.ToUpper & ",")
                Else
                    PrintLine(FileInsert, TAB(8), strTemp.Trim & " AS " & strTemp2.ToUpper & ",")
                End If
            End If
        Next

        PrintLine(FileInsert, TAB(5), "FROM")
        PrintLine(FileInsert, TAB(8), InnerJoin)
        PrintLine(FileInsert, TAB(5), "WHERE")

        If Me.chkServidor.Checked Then
            PrintLine(FileInsert, TAB(9), "dbo." & Me.cmbTabla.Text.Trim & ".operacion <> 'D' AND")
        End If

        PrintLine(FileInsert, TAB(9), Me.CheckedListBox2.SelectedItem & " LIKE @nombre+'%'")

        PrintLine(FileInsert, TAB(0), "ORDER BY")
        PrintLine(FileInsert, TAB(4), Me.CheckedListBox2.SelectedItem)

        PrintLine(FileInsert, TAB(0), "END")
        PrintLine(FileInsert, TAB(0), "GO")
        PrintLine(FileInsert, "")


        FileClose(FileInsert)

        Dim PathArchivo As String = pathAnterior & "cop_" & Me.cmbTabla.Text & "_GetCmb_2.sql"
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
        File.Close()
        File2.Close()
    End Sub


End Class