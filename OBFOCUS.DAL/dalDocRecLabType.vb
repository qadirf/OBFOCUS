
Option Explicit On 
Option Strict On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Xml
'******************************************************************************
'*
'* Name:        dalDocRecLabType
'*
'* Description: Data access layer for Table PatientInfo
'*
'* Remarks:     Uses OleDb and embedded SQL for maintaining the data.
'*-----------------------------------------------------------------------------
'*                      CHANGE HISTORY
'*   Change No:   Date:          Author:   Description:
'*   _________    ___________    ______    ____________________________________
'*      001       1/26/2005     MR        Created.                                
'* 
'******************************************************************************
Public Class dalDocRecLabType

#Region "Module level variables and enums"

    ' Public ENUM used to enumerate columns 
    Public Enum DescriptionsFields
        fldID = 0
        fldName = 1
    End Enum


    'Used for transaction support
    Private _Transaction As SqlTransaction = Nothing


    '**************************************************************************
    '*  
    '* Name:        Transaction
    '*
    '* Description: Used for transaction support.
    '*
    '* Parameters:  If this property is set, all database operations will be
    '*              performed in the context of a database transaction.
    '*
    '**************************************************************************
    Public Property Transaction() As SqlTransaction
        Get
            Return _Transaction
        End Get
        Set(ByVal Value As SqlTransaction)
            _Transaction = Value
        End Set
    End Property 'Transaction

#End Region



#Region "Constructors"

    '**************************************************************************
    '*  
    '* Name:        New
    '*
    '* Description: Initialize a new instance of the class.
    '*
    '* Parameters:  None
    '*
    '**************************************************************************
    Public Sub New()
    End Sub 'New


    '**************************************************************************
    '*  
    '* Name:        New
    '*
    '* Description: Initialize a new instance of the class.
    '*
    '* Parameters:  Transaction - used for transaction support.
    '*
    '**************************************************************************
    Public Sub New(ByRef Transaction As SqlTransaction)
        Me.Transaction = Transaction
    End Sub 'New

#End Region



