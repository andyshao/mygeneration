using System;

using MyMeta;

namespace MyMeta.VistaDB
{
#if ENTERPRISE
	using System.EnterpriseServices;
	using System.Runtime.InteropServices;
	[ComVisible(false)]
#endif
	public class ClassFactory : IClassFactory
	{
		public ClassFactory()
		{

		}

		public ITables CreateTables()
		{
			return new VistaDB.VistaDBTables();
		}

		public ITable CreateTable()
		{
			return new VistaDB.VistaDBTable();
		}

		public IColumn CreateColumn()
		{
			return new VistaDB.VistaDBColumn();
		}

		public IColumns CreateColumns()
		{
			return new VistaDB.VistaDBColumns();
		}

		public IDatabase CreateDatabase()
		{
			return new VistaDB.VistaDBDatabase();
		}

		public IDatabases CreateDatabases()
		{
			return new VistaDB.VistaDBDatabases();
		}

		public IProcedure CreateProcedure()
		{
			return new VistaDB.VistaDBProcedure();
		}

		public IProcedures CreateProcedures()
		{
			return new VistaDB.VistaDBProcedures();
		}

		public IView CreateView()
		{
			return new VistaDB.VistaDBView();
		}

		public IViews CreateViews()
		{
			return new VistaDB.VistaDBViews();
		}

		public IParameter CreateParameter()
		{
			return new VistaDB.VistaDBParameter();
		}

		public IParameters CreateParameters()
		{
			return new VistaDB.VistaDBParameters();
		}

		public IForeignKey CreateForeignKey()
		{
			return new VistaDB.VistaDBForeignKey();
		}

		public IForeignKeys CreateForeignKeys()
		{
			return new VistaDB.VistaDBForeignKeys();
		}

		public IIndex CreateIndex()
		{
			return new VistaDB.VistaDBIndex();
		}

		public IIndexes CreateIndexes()
		{
			return new VistaDB.VistaDBIndexes();
		}

		public IDomain CreateDomain()
		{
			return new VistaDBDomain();
		}

		public IDomains CreateDomains()
		{
			return new VistaDBDomains();
		}

		public IResultColumn CreateResultColumn()
		{
			return new VistaDB.VistaDBResultColumn();
		}

		public IResultColumns CreateResultColumns()
		{
			return new VistaDB.VistaDBResultColumns();
		}


		public IProviderType CreateProviderType()
		{
			return new ProviderType();
		}

		public IProviderTypes CreateProviderTypes()
		{
			return new ProviderTypes();
		}
	}
}
