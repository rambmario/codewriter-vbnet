Public Class ImprimirSql
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

    Public Sub ImprimirSps(ByVal db As String, ByVal path As String, ByVal tabla As String, ByVal filas As Integer, _
    ByVal insert As Boolean, ByVal delete As Boolean, ByVal getall As Boolean, ByVal getone As Boolean, _
    ByVal getcmb As Boolean, ByVal update As Boolean, ByVal exist As Boolean, ByVal find As Boolean, _
    ByVal insertone As Boolean, ByVal updateid As Boolean, ByVal deleteencabezado As Boolean, _
    ByVal getallencabezadoone As Boolean)
        Dim nombre As String = "" & tabla

        Dim tablaVieja As String = ""

        If updateid = True Then
            Try
                tablaVieja = tabla.Substring(7)
            Catch ex As Exception
                tablaVieja = tabla
            End Try

            ' tabla = "Cuerpo_" & tabla

        End If

        nombre = path & "cop_" & tabla & ".sql"


        ' Defino variables
        Dim FileCls As Integer = FreeFile()
        Dim reg As regSistemaTabla

        ' Abro un PathCls de texto (si el mismo existía se reempleza)
        FileOpen(FileCls, nombre, OpenMode.Output)

        Dim Prefijo As String = "cop_"
        Dim Contador As Integer = 1

        'use base datos
        PrintLine(FileCls, TAB(0), "USE [" & db & "]")
        PrintLine(FileCls, TAB(0), "GO")
        PrintLine(FileCls, "")

        'sp insert
        If insert = True Then
            PrintLine(FileCls, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & tabla & "_Insert]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
            PrintLine(FileCls, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & tabla & "_Insert]")
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & tabla & "_Insert]")

            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    PrintLine(FileCls, TAB(5), "@" & reg.nombre & "    " & reg.tipo & "  output,")
                ElseIf Contador = filas Then
                    PrintLine(FileCls, TAB(5), "@" & reg.nombre & "    " & reg.tipo & "  " & reg.sptamanio)
                Else
                    PrintLine(FileCls, TAB(5), "@" & reg.nombre & "    " & reg.tipo & "  " & reg.sptamanio & ",")
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(0), "AS")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "INSERT INTO [dbo].[" & tabla & "]")
            PrintLine(FileCls, TAB(0), "(")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                ElseIf Contador = filas Then
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "]")
                Else
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "],")
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, ")")
            PrintLine(FileCls, TAB(0), "VALUES")
            PrintLine(FileCls, TAB(0), "(")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                ElseIf Contador = filas Then
                    PrintLine(FileCls, TAB(5), "@" & reg.nombre)
                Else
                    PrintLine(FileCls, TAB(5), "@" & reg.nombre & ",")
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, ")")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    PrintLine(FileCls, TAB(0), "SET @" & reg.nombre & " = @@IDENTITY")
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
        End If


        'sp delete
        If delete = True Then
            PrintLine(FileCls, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & tabla & "_Delete]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
            PrintLine(FileCls, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & tabla & "_Delete]")
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & tabla & "_Delete]")

            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    PrintLine(FileCls, TAB(5), "@" & reg.nombre & "    " & reg.tipo)
                    Exit For
                End If
            Next
            PrintLine(FileCls, TAB(0), "AS")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "DELETE FROM [dbo].[" & tabla & "]")
            PrintLine(FileCls, TAB(0), "WHERE")
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "]  =  @" & reg.nombre)
                    Exit For
                End If
            Next
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
        End If


        'sp GetAll
        If getall = True Then
            PrintLine(FileCls, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & tabla & "_GetAll]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
            PrintLine(FileCls, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & tabla & "_GetAll]")
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & tabla & "_GetAll]")
            PrintLine(FileCls, TAB(0), "AS")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "SELECT TOP 100")

            Contador = 1
            For Each reg In arrEstructura
                If Contador = filas Then
                    If reg.tipo = "varchar" Then
                        PrintLine(FileCls, TAB(5), "RTRIM(" & reg.nombre & ") AS " & "[" & FormatoNombreLabels(reg.nombre) & "]")
                    Else
                        PrintLine(FileCls, TAB(5), "[" & reg.nombre & "] AS " & "[" & FormatoNombreLabels(reg.nombre) & "]")
                    End If
                Else
                    If reg.tipo = "varchar" Then
                        PrintLine(FileCls, TAB(5), "RTRIM(" & reg.nombre & ") AS " & "[" & FormatoNombreLabels(reg.nombre) & "]" & " ,")
                    Else
                        PrintLine(FileCls, TAB(5), "[" & reg.nombre & "] AS " & "[" & FormatoNombreLabels(reg.nombre) & "]" & " ,")
                    End If
                End If
                Contador = Contador + 1
            Next

            PrintLine(FileCls, TAB(0), "FROM")
            PrintLine(FileCls, TAB(5), "[dbo].[" & tabla & "]")
            PrintLine(FileCls, TAB(0), "ORDER BY")

            Contador = 0
            For Each reg In arrEstructura
                If Contador = 1 Then
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "]")
                    Exit For
                End If
                Contador = Contador + 1
            Next

            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
        End If


        'sp GetCmb
        If getcmb = True Then
            PrintLine(FileCls, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & tabla & "_GetCmb]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
            PrintLine(FileCls, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & tabla & "_GetCmb]")
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & tabla & "_GetCmb]")
            PrintLine(FileCls, TAB(0), "AS")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "SELECT")

            Contador = 1
            For Each reg In arrEstructura
                If Contador = filas Then
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "]")
                Else
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "],")
                End If
                Contador = Contador + 1
            Next

            PrintLine(FileCls, TAB(0), "FROM")
            PrintLine(FileCls, TAB(5), "[dbo].[" & tabla & "]")
            PrintLine(FileCls, TAB(0), "ORDER BY")

            Contador = 0
            For Each reg In arrEstructura
                If Contador = 1 Then
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "]")
                    Exit For
                End If
                Contador = Contador + 1
            Next

            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
        End If


        'sp update
        If update = True Then
            PrintLine(FileCls, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & tabla & "_Update]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
            PrintLine(FileCls, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & tabla & "_Update]")
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & tabla & "_Update]")

            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    PrintLine(FileCls, TAB(5), "@" & reg.nombre & "    " & reg.tipo & "  output,")
                ElseIf Contador = filas Then
                    PrintLine(FileCls, TAB(5), "@" & reg.nombre & "    " & reg.tipo & "  " & reg.sptamanio)
                Else
                    PrintLine(FileCls, TAB(5), "@" & reg.nombre & "    " & reg.tipo & "  " & reg.sptamanio & ",")
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(0), "AS")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "UPDATE [dbo].[" & tabla & "] SET")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                ElseIf Contador = filas Then
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "] = @" & reg.nombre)
                Else
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "] = @" & reg.nombre & ",")
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(0), "WHERE")
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "]  =  @" & reg.nombre)
                    Exit For
                End If
            Next
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
        End If


        'sp exist
        If exist = True Then
            PrintLine(FileCls, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & tabla & "_Exist]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
            PrintLine(FileCls, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & tabla & "_Exist]")
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & tabla & "_Exist]")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'PrintLine(FileCls, TAB(5), "@" & reg.nombre & "    " & reg.tipo & "  output,")
                ElseIf Contador = filas Then
                    PrintLine(FileCls, TAB(5), "@" & reg.nombre & "    " & reg.tipo & "  " & reg.sptamanio)
                Else
                    PrintLine(FileCls, TAB(5), "@" & reg.nombre & "    " & reg.tipo & "  " & reg.sptamanio & ",")
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(0), "AS")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "DECLARE @total int")
            PrintLine(FileCls, TAB(0), "SET @total = 0")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "SELECT")
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    PrintLine(FileCls, TAB(5), "@total = " & reg.nombre)
                    Exit For
                End If
            Next
            PrintLine(FileCls, TAB(0), "FROM")
            PrintLine(FileCls, TAB(5), "[dbo].[" & tabla & "]")
            PrintLine(FileCls, TAB(0), "WHERE")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                ElseIf Contador = filas Then
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "] = @" & reg.nombre)
                Else
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "] = @" & reg.nombre & " AND")
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "IF @total IS NULL")
            PrintLine(FileCls, TAB(0), "BEGIN")
            PrintLine(FileCls, TAB(5), "SET @total=0")
            PrintLine(FileCls, TAB(0), "END")
            PrintLine(FileCls, TAB(0), "SELECT @total AS Total")
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
        End If


        'sp getone
        If getone = True Then
            PrintLine(FileCls, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & tabla & "_GetOne]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
            PrintLine(FileCls, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & tabla & "_GetOne]")
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & tabla & "_GetOne]")
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    PrintLine(FileCls, TAB(5), "@" & reg.nombre & "    " & reg.tipo)
                    Exit For
                End If
            Next
            PrintLine(FileCls, TAB(0), "AS")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "SELECT")

            Contador = 1
            For Each reg In arrEstructura
                If Contador = filas Then
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "] AS " & "[" & FormatoNombreLabels(reg.nombre) & "]")
                Else
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "] AS " & "[" & FormatoNombreLabels(reg.nombre) & "]" & " ,")
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(0), "FROM")
            PrintLine(FileCls, TAB(5), "[dbo].[" & tabla & "]")
            PrintLine(FileCls, TAB(0), "WHERE")
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "]  =  @" & reg.nombre)
                    Exit For
                End If
            Next
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
        End If


        'sp find
        If find = True Then
            PrintLine(FileCls, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & tabla & "_Find]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
            PrintLine(FileCls, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & tabla & "_Find]")
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & tabla & "_Find]")
            PrintLine(FileCls, TAB(5), "@nombre NVARCHAR (30)=NULL")
            PrintLine(FileCls, TAB(0), "AS SET NOCOUNT ON")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "IF @nombre IS NOT NULL")
            PrintLine(FileCls, TAB(0), "BEGIN")
            PrintLine(FileCls, TAB(0), "SELECT @nombre=RTRIM(@nombre)+'%'")
            PrintLine(FileCls, TAB(0), "SELECT TOP 100")

            Contador = 1
            For Each reg In arrEstructura
                If Contador = filas Then
                    If reg.tipo = "varchar" Then
                        PrintLine(FileCls, TAB(5), "RTRIM(" & reg.nombre & ") AS " & "[" & FormatoNombreLabels(reg.nombre) & "]")
                    Else
                        PrintLine(FileCls, TAB(5), "[" & reg.nombre & "] AS " & "[" & FormatoNombreLabels(reg.nombre) & "]")
                    End If
                Else
                    If reg.tipo = "varchar" Then
                        PrintLine(FileCls, TAB(5), "RTRIM(" & reg.nombre & ") AS " & "[" & FormatoNombreLabels(reg.nombre) & "]" & " ,")
                    Else
                        PrintLine(FileCls, TAB(5), "[" & reg.nombre & "] AS " & "[" & FormatoNombreLabels(reg.nombre) & "]" & " ,")
                    End If
                End If
                Contador = Contador + 1
            Next

            PrintLine(FileCls, TAB(0), "FROM")
            PrintLine(FileCls, TAB(5), "[dbo].[" & tabla & "]")
            PrintLine(FileCls, TAB(0), "WHERE")
            Contador = 0
            For Each reg In arrEstructura
                If Contador = 1 Then
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "] LIKE @nombre+'%'")
                    Exit For
                End If
                Contador = Contador + 1
            Next

            PrintLine(FileCls, TAB(0), "ORDER BY")
            Contador = 0
            For Each reg In arrEstructura
                If Contador = 1 Then
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "]")
                    Exit For
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(0), "END")
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
        End If


        'sp InsertOne
        If insertone = True Then
            PrintLine(FileCls, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & tabla & "_InsertOne]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
            PrintLine(FileCls, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & tabla & "_InsertOne]")
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & tabla & "_InsertOne]")
            PrintLine(FileCls, TAB(0), "AS")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "INSERT INTO [dbo].[" & tabla & "]")
            PrintLine(FileCls, TAB(0), "(")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                ElseIf Contador = filas Then
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "]")
                Else
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "],")
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, ")")
            PrintLine(FileCls, TAB(0), "VALUES")
            PrintLine(FileCls, TAB(0), "(")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                ElseIf Contador = filas Then
                    PrintLine(FileCls, TAB(5), reg.valorinsert)
                Else
                    PrintLine(FileCls, TAB(5), reg.valorinsert & ",")
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, ")")

            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
        End If


        ''''''''''''''''''''''''
        'agregado del encabezado
        ''''''''''''''''''''''''

        'sp deleteEncabezado
        If deleteencabezado = True Then
            PrintLine(FileCls, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & "Cuerpo_" & tabla & "_Delete" & tabla & "]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
            PrintLine(FileCls, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & "Cuerpo_" & tabla & "_Delete" & tabla & "]")
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & "Cuerpo_" & tabla & "_Delete" & tabla & "]")

            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    PrintLine(FileCls, TAB(5), "@id_" & tabla & "    " & reg.tipo & ",")
                    Exit For
                End If
            Next
            PrintLine(FileCls, TAB(5), "@id_usuario int")
            PrintLine(FileCls, TAB(0), "AS")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "DELETE FROM [dbo].[" & "Cuerpo_" & tabla & "]")
            PrintLine(FileCls, TAB(0), "WHERE")
            PrintLine(FileCls, TAB(5), "[id_" & tabla & "] = @id_" & tabla & " AND")
            PrintLine(FileCls, TAB(5), "[id_usuario] = @id_usuario")
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
        End If

        If updateid = True Then
            PrintLine(FileCls, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & "Cuerpo_" & tabla & "_UpdateID]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
            PrintLine(FileCls, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & "Cuerpo_" & tabla & "_UpdateID]")
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & "Cuerpo_" & tabla & "_UpdateID]")
            PrintLine(FileCls, TAB(5), "@id_" & tabla & " int,")
            PrintLine(FileCls, TAB(5), "@id_usuario int")
            Contador = 1
            'For Each reg In arrEstructura
            '    If reg.Orden = 1 Then
            '        PrintLine(FileCls, TAB(5), "@" & reg.nombre & "    " & reg.tipo & "  output,")
            '    ElseIf Contador = filas Then
            '        PrintLine(FileCls, TAB(5), "@" & reg.nombre & "    " & reg.tipo & "  " & reg.sptamaño)
            '    Else
            '        PrintLine(FileCls, TAB(5), "@" & reg.nombre & "    " & reg.tipo & "  " & reg.sptamaño & ",")
            '    End If
            '    Contador = Contador + 1
            'Next
            PrintLine(FileCls, TAB(0), "AS")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "UPDATE [dbo].[" & "Cuerpo_" & tabla & "] SET")
            PrintLine(FileCls, TAB(5), "[id_" & tabla & "] = @id_" & tabla)
            'Contador = 1
            'For Each reg In arrEstructura
            '    If reg.Orden = 1 Then
            '    ElseIf Contador = filas Then
            '        PrintLine(FileCls, TAB(5), "[" & reg.nombre & "] = @" & reg.nombre)
            '    Else
            '        PrintLine(FileCls, TAB(5), "[" & reg.nombre & "] = @" & reg.nombre & ",")
            '    End If
            '    Contador = Contador + 1
            'Next
            PrintLine(FileCls, TAB(0), "WHERE")
            PrintLine(FileCls, TAB(5), "[id_" & tabla & "] = 0 AND")
            PrintLine(FileCls, TAB(5), "[id_usuario] = @id_usuario")
            'For Each reg In arrEstructura
            '    If reg.Orden = 1 Then
            '        PrintLine(FileCls, TAB(5), "[" & reg.nombre & "]  =  @" & reg.nombre)
            '        Exit For
            '    End If
            'Next
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
        End If


        If getallencabezadoone = True Then

            Try
                tablaVieja = tabla.ToString.Substring(7)
            Catch ex As Exception
            End Try


            'sp GetAllMovimiento
            PrintLine(FileCls, TAB(0), "IF exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & Prefijo & tabla & "_GetAllMovimiento]') and OBJECTPROPERTY(ID, N'IsProcedure') = 1)")
            PrintLine(FileCls, TAB(0), "DROP PROCEDURE [dbo].[" & Prefijo & tabla & "_GetAllMovimiento]")
            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "CREATE PROCEDURE [dbo].[" & Prefijo & tabla & "_GetAllMovimiento]")
            PrintLine(FileCls, TAB(5), "@id_" & tablaVieja & " int")
            PrintLine(FileCls, TAB(0), "AS")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "SELECT TOP 100")

            Contador = 1
            For Each reg In arrEstructura
                If Contador = filas Then
                    If reg.tipo = "varchar" Then
                        PrintLine(FileCls, TAB(5), "RTRIM(" & reg.nombre & ") AS " & "[" & FormatoNombreLabels(reg.nombre) & "]")
                    Else
                        PrintLine(FileCls, TAB(5), "[" & reg.nombre & "] AS " & "[" & FormatoNombreLabels(reg.nombre) & "]")
                    End If
                Else
                    If reg.tipo = "varchar" Then
                        PrintLine(FileCls, TAB(5), "RTRIM(" & reg.nombre & ") AS " & "[" & FormatoNombreLabels(reg.nombre) & "]" & " ,")
                    Else
                        PrintLine(FileCls, TAB(5), "[" & reg.nombre & "] AS " & "[" & FormatoNombreLabels(reg.nombre) & "]" & " ,")
                    End If
                End If
                Contador = Contador + 1
            Next

            PrintLine(FileCls, TAB(0), "FROM")
            PrintLine(FileCls, TAB(5), "[dbo].[" & tabla & "]")
            PrintLine(FileCls, TAB(0), "WHERE")
            PrintLine(FileCls, TAB(5), "id_" & tablaVieja & " = @id_" & tablaVieja)
            PrintLine(FileCls, TAB(0), "ORDER BY")

            Contador = 0
            For Each reg In arrEstructura
                If Contador = 1 Then
                    PrintLine(FileCls, TAB(5), "[" & reg.nombre & "]")
                    Exit For
                End If
                Contador = Contador + 1
            Next

            PrintLine(FileCls, TAB(0), "GO")
            PrintLine(FileCls, "")
        End If




        FileClose(FileCls)
    End Sub







End Class
