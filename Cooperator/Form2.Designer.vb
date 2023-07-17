<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form2))
        Me.txtBaseDato = New System.Windows.Forms.TextBox()
        Me.txtTabla = New System.Windows.Forms.TextBox()
        Me.txtPK = New System.Windows.Forms.TextBox()
        Me.txtCampo = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ListBox2 = New System.Windows.Forms.ListBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.txtCadena = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.lblTipoDato = New System.Windows.Forms.Label()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.txtDBNew = New System.Windows.Forms.TextBox()
        Me.txtPath = New System.Windows.Forms.TextBox()
        Me.btnPath = New System.Windows.Forms.Button()
        Me.btnMakeDB = New System.Windows.Forms.Button()
        Me.fbdDB = New System.Windows.Forms.FolderBrowserDialog()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.CheckedListBox1 = New System.Windows.Forms.CheckedListBox()
        Me.btnGetColumns = New System.Windows.Forms.Button()
        Me.cmbTabla = New System.Windows.Forms.ComboBox()
        Me.lblid_tabla = New System.Windows.Forms.Label()
        Me.btnAdd_column = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.CheckedListBox2 = New System.Windows.Forms.CheckedListBox()
        Me.btnGetAll = New System.Windows.Forms.Button()
        Me.btnSeleccionCampo = New System.Windows.Forms.Button()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.chkServidor = New System.Windows.Forms.CheckBox()
        Me.txtDimension = New System.Windows.Forms.TextBox()
        Me.btnGetOne = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtBaseDato
        '
        Me.txtBaseDato.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBaseDato.Location = New System.Drawing.Point(64, 39)
        Me.txtBaseDato.Name = "txtBaseDato"
        Me.txtBaseDato.Size = New System.Drawing.Size(323, 22)
        Me.txtBaseDato.TabIndex = 5
        '
        'txtTabla
        '
        Me.txtTabla.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTabla.Location = New System.Drawing.Point(64, 70)
        Me.txtTabla.Name = "txtTabla"
        Me.txtTabla.Size = New System.Drawing.Size(323, 22)
        Me.txtTabla.TabIndex = 6
        '
        'txtPK
        '
        Me.txtPK.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower
        Me.txtPK.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPK.Location = New System.Drawing.Point(64, 99)
        Me.txtPK.Name = "txtPK"
        Me.txtPK.Size = New System.Drawing.Size(323, 22)
        Me.txtPK.TabIndex = 7
        '
        'txtCampo
        '
        Me.txtCampo.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower
        Me.txtCampo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCampo.Location = New System.Drawing.Point(64, 171)
        Me.txtCampo.Name = "txtCampo"
        Me.txtCampo.Size = New System.Drawing.Size(323, 22)
        Me.txtCampo.TabIndex = 8
        '
        'Button1
        '
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Button1.Location = New System.Drawing.Point(288, 449)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(99, 58)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Crear Tabla"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(0, 201)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(58, 30)
        Me.ListBox1.TabIndex = 9
        Me.ListBox1.Visible = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(64, 133)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(323, 23)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "5 bit  -  6 int  -  7 char  -  8 decimal  -  9 fecha"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ListBox2
        '
        Me.ListBox2.FormattingEnabled = True
        Me.ListBox2.Location = New System.Drawing.Point(64, 243)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(218, 264)
        Me.ListBox2.TabIndex = 11
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(64, 642)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(320, 23)
        Me.ProgressBar1.TabIndex = 12
        '
        'txtCadena
        '
        Me.txtCadena.Location = New System.Drawing.Point(8, 517)
        Me.txtCadena.Name = "txtCadena"
        Me.txtCadena.Size = New System.Drawing.Size(75, 20)
        Me.txtCadena.TabIndex = 13
        Me.txtCadena.Visible = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 39)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 18)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Base"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(6, 70)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 18)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "Tabla"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(6, 99)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 18)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "PK"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(6, 171)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 18)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "Campos"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSalir
        '
        Me.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnSalir.Image = CType(resources.GetObject("btnSalir.Image"), System.Drawing.Image)
        Me.btnSalir.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnSalir.Location = New System.Drawing.Point(941, 671)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(75, 67)
        Me.btnSalir.TabIndex = 10
        Me.btnSalir.Text = "Salir"
        Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnSalir.UseVisualStyleBackColor = True
        '
        'lblTipoDato
        '
        Me.lblTipoDato.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lblTipoDato.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTipoDato.Location = New System.Drawing.Point(64, 201)
        Me.lblTipoDato.Name = "lblTipoDato"
        Me.lblTipoDato.Size = New System.Drawing.Size(218, 26)
        Me.lblTipoDato.TabIndex = 18
        Me.lblTipoDato.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblTipoDato.Visible = False
        '
        'Button3
        '
        Me.Button3.Image = CType(resources.GetObject("Button3.Image"), System.Drawing.Image)
        Me.Button3.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Button3.Location = New System.Drawing.Point(288, 244)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(99, 58)
        Me.Button3.TabIndex = 19
        Me.Button3.Text = "Borrar Campo"
        Me.Button3.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Image = CType(resources.GetObject("Button4.Image"), System.Drawing.Image)
        Me.Button4.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Button4.Location = New System.Drawing.Point(288, 308)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(99, 58)
        Me.Button4.TabIndex = 20
        Me.Button4.Text = "Borrar Todo"
        Me.Button4.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Image = CType(resources.GetObject("Button5.Image"), System.Drawing.Image)
        Me.Button5.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Button5.Location = New System.Drawing.Point(288, 385)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(99, 58)
        Me.Button5.TabIndex = 21
        Me.Button5.Text = "Nueva Tabla"
        Me.Button5.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.Button5.UseVisualStyleBackColor = True
        '
        'txtDBNew
        '
        Me.txtDBNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDBNew.Location = New System.Drawing.Point(64, 566)
        Me.txtDBNew.Name = "txtDBNew"
        Me.txtDBNew.Size = New System.Drawing.Size(319, 22)
        Me.txtDBNew.TabIndex = 22
        '
        'txtPath
        '
        Me.txtPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPath.Location = New System.Drawing.Point(64, 603)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(319, 22)
        Me.txtPath.TabIndex = 24
        '
        'btnPath
        '
        Me.btnPath.Image = CType(resources.GetObject("btnPath.Image"), System.Drawing.Image)
        Me.btnPath.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnPath.Location = New System.Drawing.Point(398, 594)
        Me.btnPath.Name = "btnPath"
        Me.btnPath.Size = New System.Drawing.Size(50, 43)
        Me.btnPath.TabIndex = 26
        Me.btnPath.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnPath.UseVisualStyleBackColor = True
        '
        'btnMakeDB
        '
        Me.btnMakeDB.Image = CType(resources.GetObject("btnMakeDB.Image"), System.Drawing.Image)
        Me.btnMakeDB.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnMakeDB.Location = New System.Drawing.Point(64, 680)
        Me.btnMakeDB.Name = "btnMakeDB"
        Me.btnMakeDB.Size = New System.Drawing.Size(99, 58)
        Me.btnMakeDB.TabIndex = 27
        Me.btnMakeDB.Text = "Crear Base Datos"
        Me.btnMakeDB.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnMakeDB.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label8.Font = New System.Drawing.Font("Comic Sans MS", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(12, 9)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(375, 19)
        Me.Label8.TabIndex = 28
        Me.Label8.Text = "CREAR TABLAS"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label9.Font = New System.Drawing.Font("Comic Sans MS", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(8, 540)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(379, 19)
        Me.Label9.TabIndex = 29
        Me.Label9.Text = "CREAR BASE DE DATOS"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CheckedListBox1
        '
        Me.CheckedListBox1.FormattingEnabled = True
        Me.CheckedListBox1.Location = New System.Drawing.Point(407, 121)
        Me.CheckedListBox1.Name = "CheckedListBox1"
        Me.CheckedListBox1.Size = New System.Drawing.Size(233, 319)
        Me.CheckedListBox1.TabIndex = 30
        '
        'btnGetColumns
        '
        Me.btnGetColumns.Image = CType(resources.GetObject("btnGetColumns.Image"), System.Drawing.Image)
        Me.btnGetColumns.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnGetColumns.Location = New System.Drawing.Point(500, 493)
        Me.btnGetColumns.Name = "btnGetColumns"
        Me.btnGetColumns.Size = New System.Drawing.Size(60, 68)
        Me.btnGetColumns.TabIndex = 31
        Me.btnGetColumns.Text = "Borrar Campos"
        Me.btnGetColumns.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnGetColumns.UseVisualStyleBackColor = True
        '
        'cmbTabla
        '
        Me.cmbTabla.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbTabla.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbTabla.FormattingEnabled = True
        Me.cmbTabla.Location = New System.Drawing.Point(407, 66)
        Me.cmbTabla.Name = "cmbTabla"
        Me.cmbTabla.Size = New System.Drawing.Size(233, 21)
        Me.cmbTabla.TabIndex = 32
        '
        'lblid_tabla
        '
        Me.lblid_tabla.AutoSize = True
        Me.lblid_tabla.BackColor = System.Drawing.Color.Red
        Me.lblid_tabla.Location = New System.Drawing.Point(558, 69)
        Me.lblid_tabla.Name = "lblid_tabla"
        Me.lblid_tabla.Size = New System.Drawing.Size(13, 13)
        Me.lblid_tabla.TabIndex = 33
        Me.lblid_tabla.Text = "0"
        Me.lblid_tabla.Visible = False
        '
        'btnAdd_column
        '
        Me.btnAdd_column.Image = CType(resources.GetObject("btnAdd_column.Image"), System.Drawing.Image)
        Me.btnAdd_column.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnAdd_column.Location = New System.Drawing.Point(581, 493)
        Me.btnAdd_column.Name = "btnAdd_column"
        Me.btnAdd_column.Size = New System.Drawing.Size(59, 68)
        Me.btnAdd_column.TabIndex = 34
        Me.btnAdd_column.Text = "Modificar Campos"
        Me.btnAdd_column.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnAdd_column.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label10.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(407, 44)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(44, 18)
        Me.Label10.TabIndex = 35
        Me.Label10.Text = "Tablas"
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label11.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(407, 99)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(62, 18)
        Me.Label11.TabIndex = 36
        Me.Label11.Text = "Columnas"
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label12.Font = New System.Drawing.Font("Comic Sans MS", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(407, 9)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(229, 19)
        Me.Label12.TabIndex = 37
        Me.Label12.Text = "MODIFICACION TABLAS"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label13.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(7, 244)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(52, 18)
        Me.Label13.TabIndex = 38
        Me.Label13.Text = "Campos"
        '
        'CheckedListBox2
        '
        Me.CheckedListBox2.FormattingEnabled = True
        Me.CheckedListBox2.Location = New System.Drawing.Point(709, 121)
        Me.CheckedListBox2.Name = "CheckedListBox2"
        Me.CheckedListBox2.Size = New System.Drawing.Size(307, 544)
        Me.CheckedListBox2.TabIndex = 40
        '
        'btnGetAll
        '
        Me.btnGetAll.Image = CType(resources.GetObject("btnGetAll.Image"), System.Drawing.Image)
        Me.btnGetAll.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnGetAll.Location = New System.Drawing.Point(709, 676)
        Me.btnGetAll.Name = "btnGetAll"
        Me.btnGetAll.Size = New System.Drawing.Size(93, 58)
        Me.btnGetAll.TabIndex = 41
        Me.btnGetAll.Text = "Generar GetAll"
        Me.btnGetAll.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnGetAll.UseVisualStyleBackColor = True
        '
        'btnSeleccionCampo
        '
        Me.btnSeleccionCampo.Image = CType(resources.GetObject("btnSeleccionCampo.Image"), System.Drawing.Image)
        Me.btnSeleccionCampo.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnSeleccionCampo.Location = New System.Drawing.Point(645, 246)
        Me.btnSeleccionCampo.Name = "btnSeleccionCampo"
        Me.btnSeleccionCampo.Size = New System.Drawing.Size(58, 56)
        Me.btnSeleccionCampo.TabIndex = 42
        Me.btnSeleccionCampo.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnSeleccionCampo.UseVisualStyleBackColor = True
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label15.Font = New System.Drawing.Font("Comic Sans MS", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(705, 9)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(311, 19)
        Me.Label15.TabIndex = 43
        Me.Label15.Text = "MODIFICACION DEL GETALL DE LA GRILLA"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label16.ForeColor = System.Drawing.Color.Black
        Me.Label16.Location = New System.Drawing.Point(709, 38)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(300, 79)
        Me.Label16.TabIndex = 45
        Me.Label16.Text = "El campo que tenga el focus sera el ORDER BY y el FIND" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Button7
        '
        Me.Button7.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button7.Image = CType(resources.GetObject("Button7.Image"), System.Drawing.Image)
        Me.Button7.Location = New System.Drawing.Point(676, 121)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(27, 26)
        Me.Button7.TabIndex = 46
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Button8
        '
        Me.Button8.Image = CType(resources.GetObject("Button8.Image"), System.Drawing.Image)
        Me.Button8.Location = New System.Drawing.Point(676, 155)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(27, 26)
        Me.Button8.TabIndex = 47
        Me.Button8.UseVisualStyleBackColor = True
        '
        'Label17
        '
        Me.Label17.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label17.ForeColor = System.Drawing.Color.Black
        Me.Label17.Location = New System.Drawing.Point(411, 449)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(252, 21)
        Me.Label17.TabIndex = 48
        Me.Label17.Text = "Para Borrar se Selecciona en la Lista de Columnas"
        '
        'Label18
        '
        Me.Label18.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label18.ForeColor = System.Drawing.Color.Black
        Me.Label18.Location = New System.Drawing.Point(411, 470)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(252, 21)
        Me.Label18.TabIndex = 49
        Me.Label18.Text = "Para Modificar se Agregan en la Lista de Campos"
        '
        'Label19
        '
        Me.Label19.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label19.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(6, 642)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(53, 18)
        Me.Label19.TabIndex = 52
        Me.Label19.Text = "%"
        '
        'Label20
        '
        Me.Label20.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label20.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(6, 607)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(53, 18)
        Me.Label20.TabIndex = 51
        Me.Label20.Text = "Path"
        '
        'Label21
        '
        Me.Label21.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label21.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(6, 570)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(53, 18)
        Me.Label21.TabIndex = 50
        Me.Label21.Text = "Base"
        '
        'chkServidor
        '
        Me.chkServidor.AutoSize = True
        Me.chkServidor.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.chkServidor.Location = New System.Drawing.Point(555, 44)
        Me.chkServidor.Name = "chkServidor"
        Me.chkServidor.Size = New System.Drawing.Size(87, 17)
        Me.chkServidor.TabIndex = 53
        Me.chkServidor.Text = "Con Servidor"
        Me.chkServidor.UseVisualStyleBackColor = False
        '
        'txtDimension
        '
        Me.txtDimension.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.txtDimension.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDimension.Location = New System.Drawing.Point(288, 202)
        Me.txtDimension.Name = "txtDimension"
        Me.txtDimension.Size = New System.Drawing.Size(53, 26)
        Me.txtDimension.TabIndex = 54
        Me.txtDimension.Text = "50"
        Me.txtDimension.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnGetOne
        '
        Me.btnGetOne.Image = CType(resources.GetObject("btnGetOne.Image"), System.Drawing.Image)
        Me.btnGetOne.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnGetOne.Location = New System.Drawing.Point(819, 676)
        Me.btnGetOne.Name = "btnGetOne"
        Me.btnGetOne.Size = New System.Drawing.Size(93, 58)
        Me.btnGetOne.TabIndex = 55
        Me.btnGetOne.Text = "Generar GetOne"
        Me.btnGetOne.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnGetOne.UseVisualStyleBackColor = True
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.CancelButton = Me.btnSalir
        Me.ClientSize = New System.Drawing.Size(1028, 746)
        Me.Controls.Add(Me.btnGetOne)
        Me.Controls.Add(Me.txtDimension)
        Me.Controls.Add(Me.chkServidor)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.Button8)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.btnSeleccionCampo)
        Me.Controls.Add(Me.btnGetAll)
        Me.Controls.Add(Me.CheckedListBox2)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.btnAdd_column)
        Me.Controls.Add(Me.lblid_tabla)
        Me.Controls.Add(Me.cmbTabla)
        Me.Controls.Add(Me.btnGetColumns)
        Me.Controls.Add(Me.CheckedListBox1)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.btnMakeDB)
        Me.Controls.Add(Me.btnPath)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.txtDBNew)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.lblTipoDato)
        Me.Controls.Add(Me.btnSalir)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtCadena)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.ListBox2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.txtCampo)
        Me.Controls.Add(Me.txtPK)
        Me.Controls.Add(Me.txtTabla)
        Me.Controls.Add(Me.txtBaseDato)
        Me.Name = "Form2"
        Me.Text = "Gestión de Base de Datos del Cooperator"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtBaseDato As System.Windows.Forms.TextBox
    Friend WithEvents txtTabla As System.Windows.Forms.TextBox
    Friend WithEvents txtPK As System.Windows.Forms.TextBox
    Friend WithEvents txtCampo As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ListBox2 As System.Windows.Forms.ListBox
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents txtCadena As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnSalir As System.Windows.Forms.Button
    Friend WithEvents lblTipoDato As System.Windows.Forms.Label
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents txtDBNew As System.Windows.Forms.TextBox
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents btnPath As System.Windows.Forms.Button
    Friend WithEvents btnMakeDB As System.Windows.Forms.Button
    Friend WithEvents fbdDB As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents CheckedListBox1 As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnGetColumns As System.Windows.Forms.Button
    Friend WithEvents cmbTabla As System.Windows.Forms.ComboBox
    Friend WithEvents lblid_tabla As System.Windows.Forms.Label
    Friend WithEvents btnAdd_column As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents CheckedListBox2 As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnGetAll As System.Windows.Forms.Button
    Friend WithEvents btnSeleccionCampo As System.Windows.Forms.Button
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents chkServidor As System.Windows.Forms.CheckBox
    Friend WithEvents txtDimension As System.Windows.Forms.TextBox
    Friend WithEvents btnGetOne As System.Windows.Forms.Button
End Class
