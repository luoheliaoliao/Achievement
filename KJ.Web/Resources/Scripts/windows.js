/* 在tab中打开新页面 selected:是否默认选中它 默认选中 modify xunm 2013-05-23 20:00*/
function openPage(title, url, selected) {
    var template = parent.KJ.template();
    var content = template.replace("[src]", url);
    if (selected == undefined || selected == null || selected.length <= 0) {
        selected = true;
    }
    parent.KJ.OpenTabPage(title, content, selected, true);
};
/* 在tab中打开新页面,并延迟加载，当点击页签的时候，才加载 */
function addTab(title, url) {
    parent.KJ.AddTab(title, url);
}