#Region "Main procedures - GetComboDual, Add, Update & Delete"
    '**************************************************************************
    '*  
    '* Name:        GetDocRecLabType
    '*
    '* Description: Returns all records in the [Labs] table according
    '*              to specified criteria.
    '*
    '*
    '* Returns:     DataReader containing the specified data. 
    '*
    '**************************************************************************
    Public Function GetDocRecLabType() As SqlDataReader
        ' Call stored procedure and return the data
        Try
            If Me.Transaction Is Nothing Then
                Return SqlHelper.ExecuteReader(Globals.ConnectionString, CommandType.StoredProcedure, "spDocRecLabTypeGet")
            Else
                Return SqlHelper.ExecuteReader(Me.Transaction, CommandType.StoredProcedure, "spDocRecLabTypeGet")
            End If
        Catch ex As Exception
            ExceptionManager.Publish(ex)
        End Try
    End Function
    '**************************************************************************
    '*  
    '* Name:        Update
    '*
    '* Description: Updates a record in the Chart table identified by a key.
    '*
    '*
    '* Returns:     Boolean indicating if record was updated or not. 
    '*              True (record found); False (otherwise).
    '*
    '**************************************************************************
    Public Function Update(ByVal ID As Integer, _
                ByVal Name As String) As Boolean

        Dim intRecordsAffected As Integer = 0
        Dim arParameters(1) As SqlParameter         ' Array to hold stored procedure parameters


        ' Set the stored procedure parameters
        arParameters(Me.DescriptionsFields.fldID) = New SqlParameter("@DocRecLabTypeID", SqlDbType.Int)
        arParameters(Me.DescriptionsFields.fldID).Value = ID
        arParameters(Me.DescriptionsFields.fldName) = New SqlParameter("@DocRecLab", SqlDbType.NVarChar, 50)
        arParameters(Me.DescriptionsFields.fldName).Value = Name
        ' Call stored procedure
        Try
            If Me.Transaction Is Nothing Then
                intRecordsAffected = SqlHelper.ExecuteNonQuery(Globals.ConnectionString, CommandType.StoredProcedure, "spDocRecLabTypeUpdate", arParameters)
            Else
                intRecordsAffected = SqlHelper.ExecuteNonQuery(Me.Transaction, CommandType.StoredProcedure, "spDocRecLabTypeUpdate", arParameters)
            End If
        Catch exception As Exception
            ExceptionManager.Publish(exception)
        End Try

        ' Return False if data was not updated.
        If intRecordsAffected = 0 Then
            Return False
        Else

            Return True
        End If

    End Function


    '**************************************************************************
    '*  
    '* Name:        Add
    '*
    '* Description: Adds a new record to the [PatientInfo] table.
    '*
    '*
    '* Returns:     Boolean indicating if record was added or not. 
    '*              True (record added); False (otherwise).
    '*
    '**************************************************************************
    Public Function Add(ByRef DocRecLabTypeID As Integer, _
                ByVal Name As String) As Boolean

        Dim intRecordsAffected As Integer = 0
        Dim arParameters(1) As SqlParameter         ' Array to hold stored procedure parameters


        ' Set the stored procedure parameters
        arParameters(Me.DescriptionsFields.fldID) = New SqlParameter("@DocRecLabTypeID", SqlDbType.Int)
        arParameters(Me.DescriptionsFields.fldID).Direction = ParameterDirection.Output
        arParameters(Me.DescriptionsFields.fldName) = New SqlParameter("@DocRecLab", SqlDbType.NVarChar, 100)
        arParameters(Me.DescriptionsFields.fldName).Value = Name
        ' Call stored procedure
        Try
            If Me.Transaction Is Nothing Then
                intRecordsAffected = SqlHelper.ExecuteNonQuery(Globals.ConnectionString, CommandType.StoredProcedure, "spDocRecLabTypeInsert", arParameters)
            Else
                intRecordsAffected = SqlHelper.ExecuteNonQuery(Me.Transaction, CommandType.StoredProcedure, "spDocRecLabTypeInsert", arParameters)
            End If
        Catch exception As Exception
            ExceptionManager.Publish(exception)
        End Try

        ' Return False if data was not found.
        If intRecordsAffected = 0 Then
            Return False
        Else
            DocRecLabTypeID = CType(arParameters(0).Value, Integer)
            Return True
        End If
    End Function




    '**************************************************************************
    '*  
    '* Name:        Delete
    '*
    '* Description: Deletes a record from the [PatientInfo] table identified by a key.
    '*
    '* Parameters:  ID - Key of record that we want to delete
    '*
    '* Returns:     Boolean indicating if record was deleted or not. 
    '*              True (record found and deleted); False (otherwise).
    '*
    '**************************************************************************
    Public Function Delete(ByVal ID As Integer) As Boolean
        Dim intRecordsAffected As Integer = 0
        Dim arParameters(0) As SqlParameter         ' Array to hold stored procedure parameters


        ' Set the stored procedure parameters
        arParameters(0) = New SqlParameter("@DocRecLabTypeID", SqlDbType.Int)
        arParameters(0).Value = ID

        ' Call stored procedure
        Try
            If Me.Transaction Is Nothing Then
                intRecordsAffected = SqlHelper.ExecuteNonQuery(Globals.ConnectionString, CommandType.StoredProcedure, "spDocRecLabTypeDelete", arParameters)
            Else
                intRecordsAffected = SqlHelper.ExecuteNonQuery(Me.Transaction, CommandType.StoredProcedure, "spDocRecLabTypeDelete", arParameters)
            End If
        Catch exception As Exception
            ExceptionManager.Publish(exception)
        End Try

        ' Return False if data was not updated.
        If intRecordsAffected = 0 Then
            Return False
        Else

            Return True
        End If

    End Function

#End Region


End Class 'dalDocRecLabType