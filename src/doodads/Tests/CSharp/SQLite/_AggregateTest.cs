
/*
'===============================================================================
'  Generated From - SQLite_CSharp_BusinessEntity.vbgen
'
'  The supporting base class SQLiteEntity is in the Architecture directory in "dOOdads".
'  
'  This object is 'abstract' which means you need to inherit from it to be able
'  to instantiate it.  This is very easilly done. You can override properties and
'  methods in your derived class, this allows you to regenerate this class at any
'  time and not worry about overwriting custom code. 
'
'  NEVER EDIT THIS FILE.
'
'  public class YourObject :  _YourObject
'  {
'
'  }
'
'===============================================================================
*/

// Generated by MyGeneration Version # (1.1.3.5)

using System;
using System.Data;
using Finisar.SQLite;
using System.Collections;
using System.Collections.Specialized;

using MyGeneration.dOOdads;

namespace MyGeneration.dOOdads.Tests.SQLite
{
	public abstract class _AggregateTest : SQLiteEntity
	{
		public _AggregateTest()
		{
			this.QuerySource = "AggregateTest";
			this.MappingName = "AggregateTest";

		}	

		//=================================================================
		//  public Overrides void AddNew()
		//=================================================================
		//
		//=================================================================
		public override void AddNew()
		{
			base.AddNew();
		}
		
		public override void FlushData()
		{
			this._whereClause = null;
			this._aggregateClause = null;
			base.FlushData();
		}
		
		public override string GetAutoKeyColumns()
		{
			return "ID";
		}
		

		//=================================================================
		//  	public Function LoadAll() As Boolean
		//=================================================================
		//  Loads all of the records in the database, and sets the currentRow to the first row
		//=================================================================
		public bool LoadAll() 
		{
			return this.Query.Load();
		}
	
	
		//=================================================================
		// public Overridable Function LoadByPrimaryKey()  As Boolean
		//=================================================================
		//  Loads a single row of via the primary key
		//=================================================================
		public virtual bool LoadByPrimaryKey(long ID)
		{
			this.Where.ID.Value = ID;
			
			return this.Query.Load();
		}
		
		
		#region Parameters
		protected class Parameters
		{
			
			public static SQLiteParameter ID
			{
				get
				{
					return new SQLiteParameter("@ID", DbType.Int64);

				}
			}
			
			public static SQLiteParameter DepartmentID
			{
				get
				{
					return new SQLiteParameter("@DepartmentID", DbType.Int64);

				}
			}
			
			public static SQLiteParameter FirstName
			{
				get
				{
					return new SQLiteParameter("@FirstName", DbType.String);

				}
			}
			
			public static SQLiteParameter LastName
			{
				get
				{
					return new SQLiteParameter("@LastName", DbType.String);

				}
			}
			
			public static SQLiteParameter Age
			{
				get
				{
					return new SQLiteParameter("@Age", DbType.Int64);

				}
			}
			
			public static SQLiteParameter HireDate
			{
				get
				{
					return new SQLiteParameter("@HireDate", DbType.DateTime);

				}
			}
			
			public static SQLiteParameter Salary
			{
				get
				{
					return new SQLiteParameter("@Salary", DbType.Decimal);

				}
			}
			
			public static SQLiteParameter IsActive
			{
				get
				{
					return new SQLiteParameter("@IsActive", DbType.Boolean);

				}
			}
			
		}
		#endregion		
	
		#region ColumnNames
		public class ColumnNames
		{  
            public const string ID = "ID";
            public const string DepartmentID = "DepartmentID";
            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string Age = "Age";
            public const string HireDate = "HireDate";
            public const string Salary = "Salary";
            public const string IsActive = "IsActive";

			static public string ToPropertyName(string columnName)
			{
				if(ht == null)
				{
					ht = new Hashtable();
					
					ht[ID] = _AggregateTest.PropertyNames.ID;
					ht[DepartmentID] = _AggregateTest.PropertyNames.DepartmentID;
					ht[FirstName] = _AggregateTest.PropertyNames.FirstName;
					ht[LastName] = _AggregateTest.PropertyNames.LastName;
					ht[Age] = _AggregateTest.PropertyNames.Age;
					ht[HireDate] = _AggregateTest.PropertyNames.HireDate;
					ht[Salary] = _AggregateTest.PropertyNames.Salary;
					ht[IsActive] = _AggregateTest.PropertyNames.IsActive;

				}
				return (string)ht[columnName];
			}

