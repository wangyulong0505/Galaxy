(function () {
    $(function () {
        $("[data-btn-type]").click(function () {
            var btnType = $(this).data("btn-type");
            //获取选中行的Id
            switch (btnType) {
                case "delete":
                    modals.confirm("是否将该消息放入回收站?", function () {
                        //
                    });
                    break;
                case "reply":

                    break;
                case "forward":

                    break;
                case "signUnread":
                    if (rowData.readYet == 0) {
                        modals.info("该消息当前为未读状态，不可标记为未读");
                        return;
                    }
                    modals.confirm("是否将该消息标记为未读?", function () {
                        //
                    });
                    break;
                case "signRead":
                    if (rowData.readYet == 1) {
                        modals.info("该消息当前为已读状态，不可标记为已读");
                        return;
                    }
                    modals.confirm("是否把该消息标记为已读?", function () {
                        //
                    });
                    break;
                case "addMessage":
                    addMessage();
                    break;
                default:
                    break;
            }
        })

        //控制左边标签选中
        $("#folder ul li").removeClass("active");
        var index = parseInt($('#status').val());
        $('#folder ul li').eq(index).addClass("active");
        //初始化ICheck
        $('#message_inbox_table input[type="checkbox"]').iCheck({
            checkboxClass: 'icheckbox_flat-blue',
            radioClass: 'iradio_flat-blue'
        });
        //
        $(".checkbox-toggle").click(function () {
            var clicks = $(this).data('clicks');
            if (clicks) {
                //Uncheck all checkboxes
                $("#message_inbox_table input[type='checkbox']").iCheck("uncheck");
                $(".fa", this).removeClass("fa-check-square-o").addClass('fa-square-o');
            } else {
                //Check all checkboxes
                $("#message_inbox_table input[type='checkbox']").iCheck("check");
                $(".fa", this).removeClass("fa-square-o").addClass('fa-check-square-o');
            }
            $(this).data("clicks", !clicks);
        });
    });

    /**
     * 启用或禁用按钮，没有入口
     */
    function enableOrDisableButtons() {
        var rowId = inboxTable.getSelectedRowId();
        var btnArr = ["delete", "reply", "share", "readYes", "readNo"];
        if (rowId) {
            $.each(btnArr, function (index, flag) {
                $("[data-btn-type='" + flag + "'").removeAttr("disabled");
            });
        }
        else {
            $.each(btnArr, function (index, flag) {
                $("[data-btn-type='" + flag + "'").attr("disabled", "disabled");
            });
        }
    }

    /**
     * 点击新建消息按钮
     * @param {any} obj
     */
    function addMessage(obj) {
        window.location.href = appPath + 'Message/Add';
        /*
        if ($(obj).data("flag") == "new") {
            $(obj).text("返回收件箱");
            $(obj).data("flag", "return");
            //清除选中
            $("#folder ul li").removeClass("active");
            $(".content-header small").text("新建消息");
        }
        else if ($(obj).data("flag") == "return") {
            $("[data-btn-type='inbox']").click();
        }
        */
    }
})();