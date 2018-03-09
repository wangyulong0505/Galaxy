(function () {
    $(function () {
        //隐藏的弹出框--数据校验
        formValidate();
        //如果Id不为0就ajax获取数据初始化Form
        if (0 != "0") {
            $.ajax({
                url: appPath + 'Role/GetRole/' + id,
                data: {},
                type: 'GET',
                dataType: 'JSON',
                success: function (data, textStatus) {
                    if (data.success) {
                        //获取数据后初始化Form
                    }
                    else {
                        modals.warn(data.result);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    modals.error(errorThrown);
                }
            });
        }
        //RoleTable tr选中样式
        $('#role_table tr').click(function () {
            //一次只能选中一行
            $(this).siblings().css('background-color', '#fff');
            $(this).addClass("trchange").siblings().removeClass("trchange");
            $(this).css('background-color', '#08C');
            //根据选择的RoleId，执行ajax获取RoleId下对应的User
            var roleId = $('#role_table tr.trchange').attr('id');
            $.ajax({
                type: 'Get',
                url: appPath + 'Roles/GetUsers/' + roleId,
                data:'null',
                dataType: 'JSON',
                success: function (data, textStatus) {
                    if (data.success) {
                        //根据获取的数据初始化Table
                    }
                    else {
                        modals.warn(data.result);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    modals.error(errorThrown);
                }
            })
        });
        //UserTable tr选中样式
        $('#userRole_table tr').click(function () {
            //一次只能选中一行
            $(this).siblings().css('background-color', '#fff');
            $(this).addClass("trchange").siblings().removeClass("trchange");
            $(this).css('background-color', '#08C');
        });

        $('button[data-btn-type]').click(function () {
            var action = $(this).attr('data-btn-type');
            var rowId = $('#role_table tr.trchange').attr('id');
            console.log(rowId);
            switch (action) {
                case 'addRole':
                    /*
                    modals.openWin({
                        winId: 'roleWin',
                        title: '新增角色',
                        width: '600px',
                        url: appPath + "Roles/RoleEdit" //通过url获取后台传递的html填充模态窗口, 这种方式很恶心，所以
                    });
                    */
                    //先清除validate状态
                    $('#role-form').bootstrapValidator('resetForm')
                    $('#roleWin').modal('show');
                    break;
                case 'editRole':
                    if (!rowId) {
                        modals.info('请选择要编辑的行');
                        return false;
                    }
                    $.ajax({
                        url: appPath + "Roles/RoleEdit/" + rowId,
                        type: 'GET',
                        data: {},
                        dataType: 'JSON',
                        success: function (data, textStatus) {
                            if (data.success) {
                                //获取数据，初始化roleWin里面的数据，然后打开窗口
                                console.log(data.result);
                                var role = JSON.parse(data.result);
                                $('#id').val(role.Id);
                                $('#name').val(role.Name);
                                $('#code').val(role.Code);
                                $('#sort').val(role.Sort);
                                $('#remark').val(role.Remark);
                                $("input[name='status'][value='" + role.Status + "']").prop("checked", "checked");
                                $('#createDate').val(role.CreateDate);
                                $('#permissionIds').val('');
                                $('#roleWin').modal('show');
                            }
                            else {
                                modals.warn(data.result);
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            modals.error(errorThrown);
                        }
                    })
                    break;
                case 'deleteRole':
                    if (!rowId) {
                        modals.info('请选择要删除的行');
                        return false;
                    }
                    modals.confirm("是否要删除该行数据？", function () {
                        $.ajax({
                            url: appPath + 'Roles/RoleDelete/' + rowId,
                            type: 'POST',
                            data: null,
                            dataType: 'JSON',
                            success: function (data, textStatus) {
                                if (data.success) {
                                    window.location.href = appPath + 'Role/Index';
                                }
                                else {
                                    modals.warn(data.result);
                                }
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                modals.error(errorThrown);
                            }
                        });
                    })
                    break;
                case 'roleSave':
                    var bootstrapValidator = $("#role-form").data('bootstrapValidator');
                    bootstrapValidator.validate();
                    if (bootstrapValidator.isValid()) {
                        var param = {
                            Id: $('#id').val() ? $('#id').val() : '0',
                            Name: $('#name').val(),
                            Code: $('#code').val(),
                            Sort: $('#sort').val(),
                            Remark: $('#remark').val(),
                            Status: $('input[name="status"]:checked').val(),
                            CreateDate: formatDate(new Date(), 'yyyy-mm-dd'),
                            PermissionIds: $('#permissionIds').val() ? $('#permissionIds').val() : ''
                        }
                        console.log(param);
                        modals.confirm('确认保存？', function () {
                            $.ajax({
                                contentType: 'application/json; charset=utf-8',
                                url: appPath + 'Roles/RoleSave',
                                data: JSON.stringify(param),
                                type: 'POST',
                                dataType: 'JSON',
                                success: function (data, textStatus) {
                                    if (data.success) {
                                        //判断是新增还是更新
                                        window.location.href = appPath + 'Roles/Index';
                                        //modals.closeWin(winId);
                                        //重新加载数据
                                    }
                                },
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    modals.error(errorThrown);
                                }
                            });
                        });
                    }
                    break;
                case 'selectUser':
                    if (!rowId) {
                        modals.info('请选择角色');
                        return;
                    }
                    //根据RoleId获取两个Json，然后绑定到Table中，最后打开modals
                    $('#userRoleWin').modal('show');
                    break;
                case 'deleteUser':
                    var userRowId = $('#userRole_table tr[class="trchange"]').attr('id');
                    if (!userRowId) {
                        modals.info("请选择要删除的用户");
                        return false;
                    }
                    modals.confirm("是否要删除该行数据", function () {
                        $.ajax({
                            url: appPath + 'Roles/Delete/' + userRowId,
                            type: 'POST',
                            data: { },
                            dataType: 'JSON',
                            success: function (data, textStatus) {
                                userRoleTable.reloadData();
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                modals.info(errorThrown);
                            }
                        });
                    });
                    break;
            }
        });

        //绑定角色到用户
        $("#btn_add_ur").click(function () {
            var rows = unselectedTable.getSelectedRowsData();
            var urlist = [];
            //console.log(JSON.stringify(rows));
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
            ajaxPost(basePath + "/userrole/save", { "urlist": JSON.stringify(urlist) }, function (data) {
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
            ajaxPost(basePath + "/userrole/delete", { "ids": idArr.join(",") }, function (data) {
                if (data.success) {
                    unselectedTable.reloadRowData();
                    selectedTable.reloadData();
                }
            })
        })
    });

    function formValidate() {
        $("#role-form").bootstrapValidator({
            message: '请输入有效值',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            excluded: [':disabled'],
            fields: {
                name: {
                    validators: {
                        notEmpty: {
                            message: '请输入姓名'
                        }
                    }
                },
                code: {
                    validators: {
                        notEmpty: {
                            message: '请输入编码'
                        },
                        /*
                        remote: {
                            url: appPath + "base/checkUnique",
                            data: function (validator) {
                                return {
                                    className: 'com.cnpc.framework.base.entity.Role',
                                    fieldName: 'code',
                                    fieldValue: $('#code').val(),
                                    id: $('#id').val()
                                };
                            },
                            message: '该编码已被使用'
                        }
                        */
                    }
                },
                sort: {
                    validators: {
                        notEmpty: {
                            message: '请输入排序'
                        }
                    }
                }
            }
        });
    }

    /**
     *
     * @param {any} tableid绑定的table的id
     * @param {any} json数组
     */
    function bindDataTable(tableid, json) {
        var str = '';
        for (var i = 0; i < json.length; i++) {
            str += '<tr id="' + json[i].Id + '" role="row" class=' + (i % 2 == 0 ? "even" : "odd") + '>';
            str += '<td class="text-center sorting_1">' + ((ViewBag.PageIndex - 1) * ViewBag.PageSize + i + 1) + '</td>';
            str += '<td class="text-center">' + json[i].Name + '</td>';
            str += '<td class="text-center">' + json[i].UserName + '</td>';
            str += '</tr>';
        }
        $("'#" + tableid + "'").find('tbody').append(str);
    }
})()