using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Plugin
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IResultColumns))]
#endif 
	public class PluginResultColumns : ResultColumns
    {
        private IMyMetaPlugin plugin;

        public PluginResultColumns(IMyMetaPlugin plugin)
        {
            this.plugin = plugin;
		}

		override internal void LoadAll()
        {
            DataTable metaData = this.plugin.GetProcedureResultColumns(this.Procedure.Database.Name, this.Procedure.Name);
            this.PopulateArray(metaData);
		}

        #region DataColumn Binding Stuff

        internal DataColumn f_Name = null;
        internal DataColumn f_Ordinal = null;
        internal DataColumn f_DataType = null;
        internal DataColumn f_DataTypeName = null;
        internal DataColumn f_DataTypeNameComplete = null;


        private void BindToColumns(DataTable metaData)
        {
            if (false == _fieldsBound)
            {
                if (metaData.Columns.Contains("COLUMN_NAME")) f_Name = metaData.Columns["COLUMN_NAME"];
                if (metaData.Columns.Contains("ORDINAL_POSITION")) f_Ordinal = metaData.Columns["ORDINAL_POSITION"];
                if (metaData.Columns.Contains("DATA_TYPE")) f_DataType = metaData.Columns["DATA_TYPE"];
                if (metaData.Columns.Contains("DATA_TYPE_NAME")) f_DataType = metaData.Columns["DATA_TYPE_NAME"];
                if (metaData.Columns.Contains("DATA_TYPE_NAME_COMPLETE")) f_DataType = metaData.Columns["DATA_TYPE_NAME_COMPLETE"];
            }
        }

        internal void PopulateArray(DataTable metaData)
        {
            BindToColumns(metaData);

            ResultColumn column = null;

            if (metaData.DefaultView.Count > 0)
            {
                IEnumerator enumerator = metaData.DefaultView.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DataRowView rowView = enumerator.Current as DataRowView;

                    column = this.dbRoot.ClassFactory.CreateResultColumn() as ResultColumn;
                    column.dbRoot = this.dbRoot;
                    column.Row = rowView.Row;
                    this._array.Add(column);
                }
            }
        }
        #endregion

	}
}
