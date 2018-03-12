(function () {
    var pageSize = 10;
    $(function () {
        //1.初始化Table
        var unbindTable = new TableInit1();
        unbindTable.Init();
        var bindTable = new TableInit2();
        bindTable.Init();

        //2.初始化Button的点击事件
        var oButtonInit = new ButtonInit();
        oButtonInit.Init();
        
        //绑定角色到用户
        $("#btn_add_ur").click(function () {
            var index = $(".info td").eq(0).text();
            if (!index) {
                modals.info("请选择数据");
            }
            $.ajax({
                url: appPath + 'UserRoles/AddUserRoles',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ UserId: index, RoleId: $('#roleId').val() }),
                type: 'POST',
                dataType: 'JSON',
                success: function (data, textStatus) {
                    if (data.success) {
                        window.location.reload();
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

        //解绑用户
        $("#btn_remove_ur").click(function () {
            var index = $(".danger td").eq(0).text();
            if (!index) {
                modals.info("请选择数据");
            }
            $.ajax({
                url: appPath + 'UserRoles/RemoveUserRoles',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ UserId: index, RoleId: $('#roleId').val() }),
                type: 'POST',
                dataType: 'JSON',
                success: function (data, textStatus) {
                    if (data.success) {
                        window.location.reload();
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

        //
        $('#userRole_unselected_table').on('click-row.bs.table', function (e, row, element) {
            //$(element).css({ "color": "blue", "font-size": "16px;" }); 
            $(".info").removeClass("info");
            $(element).addClass("info");
            console.log(row);
        });
        //
        $('#userRole_selected_table').on('click-row.bs.table', function (e, row, element) {
            //$(element).css({ "color": "blue", "font-size": "16px;" }); 
            $(".danger").removeClass("danger");
            $(element).addClass("danger");
            console.log(row);
        });
    });

    var TableInit1 = function () {
        var unbindTableInit = new Object();
        
        //初始化Table
        unbindTableInit.Init = function () {
            var url = appPath + 'UserRoles/GetUnbindList?RoleId=' + $('#roleId').val();
            $.get(url, function (result) {
                console.log(result.result);
                if (result) {
                    $('#userRole_unselected_table').bootstrapTable({
                        data: result.result.rows,
                        totalRows: result.result.total,
                        dataType: "json",
                        method: 'GET',                      //请求方式（*）
                        striped: true,                      //是否显示行间隔色
                        cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
                        pagination: true,                   //是否显示分页（*）
                        queryParams: unbindTableInit.queryParams,//传递参数（*）
                        sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
                        pageNumber: 1,                       //初始化加载第一页，默认第一页
                        pageSize: 10,                       //每页的记录行数（*）
                        pageList: [10],        //可供选择的每页的行数（*）
                        //showColumns: false,                  //是否显示所有的列
                        minimumCountColumns: 2,             //最少允许的列数
                        clickToSelect: true,                //是否启用点击选中行
                        height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
                        uniqueId: "id",                     //每一行的唯一标识，一般为主键列
                        columns: [{
                            field: 'id',
                            title: 'ID'
                        }, {
                            field: 'name',
                            title: '名字'
                        }, {
                            field: 'userName',
                            title: '登录名'
                        }]
                    });
                }
            }, 'json');
        };
        
        //得到查询的参数
        unbindTableInit.queryParams = function (params) {
            var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
                offset: params.offset,  //页码
                name: $("#txtName").val(),
                search: params.search
            };
            return temp;
        };
        return unbindTableInit;
    };

    var TableInit2 = function () {
        var bindTableInit = new Object();
        //
        bindTableInit.Init = function () {
            var url = appPath + 'UserRoles/GetBindList?RoleId=' + $('#roleId').val();
            $.get(url, function (result) {
                console.log(result.result);
                if (result) {
                    $('#userRole_selected_table').bootstrapTable({
                        data: result.result.rows,
                        totalRows: result.result.total,
                        dataType: "json",
                        method: 'GET',                      //请求方式（*）
                        striped: true,                      //是否显示行间隔色
                        cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
                        pagination: true,                   //是否显示分页（*）
                        queryParams: bindTableInit.queryParams,//传递参数（*）
                        sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
                        pageNumber: 1,                       //初始化加载第一页，默认第一页
                        pageSize: 10,                       //每页的记录行数（*）
                        pageList: [10],        //可供选择的每页的行数（*）
                        search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
                        showColumns: false,                  //是否显示所有的列
                        minimumCountColumns: 2,             //最少允许的列数
                        clickToSelect: true,                //是否启用点击选中行
                        height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
                        uniqueId: "id",                     //每一行的唯一标识，一般为主键列
                        columns: [{
                            field: 'id',
                            title: 'ID'
                        }, {
                            field: 'name',
                            title: '名字'
                        }, {
                            field: 'userName',
                            title: '登录名'
                        }]
                    });
                }
            }, 'json');
        }

        //得到查询的参数
        bindTableInit.queryParams = function (params) {
            var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
                offset: params.offset,  //页码
                name: $("#txtUsername").val(),
            };
            return temp;
        };
        return bindTableInit;
    }

    var ButtonInit = function () {
        var oInit = new Object();
        var postdata = {};

        oInit.Init = function () {
            //初始化页面上面的按钮事件
        };

        return oInit;
    };

})()