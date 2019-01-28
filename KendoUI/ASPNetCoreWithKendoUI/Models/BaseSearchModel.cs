using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetCoreWithKendoUI.Models
{
    public abstract partial class BaseSearchModel
    {
        #region Ctor

        public BaseSearchModel()
        {
            //set the default values
            this.Page = 1;
            this.PageSize = 10;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a page number
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets a page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets a comma-separated list of available page sizes
        /// </summary>
        public string AvailablePageSizes { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Set grid page parameters
        /// </summary>
        public void SetGridPageSize()
        {
            this.Page = 1;
            this.PageSize = 10;
            this.AvailablePageSizes = "10, 20, 50, 100";
        }

        /// <summary>
        /// Set popup grid page parameters
        /// </summary>
        public void SetPopupGridPageSize()
        {
            this.Page = 1;
            this.PageSize = 10;
            this.AvailablePageSizes = "10, 20, 50, 100";
        }

        #endregion
    }
}
