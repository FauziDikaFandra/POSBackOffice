Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Collections

Public Class CCombo
    Private myID As String
    Private myNumber As String
    Private myName As String
    Private myNumber_Name As String
    Private myNumber_NameDate As String
    Private mydate As Date

    Public ReadOnly Property ID() As String
        Get
            Return myID
        End Get
    End Property

    Public ReadOnly Property Number() As String
        Get
            Return myNumber
        End Get
    End Property

    Public ReadOnly Property Name() As String
        Get
            Return myName
        End Get
    End Property

    Public ReadOnly Property Number_Name() As String
        Get
            Return myNumber_Name
        End Get
    End Property

    Public ReadOnly Property Number_NameDate() As String
        Get
            Return myNumber_NameDate
        End Get
    End Property

    'Public Sub New(ByVal ID As Int16, ByVal Number As String, ByVal Name As String)

    '    myID = ID
    '    myNumber = Number
    '    myName = Name
    '    myNumber_Name = Number + "  " + Name

    'End Sub

    Public Sub New(ByVal ID As String, ByVal dt As Date)
        myID = ID
        myNumber_Name = ID.ToString + " " + dt.Day.ToString + "/" + dt.Month.ToString + "/" + dt.Year.ToString
        'myName = Name
        'myNumber_Name = Number + "  " + Name
    End Sub

    Public Sub New(ByVal ID As String, ByVal dt As Date, ByVal angka As Boolean, ByVal waktu As Boolean)
        myID = ID
        myNumber_NameDate = ID.ToString + " " + dt.Day.ToString + "-" + dt.Month.ToString + "-" + dt.Year.ToString
    End Sub
    Public Sub New(ByVal ID As String, ByVal name As String)
        myID = ID
        'myNumber_Name = name
        'myName = Name
        myNumber_Name = name
    End Sub
End Class
