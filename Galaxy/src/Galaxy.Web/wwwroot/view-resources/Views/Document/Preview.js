(function () {
    $(function () {
        var editor;
        var mid = $('#mid').val();
        editor = editormd.markdownToHTML("editormd", {
        markdown: result.content,
            htmlDecode: "style,script,iframe",  // you can filter tags decode
            emoji: true,
            tocm:true ,    // Using [TOCM]
            tocContainer: "#custom-toc-container", // 自定义 ToC 容器层
            tocDropdown:true,
            taskList: true,
            tex: true,  // 默认不解析
            flowChart: true,  // 默认不解析
            sequenceDiagram: true // 默认不解析
        });
    });
})()