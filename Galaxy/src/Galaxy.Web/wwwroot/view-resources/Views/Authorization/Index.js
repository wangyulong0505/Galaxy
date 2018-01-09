(function () {
    //tableId,queryId,conditionContainer
    var roleTable, roleFuncTable;
    $(function () {
        //init table and fill data
        var role_config = {
            rowClick: function (row, isSelected) {
                $("#roleId").val(isSelected ? row.id : "-1");
                $("#roleName").remove();
                if (isSelected) {
                    $("#searchDiv_roleFunc").prepend("<h5 id='roleName' class='pull-left'>【" + row.name + "】</h5>");
                }
                roleFuncTable.reloadData();
            }
        }
        roleTable = new CommonTable("role_table", "role_list", "searchDiv", role_config);

        //init userrole table
        roleFuncTable = new CommonTable("roleFunc_table", "roleFunc_selected_list", "searchDiv_roleFunc");

        //默认选中第一行
        setTimeout(function () {
            roleTable.selectFirstRow(true);
        }, 10);

        //button event
        $('button[data-btn-type]').click(function () {
            var action = $(this).attr('data-btn-type');
            var rowId = roleTable.getSelectedRowId();
            switch (action) {
                case 'selectRoleFunc':
                    if (!rowId) {
                        modals.info('请选择角色');
                        return;
                    }
                    modals.openWin({
                        winId: 'roleFuncWin',
                        width: 900,
                        title: '角色【' + roleTable.getSelectedRowData().name + '】绑定功能',
                        url: '/Authorization/select/'+rowId,
                        hideFunc: function () {
                            roleFuncTable.reloadData();
                        }
                    });
                    break;
                case 'deleteRoleFunc':
                    var rowId_ur = roleFuncTable.getSelectedRowId();
                    if (!rowId_ur) {
                         modals.info("请选择要解绑的功能");
                        return false;
                    }
                    modals.confirm("是否要删除该行数据", function () {
                        $.ajax({
                            url: '../Authorization/Delete/' + rowId_ur,
                            type: 'POST',
                            data: null,
                            dataType: 'JSON',
                            success: function () {
                                roleFuncTable.reloadData();
                            },
                            error: function () {
                                modals.info(data.message);
                            }
                        });
                    });
                    break;
                default:
                    break;
            }
        });
    })
})()