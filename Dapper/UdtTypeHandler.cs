﻿using System;
using System.Data;
using System.Data.SqlClient;

namespace Dapper
{
    partial class SqlMapper
    {
#if !DNXCORE50
        /// <summary>
        /// A type handler for data-types that are supported by the underlying provider, but which need
        /// a well-known UdtTypeName to be specified
        /// </summary>
        public class UdtTypeHandler : ITypeHandler
        {
            private readonly string udtTypeName;

            /// <summary>
            /// Creates a new instance of UdtTypeHandler with the specified UdtTypeName
            /// </summary>
            public UdtTypeHandler(string udtTypeName)
            {
                if (string.IsNullOrEmpty(udtTypeName))
                {
                    throw new ArgumentException("Cannot be null or empty", udtTypeName);
                }
                this.udtTypeName = udtTypeName;
            }

            object ITypeHandler.Parse(Type destinationType, object value)
            {
                return value is DBNull ? null : value;
            }

            void ITypeHandler.SetValue(IDbDataParameter parameter, object value)
            {
                parameter.Value = SanitizeParameterValue(value);
                if (parameter is SqlParameter && !(value is DBNull))
                {
                    ((SqlParameter)parameter).SqlDbType = SqlDbType.Udt;
                    ((SqlParameter)parameter).UdtTypeName = udtTypeName;
                }
            }
        }
#endif
    }
}
