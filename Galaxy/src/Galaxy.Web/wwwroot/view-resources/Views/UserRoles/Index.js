(function () {
    $(function () {
        //RoleTable tr选中样式
        $('#role_table tr').click(function () {
            //一次只能选中一行
            $(this).siblings().css('background-color', '#fff');
            $(this).addClass("trchange").siblings().removeClass("trchange");
            $(this).css('background-color', '#08C');
            //根据选择的RoleId，执行ajax获取RoleId下对应的User
            var roleId = $('#role_table tr.trchange').attr('id');
            getRoleUsers(roleId);
        });
        //UserTable tr选中样式
        $('#userRole_table tr').click(function () {
            //一次只能选中一行
            $(this).siblings().css('background-color', '#fff');
            $(this).addClass("trchange").siblings().removeClass("trchange");
            $(this).css('background-color', '#08C');
        });
        
        //绑定角色到用户
        $("#btn_add_ur").click(function () {
            var rows = unselectedTable.getSelectedRowsData();
            var urlist = [];
            if (!rows) {
                modals.info("请选择要绑定该角色的用户");
                return;
            }
            $.each(rows, function (index, row) {
                var urObj = {};
                var user = {};
                user.id = row.id;
                user.version = row.version;
                urObj.user = user;
                urObj.roleId = roleId;
                urlist.push(urObj);
            });
            ajaxPost(appPath + "UserRole/Add", { "urlist": JSON.stringify(urlist) }, function (data) {
                if (data.success) {
                    selectedTable.reloadData();
                    unselectedTable.reloadRowData();
                }
            });

        });

        //解绑用户
        $("#btn_remove_ur").click(function () {
            var rows = selectedTable.getSelectedRowsData();
            if (!rows) {
                modals.info("请选择要解绑的用户");
                return;
            }
            var idArr = [];
            $.each(rows, function (index, row) {
                idArr.push(row.id);
            })
            ajaxPost(appPath + "UserRole/Delete", { "ids": idArr.join(",") }, function (data) {
                if (data.success) {
                    unselectedTable.reloadRowData();
                    selectedTable.reloadData();
                }
            })
        });

        //静态Table分页
        var unbindtable = $('#userRole_unselected_table');
        var bindtable = $('#userRole_selected_table');
        var unbindTotalPage = $('#unbindTotalPage');
        var bindTotalPage = $('#bindTotalPage');
        var unbindPageNum = $('#userRole_unselected_table_page');
        var bindPageNum = $('#userRole_selected_table_page');
        var unbindPre = $('#userRole_unselected_table_previous');
        var unbindNext = $('#userRole_unselected_table_next');
        var bindPre = $('#userRole_selected_table_previous');
        var bindNext = $('#userRole_selected_table_next');

        var unbindNumberRows = unbindtable.rows.length;
        var bindNumberRows = bindtable.rows.length;
        var pageSize = 10;
        var page = 1;    
    });

    function next() {

        hideTable();

        currentRow = pageSize * page;
        maxRow = currentRow + pageSize;
        if (maxRow > numberRowsInTable) maxRow = numberRowsInTable;
        for (var i = currentRow; i < maxRow; i++) {
            unbindtable.rows[i].style.display = '';
        }
        page++;

        if (maxRow == numberRowsInTable) { nextText(); lastText(); }
        showPage();
        preLink();
        firstLink();
    }

    //上一页    
    function pre() {

        hideTable();

        page--;

        currentRow = pageSize * page;
        maxRow = currentRow - pageSize;
        if (currentRow > numberRowsInTable) currentRow = numberRowsInTable;
        for (var i = maxRow; i < currentRow; i++) {
            unbindtable.rows[i].style.display = '';
        }


        if (maxRow == 0) { preText(); firstText(); }
        showPage();
        nextLink();
        lastLink();
    }

    //第一页    
    function first() {
        hideTable();
        page = 1;
        for (var i = 0; i < pageSize; i++) {
            unbindtable.rows[i].style.display = '';
        }
        showPage();

        preText();
        nextLink();
        lastLink();
    }

    //最后一页    
    function last() {
        hideTable();
        page = pageCount();
        currentRow = pageSize * (page - 1);
        for (var i = currentRow; i < numberRowsInTable; i++) {
            unbindtable.rows[i].style.display = '';
        }
        showPage();

        preLink();
        nextText();
        firstLink();
    }

    function hideTable() {
        for (var i = 0; i < numberRowsInTable; i++) {
            unbindtable.rows[i].style.display = 'none';
        }
    }

    function showPage() {
        pageNum.innerHTML = page;
    }

    //总共页数    
    function pageCount() {
        var count = 0;
        if (numberRowsInTable % pageSize != 0) count = 1;
        return parseInt(numberRowsInTable / pageSize) + count;
    }

    //显示链接    
    function preLink() { spanPre.innerHTML = "<a href='javascript:pre();'>上一页</a>"; }
    function preText() { spanPre.innerHTML = "上一页"; }

    function nextLink() { spanNext.innerHTML = "<a href='javascript:next();'>下一页</a>"; }
    function nextText() { spanNext.innerHTML = "下一页"; }

    //隐藏表格    
    function hide() {
        for (var i = pageSize; i < numberRowsInTable; i++) {
            unbindtable.rows[i].style.display = 'none';
        }

        totalPage.innerHTML = pageCount();
        pageNum.innerHTML = '1';

        nextLink();
        lastLink();
    }

    hide();    
})()