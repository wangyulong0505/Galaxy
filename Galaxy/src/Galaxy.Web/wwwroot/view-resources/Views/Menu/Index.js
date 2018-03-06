(function () {
    $(function () {
        var form = $('#function-form');
        initTree(0);
        //初始化校验
        $('#function-form').bootstrapValidator({
            message: '请输入有效值',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            /*
            submitHandler: function (validator, functionform, submitButton) {
                modals.confirm('确认保存？', function () {
                    //Save Data，对应'submit-提交'
                    var params = form.getFormSimpleData();
                    ajaxPost(basePath + '/function/save', params, function (data, status) {
                        if (data.success) {
                            //var id=$("input[name='id']").val();
                            var selectedArr = $("#tree").data("treeview").getSelected();
                            var selectedNodeId = selectedArr.length > 0 ? selectedArr[0].nodeId : 0;
                            initTree(selectedNodeId);
                        }
                    });
                });
            },
            */
            fields: {
                name: {
                    validators: {
                        notEmpty: {
                            message: '请输入名称'
                        }
                    }
                },
                code: {
                    validators: {
                        notEmpty: {
                            message: '请输入编码'
                        },
                        /*
                        remote: {
                            url: basePath + "/base/checkUnique",
                            data: function (validator) {
                                return {
                                    className: 'com.cnpc.framework.base.entity.Function',
                                    fieldName: 'code',
                                    fieldValue: $('#code').val(),
                                    id: $('#id').val()
                                };
                            },
                            message: '该编码已被使用'
                        }
                        */
                    }
                },
                levelCode: {
                    validators: {
                        notEmpty: {
                            message: '请输入层级编码'
                        }
                    }
                },
                functype: {
                    validators: {
                        notEmpty: {
                            message: '请选择菜单类型'
                        }
                    }
                },
                status: {
                    validators: {
                        notEmpty: {
                            message: '请选择是否可用'
                        }
                    }
                }
            }
        });
        //按钮事件
        $('button[data-btn-type]').click(function () {
            var action = $(this).attr('data-btn-type');
            var selectedArr = $("#tree").data("treeview").getSelected();
            var selectedNode = selectedArr.length > 0 ? selectedArr[0] : null;
            switch (action) {
                case 'addRoot':
                    formWritable(action);
                    clearForm();
                    $("#icon_i").removeClass();
                    //填充上级菜单和层级编码
                    fillParentAndLevelCode(null);
                    $("#parentNodeName").attr("readonly", "readonly");
                    btntype = 'add';
                    break;
                case 'add':
                    if (!selectedNode) {
                        modals.info('请先选择上级菜单');
                        return false;
                    }
                    formWritable(action);
                    clearForm();
                    $("#icon_i").removeClass();
                    //填充上级菜单和层级编码
                    fillParentAndLevelCode(selectedNode);
                    $("#parentNodeName").attr("readonly", "readonly");
                    btntype = 'add';
                    break;
                case 'edit':
                    if (!selectedNode) {
                        modals.info('请先选择要编辑的节点');
                        return false;
                    }
                    if (btntype == 'add') {
                        fillDictForm(selectedNode);
                    }
                    formWritable(action);
                    btntype = 'edit';
                    break;
                case 'delete':
                    if (!selectedNode) {
                        modals.info('请先选择要删除的节点');
                        return false;
                    }
                    if (btntype == 'add') {
                        fillDictForm(selectedNode);
                    }
                    formReadonly();
                    $(".box-header button[data-btn-type='delete']").removeClass("btn-default").addClass("btn-primary");
                    if (selectedNode.nodes) {
                        modals.info('该节点含有子节点，请先删除子节点');
                        return false;
                    }
                    modals.confirm('是否删除该节点', function () {
                        $.ajax({
                            url: appPath + 'Menu/DeleteMenu/' + selectedNode.id,
                            data: {},
                            type: 'POST',
                            dataType: 'JSON',
                            success: function (data, textStatus) {
                                if (data.success) {
                                    modals.correct('删除成功');
                                    //定位
                                    var brothers = $("#tree").data("treeview").getSiblings(selectedNode);
                                    if (brothers.length > 0)
                                        initTree(brothers[brothers.length - 1].nodeId);
                                    else {
                                        var parent = $("#tree").data("treeview").getParent(selectedNode);
                                        initTree(parent ? parent.nodeId : 0);
                                    }
                                }
                                else {
                                    modals.warn(data.result);
                                }
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                modals.error(errorThrown);
                            }
                        });
                    });
                    break;
                case 'cancel':
                    if (btntype == 'add') {
                        fillDictForm(selectedNode);
                    }
                    formReadonly();
                    break;
                case 'selectIcon':
                    var disabled = $(this).hasClass("disabled");
                    if (disabled) {
                        break;
                    }
                    var iconName;
                    if ($("#icon").val()) {
                        iconName = encodeURIComponent($("#icon").val());
                    }
                    $('#iconWin').modal('show');
                    break;
                case 'save':
                    formSave();
                    break;
            }
        });

        var iconName = "glyphicon glyphicon-eur";
        //单击 选中样式
        $("#fa-icons div.col-md-3,#glyphicons li").click(function () {
            $("#fa-icons div.col-md-3,#glyphicons li").removeClass("icon-selected");
            $(this).addClass("icon-selected");
        });
        //双击获取图标名称 关闭窗口 并回填数据
        var iconClass = null;
        $("#fa-icons div.col-md-3,#glyphicons li").dblclick(function () {
            var fa_len = $(this).find("i.fa").length;
            var gl_len = $(this).find("span.glyphicon").length;
            if (fa_len == 1) {
                iconClass = $(this).find("i.fa").eq(0).attr("class").replace('fa-fw ', '');
            } else {
                iconClass = $(this).find("span.glyphicon").eq(0).attr("class");
            }
            $('#iconWin').modal('hide');
            fillBackIconName(iconClass);
        });
        //选中回填的图标
        if (iconName && iconName != "undefined") {
            iconName = iconName.replace(" ", ".");
            if (iconName.indexOf("fa") > -1) {
                $("#fa-icons div.col-md-3 i." + iconName).parent().addClass("icon-selected");
            }
            else {
                $("#tab-gl").click();
                $("#glyphicons li span." + iconName).parent().addClass("icon-selected");
            }
        }
    });

    function initTree(selectNodeId) {
        $.ajax({
            url: appPath + 'Menu/GetTreeData',
            data: null,
            type: 'GET',
            dataType: 'json',
            success: function (data, textStatus) {
                if (data.success) {
                    console.log(data.result);
                    var treeData = data.result;
                    $("#tree").treeview({
                        //showCheckbox: true,   //是否显示复选框
                        highlightSelected: true,    //是否高亮选中
                        data: treeData,
                        showBorder: true,
                        //nodeIcon: 'fa fa-wrench',
                        emptyIcon: '',    //没有子节点的节点图标
                        multiSelect: false,    //多选
                        expandIcon: "glyphicon glyphicon-plus",
                        collapseIcon: "glyphicon glyphicon-minus",
                        levels: 5,                  //最多可扩展几层
                        /*
                        onNodeChecked: function (event, data) {
                            //复选框勾选触发
                            console.log(data.Id);
                        },
                        */
                        onNodeSelected: function (event, node) {
                            fillOrgForm(node.id);
                        }
                    });
                    if (!treeData || treeData.length == 0) {
                        return;
                    }
                    //默认选中第一个节点
                    selectNodeId = selectNodeId || 0;
                    $("#tree").data('treeview').selectNode(selectNodeId);
                    $("#tree").data('treeview').expandNode(selectNodeId);
                    $("#tree").data('treeview').revealNode(selectNodeId);
                }
                else {
                    modals.warn(data.result);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                modals.error(errorThrown);
            }
        });
    }
    /**
     * From初始化
     * @param {any} data
     */
    function initForm(data) {
        $('#id').val(data.Id);
        $('#createDate').val(data.CreateDate);
        $("#parentNodeId").val(data.ParentNodeId);
        $('#parentNodeName').val(data.ParentNodeName);
        $("#name").val(data.Name);
        $('#code').val(data.Code);
        $("input[name='menuType'][value='" + data.MenuType + "']").prop("checked", "checked");
        $('#levelCode').val(data.LevelCode);
        $('#menuIcon').val(data.MenuIcon);
        $('#icon_i').removeClass();
        $('#icon_i').addClass('form-control-feedback')
        $('#icon_i').addClass(data.MenuIcon);
        $("input[name='status'][value='" + data.Status + "']").prop("checked", "checked");
        $('#remark').val(data.Remark);
    }
    /**
     * 清除Form内容
     */
    function clearForm() {
        $("input[type=text],input[type=hidden],textarea[name]").each(function () {
            $(this).val('');
        })
    }
    //新增时，带入父级菜单名称id,自动生成levelcode
    function fillParentAndLevelCode(selectedNode) {
        console.log(selectedNode);
        $("input[name='parentNodeName']").val(selectedNode ? selectedNode.text : '系统菜单');
        $("input[name='status'][value='0']").prop("checked", "checked");
        if (selectedNode) {
            $("input[name='parentNodeId']").val(selectedNode.id);
            var nodes = selectedNode.nodes;
            var levelCode = nodes ? nodes[nodes.length - 1].levelCode : null;
            $("input[name='levelCode']").val(getNextCode(selectedNode.levelCode, levelCode, 6));
        } else {
            var parentNode = $("#tree").data("treeview").getNode(0);
            var levelCode = "000000";
            if (parentNode) {
                var brothers = $("#tree").data("treeview").getSiblings(0);
                levelCode = brothers.length > 1 ? brothers[brothers.length - 1].levelCode : null;
            }
            $("input[name='levelCode']").val(getNextCode("", levelCode, 6));
        }
    }

    //填充form
    function fillDictForm(node) {
        clearForm();
        alert("功能未实现");
        return;
        ajaxPost(basePath + "/function/get/" + node.id, null, function (data) {
            form.initFormData(data);
            fillBackIconName(data.icon);
        })
    }

    //设置form为只读
    function formReadonly() {
        //所有文本框只读
        $("input[name],textarea[name]").attr("readonly", "readonly");
        //隐藏取消、保存按钮
        $("#function-form .box-footer").hide();
        //还原新增、编辑、删除按钮样式
        $(".box-header button").removeClass("btn-primary").addClass("btn-default");
        //选择图标按钮只读
        $("#selectIcon").addClass("disabled");
        //还原校验框
        if ($("function-form").data('bootstrapValidator')) {
            $("function-form").data('bootstrapValidator').resetForm();
        }
    }

    function formWritable(action) {
        $("input[name],textarea[name]").removeAttr("readonly");
        $("#function-form .box-footer").show();
        $(".box-header button").removeClass("btn-primary").addClass("btn-default");
        $("#selectIcon").removeClass("disabled");
        if (action) {
            $(".box-header button[data-btn-type='" + action + "']").removeClass("btn-default").addClass("btn-primary");
        }
    }

    //填充form
    function fillOrgForm(node) {
        //先清除表单的数据，然后ajax调用获取的数据重新初始化表单
        $.ajax({
            url: appPath + 'Menu/GetNodeData/' + node,
            type: 'GET',
            data: null,
            dataType: 'json',
            success: function (data, textStatus) {
                //data.result 初始化表单
                initForm(JSON.parse(data.result));
                formReadonly();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                modals.error(errorThrown);
            }
        })
    }

    //回填图标
    function fillBackIconName(icon_name) {
        $("#menuIcon").val(icon_name);
        $("#icon_i").removeClass().addClass("form-control-feedback").addClass(icon_name);
    }

    /**
     * 提交表单
     */
    function formSave() {
        //获取表单对象
        var bootstrapValidator = $('#function-form').data('bootstrapValidator');
        var token = $("input[name='GalaxyFieldName']").val();//隐藏域的名称要改
        //手动触发验证
        bootstrapValidator.validate();
        if (bootstrapValidator.isValid()) {
            var param = {
                'Id': $('#id').val() == '' ? '0' : $('#id').val(),
                'URL': $('#url').val(),
                'CreateDate': formatDate(new Date(), 'yyyy-mm-dd'),
                'ParentNodeId': $("#parentNodeId").val() == '' ? '0' : $("#parentNodeId").val(),
                'ParentNodeName': $('#parentNodeName').val(),
                'Name': $("#name").val(),
                'Code': $('#code').val(),
                'MenuType': $("input[name='menuType']:checked").val(),
                'MenuIcon': $("#menuIcon").val(),
                'LevelCode': $('#levelCode').val(),
                'Status': $("input[name='status']:checked").val(),
                'Remark': $('#remark').val(),
            }
            modals.confirm('确认保存？', function () {
                $.ajax({
                    contentType: 'application/json; charset=utf-8',
                    url: appPath + 'Menu/SaveChange',
                    type: 'POST',
                    //headers: { "GALAXY-CSRF-HEADER": token },
                    data: JSON.stringify(param),
                    dataType: 'json',
                    success: function (data, textStatus) {
                        if (data.success) {
                            var selectedArr = $("#tree").data("treeview").getSelected();
                            console.log(selectedArr);
                            var selectedNodeId = selectedArr.length > 0 ? selectedArr[0].id : 0;
                            initTree(selectedNodeId);
                            //取消form验证
                            $('#function-form').bootstrapValidator('resetForm')
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        modals.error(errorThrown);
                    }
                });
            });
        }
    }

    /**
     * 动态获取下一个编码
     * @param {any} prefix
     * @param {any} maxCode
     * @param {any} length
     */
    function getNextCode(prefix, maxCode, length) {
        if (maxCode == null) {
            var str = "";
            for (var i = 0; i < length - 1; i++) {
                str += "0";
            }
            return prefix + str + 1;
        } else {
            var str = "";
            var sno = parseInt(maxCode.substring(prefix.length)) + 1;
            for (var i = 0; i < length - sno.toString().length; i++) {
                str += "0";
            }
            return prefix + str + sno;
        }

    }
    /**
     * 日期格式化
     * @param {any} date
     * @param {any} format
     */
    function formatDate(date, format) {
        if (!date) return date;
        date = (typeof date == "number") ? new Date(date) : date;
        return date.Format(format);
    }

    Date.prototype.Format = function (fmt) {
        var o = {
            "m+": this.getMonth() + 1, // 月份
            "d+": this.getDate(), // 日
            "h+": this.getHours(), // 小时
            "i+": this.getMinutes(), // 分
            "s+": this.getSeconds(), // 秒
            "q+": Math.floor((this.getMonth() + 3) / 3), // 季度
            "S": this.getMilliseconds()
            // 毫秒
        };
        if (/(y+)/.test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        }
        for (var k in o) {
            if (new RegExp("(" + k + ")").test(fmt)) {
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            }
        }
        return fmt;
    }
})();