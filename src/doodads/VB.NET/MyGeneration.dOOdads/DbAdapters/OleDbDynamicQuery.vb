'==============================================================================
' MyGeneration.dOOdads
'
' OleDbDynamicQuery.vb
' Version 5.0
' Updated - 10/12/2005
'------------------------------------------------------------------------------
' Copyright 2004, 2005 by MyGeneration Software.
' All Rights Reserved.
'
' Permission to use, copy, modify, and distribute this software and its 
' documentation for any purpose and without fee is hereby granted, 
' provided that the above copyright notice appear in all copies and that 
' both that copyright notice and this permission notice appear in 
' supporting documentation. 
'
' MYGENERATION SOFTWARE DISCLAIMS ALL WARRANTIES WITH REGARD TO THIS 
' SOFTWARE, INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY 
' AND FITNESS, IN NO EVENT SHALL MYGENERATION SOFTWARE BE LIABLE FOR ANY 
' SPECIAL, INDIRECT OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES 
' WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, 
' WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR OTHER 
' TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE 
' OR PERFORMANCE OF THIS SOFTWARE. 
'==============================================================================

Imports System.Configuration
Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb

Namespace MyGeneration.dOOdads

    Public Class OleDbDynamicQuery
        Inherits DynamicQuery

        Public Sub New(ByVal entity As BusinessEntity)
            MyBase.New(entity)
        End Sub

		Public Overloads Overrides Sub AddOrderBy(ByVal column As String, ByVal direction As WhereParameter.Dir)
			MyBase.AddOrderBy("[" + column + "]", direction)
		End Sub

		Public Overloads Overrides Sub AddOrderBy(ByVal countAll As DynamicQuery, ByVal direction As WhereParameter.Dir)
			If countAll.CountAll Then
				MyBase.AddOrderBy("COUNT(*)", direction)
			End If
		End Sub

		Public Overloads Overrides Sub AddOrderBy(ByVal aggregate As AggregateParameter, ByVal direction As WhereParameter.Dir)
			MyBase.AddOrderBy(GetAggregate(aggregate, False), direction)
		End Sub

		Public Overloads Overrides Sub AddGroupBy(ByVal column As String)
			MyBase.AddGroupBy("[" + column + "]")
		End Sub

		Public Overloads Overrides Sub AddGroupBy(ByVal aggregate As AggregateParameter)
			MyBase.AddGroupBy(GetAggregate(aggregate, False))
		End Sub

        Public Overrides Sub AddResultColumn(ByVal columnName As String)
            MyBase.AddResultColumn("[" + columnName + "]")
        End Sub

		Protected Function GetAggregate(ByVal wItem As AggregateParameter, ByVal withAlias As Boolean) As String

			Dim query As String = String.Empty

			Select Case (wItem.Function)

				Case AggregateParameter.Func.Avg
					query += "Avg("

				Case AggregateParameter.Func.Count
					query += "Count("

				Case AggregateParameter.Func.Max
					query += "Max("

				Case AggregateParameter.Func.Min
					query += "Min("

				Case AggregateParameter.Func.Sum
					query += "Sum("

				Case AggregateParameter.Func.StdDev
					query += "StDev("

				Case AggregateParameter.Func.Var
					query += "Var("

			End Select

			If wItem.Distinct Then
				query += "DISTINCT "
			End If

			query += "[" + wItem.Column + "])"

			If withAlias AndAlso Not wItem.Alias = String.Empty Then
				query += " AS [" + wItem.Alias + "]"
			End If

			Return query

		End Function

        Protected Overrides Function _Load(Optional ByVal conjuction As String = "AND") As IDbCommand

			Dim hasColumn As Boolean = False
			Dim selectAll As Boolean = True
			Dim query As String
			Dim p As Integer = 1

			query = "SELECT "

			If Me._distinct Then query += " DISTINCT "
			If Me._top >= 0 Then query += " TOP " + Me._top.ToString() + " "

			If Me._resultColumns.Length > 0 Then
				query += Me._resultColumns
				hasColumn = True
				selectAll = False
			End If

			If Me._countAll Then

				If hasColumn Then

					query += ", "
				End If

				query += "COUNT(*)"

				If Not Me._countAllAlias = String.Empty Then
					' Need DBMS string delimiter here
					query += " AS [" + Me._countAllAlias + "]"
				End If

				hasColumn = True
				selectAll = False
			End If

			If Not _aggregateParameters Is Nothing AndAlso _aggregateParameters.Count > 0 Then
				Dim isFirst As Boolean = True

				If hasColumn Then
					query += ", "
				End If

				Dim wItem As AggregateParameter
				Dim obj As Object

				For Each obj In _aggregateParameters

					wItem = CType(obj, AggregateParameter)

					If wItem.IsDirty Then

						If isFirst Then
							query += GetAggregate(wItem, True)
							isFirst = False
						Else
							query += ", " + GetAggregate(wItem, True)
						End If
					End If
				Next

				selectAll = False
			End If

			If selectAll Then
				query += "*"
			End If

			query += " FROM [" + Me._entity.QuerySource() + "]"

			Dim cmd As New OleDbCommand

			If Not _whereParameters Is Nothing AndAlso _whereParameters.Count > 0 Then

				query += " WHERE "

				Dim first As Boolean = True

				Dim requiresParam As Boolean

				Dim obj As Object
				Dim text As String
				Dim wItem As WhereParameter
				Dim skipConjuction As Boolean = False

				Dim betweenParamRand As Integer = 1
				Dim betweenParamBegin As String
				Dim betweenParamEnd As String

				Dim qCount As Integer = _whereParameters.Count - 1
				Dim index As Integer = 0

				For index = 0 To qCount

					' Maybe we injected text or a WhereParameter
					obj = _whereParameters(index)

					If TypeOf obj Is String Then

						text = CType(obj, String)
						query += text

						If text = "(" Then
							skipConjuction = True
						End If

					Else

						wItem = CType(obj, WhereParameter)

						If wItem.IsDirty Then

							If Not first AndAlso Not skipConjuction Then

								If Not wItem.Conjuction = WhereParameter.Conj.UseDefault Then
									If wItem.Conjuction = WhereParameter.Conj.AND_ Then
										query += " AND "
									Else
										query += " OR "
									End If
								Else
									query += " " + conjuction + " "
								End If

							End If

							requiresParam = True

							Select Case wItem.[Operator]
								Case WhereParameter.Operand.Equal
									query += "[" + wItem.Column + "] = ?"
								Case WhereParameter.Operand.NotEqual
									query += "[" + wItem.Column + "] <> ?"
								Case WhereParameter.Operand.GreaterThan
									query += "[" + wItem.Column + "] > ?"
								Case WhereParameter.Operand.LessThan
									query += "[" + wItem.Column + "] < ?"
								Case WhereParameter.Operand.LessThanOrEqual
									query += "[" + wItem.Column + "] <=?"
								Case WhereParameter.Operand.GreaterThanOrEqual
									query += "[" + wItem.Column + "] >= ?"
								Case WhereParameter.Operand.Like_
									query += "[" + wItem.Column + "] LIKE ?"
								Case WhereParameter.Operand.NotLike
									query += "[" + wItem.Column + "] NOT LIKE ?"
								Case WhereParameter.Operand.IsNull
									query += "[" + wItem.Column + "] IS NULL"
									requiresParam = False
								Case WhereParameter.Operand.IsNotNull
									query += "[" + wItem.Column + "] IS NOT NULL"
									requiresParam = False
								Case WhereParameter.Operand.In_
									query += "[" + wItem.Column + "] IN (" + wItem.Value.ToString() + ") "
									requiresParam = False
								Case WhereParameter.Operand.NotIn
									query += "[" + wItem.Column + "] NOT IN (" + wItem.Value.ToString() + ") "
									requiresParam = False
								Case WhereParameter.Operand.Between
									betweenParamBegin = "@PP" + betweenParamRand.ToString()
									betweenParamRand += 1
									betweenParamEnd = "@PP" + betweenParamRand.ToString()

									query += "[" + wItem.Column + "] BETWEEN ? AND ?"
									Me.AddParameter(cmd, betweenParamBegin, wItem.BetweenBeginValue)
									Me.AddParameter(cmd, betweenParamEnd, wItem.BetweenEndValue)
									betweenParamRand += 1
									requiresParam = False
							End Select

							If requiresParam Then
								Dim dbCmd As IDbCommand = CType(cmd, IDbCommand)
								cmd.Parameters.Add(wItem.Param)
								wItem.Param.Value = wItem.Value
								p += 1
							End If

							first = False
							skipConjuction = False

						End If				   ' If wItem.IsDirty Then

					End If				' Is WhereParameter

				Next index

			End If

			If _groupBy.Length > 0 Then
				query += " GROUP BY " + _groupBy

				If Me._withRollup Then
					query += " WITH ROLLUP"
				End If
			End If

			If (_orderBy.Length > 0) Then
				query += " ORDER BY " + _orderBy
			End If

			cmd.CommandText = query
			Return cmd

		End Function

		Private Sub AddParameter(ByVal cmd As OleDbCommand, ByVal paramName As String, ByVal data As Object)
#If (VS2005) Then
			cmd.Parameters.AddWithValue(paramName, data)
#Else
			cmd.Parameters.Add(paramName, data)
#End If
		End Sub

    End Class

End Namespace
