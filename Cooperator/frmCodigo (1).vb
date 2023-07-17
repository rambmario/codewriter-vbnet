Imports System.Data.SqlClient

Public Class frmCodigo

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        For i As Integer = 0 To CInt(Me.txtCantidad.Text)
            Me.RichTextBox1.Text = Me.RichTextBox1.Text & vbCr & Me.TextBox1.Text & i.ToString & Me.TextBox3.Text & Me.TextBox4.Text
        Next

    End Sub

    Dim odtTabla As New DataTable
    Private Sub frmCodigo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.txtCadenaConexion.Text = CType(Me.Owner, frmPrincipal).txtCadenaConexion.Text


        Dim cn As New SqlConnection(Me.txtCadenaConexion.Text)
        Dim da As New SqlDataAdapter("SELECT TOP 100 PERCENT name, id FROM dbo.sysobjects WHERE (type = 'U') AND name <> 'dtproperties' ORDER BY name", cn)

        Dim cb As New SqlCommandBuilder(da)
        Dim cbSistema As New SqlCommandBuilder(da)

        odtTabla.Clear()
        '   If odtTabla.Rows.Count <= 0 Then
        da.Fill(odtTabla)
        '  End If

        'Me.lstTabla.Items.Clear()
        'For i As Integer = 0 To odtTabla.Rows.Count - 1
        '    Me.lstTabla.Items.Add(odtTabla.Rows(i).Item("name"))
        'Next
        'Me.lstTabla.Focus()
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        For i As Integer = 0 To odtTabla.Rows.Count - 1

            If Me.RadioButton1.Checked = True Then
                Me.RichTextBox1.Text = Me.RichTextBox1.Text & vbCr & Me.TextBox1.Text & i + 1 & Me.TextBox3.Text & odtTabla.Rows(i).Item("name").ToString() & Me.TextBox4.Text
            End If
            If Me.RadioButton2.Checked = True Then
                Me.RichTextBox1.Text = Me.RichTextBox1.Text & vbCr & Me.TextBox1.Text & odtTabla.Rows(i).Item("name").ToString() & Me.TextBox3.Text
            End If
            If Me.RadioButton3.Checked = True Then
                Me.RichTextBox1.Text = Me.RichTextBox1.Text & vbCr & Me.TextBox1.Text & odtTabla.Rows(i).Item("name").ToString() & Me.TextBox3.Text & odtTabla.Rows(i).Item("name").ToString() & Me.TextBox4.Text
            End If
        Next
    End Sub


End Class