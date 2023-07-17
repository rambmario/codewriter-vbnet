Public Class ImprimirTest

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

    Public Sub ImprimirTest(ByVal db As String, ByVal path As String, ByVal tabla As String, _
    ByVal insert As Boolean, ByVal delete As Boolean, ByVal getall As Boolean, ByVal getone As Boolean, _
    ByVal getcmb As Boolean, ByVal update As Boolean, ByVal exist As Boolean, ByVal find As Boolean)
        Dim nombre As String = "" & tabla

        nombre = path & tabla & "Test.vb"

            ' Defino variables
            Dim FileCls As Integer = FreeFile()
            Dim reg As regSistemaTabla


            ' Abro un PathCls de texto (si el mismo existía se reempleza)
            FileOpen(FileCls, nombre, OpenMode.Output)

            PrintLine(FileCls, TAB(0), "Imports System.Data")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "Imports Microsoft.VisualStudio.TestTools.UnitTesting")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "Imports " & db)
            PrintLine(FileCls, "")
            PrintLine(FileCls, "")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "'''<summary>")
            PrintLine(FileCls, TAB(0), "'''Se trata de una clase de prueba para " & tabla & "_entTest y se pretende que")
            PrintLine(FileCls, TAB(0), "'''contenga todas las pruebas unitarias " & tabla & "_entTest. ")
            PrintLine(FileCls, TAB(0), "'''</summary>")
            PrintLine(FileCls, TAB(0), "<TestClass()> _")
        PrintLine(FileCls, TAB(0), "Public Class " & tabla & "Test")
            PrintLine(FileCls, "")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "Private testContextInstance As TestContext")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "'''<summary>")
            PrintLine(FileCls, TAB(4), "'''Obtiene o establece el contexto de la prueba que proporciona")
            PrintLine(FileCls, TAB(4), "'''la información y funcionalidad para la ejecución de pruebas actual. ")
            PrintLine(FileCls, TAB(4), "'''</summary>")
            PrintLine(FileCls, TAB(4), "Public Property TestContext() As TestContext")
            PrintLine(FileCls, TAB(8), "Get")
            PrintLine(FileCls, TAB(12), "Return testContextInstance")
            PrintLine(FileCls, TAB(8), "End Get")
            PrintLine(FileCls, TAB(8), "Set(ByVal value As TestContext) ")
            PrintLine(FileCls, TAB(12), "testContextInstance = value")
            PrintLine(FileCls, TAB(8), "End Set")
            PrintLine(FileCls, TAB(4), "End Property")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "#Region " & """" & "Atributos de prueba adicionales" & """")
            PrintLine(FileCls, TAB(4), "'")
            PrintLine(FileCls, TAB(4), "'Puede utilizar los siguientes atributos adicionales mientras escribe sus pruebas: ")
            PrintLine(FileCls, TAB(4), "'")
            PrintLine(FileCls, TAB(4), "'Use ClassInitialize para ejecutar código antes de ejecutar la primera prueba en la clase ")
            PrintLine(FileCls, TAB(4), "'<ClassInitialize()>  _")
            PrintLine(FileCls, TAB(4), "'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext) ")
            PrintLine(FileCls, TAB(4), "'End Sub")
            PrintLine(FileCls, TAB(4), "'")
            PrintLine(FileCls, TAB(4), "'Use ClassCleanup para ejecutar código después de haber ejecutado todas las pruebas en una clase")
            PrintLine(FileCls, TAB(4), "'<ClassCleanup()>  _")
            PrintLine(FileCls, TAB(4), "'Public Shared Sub MyClassCleanup()")
            PrintLine(FileCls, TAB(4), "'End Sub")
            PrintLine(FileCls, TAB(4), "'")
            PrintLine(FileCls, TAB(4), "'Use TestInitialize para ejecutar código antes de ejecutar cada prueba")
            PrintLine(FileCls, TAB(4), "'<TestInitialize()>  _")
            PrintLine(FileCls, TAB(4), "'Public Sub MyTestInitialize()")
            PrintLine(FileCls, TAB(4), "'End Sub")
            PrintLine(FileCls, TAB(4), "'")
            PrintLine(FileCls, TAB(4), "'Use TestCleanup para ejecutar código después de que se hayan ejecutado todas las pruebas")
            PrintLine(FileCls, TAB(4), "'<TestCleanup()>  _")
            PrintLine(FileCls, TAB(4), "'Public Sub MyTestCleanup()")
            PrintLine(FileCls, TAB(4), "'End Sub")
            PrintLine(FileCls, TAB(4), "'")
            PrintLine(FileCls, TAB(0), "#End Region")
            PrintLine(FileCls, "")
            PrintLine(FileCls, "")

            'modificar
            If update = True Then
                PrintLine(FileCls, TAB(4), "'''<summary>")
                PrintLine(FileCls, TAB(4), " '''Una prueba de Modificar")
                PrintLine(FileCls, TAB(4), " '''</summary>")
                PrintLine(FileCls, TAB(4), "<TestMethod()> _")
            PrintLine(FileCls, TAB(4), "Public Sub Modificar" & tabla & "Test()")
                PrintLine(FileCls, TAB(8), "Dim target As " & tabla & "_ent = New " & tabla & "_ent ' TODO: Inicializar en un valor adecuado")
                PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
            'PrintLine(FileCls, TAB(8), "Dim actual As DataTable")
                PrintLine(FileCls, TAB(8), "target.Truncate()")
                PrintLine(FileCls, TAB(8), "target.InsertOne()")
                PrintLine(FileCls, TAB(8), "target.Cargar()")
            PrintLine(FileCls, TAB(8), "target.Modificar(1) ")
            PrintLine(FileCls, TAB(8), "target.nombre_" & tabla & " = " & """" & "modificado" & """")
            PrintLine(FileCls, TAB(8), "target.Guardar()")
                PrintLine(FileCls, "")
            'PrintLine(FileCls, TAB(8), "actual = target.Modificar")
                PrintLine(FileCls, TAB(8), "Dim dt As DataTable = New DataTable")
                PrintLine(FileCls, TAB(8), "dt = target.ConsultarTodo")
            PrintLine(FileCls, TAB(8), "Assert.IsTrue(CBool(dt.Rows(0).Item(" & """" & "nombre_" & tabla & """" & ").ToString = " & """" & "modificado" & """" & ")," & """" & "no: " & """" & " & dt.Rows(0).Item(" & """" & "nombre_" & tabla & """" & ").ToString)")
                PrintLine(FileCls, TAB(4), "End Sub")
                PrintLine(FileCls, "")
            End If

            'insertar
            If insert = True Then
                PrintLine(FileCls, TAB(4), "'''<summary>")
                PrintLine(FileCls, TAB(4), "'''Una prueba de Insertar")
                PrintLine(FileCls, TAB(4), "'''</summary>")
                PrintLine(FileCls, TAB(4), "<TestMethod()> _")
            PrintLine(FileCls, TAB(4), "Public Sub Insertar" & tabla & "Test()")
                PrintLine(FileCls, TAB(8), "Dim target As " & tabla & "_ent = New " & tabla & "_ent ' TODO: Inicializar en un valor adecuado")
                PrintLine(FileCls, "")
                PrintLine(FileCls, TAB(8), "target.Truncate()")
                PrintLine(FileCls, TAB(8), "target.InsertOne()")
                PrintLine(FileCls, TAB(8), "target.Cargar()")
                PrintLine(FileCls, TAB(8), "target.Insertar()")
            'PrintLine(FileCls, TAB(8), "target.id_" & tabla & " = 2")
                PrintLine(FileCls, TAB(8), "target.nombre_" & tabla & " = " & """" & "prueba" & """")
                PrintLine(FileCls, TAB(8), "target.Guardar()")
                PrintLine(FileCls, "")
                PrintLine(FileCls, TAB(8), "Dim dt As DataTable = New DataTable")
                PrintLine(FileCls, TAB(8), "dt = target.ConsultarTodo")
                PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "Assert.IsTrue(dt.Rows.Count = 2, " & """" & "son: " & """" & " & dt.Rows.Count & " & """" & " registros" & """" & ")")
                PrintLine(FileCls, TAB(4), "End Sub")
                PrintLine(FileCls, "")
            End If

            'getone
            If getone = True Then
                PrintLine(FileCls, TAB(4), "'''<summary>")
                PrintLine(FileCls, TAB(4), "'''Una prueba de GetOne")
                PrintLine(FileCls, TAB(4), "'''</summary>")
                PrintLine(FileCls, TAB(4), "<TestMethod()> _")
            PrintLine(FileCls, TAB(4), "Public Sub GetOne" & tabla & "Test()")
                PrintLine(FileCls, TAB(8), "Dim target As " & tabla & "_ent = New " & tabla & "_ent ' TODO: Inicializar en un valor adecuado")
                PrintLine(FileCls, TAB(8), "Dim id_" & tabla & " As Integer = 1 ' TODO: Inicializar en un valor adecuado")
                PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
                PrintLine(FileCls, TAB(8), "Dim actual As DataTable")
                PrintLine(FileCls, "")
                PrintLine(FileCls, TAB(8), "target.Truncate()")
                PrintLine(FileCls, TAB(8), "target.InsertOne()")
                PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "actual = target.GetOne(id_" & tabla & ")")
                PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "Assert.IsTrue(actual.Rows.Count = 1, " & """" & "son: " & """" & " & actual.Rows.Count & " & """" & " registros" & """" & ")")
                PrintLine(FileCls, TAB(4), "End Sub")
                PrintLine(FileCls, "")
            End If

            'getcmb
            If getcmb = True Then
                PrintLine(FileCls, TAB(4), "'''<summary>")
                PrintLine(FileCls, TAB(4), "'''Una prueba de GetCmb")
                PrintLine(FileCls, TAB(4), "'''</summary>")
                PrintLine(FileCls, TAB(4), "<TestMethod()> _")
            PrintLine(FileCls, TAB(4), "Public Sub GetCmb" & tabla & "Test()")
            PrintLine(FileCls, TAB(8), "Dim target As " & tabla & "_ent = New " & tabla & "_ent ' TODO: Inicializar en un valor adecuado")
                PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
                PrintLine(FileCls, TAB(8), "Dim actual As DataTable")
                PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "target.Truncate()")
            PrintLine(FileCls, TAB(8), "target.InsertOne()")
                PrintLine(FileCls, "")
                PrintLine(FileCls, TAB(8), "actual = target.GetCmb")
                PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "Assert.IsTrue(actual.Rows.Count = 1, " & """" & "son: " & """" & " & actual.Rows.Count & " & """" & " registros" & """" & ")")
                PrintLine(FileCls, TAB(4), "End Sub")
                PrintLine(FileCls, "")
            End If

            'exist
            If exist = True Then
                PrintLine(FileCls, TAB(4), "'''<summary>")
                PrintLine(FileCls, TAB(4), "'''Una prueba de Exist")
                PrintLine(FileCls, TAB(4), "'''</summary>")
                PrintLine(FileCls, TAB(4), "<TestMethod()> _")
            PrintLine(FileCls, TAB(4), "Public Sub Exist" & tabla & "Test()")
                PrintLine(FileCls, TAB(8), "Dim target As " & tabla & "_ent = New " & tabla & "_ent ' TODO: Inicializar en un valor adecuado")
                PrintLine(FileCls, TAB(8), "Dim expected As Boolean = False ' TODO: Inicializar en un valor adecuado")
                PrintLine(FileCls, TAB(8), "Dim actual As Boolean")
                PrintLine(FileCls, "")
                PrintLine(FileCls, TAB(8), "target.Truncate()")
                PrintLine(FileCls, TAB(8), "target.InsertOne()")
                PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "target.Modificar(1) ")
                PrintLine(FileCls, TAB(8), "target.nombre_" & tabla & " = " & """" & "prueba" & """")
                PrintLine(FileCls, TAB(8), "target.Guardar()")
                PrintLine(FileCls, "")
                PrintLine(FileCls, TAB(8), "actual = target.Exist")
                PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "Assert.IsTrue(actual = True, " & """" & "no: " & """" & " & actual) ")
                PrintLine(FileCls, TAB(4), "End Sub")
                PrintLine(FileCls, "")
            End If

            'consultar todo
            If getall = True Then
                PrintLine(FileCls, TAB(4), "'''<summary>")
                PrintLine(FileCls, TAB(4), "'''Una prueba de ConsultarTodo")
                PrintLine(FileCls, TAB(4), "'''</summary>")
                PrintLine(FileCls, TAB(4), "<TestMethod()> _")
            PrintLine(FileCls, TAB(4), "Public Sub ConsultarTodo" & tabla & "Test()")
                PrintLine(FileCls, TAB(8), "Dim target As " & tabla & "_ent = New " & tabla & "_ent ' TODO: Inicializar en un valor adecuado")
                PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
                PrintLine(FileCls, TAB(8), "Dim actual As DataTable")
                PrintLine(FileCls, "")
                PrintLine(FileCls, TAB(8), "target.Truncate()")
                PrintLine(FileCls, TAB(8), "target.InsertOne()")
                PrintLine(FileCls, TAB(8), "target.Cargar()")
                PrintLine(FileCls, TAB(8), "target.Insertar()")
            '  PrintLine(FileCls, TAB(8), "target.id_" & tabla & " = 2")
                PrintLine(FileCls, TAB(8), "target.nombre_" & tabla & " = " & """" & "prueba" & """")
                PrintLine(FileCls, TAB(8), "target.Guardar()")
                PrintLine(FileCls, "")
                PrintLine(FileCls, TAB(8), "actual = target.ConsultarTodo")
                PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "Assert.IsTrue(actual.Rows.Count = 2, " & """" & "son: " & """" & " & actual.Rows.Count & " & """" & " registros" & """" & ")")
                PrintLine(FileCls, TAB(4), "End Sub")
                PrintLine(FileCls, "")
            End If

            'buscar
            If find = True Then
                PrintLine(FileCls, TAB(4), "'''<summary>")
                PrintLine(FileCls, TAB(4), "'''Una prueba de Buscar")
                PrintLine(FileCls, TAB(4), "'''</summary>")
                PrintLine(FileCls, TAB(4), "<TestMethod()> _")
            PrintLine(FileCls, TAB(4), "Public Sub Buscar" & tabla & "Test()")
                PrintLine(FileCls, TAB(8), "Dim target As " & tabla & "_ent = New " & tabla & "_ent ' TODO: Inicializar en un valor adecuado")
                PrintLine(FileCls, TAB(8), "Dim Nombre As String = " & """" & "pru" & """" & " ' TODO: Inicializar en un valor adecuado")
                PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
                PrintLine(FileCls, TAB(8), "Dim actual As DataTable")
                PrintLine(FileCls, "")
                PrintLine(FileCls, TAB(8), "target.Truncate()")
                PrintLine(FileCls, TAB(8), "target.InsertOne()")
                PrintLine(FileCls, TAB(8), "target.Cargar()")
                PrintLine(FileCls, TAB(8), "target.Insertar()")
            ' PrintLine(FileCls, TAB(8), "target.id_" & tabla & " = 3")
                PrintLine(FileCls, TAB(8), "target.nombre_" & tabla & " = " & """" & "prueba" & """")
                PrintLine(FileCls, TAB(8), "target.Guardar()")
                PrintLine(FileCls, "")
                PrintLine(FileCls, TAB(8), "target.Insertar()")
            ' PrintLine(FileCls, TAB(8), "target.id_" & tabla & " = 4")
                PrintLine(FileCls, TAB(8), "target.nombre_" & tabla & " = " & """" & "prudente" & """")
                PrintLine(FileCls, TAB(8), "target.Guardar()")
                PrintLine(FileCls, "")
                PrintLine(FileCls, TAB(8), "actual = target.Buscar(Nombre) ")
                PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "Assert.IsTrue(actual.Rows.Count = 2, " & """" & "son: " & """" & " & actual.Rows.Count & " & """" & " registros" & """" & ")")
                PrintLine(FileCls, TAB(4), "End Sub")
                PrintLine(FileCls, "")
            End If

            'borrar
            If delete = True Then
                PrintLine(FileCls, TAB(4), "'''<summary>")
                PrintLine(FileCls, TAB(4), "'''Una prueba de Borrar")
                PrintLine(FileCls, TAB(4), "'''</summary>")
                PrintLine(FileCls, TAB(4), "<TestMethod()> _")
            PrintLine(FileCls, TAB(4), "Public Sub Borrar" & tabla & "Test()")
                PrintLine(FileCls, TAB(8), "Dim target As " & tabla & "_ent = New " & tabla & "_ent ' TODO: Inicializar en un valor adecuado")
                PrintLine(FileCls, TAB(8), "Dim id_" & tabla & " As Integer = 1 ' TODO: Inicializar en un valor adecuado")
                PrintLine(FileCls, TAB(8), "Dim expected As DataTable = Nothing ' TODO: Inicializar en un valor adecuado")
            PrintLine(FileCls, TAB(8), "'Dim actual As DataTable")
                PrintLine(FileCls, "")
                PrintLine(FileCls, TAB(8), "target.Truncate()")
                PrintLine(FileCls, TAB(8), "target.InsertOne()")
            PrintLine(FileCls, TAB(8), "target.Borrar(id_" & tabla & ") ")
                PrintLine(FileCls, "")
                PrintLine(FileCls, TAB(8), "Dim dt As DataTable = New DataTable")
                PrintLine(FileCls, TAB(8), "dt = target.GetOne(id_" & tabla & ") ")
                PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "Assert.IsTrue(dt.Rows.Count = 0, " & """" & "son: " & """" & " & dt.Rows.Count & " & """" & " registros" & """" & ")")
                PrintLine(FileCls, TAB(4), "End Sub")
                PrintLine(FileCls, TAB(0), "End Class")
            End If

            FileClose(FileCls)
    End Sub


End Class