			static private Hashtable ht = null;			 
		}
		#endregion
		
		#region PropertyNames
		public class PropertyNames
		{  
            public const string ID = "ID";
            public const string DepartmentID = "DepartmentID";
            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string Age = "Age";
            public const string HireDate = "HireDate";
            public const string Salary = "Salary";
            public const string IsActive = "IsActive";

			static public string ToColumnName(string propertyName)
			{
				if(ht == null)
				{
					ht = new Hashtable();
					
					ht[ID] = _AggregateTest.ColumnNames.ID;
					ht[DepartmentID] = _AggregateTest.ColumnNames.DepartmentID;
					ht[FirstName] = _AggregateTest.ColumnNames.FirstName;
					ht[LastName] = _AggregateTest.ColumnNames.LastName;
					ht[Age] = _AggregateTest.ColumnNames.Age;
					ht[HireDate] = _AggregateTest.ColumnNames.HireDate;
					ht[Salary] = _AggregateTest.ColumnNames.Salary;
					ht[IsActive] = _AggregateTest.ColumnNames.IsActive;

				}
				return (string)ht[propertyName];
			}

			static private Hashtable ht = null;			 
		}			 
		#endregion	

		#region StringPropertyNames
		public class StringPropertyNames
		{  
            public const string ID = "s_ID";
            public const string DepartmentID = "s_DepartmentID";
            public const string FirstName = "s_FirstName";
            public const string LastName = "s_LastName";
            public const string Age = "s_Age";
            public const string HireDate = "s_HireDate";
            public const string Salary = "s_Salary";
            public const string IsActive = "s_IsActive";

		}
		#endregion		
		
		#region Properties
	
		public virtual long ID
	    {
			get
	        {
				return base.Getlong(ColumnNames.ID);
			}
			set
	        {
				base.Setlong(ColumnNames.ID, value);
			}
		}

		public virtual long DepartmentID
	    {
			get
	        {
				return base.Getlong(ColumnNames.DepartmentID);
			}
			set
	        {
				base.Setlong(ColumnNames.DepartmentID, value);
			}
		}

		public virtual string FirstName
	    {
			get
	        {
				return base.Getstring(ColumnNames.FirstName);
			}
			set
	        {
				base.Setstring(ColumnNames.FirstName, value);
			}
		}

		public virtual string LastName
	    {
			get
	        {
				return base.Getstring(ColumnNames.LastName);
			}
			set
	        {
				base.Setstring(ColumnNames.LastName, value);
			}
		}

		public virtual long Age
	    {
			get
	        {
				return base.Getlong(ColumnNames.Age);
			}
			set
	        {
				base.Setlong(ColumnNames.Age, value);
			}
		}

		public virtual DateTime HireDate
	    {
			get
	        {
				return base.GetDateTime(ColumnNames.HireDate);
			}
			set
	        {
				base.SetDateTime(ColumnNames.HireDate, value);
			}
		}

		public virtual decimal Salary
	    {
			get
	        {
				return base.Getdecimal(ColumnNames.Salary);
			}
			set
	        {
				base.Setdecimal(ColumnNames.Salary, value);
			}
		}

		public virtual bool IsActive
	    {
			get
	        {
				return base.Getbool(ColumnNames.IsActive);
			}
			set
	        {
				base.Setbool(ColumnNames.IsActive, value);
			}
		}


		#endregion
		
		#region String Properties
	
