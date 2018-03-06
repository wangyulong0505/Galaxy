(function () {
    $(function () {
        initTree(0);
        //RoleTable tr选中样式
        $('#role_table tr').click(function () {
            //一次只能选中一行
            $(this).siblings().css('background-color', '#fff');
            $(this).addClass("trchange").siblings().removeClass("trchange");
            $(this).css('background-color', '#08C');
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
                    break;
                default:
                    break;
            }
        });
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
                    console.log(data.result);
                    var treeData = data.result;
                    $("#tree").treeview({
                        showCheckbox: true,   //是否显示复选框
                        highlightSelected: true,    //是否高亮选中
                        data: treeData,
                        showBorder: true,
                        emptyIcon: '',    //没有子节点的节点图标
                        multiSelect: false,    //多选
                        expandIcon: "glyphicon glyphicon-plus",
                        collapseIcon: "glyphicon glyphicon-minus",
                        levels: 5,                  //最多可扩展几层
                        onNodeChecked: function (event, data) {
                            //复选框勾选触发
                            console.log(data.id);
                        },
                        onNodeSelected: function (event, node) {
                            console.log(node.id);
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
})();