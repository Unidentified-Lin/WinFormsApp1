using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace WinFormsLibrary1.Extensions
{
    public static class DataGridViewExtension
    {
        /// <summary>
        /// 動態生成DataGridView對應欄位型別
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="valueType">欄位型別</param>
        /// <param name="propName">資料屬性名稱</param>
        /// <param name="isReadOnly">是否唯獨</param>
        /// <param name="colName">欄位名稱</param>
        public static void AddColumn(this DataGridView dgv, Type valueType, string propName, bool isReadOnly, string colName)
        {
            DataGridViewColumn column;
            if (valueType.IsEnum)
            {
                column = new DataGridViewComboBoxColumn();
                ((DataGridViewComboBoxColumn)column).DataSource = Enum.GetValues(valueType);
            }
            else
            {
                column = valueType.Equals(typeof(bool)) ? new DataGridViewCheckBoxColumn() : new DataGridViewTextBoxColumn();
            }

            column.ValueType = valueType;
            column.Name = colName;
            column.DataPropertyName = propName;
            column.ReadOnly = isReadOnly;

            dgv.Columns.Add(column);
        }

        /// <summary>
        /// 動態生成DataGridView欄位
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="type">使用類別</param>
        public static void AddColumns(this DataGridView dgv, Type type)
        {
            var propInfos = type.GetProperties();
            foreach (var propInfo in propInfos)
            {
                if (propInfo is null)
                {
                    throw new ArgumentException($"No accessible {propInfo.Name} property was found in the {type.Name} type.");
                }
                var browsables = (BrowsableAttribute[])propInfo.GetCustomAttributes(typeof(BrowsableAttribute), false);
                if (browsables.Length > 0 && !browsables[0].Browsable)
                {
                    throw new ArgumentException($"The {propInfo.Name} property has a Browsable(false) attribute, and therefore cannot be bound.");
                }
                dgv.AddColumn(propInfo.PropertyType, propInfo.Name, true, propInfo.Name);
            }

        }

        /// <summary>
        /// DataGridView動態生成物件對應欄位，並將物件資料陣列加入DataSource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dgv"></param>
        /// <param name="dataObjs">物件資料陣列</param>
        /// <param name="isAppend">是否新增(預設刷新)</param>
        public static void AddObjDatas<T>(this DataGridView dgv, T[] dataObjs, bool isAppend = false)
        {
            if (dgv.Columns.Count.Equals(0))
            {
                dgv.AddColumns(typeof(T));
            }

            var list = new List<T>();
            if (isAppend && dgv.DataSource is not null)
            {
                list.AddRange((T[])dgv.DataSource);
            }
            list.AddRange(dataObjs);
            dgv.DataSource = list;
        }
    }
}
