﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Este código fue generado por una herramienta.
'     Versión de runtime:4.0.30319.42000
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System


'StronglyTypedResourceBuilder generó automáticamente esta clase
'a través de una herramienta como ResGen o Visual Studio.
'Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
'con la opción /str o recompile su proyecto de VS.
'''<summary>
'''  Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
'''</summary>
<Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0"),  _
 Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
 Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
Friend Class frmAbmGeneral
    
    Private Shared resourceMan As Global.System.Resources.ResourceManager
    
    Private Shared resourceCulture As Global.System.Globalization.CultureInfo
    
    <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
    Friend Sub New()
        MyBase.New
    End Sub
    
    '''<summary>
    '''  Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
    '''</summary>
    <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Friend Shared ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
        Get
            If Object.ReferenceEquals(resourceMan, Nothing) Then
                Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("Cooperator.frmAbmGeneral", GetType(frmAbmGeneral).Assembly)
                resourceMan = temp
            End If
            Return resourceMan
        End Get
    End Property
    
    '''<summary>
    '''  Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
    '''  búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
    '''</summary>
    <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Friend Shared Property Culture() As Global.System.Globalization.CultureInfo
        Get
            Return resourceCulture
        End Get
        Set
            resourceCulture = value
        End Set
    End Property
    
    '''<summary>
    '''  Busca un recurso adaptado de tipo System.Drawing.Bitmap.
    '''</summary>
    Friend Shared ReadOnly Property btnAgregar_Image() As System.Drawing.Bitmap
        Get
            Dim obj As Object = ResourceManager.GetObject("btnAgregar.Image", resourceCulture)
            Return CType(obj,System.Drawing.Bitmap)
        End Get
    End Property
    
    '''<summary>
    '''  Busca un recurso adaptado de tipo System.Drawing.Bitmap.
    '''</summary>
    Friend Shared ReadOnly Property btnAyuda_Image() As System.Drawing.Bitmap
        Get
            Dim obj As Object = ResourceManager.GetObject("btnAyuda.Image", resourceCulture)
            Return CType(obj,System.Drawing.Bitmap)
        End Get
    End Property
    
    '''<summary>
    '''  Busca un recurso adaptado de tipo System.Drawing.Bitmap.
    '''</summary>
    Friend Shared ReadOnly Property btnBorrar_Image() As System.Drawing.Bitmap
        Get
            Dim obj As Object = ResourceManager.GetObject("btnBorrar.Image", resourceCulture)
            Return CType(obj,System.Drawing.Bitmap)
        End Get
    End Property
    
    '''<summary>
    '''  Busca un recurso adaptado de tipo System.Drawing.Bitmap.
    '''</summary>
    Friend Shared ReadOnly Property btnModificar_Image() As System.Drawing.Bitmap
        Get
            Dim obj As Object = ResourceManager.GetObject("btnModificar.Image", resourceCulture)
            Return CType(obj,System.Drawing.Bitmap)
        End Get
    End Property
    
    '''<summary>
    '''  Busca un recurso adaptado de tipo System.Drawing.Bitmap.
    '''</summary>
    Friend Shared ReadOnly Property btnSalir_Image() As System.Drawing.Bitmap
        Get
            Dim obj As Object = ResourceManager.GetObject("btnSalir.Image", resourceCulture)
            Return CType(obj,System.Drawing.Bitmap)
        End Get
    End Property
    
    '''<summary>
    '''  Busca un recurso adaptado de tipo System.Drawing.Point similar a {X=20,Y=12}.
    '''</summary>
    Friend Shared ReadOnly Property ttGeneral_TrayLocation() As System.Drawing.Point
        Get
            Dim obj As Object = ResourceManager.GetObject("ttGeneral.TrayLocation", resourceCulture)
            Return CType(obj,System.Drawing.Point)
        End Get
    End Property
End Class
