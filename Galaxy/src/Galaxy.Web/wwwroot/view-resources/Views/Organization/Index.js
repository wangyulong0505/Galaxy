(function () {
    $(function () {
        //初始化表单验证
        $("#org-form").bootstrapValidator({
            message: '请输入有效值',
            /*
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
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
                            url: appPath + "Organization/CheckCodeUnique",
                            delay: 2000,
                            
                            data: function (validator) {
                                console.log(validator);
                                return {
                                    fieldValue: $('#code').val(),
                                    id: $('#id').val()
                                };
                            },
                            
                            message: '该编码已被使用'
                        }
                        */
                    }
                },
                organizationType: {
                    validators: {
                        notEmpty: {
                            message: '请选择机构类型'
                        }
                    }
                },
                levelCode: {
                    validators: {
                        notEmpty: {
                            message: '请输入层级编码'
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
            },
            /* 版本不支持，注释掉
            submitHandler: function (form) {
                modals.confirm('确认保存？', function () {
                    $.ajax({
                        url: appPath + 'Organization/SaveChange',
                        type: 'POST',
                        data: $("#org-form").serialize(),
                        dataType: 'json',
                        success: function (data, textStatus) {
                            if (data.success) {
                                var selectedArr = $("#tree").data("treeview").getSelected();
                                var selectedNodeId = selectedArr.length > 0 ? selectedArr[0].nodeId : 0;
                                self.initTree(selectedNodeId);
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            modals.error(errorThrown);
                        }
                    });
                });
            }
            */
        });
        initICheck();
        //初始化菜单树
        initTree(0);
        //初始化按钮事件
        //initBtnEvent();
    });
    /**
     * 初始化机构单选框
     */
    function initICheck() {
        var form = $('#org-form');
        if (form.find('[data-flag="icheck"]').length > 0) {
            form.find('[data-flag="icheck"]').each(function () {
                var cls = $(this).attr("class") ? $(this).attr("class") : "square-green";
                $(this).iCheck(
                    {
                        checkboxClass: 'icheckbox_' + cls,
                        radioClass: 'iradio_' + cls
                    }
                ).on('ifChanged', function (e) {
                    var field = $(this).attr('name');
                    var validator = form.data('bootstrapValidator');
                    if (validator && validator.options.fields[field])
                        validator.updateStatus(field, 'NOT_VALIDATED', null).validateField(field);
                });
            });
        }
    }
    //根据选中的Id初始化树
    function initTree (selectNodeId) {
        $.ajax({
            url: appPath + 'Organization/GetTreeData',
            data: null,
            type: 'GET',
            dataType: 'json',
            success: function (data, textStatus) {
                if (data.success) {
                    var treeData = data.result;
                    $("#tree").treeview({
                        //showCheckbox: true,   //是否显示复选框
                        highlightSelected: true,    //是否高亮选中
                        data: treeData,
                        showBorder: true,
                        nodeIcon: 'glyphicon glyphicon-globe',
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
    //填充form
    function fillOrgForm(node) {
        //先清除表单的数据，然后ajax调用获取的数据重新初始化表单
        $.ajax({
            url: appPath + 'Organization/GetNodeData/' + node,
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
    /**
     * 新增时，带入父级机构名称id,自动生成levelcode
     * @param {any} selectedNode
     */
    function fillParentAndLevelCode(selectedNode) {
        console.log(selectedNode);
        $("input[name='parentNodeName']").val(selectedNode ? selectedNode.text : '组织机构');
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
    };
    //设置form为只读
    function formReadonly () {
        //所有文本框只读
        $("input[name],textarea[name]").attr("readonly", "readonly");
        //icheck只读

        //隐藏取消、保存按钮
        $("#org-form .box-footer").hide();
        //还原新增、编辑、删除按钮样式
        $(".box-header button").removeClass("btn-primary").addClass("btn-default");
        //还原校验框
        if ($("#org-form").data('bootstrapValidator'))
            $("#org-form").data('bootstrapValidator').resetForm();
    }
    function formWritable (action) {
        $("input[name],textarea[name]").removeAttr("readonly");
        $("#org-form .box-footer").show();
        $(".box-header button").removeClass("btn-primary").addClass("btn-default");
        if (action) {
            $(".box-header button[data-btn-type='" + action + "']").removeClass("btn-default").addClass("btn-primary");
        }
    }
    $('button[data-btn-type]').click(function () {
        var action = $(this).attr('data-btn-type');
        var selectedArr = $("#tree").data("treeview").getSelected();
        var selectedNode = selectedArr.length > 0 ? selectedArr[0] : null;
        switch (action) {
            case 'addRoot':
                addRootNode(action);
                break;
            case 'add':
                addNode(action, selectedNode);
                break;
            case 'edit':
                editNode(action, selectedNode);
                break;
            case 'delete':
                deleteNode(selectedNode);
                break;
            case 'cancel':
                cancelOpts(selectedNode);
                break;
            case 'save':
                formSave();
                break;
        }
    });
    /**
     * From初始化
     * @param {any} data
     */
    function initForm(data) {
        $('#id').val(data.Id);
        $('#createDate').val(data.CreateDate);
        $("#parentNodeId").val(data.ParentNodeId);
        $('#parentName').val(data.ParentNodeName);
        $("#name").val(data.Name);
        $('#code').val(data.Code);
        $("input[name='organizationType'][value='" + data.OrganizationType + "']").iCheck('check');;
        $('#levelCode').val(data.LevelCode);
        $("input[name='status'][value='" + data.Status + "']").prop("checked", "checked");
        $('#remark').val(data.Remark);
    }
    function clearForm() {
        $("input[type=text],input[type=hidden],textarea[name]").each(function () {
            $(this).val('');
        })
    }
    /**
     * 添加根节点
     * @param {any} action
     */
    function addRootNode (action) {
        formWritable(action);
        clearForm();
        //填充上级机构和层级编码
        fillParentAndLevelCode(null);
        this.btntype = 'add';
    }
    /**
     * 添加子节点
     * @param {any} action
     * @param {any} selectedNode
     */
    function addNode (action, selectedNode) {
        if (!selectedNode) {
            modals.info('请先选择上级机构');
            return false;
        }
        formWritable(action);
        clearForm();
        //填充上级机构和层级编码
        fillParentAndLevelCode(selectedNode);
        this.btntype = 'add';
    }
    /**
     * 编辑节点
     * @param {any} action
     * @param {any} selectedNode
     */
    function editNode (action, selectedNode) {
        if (!selectedNode) {
            modals.info('请先选择要编辑的节点');
            return false;
        }
        if (this.btntype == 'add') {
            fillOrgForm(selectedNode);
        }
        formWritable(action);
        this.btntype = 'edit';
    }
    /**
     * 删除节点
     * @param {any} selectedNode
     */
    function deleteNode (selectedNode) {
        var self = this;
        if (!selectedNode) {
            modals.info('请先选择要删除的节点');
            return false;
        }
        if (this.btntype == 'add') {
            fillOrgForm(selectedNode);
        }
        this.formReadonly();
        $(".box-header button[data-btn-type='delete']").removeClass("btn-default").addClass("btn-primary");
        if (selectedNode.nodes) {
            modals.info('该节点含有子节点，请先删除子节点');
            return false;
        }
        modals.confirm('是否删除该节点', function () {
            $.ajax({
                url: '/Organization/DeleteNode/' + selectedNode.id,
                type: 'POST',
                data: null,
                dataType: 'json',
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
    }
    /**
     * 取消编辑
     * @param {any} selectedNode
     */
    function cancelOpts (selectedNode) {
        if (this.btntype == 'add') {
            fillOrgForm(selectedNode);
        }
        formReadonly();
    }
    /**
     * 提交表单
     */
    function formSave() {
        //获取表单对象
        var bootstrapValidator = $('#org-form').data('bootstrapValidator');
        var token = $("input[name='GalaxyFieldName']").val();//隐藏域的名称要改
        //手动触发验证
        bootstrapValidator.validate();
        if (bootstrapValidator.isValid()) {
            var param = {
                'Id': $('#id').val() == '' ? '0' : $('#id').val(),
                'CreateDate': formatDate(new Date(), 'yyyy-mm-dd'),
                'ParentNodeId': $("#parentNodeId").val() == '' ? '0' : $("#parentNodeId").val(),
                'ParentNodeName': $('#parentName').val(),
                'Name': $("#name").val(),
                'Code': $('#code').val(),
                'OrganizationType': $("input[name='organizationType']:checked").val(),
                'LevelCode': $('#levelCode').val(),
                'Status': $("input[name='status']:checked").val(),
                'Remark': $('#remark').val(),
            }
            modals.confirm('确认保存？', function () {
                $.ajax({
                    contentType: 'application/json; charset=utf-8',
                    url: appPath + 'Organization/SaveChange',
                    type: 'POST',
                    //headers: { "GALAXY-CSRF-HEADER": token },
                    data: JSON.stringify(param),
                    dataType: 'json',
                    success: function (data, textStatus) {
                        if (data.success) {
                            var selectedArr = $("#tree").data("treeview").getSelected();
                            var selectedNodeId = selectedArr.length > 0 ? selectedArr[0].id : 0;
                            initTree(selectedNodeId);
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
        if (/(y+)/.test(fmt))
            fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt))
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return fmt;
    }
})();