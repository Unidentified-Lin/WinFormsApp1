Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.CompilerServices

Module DataGridViewExtension
    <Extension()>
    Public Sub AddColumn(dgv As DataGridView, valueType As Type, propName As String, isReadOnly As Boolean, colName As String)

        Dim column As DataGridViewColumn
        If valueType.IsEnum Then
            column = New DataGridViewComboBoxColumn()
            CType(column, DataGridViewComboBoxColumn).DataSource = [Enum].GetValues(valueType)
        ElseIf valueType.Equals(GetType(Boolean)) Then
            column = New DataGridViewCheckBoxColumn()
        Else
            column = New DataGridViewTextBoxColumn()
        End If

        column.ValueType = valueType
        column.Name = colName
        column.DataPropertyName = propName
        column.ReadOnly = isReadOnly

        dgv.Columns.Add(column)
    End Sub

    <Extension()>
    Public Sub AddColumns(dgv As DataGridView, type As Type)
        Dim propInfos As PropertyInfo() = type.GetProperties
        For Each propInfo In propInfos
            If (propInfo Is Nothing) Then
                Throw New ArgumentException("No accessible " & propInfo.Name & " property was found in the " & type.Name & " type.")
            End If

            Dim browsables As BrowsableAttribute() = propInfo.GetCustomAttributes(GetType(BrowsableAttribute), False)
            If browsables.Length > 0 AndAlso Not browsables(0).Browsable Then
                Throw New ArgumentException("The " & propInfo.Name & " property has a " & "Browsable(false) attribute, and therefore cannot be bound.")
            End If
            dgv.AddColumn(propInfo.PropertyType, propInfo.Name, True, propInfo.Name)
        Next
    End Sub
    <Extension()>
    Public Sub AddObjDatas(Of T)(dgv As DataGridView, dataObjs As T())
        If dgv.Columns.Count.Equals(0) Then
            dgv.AddColumns(GetType(T))
        End If
        'Dim list = New List(Of T)
        'If dgv.DataSource IsNot Nothing Then
        '    list.AddRange(dgv.DataSource)
        'End If
        'list.AddRange(dataObjs)
        'dgv.DataSource = list
        dgv.DataSource = dataObjs
    End Sub
End Module
