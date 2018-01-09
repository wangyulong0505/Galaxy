(function () {
    var messageTable, winId = "messageWin";
    //初始化WangEditor编辑器
    editor = new wangEditor("editor");
    editor.create();
    //初始化fileinput控件（第一次初始化）
    $('#attachment').fileinput({
        language: 'zh', //设置语言
        uploadUrl: "/FileUpload/Upload", //上传的地址
        maxFileCount: 10,
        msgFilesTooMany: "选择上传的文件数量({n}) 超过允许的最大数值{m}！",
    });
    $(function () {
        $("#sendContent").wysihtml5();
        //数据校验
        $("#message_form").bootstrapValidator({
            message: '请输入有效值',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            submitHandler: function () {
                saveData(4);
            }
        });
        //绑定按钮事件
        $("button[data-btn-type]").click(function () {
            var action = $(this).data("btn-type");
            switch (action) {
                case "titleBtn":
                    titleBtnClick(this);
                    break;
                case "save_draft":
                    saveData(0);
                    break;
                case "cancel_draft":
                    //返回收件箱首页
                    break;
                case "receiver_select":
                    modals.openWin({
                        winId: "receiverWin",
                        title: '选择接收人',
                        width: '1000px',
                        url: basePath + "/message/receiver/select",
                        hideFunc: function () {
                            //主要是本页面wyhtml5含有modal样式影响了模态窗体的显示
                            $(document.body).removeClass('modal-open');
                            $(document.body).css("padding-right", "0px");
                        }
                    });
                    break;
            }
        });
        //更新消息数目
        updateMsgCount();
    })

    function titleBtnClick(obj) {
        if ($(obj).data("flag") == "new") {
            loadPage(basePath + "/message/edit", "#contentBody");
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
        ajaxPost(basePath + "/message/count", null, function (map) {
            $("#folder ul li a").find("span.label").each(function (index, item) {
                var btnType = $(item).parents("li").data("btn-type");
                $(item).text(map[btnType]);
            });
        });
    }
    //保存消息 0=保存为草稿  1=保存并发送
    function saveData(status) {
        var obj = messageform.getFormSimpleData();
        obj["messageStatus"] = status;
        obj["sendContent"] = $("#sendContent").val();
        //console.log(JSON.stringify(params));
        delete obj["_wysihtml5_mode"];
        console.log(obj);
        if (!validateForm()) return;
        var confirmMsg = status == 0 ? "确定保存为草稿？" : "确定保存并发送？";
        modals.confirm(confirmMsg, function () {
            ajaxPost(basePath + "/message/save", { message: JSON.stringify(obj) }, function (result) {
                if (result.success) {
                    //保存成功跳转到首页
                    $("[data-btn-type='sent']").click();
                    updateMsgCount();
                }
            });
        })
    }

    function validateForm() {
        //接收人
        var errorMsg = "";
        if (!$("#receiverIds").val()) {
            errorMsg += "接收人不能为空<br/>";
        }
        if (!$("#sendSubject").val()) {
            errorMsg += "标题不能为空<br/>";
        }
        if ($("input[name='messageType']:checked").length == 0) {
            errorMsg += "请选择消息类型<br/>";
        }
        if (!$("#sendContent").val() || $("#sendContent").val().length > 4000) {
            errorMsg += "正文内容不能为空，且字数不能大于4000";
        }
        if (errorMsg.length > 0) {
            modals.info(errorMsg);
        return false;
        } else {
            return true;
        }
    }
})()