(function () {
    var editor;
    $(".select2").select2();
    //根据select的Id，动态生成option，并追加到select下面
    function themeSelect(id, themes, lsKey, callback) {
        var select = $("#" + id);
        for (var i = 0, len = themes.length; i < len; i++) {
            var theme = themes[i];
            var selected = (localStorage[lsKey] == theme) ? " selected=\"selected\"" : "";
            select.append("<option value=\"" + theme + "\"" + selected + ">" + theme + "</option>");
        }
        //绑定select的change事件
        select.bind("change", function () {
            var theme = $(this).val();

            if (theme === "") {
                return false;
            }
            localStorage[lsKey] = theme;
            callback(select, theme);
        });
        return select;
    }

    $(function () {
        //---初始化控件-----
        //高度自定义框
        $("#autoHeight").iCheck({
            checkboxClass: 'icheckbox_flat-green',
            radioClass: 'iradio_flat-green'
        });

        var isAutoHeight = localStorage.isAutoHeight ? localStorage.isAutoHeight : false;
        if (isAutoHeight == "false" || !isAutoHeight) {
            $("#autoHeight").iCheck("uncheck");
        } else {
            $("#autoHeight").iCheck("check");
        }
        //高度不自定义时高度
        var clientHeight = (document.body.clientHeight < document.documentElement.clientHeight) ? document.body.clientHeight : document.documentElement.clientHeight;

        $("#autoHeight").on("ifChanged", function (event) {
            isAutoHeight = $("#autoHeight").prop("checked");
            localStorage.isAutoHeight = isAutoHeight;
            if (editor) {
                editor.config("autoHeight", isAutoHeight);
                //alert(isAutoHeight+" --> "+clientHeight);
                if (!isAutoHeight) {
                    editor.config("height", clientHeight);
                }
            }
        });

        //-----数据回填-------------
        var mid = "8a991f87608c95700160b4c488590135";
        if (mid != '') {
            $("#title_sm").text("编辑");
            /*
            ajaxPost("/markdown/get/" + mid, null, function (result) {
                if (localStorage.mdtitle && localStorage.mdtitle != result.title) {
                    modals.confirm("浏览器缓存的文章与当前要编辑的文章不一致，是否替换缓存中内容？", function () {
                        fillLocalStorage(result);
                        $("#title").val(result.title);
                        $("#keywords").tagsinput("add", result.keywords);
                        editor.setMarkdown(result.content);
                    });
                } else {
                    fillLocalStorage(result);
                }
                $("#code").val(result.code);
            })
            */
        }
        //文档当前的内容替换浏览器的缓存内容
        function fillLocalStorage(result) {
            localStorage.mdtitle = result.title;
            localStorage.mdkeywords = result.keywords;
            localStorage.markdownContent = result.content;
        }
        //文章标题
        if (localStorage.mdtitle) {
            $("#title").val(localStorage.mdtitle);
        }
        $("#title").on("change", function (event) {
            var $title = $(event.target);
            localStorage.mdtitle = $title.val();
        })

        //keywords 关键字
        $("#keywords").on("change", function (event) {
            var $element = $(event.target);
            var val = $element.val();
            //修复placeholder问题
            if (val) {
                $element.prev(".bootstrap-tagsinput").find("input:eq(0)").attr("placeholder", null);
            } else {
                $element.prev(".bootstrap-tagsinput").find("input:eq(0)").attr("placeholder", "关键字");
            }
            localStorage.mdkeywords = val;
        });
        if (localStorage.mdkeywords) {
            //alert(localStorage.mdkeywords);
            $("#keywords").tagsinput("add", localStorage.mdkeywords);
        }

        //markdown 默认内容
        var markdownContent = null;

        $.ajax({
            type: "get",
            url: "/lib/editor.md/README.md",
            async: false,
            success: function (md) {
                markdownContent = localStorage.markdownContent ? localStorage.markdownContent : md;
            }
        });
        //初始化editormd
        editor = editormd("editormd", {
            width: "100%",
            height: clientHeight,
            theme: (localStorage.theme) ? localStorage.theme : "default",
            previewTheme: (localStorage.previewTheme) ? localStorage.previewTheme : "default",
            editorTheme: (localStorage.editorTheme) ? localStorage.editorTheme : "default",
            path: '/lib/editor.md/lib/',
            autoHeight: isAutoHeight == "true" ? true : false,

            codeFold: true,
            searchReplace: true,
            saveHTMLToTextarea: true,

            htmlDecode: "style,script,iframe",
            tex: true,
            emoji: true,
            taskList: true,
            flowChart: true,
            sequenceDiagram: true,
            markdown: markdownContent,
            syncScrolling: true,

            //图片上传
            imageUpload: true,
            imageFormats: ["jpg", "jpeg", "gif", "png", "bmp", "webp"],
            imageUploadURL: "/file/markdownUpload",
            onchange: function () {
                localStorage.markdownContent = editor.getMarkdown();
            }
        });


        themeSelect("editormd-theme-select", editormd.themes, "theme", function ($this, theme) {
            editor.setTheme(theme);
        });

        themeSelect("editor-area-theme-select", editormd.editorThemes, "editorTheme", function ($this, theme) {
            editor.setCodeMirrorTheme(theme);
        });

        themeSelect("preview-area-theme-select", editormd.previewThemes, "previewTheme", function ($this, theme) {
            editor.setPreviewTheme(theme);
        });

        //导出pdf
        $("#submitMD").click(function () {
            var obj_md = {};
            obj_md["content"] = editor.getMarkdown();
            obj_md["keywords"] = $("#keywords").val();
            obj_md["title"] = $("#title").val();
            obj_md["id"] = "8a991f87608c95700160b4c488590135";
            ajaxPost("/markdown/save", obj_md, function (result) {
                if (result.success) {
                    modals.info("保存成功");
                    returnToList();
                }
            })
        });

        $("#backMD").click(function () {
            returnToList();
        });
    });

    //返回首页
    function returnToList() {
        loadPage("/Docuemnt/Index");
    }
})()