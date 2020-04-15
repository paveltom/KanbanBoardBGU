﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class BoardService
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        private BusinessLayer.SecurityController SecurityController;

        public BoardService(BusinessLayer.SecurityController sc)
        {
            log.Debug("BoardService Created");
            SecurityController = sc;
        }


        public Response<Board> GetBoard(string email) //done+++++++++++++++++++++++++++++++++++++++++++
        {
            if (!SecurityController.UserValidation(email))
            {
                Response<Board> resp = new Response<Board>("Invailid current user.");
                log.Error(resp.ErrorMessage);
                return resp;
            }
            List<string> tempColumnNames = SecurityController.BoardController.GetBoard(email).getColumnNames();
            Board tempStructBoard = new Board(tempColumnNames);
            log.Debug("Board Reached Service Layer Seccessfully");
            return new Response<Board>(tempStructBoard);
        }



        public Response LimitColumnTasks(string email, int columnOrdinal, int limit) //done+++++++++++++++++++++++++++++++++++++++++++
        {
            if (!SecurityController.UserValidation(email))
            {
                Response resp = new Response("Invalid current user.");
                log.Error(resp.ErrorMessage);
                return resp;
            }
            try
            {
                SecurityController.BoardController.LimitColumnTask(email, columnOrdinal, limit);
                log.Info("Column limit has been updated successfully.");
                return new Response("Column limit has been updated successfully.");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response(ex.Message);
            }

        }



        public Response<Task> AddTask(string email, string title, string description, DateTime dueDate) //done++++++++++++++++++++++++++++++++++++++
        {
            if (!SecurityController.UserValidation(email)) return new Response<Task>("Invalid current user.");
            try
            {
                BusinessLayer.BoardPackage.Task tempTask = SecurityController.BoardController.AddTask(email, title, description, dueDate);
                Task tempStructTask = new Task(tempTask.Id, tempTask.CreationTime, title, description, dueDate);
                log.Info("Task added seccessfully.");
                return new Response<Task>(tempStructTask, "Task has been added successfully.");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response<Task>(ex.Message);
            }
        }



        public Response UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime newDueDate) //done++++++++++++++++++++++++++++++++++++++
        {
            if (!SecurityController.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                SecurityController.BoardController.UpdateTaskDueDate(email, columnOrdinal, taskId, newDueDate);
                log.Info("Task doudate updated seccessfully.");
                return new Response("Task due date has benn updated successfully.");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response(ex.Message);
            }
        }



        public Response UpdateTaskTitle(string email, int columnOrdinal, int taskId, string newTitle) //done++++++++++++++++++++++++++++++++++++++
        {
            if (!SecurityController.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                SecurityController.BoardController.UpdateTaskTitle(email, columnOrdinal, taskId, newTitle);
                log.Info("Task title updated seccessfully.");
                return new Response("Task title has been updated successfully");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message,ex);
                return new Response(ex.Message);
            }
        }



        public Response UpdateTaskDescription(string email, int columnOrdinal, int taskId, string newDescription) //done+++++++++++++++++++++++++++++
        {
            if (!SecurityController.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                SecurityController.BoardController.UpdateTaskDescription(email, columnOrdinal, taskId, newDescription);
                log.Info("Task description updated seccessfully.");
                return new Response("Task description has been updated successfully");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response(ex.Message);
            }
        }



        public Response AdvanceTask(string email, int columnOrdinal, int taskId) //done+++++++++++++++++++++++++++++
        {
            if (!SecurityController.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                SecurityController.BoardController.AdvanceTask(email, columnOrdinal, taskId);
                log.Info("Task Advanced to column" + columnOrdinal+1);
                return new Response("Task has been advanced successfully");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response(ex.Message);
            }
        }



        public Response<Column> GetColumn(string email, string columnName) ///done++++++++++++++++++++++++++++++++++++++
        {
            if (!SecurityController.UserValidation(email)) return new Response<Column>("Invalid current user.");
            try
            {
                //declaring BL column by receiving existing column from BL.BoardPackage
                BusinessLayer.BoardPackage.Column tempColumn = SecurityController.BoardController.GetColumn(email, columnName);
                //declaring List of BL.Tasks by receiving it from 'tempColumn'
                List<BusinessLayer.BoardPackage.Task> tempColumnTaskCollection = tempColumn.Tasks;

                //declaring List of struct Tasks
                List<Task> structTaskList = new List<Task>();

                //converting BL.Tasks of 'tempColumnTaskCollection' into struct Users and adding them to 'structTaskList'
                foreach (BusinessLayer.BoardPackage.Task tempTask in tempColumnTaskCollection)
                    structTaskList.Add(new Task(tempTask.Id, tempTask.CreationTime, tempTask.Title, tempTask.Description, tempTask.DueDate));

                //declaring ReadOnlyCollection by using its copying constructor with List of struct Users
                IReadOnlyCollection<Task> tempReadOnlyStructTaskList = new ReadOnlyCollection<Task>(structTaskList);

                //declaring struct Column with ReadOnlyCollection of struct Tasks
                Column tempStructColumn = new Column(tempReadOnlyStructTaskList, tempColumn.Name, tempColumn.Limit);

                log.Debug("Desired Column Reached Service Layer");
                return new Response<Column>(tempStructColumn);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response<Column>(ex.Message);
            }
        }



        public Response<Column> GetColumn(string email, int columnOrdinal) //done++++++++++++++++++++++++++++++++++++++
        {
            //this method replicates GetColumn(string email, string columnName), with only difference of calling BL.BC.GetColumn() with columnOrdinal.
            if (!SecurityController.UserValidation(email)) return new Response<Column>("Invalid current user.");
            try
            {
                BusinessLayer.BoardPackage.Column tempColumn = SecurityController.BoardController.GetColumn(email, columnOrdinal);
                List<BusinessLayer.BoardPackage.Task> tempColumnTaskCollection = tempColumn.Tasks;

                List<Task> structTaskList = new List<Task>();

                foreach (BusinessLayer.BoardPackage.Task tempTask in tempColumnTaskCollection)
                    structTaskList.Add(new Task(tempTask.Id, tempTask.CreationTime, tempTask.Title, tempTask.Description, tempTask.DueDate));

                IReadOnlyCollection<Task> tempReadOnlyStructTaskList = new ReadOnlyCollection<Task>(structTaskList);

                Column tempStructColumn = new Column(tempReadOnlyStructTaskList, tempColumn.Name, tempColumn.Limit);
                log.Debug("Desired Column Reached Service Layer");
                return new Response<Column>(tempStructColumn);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response<Column>(ex.Message);
            }
        }
    }
}
