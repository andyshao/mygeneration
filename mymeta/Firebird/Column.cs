using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace MyMeta.Firebird
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IColumn))]
#endif 
	public class FirebirdColumn : Column
	{
		public FirebirdColumn()
		{

		}

		override internal Column Clone()
		{
			Column c = base.Clone();

			return c;
		}

		override public System.Boolean IsAutoKey
		{
			get
			{
				if(null != this.Table)
				{
					if(this.Table.Properties.ContainsKey("GEN:I:" + this.Name) ||
					   this.Table.Properties.ContainsKey("GEN:I:T:" + this.Name))
					{
						return true;
					}
				}

				return false;
			}
		}

        public override int CharacterOctetLength
        {
            get
            {
                if (this.DataTypeName.StartsWith("int", StringComparison.CurrentCultureIgnoreCase) ||
                    this.DataTypeName.StartsWith("smallint", StringComparison.CurrentCultureIgnoreCase) ||
                    this.DataTypeName.StartsWith("double", StringComparison.CurrentCultureIgnoreCase) ||
                    this.DataTypeName.StartsWith("float", StringComparison.CurrentCultureIgnoreCase) ||
                    this.DataTypeName.StartsWith("bigint", StringComparison.CurrentCultureIgnoreCase) ||
                    this.DataTypeName.StartsWith("blob", StringComparison.CurrentCultureIgnoreCase) ||
                    this.DataTypeName.StartsWith("timestamp", StringComparison.CurrentCultureIgnoreCase))
				{
                    return (int)this._row["COLUMN_SIZE"];
                }
                else
                {
                    return (int)base.CharacterOctetLength;
                }
            }
        }

		override public System.Int32 CharacterMaxLength
		{
			get
			{
                if (DataTypeName.StartsWith("blob", StringComparison.CurrentCultureIgnoreCase))
                {
                    return (int)this._row["COLUMN_SIZE"];
                }
                else if (DataTypeName.StartsWith("char", StringComparison.CurrentCultureIgnoreCase) ||
                    DataTypeName.StartsWith("varchar", StringComparison.CurrentCultureIgnoreCase))
                {
                    return this.CharacterOctetLength;
                }
                else
                {
                    return 0;
                }
			}
		}

		override public System.Int32 NumericPrecision
		{
			get
			{
                if (this.DataTypeName.StartsWith("int", StringComparison.CurrentCultureIgnoreCase) ||
                    this.DataTypeName.StartsWith("smallint", StringComparison.CurrentCultureIgnoreCase) ||
                    this.DataTypeName.StartsWith("double", StringComparison.CurrentCultureIgnoreCase) ||
                    this.DataTypeName.StartsWith("float", StringComparison.CurrentCultureIgnoreCase) ||
                    this.DataTypeName.StartsWith("bigint", StringComparison.CurrentCultureIgnoreCase) ||
                    this.DataTypeName.StartsWith("blob", StringComparison.CurrentCultureIgnoreCase) ||
                    this.DataTypeName.StartsWith("timestamp", StringComparison.CurrentCultureIgnoreCase))
                {
                    return (int)this._row["COLUMN_SIZE"];
                }
                else
                {
                    return this.GetInt32(Columns.f_NumericScale);
                }
			}
		}

		override public string DataTypeName
		{
			get
			{
				if(this.dbRoot.DomainOverride)
				{
					if(this.HasDomain)
					{
						if(this.Domain != null)
						{
							return this.Domain.DataTypeName;
						}
					}
				}

				FirebirdColumns cols = Columns as FirebirdColumns;
				return this.GetString(cols.f_TypeName);
			}
		}

		override public string DataTypeNameComplete
		{
			get
			{
				if(this.dbRoot.DomainOverride)
				{
					if(this.HasDomain)
					{
						if(this.Domain != null)
						{
							return this.Domain.DataTypeNameComplete;
						}
					}
				}

				FirebirdColumns cols = Columns as FirebirdColumns;
				return this.GetString(cols.f_TypeNameComplete);
			}
		}
	}
}
