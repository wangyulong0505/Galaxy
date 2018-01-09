(function () {
    var messageform;
    var id = "8a8a84995ab76f13015abbc8b8a00000";
    //初始化fileinput控件, 获取附件填充fileinput
    /* 预览页面加载File Input插件的附件， 目前只搞图片的
     * 1、initialPreview: [] 数组里面写images的路径，只写src的值，多个以逗号隔开
     * 2、initialPreviewConfig: [] 数组里面有很多属性，常用属性如下 
     *      caption： 附件名称， width：附件宽度， url：服务器上删除方法， key：主键， extra：另外的数据， size：附件的大小（B单位）
     *
     *
     *
     *
     *
     */
    $('#attachment').fileinput({
        language: 'zh',
        validateInitialCount: true,
        overwriteInitial: false,
        showUpload: false,              //上传按钮
        showBrowse: false,              //
        showRemove: false,
        showCaption: false,             //文字描述
        showDrag: false,                 //拖放
        allowedPreviewTypes: ['image', 'html', 'txt', 'video', 'audio', 'flash', 'pdf', 'object'],
        previewFileIcon: '<i class="fa fa-file-o"></i>',
        initialPreviewShowDelete: false,        //显示删除按钮
        previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",
        otherActionButtons: '<button type="button" class="kv-file-down btn btn-kv btn-default" {dataKey} title="下载附件"><i class="fa fa-cloud-download"></i></button>',   //自定义下载按钮
        initialPreview: [
            '../../../images/desert.jpg',
        ],
        previewSettings: {
            width: "100%",
        },
        initialPreviewAsData: true, // identify if you are sending preview data only and not the raw markup
        initialPreviewFileType: 'image', // image is the default and can be overridden in config below
        initialPreviewConfig: [
            {
                caption: 'desert.jpg',
                width: 'auto',
                url: 'http://localhost/avatar/delete', // server delete action 
                key: 120,
                extra: { id: 100 },
                size: 102400
            }
        ],
        fileActionSettings: {
            showZoom: true,
            showRemove: false,
            showDownload: true,
        }
    });
    $(function () {
        //初始化表单
        messageform = $("#message_form").form();
        //初始化控件
        messageform.initComponent();
        //编辑回填
        if (id != 0) {
            ajaxPost(basePath + "/message/get/" + id, null, function (data) {
                messageform.initFormData(data);
                //回填内容（含html的内容使用form组件回填有异常，所以手动回填）
                $("#messageType").html(fnRenderMessageType(data.messageType));
                $("#messageFlag").html(fnRenderMessageFlag(data.messageFlag));
                $("#sendContent").html(data.sendContent);
                //填报人回填
                if (data.receiverType == 0) {
                    ajaxPost(basePath + "/message/receiver/user/group", { "groupIds": data.receiverIds },
                        function (map) {
                            $("#receiverUsers").text(map.name);
                        });
                }
                else {
                    ajaxPost(basePath + "/message/receiver/user/names", { userIds: data.receiverIds },
                        function (map) {
                            $("#receiverUsers").text(map.name);
                        }
                    );
                }

            })
        }
        //取消，返回到收件箱
        $("[data-btn-type='cancel_draft']").click(function () {
            $("[data-btn-type='sent']").click();
        });
    });
    var messageTable, winId = "messageWin";
    $(function () {
        //绑定按钮事件
        $("button[data-btn-type]").click(function () {
            var action = $(this).data("btn-type");
            switch (action) {
                case "titleBtn":
                    titleBtnClick(this);
                    break;
            }
        });
        updateMsgCount();
    });
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
})()