(function () {
    $(function () {
        initTree(0);
        //RoleTable tr选中样式
        $('#role_table tr').click(function () {
            //一次只能选中一行
            $(this).siblings().css('background-color', '#fff');
            $(this).addClass("trchange").siblings().removeClass("trchange");
            $(this).css('background-color', '#08C');
            var id = $(this).attr('id');
            $.ajaxSettings.async = false; 
            $.get(appPath + 'Permission/GetPermissions/' + id, function (data) {
                console.log(data.result);
                $('#ids').val(data.result);
            });
            $.ajaxSettings.async = true;
            initTree(0);
        });
        //button event
        $('button[data-btn-type]').click(function () {
            var action = $(this).attr('data-btn-type');
            var rowId = $('#role_table tr.trchange').attr('id');
            switch (action) {
                case 'search':
                    break;
                case 'authorize':
                    if (!rowId) {
                        modals.info('请选择角色');
                        return;
                    }
                    modals.confirm("确定授权？", function () {
                        $.ajax({
                            contentType: 'application/json; charset=utf-8',
                            url: appPath + 'Permission/Authorize',
                            data: JSON.stringify({ RoleId: rowId, PermissionIds: $('#ids').val() }),
                            type: 'POST',
                            dataType: 'JSON',
                            success: function (data, textStatus) {
                                if (data.success) {
                                    modals.info("授权成功");
                                }
                                else {
                                    modals.warn(data.result);
                                }
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                modals.error(errorThrown);
                            }
                        })
                    })
                    break;
                default:
                    break;
            }
        });

        //页面加载默认执行第二个点击
        $('#role_table tr').eq(1).click();
    })
    /**
     * 初始化菜单树
     * @param {any} selectNodeId
     */
    function initTree(selectNodeId) {
        $.ajax({
            url: appPath + 'Permission/GetTreeData',
            data: null,
            type: 'GET',
            dataType: 'json',
            success: function (data, textStatus) {
                if (data.success) {
                    //console.log(data.result);
                    var treeData = data.result;
                    var $tree = $("#tree").treeview({
                        showCheckbox: true,   //是否显示复选框
                        highlightSelected: true,    //是否高亮选中
                        data: treeData,
                        showBorder: true,
                        emptyIcon: '',    //没有子节点的节点图标
                        multiSelect: true,    //多选
                        expandIcon: "glyphicon glyphicon-plus",
                        collapseIcon: "glyphicon glyphicon-minus",
                        levels: 5,                  //最多可扩展几层
                        onNodeChecked: nodeChecked,
                        onNodeUnchecked: nodeUnchecked, 
                        onNodeSelected: function (event, node) {
                            console.log(node.id);
                        }
                    });
                    var checkids = seNodesSel('');
                    $tree.treeview('toggleNodeChecked', [checkids, { silent: true }]);
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
    var nodeCheckedSilent = false;  
    /**
     * 节点选中函数
     * @param {any} event
     * @param {any} node
     */
    function nodeChecked(event, node) {
        dochecklist();
        if (nodeCheckedSilent) {
            return;
        }
        nodeCheckedSilent = true;
        checkAllParent(node);
        checkAllSon(node);
        nodeCheckedSilent = false;
    }  
    var nodeUncheckedSilent = false;
    /**
     * 节点取消选中函数
     * @param {any} event
     * @param {any} node
     */
    function nodeUnchecked(event, node) {
        dochecklist();
        if (nodeUncheckedSilent) {
            return;
        }
        nodeUncheckedSilent = true;
        uncheckAllParent(node);
        uncheckAllSon(node);
        nodeUncheckedSilent = false;
    }

    /**
     * 选中全部父节点
     * @param {any} node
     */
    function checkAllParent(node) {
        $('#tree').treeview('checkNode', node.nodeId, { silent: true });
        var parentNode = $('#tree').treeview('getParent', node.nodeId);
        if (!("nodeId" in parentNode)) {
            return;
        } else {
            checkAllParent(parentNode);
        }
    }

    /**
     * 取消全部父节点
     * @param {any} node
     */
    function uncheckAllParent(node) {
        $('#tree').treeview('uncheckNode', node.nodeId, { silent: true });
        var siblings = $('#tree').treeview('getSiblings', node.nodeId);
        var parentNode = $('#tree').treeview('getParent', node.nodeId);
        if (!("nodeId" in parentNode)) {
            return;
        }
        var isAllUnchecked = true;  //是否全部没选中  
        for (var i in siblings) {
            if (siblings[i].state.checked) {
                isAllUnchecked = false;
                break;
            }
        }
        if (isAllUnchecked) {
            uncheckAllParent(parentNode);
        }
    }

    /**
     * 级联选中所有子节点
     * @param {any} node
     */
    function checkAllSon(node) {
        $('#tree').treeview('checkNode', node.nodeId, { silent: true });
        if (node.nodes != null && node.nodes.length > 0) {
            for (var i in node.nodes) {
                checkAllSon(node.nodes[i]);
            }
        }
    }

    /**
     * 级联取消所有子节点
     * @param {any} node
     */
    function uncheckAllSon(node) {
        $('#tree').treeview('uncheckNode', node.nodeId, { silent: true });
        if (node.nodes != null && node.nodes.length > 0) {
            for (var i in node.nodes) {
                uncheckAllSon(node.nodes[i]);
            }
        }
    }

    //处理选中记录
    function dochecklist() {
        var content = '';
        var ids = '';
        var list = $('#tree').treeview('getChecked', [{ silent: true }]);
        if (list) {
            $.each(list, function (name, value) {
                content += '<p><i class="fa fa-check-circle" aria-hidden="true"></i>' + value.tags + '->' + value.text + '</p>';
                if (ids == '') {
                    ids = value.id;
                }
                else {
                    ids += ',' + value.id;
                }
            })
        }
        $('#ids').val(ids);
        $('#checkable-output').html(content);
    }
    /**
     * 在id="ids"的input获取data-nodeid
     * @param {any} sids
     */
    function seNodesSel(sids) {
        var idsipt = $('#ids').val();
        if (sids != '') {
            idsipt = sids;
        }
        var retids = [];
        console.log(idsipt);
        if (idsipt != '') {
            arr = idsipt.toString().split(',');
            for (var i in arr) {
                var tmp = $("li[data-nodeid=" + arr[i] + "]").attr("data-nodeid");
                if (tmp != null && tmp != undefined && tmp != "") {
                    if (retids == '') {
                        retids[0] = parseInt(tmp);
                    } else {
                        retids.push(parseInt(tmp));
                    }
                }
            }
        }
        return retids;
    } 
})();