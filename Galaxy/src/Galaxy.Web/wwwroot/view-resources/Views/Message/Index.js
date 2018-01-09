(function () {
    var inboxTable;
    var messageTable, winId = "messageWin";

    $(function () {
        var config = {
            rowClick: function (row, isSelected) {
                enableOrDisableButtons();
            }
        }
        //inboxTable = new CommonTable("message_inbox_table", "message_inbox", "searchDiv", config);
        
        $("[data-btn-type]").click(function () {
            var btnType = $(this).data("btn-type");
            //var rId = inboxTable.getSelectedRowId();
            //var rowData = inboxTable.getSelectedRowData();
            var rId = 1;
            var rowData = { readYet: 1 };
            switch (btnType) {
                case "delete":
                    modals.confirm("是否将该消息放入回收站?", function () {
                        ajaxPost("/message/receiver/trash/" + rId, null, function () {
                            inboxTable.reloadData();
                            updateMsgCount();
                        });
                    });
                    break;
                case "reply":
                    loadPage("/message/reply/" + rId, "#contentBody");
                    $(".content-header small").text("回复消息");
                    break;
                case "share":
                    loadPage("/message/share/" + rId, "#contentBody");
                    $(".content-header small").text("转发消息");
                    break;
                case "readNo":
                    if (rowData.readYet == 0) {
                        modals.info("该消息当前为未读状态，不可标记为未读");
                        return;
                    }
                    modals.confirm("是否将该消息标记为未读?", function () {
                        ajaxPost("/message/receiver/readUpdate/" + rId, null, function () {
                            inboxTable.reloadData();
                        });
                    });
                    break;
                case "readYes":
                    if (rowData.readYet == 1) {
                        modals.info("该消息当前为已读状态，不可标记为已读");
                        return;
                    }
                    modals.confirm("是否把该消息标记为已读?", function () {
                        ajaxPost("/message/receiver/readUpdate/" + rId, null, function () {
                            inboxTable.reloadData();
                        });
                    });
                    break;
                case "titleBtn":
                    titleBtnClick(this);
                    break;
                default:
                    break;
            }
        })

        //为我的文件夹绑定事件
        $("#folder ul li").click(function () {
            //清除其他选中样式
            $("#folder ul li").removeClass("active");
            $(this).addClass("active");
            //改变title
            $(".content-header small").text($(this).find("a:eq(0)").children().eq(1).html());
            //加载内容
            loadPage($(this).data("url"), "#contentBody");
            //
            //$("[data-btn-type='titleBtn']"),text();
            $("[data-btn-type='titleBtn']").text("新建消息");
            $("[data-btn-type='titleBtn']").data("flag", "new");
            //$(obj).text("新建消息");
        });

        //初始化选中
        $("[data-btn-type='inbox']").click();

        updateMsgCount();
    });
    //点击标题，阅读邮件
    function readMsg(id) {
            //更新阅读标记和阅读时间
        loadPage("/message/receiver/read/" + id, "#contentBody");
        $(".content-header small").text("阅读消息");
    }

    function enableOrDisableButtons() {
        var rowId = inboxTable.getSelectedRowId();
        var btnArr = ["delete", "reply", "share", "readYes", "readNo"];
        if (rowId) {
            $.each(btnArr, function (index, flag) {
                $("[data-btn-type='" + flag + "'").removeAttr("disabled");
            })
        } else {
            $.each(btnArr, function (index, flag) {
                $("[data-btn-type='" + flag + "'").attr("disabled", "disabled");
            })
        }
    }

    function titleBtnClick(obj) {
        if ($(obj).data("flag") == "new") {
            loadPage("/message/edit", "#contentBody");
            $(obj).text("返回收件箱");
            $(obj).data("flag", "return");
            //清除选中
            $("#folder ul li").removeClass("active");
            $(".content-header small").text("新建消息");
        } else if ($(obj).data("flag") == "return") {
            //loadPage(basePath+"/message/inbox","#contentBody");
            //$(obj).data("flag","new");
            $("[data-btn-type='inbox']").click();
        }
    }

    function updateMsgCount() {
        ajaxPost("/message/count", null, function (map) {
            $("#folder ul li a").find("span.label").each(function (index, item) {
                var btnType = $(item).parents("li").data("btn-type");
                $(item).text(map[btnType]);
            });
        });
    }
})()