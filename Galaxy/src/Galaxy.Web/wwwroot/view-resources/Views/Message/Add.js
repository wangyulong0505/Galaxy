(function () {
    //初始化WangEditor编辑器
    var editor = new wangEditor("editor");
    $(function () {
        editor.create();
        //初始化ICheck
        initICheck();
        //初始化fileinput控件（第一次初始化）
        $('#attachment').fileinput({
            language: 'zh', //设置语言
            uploadUrl: "/FileUpload/Upload", //上传的地址
            maxFileCount: 10,
            msgFilesTooMany: "选择上传的文件数量({n}) 超过允许的最大数值{m}！",
        });
        //数据校验
        $("#message_form").bootstrapValidator({
            message: '请输入有效值',
            /*
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            */
            fields: {
                receiverIds: {
                    validators: {
                        notEmpty: {
                            Message: '请选择接收人'
                        }
                    }
                },
                sendSubject: {
                    validators: {
                        notEmpty: {
                            Message: '标题不能为空'
                        }
                    }
                },
                messageType: {
                    validators: {
                        notEmpty: {
                            Message:'请选择消息类型'
                        }
                    }
                }
            }
        });
        //绑定按钮事件
        $("button[data-btn-type]").click(function () {
            var action = $(this).data("btn-type");
            switch (action) {
                case "backInbox":
                    backInbox();
                    break;
                case "saveDraft":
                    saveData(2);
                    break;
                case "send":
                    saveData(1);
                    break;
                case "cancel":
                    //返回收件箱首页
                    window.history.back(-1);
                    break;
                case "selectReceiver":
                    modals.openWin({
                        winId: "receiverWin",
                        title: '选择接收人',
                        width: '1000px',
                        url: appPath + "Message/receiver/select",
                        hideFunc: function () {
                            //主要是本页面wyhtml5含有modal样式影响了模态窗体的显示
                            $(document.body).removeClass('modal-open');
                            $(document.body).css("padding-right", "0px");
                        }
                    });
                    break;
            }
        });
    })

    function backInbox() {
        window.location.href = appPath + 'Message/Index';
    }

    /**
     * 初始化ICheck
     */
    function initICheck() {
        var form = $('#message_form');
        if (form.find('[data-flag="icheck"]').length > 0) {
            form.find('[data-flag="icheck"]').each(function () {
                var cls = $(this).attr("class") ? $(this).attr("class") : "square-green";
                $(this).iCheck(
                    {
                        checkboxClass: 'icheckbox_' + cls,
                        radioClass: 'iradio_' + cls
                    }
                ).on('ifChanged', function (e) { });
            });
        }
    }

    //保存消息 0=保存为草稿  1=保存并发送
    function saveData(status) {
        $('#message_form').data('bootstrapValidator').validate();
        if (!$('#message_form').data('bootstrapValidator').isValid()) {
            return;
        }
        //获取WangEditor内容，判断是否为空
        var content = editor.$txt.html();
        if (!content) {
            modals.info("正文内容不能为空");
            return;
        }
        
        //判断消息类型的值
        var str = '';
        var type = 0;
        $("input[name='messageType']:checkbox").each(function () {
            if ($(this).is(':checked') == true) {
                str += $(this).val() + ",";
            }
        });
        switch (str) {
            case '0,':
                break;
            case '1,':
                type = 1;
                break;
            case '2,':
                type = 2;
                break;
            case '0,1,':
                type = 3;
                break;
            case '0,2,':
                type = 4;
                break;
            case '1,2,':
                type = 5;
                break;
            default:
                break;
        }
        //提交的参数
        var param = {
            'Title': $('#id').val() == '' ? '0' : $('#id').val(),
            'Body': ,
            'Sender': $("#parentNodeId").val() == '' ? '0' : $("#parentNodeId").val(),
            'Recipients': $('#parentName').val(),
            'RecipientIds': $("#name").val(),
            'MessageStatus': status,
            'MessageType': type,
            'Mark': $("input[name='messageFlag']:checked").val(),
            'IsRead': 0,
            'CreateTime': formatDate(new Date(), 'yyyy-mm-dd')
        }
    }
})()