(function () {
    var roleTable, userRoleTable;
    var winId = "roleWin";
    $(function () {
        //init table and fill data
        var role_config = {
            rowClick: function (row, isSelected) {
                $("#roleId").val(isSelected ? row.id : "-1");
                $("#roleName").remove();
                if (isSelected)
                    $("#searchDiv_userRole").prepend("<h5 id='roleName' class='pull-left'>【" + row.name + "】</h5>");
                userRoleTable.reloadData();
            }
        }
        roleTable = new CommonTable("role_table", "role_list", "searchDiv", role_config);

        var config = {
            lengthChange: false,
            pagingType: 'simple_numbers'
        }
        //init userrole table
        userRoleTable = new CommonTable("userRole_table", "userRole_selected_list", "searchDiv_userRole", config);

        //默认选中第一行
        setTimeout(function () { roleTable.selectFirstRow(true) }, 10);
        //make right table button on the same row

        //button event
        $('button[data-btn-type]').click(function () {
            var action = $(this).attr('data-btn-type');
            var rowId = roleTable.getSelectedRowId();
            switch (action) {
                case 'add':
                    modals.openWin({
                        winId: winId,
                        title: '新增角色',
                        width: '600px',
                        url: "/role/edit",
                        hideFunc: function () {
                            modals.info("hide me");
                        },
                        showFunc:function(){
                            modals.info("show me");
                        }
                    });
                    break;
                case 'edit':
                    if (!rowId) {
                        modals.info('请选择要编辑的行');
                        return false;
                    }
                    modals.openWin({
                        winId: winId,
                        title: '编辑角色【' + roleTable.getSelectedRowData().name + '】',
                        width: '600px',
                        url: "/role/edit?id=" + rowId
                    });
                    break;
                case 'delete':
                    if (!rowId) {
                        modals.info('请选择要删除的行');
                        return false;
                    }
                    modals.confirm("是否要删除该行数据？", function () {
                        $.ajax({
                            url: '../Roles/Delete/' + rowId,
                            type: 'POST',
                            data: null,
                            dataType: 'JSON',
                            success: function (data, textStatus) {
                                //modals.correct("已删除该数据");
                                roleTable.reloadData();
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                //setTimeout(function(){modals.info(data.message)},2000);
                                modals.info(data.message);
                            }
                        });
                    })
                    break;
                case 'selectUserRole':
                    if (!rowId) {
                        modals.info('请选择角色');
                        return;
                    }
                    modals.openWin({
                        winId: 'userRoleWin',
                        width: 1000,
                        title: '角色【' + roleTable.getSelectedRowData().name + '】绑定用户',
                        url: '/userrole/select?roleId=' + rowId,
                        hideFunc: function () { userRoleTable.reloadData(); }
                    })
                    break;
                case 'deleteUserRole':
                    var rowId_ur = userRoleTable.getSelectedRowId();
                    if (!rowId_ur) {
                        modals.info("请选择要删除的用户");
                        return false;
                    }
                    modals.confirm("是否要删除该行数据", function () {
                        $.ajax({
                            url: '../Roles/Delete',
                            type: 'POST',
                            data: { ids: rowId_ur },
                            dataType: 'JSON',
                            success: function (data, textStatus) {
                                userRoleTable.reloadData();
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                modals.info(data.message);
                            }
                        });
                    });
                    break;
            }
        });
    });
})()