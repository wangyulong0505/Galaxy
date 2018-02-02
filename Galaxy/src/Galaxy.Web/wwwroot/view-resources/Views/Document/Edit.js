(function () {
    $(function () {
        //---初始化控件-----
        //高度自定义框
        $(".select2").select2();
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
        
        var editor;
        //初始化editormd
        editor = editormd("editormd", {                                                         //editormd为div的Id
            width: "100%",
            height: clientHeight,
            theme: (localStorage.theme) ? localStorage.theme : "default",
            previewTheme: (localStorage.previewTheme) ? localStorage.previewTheme : "default",
            editorTheme: (localStorage.editorTheme) ? localStorage.editorTheme : "default",
            path: '/lib/editor.md/lib/',                                                        //editor的存放位置
            autoHeight: isAutoHeight == "true" ? true : false,

            codeFold: true,
            searchReplace: true,
            saveHTMLToTextarea: true,                                                           //将html保存到textarea，后端从这里获取数据

            htmlDecode: "style,script,iframe",
            tex: true,//开启科学公式TeX语言支持，默认关闭
            emoji: true,//emoji表情，默认关闭
            taskList: true,
            flowChart: true,//开启流程图支持，默认关闭
            sequenceDiagram: true,//开启时序/序列图支持，默认关闭,
            markdown: markdownContent,
            syncScrolling: true,

            //图片上传
            imageUpload: true,
            imageFormats: ["jpg", "jpeg", "gif", "png", "bmp", "webp"],
            imageUploadURL: appPath + "Document/MarkdownUpload",                                             //后端的上传图片服务地址
            onchange: function () {
                localStorage.markdownContent = editor.getMarkdown();
            }
        });
        
        //markdown 默认内容
        var markdownContent = null;
        $.ajax({
            type: "get",
            url: "/lib/editor.md/README.md",
            success: function (md) {
                markdownContent = localStorage.markdownContent ? localStorage.markdownContent : md;
            }
        });

        //-----数据回填-------------
        var mid = $('#Id').val();
        if (mid == 0) {
            $("#title_sm").text("新增");
        }
        else {
            $('#title_sm').text("修改");
            $.ajax({
                url: appPath + 'Document/Markdown/' + mid,
                type: 'GET',
                data: {},
                dataType: 'JSON',
                success: function (data, textStatus) {
                    if (localStorage.mdtitle && localStorage.mdtitle != data.result.title) {
                        modals.confirm("浏览器缓存的文章与当前要编辑的文章不一致，是否替换缓存中内容？", function () {
                            fillLocalStorage(data.result);
                            $("#title").val(data.result.title);
                            $("#keywords").tagsinput("add", data.result.keywords);
                            editor.setMarkdown(data.result.content);
                        });
                    } else {
                        fillLocalStorage(data.result);
                    }
                    //$("#code").val(result.code);
                },
                error: function (XMLHttpRequest, textStatus, erronThrown) {
                    modals.error(erronThrown);
                }
            });
        }
        //文档当前的内容替换浏览器的缓存内容
        function fillLocalStorage(result) {
            localStorage.mdtitle = result.title;
            localStorage.mdkeywords = result.keywords;
            localStorage.markdownContent = result.content;
        }
        
        //文章标题, 当标题改变更新缓存内容
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
            //缓存关键字
            localStorage.mdkeywords = val;
        });
        if (localStorage.mdkeywords) {
            $("#keywords").tagsinput("add", localStorage.mdkeywords);
        }

        //主题配置
        themeSelect("editormd-theme-select", editormd.themes, "theme", function ($this, theme) {
            editor.setTheme(theme);
        });

        themeSelect("editor-area-theme-select", editormd.editorThemes, "editorTheme", function ($this, theme) {
            editor.setCodeMirrorTheme(theme);
        });

        themeSelect("preview-area-theme-select", editormd.previewThemes, "previewTheme", function ($this, theme) {
            editor.setPreviewTheme(theme);
        });

        //保存编辑内容
        
        $("#submitMD").on('click', function () {
            var token = $("input[name='GalaxyFieldName']").val();//隐藏域的名称要改
            var obj_md = {};
            obj_md["Content"] = editor.getMarkdown();
            obj_md["KeyWords"] = $("#keywords").val();
            obj_md["Title"] = $("#title").val();
            obj_md["Id"] = $('#Id').val();
            var param = {
                'Content': editor.getMarkdown(),
                'KeyWords': $("#keywords").val(),
                'Title': $("#title").val(),
                'Id': $('#Id').val()
            };
            $.ajax({
                url: appPath + 'Document/PostMarkdown',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(param),
                //headers: { "GALAXY-CSRF-HEADER": token },
                dataType: 'JSON',
                success: function (data, textStatus) {
                    if (data.success) {
                        modals.info("保存成功");
                        window.location.href = appPath + 'Document/Index';
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log(XMLHttpRequest);
                    modals.error(errorThrown);
                }
            });
            
        });
        
        $("#backMD").click(function () {
            window.location.href = appPath + 'Document/Index';
        });
    });
    
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
})()