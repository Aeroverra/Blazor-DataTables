﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tech.Aerove.Blazor.DataTables.Configs;
using Tech.Aerove.Blazor.DataTables.Context;
using Tech.Aerove.Blazor.DataTables.Enums;
using Tech.Aerove.Blazor.DataTables.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Tech.Aerove.Blazor.DataTables.Api
{
    public class TableController : ITableController
    {
        private readonly ITableContext Context;
        private UIConfig UIConfig => Context.UIConfig;
        private RunningConfig RunningConfig => Context.RunningConfig;
        private IEngine Engine => Context.Engine;

        /// <summary>
        /// Tell listeners we have fresh data. The datatable component uses
        /// this to call statehaschanged and update the ui
        /// </summary>
        public event Func<Task> OnAfterUpdate = null!;

        public TableController(ITableContext tableContext)
        {
            Context = tableContext;
        }

        public int Page
        {
            get => RunningConfig.Page;
            set
            {
                RunningConfig.Page = value;
                RunningConfig.Start = (Page - 1) * Length;
            }
        }
        public int RecordsTotal => RunningConfig.RecordsTotal;
        public int RecordsFiltered => RunningConfig.RecordsFiltered;
        public int Start => RunningConfig.Start;
        public int TotalPages => RecordsFiltered / Length + (RecordsFiltered % Length > 0 ? 1 : 0);

        public void SetPageNext()
        {
            Page = Page + 1;
        }

        public void SetPagePrevious()
        {
            Page = Page - 1;
        }

        public void SetPageFirst()
        {
            Page = 1;
        }

        public void SetPageLast()
        {
            Page = TotalPages;
        }





        /// <summary>
        /// Search text
        /// </summary>
        public string? SearchText
        {
            get => RunningConfig.SearchText;
            set
            {
                RunningConfig.SearchText = $"{value}";
                Page = 1;
            }
        }

        /// <summary>
        /// length of records queried and displayed
        /// </summary>
        public int Length
        {
            get => RunningConfig.Length;
            set
            {
                if (value > RunningConfig.MaxLength)
                {
                    throw new ArgumentException($"Maxiumum length of {value} can not be set due to its constraints.");
                }
                RunningConfig.Length = value;
                Page = 1;
            }
        }


        /// <summary>
        /// Resets all column ordering
        /// </summary>
        public void OrderReset()
        {
            //Get all columns enabled except this one
            var columns = RunningConfig.Columns
                .Where(x => x.OrderableDirection != OrderableDirection.Disabled)
                .ToList();

            //reset direction
            foreach (var column in columns)
            {
                column.OrderableDirection = OrderableDirection.None;
            }

            //clear ordered list
            RunningConfig.ColumnsOrdered.Clear();
        }

        /// <summary>
        /// Swaps the ordering direction of a column 
        /// </summary>
        /// <param name="name">Case sensitive column name</param>
        /// <param name="append">If set to true will add a second order property like .ThenBy()</param>
        /// <exception cref="ArgumentException"></exception>
        public void SwapOrder(string name, bool append = false)
        {
            var column = GetColumn(name);
            SwapOrder(column, append);
        }

        /// <summary>
        /// Swaps the ordering direction of a column 
        /// </summary>
        /// <param name="column">Column</param>
        /// <param name="append">If set to true will add a second order property like .ThenBy()</param>
        /// <exception cref="ArgumentException"></exception>
        internal void SwapOrder(ColumnModel column, bool append = false)
        {
            if (column.OrderableDirection == OrderableDirection.Disabled)
            {
                throw new ArgumentException($"This column has been disabled and can not be changed!");
            }
            //get current direction so we can determine the next direction 
            var currentDirection = column.OrderableDirection;
            if (append == false)
            {
                //resets ordering in case other columns were set before this
                OrderReset();
            }
            else
            {
                //append is true so so if this column is already in the orderables
                //remove it so we don't make a duplicate.
                //append usually used in cases like the ui where shift is held down 
                RunningConfig.ColumnsOrdered.RemoveAll(x => x == column);
            }
            //switch on current direction saved before the reset and set new column value
            switch (currentDirection)
            {
                case OrderableDirection.None: column.OrderableDirection = OrderableDirection.Ascending; break;
                case OrderableDirection.Ascending: column.OrderableDirection = OrderableDirection.Descending; break;
                case OrderableDirection.Descending: column.OrderableDirection = OrderableDirection.None; break;
            }

            // Add it to our ordering list so we know which one is ordered by 1st, 2nd, 3rd etc...
            // but only if the direction is not none / neutral
            if (column.OrderableDirection != OrderableDirection.None)
            {
                RunningConfig.ColumnsOrdered.Add(column);
            }

        }

        /// <summary>
        /// Gets column by name 
        /// </summary>
        /// <param name="name">case sensitive name</param>
        /// <exception cref="ArgumentException"></exception>
        internal ColumnModel GetColumn(string name)
        {
            var column = RunningConfig.Columns.FirstOrDefault(x => x.Name == name);
            if (column == null)
            {
                throw new ArgumentException($"Could not find column with name {name}.");
            }
            return column;
        }

        /// <summary>
        /// Updates the Items then calls the event handler OnAfterUpdate
        /// To tell the listeners
        /// The Datatable component is a listener and calls Statehaschanged to update the ui
        /// </summary>
        /// <param name="visualOnly">Updates the ui without requerying the data</param>
        /// <returns></returns>
        public async Task UpdateAsync(bool visualOnly = false)
        {
            if (visualOnly == false)
            {
                //call our engine to query the data again
                await Engine.UpdateAsync();
            }

            //call our event handler so listeners can perform actions
            //like the datatable component which will call statehaschanged
            await OnAfterUpdate();
        }

    }
}