		public virtual string s_ID
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.ID) ? string.Empty : base.GetlongAsString(ColumnNames.ID);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.ID);
				else
					this.ID = base.SetlongAsString(ColumnNames.ID, value);
			}
		}

		public virtual string s_DepartmentID
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.DepartmentID) ? string.Empty : base.GetlongAsString(ColumnNames.DepartmentID);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.DepartmentID);
				else
					this.DepartmentID = base.SetlongAsString(ColumnNames.DepartmentID, value);
			}
		}

		public virtual string s_FirstName
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.FirstName) ? string.Empty : base.GetstringAsString(ColumnNames.FirstName);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.FirstName);
				else
					this.FirstName = base.SetstringAsString(ColumnNames.FirstName, value);
			}
		}

		public virtual string s_LastName
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.LastName) ? string.Empty : base.GetstringAsString(ColumnNames.LastName);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.LastName);
				else
					this.LastName = base.SetstringAsString(ColumnNames.LastName, value);
			}
		}

		public virtual string s_Age
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Age) ? string.Empty : base.GetlongAsString(ColumnNames.Age);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Age);
				else
					this.Age = base.SetlongAsString(ColumnNames.Age, value);
			}
		}

		public virtual string s_HireDate
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.HireDate) ? string.Empty : base.GetDateTimeAsString(ColumnNames.HireDate);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.HireDate);
				else
					this.HireDate = base.SetDateTimeAsString(ColumnNames.HireDate, value);
			}
		}

		public virtual string s_Salary
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.Salary) ? string.Empty : base.GetdecimalAsString(ColumnNames.Salary);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.Salary);
				else
					this.Salary = base.SetdecimalAsString(ColumnNames.Salary, value);
			}
		}

		public virtual string s_IsActive
	    {
			get
	        {
				return this.IsColumnNull(ColumnNames.IsActive) ? string.Empty : base.GetboolAsString(ColumnNames.IsActive);
			}
			set
	        {
				if(string.Empty == value)
					this.SetColumnNull(ColumnNames.IsActive);
				else
					this.IsActive = base.SetboolAsString(ColumnNames.IsActive, value);
			}
		}


		#endregion		
	
		#region Where Clause
		public class WhereClause
		{
			public WhereClause(BusinessEntity entity)
			{
				this._entity = entity;
			}
			
			public TearOffWhereParameter TearOff
			{
				get
				{
					if(_tearOff == null)
					{
						_tearOff = new TearOffWhereParameter(this);
					}

					return _tearOff;
				}
			}

			#region WhereParameter TearOff's
			public class TearOffWhereParameter
			{
				public TearOffWhereParameter(WhereClause clause)
				{
					this._clause = clause;
				}
				
				
				public WhereParameter ID
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.ID, Parameters.ID);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter DepartmentID
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.DepartmentID, Parameters.DepartmentID);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter FirstName
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.FirstName, Parameters.FirstName);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter LastName
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.LastName, Parameters.LastName);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Age
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Age, Parameters.Age);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter HireDate
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.HireDate, Parameters.HireDate);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter Salary
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.Salary, Parameters.Salary);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}

				public WhereParameter IsActive
				{
					get
					{
							WhereParameter where = new WhereParameter(ColumnNames.IsActive, Parameters.IsActive);
							this._clause._entity.Query.AddWhereParameter(where);
							return where;
					}
				}


				private WhereClause _clause;
			}
			#endregion
		
			public WhereParameter ID
		    {
				get
		        {
					if(_ID_W == null)
	        	    {
						_ID_W = TearOff.ID;
					}
					return _ID_W;
				}
			}

			public WhereParameter DepartmentID
		    {
				get
		        {
					if(_DepartmentID_W == null)
	        	    {
						_DepartmentID_W = TearOff.DepartmentID;
					}
					return _DepartmentID_W;
				}
			}

			public WhereParameter FirstName
		    {
				get
		        {
					if(_FirstName_W == null)
	        	    {
						_FirstName_W = TearOff.FirstName;
					}
					return _FirstName_W;
				}
			}

			public WhereParameter LastName
		    {
				get
		        {
					if(_LastName_W == null)
	        	    {
						_LastName_W = TearOff.LastName;
					}
					return _LastName_W;
				}
			}

			public WhereParameter Age
		    {
				get
		        {
					if(_Age_W == null)
	        	    {
						_Age_W = TearOff.Age;
					}
					return _Age_W;
				}
			}

			public WhereParameter HireDate
		    {
				get
		        {
					if(_HireDate_W == null)
	        	    {
						_HireDate_W = TearOff.HireDate;
					}
					return _HireDate_W;
				}
			}

			public WhereParameter Salary
		    {
				get
		        {
					if(_Salary_W == null)
	        	    {
						_Salary_W = TearOff.Salary;
					}
					return _Salary_W;
				}
			}

			public WhereParameter IsActive
		    {
				get
		        {
					if(_IsActive_W == null)
	        	    {
						_IsActive_W = TearOff.IsActive;
					}
					return _IsActive_W;
				}
			}

			private WhereParameter _ID_W = null;
			private WhereParameter _DepartmentID_W = null;
			private WhereParameter _FirstName_W = null;
			private WhereParameter _LastName_W = null;
			private WhereParameter _Age_W = null;
			private WhereParameter _HireDate_W = null;
			private WhereParameter _Salary_W = null;
			private WhereParameter _IsActive_W = null;

			public void WhereClauseReset()
			{
				_ID_W = null;
				_DepartmentID_W = null;
				_FirstName_W = null;
				_LastName_W = null;
				_Age_W = null;
				_HireDate_W = null;
				_Salary_W = null;
				_IsActive_W = null;

				this._entity.Query.FlushWhereParameters();

			}
	
			private BusinessEntity _entity;
			private TearOffWhereParameter _tearOff;
			
		}
	
		public WhereClause Where
		{
			get
			{
				if(_whereClause == null)
				{
					_whereClause = new WhereClause(this);
				}
		
				return _whereClause;
			}
		}
		
		private WhereClause _whereClause = null;	
		#endregion
		
		#region Aggregate Clause
		public class AggregateClause
		{
			public AggregateClause(BusinessEntity entity)
			{
				this._entity = entity;
			}
			
			public TearOffAggregateParameter TearOff
			{
				get
				{
					if(_tearOff == null)
					{
						_tearOff = new TearOffAggregateParameter(this);
					}

					return _tearOff;
				}
			}

			#region AggregateParameter TearOff's
			public class TearOffAggregateParameter
			{
				public TearOffAggregateParameter(AggregateClause clause)
				{
					this._clause = clause;
				}
				
				
				public AggregateParameter ID
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.ID, Parameters.ID);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter DepartmentID
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.DepartmentID, Parameters.DepartmentID);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter FirstName
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.FirstName, Parameters.FirstName);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter LastName
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.LastName, Parameters.LastName);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Age
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Age, Parameters.Age);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter HireDate
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.HireDate, Parameters.HireDate);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter Salary
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.Salary, Parameters.Salary);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}

				public AggregateParameter IsActive
				{
					get
					{
							AggregateParameter aggregate = new AggregateParameter(ColumnNames.IsActive, Parameters.IsActive);
							this._clause._entity.Query.AddAggregateParameter(aggregate);
							return aggregate;
					}
				}


				private AggregateClause _clause;
			}
			#endregion
		
			public AggregateParameter ID
		    {
				get
		        {
					if(_ID_W == null)
	        	    {
						_ID_W = TearOff.ID;
					}
					return _ID_W;
				}
			}

			public AggregateParameter DepartmentID
		    {
				get
		        {
					if(_DepartmentID_W == null)
	        	    {
						_DepartmentID_W = TearOff.DepartmentID;
					}
					return _DepartmentID_W;
				}
			}

			public AggregateParameter FirstName
		    {
				get
		        {
					if(_FirstName_W == null)
	        	    {
						_FirstName_W = TearOff.FirstName;
					}
					return _FirstName_W;
				}
			}

			public AggregateParameter LastName
		    {
				get
		        {
					if(_LastName_W == null)
	        	    {
						_LastName_W = TearOff.LastName;
					}
					return _LastName_W;
				}
			}

			public AggregateParameter Age
		    {
				get
		        {
					if(_Age_W == null)
	        	    {
						_Age_W = TearOff.Age;
					}
					return _Age_W;
				}
			}

			public AggregateParameter HireDate
		    {
				get
		        {
					if(_HireDate_W == null)
	        	    {
						_HireDate_W = TearOff.HireDate;
					}
					return _HireDate_W;
				}
			}

			public AggregateParameter Salary
		    {
				get
		        {
					if(_Salary_W == null)
	        	    {
						_Salary_W = TearOff.Salary;
					}
					return _Salary_W;
				}
			}

			public AggregateParameter IsActive
		    {
				get
		        {
					if(_IsActive_W == null)
	        	    {
						_IsActive_W = TearOff.IsActive;
					}
					return _IsActive_W;
				}
			}

			private AggregateParameter _ID_W = null;
			private AggregateParameter _DepartmentID_W = null;
			private AggregateParameter _FirstName_W = null;
			private AggregateParameter _LastName_W = null;
			private AggregateParameter _Age_W = null;
			private AggregateParameter _HireDate_W = null;
			private AggregateParameter _Salary_W = null;
			private AggregateParameter _IsActive_W = null;

			public void AggregateClauseReset()
			{
				_ID_W = null;
				_DepartmentID_W = null;
				_FirstName_W = null;
				_LastName_W = null;
				_Age_W = null;
				_HireDate_W = null;
				_Salary_W = null;
				_IsActive_W = null;

				this._entity.Query.FlushAggregateParameters();

			}
	
			private BusinessEntity _entity;
			private TearOffAggregateParameter _tearOff;
			
		}
	
		public AggregateClause Aggregate
		{
			get
			{
				if(_aggregateClause == null)
				{
					_aggregateClause = new AggregateClause(this);
				}
		
				return _aggregateClause;
			}
		}
		
		private AggregateClause _aggregateClause = null;	
		#endregion
	
			
		
		protected override IDbCommand GetInsertCommand()
		{
			SQLiteCommand cmd = new SQLiteCommand();
			cmd.CommandType = CommandType.Text;
			cmd.CommandText =
			@"INSERT INTO [AggregateTest]
			(
				[DepartmentID],
				[FirstName],
				[LastName],
				[Age],
				[HireDate],
				[Salary],
				[IsActive]
			)
			VALUES
			(
				@DepartmentID,
				@FirstName,
				@LastName,
				@Age,
				@HireDate,
				@Salary,
				@IsActive
			)";

			CreateParameters(cmd);
			return cmd;
		}
	
		protected override IDbCommand GetUpdateCommand()
		{
			SQLiteCommand cmd = new SQLiteCommand();
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = 
			@"UPDATE [AggregateTest] SET 
				[DepartmentID]=@DepartmentID,
				[FirstName]=@FirstName,
				[LastName]=@LastName,
				[Age]=@Age,
				[HireDate]=@HireDate,
				[Salary]=@Salary,
				[IsActive]=@IsActive
			WHERE
				[ID]=@ID";

			CreateParameters(cmd);
			return cmd;
		}
	
		protected override IDbCommand GetDeleteCommand()
		{
			SQLiteCommand cmd = new SQLiteCommand();
			cmd.CommandType = CommandType.Text;
			cmd.CommandText =
			@"DELETE FROM [AggregateTest] 
			WHERE
				[ID]=@ID";

	
			SQLiteParameter p;
			p = cmd.Parameters.Add(Parameters.ID);
			p.SourceColumn = ColumnNames.ID;
			p.SourceVersion = DataRowVersion.Current;

  
			return cmd;
		}
		
		private IDbCommand CreateParameters(SQLiteCommand cmd)
		{
			SQLiteParameter p;
		
			p = cmd.Parameters.Add(Parameters.ID);
			p.SourceColumn = ColumnNames.ID;
			p.SourceVersion = DataRowVersion.Current;

			p = cmd.Parameters.Add(Parameters.DepartmentID);
			p.SourceColumn = ColumnNames.DepartmentID;
			p.SourceVersion = DataRowVersion.Current;

			p = cmd.Parameters.Add(Parameters.FirstName);
			p.SourceColumn = ColumnNames.FirstName;
			p.SourceVersion = DataRowVersion.Current;

			p = cmd.Parameters.Add(Parameters.LastName);
			p.SourceColumn = ColumnNames.LastName;
			p.SourceVersion = DataRowVersion.Current;

			p = cmd.Parameters.Add(Parameters.Age);
			p.SourceColumn = ColumnNames.Age;
			p.SourceVersion = DataRowVersion.Current;

			p = cmd.Parameters.Add(Parameters.HireDate);
			p.SourceColumn = ColumnNames.HireDate;
			p.SourceVersion = DataRowVersion.Current;

			p = cmd.Parameters.Add(Parameters.Salary);
			p.SourceColumn = ColumnNames.Salary;
			p.SourceVersion = DataRowVersion.Current;

			p = cmd.Parameters.Add(Parameters.IsActive);
			p.SourceColumn = ColumnNames.IsActive;
			p.SourceVersion = DataRowVersion.Current;


			return cmd;
		}		
		
	
	}
}
