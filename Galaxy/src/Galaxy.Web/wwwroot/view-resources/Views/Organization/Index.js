(function () {
    $(function () {
        //初始化表单验证
        $("#org-form").bootstrapValidator({
            message: '请输入有效值',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            submitHandler: function (validator, orgform, submitButton) {
                modals.confirm('确认保存？', function () {
                    //Save Data，对应'submit-提交'
                    var params = self.form.getFormSimpleData();
                    $.ajax({
                        url: '/org/save',
                        type: 'POST',
                        data: params,
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
            },
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
                        remote: {
                            url: "/base/checkUnique",
                            data: function (validator) {
                                return {
                                    className: 'com.cnpc.framework.base.entity.Org',
                                    fieldName: 'code',
                                    fieldValue: $('#code').val(),
                                    id: $('#id').val()
                                };
                            },
                            message: '该编码已被使用'
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
                deleted: {
                    validators: {
                        notEmpty: {
                            message: '请选择是否可用'
                        }
                    }
                }
            }
        });
        //初始化菜单树
        initTree(0);
        //初始化按钮事件
        initBtnEvent();
    });
    //根据选中的Id初始化树
    function initTree (selectNodeId) {
        var treeData = null;
        $.ajax({
            url: '/Organization/GetTreeData',
            data: null,
            type: 'GET',
            dataType: 'json',
            success: function (data, textStatus) {
                treeData = data;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                modals.error(errorThrown);
            }
        })
        $("#tree").treeview({
            data: treeData,
            showBorder: true,
            expandIcon: "glyphicon glyphicon-stop",
            collapseIcon: "glyphicon glyphicon-unchecked",
            levels: 1,
            onNodeSelected: function (event, data) {
                fillOrgForm(data);
                formReadonly();
            }
        });
        if (treeData.length == 0)
            return;
        //默认选中第一个节点
        selectNodeId = selectNodeId || 0;
        $("#tree").data('treeview').selectNode(selectNodeId);
        $("#tree").data('treeview').expandNode(selectNodeId);
        $("#tree").data('treeview').revealNode(selectNodeId);
    }
    //填充form
    function fillOrgForm (node) {
        //先清除表单的数据，然后ajax调用获取的数据重新初始化表单
        $.ajax({
            url: '/Organization/GetNodeData/' + node,
            type: 'GET',
            data: null,
            dataType: 'json',
            success: function (data, textStatus) {
                //data.result 初始化表单
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                modals.error(errorThrown);
            }
        })
        ajaxPost(basePath + "/org/get/" + node.id, null, function (data) {
            self.form.initFormData(data);
        })
    }
    //新增时，带入父级机构名称id,自动生成levelcode
    function fillParentAndLevelCode (selectedNode) {
        $("input[name='parentName']").val(selectedNode ? selectedNode.text : '组织机构');
        $("input[name='deleted'][value='0']").prop("checked", "checked");
        if (selectedNode) {
            $("input[name='parentId']").val(selectedNode.id);
            var nodes = selectedNode.nodes;
            var levelCode = nodes ? nodes[nodes.length - 1].levelCode : null;
            $("input[name='levelCode']").val(getNextCode(selectedNode.levelCode, levelCode, 6));
        } else {
            var parentNode = $("#tree").data("treeview").getNode(0);
            var levelCode = "000000";
            if (parentNode) {
                var brothers = $("#tree").data("treeview").getSiblings(0);
                levelCode = brothers[brothers.length - 1].levelCode;
            }
            $("input[name='levelCode']").val(getNextCode("", levelCode, 6));
        }
    },
    //设置form为只读
    function formReadonly () {
        //所有文本框只读
        $("input[name],textarea[name]").attr("readonly", "readonly");
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
        if (action)
            $(".box-header button[data-btn-type='" + action + "']").removeClass("btn-default").addClass("btn-primary");
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
        }
    });

    function addRootNode (action) {
        formWritable(action);
        this.form.clearForm();
        //填充上级机构和层级编码
        fillParentAndLevelCode(null);
        this.btntype = 'add';
    }
    function addNode (action, selectedNode) {
        if (!selectedNode) {
            modals.info('请先选择上级机构');
            return false;
        }
        formWritable(action);
        this.form.clearForm();
        //填充上级机构和层级编码
        fillParentAndLevelCode(selectedNode);
        this.btntype = 'add';
    }
    function editNode (action, selectedNode) {
        if (!selectedNode) {
            modals.info('请先选择要编辑的节点');
            return false;
        }
        if (this.btntype == 'add') {
            this.fillOrgForm(selectedNode);
        }
        this.formWritable(action);
        this.btntype = 'edit';
    }
    function deleteNode (selectedNode) {
        var self = this;
        if (!selectedNode) {
            modals.info('请先选择要删除的节点');
            return false;
        }
        if (this.btntype == 'add')
            this.fillOrgForm(selectedNode);
        this.formReadonly();
        $(".box-header button[data-btn-type='delete']").removeClass("btn-default").addClass("btn-primary");
        if (selectedNode.nodes) {
            modals.info('该节点含有子节点，请先删除子节点');
            return false;
        }
        modals.confirm('是否删除该节点', function () {
            $.ajax({
                url: '/org/delete/' + selectedNode.id,
                type: 'POST',
                data: null,
                dataType: 'json',
                success: function (data, textStatus) {
                    modals.correct('删除成功');
                    //定位
                    var brothers = $("#tree").data("treeview").getSiblings(selectedNode);
                    if (brothers.length > 0)
                        self.initTree(brothers[brothers.length - 1].nodeId);
                    else {
                        var parent = $("#tree").data("treeview").getParent(selectedNode);
                        self.initTree(parent ? parent.nodeId : 0);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    modals.error(errorThrown);
                }
            });
        });
    }
    function cancelOpts (selectedNode) {
        if (this.btntype == 'add')
            this.fillOrgForm(selectedNode);
        this.formReadonly();
    }

})();