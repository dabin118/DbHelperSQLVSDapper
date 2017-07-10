using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbHelperSQLVSDapper
{
    /// <summary>
    /// 拼接Sql Server语句
    /// </summary>
    public class SqlServerMaker
    {

        #region 实体反射 object
        

        public static string MakeInsertSql(string table, object entity, bool hasUnicode = false)
        {
            Type _entity = entity.GetType();

            string sql = null;
            string fileds = null;
            string values = null;

            foreach (var pi in _entity.GetProperties())
            {
                if (pi.Name != "ID" && pi.CanRead && pi.CanWrite)
                {
                    fileds += pi.Name + ",";
                    values += FieldNeedQuote(pi.GetValue(entity, null), hasUnicode) + ",";
                }
            }

            sql += "insert into " + table + "(";
            sql += fileds.TrimEnd(',');
            sql += ") values(";
            sql += values.TrimEnd(',');
            sql += ") ";
            return sql;
        }

        public static string MakeUpdateSql(string table, object entity, string where = null, bool hasUnicode = false)
        {
            Type _entity = entity.GetType();
            string sql = null;
            int id = 0;
            sql = "update " + table + " set ";
            foreach (var pi in _entity.GetProperties())
            {
                if (pi.Name == "ID")
                { id = Convert.ToInt32(pi.GetValue(entity, null)); }
                else if (pi.CanRead && pi.CanWrite)
                { sql += pi.Name + "=" + FieldNeedQuote(pi.GetValue(entity, null), hasUnicode) + ","; }
            }

            sql = sql.TrimEnd(',');

            if (string.IsNullOrEmpty(where)) { sql += " where ID=" + id; }
            else { sql += " where " + where; }

            return sql;
        }

        private static string FieldNeedQuote(Type type, object value, bool hasUnicode = false)
        {
            string ret = null;
            if (value == null) { return " null "; }
            switch (type.FullName)
            {
                case "System.Int32":
                case "System.Double":
                case "System.Decimal":
                    ret = value.ToString();
                    break;
                case "System.Boolean":
                    if (Convert.ToBoolean(value))
                        ret = "1";
                    else
                        ret = "0";
                    break;
                case "System.DateTime":
                    ret = "'" + Convert.ToString(DateTime.MinValue == Convert.ToDateTime(value) ? System.Data.SqlTypes.SqlDateTime.MinValue : value) + "'";
                    break;
                //case "System.String":
                default:
                    // 防SQL注入
                    if (string.IsNullOrEmpty(value.ToString()))
                    {
                        ret = "''";
                    }
                    else
                    {
                        if (hasUnicode) { ret = "N"; }

                        if (value.ToString().Replace("''", "").IndexOf('\'') >= 0)
                        {

                            ret += "'" + value.ToString().Replace("'", "''") + "'";
                        }
                        else
                        {
                            ret += "'" + value + "'";
                        }
                    }
                    break;
            }
            return ret;
        }

        #endregion

        #region 把对象放在IDictionary 或 Hashtable中来拼装SQL
        public static string MakeInsertSql(string tablename, Hashtable ht)
        {
            string sql = null;
            string fileds = null;
            string values = null;
            foreach (DictionaryEntry de in ht)
            {
                fileds += de.Key.ToString() + ",";
                values += FieldNeedQuote(de.Value) + ",";
            }
            sql += "insert into " + tablename + "(";
            sql += fileds.TrimEnd(',');
            sql += ") values(";
            sql += values.TrimEnd(',');
            sql += ") ";
            return sql;
        }
        public static string MakeUpdateSql(string tablename, Hashtable ht, string where)
        {
            string sql = null;
            sql = "update " + tablename + " set ";
            foreach (DictionaryEntry de in ht)
            {
                sql += de.Key + "=" + FieldNeedQuote(de.Value) + ",";
            }
            sql = sql.TrimEnd(',');
            sql += " where " + where;
            return sql;
        }

        public static string MakeInsertSql(string tablename, IDictionary dict, bool hasUnicode = false)
        {
            string sql = null;
            string fileds = null;
            string values = null;
            foreach (DictionaryEntry de in dict)
            {
                fileds += de.Key.ToString() + ",";
                values += FieldNeedQuote(de.Value, hasUnicode) + ",";
            }
            sql += "insert into " + tablename + "(";
            sql += fileds.TrimEnd(',');
            sql += ") values(";
            sql += values.TrimEnd(',');
            sql += ") ";
            return sql;
        }
        public static string MakeUpdateSql(string tablename, IDictionary dict, string where, bool hasUnicode = false)
        {
            string sql = null;
            sql = "update " + tablename + " set ";
            foreach (DictionaryEntry de in dict)
            {
                sql += de.Key + "=" + FieldNeedQuote(de.Value, hasUnicode) + ",";
            }
            sql = sql.TrimEnd(',');
            sql += " where " + where;
            return sql;
        }

        private static string FieldNeedQuote(object value, bool hasUnicode = false)
        {
            string ret = null;
            if (value == null) { return " null "; }

            switch (value.GetType().ToString())
            {
                case "System.Int32":
                case "System.Double":
                case "System.Decimal":
                    ret = value.ToString();
                    break;
                case "System.Boolean":
                    if (Convert.ToBoolean(value))
                        ret = "1";
                    else
                        ret = "0";
                    break;
                case "System.DateTime":
                    ret = "'" + Convert.ToString(DateTime.MinValue == Convert.ToDateTime(value) ? System.Data.SqlTypes.SqlDateTime.MinValue : value) + "'";
                    break;
                //case "System.String":
                default:
                    // 防SQL注入
                    if (string.IsNullOrEmpty(value.ToString()))
                    {
                        ret = "''";
                    }
                    else
                    {
                        if (hasUnicode) { ret = "N"; }

                        if (value.ToString().Replace("''", "").IndexOf('\'') >= 0)
                        {

                            ret += "'" + value.ToString().Replace("'", "''") + "'";
                        }
                        else
                        {
                            ret += "'" + value + "'";
                        }
                    }
                    break;
            }
            return ret;
        }

        #endregion


    }
}
