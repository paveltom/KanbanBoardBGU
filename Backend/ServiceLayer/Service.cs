﻿using System;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    /// The service for using the Kanban board.
    /// It allows executing all of the required behaviors by the Kanban board.
    /// You are not allowed (and can't due to the interfance) to change the signatures
    /// Do not add public methods\members! Your client expects to use specifically these functions.
    /// You may add private, non static fields (if needed).
    /// You are expected to implement all of the methods.
    /// Good luck.
    /// </summary>
    public class Service : IService
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        private BoardService BoardService;
        private UserService UserService;

        /// <summary>
        /// Simple public constructor.
        /// </summary>
        public Service()
        {
            log.Info("Creates a service object");
            BoardService = null;
            UserService = null;
            LoadData();
        }

        /// <summary>        
        /// Loads the data. Intended be invoked only when the program starts
        /// </summary>
        /// <returns>A response object. The response should contain an error message in case of an error.</returns>
        public Response LoadData() 
        {
            log.Debug("Attempting to load the program data.");
            if (UserService != null)
            {
                Response r = new Response();
                log.Warn("The data is already loaded.");
                return r;
            }
            try
            {
                BusinessLayer.SecurityController _securityController = new BusinessLayer.SecurityController();
                BoardService = new BoardService(_securityController);
                UserService = new UserService(_securityController);
                log.Info("The data was loaded successfully.");
                return new Response();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response(ex.Message);
            }
        }


        /// <summary>Remove all persistent data.</summary>
        /// <returns>A response object. The response should contain an error message in case of an error.</returns>
        public Response DeleteData()
        {
            log.Debug("Attempting to delete the program data.");
            if (UserService == null)
            {
                Response r = new Response();
                log.Warn("There is no data to delete.");
                return r;
            }
            try
            {
                UserService.SecurityController.DeleteData();
                log.Info("The data was deleted successfully.");
                UserService = null;
                BoardService = null;
                LoadData();
                log.Info("The data was resetted successfully.");
                return new Response();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response(ex.Message);
            }
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="email">The email address of the user to register</param>
        /// <param name="password">The password of the user to register</param>
        /// <param name="nickname">The nickname of the user to register</param>
        /// <returns>A response object. The response should contain a error message in case of an error<returns>
        public Response Register(string email, string password, string nickname)
        {
            if (email == null | password == null | nickname == null) return new Response("One of the parameters is invalid.");
            return UserService.Register(email.ToLower(), password, nickname);
        }

        /// <summary>
		/// Registers a new user and joins the user to an existing board.
		/// </summary>
		/// <param name="email">The email address of the user to register</param>
		/// <param name="password">The password of the user to register</param>
		/// <param name="nickname">The nickname of the user to register</param>
		/// <param name="emailHost">The email address of the host user which owns the board</param>
		/// <returns>A response object. The response should contain a error message in case of an error<returns>
        public Response Register(string email, string password, string nickname, string emailHost)
        {
            if(email == null | password == null | nickname == null | emailHost == null) return new Response("One of the parameters is invalid.");
            return UserService.Register(email.ToLower(), password, nickname, emailHost.ToLower());
        }

        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response object with a value set to the user, instead the response should contain a error message in case of an error</returns>
        public Response<User> Login(string email, string password)
        {
            if (email == null | password == null) return new Response<User>("One of the parameters is invalid.");
            return UserService.Login(email.ToLower(), password);
        }

        /// <summary>        
        /// Log out an logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response Logout(string email)
        {
            if (email == null) return new Response("One of the parameters is invalid.");
            return UserService.Logout(email.ToLower());
        }

        /// <summary>
        /// Returns the board of a user. The user must be logged in
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>A response object with a value set to the board, instead the response should contain a error message in case of an error</returns>
        public Response<Board> GetBoard(string email)
        {
            if (email == null) return new Response<Board>("One of the parameters is invalid.");
            return BoardService.GetBoard(email.ToLower());
        }

        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            if (email == null) return new Response("One of the parameters is invalid.");
            return BoardService.LimitColumnTasks(email.ToLower(), columnOrdinal, limit);
        }

        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        public Response<Task> AddTask(string email, string title, string description, DateTime dueDate)
        {
            if (email == null | title == null) return new Response<Task>("One of the parameters is invalid.");
            return BoardService.AddTask(email.ToLower(), title, description, dueDate);
        }

        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            if (email == null) return new Response("One of the parameters is invalid.");
            return BoardService.UpdateTaskDueDate(email.ToLower(), columnOrdinal, taskId, dueDate);
        }

        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            if (email == null | title == null) return new Response("One of the parameters is invalid.");
            return BoardService.UpdateTaskTitle(email.ToLower(), columnOrdinal, taskId, title);
        }

        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            if (email == null) return new Response("One of the parameters is invalid.");
            return BoardService.UpdateTaskDescription(email.ToLower(), columnOrdinal, taskId, description);
        }


        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        
        /// <param name="emailAssignee">Email of the user to assign to task to</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            if (email == null | emailAssignee == null) return new Response("One of the parameters is invalid.");
            return BoardService.AssignTask(email.ToLower(), columnOrdinal, taskId, emailAssignee);
        }


        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        		
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response DeleteTask(string email, int columnOrdinal, int taskId)
        {
            if (email == null) return new Response("One of the parameters is invalid.");
            return BoardService.DeleteTask(email.ToLower(), columnOrdinal, taskId);
        }

        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            if (email == null) return new Response("One of the parameters is invalid.");
            return BoardService.AdvanceTask(email.ToLower(), columnOrdinal, taskId);
        }


        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        public Response<Column> GetColumn(string email, string columnName)
        {
            if (email == null | columnName == null) return new Response<Column>("One of the parameters is invalid.");
            return BoardService.GetColumn(email.ToLower(), columnName.ToLower());
        }

        /// <summary>
        /// Returns a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>

        public Response<Column> GetColumn(string email, int columnOrdinal)
        {
            if (email == null) return new Response<Column>("One of the parameters is invalid.");
            return BoardService.GetColumn(email.ToLower(), columnOrdinal);
        }

        /// <summary>
        /// Removes a column given it's identifier.
        /// The first column is identified by 0, the ID increases by 1 for each column
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Column ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RemoveColumn(string email, int columnOrdinal)
        {
            if (email == null) return new Response<Column>("One of the parameters is invalid.");
            return BoardService.RemoveColumn(email.ToLower(), columnOrdinal);
        }

        /// <summary>
        /// Adds a new column, given it's name and a location to place it.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Location to place to column</param>
        /// <param name="Name">new Column name</param>
        /// <returns>A response object with a value set to the new Column, the response should contain a error message in case of an error</returns>
        public Response<Column> AddColumn(string email, int columnOrdinal, string Name)
        {
            if (email == null | Name == null) return new Response<Column>("One of the parameters is invalid.");
            return BoardService.AddColumn(email.ToLower(), columnOrdinal, Name);
        }

        /// <summary>
        /// Moves a column to the right, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column        
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the column</param>
        /// <returns>A response object with a value set to the moved Column, the response should contain a error message in case of an error</returns>
        public Response<Column> MoveColumnRight(string email, int columnOrdinal)
        {
            if (email == null) return new Response<Column>("One of the parameters is invalid.");
            return BoardService.MoveColumnRight(email.ToLower(), columnOrdinal);
        }

        /// <summary>
        /// Moves a column to the left, swapping it with the column wich is currently located there.
        /// The first column is identified by 0, the ID increases by 1 for each column.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">Current location of the column</param>
        /// <returns>A response object with a value set to the moved Column, the response should contain a error message in case of an error</returns>
        public Response<Column> MoveColumnLeft(string email, int columnOrdinal)
        {
            if (email == null) return new Response<Column>("One of the parameters is invalid.");
            return BoardService.MoveColumnLeft(email.ToLower(), columnOrdinal);
        }

        /// <summary>
        /// Change the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="newName">The new name.</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            if (email == null | newName == null) return new Response<Column>("One of the parameters is invalid.");
            return BoardService.ChangeColumnName(email.ToLower(), columnOrdinal, newName);
        }
    }
}
