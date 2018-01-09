(function () {
    $(function () {
        //选中table行高亮显示
        $('#user_table tr').click(function () {
            //一次只能选中一行
            $(this).siblings().css('background-color', '#fff');
            $(this).addClass("trchange").siblings().removeClass("trchange");
            $(this).css('background-color', '#08C');
            /*
            //背景变色
            $(this).css('background-color', '#08C').siblings().css('background-color');
            //前景色变色
            $(this).css('color', 'white').siblings().css('color');
            */
        });
        $('button[data-btn-type]').click(function () {
            var action = $(this).attr('data-btn-type');
            var rowId = $('#user_table tr').attr('id');
            switch (action) {
                case 'add':
                    window.location.href = '../Users/UsersAdd';
                    break;
                case 'edit':
                    if (!rowId) {
                        modals.info('请选择要编辑的行');
                        return false;
                    }
                    window.location.href = "/Users/UsersEdit/" + rowId;
                    break;
                case 'delete':
                    if (!rowId) {
                        modals.info('请选择要删除的行');
                        return false;
                    }
                    modals.confirm("是否要删除该行数据？", function () {
                        $.post("/Users/UsersDelete/" + rowId, null, function (data) {
                            if (data.success) {
                                modals.correct("已删除该数据");
                                //刷新当前页面
                            } else {
                                modals.error("用户数据被引用，不可删除！");
                            }
                        });
                    });
                    break;
            }

        });

        //下拉框值改变触发
        $('#pageSize').change(function () {
            //获取选中值
            var pageItems = $(this).children('option:selected').val();
            //获取
            modals.info(pageItems);
        });
    });
})();