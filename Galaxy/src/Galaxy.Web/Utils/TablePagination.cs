using System.Text;

namespace Galaxy.Web.Utils
{
    public static class TablePagination
    {
        public static string PagingHtml(int pageIndex, int pageSize, int pageCount, int itemCount, string strKey, string path = "")
        {
            if (!string.IsNullOrEmpty(strKey))
            {
                if (!strKey.StartsWith('&'))
                {
                    strKey = '&' + strKey;
                }
            }
            StringBuilder sb = new StringBuilder();

            #region template

            /* template
            <div class="row">
                <div class="col-sm-5">
                    <div class="dataTables_info" id="markdown_table_info" role="status" aria-live="polite">第 1 至 10 项记录，共 14 项</div>
                </div>
                <div class="col-sm-7">
                    <div class="dataTables_paginate paging_full_numbers" id="markdown_table_paginate">
                        <ul class="pagination">
                            <li class="paginate_button first disabled" id="markdown_table_first"><a href = "#" aria-controls="markdown_table" data-dt-idx="0" tabindex="0">首页</a></li>
                            <li class="paginate_button previous disabled" id="markdown_table_previous"><a href = "#" aria-controls="markdown_table" data-dt-idx="1" tabindex="0">上页</a></li>
                            <li class="paginate_button active"><a href = "#" aria-controls="markdown_table" data-dt-idx="2" tabindex="0">1</a></li>
                            <li class="paginate_button "><a href = "#" aria-controls="markdown_table" data-dt-idx="3" tabindex="0">2</a></li>
                            <li class="paginate_button next" id="markdown_table_next"><a href = "#" aria-controls="markdown_table" data-dt-idx="4" tabindex="0">下页</a></li>
                            <li class="paginate_button last" id="markdown_table_last"><a href = "#" aria-controls="markdown_table" data-dt-idx="5" tabindex="0">末页</a></li>
                        </ul>
                    </div>
                </div>
            </div>
            */
            #endregion

            #region header

            sb.Append("<div class='row'>");
            sb.Append("<div class='col-sm-5'>");
            sb.Append($"<div class='dataTables_info' id='markdown_table_info' role='status' aria-live='polite'>第 {((pageIndex-1) * pageSize) + 1} 至 {pageIndex * pageSize} 项记录，共 {itemCount} 项</div>");
            sb.Append("</div>");
            sb.Append("<div class='col-sm-7'>");
            sb.Append("<div class='dataTables_paginate paging_full_numbers' id='markdown_table_paginate'>");
            sb.Append("<ul class='pagination'>");

            #endregion

            #region body

            if (pageIndex == 1)
            {
                sb.Append($"<li class='paginate_button first disabled' id='markdown_table_first'><a href='#' aria-controls='markdown_table' data-dt-idx='0' tabindex='0'>首页</a></li>");
                sb.Append($"<li class='paginate_button previous disabled' id='markdown_table_previous'><a href='#' aria-controls='markdown_table' data-dt-idx='1' tabindex='0'>上页</a></li>");
            }
            else
            {
                sb.Append($"<li class='paginate_button first' id='markdown_table_first'><a href='{path}?pageIndex=1&pageSize={pageSize}{strKey}' aria-controls='markdown_table' data-dt-idx='0' tabindex='0'>首页</a></li>");
                sb.Append($"<li class='paginate_button previous' id='markdown_table_previous'><a href='{path}?pageIndex={pageIndex - 1}&pageSize={pageSize}{strKey}' aria-controls='markdown_table' data-dt-idx='1' tabindex='0'>上页</a></li>");
            }

            if (pageCount == 0)
            {
                //无数据时只显示1页
                sb.Append($"<li class='paginate_button active'><a href='{path}?pageIndex=1&{strKey}' aria-controls='markdown_table' data-dt-idx='2' tabindex='0'>1</a></li>");
                sb.Append($"<li class='paginate_button next disabled' id='markdown_table_next'><a href='#' aria-controls='markdown_table' data-dt-idx='3' tabindex='0'>下页</a></li>");
                sb.Append($"<li class='paginate_button last disabled' id='markdown_table_last'><a href='#' aria-controls='markdown_table' data-dt-idx='4' tabindex='0'>末页</a></li>");
            }
            else
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    if (pageIndex == i)
                    {
                        sb.Append($"<li class='paginate_button active'><a href='{path}?pageIndex={i}&pageSize={pageSize}{strKey}' aria-controls='markdown_table' data-dt-idx='{i + 1}' tabindex='0'>{i}</a></li>");
                    }
                    else
                    {
                        sb.Append($"<li class='paginate_button'><a href='{path}?pageIndex={i}&pageSize={pageSize}{strKey}' aria-controls='markdown_table' data-dt-idx='{i + 1}' tabindex='0'>{i}</a></li>");
                    }
                }

                if (pageIndex == pageCount)
                {
                    sb.Append($"<li class='paginate_button next disabled' id='markdown_table_next'><a href='#' aria-controls='markdown_table' data-dt-idx='{pageCount + 2}' tabindex='0'>下页</a></li>");
                    sb.Append($"<li class='paginate_button last disabled' id='markdown_table_last'><a href='#' aria-controls='markdown_table' data-dt-idx='{pageCount + 3}' tabindex='0'>末页</a></li>");
                }
                else
                {
                    sb.Append($"<li class='paginate_button next' id='markdown_table_next'><a href='{path}?pageIndex={pageIndex + 1}&pageSize={pageSize}{strKey}' aria-controls='markdown_table' data-dt-idx='{pageCount + 2}' tabindex='0'>下页</a></li>");
                    sb.Append($"<li class='paginate_button last' id='markdown_table_last'><a href='{path}?pageIndex={pageCount}&pageSize={pageSize}{strKey}' aria-controls='markdown_table' data-dt-idx='{pageCount + 3}' tabindex='0'>末页</a></li>");
                }
            }

            #endregion

            #region footer

            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            #endregion

            return sb.ToString();
        }
    }
}
