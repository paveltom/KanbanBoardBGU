﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{

    /// <summary>
    ///Represents the Kanban Board
    /// </summary>
    internal class Board : PersistedObject<DataAccessLayer.Board>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public List<Column> Columns { get; }
        public string UserEmail { get; }
        public int TaskCounter { get; set; }

        public Board(string email)
        {
            UserEmail = email;
            TaskCounter = 0;
            Columns = new List<Column>();
            Columns.Add(newColumn("Backlog"));
            Columns.Add(newColumn("In Progress"));
            Columns.Add(newColumn("Done"));
            log.Info("New board created");
        }

        public Board(string email, int taskCounter, List<Column> columns)
        {
            UserEmail = email;
            TaskCounter = taskCounter;
            Columns = columns;
            log.Info("load - Board " + email + "was loaded from memory");
        }

        /// <summary>
        /// Creates a new Column and returns it.
        /// </summary>
        /// <param name="name">The name of the column to be created.</param>
        /// <returns>Returns the created Column</returns>
        private Column newColumn(string name) {
            Column newColumn = new Column(name);
            newColumn.Save("Boards\\" + UserEmail + "\\");
            return newColumn;
        }

        /// <summary>
        /// get the column the the specified name
        /// </summary>
        /// <param name="columnName">The column name to return</param>
        /// <returns></returns>
        public Column GetColumn(string columnName)
        {
            log.Debug(UserEmail + ": returned column " + columnName);
            return Columns.Find(x => x.Name.Equals(columnName));
        }
        /// <summary>
        /// get the column the the specified Index
        /// </summary>
        /// <param name="columnOrdinal">the index of the column</param>
        /// <returns></returns>
        public Column GetColumn(int columnOrdinal)
        {
            if (columnOrdinal >= Columns.Count || columnOrdinal<0)
            {
                log.Warn("columnOrdinal was out of range");
                throw new ArgumentOutOfRangeException("Column index out of range");
            }
            log.Debug(UserEmail + ": returned column no." + columnOrdinal);
               return Columns[columnOrdinal];
        }
        /// <summary>
        /// get the names of the columns as a list
        /// </summary>
        /// <returns>
        /// return a List of string with the column names
        /// </returns>
        public List<string> getColumnNames()
        {
            List<string> columnNames = new List<string>();
            foreach(Column c in Columns)
            {
                columnNames.Add(c.Name);
            }
            log.Debug("Returned column's names");
            return columnNames;
        }

        ///<inheretdoc/>
        public void Save(string path)
        {
            log.Info("Board.save was called");
            ToDalObject().Save(path);         
        }

        ///<inheretdoc/>
        public DataAccessLayer.Board ToDalObject()
        {
            log.Debug("Creating DalObject<Board>");
            List<DataAccessLayer.Column> dalColumns = new List<DataAccessLayer.Column>();
            foreach(Column c in Columns)
            {
                dalColumns.Add(c.ToDalObject());
            }
            return new DataAccessLayer.Board(UserEmail, TaskCounter, dalColumns);
        }


    }

}
