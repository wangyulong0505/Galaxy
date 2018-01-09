(function () {
    var editor;
    /*
        var height=$(document).height();
        $(".navbar-fixed-middle").css({"top":height/2+"px"});
    */
    $(function () {
        var mid = "8a991f87608c95700160b4c488590135";
        ajaxPost(basePath + "/markdown/get/" + mid, null, function (result) {
            $("#title").text(result.title);
        $("#keywords").text(result.keywords);
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
    });

    function returnToList() {
        loadPage("/Document/Index");
    }
})()