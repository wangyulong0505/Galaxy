(function () {
    $(function () {
        //选中table行高亮显示
        $('#user_table tr').click(function () {
            //一次只能选中一行
            $(this).siblings().css('background-color', '#fff');
            $(this).addClass("trchange").siblings().removeClass("trchange");
            $(this).css('background-color', '#08C');
        });
        $('button[data-btn-type]').click(function () {
            var action = $(this).attr('data-btn-type');
            //获取选中数据的Id
            var rowId = $('#user_table tr.trchange').attr('id');
            switch (action) {
                case 'add':
                    window.location.href = appPath + 'Users/UsersAdd';
                    break;
                case 'edit':
                    console.log(rowId);
                    if (!rowId) {
                        modals.info('请选择要编辑的行');
                        return false;
                    }
                    window.location.href = appPath + "Users/UsersEdit/" + rowId;
                    break;
                case 'delete':
                    if (!rowId) {
                        modals.info('请选择要删除的行');
                        return false;
                    }
                    modals.confirm("是否要删除该行数据？", function () {
                        $.post(appPath + "Users/UsersDelete/" + rowId, null, function (data) {
                            if (data.success) {
                                modals.correct("已删除该数据");
                                //刷新当前页面
                            } else {
                                modals.error("用户数据被引用，不可删除！");
                            }
                        });
                    });
                    break;
                case 'search':
                    //类似于 &strkey=name=wangshibang|username=admin
                    var searchKey = '';
                    if ($('#search_name').val() && !$('#search_username').val()) {
                        searchKey += 'name=' + $('#search_name').val();
                    }
                    if (!$('#search_username').val() && $('#search_username').val()) {
                        searchKey += 'username=' + $('#search_username').val();
                    }
                    if ($('#search_username').val() && $('#search_username').val()) {
                        searchKey += 'name=' + $('#search_name').val() + '|username=' + $('#search_username').val();
                    }
                    window.location.href = appPath + 'Users/Index?pageIndex=1&pageSize=' + $("#user_page").find("option:selected").val() + '&strKey=' + searchKey;
                    break;
                case 'reset':
                    $('#search_name').val('');
                    $('#search_username').val('');
                    break;
            }

        });

        //下拉框值改变触发
        $('#user_page').change(function () {
            //获取选中值
            var vs = this.value;
            window.location.href = appPath + 'Users/Index?pageIndex=1&pageSize=' + vs + '&strKey=' + $('#key').val();
        });
    });
})();