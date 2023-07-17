Public Class ImprimirAbmEncabezadoCuerpo

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

    Sub ImprimirAbm(ByVal path As String, ByVal tabla As String, ByVal abm As Boolean, ByVal detalle As Boolean, ByVal usuario As Boolean)
        Dim nombreU As String = "" & tabla

        ' Defino variables
        Dim FileCls As Integer = FreeFile()
        Dim reg As regSistemaTabla

        Dim sErr As String = ""
        Dim sContents As String
        Dim bAns As String


        If abm = True Then



            nombreU = path & "frmAbm" & nombreU & ".vb"






            ' Abro un PathCls de texto (si el mismo existía se reempleza)
            FileOpen(FileCls, nombreU, OpenMode.Output)

            'definicion de variables

            PrintLine(FileCls, TAB(0), "Public Class frmAbm" & tabla)
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "Dim odt As DataTable")
            PrintLine(FileCls, TAB(4), "Dim BanderaConsulta" & tabla & " As Integer")
            PrintLine(FileCls, "")

            'Private Sub frmAbm_Load

            PrintLine(FileCls, TAB(4), "Private Sub frmAbm" & tabla & "_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load")
            PrintLine(FileCls, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnAgregar, " & """" & "Incorporar Nuevo " & tabla & """" & ")")
            PrintLine(FileCls, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnBorrar, " & """" & "Borrar un  " & tabla & " Existente" & """" & ")")
            PrintLine(FileCls, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnModificar, " & """" & "Modificar  un " & tabla & " Existente" & """" & ")")
            PrintLine(FileCls, TAB(8), "'Me.ttGeneral.SetToolTip(Me.btnConsultar, " & """" & "Consultar Datos del  " & tabla & " Existente" & """" & ")")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "Dim odt As DataTable")
            PrintLine(FileCls, TAB(8), "odt = o" & tabla & ".Cargar()")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "RefrescarGrilla()")
            PrintLine(FileCls, TAB(8), "Me.txtBuscar.Focus()")
            PrintLine(FileCls, TAB(8), "Me.lblTitulo.Text = " & """" & tabla & """")
            PrintLine(FileCls, TAB(8), "Me.Text = " & """" & tabla & """")
            PrintLine(FileCls, TAB(8), "Me.CancelButton = Me.btnSalir")
            PrintLine(FileCls, TAB(8), "Me.BackColor = Color.Gainsboro")
            PrintLine(FileCls, TAB(8), "Me.StartPosition = FormStartPosition.CenterScreen")
            PrintLine(FileCls, TAB(8), "Me.MinimizeBox = False")
            PrintLine(FileCls, TAB(8), "Me.MaximizeBox = False")
            PrintLine(FileCls, TAB(8), "")

            If usuario = True Then
                PrintLine(FileCls, TAB(8), "'control de acceso y de permisos")
                PrintLine(FileCls, TAB(8), "Dim odtUser As New DataTable")
                PrintLine(FileCls, TAB(8), "odtUser = oUsuario.GetPermiso(NombreUsuario, Me.Name)")
                PrintLine(FileCls, TAB(8), "If odtUser.Rows.Count = 0 OrElse CDbl(odtUser.Rows(0).Item(" & """" & "habilitado" & """" & ")) = 0 Then")
                PrintLine(FileCls, TAB(12), "Me.btnAgregar.Enabled = False")
                PrintLine(FileCls, TAB(12), "Me.btnBorrar.Enabled = False")
                PrintLine(FileCls, TAB(12), "Me.btnModificar.Enabled = False")
                PrintLine(FileCls, TAB(8), "End If")
                PrintLine(FileCls, TAB(8), "Me.btnAyuda.Visible = False")
                PrintLine(FileCls, TAB(8), "")
            End If

            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Public Sub RefrescarGrilla()

            PrintLine(FileCls, TAB(4), "Public Sub RefrescarGrilla()")
            PrintLine(FileCls, TAB(8), "Dim odt As DataTable")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "odt = o" & tabla & ".ConsultarTodo()")
            PrintLine(FileCls, TAB(8), "Me.dgv1.DataSource = odt")
            PrintLine(FileCls, TAB(8), "Me.dgv1.Columns(0).Visible = False")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub txtBuscar_KeyPress

            PrintLine(FileCls, TAB(4), "Private Sub txtBuscar_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBuscar.KeyPress")
            PrintLine(FileCls, TAB(8), "If e.KeyChar.ToString = vbCr Then")
            PrintLine(FileCls, TAB(12), "Me.btnModificar.Focus()")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub txtBuscar_TextChanged

            PrintLine(FileCls, TAB(4), "Private Sub txtBuscar_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged")
            PrintLine(FileCls, TAB(8), "If Me.txtBuscar.Text = " & """""" & " Then")
            PrintLine(FileCls, TAB(12), "Me.txtBuscar.Text = " & """" & " " & """")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(8), "odt = o" & tabla & ".Buscar(Me.txtBuscar.Text)")
            PrintLine(FileCls, TAB(8), "Me.dgv1.DataSource = odt")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub dgv1_CurrentCellChanged

            PrintLine(FileCls, TAB(4), "Private Sub dgv1_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv1.CurrentCellChanged")
            PrintLine(FileCls, TAB(8), "Try")
            PrintLine(FileCls, TAB(12), "Me.lblid_pk.Text = CStr(Me.dgv1.Item(0, Me.dgv1.CurrentRow.Index).Value)")
            PrintLine(FileCls, TAB(8), "Catch ex As Exception")
            PrintLine(FileCls, TAB(8), "End Try")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub Botones_Click

            PrintLine(FileCls, TAB(4), "Private Sub Botones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click, btnAyuda.Click, btnBorrar.Click, btnModificar.Click, btnSalir.Click")
            PrintLine(FileCls, TAB(8), "Dim btnTemp As New Button")
            PrintLine(FileCls, TAB(8), "Dim frmDetalle As New frmDetalle" & tabla)
            PrintLine(FileCls, TAB(8), "btnTemp = CType(sender, Button)")
            PrintLine(FileCls, TAB(8), "Try")
            PrintLine(FileCls, TAB(12), "Select Case btnTemp.Name")

            PrintLine(FileCls, TAB(16), "Case " & """" & "btnAgregar" & """")
            PrintLine(FileCls, TAB(20), "Bandera" & tabla & " = 1")
            PrintLine(FileCls, TAB(20), "Me.AddOwnedForm(frmDetalle)")
            PrintLine(FileCls, TAB(20), "o" & tabla & ".Insertar()")
            PrintLine(FileCls, TAB(20), "frmDetalle.CargarCombos()")
            PrintLine(FileCls, TAB(20), "frmDetalle.ShowDialog()")

            PrintLine(FileCls, TAB(16), "Case " & """" & "btnModificar" & """")
            PrintLine(FileCls, TAB(20), "If Not IsNumeric(Me.lblid_pk.Text) Then")
            PrintLine(FileCls, TAB(24), "Exit Sub")
            PrintLine(FileCls, TAB(20), "End If")

            '  PrintLine(FileCls, TAB(16), "If Me.lblid_pk.Text <= 1 Then")
            '  PrintLine(FileCls, TAB(20), "MessageBox.Show(" & """" & "No se Puede Modificar el Registro" & """" & ")")
            '  PrintLine(FileCls, TAB(20), "Exit Sub")
            '  PrintLine(FileCls, TAB(16), "End If")

            PrintLine(FileCls, TAB(20), "Bandera" & tabla & " = 2")
            PrintLine(FileCls, TAB(20), "Me.AddOwnedForm(frmDetalle)")
            PrintLine(FileCls, TAB(20), "o" & tabla & ".Modificar(CInt(Me.lblid_pk.Text))")
            PrintLine(FileCls, TAB(20), "frmDetalle.CargarCombos()")
            PrintLine(FileCls, TAB(20), "frmDetalle.ShowDialog()")
            PrintLine(FileCls, TAB(20), "RefrescarGrilla()")

            PrintLine(FileCls, TAB(16), "Case " & """" & "btnBorrar" & """")
            PrintLine(FileCls, TAB(20), "If Not IsNumeric(Me.lblid_pk.Text) Then")
            PrintLine(FileCls, TAB(24), "Exit Sub")
            PrintLine(FileCls, TAB(20), "End If")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(20), "If MessageBox.Show(" & """" & "Desea Realmente Eliminar el Registro " & """" & ", " & """" & "BORRAR" & """" & ", _")
            PrintLine(FileCls, TAB(20), "MessageBoxButtons.YesNo, MessageBoxIcon.Question) _")
            PrintLine(FileCls, TAB(20), "= Windows.Forms.DialogResult.No Then")
            PrintLine(FileCls, TAB(24), "Exit Sub")
            PrintLine(FileCls, TAB(20), "End If")
            PrintLine(FileCls, "")

            '   PrintLine(FileCls, TAB(16), "If Me.lblid_pk.Text <= 1 Then")
            '   PrintLine(FileCls, TAB(20), "MessageBox.Show(" & """" & "No se Puede Borrar el Registro" & """" & ")")
            '   PrintLine(FileCls, TAB(20), "Exit Sub")
            '   PrintLine(FileCls, TAB(16), "End If")

            PrintLine(FileCls, TAB(20), "oCuerpo_" & tabla & ".Delete" & tabla & "(CInt(Me.lblid_pk.Text), CodigoUsuario)")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(20), "o" & tabla & ".Borrar(CInt(Me.lblid_pk.Text))")
            PrintLine(FileCls, TAB(20), "RefrescarGrilla()")

            PrintLine(FileCls, TAB(16), "Case " & """" & "btnAyuda" & """")
            PrintLine(FileCls, TAB(20), "'Process.Start(PathAyuda + " & """" & "frmAbm" & tabla & ".avi" & """" & ")")
            PrintLine(FileCls, TAB(16), "Case " & """" & "btnSalir" & """")
            PrintLine(FileCls, TAB(20), "Me.Close()")
            PrintLine(FileCls, TAB(12), "End Select")
            PrintLine(FileCls, TAB(12), "Me.txtBuscar.Text = """"")
            PrintLine(FileCls, TAB(12), "Me.txtBuscar.Focus()")
            PrintLine(FileCls, TAB(8), "Catch ex As Exception")
            PrintLine(FileCls, TAB(8), "MessageBox.Show(ex.Message)")
            PrintLine(FileCls, TAB(8), "End Try")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "End Class")
            FileClose(FileCls)


            '*********************************************
            '
            'DEFINICION DEL CODIGO PARA FORMULARIOS ABM 2 PARTE
            '
            '*********************************************


            nombreU = path & "frmAbm" & tabla & ".Designer.vb"


            ' Defino variables
            FileCls = FreeFile()

            ' Abro un PathCls de texto (si el mismo existía se reempleza)
            FileOpen(FileCls, nombreU, OpenMode.Output)

            'definicion de variables
            PrintLine(FileCls, TAB(0), "<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _")
            PrintLine(FileCls, TAB(0), "Partial Class frmAbm" & tabla)
            PrintLine(FileCls, TAB(4), "Inherits System.Windows.Forms.Form")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "'Form reemplaza a Dispose para limpiar la lista de componentes.")
            PrintLine(FileCls, TAB(4), "<System.Diagnostics.DebuggerNonUserCode()> _")
            PrintLine(FileCls, TAB(4), "Protected Overrides Sub Dispose(ByVal disposing As Boolean)")
            PrintLine(FileCls, TAB(8), "If disposing AndAlso components IsNot Nothing Then")
            PrintLine(FileCls, TAB(12), "components.Dispose()")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(8), "MyBase.Dispose(disposing)")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "'Requerido por el Diseñador de Windows Forms")
            PrintLine(FileCls, TAB(4), "Private components As System.ComponentModel.IContainer")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento")
            PrintLine(FileCls, TAB(4), "'Se puede modificar usando el Diseñador de Windows Forms.")
            PrintLine(FileCls, TAB(4), "'No lo modifique con el editor de código.")
            PrintLine(FileCls, TAB(4), "<System.Diagnostics.DebuggerStepThrough()> _")
            PrintLine(FileCls, TAB(4), "Private Sub InitializeComponent()")
            PrintLine(FileCls, TAB(8), "Me.components = New System.ComponentModel.Container")
            PrintLine(FileCls, TAB(8), "Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAbm" & tabla & "))")
            PrintLine(FileCls, TAB(8), "Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle")
            PrintLine(FileCls, TAB(8), "Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle")
            PrintLine(FileCls, TAB(8), "Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle")
            PrintLine(FileCls, TAB(8), "Me.txtBuscar = New System.Windows.Forms.TextBox")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1 = New System.Windows.Forms.GroupBox")
            PrintLine(FileCls, TAB(8), "Me.btnAyuda = New System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(8), "Me.btnSalir = New System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar = New System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(8), "Me.btnModificar = New System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar = New System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(8), "Me.lblconsultar = New System.Windows.Forms.Label")
            PrintLine(FileCls, TAB(8), "Me.ttGeneral = New System.Windows.Forms.ToolTip(Me.components)")
            PrintLine(FileCls, TAB(8), "Me.lblTitulo = New System.Windows.Forms.Label")
            PrintLine(FileCls, TAB(8), "Me.dgv1 = New System.Windows.Forms.DataGridView")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk = New System.Windows.Forms.Label")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.SuspendLayout()")
            PrintLine(FileCls, TAB(8), "CType(Me.dgv1, System.ComponentModel.ISupportInitialize).BeginInit()")
            PrintLine(FileCls, TAB(8), "Me.SuspendLayout()")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'txtBuscar")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.txtBuscar.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper")
            PrintLine(FileCls, TAB(8), "Me.txtBuscar.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "Me.txtBuscar.Location = New System.Drawing.Point(128, 561)")
            PrintLine(FileCls, TAB(8), "Me.txtBuscar.Name = ""txtBuscar""")
            PrintLine(FileCls, TAB(8), "Me.txtBuscar.Size = New System.Drawing.Size(873, 26)")
            PrintLine(FileCls, TAB(8), "Me.txtBuscar.TabIndex = 563")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'GroupBox1")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.Controls.Add(Me.btnAyuda)")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.Controls.Add(Me.btnSalir)")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.Controls.Add(Me.btnBorrar)")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.Controls.Add(Me.btnModificar)")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.Controls.Add(Me.btnAgregar)")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.Location = New System.Drawing.Point(15, 606)")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.Name = ""GroupBox1""")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.Size = New System.Drawing.Size(986, 107)")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.TabIndex = 564")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.TabStop = False")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'btnAyuda")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.btnAyuda.BackColor = System.Drawing.Color.Gainsboro")
            PrintLine(FileCls, TAB(8), "Me.btnAyuda.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "Me.btnAyuda.ForeColor = System.Drawing.Color.Black")
            PrintLine(FileCls, TAB(8), "Me.btnAyuda.Image = CType(resources.GetObject(""btnAyuda.Image""), System.Drawing.Image)")
            PrintLine(FileCls, TAB(8), "Me.btnAyuda.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
            PrintLine(FileCls, TAB(8), "Me.btnAyuda.Location = New System.Drawing.Point(715, 19)")
            PrintLine(FileCls, TAB(8), "Me.btnAyuda.Name = ""btnAyuda""")
            PrintLine(FileCls, TAB(8), "Me.btnAyuda.Size = New System.Drawing.Size(86, 71)")
            PrintLine(FileCls, TAB(8), "Me.btnAyuda.TabIndex = 11")
            PrintLine(FileCls, TAB(8), "Me.btnAyuda.Text = ""A&yuda""")
            PrintLine(FileCls, TAB(8), "Me.btnAyuda.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
            PrintLine(FileCls, TAB(8), "Me.btnAyuda.UseVisualStyleBackColor = True")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'btnSalir")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.ForeColor = System.Drawing.Color.Black")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Image = CType(resources.GetObject(""btnSalir.Image""), System.Drawing.Image)")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Location = New System.Drawing.Point(858, 19)")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Name = ""btnSalir""")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Size = New System.Drawing.Size(86, 71)")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.TabIndex = 12")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Text = ""&Salir""")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.UseVisualStyleBackColor = True")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'btnBorrar")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.Cursor = System.Windows.Forms.Cursors.Hand")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.Image = CType(resources.GetObject(""btnBorrar.Image""), System.Drawing.Image)")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.Location = New System.Drawing.Point(292, 19)")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.Name = ""btnBorrar""")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.Size = New System.Drawing.Size(86, 71)")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.TabIndex = 10")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.Text = ""&Borrar""")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.UseVisualStyleBackColor = True")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'btnModificar")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.Cursor = System.Windows.Forms.Cursors.Hand")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.Image = CType(resources.GetObject(""btnModificar.Image""), System.Drawing.Image)")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.Location = New System.Drawing.Point(160, 19)")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.Name = ""btnModificar""")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.Size = New System.Drawing.Size(86, 71)")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.TabIndex = 9")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.Text = ""&Modificar""")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.UseVisualStyleBackColor = True")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'btnAgregar")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.Cursor = System.Windows.Forms.Cursors.Hand")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.Image = CType(resources.GetObject(""btnAgregar.Image""), System.Drawing.Image)")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.Location = New System.Drawing.Point(28, 19)")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.Name = ""btnAgregar""")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.Size = New System.Drawing.Size(86, 71)")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.TabIndex = 8")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.Text = ""&Agregar""")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.UseVisualStyleBackColor = True")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'lblconsultar")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.lblconsultar.AutoSize = True")
            PrintLine(FileCls, TAB(8), "Me.lblconsultar.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
            PrintLine(FileCls, TAB(8), "Me.lblconsultar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
            PrintLine(FileCls, TAB(8), "Me.lblconsultar.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "Me.lblconsultar.ForeColor = System.Drawing.Color.Blue")
            PrintLine(FileCls, TAB(8), "Me.lblconsultar.Location = New System.Drawing.Point(15, 561)")
            PrintLine(FileCls, TAB(8), "Me.lblconsultar.Name = ""lblconsultar""")
            PrintLine(FileCls, TAB(8), "Me.lblconsultar.Size = New System.Drawing.Size(100, 26)")
            PrintLine(FileCls, TAB(8), "Me.lblconsultar.TabIndex = 567")
            PrintLine(FileCls, TAB(8), "Me.lblconsultar.Text = ""Consultar""")
            PrintLine(FileCls, TAB(8), "Me.lblconsultar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'lblTitulo")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.lblTitulo.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
            PrintLine(FileCls, TAB(8), "Me.lblTitulo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
            PrintLine(FileCls, TAB(8), "Me.lblTitulo.Font = New System.Drawing.Font(""Times New Roman"", 18.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "Me.lblTitulo.ForeColor = System.Drawing.Color.Red")
            PrintLine(FileCls, TAB(8), "Me.lblTitulo.Location = New System.Drawing.Point(281, 22)")
            PrintLine(FileCls, TAB(8), "Me.lblTitulo.Name = ""lblTitulo""")
            PrintLine(FileCls, TAB(8), "Me.lblTitulo.Size = New System.Drawing.Size(456, 30)")
            PrintLine(FileCls, TAB(8), "Me.lblTitulo.TabIndex = 566")
            PrintLine(FileCls, TAB(8), "Me.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'dgv1")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.dgv1.AllowUserToAddRows = False")
            PrintLine(FileCls, TAB(8), "Me.dgv1.AllowUserToResizeColumns = False")
            PrintLine(FileCls, TAB(8), "Me.dgv1.AllowUserToResizeRows = False")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle1.BackColor = System.Drawing.Color.LightBlue")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightGoldenrodYellow")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Brown")
            PrintLine(FileCls, TAB(8), "Me.dgv1.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1")
            PrintLine(FileCls, TAB(8), "Me.dgv1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells")
            PrintLine(FileCls, TAB(8), "Me.dgv1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells")
            PrintLine(FileCls, TAB(8), "Me.dgv1.BackgroundColor = System.Drawing.Color.PeachPuff")
            PrintLine(FileCls, TAB(8), "Me.dgv1.BorderStyle = System.Windows.Forms.BorderStyle.None")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle2.BackColor = System.Drawing.Color.Gold")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle2.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]")
            PrintLine(FileCls, TAB(8), "Me.dgv1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2")
            PrintLine(FileCls, TAB(8), "Me.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle3.BackColor = System.Drawing.Color.Lavender")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle3.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Beige")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Brown")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]")
            PrintLine(FileCls, TAB(8), "Me.dgv1.DefaultCellStyle = DataGridViewCellStyle3")
            PrintLine(FileCls, TAB(8), "Me.dgv1.GridColor = System.Drawing.Color.MediumPurple")
            PrintLine(FileCls, TAB(8), "Me.dgv1.Location = New System.Drawing.Point(15, 83)")
            PrintLine(FileCls, TAB(8), "Me.dgv1.Name = ""dgv1""")
            PrintLine(FileCls, TAB(8), "Me.dgv1.ReadOnly = True")
            PrintLine(FileCls, TAB(8), "Me.dgv1.ShowCellErrors = False")
            PrintLine(FileCls, TAB(8), "Me.dgv1.ShowRowErrors = False")
            PrintLine(FileCls, TAB(8), "Me.dgv1.Size = New System.Drawing.Size(986, 457)")
            PrintLine(FileCls, TAB(8), "Me.dgv1.StandardTab = True")
            PrintLine(FileCls, TAB(8), "Me.dgv1.TabIndex = 562")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'lblid_pk")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk.AutoSize = True")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk.BackColor = System.Drawing.Color.Red")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk.Location = New System.Drawing.Point(21, 94)")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk.Name = ""lblid_pk""")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk.Size = New System.Drawing.Size(13, 13)")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk.TabIndex = 565")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk.Text = ""0""")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk.Visible = False")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'frmAbm" & tabla)
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)")
            PrintLine(FileCls, TAB(8), "Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font")
            PrintLine(FileCls, TAB(8), "Me.ClientSize = New System.Drawing.Size(1016, 734)")
            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.txtBuscar)")
            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.GroupBox1)")
            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.lblconsultar)")
            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.lblTitulo)")
            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.lblid_pk)")
            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.dgv1)")
            PrintLine(FileCls, TAB(8), "Me.Name = ""frmAbm" & tabla & """")
            PrintLine(FileCls, TAB(8), "Me.Text = ""frmAbm" & tabla & """")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.ResumeLayout(False)")
            PrintLine(FileCls, TAB(8), "CType(Me.dgv1, System.ComponentModel.ISupportInitialize).EndInit()")
            PrintLine(FileCls, TAB(8), "Me.ResumeLayout(False)")
            PrintLine(FileCls, TAB(8), "Me.PerformLayout()")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, TAB(4), "Protected WithEvents txtBuscar As System.Windows.Forms.TextBox")
            PrintLine(FileCls, TAB(4), "Protected WithEvents GroupBox1 As System.Windows.Forms.GroupBox")
            PrintLine(FileCls, TAB(4), "Protected WithEvents btnAyuda As System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(4), "Protected WithEvents btnSalir As System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(4), "Protected WithEvents btnBorrar As System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(4), "Protected WithEvents btnModificar As System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(4), "Protected WithEvents btnAgregar As System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(4), "Protected WithEvents lblconsultar As System.Windows.Forms.Label")
            PrintLine(FileCls, TAB(4), "Protected WithEvents ttGeneral As System.Windows.Forms.ToolTip")
            PrintLine(FileCls, TAB(4), "Protected WithEvents lblTitulo As System.Windows.Forms.Label")
            PrintLine(FileCls, TAB(4), "Protected WithEvents dgv1 As System.Windows.Forms.DataGridView")
            PrintLine(FileCls, TAB(4), "Friend WithEvents lblid_pk As System.Windows.Forms.Label")
            PrintLine(FileCls, TAB(0), "End Class")
            PrintLine(FileCls, "")
            FileClose(FileCls)



            sContents = GetFileContents(path & "frmAbmGeneral.resx", sErr)
            If sErr = "" Then
                Debug.WriteLine("File Contents: " & sContents)
                'Save to different file
                bAns = SaveTextToFile(sContents, path & "frmAbm" & tabla & ".resx", sErr)
            End If
        End If



















        '**************************************************
        '
        'DEFINICION DEL CODIGA PARA FORMULARIOS DE DETALLE
        '
        '**************************************************
        If abm = True Then



            Dim Contador As Integer = 1


            nombreU = path & "frmDetalle" & tabla & ".vb"

            ' Defino variables
            FileCls = FreeFile()

            ' Abro un PathCls de texto (si el mismo existía se reempleza)
            FileOpen(FileCls, nombreU, OpenMode.Output)


            'saca el nombre del primer control para el focus
            Contador = 1
            Dim strFocus As String = ""
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)
                            strFocus = "Me.cmb" & Nombre
                            Exit For
                        Case "String", "Decimal"
                            strFocus = "Me.txt" & reg.nombre
                            Exit For
                        Case "DateTime"
                            strFocus = "Me.dtp" & reg.nombre
                            Exit For
                        Case "Boolean"
                            strFocus = "Me.chk" & reg.nombre
                            Exit For
                    End Select
                End If
                Contador = Contador + 1
            Next

            'saca el nombre del primer txt para los vacios
            Contador = 1
            Dim strVacio As String = ""
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                Else
                    Select Case reg.tiposql
                        Case "String", "Decimal"
                            strVacio = "Me.txt" & reg.nombre
                            Exit For
                    End Select
                End If
                Contador = Contador + 1
            Next

            'Private Sub frmDetalle_Load
            PrintLine(FileCls, TAB(0), "Public Class frmDetalle" & tabla)
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "Dim BanderaCierreForm As Boolean = False")
            PrintLine(FileCls, TAB(4), "Private Sub frmDetalle" & tabla & "_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing")
            PrintLine(FileCls, TAB(8), "If BanderaCierreForm = False Then")
            PrintLine(FileCls, TAB(12), "e.Cancel = True")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "Private Sub frmDetalle" & tabla & "_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)
                            PrintLine(FileCls, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnBusca" & Nombre & ", " & """" & "Buscar Nuevo " & Nombre & """" & ")")
                    End Select
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnGuardar, " & """" & "Guardar Datos del " & tabla & """" & ")")
            PrintLine(FileCls, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnSalir, " & """" & "Volver a la Pantalla Anterior" & """" & ")")
            PrintLine(FileCls, TAB(8), "Try")
            PrintLine(FileCls, TAB(12), "ObtenerDatos()")
            PrintLine(FileCls, "")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)
                            ' PrintLine(FileCls, TAB(8), "o" & Nombre & ".Modificar(Me.lblid_" & Nombre & ".Text)")
                            ' PrintLine(FileCls, TAB(8), "Me.cmb" & Nombre & ".Text = o" & Nombre & ".nombre_" & Nombre)
                            PrintLine(FileCls, TAB(12), "Me.cmb" & Nombre & ".Text = o" & Nombre & ".GetOne(CInt(Me.lblid_" & Nombre & ".Text)).Rows(0).Item(" & """" & "nombre_" & Nombre & """" & ").ToString.Trim")
                    End Select
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(8), "Catch ex As Exception")
            PrintLine(FileCls, TAB(12), "'    MessageBox.Show(ex.Message)")
            PrintLine(FileCls, TAB(8), "End Try")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "Me.lblid_" & tabla & ".Text = CType(Me.Owner, frmAbm" & tabla & ").lblid_pk.Text")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "If Bandera" & tabla & "  = 1 Then")
            PrintLine(FileCls, TAB(12), "Me.LimpiarControles()")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(8), "Me.Text = " & """" & tabla & """")
            PrintLine(FileCls, TAB(8), "Me.CancelButton = Me.btnSalir")
            PrintLine(FileCls, TAB(8), "Me.BackColor = Color.Gainsboro")
            PrintLine(FileCls, TAB(8), "Me.StartPosition = FormStartPosition.CenterScreen")
            PrintLine(FileCls, TAB(8), "Me.MinimizeBox = False")
            PrintLine(FileCls, TAB(8), "Me.MaximizeBox = False")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "oCuerpo_" & tabla & ".Cargar()")
            PrintLine(FileCls, TAB(4), "Me.RefrescarGrilla()")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Sub CargarCombos()

            PrintLine(FileCls, TAB(4), "Sub CargarCombos()")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)

                            PrintLine(FileCls, TAB(8), "Me.Cargar" & Nombre)
                    End Select
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)

                            PrintLine(FileCls, TAB(4), "Sub Cargar" & Nombre & "()")
                            PrintLine(FileCls, TAB(8), "Dim odt As New DataTable")
                            PrintLine(FileCls, "")
                            PrintLine(FileCls, TAB(8), "odt = o" & Nombre & ".GetCmb")
                            PrintLine(FileCls, TAB(8), "With Me.cmb" & Nombre)
                            PrintLine(FileCls, TAB(12), ".DataSource = odt")
                            PrintLine(FileCls, TAB(12), ".DisplayMember = " & """" & "nombre_" & Nombre & """")
                            PrintLine(FileCls, TAB(12), ".ValueMember = " & """" & "id_" & Nombre & """")
                            PrintLine(FileCls, TAB(8), "End With")
                            PrintLine(FileCls, TAB(8), "If Me.cmb" & Nombre & ".SelectedIndex >= 0 Then")
                            PrintLine(FileCls, TAB(12), "cmb" & Nombre & ".SelectedIndex = 0")
                            PrintLine(FileCls, TAB(12), "Me.lblid_" & Nombre & ".Text = cmb" & Nombre & ".SelectedValue.ToString")
                            PrintLine(FileCls, TAB(8), "End If")
                            PrintLine(FileCls, TAB(4), "End Sub")
                            PrintLine(FileCls, "")
                    End Select
                End If
                Contador = Contador + 1
            Next

            'Sub LimpiarControles()

            PrintLine(FileCls, TAB(4), "Sub LimpiarControles()")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".Text = " & """" & "0" & """")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & Nombre & ".Text =  """"")
                        Case "String", "Decimal"
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Text = """"")
                        Case "DateTime"
                            PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".Value = DateTime.Now")
                        Case "Boolean"
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Checked = False")
                        Case Else
                            PrintLine(FileCls, TAB(8), "Me." & reg.nombre & ".Text = """"")
                    End Select
                End If
                ' PrintLine(FileCls, TAB(8), "Me.lblid_" & tabla & ".Text = " & """" & "0" & """")
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub ObtenerDatos()

            PrintLine(FileCls, TAB(4), "Private Sub ObtenerDatos()")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".Text = o" & tabla & "." & reg.nombre & ".ToString")
                        Case "String"
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Text = o" & tabla & "." & reg.nombre & ".Trim")
                        Case "Decimal"
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Text = o" & tabla & "." & reg.nombre & ".ToString.Trim")
                        Case "DateTime"
                            PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".Value = o" & tabla & "." & reg.nombre)
                        Case "Boolean"
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Checked = o" & tabla & "." & reg.nombre)
                        Case Else
                            PrintLine(FileCls, TAB(8), "Me." & reg.nombre & ".Text = Trim(o" & tabla & "." & reg.nombre & ")")
                    End Select

                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub AsignarDatos()

            PrintLine(FileCls, TAB(4), "Private Sub AsignarDatos()")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "operacion" Or reg.nombre = "sincronizado" Then
                    'next row
                ElseIf reg.nombre = "id_cliente_maestro" Then
                    PrintLine(FileCls, TAB(8), "o" & tabla & "." & reg.nombre & " = id_cliente_maestro")
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            PrintLine(FileCls, TAB(8), "o" & tabla & "." & reg.nombre & " = CInt(Me.lbl" & reg.nombre & ".Text)")
                        Case "String"
                            PrintLine(FileCls, TAB(8), "o" & tabla & "." & reg.nombre & " = Me.txt" & reg.nombre & ".Text")
                        Case "Decimal"
                            PrintLine(FileCls, TAB(8), "o" & tabla & "." & reg.nombre & " = CDec(Me.txt" & reg.nombre & ".Text)")
                        Case "DateTime"
                            PrintLine(FileCls, TAB(8), "o" & tabla & "." & reg.nombre & " = Me.dtp" & reg.nombre & ".Value.Date")
                        Case "Boolean"
                            PrintLine(FileCls, TAB(8), "o" & tabla & "." & reg.nombre & " = Me.chk" & reg.nombre & ".Checked")
                        Case Else
                            PrintLine(FileCls, TAB(8), "o" & tabla & "." & reg.nombre & " = Me." & reg.nombre & ".Text")
                    End Select

                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Public Sub SoloLectura()

            PrintLine(FileCls, TAB(4), "Public Sub SoloLectura()")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)
                            '  PrintLine(FileCls, TAB(4), "Me.lbl" & reg.nombre & ".Text = " & """" & "0" & """")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & Nombre & ".Enabled = False")
                            PrintLine(FileCls, TAB(8), "Me.btnBusca" & Nombre & ".Enabled = False")
                        Case "String", "Decimal"
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Enabled = False")
                        Case "DateTime"
                            PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".Enabled = False")
                        Case "Boolean"
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Enabled = False")
                        Case Else
                            PrintLine(FileCls, TAB(8), "Me." & reg.nombre & ".Enabled = False")
                    End Select
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.Visible = False")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub Guardar()

            PrintLine(FileCls, TAB(4), "Private Sub Guardar()")
            PrintLine(FileCls, TAB(8), "Try")
            PrintLine(FileCls, TAB(12), "Me.AsignarDatos()")
            PrintLine(FileCls, TAB(12), "If o" & tabla & ".Exist() Then")
            PrintLine(FileCls, TAB(16), "If Bandera" & tabla & " = 1 Then")
            PrintLine(FileCls, TAB(20), "MessageBox.Show(" & """" & "Sr. Usuario: Los Datos que Pretende Ingresar ya Fueron Cargados en el Sistema" & """" & ")")
            PrintLine(FileCls, TAB(16), "Exit Sub")
            PrintLine(FileCls, TAB(16), "ElseIf Bandera" & tabla & " = 2 Then")
            PrintLine(FileCls, TAB(20), "If MessageBox.Show(" & """" & "Sr. Usuario: Los Datos que Desea Modificar ya Existen ¿Desea Reemplazarlos?" & """" & ", " & """" & "MODIFICAR" & """" & ", _")
            PrintLine(FileCls, TAB(24), "MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) _")
            PrintLine(FileCls, TAB(24), "= Windows.Forms.DialogResult.No Then")
            PrintLine(FileCls, TAB(24), "Exit Sub")
            PrintLine(FileCls, TAB(20), "End If")
            PrintLine(FileCls, TAB(16), "End If")
            PrintLine(FileCls, TAB(12), "End If")

            PrintLine(FileCls, TAB(12), "Select Case Bandera" & tabla)
            PrintLine(FileCls, TAB(16), "Case 1 'GUARDA,REFRESCA LA GRILLA E INSERTA UNO NUEVO (AGREGAR)")
            PrintLine(FileCls, TAB(20), "o" & tabla & ".Guardar()")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(20), "Me.lblid_" & tabla & ".Text = o" & tabla & ".id_" & tabla & ".ToString")
            PrintLine(FileCls, TAB(20), "oCuerpo_" & tabla & ".UpdateID(CInt(Me.lblid_" & tabla & ".Text), CodigoUsuario)")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(20), "CType(Me.Owner, frmAbm" & tabla & ").RefrescarGrilla()")
            PrintLine(FileCls, TAB(20), "'Me.CargarCombos()")
            PrintLine(FileCls, TAB(20), "o" & tabla & ".Insertar()")
            PrintLine(FileCls, TAB(20), "'Me.ObtenerDatos()")
            PrintLine(FileCls, TAB(20), "Me.LimpiarControles()")
            PrintLine(FileCls, TAB(20), strFocus & ".Focus")
            PrintLine(FileCls, TAB(20), "Me.RefrescarGrilla()")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(20), "Me.lblid_" & tabla & ".Text = ""0""")
            PrintLine(FileCls, TAB(20), "Me.RefrescarGrilla()")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(16), "Case 2 'GUARDA Y SALE (MODIFICAR)")
            PrintLine(FileCls, TAB(20), "o" & tabla & ".Guardar()")
            PrintLine(FileCls, TAB(20), "BanderaCierreForm = True")
            PrintLine(FileCls, TAB(20), "Me.Close()")
            PrintLine(FileCls, TAB(12), "End Select")
            PrintLine(FileCls, TAB(8), "Catch ex As Exception")
            PrintLine(FileCls, TAB(12), "MessageBox.Show(ex.Message)")
            PrintLine(FileCls, TAB(8), "End Try")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Function ChequearVacios()

            PrintLine(FileCls, TAB(4), "Private Function ChequearVacios() As Boolean")
            PrintLine(FileCls, TAB(8), "Dim bandera As Boolean")
            PrintLine(FileCls, TAB(8), "If " & strVacio & ".Text = """"" & " Then")
            PrintLine(FileCls, TAB(12), "bandera = False")
            PrintLine(FileCls, TAB(8), "Else")
            PrintLine(FileCls, TAB(12), "bandera = True")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(8), "Return bandera")
            PrintLine(FileCls, TAB(4), "End Function")
            PrintLine(FileCls, "")

            'Private Sub btnAyuda_Click ANULADO

            '    PrintLine(FileCls, TAB(0), "Private Sub btnAyuda_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAyuda.Click")
            '    PrintLine(FileCls, TAB(4), "'      System.Diagnostics.Process.Start(PathAyuda + " & """" & "FrmDetalle" & tabla & """" & ".htm)")
            '    PrintLine(FileCls, TAB(0), "End Sub")
            '    PrintLine(FileCls, "")

            'Private Sub btnGuardar_Click

            PrintLine(FileCls, TAB(4), "Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click")
            PrintLine(FileCls, TAB(8), "Dim blnVacios As Boolean")
            PrintLine(FileCls, "")


            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            PrintLine(FileCls, TAB(8), "If CDbl(Me.lbl" & reg.nombre & ".Text) = 0 Then")
                            'si no tiene 'id_' salta el error
                            Try
                                PrintLine(FileCls, TAB(12), "MessageBox.Show(" & """" & "Debe Seleccionar un Dato del Combo de " & reg.nombre.Substring(3) & """" & ")")
                            Catch ex As Exception
                            End Try
                            PrintLine(FileCls, TAB(12), "Exit Sub")
                            PrintLine(FileCls, TAB(8), "End If")
                    End Select
                End If
                Contador = Contador + 1
            Next

            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "blnVacios = Me.ChequearVacios")
            PrintLine(FileCls, TAB(8), "If blnVacios = False Then")
            PrintLine(FileCls, TAB(12), "MessageBox.Show(" & """" & "Debe Llenar los Campos Obligatorios" & """" & ")")
            PrintLine(FileCls, TAB(12), "Exit Sub")
            PrintLine(FileCls, TAB(8), "Else")
            PrintLine(FileCls, TAB(12), "If Bandera" & tabla & "  = 1 Then")
            PrintLine(FileCls, TAB(16), "Me.Guardar()")
            PrintLine(FileCls, TAB(12), "Else")
            PrintLine(FileCls, TAB(16), "Me.Guardar()")
            PrintLine(FileCls, TAB(12), "End If")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub btnSalir_Click

            PrintLine(FileCls, TAB(4), "Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click")
            PrintLine(FileCls, TAB(8), "BanderaCierreForm = True")
            PrintLine(FileCls, TAB(8), "If Me.lblid_" & tabla & ".Text = ""0""" & " Then")
            PrintLine(FileCls, TAB(12), "oCuerpo_" & tabla & ".Delete" & tabla & "(0, CodigoUsuario)")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "Me.Close()")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub cmb

            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)

                            PrintLine(FileCls, TAB(4), "Private Sub cmb" & Nombre & "_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmb" & Nombre & ".SelectedIndexChanged")
                            PrintLine(FileCls, TAB(8), "If Me.cmb" & Nombre & ".SelectedIndex >= 0 Then")
                            PrintLine(FileCls, TAB(12), "Me.lbl" & reg.nombre & ".Text = cmb" & Nombre & ".SelectedValue.ToString")
                            PrintLine(FileCls, TAB(8), "End If")
                            PrintLine(FileCls, TAB(4), "End Sub")
                            PrintLine(FileCls, "")

                            PrintLine(FileCls, TAB(4), "Private Sub btnBusca" & Nombre & "_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBusca" & Nombre & ".Click")
                            PrintLine(FileCls, TAB(8), "Dim frmTemporal As New FrmAbm" & Nombre)
                            PrintLine(FileCls, TAB(8), "frmTemporal.ShowDialog()")
                            PrintLine(FileCls, TAB(8), "Me.Cargar" & Nombre & "()")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & Nombre & ".Focus()")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & Nombre & ".Text = """"")
                            PrintLine(FileCls, TAB(8), "Me.lblid_" & Nombre & ".Text = " & """" & "0" & """")
                            PrintLine(FileCls, TAB(4), "End Sub")
                            PrintLine(FileCls, "")
                    End Select
                End If
                Contador = Contador + 1
            Next

            'arma las cadenas de tabulacion
            Contador = 1
            Dim TabPress As String = ""
            Dim TabDown As String = ""
            Dim TabDecimal As String = ""

            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)
                            '  PrintLine(FileCls, TAB(4), "Me.lbl" & reg.nombre & ".Text = " & """" & "0" & """")
                            TabDown = TabDown & "cmb" & Nombre & ".KeyDown, "
                        Case "String"
                            TabPress = TabPress & "txt" & reg.nombre & ".KeyPress, "
                        Case "Decimal"
                            TabDecimal = TabDecimal & "txt" & reg.nombre & ".KeyPress, "
                        Case "DateTime"
                            TabDown = TabDown & "dtp" & reg.nombre & ".KeyDown, "
                        Case "Boolean"
                            TabPress = TabPress & "chk" & reg.nombre & ".KeyPress, "
                    End Select
                End If
                Contador = Contador + 1
            Next
            Dim LargoTab As Integer = TabPress.Length
            If LargoTab >= 2 Then
                TabPress = TabPress.Substring(0, LargoTab - 2)
            End If
            LargoTab = TabDown.Length
            If LargoTab >= 2 Then
                TabDown = TabDown.Substring(0, LargoTab - 2)
            End If
            LargoTab = TabDecimal.Length
            If LargoTab >= 2 Then
                TabDecimal = TabDecimal.Substring(0, LargoTab - 2)
            End If

            'Private Sub TabulacionTextBox

            PrintLine(FileCls, TAB(4), "Private Sub TabulacionTextBox(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _") 'Handles " & TabPress)
            If TabPress = "" Then
                PrintLine(FileCls, TAB(4), "'Handles " & TabPress)
            Else
                PrintLine(FileCls, TAB(4), "Handles " & TabPress)
            End If
            PrintLine(FileCls, TAB(8), "If e.KeyChar.ToString = vbCr Then")
            PrintLine(FileCls, TAB(12), "Me.SelectNextControl(Me.ActiveControl, True, True, True, True)")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub TabulacionCombos

            PrintLine(FileCls, TAB(4), "Private Sub TabulacionCombos(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) _") 'Handles " & TabDown)
            If TabDown = "" Then
                PrintLine(FileCls, TAB(4), "'Handles " & TabDown)
            Else
                PrintLine(FileCls, TAB(4), "Handles " & TabDown)
            End If
            PrintLine(FileCls, TAB(8), "If CDbl(e.KeyValue.ToString) = 13 Then")
            PrintLine(FileCls, TAB(12), "Me.SelectNextControl(Me.ActiveControl, True, True, True, True)")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private sub tabulacion decimales
            PrintLine(FileCls, TAB(4), "Private Sub Decimales(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _")
            If TabDecimal = "" Then
                PrintLine(FileCls, TAB(4), "'Handles " & TabDecimal)
            Else
                PrintLine(FileCls, TAB(4), "Handles " & TabDecimal)
            End If
            PrintLine(FileCls, TAB(8), "Dim txtTemp As TextBox")
            PrintLine(FileCls, TAB(8), "txtTemp = CType(sender, TextBox)")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "If e.KeyChar.ToString = vbCr Then")
            PrintLine(FileCls, TAB(12), "Me.SelectNextControl(Me.ActiveControl, True, True, True, True)")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(8), "If e.KeyChar.ToString = " & """" & "." & """" & " Or e.KeyChar.ToString = " & """" & "," & """" & " Then")
            PrintLine(FileCls, TAB(12), "If InStr(txtTemp.Text, " & """" & "," & """" & ") <> 0 Then")
            PrintLine(FileCls, TAB(16), "e.Handled = True")
            PrintLine(FileCls, TAB(12), "Else")
            PrintLine(FileCls, TAB(16), "e.KeyChar = CChar(" & """" & "," & """" & ")")
            PrintLine(FileCls, TAB(12), "End If")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "Dim Largo As Integer = InStr(txtTemp.Text, " & """" & "," & """" & ")")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "If txtTemp.Text.Length > Largo + 2 And Largo <> 0 And e.KeyChar.ToString <> vbBack Then")
            PrintLine(FileCls, TAB(12), "e.Handled = True")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "If (e.KeyChar.ToString >= " & """" & "0" & """" & " And e.KeyChar.ToString <= " & """" & "9" & """" & ") Or e.KeyChar.ToString = " & """" & "," & """" & " Or e.KeyChar = vbBack Then")
            PrintLine(FileCls, TAB(12), "'  e.Handled = False")
            PrintLine(FileCls, TAB(8), "Else")
            PrintLine(FileCls, TAB(12), "e.Handled = True")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")
            PrintLine(FileCls, "")
            PrintLine(FileCls, "")
            PrintLine(FileCls, "")
            PrintLine(FileCls, "")
            PrintLine(FileCls, "")
            PrintLine(FileCls, "")

            'Public Sub RefrescarGrilla()

            PrintLine(FileCls, TAB(4), "Public Sub RefrescarGrilla()")
            PrintLine(FileCls, TAB(8), "Dim odt As DataTable")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "odt = oCuerpo_" & tabla & ".GetAllMovimiento(CInt(Me.lblid_" & tabla & ".Text))")
            PrintLine(FileCls, TAB(8), "Me.dgv1.DataSource = odt")
            PrintLine(FileCls, TAB(8), "Me.dgv1.Columns(0).Visible = False")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub dgv1_CurrentCellChanged

            PrintLine(FileCls, TAB(4), "Private Sub dgv1_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv1.CurrentCellChanged")
            PrintLine(FileCls, TAB(8), "Try")
            PrintLine(FileCls, TAB(12), "Me.lblid_pk.Text = CStr(Me.dgv1.Item(0, Me.dgv1.CurrentRow.Index).Value)")
            PrintLine(FileCls, TAB(8), "Catch ex As Exception")
            PrintLine(FileCls, TAB(8), "End Try")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub Botones_Click

            PrintLine(FileCls, TAB(4), "Private Sub Botones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click,  btnBorrar.Click, btnModificar.Click")
            PrintLine(FileCls, TAB(8), "Dim btnTemp As New Button")
            PrintLine(FileCls, TAB(8), "Dim frmDetalle As New frmDetalleCuerpo_" & tabla)
            PrintLine(FileCls, TAB(8), "btnTemp = CType(sender, Button)")
            PrintLine(FileCls, TAB(8), "Try")
            PrintLine(FileCls, TAB(12), "Select Case btnTemp.Name")

            PrintLine(FileCls, TAB(16), "Case " & """" & "btnAgregar" & """")
            PrintLine(FileCls, TAB(20), "BanderaCuerpo_" & tabla & " = 1")
            PrintLine(FileCls, TAB(20), "Me.AddOwnedForm(frmDetalle)")
            PrintLine(FileCls, TAB(20), "oCuerpo_" & tabla & ".Insertar()")
            PrintLine(FileCls, TAB(20), "frmDetalle.CargarCombos()")
            PrintLine(FileCls, TAB(20), "frmDetalle.ShowDialog()")

            PrintLine(FileCls, TAB(16), "Case " & """" & "btnModificar" & """")
            PrintLine(FileCls, TAB(20), "If Not IsNumeric(Me.lblid_pk.Text) Then")
            PrintLine(FileCls, TAB(24), "Exit Sub")
            PrintLine(FileCls, TAB(20), "End If")

            '  PrintLine(FileCls, TAB(16), "If Me.lblid_pk.Text <= 1 Then")
            '  PrintLine(FileCls, TAB(20), "MessageBox.Show(" & """" & "No se Puede Modificar el Registro" & """" & ")")
            '  PrintLine(FileCls, TAB(20), "Exit Sub")
            '  PrintLine(FileCls, TAB(16), "End If")

            PrintLine(FileCls, TAB(20), "BanderaCuerpo_" & tabla & " = 2")
            PrintLine(FileCls, TAB(20), "Me.AddOwnedForm(frmDetalle)")
            PrintLine(FileCls, TAB(20), "oCuerpo_" & tabla & ".Modificar(CInt(Me.lblid_pk.Text))")
            PrintLine(FileCls, TAB(20), "frmDetalle.CargarCombos()")
            PrintLine(FileCls, TAB(20), "frmDetalle.ShowDialog()")
            PrintLine(FileCls, TAB(20), "RefrescarGrilla()")

            PrintLine(FileCls, TAB(16), "Case " & """" & "btnBorrar" & """")
            PrintLine(FileCls, TAB(20), "If Not IsNumeric(Me.lblid_pk.Text) Then")
            PrintLine(FileCls, TAB(24), "Exit Sub")
            PrintLine(FileCls, TAB(20), "End If")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(20), "If MessageBox.Show(" & """" & "Desea Realmente Eliminar el Registro " & """" & ", " & """" & "BORRAR" & """" & ", _")
            PrintLine(FileCls, TAB(20), "MessageBoxButtons.YesNo, MessageBoxIcon.Question) _")
            PrintLine(FileCls, TAB(20), "= Windows.Forms.DialogResult.No Then")
            PrintLine(FileCls, TAB(24), "Exit Sub")
            PrintLine(FileCls, TAB(20), "End If")
            PrintLine(FileCls, "")

            '   PrintLine(FileCls, TAB(16), "If Me.lblid_pk.Text <= 1 Then")
            '   PrintLine(FileCls, TAB(20), "MessageBox.Show(" & """" & "No se Puede Borrar el Registro" & """" & ")")
            '   PrintLine(FileCls, TAB(20), "Exit Sub")
            '   PrintLine(FileCls, TAB(16), "End If")

            PrintLine(FileCls, TAB(20), "oCuerpo_" & tabla & ".Borrar(CInt(Me.lblid_pk.Text))")
            PrintLine(FileCls, TAB(20), "RefrescarGrilla()")

            PrintLine(FileCls, TAB(16), "'Case " & """" & "btnAyuda" & """")
            PrintLine(FileCls, TAB(20), "'Process.Start(PathAyuda + " & """" & "frmAbm" & tabla & ".avi" & """" & ")")
            PrintLine(FileCls, TAB(16), "'Case " & """" & "btnSalir" & """")
            PrintLine(FileCls, TAB(20), "'Me.Close()")
            PrintLine(FileCls, TAB(12), "End Select")
            PrintLine(FileCls, TAB(12), "'Me.txtBuscar.Text = """"")
            PrintLine(FileCls, TAB(12), "'Me.txtBuscar.Focus()")
            PrintLine(FileCls, TAB(8), "Catch ex As Exception")
            PrintLine(FileCls, TAB(8), "MessageBox.Show(ex.Message)")
            PrintLine(FileCls, TAB(8), "End Try")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "End Class")

            FileClose(FileCls)





            '--------- formulario de detalle parte del diseño

            sContents = GetFileContents(path & "frmDetalleModelo2.resx", sErr)
            If sErr = "" Then
                Debug.WriteLine("File Contents: " & sContents)
                'Save to different file
                bAns = SaveTextToFile(sContents, path & "frmDetalle" & tabla & ".resx", sErr)
            End If


            Contador = 1


            nombreU = path & "frmDetalle" & tabla & ".Designer.vb"


            ' Defino variables
            FileCls = FreeFile()

            ' Abro un PathCls de texto (si el mismo existía se reempleza)
            FileOpen(FileCls, nombreU, OpenMode.Output)

            PrintLine(FileCls, TAB(0), "<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _")
            PrintLine(FileCls, TAB(0), "Partial Class frmDetalle" & tabla)
            PrintLine(FileCls, TAB(4), "Inherits System.Windows.Forms.Form")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "'Form reemplaza a Dispose para limpiar la lista de componentes.")
            PrintLine(FileCls, TAB(4), "<System.Diagnostics.DebuggerNonUserCode()> _")
            PrintLine(FileCls, TAB(4), "Protected Overrides Sub Dispose(ByVal disposing As Boolean)")
            PrintLine(FileCls, TAB(8), "If disposing AndAlso components IsNot Nothing Then")
            PrintLine(FileCls, TAB(12), "components.Dispose()")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(8), "MyBase.Dispose(disposing)")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "'Requerido por el Diseñador de Windows Forms")
            PrintLine(FileCls, TAB(4), "Private components As System.ComponentModel.IContainer")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento")
            PrintLine(FileCls, TAB(4), "'Se puede modificar usando el Diseñador de Windows Forms.")
            PrintLine(FileCls, TAB(4), "'No lo modifique con el editor de código.")
            PrintLine(FileCls, TAB(4), "<System.Diagnostics.DebuggerStepThrough()> _")
            PrintLine(FileCls, TAB(4), "Private Sub InitializeComponent()")
            PrintLine(FileCls, TAB(8), "Me.components = New System.ComponentModel.Container")

            PrintLine(FileCls, TAB(8), "Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetalle" & tabla & "))")
            PrintLine(FileCls, TAB(8), "Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle")
            PrintLine(FileCls, TAB(8), "Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle")
            PrintLine(FileCls, TAB(8), "Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle")

            PrintLine(FileCls, TAB(8), "Me.btnSalir = New System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar = New System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1 = New System.Windows.Forms.GroupBox")
            PrintLine(FileCls, TAB(8), "Me.ttGeneral = New System.Windows.Forms.ToolTip(Me.components)")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar = New System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(8), "Me.btnModificar = New System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar = New System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(8), "Me.dgv1 = New System.Windows.Forms.DataGridView")
            PrintLine(FileCls, TAB(8), "Me.lblid_" & tabla & " = New System.Windows.Forms.Label")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk = New System.Windows.Forms.Label")

            For Each reg In arrEstructura
                If reg.Orden = 1 Or reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & " = New System.Windows.Forms.Label")
                            PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & " = New System.Windows.Forms.Button")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & " = New System.Windows.Forms.ComboBox")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & " = New System.Windows.Forms.Label")
                        Case "String", "Decimal"
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & " = New System.Windows.Forms.TextBox")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & " = New System.Windows.Forms.Label")
                        Case "DateTime"
                            PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & " = New System.Windows.Forms.DateTimePicker")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & " = New System.Windows.Forms.Label")
                        Case "Boolean"
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & " = New System.Windows.Forms.CheckBox")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & " = New System.Windows.Forms.Label")
                        Case Else
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & " = New System.Windows.Forms.TextBox")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & " = New System.Windows.Forms.Label")
                    End Select
                End If
            Next
            PrintLine(FileCls, TAB(8), "Me.SuspendLayout()")

            Dim tabID As Integer = 500
            Dim tabBtn As Integer = 30
            Dim tabIndex As Integer = 4
            Dim posY As Integer = 0

            Dim col As Integer = 1

            For Each reg In arrEstructura
                If reg.Orden = 1 Or reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                Else

                    If posY = 0 Then
                        posY = 48
                    End If

                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'lbl" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".AutoSize = True")
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".BackColor = System.Drawing.Color.Red")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".Location = New System.Drawing.Point(370, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".Location = New System.Drawing.Point(925, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".Name = ""lbl" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".Size = New System.Drawing.Size(13, 13)")
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".TabIndex = " & tabID)
                            tabID = tabID + 1
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".Text = ""0""")
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".Visible = False")
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'btnBusca" & reg.nombre.Substring(3))
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Cursor = System.Windows.Forms.Cursors.Hand")
                            PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Image = CType(Resources.GetObject(""btnBuscarChico.Image""), System.Drawing.Image)")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Location = New System.Drawing.Point(505, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Location = New System.Drawing.Point(1060, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Name = ""btnBusca" & reg.nombre.Substring(3) & """")
                            PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Size = New System.Drawing.Size(30, 21)")
                            PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".TabIndex = " & tabBtn)
                            tabBtn = tabBtn + 1
                            PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".UseVisualStyleBackColor = True")
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'cmb" & reg.nombre.Substring(3))
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".FormattingEnabled = True")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".Location = New System.Drawing.Point(161, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".Location = New System.Drawing.Point(716, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".Name = ""cmb" & reg.nombre.Substring(3) & """")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".Size = New System.Drawing.Size(329, 21)")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".TabIndex = " & tabIndex)
                            tabIndex = tabIndex + 1
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'etiquieta" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".ForeColor = System.Drawing.Color.Black")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(28, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(583, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Size = New System.Drawing.Size(127, 21)")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TabIndex = " & tabID)
                            tabID = tabID + 1

                            Dim largo As Integer = reg.nombre.Length
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Text = """ & FormatoNombre(reg.nombre.Substring(3, largo - 3)) & """")

                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
                            PrintLine(FileCls, TAB(8), "'")
                        Case "String"
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'txt" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".BackColor = System.Drawing.Color.White")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Location = New System.Drawing.Point(161, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Location = New System.Drawing.Point(716, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".MaxLength = 50")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Name = ""txt" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Size = New System.Drawing.Size(385, 20)")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".TabIndex = " & tabIndex)
                            tabIndex = tabIndex + 1
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'etiquieta" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".ForeColor = System.Drawing.Color.Black")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(28, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(583, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Size = New System.Drawing.Size(127, 21)")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TabIndex = " & tabID)
                            tabID = tabID + 1
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Text = """ & FormatoNombre(reg.nombre) & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
                            PrintLine(FileCls, TAB(8), "'")
                        Case "Decimal"
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'txt" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".BackColor = System.Drawing.Color.White")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".ForeColor = System.Drawing.Color.Blue")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Location = New System.Drawing.Point(161, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Location = New System.Drawing.Point(716, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".MaxLength = 50")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Name = ""txt" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Size = New System.Drawing.Size(222, 31)")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".TabIndex = " & tabIndex)
                            tabIndex = tabIndex + 1
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".TextAlign = System.Windows.Forms.HorizontalAlignment.Right")
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'etiquieta" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".ForeColor = System.Drawing.Color.Black")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(28, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(583, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Size = New System.Drawing.Size(127, 21)")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TabIndex = " & tabID)
                            tabID = tabID + 1
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Text = """ & FormatoNombre(reg.nombre) & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
                            PrintLine(FileCls, TAB(8), "'")
                        Case "DateTime"
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'dtp" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".Format = System.Windows.Forms.DateTimePickerFormat.[Short]")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".Location = New System.Drawing.Point(161, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".Location = New System.Drawing.Point(716, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".Name = ""cmb" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".Size = New System.Drawing.Size(93, 20)")
                            PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".TabIndex = " & tabIndex)
                            tabIndex = tabIndex + 1
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'etiquieta" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".ForeColor = System.Drawing.Color.Black")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(28, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(583, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Size = New System.Drawing.Size(127, 21)")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TabIndex = " & tabID)
                            tabID = tabID + 1
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Text = """ & FormatoNombre(reg.nombre) & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
                            PrintLine(FileCls, TAB(8), "'")
                        Case "Boolean"
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'chk" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".AutoSize = True")
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Location = New System.Drawing.Point(161, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Location = New System.Drawing.Point(716, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Size = New System.Drawing.Size(15, 14)")
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".TabIndex = " & tabIndex)
                            tabIndex = tabIndex + 1
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Text = """ & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'etiquieta" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".ForeColor = System.Drawing.Color.Black")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(28, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(583, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Size = New System.Drawing.Size(127, 21)")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TabIndex = " & tabID)
                            tabID = tabID + 1
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Text = """ & FormatoNombre(reg.nombre) & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
                            PrintLine(FileCls, TAB(8), "'")
                        Case Else
                    End Select
                    'incremento la posicion
                    If reg.tiposql = "Decimal" Then
                        posY = posY + 53
                    Else
                        posY = posY + 43
                    End If

                    If posY > 568 Then
                        col = 2
                        posY = 0
                    End If

                End If
            Next
            'posY = posY + 43
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'btnSalir")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Font = New System.Drawing.Font(""Verdana"", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.ForeColor = System.Drawing.Color.Black")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Image = CType(Resources.GetObject(""btnSalir24.Image""), System.Drawing.Image)")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Location = New System.Drawing.Point(28, 577)")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Name = ""btnSalir""")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Size = New System.Drawing.Size(75, 49)")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.TabIndex = 61")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Text = ""&Salir""")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.UseVisualStyleBackColor = True")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'btnGuardar")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.Font = New System.Drawing.Font(""Verdana"", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.ForeColor = System.Drawing.Color.Black")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.Image = CType(Resources.GetObject(""btnAceptar24.Image""), System.Drawing.Image)")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.Location = New System.Drawing.Point(28, 512)")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.Name = ""btnGuardar""")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.Size = New System.Drawing.Size(75, 49)")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.TabIndex = 60")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.Text = ""&Guardar""")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.UseVisualStyleBackColor = True")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'btnBorrar")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.Cursor = System.Windows.Forms.Cursors.Hand")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.Font = New System.Drawing.Font(""Verdana"", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.Image = CType(resources.GetObject(""btnEliminar24.Image""), System.Drawing.Image)")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.Location = New System.Drawing.Point(28, 437)")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.Name = ""btnBorrar""")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.Size = New System.Drawing.Size(75, 49)")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.TabIndex = 52")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.Text = ""&Borrar""")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
            PrintLine(FileCls, TAB(8), "Me.btnBorrar.UseVisualStyleBackColor = True")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'btnModificar")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.Cursor = System.Windows.Forms.Cursors.Hand")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.Font = New System.Drawing.Font(""Verdana"", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.Image = CType(resources.GetObject(""btnModificar24.Image""), System.Drawing.Image)")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.Location = New System.Drawing.Point(28, 366)")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.Name = ""btnModificar""")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.Size = New System.Drawing.Size(75, 49)")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.TabIndex = 51")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.Text = ""&Modificar""")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
            PrintLine(FileCls, TAB(8), "Me.btnModificar.UseVisualStyleBackColor = True")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'btnAgregar")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.Cursor = System.Windows.Forms.Cursors.Hand")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.Font = New System.Drawing.Font(""Verdana"", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.Image = CType(resources.GetObject(""btnNuevo24.Image""), System.Drawing.Image)")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.Location = New System.Drawing.Point(28, 296)")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.Name = ""btnAgregar""")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.Size = New System.Drawing.Size(75, 49)")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.TabIndex = 50")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.Text = ""&Agregar""")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
            PrintLine(FileCls, TAB(8), "Me.btnAgregar.UseVisualStyleBackColor = True")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'dgv1")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.dgv1.AllowUserToAddRows = False")
            PrintLine(FileCls, TAB(8), "Me.dgv1.AllowUserToResizeColumns = False")
            PrintLine(FileCls, TAB(8), "Me.dgv1.AllowUserToResizeRows = False")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle1.BackColor = System.Drawing.Color.LightBlue")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightGoldenrodYellow")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Brown")
            PrintLine(FileCls, TAB(8), "Me.dgv1.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1")
            PrintLine(FileCls, TAB(8), "Me.dgv1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells")
            PrintLine(FileCls, TAB(8), "Me.dgv1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells")
            PrintLine(FileCls, TAB(8), "Me.dgv1.BackgroundColor = System.Drawing.Color.SkyBlue")
            'PrintLine(FileCls, TAB(8), "Me.dgv1.BorderStyle = System.Windows.Forms.BorderStyle.None")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle2.BackColor = System.Drawing.Color.Gold")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle2.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]")
            PrintLine(FileCls, TAB(8), "Me.dgv1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2")
            PrintLine(FileCls, TAB(8), "Me.dgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle3.BackColor = System.Drawing.Color.Lavender")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle3.Font = New System.Drawing.Font(""Microsoft Sans Serif"", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Beige")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Brown")
            PrintLine(FileCls, TAB(8), "DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]")
            PrintLine(FileCls, TAB(8), "Me.dgv1.DefaultCellStyle = DataGridViewCellStyle3")
            PrintLine(FileCls, TAB(8), "Me.dgv1.GridColor = System.Drawing.Color.MediumPurple")
            PrintLine(FileCls, TAB(8), "Me.dgv1.Location = New System.Drawing.Point(135, 296)")
            PrintLine(FileCls, TAB(8), "Me.dgv1.Name = ""dgv1""")
            PrintLine(FileCls, TAB(8), "Me.dgv1.ReadOnly = True")
            PrintLine(FileCls, TAB(8), "Me.dgv1.ShowCellErrors = False")
            PrintLine(FileCls, TAB(8), "Me.dgv1.ShowRowErrors = False")
            PrintLine(FileCls, TAB(8), "Me.dgv1.Size = New System.Drawing.Size(845, 330)")
            PrintLine(FileCls, TAB(8), "Me.dgv1.StandardTab = True")
            PrintLine(FileCls, TAB(8), "Me.dgv1.TabIndex = 500")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'lblid_" & tabla)
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.lblid_" & tabla & ".AutoSize = True")
            PrintLine(FileCls, TAB(8), "Me.lblid_" & tabla & ".BackColor = System.Drawing.Color.Red")
            PrintLine(FileCls, TAB(8), "Me.lblid_" & tabla & ".Location = New System.Drawing.Point(182, 5)")
            PrintLine(FileCls, TAB(8), "Me.lblid_" & tabla & ".Name = ""lblid_" & tabla & """")
            PrintLine(FileCls, TAB(8), "Me.lblid_" & tabla & ".Size = New System.Drawing.Size(13, 13)")
            PrintLine(FileCls, TAB(8), "Me.lblid_" & tabla & ".TabIndex = 565")
            PrintLine(FileCls, TAB(8), "Me.lblid_" & tabla & ".Text = ""0""")
            PrintLine(FileCls, TAB(8), "Me.lblid_" & tabla & ".Visible = False")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'lblid_pk")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk.AutoSize = True")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk.BackColor = System.Drawing.Color.Red")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk.Location = New System.Drawing.Point(158, 309)")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk.Name = ""lblid_pk""")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk.Size = New System.Drawing.Size(13, 13)")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk.TabIndex = 565")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk.Text = ""0""")
            PrintLine(FileCls, TAB(8), "Me.lblid_pk.Visible = False")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'GroupBox1")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.Location = New System.Drawing.Point(12, 17)")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.Name = ""GroupBox1""")
            If col = 1 Then
                PrintLine(FileCls, TAB(8), "Me.GroupBox1.Size = New System.Drawing.Size(568, " & posY & ")")
            Else
                PrintLine(FileCls, TAB(8), "Me.GroupBox1.Size = New System.Drawing.Size(1000, 606)")
            End If
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.TabIndex = 0")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.TabStop = False")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'frmDetalle" & tabla)
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)")
            PrintLine(FileCls, TAB(8), "Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font")
            If col = 1 Then
                PrintLine(FileCls, TAB(8), "Me.ClientSize = New System.Drawing.Size(600, " & posY + 120 & ")")
            Else
                PrintLine(FileCls, TAB(8), "Me.ClientSize = New System.Drawing.Size(1024, 768)")
            End If
            'control.add
            For Each reg In arrEstructura
                If reg.Orden = 1 Or reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.lbl" & reg.nombre & ")")
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.btnBusca" & reg.nombre.Substring(3) & ")")
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.cmb" & reg.nombre.Substring(3) & ")")
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.etiqueta" & reg.nombre & ")")
                        Case "String", "Decimal"
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.txt" & reg.nombre & ")")
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.etiqueta" & reg.nombre & ")")
                        Case "DateTime"
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.dtp" & reg.nombre & ")")
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.etiqueta" & reg.nombre & ")")
                        Case "Boolean"
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.chk" & reg.nombre & ")")
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.etiqueta" & reg.nombre & ")")
                        Case Else
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me." & reg.nombre & ")")
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.etiqueta" & reg.nombre & ")")
                    End Select
                End If
            Next
            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.btnSalir)")
            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.btnGuardar)")
            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.GroupBox1)")

            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.btnAgregar)")
            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.btnModificar)")
            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.btnBorrar)")
            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.dgv1)")
            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.lblid_pk)")
            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.lblid_" & tabla & ")")

            PrintLine(FileCls, TAB(8), "Me.Name = ""frmDetalle" & tabla & """")
            PrintLine(FileCls, TAB(8), "Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen")
            PrintLine(FileCls, TAB(8), "Me.Text = ""frmDetalle" & tabla & """")
            PrintLine(FileCls, TAB(8), "Me.ResumeLayout(False)")
            PrintLine(FileCls, TAB(8), "Me.PerformLayout()")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "End Sub")

            'withevents
            For Each reg In arrEstructura
                If reg.Orden = 1 Or reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            PrintLine(FileCls, TAB(4), "Friend WithEvents lbl" & reg.nombre & " As System.Windows.Forms.Label")
                            PrintLine(FileCls, TAB(4), "Friend WithEvents btnBusca" & reg.nombre.Substring(3) & " As System.Windows.Forms.Button")
                            PrintLine(FileCls, TAB(4), "Friend WithEvents cmb" & reg.nombre.Substring(3) & " As System.Windows.Forms.ComboBox")
                            PrintLine(FileCls, TAB(4), "Friend WithEvents etiqueta" & reg.nombre & " As System.Windows.Forms.Label")
                        Case "String", "Decimal"
                            PrintLine(FileCls, TAB(4), "Protected WithEvents txt" & reg.nombre & " As System.Windows.Forms.TextBox")
                            PrintLine(FileCls, TAB(4), "Friend WithEvents etiqueta" & reg.nombre & " As System.Windows.Forms.Label")
                        Case "DateTime"
                            PrintLine(FileCls, TAB(4), "Protected WithEvents dtp" & reg.nombre & " As System.Windows.Forms.DateTimePicker")
                            PrintLine(FileCls, TAB(4), "Friend WithEvents etiqueta" & reg.nombre & " As System.Windows.Forms.Label")
                        Case "Boolean"
                            PrintLine(FileCls, TAB(4), "Protected WithEvents chk" & reg.nombre & " As System.Windows.Forms.CheckBox")
                            PrintLine(FileCls, TAB(4), "Friend WithEvents etiqueta" & reg.nombre & " As System.Windows.Forms.Label")
                        Case Else
                            PrintLine(FileCls, TAB(4), "Protected WithEvents txt" & reg.nombre & " As System.Windows.Forms.TextBox")
                            PrintLine(FileCls, TAB(4), "Friend WithEvents etiqueta" & reg.nombre & " As System.Windows.Forms.Label")
                    End Select
                End If
            Next

            PrintLine(FileCls, TAB(4), "Protected WithEvents btnSalir As System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(4), "Protected WithEvents btnGuardar As System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(4), "Protected WithEvents GroupBox1 As System.Windows.Forms.GroupBox")
            PrintLine(FileCls, TAB(4), "Protected WithEvents ttGeneral As System.Windows.Forms.ToolTip")
            PrintLine(FileCls, TAB(4), "Protected WithEvents btnBorrar As System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(4), "Protected WithEvents btnModificar As System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(4), "Protected WithEvents btnAgregar As System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(4), "Protected WithEvents dgv1 As System.Windows.Forms.DataGridView")
            PrintLine(FileCls, TAB(4), "Friend WithEvents lblid_pk As System.Windows.Forms.Label")
            PrintLine(FileCls, TAB(4), "Friend WithEvents lblid_" & tabla & " As System.Windows.Forms.Label")
            PrintLine(FileCls, TAB(0), "End Class")
            PrintLine(FileCls, "")


            FileClose(FileCls)
        End If




        '**************************************************
        '
        'DEFINICION DEL CODIGA PARA FORMULARIOS DE DETALLE DE LA FACTURA
        '
        '**************************************************
        If detalle = True Then



            Dim Contador As Integer = 1

            Dim tablaVieja As String = tabla
            'seteo el nombre de la tabla con el detalle del cuerpo
            'tabla = "Cuerpo_" & tabla


            Dim tablaMaestra As String = ""
            tablaMaestra = tabla.Substring(7)

            nombreU = path & "frmDetalle" & tabla & ".vb"

            ' Defino variables
            FileCls = FreeFile()

            ' Abro un PathCls de texto (si el mismo existía se reempleza)
            FileOpen(FileCls, nombreU, OpenMode.Output)


            'saca el nombre del primer control para el focus
            Contador = 1
            Dim strFocus As String = ""
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)
                            strFocus = "Me.cmb" & Nombre
                            Exit For
                        Case "String", "Decimal"
                            strFocus = "Me.txt" & reg.nombre
                            Exit For
                        Case "DateTime"
                            strFocus = "Me.dtp" & reg.nombre
                            Exit For
                        Case "Boolean"
                            strFocus = "Me.chk" & reg.nombre
                            Exit For
                    End Select
                End If
                Contador = Contador + 1
            Next

            'saca el nombre del primer txt para los vacios
            Contador = 1
            Dim strVacio As String = ""
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                Else
                    Select Case reg.tiposql
                        Case "String", "Decimal"
                            strVacio = "Me.txt" & reg.nombre
                            Exit For
                    End Select
                End If
                Contador = Contador + 1
            Next

            'Private Sub frmDetalle_Load
            PrintLine(FileCls, TAB(0), "Public Class frmDetalle" & tabla)
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "Private Sub frmDetalle" & tabla & "_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Or reg.nombre = "id_" & tablaMaestra Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)
                            PrintLine(FileCls, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnBusca" & Nombre & ", " & """" & "Buscar Nuevo " & Nombre & """" & ")")
                    End Select
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnGuardar, " & """" & "Guardar Datos del " & tabla & """" & ")")
            PrintLine(FileCls, TAB(8), "Me.ttGeneral.SetToolTip(Me.btnSalir, " & """" & "Volver a la Pantalla Anterior" & """" & ")")
            PrintLine(FileCls, TAB(8), "Try")
            PrintLine(FileCls, TAB(12), "ObtenerDatos()")
            PrintLine(FileCls, "")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Or reg.nombre = "id_" & tablaMaestra Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)
                            ' PrintLine(FileCls, TAB(8), "o" & Nombre & ".Modificar(Me.lblid_" & Nombre & ".Text)")
                            ' PrintLine(FileCls, TAB(8), "Me.cmb" & Nombre & ".Text = o" & Nombre & ".nombre_" & Nombre)
                            PrintLine(FileCls, TAB(12), "Me.cmb" & Nombre & ".Text = o" & Nombre & ".GetOne(CInt(Me.lblid_" & Nombre & ".Text)).Rows(0).Item(" & """" & "nombre_" & Nombre & """" & ").ToString.Trim")
                    End Select
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(8), "Catch ex As Exception")
            PrintLine(FileCls, TAB(12), "'    MessageBox.Show(ex.Message)")
            PrintLine(FileCls, TAB(8), "End Try")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "Me.lblid_" & tablaMaestra & ".Text = CType(Me.Owner, frmDetalle" & tablaMaestra & ").lblid_" & tablaMaestra & ".Text")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "If Bandera" & tabla & "  = 1 Then")
            PrintLine(FileCls, TAB(12), "Me.LimpiarControles()")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(8), "Me.Text = " & """" & tabla & """")
            PrintLine(FileCls, TAB(8), "Me.CancelButton = Me.btnSalir")
            PrintLine(FileCls, TAB(8), "Me.BackColor = Color.Gainsboro")
            PrintLine(FileCls, TAB(8), "Me.StartPosition = FormStartPosition.CenterScreen")
            PrintLine(FileCls, TAB(8), "Me.MinimizeBox = False")
            PrintLine(FileCls, TAB(8), "Me.MaximizeBox = False")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Sub CargarCombos()

            PrintLine(FileCls, TAB(4), "Sub CargarCombos()")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Or reg.nombre = "id_" & tablaMaestra Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)

                            PrintLine(FileCls, TAB(8), "Me.Cargar" & Nombre)
                    End Select
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Or reg.nombre = "id_" & tablaMaestra Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)

                            PrintLine(FileCls, TAB(4), "Sub Cargar" & Nombre & "()")
                            PrintLine(FileCls, TAB(8), "Dim odt As New DataTable")
                            PrintLine(FileCls, "")
                            PrintLine(FileCls, TAB(8), "odt = o" & Nombre & ".GetCmb")
                            PrintLine(FileCls, TAB(8), "With Me.cmb" & Nombre)
                            PrintLine(FileCls, TAB(12), ".DataSource = odt")
                            PrintLine(FileCls, TAB(12), ".DisplayMember = " & """" & "nombre_" & Nombre & """")
                            PrintLine(FileCls, TAB(12), ".ValueMember = " & """" & "id_" & Nombre & """")
                            PrintLine(FileCls, TAB(8), "End With")
                            PrintLine(FileCls, TAB(8), "If Me.cmb" & Nombre & ".SelectedIndex >= 0 Then")
                            PrintLine(FileCls, TAB(12), "cmb" & Nombre & ".SelectedIndex = 0")
                            PrintLine(FileCls, TAB(12), "Me.lblid_" & Nombre & ".Text = cmb" & Nombre & ".SelectedValue.ToString")
                            PrintLine(FileCls, TAB(8), "End If")
                            PrintLine(FileCls, TAB(4), "End Sub")
                            PrintLine(FileCls, "")
                    End Select
                End If
                Contador = Contador + 1
            Next

            'Sub LimpiarControles()

            PrintLine(FileCls, TAB(4), "Sub LimpiarControles()")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                ElseIf reg.nombre = "id_" & tablaMaestra Then
                    Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                    PrintLine(FileCls, TAB(8), "'Me.lbl" & reg.nombre & ".Text = " & """" & "0" & """")
                    PrintLine(FileCls, TAB(8), "'Me.cmb" & Nombre & ".Text =  """"")
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".Text = " & """" & "0" & """")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & Nombre & ".Text =  """"")
                        Case "String", "Decimal"
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Text = """"")
                        Case "DateTime"
                            PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".Value = DateTime.Now")
                        Case "Boolean"
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Checked = False")
                        Case Else
                            PrintLine(FileCls, TAB(8), "Me." & reg.nombre & ".Text = """"")
                    End Select

                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub ObtenerDatos()

            PrintLine(FileCls, TAB(4), "Private Sub ObtenerDatos()")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                ElseIf reg.nombre = "id_" & tablaMaestra Then
                    PrintLine(FileCls, TAB(8), "'Me.lbl" & reg.nombre & ".Text = o" & tabla & "." & reg.nombre & ".ToString")
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".Text = o" & tabla & "." & reg.nombre & ".ToString")
                        Case "String"
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Text = o" & tabla & "." & reg.nombre & ".Trim")
                        Case "Decimal"
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Text = o" & tabla & "." & reg.nombre & ".ToString.Trim")
                        Case "DateTime"
                            PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".Value = o" & tabla & "." & reg.nombre)
                        Case "Boolean"
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Checked = o" & tabla & "." & reg.nombre)
                        Case Else
                            PrintLine(FileCls, TAB(8), "Me." & reg.nombre & ".Text = Trim(o" & tabla & "." & reg.nombre & ")")
                    End Select

                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub AsignarDatos()

            PrintLine(FileCls, TAB(4), "Private Sub AsignarDatos()")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "operacion" Or reg.nombre = "sincronizado" Then
                    'next row
                ElseIf reg.nombre = "id_cliente_maestro" Then
                    PrintLine(FileCls, TAB(8), "o" & tabla & "." & reg.nombre & " = id_cliente_maestro")
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            PrintLine(FileCls, TAB(8), "o" & tabla & "." & reg.nombre & " = CInt(Me.lbl" & reg.nombre & ".Text)")
                        Case "String"
                            PrintLine(FileCls, TAB(8), "o" & tabla & "." & reg.nombre & " = Me.txt" & reg.nombre & ".Text")
                        Case "Decimal"
                            PrintLine(FileCls, TAB(8), "o" & tabla & "." & reg.nombre & " = CDec(Me.txt" & reg.nombre & ".Text)")
                        Case "DateTime"
                            PrintLine(FileCls, TAB(8), "o" & tabla & "." & reg.nombre & " = Me.dtp" & reg.nombre & ".Value.Date")
                        Case "Boolean"
                            PrintLine(FileCls, TAB(8), "o" & tabla & "." & reg.nombre & " = Me.chk" & reg.nombre & ".Checked")
                        Case Else
                            PrintLine(FileCls, TAB(8), "o" & tabla & "." & reg.nombre & " = Me." & reg.nombre & ".Text")
                    End Select

                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Public Sub SoloLectura()

            PrintLine(FileCls, TAB(4), "Public Sub SoloLectura()")
            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                ElseIf reg.nombre = "id_" & tablaMaestra Then
                    Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                    Nombre = Mid$(Nombre, 4)
                    '  PrintLine(FileCls, TAB(4), "Me.lbl" & reg.nombre & ".Text = " & """" & "0" & """")
                    PrintLine(FileCls, TAB(8), "'Me.cmb" & Nombre & ".Enabled = False")
                    PrintLine(FileCls, TAB(8), "'Me.btnBusca" & Nombre & ".Enabled = False")
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)
                            '  PrintLine(FileCls, TAB(4), "Me.lbl" & reg.nombre & ".Text = " & """" & "0" & """")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & Nombre & ".Enabled = False")
                            PrintLine(FileCls, TAB(8), "Me.btnBusca" & Nombre & ".Enabled = False")
                        Case "String", "Decimal"
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Enabled = False")
                        Case "DateTime"
                            PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".Enabled = False")
                        Case "Boolean"
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Enabled = False")
                        Case Else
                            PrintLine(FileCls, TAB(8), "Me." & reg.nombre & ".Enabled = False")
                    End Select
                End If
                Contador = Contador + 1
            Next
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.Visible = False")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub Guardar()

            PrintLine(FileCls, TAB(4), "Private Sub Guardar()")
            PrintLine(FileCls, TAB(8), "Try")
            PrintLine(FileCls, TAB(12), "Me.AsignarDatos()")

            PrintLine(FileCls, TAB(12), "If o" & tabla & ".Exist() Then")
            PrintLine(FileCls, TAB(16), "If Bandera" & tabla & " = 1 Then")
            PrintLine(FileCls, TAB(20), "MessageBox.Show(" & """" & "Sr. Usuario: Los Datos que Pretende Ingresar ya Fueron Cargados en el Sistema" & """" & ")")
            PrintLine(FileCls, TAB(16), "Exit Sub")
            PrintLine(FileCls, TAB(16), "ElseIf Bandera" & tabla & " = 2 Then")
            PrintLine(FileCls, TAB(20), "If MessageBox.Show(" & """" & "Sr. Usuario: Los Datos que Desea Modificar ya Existen ¿Desea Reemplazarlos?" & """" & ", " & """" & "MODIFICAR" & """" & ", _")
            PrintLine(FileCls, TAB(24), "MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) _")
            PrintLine(FileCls, TAB(24), "= Windows.Forms.DialogResult.No Then")
            PrintLine(FileCls, TAB(24), "Exit Sub")
            PrintLine(FileCls, TAB(20), "End If")
            PrintLine(FileCls, TAB(16), "End If")
            PrintLine(FileCls, TAB(12), "End If")

            PrintLine(FileCls, TAB(12), "Select Case Bandera" & tabla)
            PrintLine(FileCls, TAB(16), "Case 1 'GUARDA,REFRESCA LA GRILLA E INSERTA UNO NUEVO (AGREGAR)")
            PrintLine(FileCls, TAB(20), "o" & tabla & ".Guardar()")
            PrintLine(FileCls, TAB(20), "CType(Me.Owner, frmDetalle" & tablaMaestra & ").RefrescarGrilla()")
            PrintLine(FileCls, TAB(20), "Me.CargarCombos()")
            PrintLine(FileCls, TAB(20), "o" & tabla & ".Insertar()")
            PrintLine(FileCls, TAB(20), "Me.ObtenerDatos()")
            PrintLine(FileCls, TAB(20), "Me.LimpiarControles()")
            PrintLine(FileCls, TAB(20), strFocus & ".Focus")
            PrintLine(FileCls, TAB(16), "Case 2 'GUARDA Y SALE (MODIFICAR)")
            PrintLine(FileCls, TAB(20), "o" & tabla & ".Guardar()")
            PrintLine(FileCls, TAB(20), "Me.Close()")
            PrintLine(FileCls, TAB(12), "End Select")
            PrintLine(FileCls, TAB(8), "Catch ex As Exception")
            PrintLine(FileCls, TAB(12), "MessageBox.Show(ex.Message)")
            PrintLine(FileCls, TAB(8), "End Try")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Function ChequearVacios()

            PrintLine(FileCls, TAB(4), "Private Function ChequearVacios() As Boolean")
            PrintLine(FileCls, TAB(8), "Dim bandera As Boolean")
            PrintLine(FileCls, TAB(8), "If " & strVacio & ".Text = """"" & " Then")
            PrintLine(FileCls, TAB(12), "bandera = False")
            PrintLine(FileCls, TAB(8), "Else")
            PrintLine(FileCls, TAB(12), "bandera = True")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(8), "Return bandera")
            PrintLine(FileCls, TAB(4), "End Function")
            PrintLine(FileCls, "")

            'Private Sub btnAyuda_Click ANULADO

            '    PrintLine(FileCls, TAB(0), "Private Sub btnAyuda_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAyuda.Click")
            '    PrintLine(FileCls, TAB(4), "'      System.Diagnostics.Process.Start(PathAyuda + " & """" & "FrmDetalle" & tabla & """" & ".htm)")
            '    PrintLine(FileCls, TAB(0), "End Sub")
            '    PrintLine(FileCls, "")

            'Private Sub btnGuardar_Click

            PrintLine(FileCls, TAB(4), "Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click")
            PrintLine(FileCls, TAB(8), "Dim blnVacios As Boolean")
            PrintLine(FileCls, "")


            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                ElseIf reg.nombre = "id_" & tablaMaestra Then
                    PrintLine(FileCls, TAB(8), "'If CDbl(Me.lbl" & reg.nombre & ".Text) = 0 Then")
                    'si no tiene 'id_' salta el error
                    Try
                        PrintLine(FileCls, TAB(12), "'MessageBox.Show(" & """" & "Debe Seleccionar un Dato del Combo de " & reg.nombre.Substring(3) & """" & ")")
                    Catch ex As Exception
                    End Try
                    PrintLine(FileCls, TAB(12), "'Exit Sub")
                    PrintLine(FileCls, TAB(8), "'End If")
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            PrintLine(FileCls, TAB(8), "If CDbl(Me.lbl" & reg.nombre & ".Text) = 0 Then")
                            'si no tiene 'id_' salta el error
                            Try
                                PrintLine(FileCls, TAB(12), "MessageBox.Show(" & """" & "Debe Seleccionar un Dato del Combo de " & reg.nombre.Substring(3) & """" & ")")
                            Catch ex As Exception
                            End Try
                            PrintLine(FileCls, TAB(12), "Exit Sub")
                            PrintLine(FileCls, TAB(8), "End If")
                    End Select
                End If
                Contador = Contador + 1
            Next

            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "blnVacios = Me.ChequearVacios")
            PrintLine(FileCls, TAB(8), "If blnVacios = False Then")
            PrintLine(FileCls, TAB(12), "MessageBox.Show(" & """" & "Debe Llenar los Campos Obligatorios" & """" & ")")
            PrintLine(FileCls, TAB(12), "Exit Sub")
            PrintLine(FileCls, TAB(8), "Else")
            PrintLine(FileCls, TAB(12), "If Bandera" & tabla & "  = 1 Then")
            PrintLine(FileCls, TAB(16), "Me.Guardar()")
            PrintLine(FileCls, TAB(12), "Else")
            PrintLine(FileCls, TAB(16), "Me.Guardar()")
            PrintLine(FileCls, TAB(12), "End If")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub btnSalir_Click

            PrintLine(FileCls, TAB(4), "Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click")
            PrintLine(FileCls, TAB(8), "Me.Close()")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub cmb

            Contador = 1
            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                ElseIf reg.nombre = "id_" & tablaMaestra Then

                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)

                            PrintLine(FileCls, TAB(4), "Private Sub cmb" & Nombre & "_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmb" & Nombre & ".SelectedIndexChanged")
                            PrintLine(FileCls, TAB(8), "If Me.cmb" & Nombre & ".SelectedIndex >= 0 Then")
                            PrintLine(FileCls, TAB(12), "Me.lbl" & reg.nombre & ".Text = cmb" & Nombre & ".SelectedValue.ToString")
                            PrintLine(FileCls, TAB(8), "End If")
                            PrintLine(FileCls, TAB(4), "End Sub")
                            PrintLine(FileCls, "")

                            PrintLine(FileCls, TAB(4), "Private Sub btnBusca" & Nombre & "_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBusca" & Nombre & ".Click")
                            PrintLine(FileCls, TAB(8), "Dim frmTemporal As New FrmAbm" & Nombre)
                            PrintLine(FileCls, TAB(8), "frmTemporal.ShowDialog()")
                            PrintLine(FileCls, TAB(8), "Me.Cargar" & Nombre & "()")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & Nombre & ".Focus()")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & Nombre & ".Text = """"")
                            PrintLine(FileCls, TAB(8), "Me.lblid_" & Nombre & ".Text = " & """" & "0" & """")
                            PrintLine(FileCls, TAB(4), "End Sub")
                            PrintLine(FileCls, "")
                    End Select
                End If
                Contador = Contador + 1
            Next

            'arma las cadenas de tabulacion
            Contador = 1
            Dim TabPress As String = ""
            Dim TabDown As String = ""
            Dim TabDecimal As String = ""

            For Each reg In arrEstructura
                If reg.Orden = 1 Then
                    'SALTA LA PK
                ElseIf reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                ElseIf reg.nombre = "id_" & tablaMaestra Then
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            Dim Nombre As String = Mid$(reg.nombre, 1, 4).ToUpper & Mid$(reg.nombre, 5).ToLower
                            Nombre = Mid$(Nombre, 4)
                            '  PrintLine(FileCls, TAB(4), "Me.lbl" & reg.nombre & ".Text = " & """" & "0" & """")
                            TabDown = TabDown & "cmb" & Nombre & ".KeyDown, "
                        Case "String"
                            TabPress = TabPress & "txt" & reg.nombre & ".KeyPress, "
                        Case "Decimal"
                            TabDecimal = TabDecimal & "txt" & reg.nombre & ".KeyPress, "
                        Case "DateTime"
                            TabDown = TabDown & "dtp" & reg.nombre & ".KeyDown, "
                        Case "Boolean"
                            TabPress = TabPress & "chk" & reg.nombre & ".KeyPress, "
                    End Select
                End If
                Contador = Contador + 1
            Next
            Dim LargoTab As Integer = TabPress.Length
            If LargoTab >= 2 Then
                TabPress = TabPress.Substring(0, LargoTab - 2)
            End If
            LargoTab = TabDown.Length
            If LargoTab >= 2 Then
                TabDown = TabDown.Substring(0, LargoTab - 2)
            End If
            LargoTab = TabDecimal.Length
            If LargoTab >= 2 Then
                TabDecimal = TabDecimal.Substring(0, LargoTab - 2)
            End If

            'Private Sub TabulacionTextBox

            PrintLine(FileCls, TAB(4), "Private Sub TabulacionTextBox(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _") 'Handles " & TabPress)
            If TabPress = "" Then
                PrintLine(FileCls, TAB(4), "'Handles " & TabPress)
            Else
                PrintLine(FileCls, TAB(4), "Handles " & TabPress)
            End If
            PrintLine(FileCls, TAB(8), "If e.KeyChar.ToString = vbCr Then")
            PrintLine(FileCls, TAB(12), "Me.SelectNextControl(Me.ActiveControl, True, True, True, True)")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private Sub TabulacionCombos

            PrintLine(FileCls, TAB(4), "Private Sub TabulacionCombos(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) _") 'Handles " & TabDown)
            If TabDown = "" Then
                PrintLine(FileCls, TAB(4), "'Handles " & TabDown)
            Else
                PrintLine(FileCls, TAB(4), "Handles " & TabDown)
            End If
            PrintLine(FileCls, TAB(8), "If CDbl(e.KeyValue.ToString) = 13 Then")
            PrintLine(FileCls, TAB(12), "Me.SelectNextControl(Me.ActiveControl, True, True, True, True)")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")

            'Private sub tabulacion decimales
            PrintLine(FileCls, TAB(4), "Private Sub Decimales(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _")
            If TabDecimal = "" Then
                PrintLine(FileCls, TAB(4), "'Handles " & TabDecimal)
            Else
                PrintLine(FileCls, TAB(4), "Handles " & TabDecimal)
            End If
            PrintLine(FileCls, TAB(8), "Dim txtTemp As TextBox")
            PrintLine(FileCls, TAB(8), "txtTemp = CType(sender, TextBox)")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "If e.KeyChar.ToString = vbCr Then")
            PrintLine(FileCls, TAB(12), "Me.SelectNextControl(Me.ActiveControl, True, True, True, True)")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(8), "If e.KeyChar.ToString = " & """" & "." & """" & " Or e.KeyChar.ToString = " & """" & "," & """" & " Then")
            PrintLine(FileCls, TAB(12), "If InStr(txtTemp.Text, " & """" & "," & """" & ") <> 0 Then")
            PrintLine(FileCls, TAB(16), "e.Handled = True")
            PrintLine(FileCls, TAB(12), "Else")
            PrintLine(FileCls, TAB(16), "e.KeyChar = CChar(" & """" & "," & """" & ")")
            PrintLine(FileCls, TAB(12), "End If")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "Dim Largo As Integer = InStr(txtTemp.Text, " & """" & "," & """" & ")")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "If txtTemp.Text.Length > Largo + 2 And Largo <> 0 And e.KeyChar.ToString <> vbBack Then")
            PrintLine(FileCls, TAB(12), "e.Handled = True")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(8), "If (e.KeyChar.ToString >= " & """" & "0" & """" & " And e.KeyChar.ToString <= " & """" & "9" & """" & ") Or e.KeyChar.ToString = " & """" & "," & """" & " Or e.KeyChar = vbBack Then")
            PrintLine(FileCls, TAB(12), "'  e.Handled = False")
            PrintLine(FileCls, TAB(8), "Else")
            PrintLine(FileCls, TAB(12), "e.Handled = True")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(0), "End Class")

            FileClose(FileCls)





            '--------- formulario de detalle parte del diseño

            sContents = GetFileContents(path & "frmDetalleModelo2.resx", sErr)
            If sErr = "" Then
                Debug.WriteLine("File Contents: " & sContents)
                'Save to different file
                bAns = SaveTextToFile(sContents, path & "frmDetalle" & tabla & ".resx", sErr)
            End If


            Contador = 1


            nombreU = path & "frmDetalle" & tabla & ".Designer.vb"


            ' Defino variables
            FileCls = FreeFile()

            ' Abro un PathCls de texto (si el mismo existía se reempleza)
            FileOpen(FileCls, nombreU, OpenMode.Output)

            PrintLine(FileCls, TAB(0), "<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _")
            PrintLine(FileCls, TAB(0), "Partial Class frmDetalle" & tabla)
            PrintLine(FileCls, TAB(4), "Inherits System.Windows.Forms.Form")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "'Form reemplaza a Dispose para limpiar la lista de componentes.")
            PrintLine(FileCls, TAB(4), "<System.Diagnostics.DebuggerNonUserCode()> _")
            PrintLine(FileCls, TAB(4), "Protected Overrides Sub Dispose(ByVal disposing As Boolean)")
            PrintLine(FileCls, TAB(8), "If disposing AndAlso components IsNot Nothing Then")
            PrintLine(FileCls, TAB(12), "components.Dispose()")
            PrintLine(FileCls, TAB(8), "End If")
            PrintLine(FileCls, TAB(8), "MyBase.Dispose(disposing)")
            PrintLine(FileCls, TAB(4), "End Sub")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "'Requerido por el Diseñador de Windows Forms")
            PrintLine(FileCls, TAB(4), "Private components As System.ComponentModel.IContainer")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento")
            PrintLine(FileCls, TAB(4), "'Se puede modificar usando el Diseñador de Windows Forms.")
            PrintLine(FileCls, TAB(4), "'No lo modifique con el editor de código.")
            PrintLine(FileCls, TAB(4), "<System.Diagnostics.DebuggerStepThrough()> _")
            PrintLine(FileCls, TAB(4), "Private Sub InitializeComponent()")
            PrintLine(FileCls, TAB(8), "Me.components = New System.ComponentModel.Container")
            PrintLine(FileCls, TAB(8), "Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetalle" & tabla & "))")

            PrintLine(FileCls, TAB(8), "Me.btnSalir = New System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar = New System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1 = New System.Windows.Forms.GroupBox")
            PrintLine(FileCls, TAB(8), "Me.ttGeneral = New System.Windows.Forms.ToolTip(Me.components)")

            For Each reg In arrEstructura
                If reg.Orden = 1 Or reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & " = New System.Windows.Forms.Label")
                            PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & " = New System.Windows.Forms.Button")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & " = New System.Windows.Forms.ComboBox")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & " = New System.Windows.Forms.Label")
                        Case "String", "Decimal"
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & " = New System.Windows.Forms.TextBox")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & " = New System.Windows.Forms.Label")
                        Case "DateTime"
                            PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & " = New System.Windows.Forms.DateTimePicker")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & " = New System.Windows.Forms.Label")
                        Case "Boolean"
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & " = New System.Windows.Forms.CheckBox")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & " = New System.Windows.Forms.Label")
                        Case Else
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & " = New System.Windows.Forms.TextBox")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & " = New System.Windows.Forms.Label")
                    End Select
                End If
            Next
            PrintLine(FileCls, TAB(8), "Me.SuspendLayout()")

            Dim tabID As Integer = 500
            Dim tabBtn As Integer = 30
            Dim tabIndex As Integer = 4
            Dim posY As Integer = 0

            Dim col As Integer = 1

            For Each reg In arrEstructura
                If reg.Orden = 1 Or reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                Else

                    If posY = 0 Then
                        posY = 48
                    End If

                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'lbl" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".AutoSize = True")
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".BackColor = System.Drawing.Color.Red")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".Location = New System.Drawing.Point(370, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".Location = New System.Drawing.Point(925, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".Name = ""lbl" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".Size = New System.Drawing.Size(13, 13)")
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".TabIndex = " & tabID)
                            tabID = tabID + 1
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".Text = ""0""")
                            PrintLine(FileCls, TAB(8), "Me.lbl" & reg.nombre & ".Visible = False")
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'btnBusca" & reg.nombre.Substring(3))
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Cursor = System.Windows.Forms.Cursors.Hand")
                            PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Image = CType(Resources.GetObject(""btnBuscarChico.Image""), System.Drawing.Image)")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Location = New System.Drawing.Point(505, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Location = New System.Drawing.Point(1060, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Name = ""btnBusca" & reg.nombre.Substring(3) & """")
                            PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".Size = New System.Drawing.Size(30, 21)")
                            PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".TabIndex = " & tabBtn)
                            tabBtn = tabBtn + 1
                            PrintLine(FileCls, TAB(8), "Me.btnBusca" & reg.nombre.Substring(3) & ".UseVisualStyleBackColor = True")
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'cmb" & reg.nombre.Substring(3))
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".FormattingEnabled = True")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".Location = New System.Drawing.Point(161, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".Location = New System.Drawing.Point(716, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".Name = ""cmb" & reg.nombre.Substring(3) & """")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".Size = New System.Drawing.Size(329, 21)")
                            PrintLine(FileCls, TAB(8), "Me.cmb" & reg.nombre.Substring(3) & ".TabIndex = " & tabIndex)
                            tabIndex = tabIndex + 1
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'etiquieta" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".ForeColor = System.Drawing.Color.Black")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(28, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(583, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Size = New System.Drawing.Size(127, 21)")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TabIndex = " & tabID)
                            tabID = tabID + 1

                            Dim largo As Integer = reg.nombre.Length
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Text = """ & FormatoNombre(reg.nombre.Substring(3, largo - 3)) & """")

                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
                            PrintLine(FileCls, TAB(8), "'")
                        Case "String"
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'txt" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".BackColor = System.Drawing.Color.White")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Location = New System.Drawing.Point(161, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Location = New System.Drawing.Point(716, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".MaxLength = 50")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Name = ""txt" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Size = New System.Drawing.Size(385, 20)")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".TabIndex = " & tabIndex)
                            tabIndex = tabIndex + 1
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'etiquieta" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".ForeColor = System.Drawing.Color.Black")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(28, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(583, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Size = New System.Drawing.Size(127, 21)")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TabIndex = " & tabID)
                            tabID = tabID + 1
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Text = """ & FormatoNombre(reg.nombre) & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
                            PrintLine(FileCls, TAB(8), "'")
                        Case "Decimal"
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'txt" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".BackColor = System.Drawing.Color.White")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".ForeColor = System.Drawing.Color.Blue")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Location = New System.Drawing.Point(161, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Location = New System.Drawing.Point(716, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".MaxLength = 50")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Name = ""txt" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".Size = New System.Drawing.Size(222, 31)")
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".TabIndex = " & tabIndex)
                            tabIndex = tabIndex + 1
                            PrintLine(FileCls, TAB(8), "Me.txt" & reg.nombre & ".TextAlign = System.Windows.Forms.HorizontalAlignment.Right")
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'etiquieta" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".ForeColor = System.Drawing.Color.Black")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(28, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(583, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Size = New System.Drawing.Size(127, 21)")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TabIndex = " & tabID)
                            tabID = tabID + 1
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Text = """ & FormatoNombre(reg.nombre) & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
                            PrintLine(FileCls, TAB(8), "'")
                        Case "DateTime"
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'dtp" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".Format = System.Windows.Forms.DateTimePickerFormat.[Short]")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".Location = New System.Drawing.Point(161, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".Location = New System.Drawing.Point(716, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".Name = ""cmb" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".Size = New System.Drawing.Size(93, 20)")
                            PrintLine(FileCls, TAB(8), "Me.dtp" & reg.nombre & ".TabIndex = " & tabIndex)
                            tabIndex = tabIndex + 1
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'etiquieta" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".ForeColor = System.Drawing.Color.Black")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(28, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(583, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Size = New System.Drawing.Size(127, 21)")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TabIndex = " & tabID)
                            tabID = tabID + 1
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Text = """ & FormatoNombre(reg.nombre) & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
                            PrintLine(FileCls, TAB(8), "'")
                        Case "Boolean"
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'chk" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".AutoSize = True")
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Location = New System.Drawing.Point(161, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Location = New System.Drawing.Point(716, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Size = New System.Drawing.Size(15, 14)")
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".TabIndex = " & tabIndex)
                            tabIndex = tabIndex + 1
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".Text = """ & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.chk" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "'etiquieta" & reg.nombre)
                            PrintLine(FileCls, TAB(8), "'")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Font = New System.Drawing.Font(""Microsoft Sans Serif"", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".ForeColor = System.Drawing.Color.Black")
                            If col = 1 Then
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(28, " & posY & ")")
                            Else
                                PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Location = New System.Drawing.Point(583, " & posY & ")")
                            End If
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Name = ""etiqueta" & reg.nombre & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Size = New System.Drawing.Size(127, 21)")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TabIndex = " & tabID)
                            tabID = tabID + 1
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".Text = """ & FormatoNombre(reg.nombre) & """")
                            PrintLine(FileCls, TAB(8), "Me.etiqueta" & reg.nombre & ".TextAlign = System.Drawing.ContentAlignment.MiddleLeft")
                            PrintLine(FileCls, TAB(8), "'")
                        Case Else
                    End Select
                    'incremento la posicion
                    If reg.tiposql = "Decimal" Then
                        posY = posY + 53
                    Else
                        posY = posY + 43
                    End If

                    If posY > 568 Then
                        col = 2
                        posY = 0
                    End If

                End If
            Next
            'posY = posY + 43
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'btnSalir")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.ForeColor = System.Drawing.Color.Black")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Image = CType(Resources.GetObject(""btnSalir.Image""), System.Drawing.Image)")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
            If col = 1 Then
                PrintLine(FileCls, TAB(8), "Me.btnSalir.Location = New System.Drawing.Point(499, " & posY + 40 & ")")
            Else
                PrintLine(FileCls, TAB(8), "Me.btnSalir.Location = New System.Drawing.Point(1000, 653)")
            End If
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Name = ""btnSalir""")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Size = New System.Drawing.Size(81, 69)")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.TabIndex = 21")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.Text = ""&Salir""")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
            PrintLine(FileCls, TAB(8), "Me.btnSalir.UseVisualStyleBackColor = True")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'btnGuardar")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.Font = New System.Drawing.Font(""Verdana"", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.ForeColor = System.Drawing.Color.Black")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.Image = CType(Resources.GetObject(""btnGuardar.Image""), System.Drawing.Image)")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.TopCenter")
            If col = 1 Then
                PrintLine(FileCls, TAB(8), "Me.btnGuardar.Location = New System.Drawing.Point(392," & posY + 40 & ")")
            Else
                PrintLine(FileCls, TAB(8), "Me.btnGuardar.Location = New System.Drawing.Point(947, 653)")
            End If
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.Name = ""btnGuardar""")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.Size = New System.Drawing.Size(78, 69)")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.TabIndex = 20")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.Text = ""&Guardar""")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.TextAlign = System.Drawing.ContentAlignment.BottomCenter")
            PrintLine(FileCls, TAB(8), "Me.btnGuardar.UseVisualStyleBackColor = True")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'GroupBox1")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.Location = New System.Drawing.Point(12, 17)")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.Name = ""GroupBox1""")
            If col = 1 Then
                PrintLine(FileCls, TAB(8), "Me.GroupBox1.Size = New System.Drawing.Size(568, " & posY & ")")
            Else
                PrintLine(FileCls, TAB(8), "Me.GroupBox1.Size = New System.Drawing.Size(1000, 606)")
            End If
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.TabIndex = 0")
            PrintLine(FileCls, TAB(8), "Me.GroupBox1.TabStop = False")
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "'frmDetalle" & tabla)
            PrintLine(FileCls, TAB(8), "'")
            PrintLine(FileCls, TAB(8), "Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)")
            PrintLine(FileCls, TAB(8), "Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font")
            If col = 1 Then
                PrintLine(FileCls, TAB(8), "Me.ClientSize = New System.Drawing.Size(600, " & posY + 120 & ")")
            Else
                PrintLine(FileCls, TAB(8), "Me.ClientSize = New System.Drawing.Size(1024, 768)")
            End If
            'control.add
            For Each reg In arrEstructura
                If reg.Orden = 1 Or reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.lbl" & reg.nombre & ")")
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.btnBusca" & reg.nombre.Substring(3) & ")")
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.cmb" & reg.nombre.Substring(3) & ")")
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.etiqueta" & reg.nombre & ")")
                        Case "String", "Decimal"
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.txt" & reg.nombre & ")")
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.etiqueta" & reg.nombre & ")")
                        Case "DateTime"
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.dtp" & reg.nombre & ")")
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.etiqueta" & reg.nombre & ")")
                        Case "Boolean"
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.chk" & reg.nombre & ")")
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.etiqueta" & reg.nombre & ")")
                        Case Else
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me." & reg.nombre & ")")
                            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.etiqueta" & reg.nombre & ")")
                    End Select
                End If
            Next
            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.btnSalir)")
            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.btnGuardar)")
            PrintLine(FileCls, TAB(8), "Me.Controls.Add(Me.GroupBox1)")

            PrintLine(FileCls, TAB(8), "Me.Name = ""frmDetalle" & tabla & """")
            PrintLine(FileCls, TAB(8), "Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen")
            PrintLine(FileCls, TAB(8), "Me.Text = ""frmDetalle" & tabla & """")
            PrintLine(FileCls, TAB(8), "Me.ResumeLayout(False)")
            PrintLine(FileCls, TAB(8), "Me.PerformLayout()")
            PrintLine(FileCls, "")
            PrintLine(FileCls, TAB(4), "End Sub")

            'withevents
            For Each reg In arrEstructura
                If reg.Orden = 1 Or reg.nombre = "id_cliente_maestro" Or reg.nombre = "sincronizado" Or reg.nombre = "operacion" Then
                    'next row
                Else
                    Select Case reg.tiposql
                        Case "Int32", "Int64", "Int16", "Integer"
                            PrintLine(FileCls, TAB(4), "Friend WithEvents lbl" & reg.nombre & " As System.Windows.Forms.Label")
                            PrintLine(FileCls, TAB(4), "Friend WithEvents btnBusca" & reg.nombre.Substring(3) & " As System.Windows.Forms.Button")
                            PrintLine(FileCls, TAB(4), "Friend WithEvents cmb" & reg.nombre.Substring(3) & " As System.Windows.Forms.ComboBox")
                            PrintLine(FileCls, TAB(4), "Friend WithEvents etiqueta" & reg.nombre & " As System.Windows.Forms.Label")
                        Case "String", "Decimal"
                            PrintLine(FileCls, TAB(4), "Protected WithEvents txt" & reg.nombre & " As System.Windows.Forms.TextBox")
                            PrintLine(FileCls, TAB(4), "Friend WithEvents etiqueta" & reg.nombre & " As System.Windows.Forms.Label")
                        Case "DateTime"
                            PrintLine(FileCls, TAB(4), "Protected WithEvents dtp" & reg.nombre & " As System.Windows.Forms.DateTimePicker")
                            PrintLine(FileCls, TAB(4), "Friend WithEvents etiqueta" & reg.nombre & " As System.Windows.Forms.Label")
                        Case "Boolean"
                            PrintLine(FileCls, TAB(4), "Protected WithEvents chk" & reg.nombre & " As System.Windows.Forms.CheckBox")
                            PrintLine(FileCls, TAB(4), "Friend WithEvents etiqueta" & reg.nombre & " As System.Windows.Forms.Label")
                        Case Else
                            PrintLine(FileCls, TAB(4), "Protected WithEvents txt" & reg.nombre & " As System.Windows.Forms.TextBox")
                            PrintLine(FileCls, TAB(4), "Friend WithEvents etiqueta" & reg.nombre & " As System.Windows.Forms.Label")
                    End Select
                End If
            Next

            PrintLine(FileCls, TAB(4), "Protected WithEvents btnSalir As System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(4), "Protected WithEvents btnGuardar As System.Windows.Forms.Button")
            PrintLine(FileCls, TAB(4), "Protected WithEvents GroupBox1 As System.Windows.Forms.GroupBox")
            PrintLine(FileCls, TAB(4), "Protected WithEvents ttGeneral As System.Windows.Forms.ToolTip")
            PrintLine(FileCls, TAB(0), "End Class")
            PrintLine(FileCls, "")


            FileClose(FileCls)
        End If

    End Sub



End Class
